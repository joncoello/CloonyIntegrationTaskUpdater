using Central.CSSContactAPI;
using CloonyIntegrationTaskUpdater;
using MYOB.CSS;
using MYOB.CSSInterface;
using MYOB.CSSTaskManagement;
using MYOB.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Central.CloonyIntegration {
    public class AssignmentSidebar : CSSTabSideBarItems {

        private PropertyBag _pb;

        public override AvailableArea Area {
            get {
                return AvailableArea.Assignment;
            }
        }

        public override SideBarGroup[] AddItems() {

            var sbgs = new SideBarGroup[1];

            sbgs[0] = new SideBarGroup();
            sbgs[0].Name = "Cloony";

            sbgs[0].Add("Create tasks", 0, CreateTasks);

            return sbgs;

        }

        private void CreateTasks(object Sender, SideBarEventArgs e) {

            int createdTaskActionID = 1;

            var centralDal = CssContext.Instance.GetDAL(string.Empty) as DAL;
            var gateway = new CentralGateway(centralDal);
            var assignment = gateway.FindAssignment(this.PropertyBag.AssignmentId, CssContext.Instance.Host.EmployeeId);
            
            var clientApi = new CloonyClient();
            clientApi.Login();
            clientApi.GetOrgSet();
            clientApi.GetOrgInfo();

            var clientList = clientApi.GetClientList(assignment.Client.ClientCode);

            var client = clientList.Data.FirstOrDefault(c => c.contactCode == assignment.Client.ClientCode);

            var timeline = clientApi.GetTimeline(client.contactId);

            var stepsForService = timeline.timeline.Where(s => s.serviceAgreementName == assignment.Name);

            foreach (var step in stepsForService) {

                var task = new CSSTask(centralDal);
                task.Description = step.processInstanceName + " - " + step.taskName;
                task.CodeId = 4; // crm
                task.Save();

                task.AssignToContactAssignment(CSSTask.CSSAssignToType.Assignment, assignment.AssignmentId);
                task.AssignTo(CssContext.Instance.Host.EmployeeId, CssContext.Instance.Host.EmployeeId, DateTime.Now, "Assigned by cloony", createdTaskActionID);

            }

            MessageBox.Show("Done");

        }
        
    }

}
