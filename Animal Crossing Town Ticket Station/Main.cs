using ACPasswordLibrary.Core.AnimalCrossing;
using Animal_Crossing_Town_Ticket_Station.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Animal_Crossing_Town_Ticket_Station
{
    public partial class Main : Form
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection();
        Font? fontFink36;
        TimeSpan timeOffset = new TimeSpan(0, 0, 0, 0, 0);
        int intWeather = 0;
        static string filedir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\ACTownTickets\";
        static string filepath = filedir + "SaveData.txt";
        static int intDataLines = 50;
        string[] data = new string[intDataLines];

        public Main()
        {
            Task.Run(() =>
            {
                Span<byte> bytes = new(new byte[28]);
                byte[] str = new byte[8] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
                ACPasswordLibrary.Core.AnimalCrossing.Encoder.MakePassword(ref bytes, CodeType.Magazine, 0, str, str, 0x1000, 0, 0);
            });

            InitializeComponent();
            CreateFonts();

            loadData();
            ChangeCheckboxes();

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

            fontFink36 = new Font(fonts.Families[0], 36.0F);

            btnTasks.Font = fontFink36;
            btnBuy.Font = fontFink36;
            btnCodes.Font = fontFink36;
            btnSettings.Font = fontFink36;
            btnAchievements.Font = fontFink36;
            btnExit.Font = fontFink36;
        }

        private void ChangeCheckboxes()
        {
            chkWeatherClear.Appearance = Appearance.Button;
            chkWeatherClear.Cursor = Cursors.Hand;
            chkWeatherClear.AutoSize = false;
            chkWeatherClear.Size = new Size(64, 64);
            chkWeatherClear.BackgroundImage = Properties.Resources.UI_Weather_SunHalf;
            chkWeatherClear.FlatStyle = FlatStyle.Flat;
            chkWeatherClear.FlatAppearance.BorderSize = 0;
            chkWeatherClear.BackColor = Color.FromArgb(255, 137, 155, 240);
            chkWeatherClear.ForeColor = Color.FromArgb(255, 137, 155, 240);
            if (chkWeatherClear.Checked == true)
                chkWeatherClear.BackgroundImage = Properties.Resources.UI_Weather_SunFull;

            chkWeatherRain.Appearance = Appearance.Button;
            chkWeatherRain.Cursor = Cursors.Hand;
            chkWeatherRain.AutoSize = false;
            chkWeatherRain.Size = new Size(64, 64);
            chkWeatherRain.BackgroundImage = Properties.Resources.UI_Weather_RainHalf;
            chkWeatherRain.FlatStyle = FlatStyle.Flat;
            chkWeatherRain.FlatAppearance.BorderSize = 0;
            chkWeatherRain.BackColor = Color.FromArgb(255, 137, 155, 240);
            chkWeatherRain.ForeColor = Color.FromArgb(255, 137, 155, 240);
            if (chkWeatherRain.Checked == true)
                chkWeatherRain.BackgroundImage = Properties.Resources.UI_Weather_RainFull;

            chkWeatherSnow.Appearance = Appearance.Button;
            chkWeatherSnow.Cursor = Cursors.Hand;
            chkWeatherSnow.AutoSize = false;
            chkWeatherSnow.Size = new Size(64, 64);
            chkWeatherSnow.BackgroundImage = Properties.Resources.UI_Weather_SnowHalf;
            chkWeatherSnow.FlatStyle = FlatStyle.Flat;
            chkWeatherSnow.FlatAppearance.BorderSize = 0;
            chkWeatherSnow.BackColor = Color.FromArgb(255, 137, 155, 240);
            chkWeatherSnow.ForeColor = Color.FromArgb(255, 137, 155, 240);
            if (chkWeatherSnow.Checked == true)
                chkWeatherSnow.BackgroundImage = Properties.Resources.UI_Weather_SnowFull;

            if (chkWeatherClear.Checked == false && chkWeatherRain.Checked == false && chkWeatherSnow.Checked == false)
                chkWeatherClear.Checked = true;
        }

        private void UpdateClock(object sender, EventArgs e)
        {
            GC.Collect();

            UpdateClockImages();

        }

        private void UpdateClockImages()
        {
            if (SaveData.GetSaveData().Item1.Data[1] != "")
                timeOffset = TimeSpan.Parse(SaveData.GetSaveData().Item1.Data[1]);
            else
                timeOffset = TimeSpan.Zero;
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


        }

        private void saveData()
        {
            data = SaveData.GetSaveData().Item1.Data;
            data[4] = intWeather.ToString();

            File.WriteAllLines(filepath, data);
        }

        private void loadData()
        {
            CheckToCreateSaveData();

            data = SaveData.GetSaveData().Item1.Data;
            if (data[1] != "")
                timeOffset = TimeSpan.Parse(data[1]);
            else
                timeOffset = TimeSpan.Zero;
            switch (data[4])
            {
                case "0": chkWeatherClear.Checked = true; intWeather = 0; break;
                case "1": chkWeatherRain.Checked = true; intWeather = 1; break;
                case "2": chkWeatherSnow.Checked = true; intWeather = 2; break;
                default: break;
            }

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


        private void btnExit_Click(object sender, EventArgs e)
        {
            saveData();
            this.Close();
        }

        private void btnTasks_Click(object sender, EventArgs e)
        {
            saveData();
            Tasks frmTasks = new Tasks();
            frmTasks.ShowInTaskbar = false;
            frmTasks.Owner = this;
            frmTasks.Show();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            saveData();
            MyTown frmSettings = new MyTown();
            frmSettings.ShowInTaskbar = false;
            frmSettings.Owner = this;
            frmSettings.Show();
        }

        private void CheckWeather()
        {
            if (chkWeatherClear.Checked == false && chkWeatherRain.Checked == false && chkWeatherSnow.Checked == false)
                chkWeatherClear.Checked = true;
        }

        private void chkWeatherClear_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWeatherClear.Checked == true)
            {
                chkWeatherClear.BackgroundImage = Properties.Resources.UI_Weather_SunFull;
                chkWeatherRain.Checked = false;
                chkWeatherSnow.Checked = false;
                intWeather = 0;
            }
            else
            {
                chkWeatherClear.BackgroundImage = Properties.Resources.UI_Weather_SunHalf;
                CheckWeather();
            }
            saveData();
        }

        private void chkWeatherRain_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWeatherRain.Checked == true)
            {
                chkWeatherRain.BackgroundImage = Properties.Resources.UI_Weather_RainFull;
                chkWeatherClear.Checked = false;
                chkWeatherSnow.Checked = false;
                intWeather = 1;
            }
            else
            {
                chkWeatherRain.BackgroundImage = Properties.Resources.UI_Weather_RainHalf;
                CheckWeather();
            }
            saveData();
        }

        private void chkWeatherSnow_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWeatherSnow.Checked == true)
            {
                chkWeatherSnow.BackgroundImage = Properties.Resources.UI_Weather_SnowFull;
                chkWeatherRain.Checked = false;
                chkWeatherClear.Checked = false;
                intWeather = 2;
            }
            else
            {
                chkWeatherSnow.BackgroundImage = Properties.Resources.UI_Weather_SnowHalf;
                CheckWeather();
            }
            saveData();
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {
            saveData();
            ShopAround frmShop = new ShopAround();
            frmShop.ShowInTaskbar = false;
            frmShop.Owner = this;
            frmShop.Show();
        }

        private void btnAchievements_Click(object sender, EventArgs e)
        {
            saveData();
            Achievements frmAchievements = new Achievements();
            frmAchievements.ShowInTaskbar = false;
            frmAchievements.Owner = this;
            frmAchievements.Show();
        }

        private void btnCodes_Click(object sender, EventArgs e)
        {
            saveData();
            Catalog frmCatalog = new Catalog();
            frmCatalog.ShowInTaskbar = false;
            frmCatalog.Owner = this;
            frmCatalog.Show();
        }
    }
}
