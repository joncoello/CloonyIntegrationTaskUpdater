using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CloonyIntegrationTaskUpdater {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void cmdGetTimeLine_Click(object sender, EventArgs e) {

            var client = new CloonyClient();
            client.Login();
            client.GetOrgSet();
            client.GetOrgInfo();

            var clientList = client.GetClientList();

            var timeline = client.GetTimeline();
            
        }


    }
}
