using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace App
{
    internal class Ngrok
    {
        private Process ng;
        private string token;
        private int port;
        public EventHandler<string> OnOpen;
        public string Token { get => token; set => token = value; }
        public int Port { get => port; set => port = value; }

        public Ngrok()
        {
            port = 80;
        }

        async public void Start()
        {
            killProcess("ngrok.exe");

            if (token == null)
                throw new Exception("No token provided.");

            if (ng != null && ng.HasExited)
                ng.Kill();

            ProcessStartInfo opts = new ProcessStartInfo();
            opts.UseShellExecute = false;
            opts.RedirectStandardOutput = true;
            opts.RedirectStandardError = true;
            opts.RedirectStandardInput = true;
            opts.CreateNoWindow = true;

            opts.FileName = "ngrok.exe";
            opts.Arguments = $"http {port} --authtoken=\"{Token}\" --log stdout";

            ng = new Process();
            ng.StartInfo.ErrorDialog = false;
            ng.StartInfo = opts;

            ng.ErrorDataReceived += (object sender, DataReceivedEventArgs e) => Console.WriteLine("NGROK Error: " + e.Data);
            ng.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                Match m = Regex.Match(e.Data ?? "", "url=(.*)io");

                if (m.Success)
                {
                    ng.CancelOutputRead();
                    OnOpen.Invoke(this, m.Value.Replace("url=", ""));
                }
            };

            ng.Start();
            ng.BeginOutputReadLine();
            ng.BeginErrorReadLine();
        }
        public void Stop()
        {
            if (ng != null && !ng.HasExited)
                ng.Kill();
        }

        private void killProcess(string proces)
        {
            ProcessStartInfo opts = new ProcessStartInfo("taskkill");
            opts.Arguments = $"/F /IM \"{proces}\"";
            opts.UseShellExecute = false;

            Process p = new Process() { StartInfo = opts };
            p.Start();
            p.WaitForExit();
            p.Dispose();
        }
    }
}
