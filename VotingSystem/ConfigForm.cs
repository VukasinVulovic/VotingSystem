using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();
        }

        private void Init(object sender, EventArgs e) //initialize objects...
        {
            btn_accept.Enabled = false;
            tb_password.UseSystemPasswordChar = true;
            ofd_database.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (Config.Candidates == null || Config.Candidates.Count == 0)
            {
                Config.Candidates = new List<Types.Candidate>();
                lbl_candidates.Text = "0";
                addCandidate();
                addCandidate();
            }
            else
            {
                for (int i = 0; i < Config.Candidates.Count; i++)
                {
                    Types.Candidate candidate = Config.Candidates[i];
                    addCandidate(candidate.Title, candidate.Description);
                }

                btn_accept.Enabled = true;
                Text = "Konfiguracija";
            }

            if (Database.Voters != null)
                lbl_voters.Text = Database.Voters.Count + "";
        }

        private void checkPasswordLength(object sender, KeyEventArgs e) => btn_decrypt.Enabled = tb_password.Text.Length > 3;

        private void Decrypt(object sender, EventArgs e) //load and decrypt database
        {
            if (tb_password.Text.Length < 4)
            {
                MessageBox.Show("Pogrešna šifra.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            btn_accept.Enabled = false;
            //db.PassPhrase = tb_password.Text;

            if (!Database.LoadVoters()) //load voters from database
            {
                MessageBox.Show("Greška pri učitavanju glasača.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            if (Database.Voters != null)
            {
                lbl_voters.Text = Database.Voters.Count + "";
                btn_accept.Enabled = true;
            }

            //Close();
        }

        private void addCandidate(string title="-", string desc="---")
        {
            Text = "Konfiguracija*";

            CandidateComponent can = new CandidateComponent();
            can.Top = pnl_candidates.Controls.Count * can.Height;
            can.Tb_title.Text = title;
            can.Tb_description.Text = desc;

            lbl_candidates.Text = int.Parse(lbl_candidates.Text) + 1 + "";

            if (pnl_candidates.Controls.Count == 0)
            {
                can.Pb_remove.Visible = false;
                can.Pb_add.Enabled = false;
                can.Pb_add.Visible = false;
            }
            else if (pnl_candidates.Controls.Count == 1)
                can.Pb_remove.Visible = false;
            else
            {
                can.Pb_remove.Click += (object sender, EventArgs e) =>
                {
                    lbl_candidates.Text = int.Parse(lbl_candidates.Text) - 1 + "";


                    int index = pnl_candidates.Controls.IndexOf(can);
                    pnl_candidates.Controls.Remove(can);

                    for (; index < pnl_candidates.Controls.Count; index++)
                        pnl_candidates.Controls[index].Top -= can.Height;

                    ((CandidateComponent)pnl_candidates.Controls[index - 1]).Pb_add.Enabled = true;
                    ((CandidateComponent)pnl_candidates.Controls[index - 1]).Pb_add.Visible = true;
                };
            }

            can.Pb_add.Click += (object sender, EventArgs e) =>
            {
                can.Pb_add.Enabled = false;
                can.Pb_add.Visible = false;
                addCandidate();
            };

            pnl_candidates.Controls.Add(can);
        } //add new Candidate control

        private void showHidePassword(object sender, EventArgs e)
        {
            if (!tb_password.UseSystemPasswordChar)
            {
                tb_password.UseSystemPasswordChar = true;
                pb_eye.Image = Properties.Resources.eye;
            }
            else
            {
                tb_password.UseSystemPasswordChar = false;
                pb_eye.Image = Properties.Resources.eye_closed;
            }
        } //button to show / hide password

        private void LoadDB(object sender, EventArgs e) //load db with settings
        {
            btn_accept.Enabled = false;
            pb_progress.Visible = true;
            pb_progress.Value = 0;

            if (ofd_database.ShowDialog() != DialogResult.OK)
            {
                lbl_dbName.Text = "Nije Učitana.";
                return;
            }

            pb_progress.Value = 50;

            Database.Init(ofd_database.FileName); //initialize database

            pb_progress.Value = 100;
            pnl_password.Enabled = true;
            pb_progress.Visible = false;
            lbl_dbName.Text = "Baza Učitana.";
        }

        private void Save(object sender, EventArgs e)
        {
            Config.Candidates.Clear();

            string title, desc;
            Types.Candidate candidate;


            if (Config.VotingStatus == null)
                Config.VotingStatus = new Dictionary<string, int>();

            Config.VotingStatus.Clear();

            foreach (Control control in pnl_candidates.Controls)
            {
                if (control.GetType() == typeof(CandidateComponent))
                {
                    title = ((CandidateComponent)control).Tb_title.Text;
                    desc = ((CandidateComponent)control).Tb_description.Text;
                    candidate = new Types.Candidate();

                    if (title.Length == 0 || desc.Length == 0)
                        return;

                    candidate.Title = title;
                    candidate.Description = desc;

                    if (Config.VotingStatus.ContainsKey(candidate.Title))
                    {
                        MessageBox.Show("Kandidati moraju biti različiti!", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    Config.VotingStatus.Add(candidate.Title, 0);
                    Config.Candidates.Add(candidate);
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        } //load all candidates
    }
}
