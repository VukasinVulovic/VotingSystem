using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;

namespace App
{
    public partial class MainForm : Form
    {
        private HTTPServer httpServer;
        private Ngrok ng;

        public MainForm()
        {
            InitializeComponent();
        }

        public void ExecThreadSafe(Action action) => Invoke(action);

        private void init(object sender, EventArgs e)
        {
            httpServer = new HTTPServer();
            ng = new Ngrok();

            RSAKeys rsaKeys = new RSAKeys();

            httpServer.RootDirectory = Path.Combine(Environment.CurrentDirectory, "www");
            httpServer.Port = 3000;
            httpServer.BufferSize = 100; //100kb

            httpServer.On("/candidates", "GET", (req, res) =>
            {
                string json = "[";

                foreach (Types.Candidate candidate in Config.Candidates)
                    json += candidate.ToString() + ",";

                json = json.Substring(0, json.Length - 1) + "]";

                byte[] buff = Encoding.Default.GetBytes(json);
                res.StatusCode = 200;
                res.AddHeader("Content-Type", "application/json");
                res.OutputStream.Write(buff, 0, buff.Length);
                res.Close();
            });

            httpServer.On("/public_key", "GET", (req, res) =>
            {
                res.StatusCode = 200;
                res.AddHeader("Content-Type", "text/plain");

                byte[] publicKey = Encoding.UTF8.GetBytes(rsaKeys.GetPublicKey());
                res.OutputStream.Write(publicKey, 0, publicKey.Length);

                res.Close();
            });

            httpServer.On("/login_voter", "POST", (req, res) =>
            {
                string body = "";
                byte[] buff = new byte[4096];
                int read;

                while ((read = req.InputStream.Read(buff, 0, buff.Length)) != 0)
                    body += Encoding.UTF8.GetString(buff, 0, read);

                body = Crypto.DecryptAsyemetricMessage(body, rsaKeys.PrivateKey);

                Dictionary<string, string> parsed = HTTPServer.ParseJSON(body);

                if (parsed == null || !parsed.ContainsKey("message") || !parsed.ContainsKey("shared_key"))
                {
                    res.StatusCode = 406;
                    res.Close();
                    return;
                }

                Dictionary<string, string> message = HTTPServer.ParseJSON(parsed["message"]);

                if (message == null || !message.ContainsKey("voter_id") || !message.ContainsKey("voter_password"))
                {
                    res.StatusCode = 406;
                    res.Close();
                    return;
                }

                Types.Voter voter = Database.GetVoter(message["voter_id"], message["voter_password"]);

                if (voter == null)
                {
                    res.StatusCode = 403;
                    res.Close();
                    return;
                }

                Types.Token token = Database.GetToken(voter.Id);

                //send response
                string responseMsg = HTTPServer.StringifyJSON(new Dictionary<string, string>
                {
                    { "id", voter.Id + "" },
                    { "fname", voter.FName },
                    { "lname", voter.LName },
                    { "token", token.Value },
                    { "date", token.Date + "" }
                });

                string encrypted = Crypto.EncryptSyemetricMessage(responseMsg, parsed["shared_key"]);
                byte[] msg = Encoding.UTF8.GetBytes(encrypted);
                res.OutputStream.Write(msg, 0, msg.Length);

                res.StatusCode = 200;
                res.Close();
            });

            httpServer.On("/vote", "POST", (req, res) =>
            {
                string body = "";
                byte[] buff = new byte[4096];
                int read;

                while ((read = req.InputStream.Read(buff, 0, buff.Length)) != 0)
                    body += Encoding.UTF8.GetString(buff, 0, read);

                body = Crypto.DecryptAsyemetricMessage(body, rsaKeys.PrivateKey);

                Dictionary<string, string> parsed = HTTPServer.ParseJSON(body);

                if (parsed == null || !parsed.ContainsKey("message") || !parsed.ContainsKey("shared_key"))
                {
                    res.StatusCode = 406;
                    res.Close();
                    return;
                }

                Dictionary<string, string> message = HTTPServer.ParseJSON(parsed["message"]);

                if (message == null || !message.ContainsKey("token"))
                {
                    res.StatusCode = 406;
                    res.Close();
                    return;
                }

                string token = message["token"];

                if(Regex.Match(token, "^[a-z0-9]{64}$") == Match.Empty) //bad token
                {
                    res.StatusCode = 403;
                    res.Close();
                }

                long date = Database.GetTokenDate(token);

                if (date > 0) //already voted
                {
                    res.StatusDescription = $"Already voted on \"{date}\".";
                    res.StatusCode = 403;
                    res.Close();
                }
                else if (date == 0) //ok, hasn't voted
                {
                    res.StatusCode = 200;
                    res.AddHeader("Content-Type", "text/plain");

                    if (!Database.UpdateTokenDate(token, DateTimeOffset.Now.ToUnixTimeMilliseconds()))
                    {
                        res.StatusCode = 500;
                        res.Close();
                        return;
                    }

                    //send response
                    string encrypted = Crypto.EncryptSyemetricMessage("{ \"response\": \"OK\" }", parsed["shared_key"]);
                    byte[] msg = Encoding.UTF8.GetBytes(encrypted);
                    res.OutputStream.Write(msg, 0, msg.Length);

                    int option;

                    try
                    {
                        option = int.Parse(message["option"]) - 1; //NaN
                    }
                    catch
                    {
                        res.StatusCode = 400;
                        res.Close();
                        return;
                    }

                    if (option < 0 || option > Config.VotingStatus.Keys.Count) //not an option
                    {
                        res.StatusCode = 400;
                        res.Close();
                        return;
                    }

                    //handle candidate voting
                    string candidate = Config.VotingStatus.Keys.ToArray()[option]; //candidate
                    Config.VotingStatus[candidate]++; //add a vote for the candidate
                    ExecThreadSafe(() => updateChart()); //show result

                    res.StatusCode = 200;
                    res.Close();
                }
                else //unknown token
                {
                    res.StatusCode = 403;
                    res.Close();
                }
            });

            ng.Port = httpServer.Port;
            ng.Token = "1rDdXj1Cowb3Ph8IbKbFxv9DiwN_25h7ReRKBbsW7sjBj3NW8";

            ng.OnOpen += (object s, string url) =>
            {
                ExecThreadSafe(() => tb_url.Text = url);
            };
        }

        private void copyURL(object sender, EventArgs e) //copy url from textbox
        {
            tb_url.SelectAll();
            tb_url.Copy();
            tb_url.DeselectAll();
        }

        private void initChart()
        {
            if (Config.VotingStatus == null)
                return;

            Config.VotersVoted = 0;
            chart.Visible = true;

            chart.Series.Clear();
            chart.Titles.Add("Kandidati");

            chart.ChartAreas[0].AxisY.Minimum = 0;
            chart.ChartAreas[0].AxisY.Maximum = Config.VoterCount;

            string[] keys = Config.VotingStatus.Keys.ToArray();
            int[] values = Config.VotingStatus.Values.ToArray();

            for (int i = 0; i < Config.VotingStatus.Count; i++)
            {
                Series series = chart.Series.Add(keys[i]);
                series.Points.Add(values[i]);
                chart.Series[i].Label = keys[i].Length > 5 ? keys[i].Substring(0, 5) + "..." : keys[i];
            }
        }

        private void updateChart()
        {
            if (Config.VotingStatus == null || Config.VotingStatus.Count == 0)
                return;

            if (Config.VoterCount == Config.VotersVoted)
            {
                MessageBox.Show("Svi glasači su glasali.", "Sistemka Poruka", MessageBoxButtons.OK, MessageBoxIcon.Information);

                endVoting(this, null);
                return;
            }

            int[] values = Config.VotingStatus.Values.ToArray();

            if (values.Length == 0)
                return;

            for (int i = 0; i < chart.Series.Count; i++)
            {
                for (int j = 0; j < chart.Series[i].Points.Count; j++)
                    chart.Series[i].Points[j] = new DataPoint(0, values[i]);
            }

            chart.Update();
            Config.VotersVoted++;
        }

        private void beginVoting(object sender, EventArgs e) //start server and stuff
        {
            if (!Database.RemoveAllTokens()) //clear tokens from database
            {
                MessageBox.Show("Sistem nije usspeo da izbriše tokene prošle sesije.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btn_start.Enabled = false;
            btn_stop.Enabled = true;

            initChart();

            httpServer.Start();

            //ng.Start();
        }

        private void endVoting(object sender, EventArgs e)
        {
            btn_start.Enabled = true;
            btn_stop.Enabled = false;

            if (httpServer != null)
                httpServer.Stop();

            if (ng != null)
                ng.Stop();
        } //stop server and stuff

        private void close(object sender, FormClosedEventArgs e)
        {
            if (httpServer != null)
                httpServer.Stop();

            if (ng != null)
                ng.Stop();
        }

        private void openConfig(object sender, EventArgs e) //open database config form
        {
            ConfigForm form = new ConfigForm();

            if (form.ShowDialog() != DialogResult.OK)
                return;

            btn_start.Enabled = true;
        }

        public void LogError(Exception err)
        {
            rtb_status.ForeColor = Color.Red;
            rtb_status.Text += $"Server Error: {err.Message}\r\n";
            rtb_status.ForeColor = Color.Black;
        }
    }
}
