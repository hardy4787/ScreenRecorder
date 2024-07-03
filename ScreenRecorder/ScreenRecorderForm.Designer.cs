namespace ScreenRecorder
{
    partial class ScreenRecorderForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScreenRecorderForm));
            stopBtn = new Button();
            microphoneBtn = new Button();
            startBtn = new Button();
            SuspendLayout();
            // 
            // stopBtn
            // 
            stopBtn.Image = Properties.Resources.icons8_stop_48;
            stopBtn.Location = new Point(97, 12);
            stopBtn.Name = "stopBtn";
            stopBtn.Size = new Size(65, 65);
            stopBtn.TabIndex = 1;
            stopBtn.UseVisualStyleBackColor = true;
            stopBtn.Click += stopBtn_Click;
            // 
            // microphoneBtn
            // 
            microphoneBtn.Image = Properties.Resources.icons8_block_microphone_50;
            microphoneBtn.Location = new Point(178, 12);
            microphoneBtn.Name = "microphoneBtn";
            microphoneBtn.Size = new Size(65, 65);
            microphoneBtn.TabIndex = 2;
            microphoneBtn.UseVisualStyleBackColor = true;
            microphoneBtn.Click += microphoneBtn_Click;
            // 
            // startBtn
            // 
            startBtn.Image = Properties.Resources.icons8_play_48;
            startBtn.Location = new Point(12, 12);
            startBtn.Name = "startBtn";
            startBtn.Size = new Size(65, 65);
            startBtn.TabIndex = 0;
            startBtn.UseVisualStyleBackColor = true;
            startBtn.Click += startBtn_Click;
            // 
            // ScreenRecorderForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(264, 96);
            Controls.Add(microphoneBtn);
            Controls.Add(stopBtn);
            Controls.Add(startBtn);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "ScreenRecorderForm";
            Text = "Screen Rec";
            ResumeLayout(false);
        }

        #endregion

        private Button startBtn;
        private Button stopBtn;
        private Button microphoneBtn;
    }
}
