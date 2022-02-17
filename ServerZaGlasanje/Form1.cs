using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerZaGlasanje
{
    public partial class MainForm : Form
    {
        private Server server;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Init(object sender, EventArgs e)
        {

        }

        private void kopirajURL(object sender, EventArgs e) {
            tb_url.SelectAll();
            tb_url.Copy();
            tb_url.DeselectAll();
        }

        private void zapocniGlasanje(object sender, EventArgs e)
        {
            btn_zapocni.Enabled = false;
            btn_zaustavi.Enabled = true;

            server = new Server();
            server.OnReady = (string url) =>
            {
                Invoke(new Action(() =>
                {
                    tb_url.Text = url;
                }));
            };
        }

        private void zaustaviGlasanje(object sender, EventArgs e)
        {
            btn_zapocni.Enabled = true;
            btn_zaustavi.Enabled = false;
        }

        public void logError(Exception err) 
        {
            rtb_status.ForeColor = Color.Red;
            rtb_status.Text += $"Server Error: {err.Message}\r\n";
            rtb_status.ForeColor = Color.Black;
        }

        private void Close(object sender, FormClosedEventArgs e)
        {
            if(server != null)
                server.Stop();
        }
    }
}
