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
            label1 = new Label();
            decodeBtn = new Button();
            decodeOutput = new TextBox();
            ((System.ComponentModel.ISupportInitialize)imageDisplay).BeginInit();
            SuspendLayout();
            // 
            // fileName
            // 
            fileName.AutoSize = true;
            fileName.Location = new Point(15, 15);
            fileName.Name = "fileName";
            fileName.Size = new Size(87, 15);
            fileName.TabIndex = 0;
            fileName.Text = "QRCode Image";
            // 
            // fileBtn
            // 
            fileBtn.Location = new Point(15, 40);
            fileBtn.Name = "fileBtn";
            fileBtn.Size = new Size(85, 23);
            fileBtn.TabIndex = 1;
            fileBtn.Text = "Select Image";
            fileBtn.UseVisualStyleBackColor = true;
            fileBtn.Click += fileBtn_Click;
            // 
            // imageDisplay
            // 
            imageDisplay.ImageLocation = "";
            imageDisplay.Location = new Point(120, 15);
            imageDisplay.Name = "imageDisplay";
            imageDisplay.Size = new Size(150, 150);
            imageDisplay.SizeMode = PictureBoxSizeMode.Zoom;
            imageDisplay.TabIndex = 2;
            imageDisplay.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(30, 205);
            label1.Name = "label1";
            label1.Size = new Size(45, 15);
            label1.TabIndex = 3;
            label1.Text = "Output";
            // 
            // decodeBtn
            // 
            decodeBtn.Location = new Point(15, 175);
            decodeBtn.Name = "decodeBtn";
            decodeBtn.Size = new Size(75, 23);
            decodeBtn.TabIndex = 4;
            decodeBtn.Text = "Decode QR Code";
            decodeBtn.UseVisualStyleBackColor = true;
            decodeBtn.Click += decodeBtn_Click;
            // 
            // decodeOutput
            // 
            decodeOutput.Location = new Point(15, 230);
            decodeOutput.Name = "decodeOutput";
            decodeOutput.ReadOnly = true;
            decodeOutput.Size = new Size(255, 23);
            decodeOutput.TabIndex = 5;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 266);
            Controls.Add(decodeOutput);
            Controls.Add(decodeBtn);
            Controls.Add(label1);
            Controls.Add(imageDisplay);
            Controls.Add(fileBtn);
            Controls.Add(fileName);
            Name = "Form1";
            Text = "QR Decoder";
            ((System.ComponentModel.ISupportInitialize)imageDisplay).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label fileName;
        private Button fileBtn;
        private PictureBox imageDisplay;
        private Label label1;
        private Button decodeBtn;
        private TextBox decodeOutput;
    }
}
