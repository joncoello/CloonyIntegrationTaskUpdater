using MYOB.CSSInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central.CloonyIntegration {
    public class AssignmentSidebar : CSSTabSideBarItems, ICSSPropertyBag {

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
            System.Windows.Forms.MessageBox.Show("Test");
        }

        void ICSSPropertyBag.PropertyBag(PropertyBag PropertyBag) {
            _pb = PropertyBag;
        }

    }

}
