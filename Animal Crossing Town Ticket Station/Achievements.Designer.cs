namespace Animal_Crossing_Town_Ticket_Station
{
    partial class Achievements
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
            btnBack = new Button();
            lblTest = new Label();
            lstAchievements = new ListView();
            picTest = new PictureBox();
            chkFilterAll = new CheckBox();
            chkFilterComplete = new CheckBox();
            chkFilterIncomplete = new CheckBox();
            lblFilters = new Label();
            ((System.ComponentModel.ISupportInitialize)picTest).BeginInit();
            SuspendLayout();
            // 
            // btnBack
            // 
            btnBack.BackColor = Color.Transparent;
            btnBack.Cursor = Cursors.Hand;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Font = new Font("Arial", 24F, FontStyle.Regular, GraphicsUnit.Point);
            btnBack.ForeColor = Color.Orange;
            btnBack.Location = new Point(16, 990);
            btnBack.Margin = new Padding(7, 8, 7, 8);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(174, 77);
            btnBack.TabIndex = 9;
            btnBack.Text = "Main Menu";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;
            // 
            // lblTest
            // 
            lblTest.AutoSize = true;
            lblTest.Location = new Point(557, 572);
            lblTest.Name = "lblTest";
            lblTest.Size = new Size(309, 36);
            lblTest.TabIndex = 10;
            lblTest.Text = "Display Listview Item";
            lblTest.Visible = false;
            // 
            // lstAchievements
            // 
            lstAchievements.BackColor = Color.FromArgb(193, 131, 106);
            lstAchievements.BorderStyle = BorderStyle.FixedSingle;
            lstAchievements.Font = new Font("Arial", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            lstAchievements.ForeColor = Color.FromArgb(32, 32, 32);
            lstAchievements.Location = new Point(908, 197);
            lstAchievements.MaximumSize = new Size(760, 820);
            lstAchievements.MinimumSize = new Size(10, 10);
            lstAchievements.Name = "lstAchievements";
            lstAchievements.Size = new Size(760, 820);
            lstAchievements.TabIndex = 11;
            lstAchievements.UseCompatibleStateImageBehavior = false;
            lstAchievements.ItemMouseHover += lstAchievements_ItemMouseHover;
            lstAchievements.SelectedIndexChanged += lstAchievements_SelectedIndexChanged;
            lstAchievements.MouseLeave += lstAchievements_MouseLeave;
            // 
            // picTest
            // 
            picTest.BackColor = Color.WhiteSmoke;
            picTest.Location = new Point(557, 611);
            picTest.Name = "picTest";
            picTest.Size = new Size(100, 50);
            picTest.TabIndex = 12;
            picTest.TabStop = false;
            picTest.Visible = false;
            // 
            // chkFilterAll
            // 
            chkFilterAll.AutoSize = true;
            chkFilterAll.BackColor = SystemColors.Control;
            chkFilterAll.ForeColor = Color.Transparent;
            chkFilterAll.Location = new Point(601, 378);
            chkFilterAll.Name = "chkFilterAll";
            chkFilterAll.Size = new Size(15, 14);
            chkFilterAll.TabIndex = 17;
            chkFilterAll.UseVisualStyleBackColor = false;
            chkFilterAll.CheckedChanged += chkFilterAll_CheckedChanged;
            chkFilterAll.MouseEnter += chkFilterAll_MouseEnter;
            chkFilterAll.MouseLeave += chkFilterAll_MouseLeave;
            // 
            // chkFilterComplete
            // 
            chkFilterComplete.AutoSize = true;
            chkFilterComplete.BackColor = SystemColors.Control;
            chkFilterComplete.ForeColor = Color.Transparent;
            chkFilterComplete.Location = new Point(682, 378);
            chkFilterComplete.Name = "chkFilterComplete";
            chkFilterComplete.Size = new Size(15, 14);
            chkFilterComplete.TabIndex = 18;
            chkFilterComplete.UseVisualStyleBackColor = false;
            chkFilterComplete.CheckedChanged += chkFilterComplete_CheckedChanged;
            chkFilterComplete.MouseEnter += chkFilterComplete_MouseEnter;
            chkFilterComplete.MouseLeave += chkFilterComplete_MouseLeave;
            // 
            // chkFilterIncomplete
            // 
            chkFilterIncomplete.AutoSize = true;
            chkFilterIncomplete.BackColor = SystemColors.Control;
            chkFilterIncomplete.ForeColor = Color.Transparent;
            chkFilterIncomplete.Location = new Point(761, 378);
            chkFilterIncomplete.Name = "chkFilterIncomplete";
            chkFilterIncomplete.Size = new Size(15, 14);
            chkFilterIncomplete.TabIndex = 19;
            chkFilterIncomplete.UseVisualStyleBackColor = false;
            chkFilterIncomplete.CheckedChanged += chkFilterIncomplete_CheckedChanged;
            chkFilterIncomplete.MouseEnter += chkFilterIncomplete_MouseEnter;
            chkFilterIncomplete.MouseLeave += chkFilterIncomplete_MouseLeave;
            // 
            // lblFilters
            // 
            lblFilters.AutoSize = true;
            lblFilters.BackColor = Color.FromArgb(193, 131, 106);
            lblFilters.Font = new Font("Arial", 14.2499981F, FontStyle.Regular, GraphicsUnit.Point);
            lblFilters.Location = new Point(559, 470);
            lblFilters.MaximumSize = new Size(300, 62);
            lblFilters.MinimumSize = new Size(300, 62);
            lblFilters.Name = "lblFilters";
            lblFilters.Size = new Size(300, 62);
            lblFilters.TabIndex = 20;
            lblFilters.Text = "Completed Achievements";
            lblFilters.TextAlign = ContentAlignment.TopCenter;
            // 
            // Achievements
            // 
            AutoScaleDimensions = new SizeF(18F, 36F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.UI_BG_Achievements;
            ClientSize = new Size(1920, 1061);
            Controls.Add(lblFilters);
            Controls.Add(chkFilterIncomplete);
            Controls.Add(chkFilterComplete);
            Controls.Add(chkFilterAll);
            Controls.Add(picTest);
            Controls.Add(lstAchievements);
            Controls.Add(lblTest);
            Controls.Add(btnBack);
            DoubleBuffered = true;
            Font = new Font("Arial", 24F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(10, 8, 10, 8);
            MaximumSize = new Size(1920, 1080);
            MinimumSize = new Size(1918, 1038);
            Name = "Achievements";
            Text = "Achievements";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)picTest).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnBack;
        private Label lblTest;
        private ListView lstAchievements;
        private PictureBox picTest;
        private CheckBox chkFilterAll;
        private CheckBox chkFilterComplete;
        private CheckBox chkFilterIncomplete;
        private Label lblFilters;
    }
}