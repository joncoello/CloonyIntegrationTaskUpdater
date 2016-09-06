using MYOB.CSS;
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
            MessageBox.Show("Cloony shutdown");
        }

        private void CentralApplication_Startup(object sender, EventArgs e) {
            MessageBox.Show("Cloony startup");
        }
    }
}
