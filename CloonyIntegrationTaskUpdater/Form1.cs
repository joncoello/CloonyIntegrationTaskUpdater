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

            var token = GetBearerToken();

            var sid = GetSessionID(token);

            using (var client = new HttpClient()) {

                client.BaseAddress = new Uri("https://test.wk-cpm.de");

                // headers
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Referrer = new Uri("https://test.wk-cpm.de/");
                
                string getUrl = "/sdn/rest/domainsvc/get/sdn.intl.domain.cpm/PracticeManagementDomain/ContactList/f2270708-103d-46fc-b952-3557c1a2e912/81b18d66-bdf7-4351-ad78-f220426fa945?orga=6feb1454-71df-4c19-be9f-c6815c037c15&prj=f2270708-103d-46fc-b952-3557c1a2e912&contract=sdn.intl.domain.cpm.contact.query.contracts.ContactTimelineContract&sid=" + sid;

                var response = client.GetAsync(getUrl).Result;

                var result = response.Content.ReadAsStringAsync().Result;

            }

        }

        private object GetSessionID(string token) {

            using (var client = new HttpClient()) {

                client.BaseAddress = new Uri("https://test.wk-cpm.de");

                // headers
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Referrer = new Uri("https://test.wk-cpm.de/");

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                string getUrl = "sdn/rest/directory/CreateSession?locale=en";

                var response = client.GetAsync(getUrl).Result;

                var result = response.Content.ReadAsStringAsync().Result;

                var session = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Session>(result);

                return session.id;

            }

        }

        private string GetBearerToken() {

            using (var client = new HttpClient()) {

                client.BaseAddress = new Uri("https://test.wk-cpm.de");

                // headers
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Referrer = new Uri("https://test.wk-cpm.de/");

                var keyValues = new List<KeyValuePair<string, string>>();
                keyValues.Add(new KeyValuePair<string, string>("clientnumber", "25031"));
                keyValues.Add(new KeyValuePair<string, string>("username", "anna.copeland"));
                keyValues.Add(new KeyValuePair<string, string>("password", "anna"));
                
                var content = new FormUrlEncodedContent(keyValues);

                string postUrl = "/sdn/oauth/token?response_type=code&grant_type=password";

                var response = client.PostAsync(postUrl, content).Result;

                var result = response.Content.ReadAsStringAsync().Result;

                var token = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Token>(result);

                return token.access_token;

            }

        }

    }
}
