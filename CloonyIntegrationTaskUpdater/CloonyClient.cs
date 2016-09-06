using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CloonyIntegrationTaskUpdater {
    public class CloonyClient {

        private string _token;
        private Models.Session _session;
        private string _pmDomainGuid;
        private string _orgSetID;

        public void Login() {
            _token = GetBearerToken();
            _session = GetSessionID();
        }

        private Models.Session GetSessionID() {

            using (var client = CreateHttpClient(true)) {

                string getUrl = "sdn/rest/directory/CreateSession?locale=en";

                var response = client.GetAsync(getUrl).Result;

                var result = response.Content.ReadAsStringAsync().Result;

                var session = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Session>(result);

                return session;

            }

        }

        private string GetBearerToken() {

            using (var client = CreateHttpClient(false)) {

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

        public void GetOrgSet() {

            using (var client = CreateHttpClient(true)) {

                var getUrl = 
                    "/sdn/rest/domainsvc/get/sdn.core.directory/DirectoryDomain/" + 
                    "OrganisationSet/7401d334-2a41-4e65-8c05-8dfba9f773be/6feb1454-71df-4c19-be9f-c6815c037c15" + 
                    "?orga=7401d334-2a41-4e65-8c05-8dfba9f773be" + 
                    "&prj=7401d334-2a41-4e65-8c05-8dfba9f773be" + 
                    "&sid=" + _session.id;

                var response = client.GetAsync(getUrl).Result;

                var result = response.Content.ReadAsStringAsync().Result;

                var resource = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.OrganisationalSet>(result);

                _pmDomainGuid = resource.Projekte.FirstOrDefault(p => p.Name == "PracticeManagementDomain").id;
                _orgSetID = resource.id;

            }

        }

        public void GetOrgInfo() {

            using (var client = CreateHttpClient(true)) {

                string resourceParentUrl = "/sdn/rest/contractschema/createDataSourceSchema/sdn.core.directory/" +
                "DirectoryDomain/" + _session.organization + "/" + _session.organization + "/" + _session.organization + "/" +
                "OrganizationAggregates/OrganizationInfo?namespace=&sid=" + _session.id;

                var resourceParentResponse = client.GetAsync(resourceParentUrl).Result;

                var resourceParentResult = resourceParentResponse.Content.ReadAsStringAsync().Result;

                var resource = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Resource>(resourceParentResult);

                string practiceManagementDomainUrl = "/sdn/rest/domainsvc/get/sdn.core.directory/DirectoryDomain/" +
                    "OrganisationSet/7401d334-2a41-4e65-8c05-8dfba9f773be/6feb1454-71df-4c19-be9f-c6815c037c15?" +
                    "orga=7401d334-2a41-4e65-8c05-8dfba9f773be" +
                    "&prj=7401d334-2a41-4e65-8c05-8dfba9f773be" +
                    "&sid=e77009ba-cd06-4813-b385-178fa9f49f05";

                string getUrl = "/sdn/rest/domainsvc/query/sdn.intl.domain.cpm/PracticeManagementDomain/ClientList/f2270708-103d-46fc-b952-3557c1a2e912?orga=6feb1454-71df-4c19-be9f-c6815c037c15&prj=f2270708-103d-46fc-b952-3557c1a2e912&sid=96053c3e-eb2d-4172-ac04-e887d8423785";

            }
        }

        public Models.ClientList GetClientList() {

            using (var client = new HttpClient()) {

                client.DefaultRequestHeaders.Add("Host", "test.wk-cpm.de");
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                //client.DefaultRequestHeaders.Add("Content-Length", "5236");
                client.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
                client.DefaultRequestHeaders.Add("Origin", "https://test.wk-cpm.de");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
                //client.DefaultRequestHeaders.Add("Content-Type", "application/json;charset=UTF-8");
                client.DefaultRequestHeaders.Add("Referer", "https://test.wk-cpm.de/");
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                client.DefaultRequestHeaders.Add("Accept-Language", "en-GB,en-US;q=0.8,en;q=0.6");

                string json = 
                    "{\"customProperties\":{},\"resourceIdentifier\":{\"resourceParentId\":\"" + _pmDomainGuid + "\"," + 
                    "\"resourceId\":\"00000000-0000-0000-0000-000000000000\",\"businessEntity\":\"ClientList\"}," + 
                    "\"domainIdentifier\":{\"organisationIdentifier\":\"" + _orgSetID + "\"," + 
                    "\"namespaceName\":\"sdn.intl.domain.cpm\",\"domainName\":\"PracticeManagementDomain\"," + 
                    "\"domainIdentifier\":\"" + _pmDomainGuid + "\"},\"contractName\":\"ClientListContract\"," + 
                    "\"commandInterface\":null,\"Felder\":[{\"optFields\":null,\"Name\":\"\",\"DataColumn\":\"version\",\"Width\":200,\"Filterable\":false,\"Visible\":false,\"Sortable\":true,\"Groupable\":false,\"DataType\":\"Integer\",\"id\":false,\"ColumnOrder\":0,\"ObjectName\":\"\",\"Format\":\"Number\",\"readOnly\":false,\"Locked\":false,\"popupTitle\":false,\"highlighted\":false},{\"optFields\":null,\"Name\":\"\",\"DataColumn\":\"wrappedClassName\",\"Width\":200,\"Filterable\":false,\"Visible\":false,\"Sortable\":true,\"Groupable\":false,\"DataType\":\"String\",\"id\":false,\"ColumnOrder\":0,\"ObjectName\":\"\",\"Format\":\"String\",\"readOnly\":false,\"Locked\":false,\"popupTitle\":false,\"highlighted\":false},{\"optFields\":null,\"Name\":\"\",\"DataColumn\":\"contactId\",\"Width\":0,\"Filterable\":false,\"Visible\":false,\"Sortable\":false,\"Groupable\":false,\"DataType\":\"UUID\",\"id\":false,\"ColumnOrder\":0,\"ObjectName\":\"\",\"Format\":\"UUID\",\"readOnly\":false,\"Locked\":false,\"popupTitle\":false,\"highlighted\":false},{\"optFields\":null,\"Name\":\"\",\"DataColumn\":\"contactType\",\"Width\":0,\"Filterable\":false,\"Visible\":true,\"Sortable\":false,\"Groupable\":false,\"DataType\":\"Integer\",\"id\":false,\"ColumnOrder\":0,\"ObjectName\":\"\",\"Format\":\"Number\",\"readOnly\":false,\"Locked\":false,\"popupTitle\":false,\"highlighted\":false},{\"optFields\":null,\"Name\":\"\",\"DataColumn\":\"contactName\",\"Width\":0,\"Filterable\":false,\"Visible\":true,\"Sortable\":false,\"Groupable\":false,\"DataType\":\"String\",\"id\":false,\"ColumnOrder\":0,\"ObjectName\":\"\",\"Format\":\"String\",\"readOnly\":false,\"Locked\":false,\"popupTitle\":false,\"highlighted\":false},{\"optFields\":null,\"Name\":\"\",\"DataColumn\":\"contactProfession\",\"Width\":0,\"Filterable\":false,\"Visible\":true,\"Sortable\":false,\"Groupable\":false,\"DataType\":\"String\",\"id\":false,\"ColumnOrder\":0,\"ObjectName\":\"\",\"Format\":\"String\",\"readOnly\":false,\"Locked\":false,\"popupTitle\":false,\"highlighted\":false},{\"optFields\":null,\"Name\":\"\",\"DataColumn\":\"contactBusiness\",\"Width\":0,\"Filterable\":false,\"Visible\":true,\"Sortable\":false,\"Groupable\":false,\"DataType\":\"String\",\"id\":false,\"ColumnOrder\":0,\"ObjectName\":\"\",\"Format\":\"String\",\"readOnly\":false,\"Locked\":false,\"popupTitle\":false,\"highlighted\":false},{\"optFields\":null,\"Name\":\"\",\"DataColumn\":\"contactCompanyName\",\"Width\":0,\"Filterable\":false,\"Visible\":true,\"Sortable\":false,\"Groupable\":false,\"DataType\":\"String\",\"id\":false,\"ColumnOrder\":0,\"ObjectName\":\"\",\"Format\":\"String\",\"readOnly\":false,\"Locked\":false,\"popupTitle\":false,\"highlighted\":false},{\"optFields\":null,\"Name\":\"\",\"DataColumn\":\"responsibleEmployeeName\",\"Width\":0,\"Filterable\":false,\"Visible\":true,\"Sortable\":false,\"Groupable\":false,\"DataType\":\"String\",\"id\":false,\"ColumnOrder\":0,\"ObjectName\":\"\",\"Format\":\"String\",\"readOnly\":false,\"Locked\":false,\"popupTitle\":false,\"highlighted\":false},{\"optFields\":null,\"Name\":\"\",\"DataColumn\":\"searchString\",\"Width\":0,\"Filterable\":false,\"Visible\":true,\"Sortable\":false,\"Groupable\":false,\"DataType\":\"String\",\"id\":false,\"ColumnOrder\":0,\"ObjectName\":\"\",\"Format\":\"String\",\"readOnly\":false,\"Locked\":false,\"popupTitle\":false,\"highlighted\":false},{\"optFields\":null,\"Name\":\"\",\"DataColumn\":\"nextDueDate\",\"Width\":0,\"Filterable\":false,\"Visible\":true,\"Sortable\":false,\"Groupable\":false,\"DataType\":\"String\",\"id\":false,\"ColumnOrder\":0,\"ObjectName\":\"\",\"Format\":\"String\",\"readOnly\":false,\"Locked\":false,\"popupTitle\":false,\"highlighted\":false},{\"optFields\":null,\"Name\":\"\",\"DataColumn\":\"urlAvatar\",\"Width\":0,\"Filterable\":false,\"Visible\":true,\"Sortable\":false,\"Groupable\":false,\"DataType\":\"String\",\"id\":false,\"ColumnOrder\":0,\"ObjectName\":\"\",\"Format\":\"String\",\"readOnly\":false,\"Locked\":false,\"popupTitle\":false,\"highlighted\":false},{\"optFields\":null,\"Name\":\"\",\"DataColumn\":\"contactCode\",\"Width\":0,\"Filterable\":false,\"Visible\":true,\"Sortable\":false,\"Groupable\":false,\"DataType\":\"String\",\"id\":false,\"ColumnOrder\":0,\"ObjectName\":\"\",\"Format\":\"String\",\"readOnly\":false,\"Locked\":false,\"popupTitle\":false,\"highlighted\":false},{\"optFields\":null,\"Name\":\"\",\"DataColumn\":\"contactState\",\"Width\":0,\"Filterable\":false,\"Visible\":true,\"Sortable\":false,\"Groupable\":false,\"DataType\":\"Integer\",\"id\":false,\"ColumnOrder\":0,\"ObjectName\":\"\",\"Format\":\"Number\",\"readOnly\":false,\"Locked\":false,\"popupTitle\":false,\"highlighted\":false},{\"optFields\":null,\"Name\":\"\",\"DataColumn\":\"contactVersion\",\"Width\":0,\"Filterable\":false,\"Visible\":true,\"Sortable\":false,\"Groupable\":false,\"DataType\":\"Integer\",\"id\":false,\"ColumnOrder\":0,\"ObjectName\":\"\",\"Format\":\"Number\",\"readOnly\":false,\"Locked\":false,\"popupTitle\":false,\"highlighted\":false}]," + 
                    "\"subItem\":null,\"FilterOptions\":[],\"SortOptions\":[{\"FieldName\":\"searchString\",\"Descending\":0}]," + 
                    "\"AggregateCount\":0,\"GenerateAggregates\":false,\"PageStart\":0,\"PageSize\":25,\"BreadcrumbItems\":[]," + 
                    "\"DrilldownContextItems\":[],\"DrilldownDefaultItem\":null,\"CurrentDrilldownPath\":[]," + 
                    "\"CurrentDataId\":\"00000000-0000-0000-0000-000000000000\",\"currentLevelBreadcrumb\":null," + 
                    "\"IsDetailViewDescription\":false,\"workId\":null,\"ViewPath\":null}";

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                string postUrl =
                    "https://test.wk-cpm.de/sdn/rest/domainsvc/query/sdn.intl.domain.cpm/PracticeManagementDomain/" +
                    "ClientList/" + _pmDomainGuid  +
                    "?orga=" + _orgSetID + 
                    "&prj=" + _pmDomainGuid + 
                    "&sid=" + _session.id;

                var response = client.PostAsync(postUrl, content).Result;

                var result = response.Content.ReadAsStringAsync().Result;

                var resource = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.ClientList>(result);

                return resource;

            }

        }

        public Models.TimelineDetails GetTimeline(string contactID) {

            using (var client = CreateHttpClient(true)) {

                string getUrl = 
                    "/sdn/rest/domainsvc/get/sdn.intl.domain.cpm/PracticeManagementDomain/" + 
                    "ContactList/" + _pmDomainGuid + "/" + contactID  + 
                    "?orga=" + _orgSetID + 
                    "&prj=" + _pmDomainGuid + 
                    "&contract=sdn.intl.domain.cpm.contact.query.contracts.ContactTimelineContract&sid=" + _session.id;

                var response = client.GetAsync(getUrl).Result;

                var result = response.Content.ReadAsStringAsync().Result;

                var resource = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.TimelineDetails>(result);

                return resource;

            }

        }

        public string UpdateStep(Models.TimelineStep step) {

            using (var client = new HttpClient()) {

                client.DefaultRequestHeaders.Add("Host", "test.wk-cpm.de");
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                //client.DefaultRequestHeaders.Add("Content-Length", "5236");
                client.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
                client.DefaultRequestHeaders.Add("Origin", "https://test.wk-cpm.de");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
                //client.DefaultRequestHeaders.Add("Content-Type", "application/json;charset=UTF-8");
                client.DefaultRequestHeaders.Add("Referer", "https://test.wk-cpm.de/");
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                client.DefaultRequestHeaders.Add("Accept-Language", "en-GB,en-US;q=0.8,en;q=0.6");

                string postUrl =
                "https://test.wk-cpm.de/sdn/rest/businesscommand/command/sdn.intl.domain.cpm/PracticeManagementDomain/ClientList" +
                "/ServiceAgreementChangeStepStateCommand" +
                "?domainid=" + _pmDomainGuid +
                "&resid=" + step.serviceAgreementId +
                "&revision=" + step.serviceAgreementVersion +
                "&orga=" + _orgSetID +
                "&sid=" + _session.id;
                
                string json =
                    "{\"fields\":{\"processChainPosition\":\"" + step.processChainPosition + "\",\"processId\":\"" + step.processId + "\"," +
                    "\"processOriginId\":\"" + step.processOriginId + "\",\"id\":\"" + step.serviceAgreementId + "\"," +
                    "\"state\":\"3\",\"isProposedState\":\"false\",\"version\":\"" + step.serviceAgreementVersion + "\",\"taskId\":\"" + step.taskId + "\"}}";

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = client.PostAsync(postUrl, content).Result;

                var result = response.Content.ReadAsStringAsync().Result;

                return result;

            }

        }

        private HttpClient CreateHttpClient(bool includeToken) {

            var client = new HttpClient();

            client.BaseAddress = new Uri("https://test.wk-cpm.de");

            // headers
            if (includeToken) {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
            }
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Referrer = new Uri("https://test.wk-cpm.de/");

            return client;

        }

    }
}
