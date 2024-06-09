namespace WinFormsApp1
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
            listBox1 = new ListBox();
            textBox1 = new TextBox();
            button1 = new Button();
            ChooseFileButton = new Button();
            SendFileButton = new Button();
            FilePath1 = new Label();
            FilePath2 = new Label();
            PlayReciewved = new Button();
            FileRecieved2 = new Label();
            FileRecieved1 = new Label();
            LabelSend1 = new Label();
            LabelSend2 = new Label();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(12, 12);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(776, 94);
            listBox1.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(22, 182);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(22, 247);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // ChooseFileButton
            // 
            ChooseFileButton.Location = new Point(403, 182);
            ChooseFileButton.Name = "ChooseFileButton";
            ChooseFileButton.Size = new Size(113, 23);
            ChooseFileButton.TabIndex = 3;
            ChooseFileButton.Text = "Выбрать файл";
            ChooseFileButton.UseVisualStyleBackColor = true;
            ChooseFileButton.Click += button2_Click;
            // 
            // SendFileButton
            // 
            SendFileButton.Location = new Point(403, 261);
            SendFileButton.Name = "SendFileButton";
            SendFileButton.Size = new Size(113, 23);
            SendFileButton.TabIndex = 4;
            SendFileButton.Text = "Отправить файл";
            SendFileButton.UseVisualStyleBackColor = true;
            SendFileButton.Click += SendFileButton_Click;
            // 
            // FilePath1
            // 
            FilePath1.AutoSize = true;
            FilePath1.Location = new Point(287, 229);
            FilePath1.Name = "FilePath1";
            FilePath1.Size = new Size(108, 15);
            FilePath1.TabIndex = 5;
            FilePath1.Text = "Выбранный файл:";
            // 
            // FilePath2
            // 
            FilePath2.AutoSize = true;
            FilePath2.Location = new Point(401, 229);
            FilePath2.Name = "FilePath2";
            FilePath2.Size = new Size(36, 15);
            FilePath2.TabIndex = 6;
            FilePath2.Text = "None";
            // 
            // PlayReciewved
            // 
            PlayReciewved.Location = new Point(398, 342);
            PlayReciewved.Name = "PlayReciewved";
            PlayReciewved.Size = new Size(241, 23);
            PlayReciewved.TabIndex = 7;
            PlayReciewved.Text = "Запустить принятый файл";
            PlayReciewved.UseVisualStyleBackColor = true;
            PlayReciewved.Click += PlayReciewved_Click;
            // 
            // FileRecieved2
            // 
            FileRecieved2.AutoSize = true;
            FileRecieved2.Location = new Point(401, 309);
            FileRecieved2.Name = "FileRecieved2";
            FileRecieved2.Size = new Size(36, 15);
            FileRecieved2.TabIndex = 9;
            FileRecieved2.Text = "None";
            // 
            // FileRecieved1
            // 
            FileRecieved1.AutoSize = true;
            FileRecieved1.Location = new Point(287, 309);
            FileRecieved1.Name = "FileRecieved1";
            FileRecieved1.Size = new Size(89, 15);
            FileRecieved1.TabIndex = 8;
            FileRecieved1.Text = "Файл получен:";
            // 
            // LabelSend1
            // 
            LabelSend1.AutoSize = true;
            LabelSend1.Location = new Point(576, 309);
            LabelSend1.Name = "LabelSend1";
            LabelSend1.Size = new Size(77, 15);
            LabelSend1.TabIndex = 10;
            LabelSend1.Text = "Отправлено:";
            // 
            // LabelSend2
            // 
            LabelSend2.AutoSize = true;
            LabelSend2.Location = new Point(659, 309);
            LabelSend2.Name = "LabelSend2";
            LabelSend2.Size = new Size(36, 15);
            LabelSend2.TabIndex = 11;
            LabelSend2.Text = "None";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(LabelSend2);
            Controls.Add(LabelSend1);
            Controls.Add(FileRecieved2);
            Controls.Add(FileRecieved1);
            Controls.Add(PlayReciewved);
            Controls.Add(FilePath2);
            Controls.Add(FilePath1);
            Controls.Add(SendFileButton);
            Controls.Add(ChooseFileButton);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(listBox1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBox1;
        private TextBox textBox1;
        private Button button1;
        private Button ChooseFileButton;
        private Button SendFileButton;
        private Label FilePath1;
        private Label FilePath2;
        private Button PlayReciewved;
        private Label FileRecieved2;
        private Label FileRecieved1;
        private Label LabelSend1;
        private Label LabelSend2;
    }
}
