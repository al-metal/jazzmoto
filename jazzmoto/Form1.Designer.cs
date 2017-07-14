namespace jazzmoto
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSaveTemplate = new System.Windows.Forms.Button();
            this.btnImages = new System.Windows.Forms.Button();
            this.tbKeywords = new System.Windows.Forms.TextBox();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.rtbFullText = new System.Windows.Forms.RichTextBox();
            this.rtbMiniText = new System.Windows.Forms.RichTextBox();
            this.btnActual = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbLoginNethouse = new System.Windows.Forms.TextBox();
            this.tbPassNethouse = new System.Windows.Forms.TextBox();
            this.cbSEO = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSaveTemplate
            // 
            this.btnSaveTemplate.Location = new System.Drawing.Point(460, 139);
            this.btnSaveTemplate.Name = "btnSaveTemplate";
            this.btnSaveTemplate.Size = new System.Drawing.Size(218, 23);
            this.btnSaveTemplate.TabIndex = 21;
            this.btnSaveTemplate.Text = "Сохранить шаблон";
            this.btnSaveTemplate.UseVisualStyleBackColor = true;
            this.btnSaveTemplate.Click += new System.EventHandler(this.btnSaveTemplate_Click);
            // 
            // btnImages
            // 
            this.btnImages.Location = new System.Drawing.Point(460, 110);
            this.btnImages.Name = "btnImages";
            this.btnImages.Size = new System.Drawing.Size(218, 23);
            this.btnImages.TabIndex = 20;
            this.btnImages.Text = "Обработать картинки";
            this.btnImages.UseVisualStyleBackColor = true;
            this.btnImages.Click += new System.EventHandler(this.btnImages_Click);
            // 
            // tbKeywords
            // 
            this.tbKeywords.Location = new System.Drawing.Point(12, 308);
            this.tbKeywords.Name = "tbKeywords";
            this.tbKeywords.Size = new System.Drawing.Size(442, 20);
            this.tbKeywords.TabIndex = 19;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(12, 282);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(442, 20);
            this.tbDescription.TabIndex = 18;
            // 
            // tbTitle
            // 
            this.tbTitle.Location = new System.Drawing.Point(12, 256);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(442, 20);
            this.tbTitle.TabIndex = 17;
            // 
            // rtbFullText
            // 
            this.rtbFullText.Location = new System.Drawing.Point(12, 134);
            this.rtbFullText.Name = "rtbFullText";
            this.rtbFullText.Size = new System.Drawing.Size(442, 116);
            this.rtbFullText.TabIndex = 16;
            this.rtbFullText.Text = "";
            // 
            // rtbMiniText
            // 
            this.rtbMiniText.Location = new System.Drawing.Point(12, 12);
            this.rtbMiniText.Name = "rtbMiniText";
            this.rtbMiniText.Size = new System.Drawing.Size(442, 116);
            this.rtbMiniText.TabIndex = 15;
            this.rtbMiniText.Text = "";
            // 
            // btnActual
            // 
            this.btnActual.Location = new System.Drawing.Point(460, 81);
            this.btnActual.Name = "btnActual";
            this.btnActual.Size = new System.Drawing.Size(218, 23);
            this.btnActual.TabIndex = 14;
            this.btnActual.Text = "Обработать сайт";
            this.btnActual.UseVisualStyleBackColor = true;
            this.btnActual.Click += new System.EventHandler(this.btnActual_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbLoginNethouse);
            this.groupBox1.Controls.Add(this.tbPassNethouse);
            this.groupBox1.Location = new System.Drawing.Point(460, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(218, 63);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "nethouse";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(109, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Пароль";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Логин";
            // 
            // tbLoginNethouse
            // 
            this.tbLoginNethouse.Location = new System.Drawing.Point(6, 32);
            this.tbLoginNethouse.Name = "tbLoginNethouse";
            this.tbLoginNethouse.Size = new System.Drawing.Size(100, 20);
            this.tbLoginNethouse.TabIndex = 0;
            // 
            // tbPassNethouse
            // 
            this.tbPassNethouse.Location = new System.Drawing.Point(112, 32);
            this.tbPassNethouse.Name = "tbPassNethouse";
            this.tbPassNethouse.PasswordChar = '*';
            this.tbPassNethouse.Size = new System.Drawing.Size(100, 20);
            this.tbPassNethouse.TabIndex = 1;
            // 
            // cbSEO
            // 
            this.cbSEO.AutoSize = true;
            this.cbSEO.Location = new System.Drawing.Point(460, 168);
            this.cbSEO.Name = "cbSEO";
            this.cbSEO.Size = new System.Drawing.Size(100, 17);
            this.cbSEO.TabIndex = 22;
            this.cbSEO.Text = "Обновить СЕО";
            this.cbSEO.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 339);
            this.Controls.Add(this.cbSEO);
            this.Controls.Add(this.btnSaveTemplate);
            this.Controls.Add(this.btnImages);
            this.Controls.Add(this.tbKeywords);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.tbTitle);
            this.Controls.Add(this.rtbFullText);
            this.Controls.Add(this.rtbMiniText);
            this.Controls.Add(this.btnActual);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "JazzMoto";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSaveTemplate;
        private System.Windows.Forms.Button btnImages;
        private System.Windows.Forms.TextBox tbKeywords;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.TextBox tbTitle;
        private System.Windows.Forms.RichTextBox rtbFullText;
        private System.Windows.Forms.RichTextBox rtbMiniText;
        private System.Windows.Forms.Button btnActual;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbLoginNethouse;
        private System.Windows.Forms.TextBox tbPassNethouse;
        private System.Windows.Forms.CheckBox cbSEO;
    }
}

