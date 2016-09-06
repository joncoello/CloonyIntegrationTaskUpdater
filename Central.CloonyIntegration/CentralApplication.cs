using MYOB.CSS;
using MYOB.CSSInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Central.CloonyIntegration {
    public class CentralApplication : CSSApplication {

        public CentralApplication() {
            this.Startup += CentralApplication_Startup;
            this.ShutDown += CentralApplication_ShutDown;
        }

        private void CentralApplication_ShutDown(object sender, EventArgs e) {
            CSSFormEventHandler.Instance.RemoveHandle("Central", "Task.StatusChanging", TaskStatusChanging);
        }

        private void CentralApplication_Startup(object sender, EventArgs e) {
            CSSFormEventHandler.Instance.AddHandle("Central", "Task.StatusChanging", TaskStatusChanging);
        }

        private void TaskStatusChanging(object sender, EventArgs e) {
            MessageBox.Show("Task status changing");
        }

    }

}
