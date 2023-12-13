namespace Modux_QRCodes
{
    partial class Form1
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
            fileName = new Label();
            fileBtn = new Button();
            imageDisplay = new PictureBox();
            decodeOutput = new Label();
            decodeBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)imageDisplay).BeginInit();
            SuspendLayout();
            // 
            // fileName
            // 
            fileName.AutoSize = true;
            fileName.Location = new Point(12, 13);
            fileName.Name = "fileName";
            fileName.Size = new Size(38, 15);
            fileName.TabIndex = 0;
            fileName.Text = "label1";
            // 
            // fileBtn
            // 
            fileBtn.Location = new Point(67, 13);
            fileBtn.Name = "fileBtn";
            fileBtn.Size = new Size(75, 23);
            fileBtn.TabIndex = 1;
            fileBtn.Text = "button1";
            fileBtn.UseVisualStyleBackColor = true;
            fileBtn.Click += fileBtn_Click;
            // 
            // imageDisplay
            // 
            imageDisplay.ImageLocation = "";
            imageDisplay.Location = new Point(162, 16);
            imageDisplay.Name = "imageDisplay";
            imageDisplay.Size = new Size(150, 150);
            imageDisplay.SizeMode = PictureBoxSizeMode.Zoom;
            imageDisplay.TabIndex = 2;
            imageDisplay.TabStop = false;
            // 
            // decodeOutput
            // 
            decodeOutput.AutoSize = true;
            decodeOutput.Location = new Point(37, 190);
            decodeOutput.Name = "decodeOutput";
            decodeOutput.Size = new Size(38, 15);
            decodeOutput.TabIndex = 3;
            decodeOutput.Text = "label1";
            // 
            // decodeBtn
            // 
            decodeBtn.Location = new Point(16, 154);
            decodeBtn.Name = "decodeBtn";
            decodeBtn.Size = new Size(75, 23);
            decodeBtn.TabIndex = 4;
            decodeBtn.Text = "button1";
            decodeBtn.UseVisualStyleBackColor = true;
            decodeBtn.Click += decodeBtn_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(decodeBtn);
            Controls.Add(decodeOutput);
            Controls.Add(imageDisplay);
            Controls.Add(fileBtn);
            Controls.Add(fileName);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)imageDisplay).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label fileName;
        private Button fileBtn;
        private PictureBox imageDisplay;
        private Label decodeOutput;
        private Button decodeBtn;
    }
}
