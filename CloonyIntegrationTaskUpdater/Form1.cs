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

            var clientApi = new CloonyClient();
            clientApi.Login();
            clientApi.GetOrgSet();
            clientApi.GetOrgInfo();

            var clientList = clientApi.GetClientList();

            var client = clientList.Data.FirstOrDefault(c => c.contactCode != null && c.contactCode == "TEST001");

            var timeline = clientApi.GetTimeline(client.contactId);

            var step = timeline.timeline.FirstOrDefault(s => s.taskState == "0");

            if (step != null) {
                clientApi.UpdateStep(step);
            }
            
        }


    }
}
