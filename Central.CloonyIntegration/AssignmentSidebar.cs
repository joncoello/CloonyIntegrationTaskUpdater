using Central.CSSContactAPI;
using MYOB.CSS;
using MYOB.CSSInterface;
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

            var centralDal = CssContext.Instance.GetDAL(string.Empty) as DAL;
            var gateway = new CentralGateway(centralDal);
            var assignment = gateway.FindAssignment(this.PropertyBag.AssignmentId, CssContext.Instance.Host.EmployeeId);

            MessageBox.Show("Open assignment is " + assignment.Name);

        }
        
    }

}
