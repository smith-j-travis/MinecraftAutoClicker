namespace AutoClicker
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_start = new System.Windows.Forms.Button();
            this.lblStartTime = new System.Windows.Forms.Label();
            this.btn_stop = new System.Windows.Forms.Button();
            this.biLeftMouse = new AutoClicker.ButtonInputs("Left mouse button", Win32Api.WmLbuttonDown, Win32Api.WmLbuttonDown + 1);
            this.biRightMouse = new AutoClicker.ButtonInputs("Right mouse button", Win32Api.WmRbuttonDown, Win32Api.WmRbuttonDown + 1);
            this.lblStarted = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(269, 160);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(88, 51);
            this.btn_start.TabIndex = 0;
            this.btn_start.Text = "START!";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.Btn_action_Click);
            // 
            // lblStartTime
            // 
            this.lblStartTime.AutoSize = true;
            this.lblStartTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblStartTime.Location = new System.Drawing.Point(233, 222);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(34, 17);
            this.lblStartTime.TabIndex = 2;
            this.lblStartTime.Text = "time";
            this.lblStartTime.Visible = false;
            // 
            // btn_stop
            // 
            this.btn_stop.Enabled = false;
            this.btn_stop.Location = new System.Drawing.Point(165, 160);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(88, 51);
            this.btn_stop.TabIndex = 3;
            this.btn_stop.Text = "STOP!";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.Btn_stop_Click);
            // 
            // biLeftMouse
            // 
            this.biLeftMouse.Location = new System.Drawing.Point(12, 18);
            this.biLeftMouse.Name = "biLeftMouse";
            this.biLeftMouse.Size = new System.Drawing.Size(240, 130);
            this.biLeftMouse.TabIndex = 4;
            // 
            // biRightMouse
            // 
            this.biRightMouse.Location = new System.Drawing.Point(258, 18);
            this.biRightMouse.Name = "biRightMouse";
            this.biRightMouse.Size = new System.Drawing.Size(240, 130);
            this.biRightMouse.TabIndex = 5;
            // 
            // lblStarted
            // 
            this.lblStarted.AutoSize = true;
            this.lblStarted.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblStarted.Location = new System.Drawing.Point(162, 222);
            this.lblStarted.Name = "lblStarted";
            this.lblStarted.Size = new System.Drawing.Size(74, 17);
            this.lblStarted.TabIndex = 6;
            this.lblStarted.Text = "Started at:";
            this.lblStarted.Visible = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 250);
            this.Controls.Add(this.lblStarted);
            this.Controls.Add(this.biRightMouse);
            this.Controls.Add(this.biLeftMouse);
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.lblStartTime);
            this.Controls.Add(this.btn_start);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.Text = "Auto-Clicker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.Button btn_stop;
        private ButtonInputs biLeftMouse;
        private ButtonInputs biRightMouse;
        private System.Windows.Forms.Label lblStarted;
    }
}

