namespace InvadersDatFileConverter
{
    partial class frmMain
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
            this.btnLoadAndDisplayPlayer = new System.Windows.Forms.Button();
            this.cboDrawScale = new System.Windows.Forms.ComboBox();
            this.lblScale = new System.Windows.Forms.Label();
            this.btnLoadAndDisplayEnemy = new System.Windows.Forms.Button();
            this.btnLoadAndDisplayTitle = new System.Windows.Forms.Button();
            this.btnLoadAndDisplayPlayerLive = new System.Windows.Forms.Button();
            this.grpLoad = new System.Windows.Forms.GroupBox();
            this.grpConvert = new System.Windows.Forms.GroupBox();
            this.btnConvertEnemy = new System.Windows.Forms.Button();
            this.btnConvertPlayer = new System.Windows.Forms.Button();
            this.btnConvertPlayerLive = new System.Windows.Forms.Button();
            this.btnConvertTitle = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpLoad.SuspendLayout();
            this.grpConvert.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoadAndDisplayPlayer
            // 
            this.btnLoadAndDisplayPlayer.Location = new System.Drawing.Point(6, 77);
            this.btnLoadAndDisplayPlayer.Name = "btnLoadAndDisplayPlayer";
            this.btnLoadAndDisplayPlayer.Size = new System.Drawing.Size(114, 23);
            this.btnLoadAndDisplayPlayer.TabIndex = 0;
            this.btnLoadAndDisplayPlayer.Text = "Player";
            this.btnLoadAndDisplayPlayer.UseVisualStyleBackColor = true;
            this.btnLoadAndDisplayPlayer.Click += new System.EventHandler(this.btnLoadAndDisplayPlayer_Click);
            // 
            // cboDrawScale
            // 
            this.cboDrawScale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDrawScale.FormattingEnabled = true;
            this.cboDrawScale.Items.AddRange(new object[] {
            "1x",
            "2x",
            "3x",
            "4x",
            "5x",
            "6x"});
            this.cboDrawScale.Location = new System.Drawing.Point(58, 163);
            this.cboDrawScale.Name = "cboDrawScale";
            this.cboDrawScale.Size = new System.Drawing.Size(74, 21);
            this.cboDrawScale.TabIndex = 1;
            this.cboDrawScale.SelectedIndexChanged += new System.EventHandler(this.cboDrawScale_SelectedIndexChanged);
            // 
            // lblScale
            // 
            this.lblScale.AutoSize = true;
            this.lblScale.Location = new System.Drawing.Point(15, 166);
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(37, 13);
            this.lblScale.TabIndex = 2;
            this.lblScale.Text = "Scale:";
            // 
            // btnLoadAndDisplayEnemy
            // 
            this.btnLoadAndDisplayEnemy.Location = new System.Drawing.Point(6, 106);
            this.btnLoadAndDisplayEnemy.Name = "btnLoadAndDisplayEnemy";
            this.btnLoadAndDisplayEnemy.Size = new System.Drawing.Size(114, 23);
            this.btnLoadAndDisplayEnemy.TabIndex = 5;
            this.btnLoadAndDisplayEnemy.Text = "Enemy";
            this.btnLoadAndDisplayEnemy.UseVisualStyleBackColor = true;
            this.btnLoadAndDisplayEnemy.Click += new System.EventHandler(this.btnLoadAndDisplayEnemy_Click);
            // 
            // btnLoadAndDisplayTitle
            // 
            this.btnLoadAndDisplayTitle.Location = new System.Drawing.Point(6, 19);
            this.btnLoadAndDisplayTitle.Name = "btnLoadAndDisplayTitle";
            this.btnLoadAndDisplayTitle.Size = new System.Drawing.Size(114, 23);
            this.btnLoadAndDisplayTitle.TabIndex = 6;
            this.btnLoadAndDisplayTitle.Text = "Title";
            this.btnLoadAndDisplayTitle.UseVisualStyleBackColor = true;
            this.btnLoadAndDisplayTitle.Click += new System.EventHandler(this.btnLoadAndDisplayTitle_Click);
            // 
            // btnLoadAndDisplayPlayerLive
            // 
            this.btnLoadAndDisplayPlayerLive.Location = new System.Drawing.Point(6, 48);
            this.btnLoadAndDisplayPlayerLive.Name = "btnLoadAndDisplayPlayerLive";
            this.btnLoadAndDisplayPlayerLive.Size = new System.Drawing.Size(114, 23);
            this.btnLoadAndDisplayPlayerLive.TabIndex = 7;
            this.btnLoadAndDisplayPlayerLive.Text = "Player Live";
            this.btnLoadAndDisplayPlayerLive.UseVisualStyleBackColor = true;
            this.btnLoadAndDisplayPlayerLive.Click += new System.EventHandler(this.btnLoadAndDisplayPlayerLive_Click);
            // 
            // grpLoad
            // 
            this.grpLoad.Controls.Add(this.btnLoadAndDisplayTitle);
            this.grpLoad.Controls.Add(this.btnLoadAndDisplayPlayerLive);
            this.grpLoad.Controls.Add(this.btnLoadAndDisplayPlayer);
            this.grpLoad.Controls.Add(this.btnLoadAndDisplayEnemy);
            this.grpLoad.Location = new System.Drawing.Point(12, 8);
            this.grpLoad.Name = "grpLoad";
            this.grpLoad.Size = new System.Drawing.Size(131, 149);
            this.grpLoad.TabIndex = 8;
            this.grpLoad.TabStop = false;
            this.grpLoad.Text = "Load";
            // 
            // grpConvert
            // 
            this.grpConvert.Controls.Add(this.btnConvertEnemy);
            this.grpConvert.Controls.Add(this.btnConvertPlayer);
            this.grpConvert.Controls.Add(this.btnConvertPlayerLive);
            this.grpConvert.Controls.Add(this.btnConvertTitle);
            this.grpConvert.Location = new System.Drawing.Point(12, 190);
            this.grpConvert.Name = "grpConvert";
            this.grpConvert.Size = new System.Drawing.Size(131, 149);
            this.grpConvert.TabIndex = 9;
            this.grpConvert.TabStop = false;
            this.grpConvert.Text = "Convert";
            // 
            // btnConvertEnemy
            // 
            this.btnConvertEnemy.Location = new System.Drawing.Point(6, 106);
            this.btnConvertEnemy.Name = "btnConvertEnemy";
            this.btnConvertEnemy.Size = new System.Drawing.Size(114, 23);
            this.btnConvertEnemy.TabIndex = 3;
            this.btnConvertEnemy.Text = "Enemy";
            this.btnConvertEnemy.UseVisualStyleBackColor = true;
            this.btnConvertEnemy.Click += new System.EventHandler(this.btnConvertEnemy_Click);
            // 
            // btnConvertPlayer
            // 
            this.btnConvertPlayer.Location = new System.Drawing.Point(6, 77);
            this.btnConvertPlayer.Name = "btnConvertPlayer";
            this.btnConvertPlayer.Size = new System.Drawing.Size(114, 23);
            this.btnConvertPlayer.TabIndex = 2;
            this.btnConvertPlayer.Text = "Player";
            this.btnConvertPlayer.UseVisualStyleBackColor = true;
            this.btnConvertPlayer.Click += new System.EventHandler(this.btnConvertPlayer_Click);
            // 
            // btnConvertPlayerLive
            // 
            this.btnConvertPlayerLive.Location = new System.Drawing.Point(6, 48);
            this.btnConvertPlayerLive.Name = "btnConvertPlayerLive";
            this.btnConvertPlayerLive.Size = new System.Drawing.Size(114, 23);
            this.btnConvertPlayerLive.TabIndex = 1;
            this.btnConvertPlayerLive.Text = "Player Live";
            this.btnConvertPlayerLive.UseVisualStyleBackColor = true;
            this.btnConvertPlayerLive.Click += new System.EventHandler(this.btnConvertPlayerLive_Click);
            // 
            // btnConvertTitle
            // 
            this.btnConvertTitle.Location = new System.Drawing.Point(6, 19);
            this.btnConvertTitle.Name = "btnConvertTitle";
            this.btnConvertTitle.Size = new System.Drawing.Size(114, 23);
            this.btnConvertTitle.TabIndex = 0;
            this.btnConvertTitle.Text = "Title";
            this.btnConvertTitle.UseVisualStyleBackColor = true;
            this.btnConvertTitle.Click += new System.EventHandler(this.btnConvertTitle_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusInfo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 346);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(833, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusInfo
            // 
            this.toolStripStatusInfo.Name = "toolStripStatusInfo";
            this.toolStripStatusInfo.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusInfo.Text = "Info: -";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 368);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.grpConvert);
            this.Controls.Add(this.grpLoad);
            this.Controls.Add(this.lblScale);
            this.Controls.Add(this.cboDrawScale);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Invaders \'dat\' File Reader and Converter";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.grpLoad.ResumeLayout(false);
            this.grpConvert.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoadAndDisplayPlayer;
        private System.Windows.Forms.ComboBox cboDrawScale;
        private System.Windows.Forms.Label lblScale;
        private System.Windows.Forms.Button btnLoadAndDisplayEnemy;
        private System.Windows.Forms.Button btnLoadAndDisplayTitle;
        private System.Windows.Forms.Button btnLoadAndDisplayPlayerLive;
        private System.Windows.Forms.GroupBox grpLoad;
        private System.Windows.Forms.GroupBox grpConvert;
        private System.Windows.Forms.Button btnConvertEnemy;
        private System.Windows.Forms.Button btnConvertPlayer;
        private System.Windows.Forms.Button btnConvertPlayerLive;
        private System.Windows.Forms.Button btnConvertTitle;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusInfo;
    }
}

