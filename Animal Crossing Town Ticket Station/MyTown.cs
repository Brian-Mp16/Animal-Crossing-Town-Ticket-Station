using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Animal_Crossing_Town_Ticket_Station
{
    public partial class MyTown : Form
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection();
        Font? fontConsolas15;
        Font? fontConsolas16;
        Font? fontConsolas21;
        Font? fontFink12;
        Font? fontFink18;
        Font? fontFink20;
        Font? fontFink21;
        Font? fontFink24;
        DateTime loadTime;
        DateTime birthday;
        TimeSpan timeOffset = new TimeSpan(0, 0, 0, 0, 0);
        static string filedir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\ACTownTickets\";
        static string filepath = filedir + "SaveData.txt";
        static int intDataLines = 50;
        string[] data = new string[intDataLines];
        bool hasShovel = false;
        bool hasNet = false;
        bool hasRod = false;
        bool hasAxe = false;
        bool hasIsland = false;
        bool hasEreader = false;
        int intNookStore = 0;
        bool boolChangeName = false;
        bool boolBirthdaySet = false;
        string strPlayerName = "";
        string strTownName = "";
        string strNameTemp = "Not Set";
        string strTownTemp = "Not Set";
        int intTimePlayedSeconds = 0;
        bool boolBrianMp16VideosOff = false;

        public MyTown()
        {
            InitializeComponent();
            CreateFonts();

            loadData();
            UpdateClockImages();
            ChangeCheckboxes();
            ChangeLabels();

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Tick += new EventHandler(UpdateClock);
            timer.Start();
        }

        private void CreateFonts()
        {
            byte[] fontData = Properties.Resources.FinkHeavy_Regular;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Properties.Resources.FinkHeavy_Regular.Length);
            AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.FinkHeavy_Regular.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);

            byte[] fontData1 = Properties.Resources.JetBrainsMono_Regular;
            IntPtr fontPtr1 = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData1.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData1, 1, fontPtr1, fontData1.Length);
            uint dummy1 = 0;
            fonts.AddMemoryFont(fontPtr1, Properties.Resources.JetBrainsMono_Regular.Length);
            AddFontMemResourceEx(fontPtr1, (uint)Properties.Resources.JetBrainsMono_Regular.Length, IntPtr.Zero, ref dummy1);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr1);

            fontConsolas15 = new Font(fonts.Families[1], 14.25F);
            fontConsolas16 = new Font(fonts.Families[1], 15.75F);
            fontConsolas21 = new Font(fonts.Families[1], 20.25F);
            fontFink12 = new Font(fonts.Families[0], 12.0F);
            fontFink18 = new Font(fonts.Families[0], 18.0F);
            fontFink21 = new Font(fonts.Families[0], 20.25F);
            fontFink20 = new Font(fonts.Families[0], 20.25F, FontStyle.Underline, GraphicsUnit.Point);
            fontFink24 = new Font(fonts.Families[0], 24.0F);

            btnBack.Font = fontFink24;
            calChangeTimeCalendar.Font = fontFink12;
            btnChangeTime.Font = fontFink18;
            lblChangeTime.Font = fontFink21;
            cmbChangeTimeHour.Font = fontFink18; // mismatched in Windows Form Builder, Segoe UI (18F)
            cmbChangeTimeMinute.Font = fontFink18; // mismatched in Windows Form Builder, Segoe UI (18F)
            btnChangeTimeAccept.Font = fontFink18;
            btnChangeTimeCancel.Font = fontFink18;
            btnChangeTimeActual.Font = fontFink18;
            btnEraseData.Font = fontFink21;
            btnChangeName.Font = fontFink18;
            txtPlayerName.Font = fontConsolas21;
            txtTownName.Font = fontConsolas21;
            txtBirthday.Font = fontConsolas21;
            txtSymbols.Font = fontConsolas15;
            lblPlayerName.Font = fontFink20;
            lblTownName.Font = fontFink20;
            lblBirthday.Font = fontFink20;
            lblSymbolHelp.Font = fontFink18;
            chkBrianMp16.Font = fontConsolas16;
        }

        private void ChangeCheckboxes()
        {
            chkHaveShovel.Appearance = Appearance.Button;
            chkHaveShovel.BackColor = Color.FromArgb(255, 147, 194, 255);
            chkHaveShovel.AutoSize = false;
            chkHaveShovel.Size = new Size(40, 40);

            chkHaveNet.Appearance = Appearance.Button;
            chkHaveNet.BackColor = Color.FromArgb(255, 147, 194, 255);
            chkHaveNet.AutoSize = false;
            chkHaveNet.Size = new Size(40, 40);

            chkHaveRod.Appearance = Appearance.Button;
            chkHaveRod.BackColor = Color.FromArgb(255, 147, 194, 255);
            chkHaveRod.AutoSize = false;
            chkHaveRod.Size = new Size(40, 40);

            chkHaveAxe.Appearance = Appearance.Button;
            chkHaveAxe.BackColor = Color.FromArgb(255, 147, 194, 255);
            chkHaveAxe.AutoSize = false;
            chkHaveAxe.Size = new Size(40, 40);

            chkHaveIsland.Appearance = Appearance.Button;
            chkHaveIsland.BackColor = Color.FromArgb(255, 147, 194, 255);
            chkHaveIsland.AutoSize = false;
            chkHaveIsland.Size = new Size(40, 40);

            chkHaveEReader.Appearance = Appearance.Button;
            chkHaveEReader.BackColor = Color.FromArgb(255, 147, 194, 255);
            chkHaveEReader.AutoSize = false;
            chkHaveEReader.Size = new Size(40, 40);

            chkStoreNooksCranny.Appearance = Appearance.Button;
            chkStoreNooksCranny.BackColor = Color.FromArgb(255, 147, 194, 255);
            chkStoreNooksCranny.AutoSize = false;
            chkStoreNooksCranny.Size = new Size(40, 40);

            chkStoreNookNGo.Appearance = Appearance.Button;
            chkStoreNookNGo.BackColor = Color.FromArgb(255, 147, 194, 255);
            chkStoreNookNGo.AutoSize = false;
            chkStoreNookNGo.Size = new Size(40, 40);

            chkStoreNookway.Appearance = Appearance.Button;
            chkStoreNookway.BackColor = Color.FromArgb(255, 147, 194, 255);
            chkStoreNookway.AutoSize = false;
            chkStoreNookway.Size = new Size(40, 40);

            chkStoreNookingtons.Appearance = Appearance.Button;
            chkStoreNookingtons.BackColor = Color.FromArgb(255, 147, 194, 255);
            chkStoreNookingtons.AutoSize = false;
            chkStoreNookingtons.Size = new Size(40, 40);

            chkStoreRemodeling.Appearance = Appearance.Button;
            chkStoreRemodeling.BackColor = Color.FromArgb(255, 147, 194, 255);
            chkStoreRemodeling.AutoSize = false;
            chkStoreRemodeling.Size = new Size(40, 40);

            ChangeCheckboxChecks();
        }

        private void ChangeCheckboxChecks()
        {
            chkHaveShovel.BackgroundImage = hasShovel == true ? Properties.Resources.ST_Checkmark : null;
            chkHaveNet.BackgroundImage = hasNet == true ? Properties.Resources.ST_Checkmark : null;
            chkHaveRod.BackgroundImage = hasRod == true ? Properties.Resources.ST_Checkmark : null;
            chkHaveAxe.BackgroundImage = hasAxe == true ? Properties.Resources.ST_Checkmark : null;
            chkHaveIsland.BackgroundImage = hasIsland == true ? Properties.Resources.ST_Checkmark : null;
            chkHaveEReader.BackgroundImage = hasEreader == true ? Properties.Resources.ST_Checkmark : null;
            chkStoreNooksCranny.BackgroundImage = intNookStore == 1 ? Properties.Resources.ST_Checkmark : null;
            chkStoreNookNGo.BackgroundImage = intNookStore == 2 ? Properties.Resources.ST_Checkmark : null;
            chkStoreNookway.BackgroundImage = intNookStore == 3 ? Properties.Resources.ST_Checkmark : null;
            chkStoreNookingtons.BackgroundImage = intNookStore == 4 ? Properties.Resources.ST_Checkmark : null;
            chkStoreRemodeling.BackgroundImage = intNookStore == 5 ? Properties.Resources.ST_Checkmark : null;
            chkBrianMp16.Checked = boolBrianMp16VideosOff == true ? chkBrianMp16.Checked = true : false;

            saveData();
        }

        private void ChangeLabels()
        {
            txtPlayerName.Text = strPlayerName;
            txtTownName.Text = strTownName;
            if (boolBirthdaySet == false)
                txtBirthday.Text = "Not Set";
            else
                txtBirthday.Text = String.Format("{0} {1}", birthday.ToString("MMMM"), AddOrdinal(birthday.Day));

            lblBirthday.Visible = true;
            lblPlayerName.Visible = true;
            lblTownName.Visible = true;
            txtBirthday.Visible = true;
            txtPlayerName.Visible = true;
            txtTownName.Visible = true;
            txtPlayerName.Enabled = false;
            txtTownName.Enabled = false;
            txtBirthday.Enabled = false;

            if (boolChangeName == false)
            {
                txtPlayerName.ReadOnly = true;
                txtPlayerName.BackColor = Color.FromArgb(255, 147, 194, 255);
                txtPlayerName.BorderStyle = BorderStyle.None;
                txtTownName.ReadOnly = true;
                txtTownName.BackColor = Color.FromArgb(255, 147, 194, 255);
                txtTownName.BorderStyle = BorderStyle.None;
                txtBirthday.BackColor = Color.FromArgb(255, 147, 194, 255);
                txtBirthday.BorderStyle = BorderStyle.None;
            }
            else
            {
                txtPlayerName.ReadOnly = false;
                txtPlayerName.BackColor = Color.PapayaWhip;
                txtPlayerName.BorderStyle = BorderStyle.Fixed3D;
                txtTownName.ReadOnly = false;
                txtTownName.BackColor = Color.PapayaWhip;
                txtTownName.BorderStyle = BorderStyle.Fixed3D;
                txtBirthday.BackColor = Color.Gainsboro;
                txtBirthday.BorderStyle = BorderStyle.Fixed3D;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            saveData();
            this.Hide();
        }

        private void saveData()
        {
            data = SaveData.GetSaveData().Item1.Data;
            data[1] = timeOffset.ToString();
            data[2] = strPlayerName;
            data[3] = strTownName;
            data[5] = hasShovel == true ? "true" : "false";
            data[6] = hasNet == true ? "true" : "false";
            data[7] = hasRod == true ? "true" : "false";
            data[8] = hasAxe == true ? "true" : "false";
            data[9] = hasIsland == true ? "true" : "false";
            data[10] = hasEreader == true ? "true" : "false";
            data[27] = intTimePlayedSeconds.ToString();
            data[28] = birthday.ToString();
            data[39] = boolBirthdaySet == true ? "true" : "false";
            data[42] = boolBrianMp16VideosOff == true ? "true" : "false";
            switch (intNookStore)
            {
                case 1: data[11] = "1"; break;
                case 2: data[11] = "2"; break;
                case 3: data[11] = "3"; break;
                case 4: data[11] = "4"; break;
                case 5: data[11] = "5"; break;
                default: data[11] = "1"; break;
            }
            File.WriteAllLines(filepath, data);
        }

        private void loadData()
        {
            data = SaveData.GetSaveData().Item1.Data;

            loadTime = data[0] != "" ? Convert.ToDateTime(data[0]) : DateTime.Now;
            if (data[1] != "")
                timeOffset = TimeSpan.Parse(data[1]);
            strPlayerName = data[2] == "" ? "Not Set" : data[2];
            strTownName = data[3] == "" ? "Not Set" : data[3];
            hasShovel = data[5] == "true" ? true : false;
            hasNet = data[6] == "true" ? true : false;
            hasRod = data[7] == "true" ? true : false;
            hasAxe = data[8] == "true" ? true : false;
            hasIsland = data[9] == "true" ? true : false;
            hasEreader = data[10] == "true" ? true : false;
            intTimePlayedSeconds = data[27] != "" ? Convert.ToInt32(data[27]) : 0;
            birthday = data[28] != "" ? Convert.ToDateTime(data[28]) : DateTime.Now;
            boolBirthdaySet = data[39] == "true" ? true : false;
            boolBrianMp16VideosOff = data[42].ToLower() == "true" ? true : false;
            switch (data[11])
            {
                case "1": intNookStore = 1; break;
                case "2": intNookStore = 2; break;
                case "3": intNookStore = 3; break;
                case "4": intNookStore = 4; break;
                case "5": intNookStore = 5; break;
                default: intNookStore = 1; break;
            }
        }

        private void UpdateClock(object sender, EventArgs e)
        {
            GC.Collect();

            intTimePlayedSeconds++;
            UpdateClockImages();


        }

        private void UpdateClockImages()
        {
            DateTime now = DateTime.Now + timeOffset;
            switch (now.Month)
            {
                case 1: imgCL_day1.Image = Properties.Resources.CL_date_1; break;
                case 2: imgCL_day1.Image = Properties.Resources.CL_date_2; break;
                case 3: imgCL_day1.Image = Properties.Resources.CL_date_3; break;
                case 4: imgCL_day1.Image = Properties.Resources.CL_date_4; break;
                case 5: imgCL_day1.Image = Properties.Resources.CL_date_5; break;
                case 6: imgCL_day1.Image = Properties.Resources.CL_date_6; break;
                case 7: imgCL_day1.Image = Properties.Resources.CL_date_7; break;
                case 8: imgCL_day1.Image = Properties.Resources.CL_date_8; break;
                case 9: imgCL_day1.Image = Properties.Resources.CL_date_9; break;
                case 10: imgCL_day1.Image = Properties.Resources.CL_date_10; break;
                case 11: imgCL_day1.Image = Properties.Resources.CL_date_11; break;
                case 12: imgCL_day1.Image = Properties.Resources.CL_date_12; break;
                default: imgCL_day1.Image = Properties.Resources.CL_date_1; break;
            }
            switch (now.Day)
            {
                case 1: imgCL_day2.Image = Properties.Resources.CL_date_1; break;
                case 2: imgCL_day2.Image = Properties.Resources.CL_date_2; break;
                case 3: imgCL_day2.Image = Properties.Resources.CL_date_3; break;
                case 4: imgCL_day2.Image = Properties.Resources.CL_date_4; break;
                case 5: imgCL_day2.Image = Properties.Resources.CL_date_5; break;
                case 6: imgCL_day2.Image = Properties.Resources.CL_date_6; break;
                case 7: imgCL_day2.Image = Properties.Resources.CL_date_7; break;
                case 8: imgCL_day2.Image = Properties.Resources.CL_date_8; break;
                case 9: imgCL_day2.Image = Properties.Resources.CL_date_9; break;
                case 10: imgCL_day2.Image = Properties.Resources.CL_date_10; break;
                case 11: imgCL_day2.Image = Properties.Resources.CL_date_11; break;
                case 12: imgCL_day2.Image = Properties.Resources.CL_date_12; break;
                case 13: imgCL_day2.Image = Properties.Resources.CL_date_13; break;
                case 14: imgCL_day2.Image = Properties.Resources.CL_date_14; break;
                case 15: imgCL_day2.Image = Properties.Resources.CL_date_15; break;
                case 16: imgCL_day2.Image = Properties.Resources.CL_date_16; break;
                case 17: imgCL_day2.Image = Properties.Resources.CL_date_17; break;
                case 18: imgCL_day2.Image = Properties.Resources.CL_date_18; break;
                case 19: imgCL_day2.Image = Properties.Resources.CL_date_19; break;
                case 20: imgCL_day2.Image = Properties.Resources.CL_date_20; break;
                case 21: imgCL_day2.Image = Properties.Resources.CL_date_21; break;
                case 22: imgCL_day2.Image = Properties.Resources.CL_date_22; break;
                case 23: imgCL_day2.Image = Properties.Resources.CL_date_23; break;
                case 24: imgCL_day2.Image = Properties.Resources.CL_date_24; break;
                case 25: imgCL_day2.Image = Properties.Resources.CL_date_25; break;
                case 26: imgCL_day2.Image = Properties.Resources.CL_date_26; break;
                case 27: imgCL_day2.Image = Properties.Resources.CL_date_27; break;
                case 28: imgCL_day2.Image = Properties.Resources.CL_date_28; break;
                case 29: imgCL_day2.Image = Properties.Resources.CL_date_29; break;
                case 30: imgCL_day2.Image = Properties.Resources.CL_date_30; break;
                case 31: imgCL_day2.Image = Properties.Resources.CL_date_31; break;
                default: imgCL_day2.Image = Properties.Resources.CL_date_1; break;
            }
            switch (now.DayOfWeek)
            {
                case DayOfWeek.Sunday: imgCL_weekday.Image = Properties.Resources.CL_day_sun; break;
                case DayOfWeek.Monday: imgCL_weekday.Image = Properties.Resources.CL_day_mon; break;
                case DayOfWeek.Tuesday: imgCL_weekday.Image = Properties.Resources.CL_day_tue; break;
                case DayOfWeek.Wednesday: imgCL_weekday.Image = Properties.Resources.CL_day_wed; break;
                case DayOfWeek.Thursday: imgCL_weekday.Image = Properties.Resources.CL_day_thu; break;
                case DayOfWeek.Friday: imgCL_weekday.Image = Properties.Resources.CL_day_fri; break;
                case DayOfWeek.Saturday: imgCL_weekday.Image = Properties.Resources.CL_day_sat; break;
                default: imgCL_weekday.Image = Properties.Resources.CL_day_sun; break;
            }
            switch (now.Hour % 12)
            {
                case 0: imgCL_hour1.Image = Properties.Resources.CL_time_1; imgCL_hour2.Image = Properties.Resources.CL_time_2; break;
                case 1: imgCL_hour1.Image = Properties.Resources.CL_time_0; imgCL_hour2.Image = Properties.Resources.CL_time_1; break;
                case 2: imgCL_hour1.Image = Properties.Resources.CL_time_0; imgCL_hour2.Image = Properties.Resources.CL_time_2; break;
                case 3: imgCL_hour1.Image = Properties.Resources.CL_time_0; imgCL_hour2.Image = Properties.Resources.CL_time_3; break;
                case 4: imgCL_hour1.Image = Properties.Resources.CL_time_0; imgCL_hour2.Image = Properties.Resources.CL_time_4; break;
                case 5: imgCL_hour1.Image = Properties.Resources.CL_time_0; imgCL_hour2.Image = Properties.Resources.CL_time_5; break;
                case 6: imgCL_hour1.Image = Properties.Resources.CL_time_0; imgCL_hour2.Image = Properties.Resources.CL_time_6; break;
                case 7: imgCL_hour1.Image = Properties.Resources.CL_time_0; imgCL_hour2.Image = Properties.Resources.CL_time_7; break;
                case 8: imgCL_hour1.Image = Properties.Resources.CL_time_0; imgCL_hour2.Image = Properties.Resources.CL_time_8; break;
                case 9: imgCL_hour1.Image = Properties.Resources.CL_time_0; imgCL_hour2.Image = Properties.Resources.CL_time_9; break;
                case 10: imgCL_hour1.Image = Properties.Resources.CL_time_1; imgCL_hour2.Image = Properties.Resources.CL_time_0; break;
                case 11: imgCL_hour1.Image = Properties.Resources.CL_time_1; imgCL_hour2.Image = Properties.Resources.CL_time_1; break;
                default: imgCL_hour1.Image = Properties.Resources.CL_time_0; imgCL_hour2.Image = Properties.Resources.CL_time_0; break;
            }
            switch (Math.Floor((double)(now.Minute / 10)))
            {
                case 0: imgCL_min1.Image = Properties.Resources.CL_time_0; break;
                case 1: imgCL_min1.Image = Properties.Resources.CL_time_1; break;
                case 2: imgCL_min1.Image = Properties.Resources.CL_time_2; break;
                case 3: imgCL_min1.Image = Properties.Resources.CL_time_3; break;
                case 4: imgCL_min1.Image = Properties.Resources.CL_time_4; break;
                case 5: imgCL_min1.Image = Properties.Resources.CL_time_5; break;
                default: imgCL_min1.Image = Properties.Resources.CL_time_0; break;
            }
            switch (now.Minute % 10)
            {
                case 0: imgCL_min2.Image = Properties.Resources.CL_time_0; break;
                case 1: imgCL_min2.Image = Properties.Resources.CL_time_1; break;
                case 2: imgCL_min2.Image = Properties.Resources.CL_time_2; break;
                case 3: imgCL_min2.Image = Properties.Resources.CL_time_3; break;
                case 4: imgCL_min2.Image = Properties.Resources.CL_time_4; break;
                case 5: imgCL_min2.Image = Properties.Resources.CL_time_5; break;
                case 6: imgCL_min2.Image = Properties.Resources.CL_time_6; break;
                case 7: imgCL_min2.Image = Properties.Resources.CL_time_7; break;
                case 8: imgCL_min2.Image = Properties.Resources.CL_time_8; break;
                case 9: imgCL_min2.Image = Properties.Resources.CL_time_9; break;
                default: imgCL_min2.Image = Properties.Resources.CL_time_0; break;
            }
            switch (Math.Floor((double)(now.Hour / 12)))
            {
                case 0: imgCL_half.Image = Properties.Resources.CL_time_am; break;
                case 1: imgCL_half.Image = Properties.Resources.CL_time_pm; break;
                default: imgCL_half.Image = Properties.Resources.CL_time_am; break;
            }

            GC.Collect();
        }

        public static string AddOrdinal(int num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }
        }

        public void CheckIfNamesAreValid()
        {
            string strName = txtPlayerName.Text;
            string strTown = txtTownName.Text;

            StringInfo info = new StringInfo(strName);
            int unicode_length = info.LengthInTextElements;
            if (unicode_length > 8)
                txtPlayerName.Text = "Too Long";
            info = new StringInfo(strTown);
            unicode_length = info.LengthInTextElements;
            if (unicode_length > 8)
                txtTownName.Text = "Too Long";

            bool boolNamePass = true;
            foreach (char c in strName)
                if (txtSymbols.Text.Contains(c) == false)
                    boolNamePass = false;
            if (boolNamePass == false)
                txtPlayerName.Text = "Invalid";

            bool boolTownPass = true;
            foreach (char c in strTown)
                if (txtSymbols.Text.Contains(c) == false)
                    boolTownPass = false;
            if (boolTownPass == false)
                txtTownName.Text = "Invalid";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHaveShovel.Checked == true)
                hasShovel = true;
            else
                hasShovel = false;
            ChangeCheckboxChecks();
        }

        private void chkHaveNet_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHaveNet.Checked == true)
                hasNet = true;
            else
                hasNet = false;
            ChangeCheckboxChecks();
        }

        private void chkHaveRod_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHaveRod.Checked == true)
                hasRod = true;
            else
                hasRod = false;
            ChangeCheckboxChecks();
        }

        private void chkHaveAxe_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHaveAxe.Checked == true)
                hasAxe = true;
            else
                hasAxe = false;
            ChangeCheckboxChecks();
        }

        private void chkHaveIsland_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHaveIsland.Checked == true)
                hasIsland = true;
            else
                hasIsland = false;
            ChangeCheckboxChecks();
        }

        private void chkHaveEReader_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHaveEReader.Checked == true)
                hasEreader = true;
            else
                hasEreader = false;
            ChangeCheckboxChecks();
        }

        private void chkStoreNooksCranny_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStoreNooksCranny.Checked == true)
                intNookStore = 1;
            ChangeCheckboxChecks();
        }

        private void chkStoreNookNGo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStoreNookNGo.Checked == true)
                intNookStore = 2;
            ChangeCheckboxChecks();
        }

        private void chkStoreNookway_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStoreNookway.Checked == true)
                intNookStore = 3;
            ChangeCheckboxChecks();
        }

        private void chkStoreNookingtons_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStoreNookingtons.Checked == true)
                intNookStore = 4;
            ChangeCheckboxChecks();
        }

        private void chkStoreRemodeling_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStoreRemodeling.Checked == true)
                intNookStore = 5;
            ChangeCheckboxChecks();
        }

        private void monthCalendar1_KeyUp(object sender, KeyEventArgs e)
        {
            DateTime curTime = calChangeTimeCalendar.SelectionRange.Start;
            txtBirthday.Text = curTime.ToString();
        }

        private void btnChangeTime_Click(object sender, EventArgs e)
        {
            lblChangeTime.Visible = true;
            lblChangeTime.Text = "Choose a new Time";
            btnChangeTimeActual.Visible = true;
            calChangeTimeCalendar.Visible = true;
            calChangeTimeCalendar.SetDate(DateTime.Now + timeOffset);
            cmbChangeTimeHour.Visible = true;
            cmbChangeTimeHour.SelectedIndex = (DateTime.Now + timeOffset).Hour;
            cmbChangeTimeMinute.SelectedIndex = (DateTime.Now + timeOffset).Minute;
            cmbChangeTimeMinute.Visible = true;
            btnChangeTimeAccept.Visible = true;
            btnChangeTimeCancel.Visible = true;
            btnEraseData.Visible = true;
            btnChangeTime.Visible = false;
            btnChangeName.Visible = false;

            lblBirthday.Visible = false;
            lblPlayerName.Visible = false;
            lblTownName.Visible = false;
            txtBirthday.Visible = false;
            txtPlayerName.Visible = false;
            txtTownName.Visible = false;
        }

        private void btnChangeTimeAccept_Click(object sender, EventArgs e)
        {
            DateTime dateNew = calChangeTimeCalendar.SelectionRange.Start;
            int intHour = cmbChangeTimeHour.SelectedIndex;
            int intMinute = cmbChangeTimeMinute.SelectedIndex;
            if (boolChangeName == false)
            {
                timeOffset = new DateTime(dateNew.Year, dateNew.Month, dateNew.Day, intHour, intMinute, 0).Subtract(DateTime.Now);
            }
            else
            {
                boolBirthdaySet = true;
                birthday = new DateTime(dateNew.Year, dateNew.Month, dateNew.Day);
                txtBirthday.Text = String.Format("{0} {1}", birthday.ToString("MMMM"), AddOrdinal(birthday.Day));
                CheckIfNamesAreValid();
                strPlayerName = txtPlayerName.Text;
                strTownName = txtTownName.Text;
            }

            boolChangeName = false;
            ChangeLabels();
            UpdateLabelsOff();
            saveData();
        }

        private void btnChangeTimeCancel_Click(object sender, EventArgs e)
        {
            boolChangeName = false;
            txtPlayerName.Text = strNameTemp;
            txtTownName.Text = strTownTemp;

            ChangeLabels();
            UpdateLabelsOff();
        }

        private void btnChangeTimeActual_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            calChangeTimeCalendar.SelectionStart = now;
            cmbChangeTimeHour.SelectedIndex = now.Hour;
            cmbChangeTimeMinute.SelectedIndex = now.Minute;
        }

        private void UpdateLabelsOff()
        {
            lblChangeTime.Visible = false;
            btnChangeTimeActual.Visible = false;
            calChangeTimeCalendar.Visible = false;
            cmbChangeTimeHour.Visible = false;
            cmbChangeTimeMinute.Visible = false;
            btnChangeTimeAccept.Visible = false;
            btnChangeTimeCancel.Visible = false;
            btnChangeName.Visible = true;
            btnChangeTime.Visible = true;
            btnEraseData.Visible = false;
            txtSymbols.Visible = false;
            lblSymbolHelp.Visible = false;
        }

        private void CheckToCreateSaveData()
        {
            if (SaveData.GetSaveData().Item1.Data == null)
            {
                for (int i = 0; i < intDataLines; i++)
                    data[i] = "";
                SaveData.GetSaveData().Item1.Data = data;

                if (File.Exists(filepath))
                {
                    SaveData.GetSaveData().Item1.Data = File.ReadAllLines(filepath);
                }
                else
                {
                    if (!Directory.Exists(filedir))
                        Directory.CreateDirectory(filedir);
                    saveData();
                }
            }
        }


        private void btnEraseData_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Do you really want to erase all your Data?", "ERASE ALL DATA??", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
            if (dr == DialogResult.Yes)
            {
                DialogResult dr2 = MessageBox.Show("Are you REALLY sure want to erase all your DATA?", "ERASE ALL DATA??", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (dr2 == DialogResult.Yes)
                {
                    DateTime now = DateTime.Now;
                    timeOffset = TimeSpan.Zero;
                    hasShovel = false;
                    hasNet = false;
                    hasRod = false;
                    hasAxe = false;
                    hasIsland = false;
                    hasEreader = false;
                    intNookStore = 1;
                    chkHaveShovel.Checked = false;
                    chkHaveNet.Checked = false;
                    chkHaveRod.Checked = false;
                    chkHaveAxe.Checked = false;
                    chkHaveIsland.Checked = false;
                    chkHaveEReader.Checked = false;
                    chkStoreNooksCranny.Checked = true;
                    strPlayerName = "Not Set";
                    strTownName = "Not Set";
                    boolBirthdaySet = false;
                    birthday = DateTime.Now;
                    intTimePlayedSeconds = 0;

                    if (File.Exists(filepath))
                        File.Delete(filepath);

                    SaveData.GetSaveData().Item1.Data = null;
                    CheckToCreateSaveData();

                    ChangeLabels();
                    UpdateLabelsOff();
                }
                else if (dr2 == DialogResult.No)
                {
                    chkBrianMp16.Visible = true;
                }
            }
        }

        private void btnChangeName_Click(object sender, EventArgs e)
        {
            strNameTemp = txtPlayerName.Text;
            strTownTemp = txtTownName.Text;
            boolChangeName = true;
            ChangeLabels();
            txtPlayerName.Enabled = true;
            txtTownName.Enabled = true;
            txtBirthday.Enabled = true;
            btnChangeName.Visible = false;
            btnChangeTime.Visible = false;
            lblChangeTime.Visible = true;
            lblChangeTime.Text = "When's your Birthday?";
            txtSymbols.Visible = true;
            lblSymbolHelp.Visible = true;
            btnChangeTimeCancel.Visible = true;
            btnChangeTimeAccept.Visible = true;
            calChangeTimeCalendar.Visible = true;
            if (boolBirthdaySet == true)
                calChangeTimeCalendar.SelectionStart = birthday;
            else
                calChangeTimeCalendar.SelectionStart = DateTime.Now;
        }

        private void calChangeTimeCalendar_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void calChangeTimeCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            DateTime curTime = calChangeTimeCalendar.SelectionRange.Start;
            txtBirthday.Text = String.Format("{0} {1}", curTime.ToString("MMMM"), AddOrdinal(curTime.Day));
        }

        private void chkBrianMp16_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBrianMp16.Checked == true)
                boolBrianMp16VideosOff = true;
            else
                boolBrianMp16VideosOff = false;
        }
    }
}
