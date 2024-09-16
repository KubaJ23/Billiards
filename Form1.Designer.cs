
namespace billard
{
    partial class BillardGame
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.refresh_Timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 5;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // refresh_Timer
            // 
            this.refresh_Timer.Enabled = true;
            this.refresh_Timer.Interval = 10;
            this.refresh_Timer.Tick += new System.EventHandler(this.refresh_Timer_Tick);
            // 
            // BillardGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 561);
            this.KeyPreview = true;
            this.Name = "BillardGame";
            this.Text = "Billard";
            this.Load += new System.EventHandler(this.BillardGame_Load);
            this.Click += new System.EventHandler(this.BillardGame_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.BillardGame_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer refresh_Timer;
    }
}

