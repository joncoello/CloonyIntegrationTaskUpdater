namespace CloonyIntegrationTaskUpdater.Models
{
    public class TimelineStep
    {

        public enum Status
        {
            unticked = 0,
            ticked = 3
        }

        public string id { get; set; }
        public string taskName { get; set; }
        public string processName { get; set; }
        public string processInstanceName { get; set; }
        public string serviceAgreementId { get; set; }
        public string processChainPosition { get; set; }
        public string processOriginId { get; set; }
        public string processId { get; set; }
        public string taskId { get; set; }
        public string taskState { get; set; }
        public string serviceAgreementVersion { get; set; }
        public string serviceAgreementName { get; set; }

        public UpdateTimeStep TimeStepUpdate(TimelineStep.Status statusCode)
        {
            return  new UpdateTimeStep(this, statusCode);

        }

        public class UpdateTimeStep
        {
            public Fields fields { get; set; }

            public UpdateTimeStep(TimelineStep step, TimelineStep.Status statusCode)
            {
                this.fields.processChainPosition = step.processChainPosition;
                this.fields.processId = step.processId;
                this.fields.processOriginId = step.processOriginId;
                this.fields.id = step.serviceAgreementId;
                this.fields.state = statusCode.ToString();
                this.fields.isProposedState = "false";
                this.fields.version = step.serviceAgreementVersion;
                this.fields.taskId = step.taskId;

            }
        }

        public class Fields
        {
            public string processChainPosition { get; set; }
            public string processId { get; set; }
            public string processOriginId { get; set; }
            public string id { get; set; }
            public string state { get; set; }
            public string isProposedState { get; set; }
            public string version { get; set; }
            public string taskId { get; set; }
        }

    }
}