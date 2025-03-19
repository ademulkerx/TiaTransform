namespace TiaTransform
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Txt_Plc = new System.Windows.Forms.TextBox();
            this.Txt_Pc = new System.Windows.Forms.TextBox();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.Btn_Copy = new DevExpress.XtraEditors.SimpleButton();
            this.Btn_Convert = new DevExpress.XtraEditors.SimpleButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Btn_Paste = new DevExpress.XtraEditors.SimpleButton();
            this.label3 = new System.Windows.Forms.Label();
            this.Nmc_DbNumber = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Nmc_DbNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // Txt_Plc
            // 
            this.Txt_Plc.Location = new System.Drawing.Point(630, 28);
            this.Txt_Plc.Multiline = true;
            this.Txt_Plc.Name = "Txt_Plc";
            this.Txt_Plc.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Txt_Plc.Size = new System.Drawing.Size(433, 474);
            this.Txt_Plc.TabIndex = 2;
            // 
            // Txt_Pc
            // 
            this.Txt_Pc.Location = new System.Drawing.Point(12, 28);
            this.Txt_Pc.Multiline = true;
            this.Txt_Pc.Name = "Txt_Pc";
            this.Txt_Pc.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Txt_Pc.Size = new System.Drawing.Size(433, 474);
            this.Txt_Pc.TabIndex = 1;
            // 
            // radioGroup1
            // 
            this.radioGroup1.EnterMoveNextControl = true;
            this.radioGroup1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioGroup1.Location = new System.Drawing.Point(451, 28);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.radioGroup1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.radioGroup1.Properties.Appearance.ForeColor = System.Drawing.Color.White;
            this.radioGroup1.Properties.Appearance.Options.UseBackColor = true;
            this.radioGroup1.Properties.Appearance.Options.UseFont = true;
            this.radioGroup1.Properties.Appearance.Options.UseForeColor = true;
            this.radioGroup1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Direkt DB", true, null, ""),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "HMI Tags"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Giriş/Çıkış (I/O) Durumu"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Sürücü Parametreleri")});
            this.radioGroup1.Size = new System.Drawing.Size(173, 204);
            this.radioGroup1.TabIndex = 10;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            this.radioGroup1.Paint += new System.Windows.Forms.PaintEventHandler(this.radioGroup1_Paint);
            // 
            // Btn_Copy
            // 
            this.Btn_Copy.Appearance.BackColor = System.Drawing.Color.White;
            this.Btn_Copy.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Btn_Copy.Appearance.Options.UseBackColor = true;
            this.Btn_Copy.Appearance.Options.UseFont = true;
            this.Btn_Copy.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("Btn_Copy.ImageOptions.SvgImage")));
            this.Btn_Copy.Location = new System.Drawing.Point(341, 460);
            this.Btn_Copy.Name = "Btn_Copy";
            this.Btn_Copy.Size = new System.Drawing.Size(85, 40);
            this.Btn_Copy.TabIndex = 11;
            this.Btn_Copy.Text = "Copy";
            this.Btn_Copy.Click += new System.EventHandler(this.Btn_Copy_Click);
            // 
            // Btn_Convert
            // 
            this.Btn_Convert.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(180)))));
            this.Btn_Convert.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Btn_Convert.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Btn_Convert.Appearance.Options.UseBackColor = true;
            this.Btn_Convert.Appearance.Options.UseFont = true;
            this.Btn_Convert.Appearance.Options.UseForeColor = true;
            this.Btn_Convert.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("Btn_Convert.ImageOptions.SvgImage")));
            this.Btn_Convert.Location = new System.Drawing.Point(451, 308);
            this.Btn_Convert.Name = "Btn_Convert";
            this.Btn_Convert.Size = new System.Drawing.Size(173, 43);
            this.Btn_Convert.TabIndex = 12;
            this.Btn_Convert.Text = "Dönüştür";
            this.Btn_Convert.Click += new System.EventHandler(this.Btn_Convert_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(630, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(433, 23);
            this.label1.TabIndex = 13;
            this.label1.Text = "TiaPortal Text Data";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(433, 23);
            this.label2.TabIndex = 14;
            this.label2.Text = "C# Class Variables";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Btn_Paste
            // 
            this.Btn_Paste.Appearance.BackColor = System.Drawing.Color.White;
            this.Btn_Paste.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Btn_Paste.Appearance.Options.UseBackColor = true;
            this.Btn_Paste.Appearance.Options.UseFont = true;
            this.Btn_Paste.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("Btn_Paste.ImageOptions.SvgImage")));
            this.Btn_Paste.Location = new System.Drawing.Point(959, 460);
            this.Btn_Paste.Name = "Btn_Paste";
            this.Btn_Paste.Size = new System.Drawing.Size(85, 40);
            this.Btn_Paste.TabIndex = 15;
            this.Btn_Paste.Text = "Paste";
            this.Btn_Paste.Click += new System.EventHandler(this.Btn_Paste_Click);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(468, 235);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 26);
            this.label3.TabIndex = 17;
            this.label3.Text = "DB Number :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Nmc_DbNumber
            // 
            this.Nmc_DbNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Nmc_DbNumber.Location = new System.Drawing.Point(468, 264);
            this.Nmc_DbNumber.Name = "Nmc_DbNumber";
            this.Nmc_DbNumber.Size = new System.Drawing.Size(138, 38);
            this.Nmc_DbNumber.TabIndex = 18;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1074, 509);
            this.Controls.Add(this.Nmc_DbNumber);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Btn_Paste);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Btn_Convert);
            this.Controls.Add(this.Btn_Copy);
            this.Controls.Add(this.radioGroup1);
            this.Controls.Add(this.Txt_Pc);
            this.Controls.Add(this.Txt_Plc);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.RightToLeftLayout = true;
            this.Text = " TiaTransform";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Nmc_DbNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Txt_Plc;
        private System.Windows.Forms.TextBox Txt_Pc;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.SimpleButton Btn_Copy;
        private DevExpress.XtraEditors.SimpleButton Btn_Convert;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.SimpleButton Btn_Paste;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown Nmc_DbNumber;
    }
}

