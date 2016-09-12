namespace CloonyIntegrationTaskUpdater {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.cmdGetTimeLine = new System.Windows.Forms.Button();
            this.cmdCreateTemplate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdGetTimeLine
            // 
            this.cmdGetTimeLine.Location = new System.Drawing.Point(12, 12);
            this.cmdGetTimeLine.Name = "cmdGetTimeLine";
            this.cmdGetTimeLine.Size = new System.Drawing.Size(139, 23);
            this.cmdGetTimeLine.TabIndex = 0;
            this.cmdGetTimeLine.Text = "Get timeline";
            this.cmdGetTimeLine.UseVisualStyleBackColor = true;
            this.cmdGetTimeLine.Click += new System.EventHandler(this.cmdGetTimeLine_Click);
            // 
            // cmdCreateTemplate
            // 
            this.cmdCreateTemplate.Location = new System.Drawing.Point(12, 52);
            this.cmdCreateTemplate.Name = "cmdCreateTemplate";
            this.cmdCreateTemplate.Size = new System.Drawing.Size(139, 23);
            this.cmdCreateTemplate.TabIndex = 1;
            this.cmdCreateTemplate.Text = "Create template";
            this.cmdCreateTemplate.UseVisualStyleBackColor = true;
            this.cmdCreateTemplate.Click += new System.EventHandler(this.cmdCreateTemplate_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.cmdCreateTemplate);
            this.Controls.Add(this.cmdGetTimeLine);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdGetTimeLine;
        private System.Windows.Forms.Button cmdCreateTemplate;
    }
}

