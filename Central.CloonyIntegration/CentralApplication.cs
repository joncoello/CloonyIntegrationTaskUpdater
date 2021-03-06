﻿using CloonyIntegrationTaskUpdater;
using MYOB.CSS;
using MYOB.CSSInterface;
using MYOB.DAL;
using System;
using System.Collections.Generic;
using System.Data;
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

            var eArgs = e as FrameworkCancelEventArgs;

            if (eArgs != null) {

                int activityID = Convert.ToInt32(eArgs.PropertyBag["ActivityId"]);

                var centralDal = CssContext.Instance.GetDAL(string.Empty) as DAL;

                var data = centralDal.RunSpReturnDs("spGetTaskCloonyData",
                    new DalParm("@activityid", SqlDbType.Int, 0, activityID));

                if(data.Tables.Count==1 && data.Tables[0].Rows.Count == 1) {
                    var row = data.Tables[0].Rows[0];

                    string clientCode = Convert.ToString(row["ClientCode"]);
                    string assignment = Convert.ToString(row["assignment"]);
                    string taskName = Convert.ToString(row["taskName"]);

                    string period = taskName.Split('-')[0].Trim();
                    string task = taskName.Split('-')[1].Trim();

                    //MessageBox.Show(clientCode + assignment + taskName);

                    var clientApi = new CloonyClient();
                    clientApi.Login();
                    clientApi.GetOrgSet();
                    clientApi.GetOrgInfo();

                    var clientList = clientApi.GetClientList(clientCode);

                    var client = clientList.Data.FirstOrDefault(c => c.contactCode == clientCode);

                    var timeline = clientApi.GetTimeline(client.contactId);

                    var stepsForService = timeline.timeline.Where(s=>s.serviceAgreementName == assignment);
                    if (stepsForService.Count() == 0) {
                        eArgs.Cancel = true;
                        MessageBox.Show($"No service agreement called '{assignment}' found in the running tasks in Cloony", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    var stepsForPeriod = stepsForService.Where(s => s.processInstanceName == period);
                    if (stepsForPeriod.Count() == 0) {
                        eArgs.Cancel = true;
                        MessageBox.Show($"No process period called '{period}' found in the running tasks in Cloony", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    
                    var step = stepsForPeriod.FirstOrDefault(s => s.taskName == task);
                    if (step == null) {
                        eArgs.Cancel = true;
                        MessageBox.Show($"No task called '{task}' found in the running tasks in Cloony", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    clientApi.UpdateStep(step);
                    
                    MessageBox.Show("Cloony task updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }

            }

        }

    }

}
