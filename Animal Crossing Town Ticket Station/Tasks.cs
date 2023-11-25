using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.PerformanceData;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Animal_Crossing_Town_Ticket_Station
{
    public partial class Tasks : Form
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection();
        Font? fontFink24;
        Font? fontFink36;
        Font? fontFink38;
        Font? fontFink42;
        Font? fontFink48;
        DateTime loadTimeTasks;
        TimeSpan timeOffset = new TimeSpan(0, 0, 0, 0, 0);
        int intWeather = 0;
        int intNookStore = 0;
        bool hasShovel = false;
        bool hasNet = false;
        bool hasRod = false;
        bool hasAxe = false;
        bool hasIsland = false;
        bool hasEReader = false;
        bool isFirstDay = true;
        bool boolFirstDayTasksAssigned = false;
        bool boolKKSliderTask = false;
        int intTaskRemovePrice = 50;
        int intTasksDiscarded = 0;
        static int intTasksMax = 5;
        static int intTasksTotal = 300;
        static int dataLines = 40;
        int[] intTasks = new int[intTasksMax];
        int[] intTasksLast = new int[intTasksMax];
        int[] intTasksAmount = new int[intTasksMax];
        int[] intTasksComplete = new int[intTasksTotal];
        int intTasksCompleteTotal = 0;
        static string filedir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\ACTownTickets\";
        static string filepath = filedir + "SaveData.txt";
        int intTicketsCurrent = 0;
        int intTicketsTotal = 0;
        int intFishTaskIndex = 0;
        int intBugTaskIndex = 0;
        int intDailyTask = 0;
        int intDailyTaskCount = 0;
        int intTimePlayedDays = 1;
        int intTimePlayedSeconds = 0;
        static int intBoughtUniqueCategories = 18;
        int[] intBoughtUnique = new int[intBoughtUniqueCategories];
        bool boolHasNESGame = false;
        bool boolDiscardTask = false;
        string strPlayerName = "";
        bool boolBirthdaySet = false;
        DateTime birthday;
        bool boolBrianMp16VideosOff = false;
        Random rnd = new Random();

        public Tasks()
        {
            InitializeComponent();
            CreateFonts();

            loadData();
            UpdateClockImages();
            UpdateFormControlColors();
            UpdateTasks();

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

            fontFink24 = new Font(fonts.Families[0], 24.0F);
            fontFink36 = new Font(fonts.Families[0], 36.0F);
            fontFink38 = new Font(fonts.Families[0], 38.0F);
            fontFink42 = new Font(fonts.Families[0], 42.0F);
            fontFink48 = new Font(fonts.Families[0], 48.0F);

            lblTickets.Font = fontFink42;
            lblTickets1.Font = fontFink38;
            lblTickets2.Font = fontFink38;
            lblTickets3.Font = fontFink38;
            lblTickets4.Font = fontFink38;
            lblTickets5.Font = fontFink38;
            lblTask1.Font = fontFink48;
            lblTask2.Font = fontFink48;
            lblTask3.Font = fontFink48;
            lblTask4.Font = fontFink48;
            lblTask5.Font = fontFink48;
            lblTaskDescription.Font = fontFink36;
            btnBack.Font = fontFink24;
        }

        private void UpdateClock(object sender, EventArgs e)
        {
            GC.Collect();

            intTimePlayedSeconds++;
            DateTime now = DateTime.Now + timeOffset;

            if (loadTimeTasks.Day != now.Day && now.Hour > 5 || loadTimeTasks.Day != now.Day && now.Hour < 6 && loadTimeTasks.Hour < 6 ||
                loadTimeTasks.Day == now.Day && loadTimeTasks.Hour <= 5 && now.Hour > 5 || Math.Abs((now - loadTimeTasks).TotalHours) >= 24)
            {
                intTimePlayedDays++;
                loadTimeTasks = now;
                isFirstDay = false;
                boolKKSliderTask = false;
                intDailyTask = 0;
                intFishTaskIndex = 0;
                intBugTaskIndex = 0;
                intTaskRemovePrice = 50;
                for (int i = 0; i < intTasksMax; i++)
                {
                    intTasks[i] = 0;
                    intTasksAmount[i] = 0;
                    intTasksLast[i] = 0;
                }
                SelectDailyTasks(now);
                UpdateTasks();
            }

            if ((int)now.DayOfWeek == 6 && now.Hour > 19 && boolKKSliderTask == false && loadTimeTasks.Day == now.Day && loadTimeTasks.Month == now.Month && loadTimeTasks.Year == now.Year)
            {
                loadTimeTasks = now;
                boolKKSliderTask = true;
                intTasks[intTasksMax - 1] = 109; //KK Slider
                intTasksAmount[intTasksMax - 1] = 0;
                UpdateTasks();
            }

            UpdateFormControlColors();
            UpdateClockImages();
        }

        private void loadData()
        {
            string[] data = SaveData.GetSaveData().Item1.Data;
            string[] strTaskData = new string[intTasksMax];
            string[] strTaskCompleteData = new string[intTasksTotal];

            loadTimeTasks = data[0] != "" ? Convert.ToDateTime(data[0]) : DateTime.Now;
            if (data[1] != "")
                timeOffset = TimeSpan.Parse(data[1]);
            strPlayerName = data[2] == "" ? "Not Set" : data[2];
            intWeather = data[4] != "" ? Convert.ToInt32(data[4]) : 0;
            hasShovel = data[5] != "" ? Convert.ToBoolean(data[5]) : false;
            hasNet = data[6] != "" ? Convert.ToBoolean(data[6]) : false;
            hasRod = data[7] != "" ? Convert.ToBoolean(data[7]) : false;
            hasAxe = data[8] != "" ? Convert.ToBoolean(data[8]) : false;
            hasIsland = data[9] != "" ? Convert.ToBoolean(data[9]) : false;
            hasEReader = data[10] != "" ? Convert.ToBoolean(data[10]) : false;
            switch (data[11])
            {
                case "1": intNookStore = data[11] != "" ? 1 : 1; break;
                case "2": intNookStore = data[11] != "" ? 2 : 1; break;
                case "3": intNookStore = data[11] != "" ? 3 : 1; break;
                case "4": intNookStore = data[11] != "" ? 4 : 1; break;
                case "5": intNookStore = data[11] != "" ? 5 : 1; break;
                default: intNookStore = 1; break;
            }

            intTicketsCurrent = data[12] != "" ? Convert.ToInt32(data[12]) : 0;
            intTicketsTotal = data[13] != "" ? Convert.ToInt32(data[13]) : 0;

            strTaskData = data[14].Split(',');
            for (int i = 0; i < intTasksMax; i++)
                intTasks[i] = data[14] != "" ? Convert.ToInt32(strTaskData[i]) : 0;
            strTaskData = data[15].Split(',');
            for (int i = 0; i < intTasksMax; i++)
                intTasksAmount[i] = data[15] != "" ? Convert.ToInt32(strTaskData[i]) : 0;
            strTaskData = data[16].Split(',');
            for (int i = 0; i < intTasksMax; i++)
                intTasksLast[i] = data[16] != "" ? Convert.ToInt32(strTaskData[i]) : 0;

            intFishTaskIndex = data[17] != "" ? Convert.ToInt32(data[17]) : 0;
            intBugTaskIndex = data[18] != "" ? Convert.ToInt32(data[18]) : 0;
            boolKKSliderTask = data[19] != "" ? Convert.ToBoolean(data[19]) : false;
            isFirstDay = data[20] != "" ? Convert.ToBoolean(data[20]) : true;
            boolFirstDayTasksAssigned = data[21] != "" ? Convert.ToBoolean(data[21]) : false;
            intTasksCompleteTotal = data[22] != "" ? Convert.ToInt32(data[22]) : 0;
            strTaskCompleteData = data[23].Split(',');
            for (int i = 0; i < intTasksTotal; i++)
                intTasksComplete[i] = data[23] != "" ? Convert.ToInt32(strTaskCompleteData[i]) : 0;
            intTaskRemovePrice = data[24] != "" ? Convert.ToInt32(data[24]) : 50;
            intTasksDiscarded = data[25] != "" ? Convert.ToInt32(data[25]) : 0;
            intTimePlayedDays = data[26] != "" ? Convert.ToInt32(data[26]) : 1;
            intTimePlayedSeconds = data[27] != "" ? Convert.ToInt32(data[27]) : 0;
            birthday = data[28] != "" ? Convert.ToDateTime(data[28]) : DateTime.Now;
            strTaskData = data[33].Split(',');
            for (int i = 0; i < intBoughtUniqueCategories; i++)
                intBoughtUnique[i] = data[33] != "" ? Convert.ToInt32(strTaskData[i]) : 0;
            boolHasNESGame = data[38] != "" ? Convert.ToBoolean(data[38]) : false;
            boolBirthdaySet = data[39].ToLower() == "true" ? true : false;
            boolBrianMp16VideosOff = data[42].ToLower() == "true" ? true : false;

            UpdateTasks();
        }

        private void saveData()
        {
            string[] data = SaveData.GetSaveData().Item1.Data;
            string sb = "";

            data[0] = (DateTime.Now + timeOffset).ToString();
            data[12] = intTicketsCurrent.ToString();
            data[13] = intTicketsTotal.ToString();
            sb = "";
            for (int i = 0; i < intTasksMax; i++)
                sb += intTasks[i] + ",";
            data[14] = sb;
            sb = "";
            for (int i = 0; i < intTasksMax; i++)
                sb += intTasksAmount[i] + ",";
            data[15] = sb;
            sb = "";
            for (int i = 0; i < intTasksMax; i++)
                sb += intTasksLast[i] + ",";
            data[16] = sb;
            data[17] = intFishTaskIndex.ToString();
            data[18] = intBugTaskIndex.ToString();
            data[19] = boolKKSliderTask.ToString();
            data[20] = isFirstDay.ToString();
            data[21] = boolFirstDayTasksAssigned.ToString();
            data[22] = intTasksCompleteTotal.ToString();
            sb = "";
            for (int i = 0; i < intTasksTotal; i++)
                sb += intTasksComplete[i] + ",";
            data[23] = sb;
            data[24] = intTaskRemovePrice.ToString();
            data[25] = intTasksDiscarded.ToString();
            data[26] = intTimePlayedDays.ToString();
            data[27] = intTimePlayedSeconds.ToString();

            File.WriteAllLines(filepath, SaveData.GetSaveData().Item1.Data);
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

        }


        private void UpdateFormControlColors()
        {
            switch (intWeather)
            {
                case 0: break;
                case 1: this.BackgroundImage = Properties.Resources.UI_BG_TasksRain; break;
                case 2: this.BackgroundImage = Properties.Resources.UI_BG_TasksSnow; break;
                default: break;
            }

            DateTime now = DateTime.Now + timeOffset;
            int intMinutes = (int)now.TimeOfDay.TotalMinutes;
            int intColor = 0;

            if (now.Hour < 5 || now.Hour > 20 || now.Hour == 20 && now.Minute >= 16)
                intColor = 0;
            else if (now.Hour >= 5 && now.Hour < 9 || now.Hour == 9 & now.Minute < 16)
                intColor = (now.Hour - 5) * 60 + now.Minute;
            else if (now.Hour >= 16 && now.Hour < 20 || now.Hour == 20 && now.Minute < 16)
                intColor = 255 - (now.Hour - 16) * 60 - now.Minute;
            else
                intColor = 255;

            this.BackColor = Color.FromArgb(255, intColor, intColor, intColor);
            lblTickets1.ForeColor = Color.FromArgb(255, 178 + intColor / 10, 168 + intColor / 10, 157 + intColor / 10);
            lblTickets2.ForeColor = Color.FromArgb(255, 178 + intColor / 10, 168 + intColor / 10, 157 + intColor / 10);
            lblTickets3.ForeColor = Color.FromArgb(255, 178 + intColor / 10, 168 + intColor / 10, 157 + intColor / 10);
            lblTickets4.ForeColor = Color.FromArgb(255, 178 + intColor / 10, 168 + intColor / 10, 157 + intColor / 10);
            lblTickets5.ForeColor = Color.FromArgb(255, 178 + intColor / 10, 168 + intColor / 10, 157 + intColor / 10);

            if (now.Hour < 6 || now.Hour > 19 || now.Hour == 19 && now.Minute >= 30)
                lblTaskDescription.ForeColor = Color.FromArgb(255, 210, 255, 255);
            else
                lblTaskDescription.ForeColor = Color.FromArgb(255, 0, 0, 255);
        }

        private void TaskComplete(int intTask, string strTaskTag, int intTaskValue, int intTickets)
        {
            intTasks[intTask] = 0;
            intTasksAmount[intTask] = 0;
            intTicketsCurrent += intTickets;
            intTicketsTotal += intTickets;
            intTasksComplete[intTaskValue]++;
            intTasksCompleteTotal++;
            switch (strTaskTag)
            {
                case "Fish": intTasksComplete[intFishTaskIndex + 200]++; intFishTaskIndex = 0; break;
                case "Bug": intTasksComplete[intBugTaskIndex + 250]++; intBugTaskIndex = 0; break;
                default: break;
            }

            for (int i = 0; i < intTasksMax; i++)
            {
                if (i > 0)
                    intTasksLast[i - 1] = intTasksLast[i];
                if (intTasksLast[i] == 0 || i == intTasksMax - 1)
                    intTasksLast[i] = intTaskValue;
            }
            UpdateTasks();

        }

        private void UpdateDailyTask()
        {
            if (intDailyTask > 0)
            {
                intTasks[intDailyTaskCount] = intDailyTask;
                intTasksAmount[intDailyTaskCount] = 0;
                intDailyTask = 0;
                intDailyTaskCount++;
            }
        }

        private void UpdateTasks()
        {
            if (boolFirstDayTasksAssigned == false)
            {
                SelectDailyTasks(DateTime.Now + timeOffset);
                boolFirstDayTasksAssigned = true;
            }

            for (int i = 0; i < intTasksMax; i++)
            {
                if (intTasks[i] == 0 && i < intTasksMax - 1)
                { //Shifts Tasks over
                    intTasks[i] = intTasks[i + 1];
                    intTasks[i + 1] = 0;
                    intTasksAmount[i] = intTasksAmount[i + 1];
                    intTasksAmount[i + 1] = 0;
                }
                if (intTasks[i] == 0)
                    intTasks[i] = SelectTaskRecurring();

                Tuple<TaskInfo, int> taskCodeData = TaskData.GetTaskCheckByIndex(intTasks[i]);
                Tuple<TortInfo, int> tortCodeData = TortData.GetTortCheckByIndex(intTasks[i]);
                Image imgTask = null;

                if (taskCodeData.Item1 != null) //Updates Image
                    imgTask = taskCodeData.Item1.ImageName;
                if (tortCodeData.Item1 != null)
                    imgTask = tortCodeData.Item1.ImageName;
                switch (intTasks[i])
                {
                    case 90: imgTask = Properties.Resources.TK_Icon_Daily_Flowers; break;
                    case 91: imgTask = Properties.Resources.TK_Icon_Daily_Fossil; break;
                    case 92: imgTask = Properties.Resources.TK_Icon_Daily_MoneyRock; break;
                    case 93: imgTask = Properties.Resources.TK_Icon_Daily_ShinySpot; break;
                    case 94: imgTask = Properties.Resources.TK_Icon_Daily_Weeds; break;
                    case 95: imgTask = Properties.Resources.TK_Icon_Daily_Greeting; break;
                    case 99: imgTask = Properties.Resources.TK_Icon_Birthday; break;
                    case 100: imgTask = Properties.Resources.TK_Icon_Hol_FishingTourney; break;
                    case 101: imgTask = Properties.Resources.TK_Icon_Hol_Halloween; break;
                    case 102: imgTask = Properties.Resources.TK_Icon_Hol_Thanksgiving; break;
                    case 103: imgTask = Properties.Resources.TK_Icon_Hol_SaleDay; break;
                    case 104: imgTask = Properties.Resources.TK_Icon_Hol_Christmas; break;
                    case 105: imgTask = Properties.Resources.TK_Icon_Hol_Mushroom; break;
                    case 106: imgTask = Properties.Resources.TK_Icon_Hol_Snowman; break;
                    case 107: imgTask = Properties.Resources.TK_Icon_Hol_NookLottery; break;
                    case 108: imgTask = Properties.Resources.TK_Icon_Hol_Joan; break;
                    case 109: imgTask = Properties.Resources.TK_Icon_Hol_KKSlider; break;
                    default: break;
                }

                switch (i)
                {
                    case 0:
                        lblTask1.Location = btnTask1.PointToClient(lblTask1.Parent.PointToScreen(lblTask1.Location));
                        btnTask1.BackgroundImage = imgTask;
                        lblTask1.Parent = btnTask1;
                        break;
                    case 1:
                        lblTask2.Location = btnTask2.PointToClient(lblTask2.Parent.PointToScreen(lblTask2.Location));
                        btnTask2.BackgroundImage = imgTask;
                        lblTask2.Parent = btnTask2;
                        break;
                    case 2:
                        lblTask3.Location = btnTask3.PointToClient(lblTask3.Parent.PointToScreen(lblTask3.Location));
                        btnTask3.BackgroundImage = imgTask;
                        lblTask3.Parent = btnTask3;
                        break;
                    case 3:
                        lblTask4.Location = btnTask4.PointToClient(lblTask4.Parent.PointToScreen(lblTask4.Location));
                        btnTask4.BackgroundImage = imgTask;
                        lblTask4.Parent = btnTask4;
                        break;
                    case 4:
                        lblTask5.Location = btnTask5.PointToClient(lblTask5.Parent.PointToScreen(lblTask5.Location));
                        btnTask5.BackgroundImage = imgTask;
                        lblTask5.Parent = btnTask5;
                        break;
                    default: break;
                }
            }
            UpdateTaskLabels();
            lblTickets.Text = intTicketsCurrent.ToString("N0");
            saveData();
        }

        private void SelectDailyTasks(DateTime now)
        {
            intDailyTaskCount = 0;
            intDailyTask = 0;

            CheckBirthdayTime(now);
            CheckTortimerTime(now);
            if (intTasks[0] != 0 && intTasks[0] != 121 && intTasks[0] != 122) { CheckOtherTortimerTime(now); }//Meteor Shower and Founder's Day
            CheckHolidayTime(now); //Fishing Tourney Chip, //Halloween Jack, Thanksgiving Franklin, Sale Day Grab Bags, Christmas Jingle
            CheckLotteryTime(now);
            CheckMushroomTime(now);
            CheckSnowballTime(now);
            CheckJoanTime(now);
            CheckKKSliderTime(now);

            int intDailyTaskAttempts = 0;
            while (intDailyTaskCount < 3 && intDailyTaskAttempts < 100)
            {
                intDailyTask = GetRandomDailyTask(now);//Select random numbers from TaskDailyInfo List
                bool boolDailyTaskPass = true;
                for (int i = 0; i < intTasksMax; i++)
                    if (intDailyTask == intTasks[i])
                        boolDailyTaskPass = false;
                if (boolDailyTaskPass == true)
                    UpdateDailyTask();
                intDailyTaskAttempts++;
            }
        }

        private int SelectTaskRecurring()
        {
            int intTaskValue = 0;
            bool boolPass = false;
            string strTaskName = "";

            while (boolPass == false)
            {
                boolPass = true;
                bool boolTaskChange = false;
                Tuple<TaskInfo, int> taskCodeData = new Tuple<TaskInfo, int>(null, 0);
                DateTime now = DateTime.Now + timeOffset;
                if (strTaskName == "")
                    taskCodeData = TaskData.GetTaskCheckByRNGValue(rnd.Next(1, 8193));
                else
                    taskCodeData = TaskData.GetTaskCheckByName(strTaskName);

                intTaskValue = taskCodeData.Item1.Index;
                switch (taskCodeData.Item1.Tag)
                {
                    case "Igloo": if (TaskCheckTimeIgloo(now) == false) { boolTaskChange = true; } break;
                    case "SummerCamper": if (TaskCheckTimeCamper(now) == false) { boolTaskChange = true; } break;
                    case "Fish5": case "Trash": case "FishCliff": if (hasRod == false) { boolTaskChange = true; } break;
                    case "Bug5": case "BugClap": if (hasNet == false) { boolTaskChange = true; } break;
                    case "HouseNES": if (boolHasNESGame == false || intNookStore > 4) { boolTaskChange = true; } break;
                    case "HouseKKSong": if (intTasksComplete[109] < 1) { boolTaskChange = true; } break;
                    case "HouseGyroid2": case "TreePlant": case "VillagerPitfall": if (hasShovel == false) { boolTaskChange = true; } break;
                    case "VillagerRoof": if (intNookStore < 2 || intNookStore > 4) { boolTaskChange = true; } break;
                    case "TreeChop": if (hasAxe == false) { boolTaskChange = true; } break;
                    case "TownSign": if (intNookStore < 3 || intNookStore > 4) { boolTaskChange = true; } break;
                    case "TownPerfect": if (hasShovel == false || hasAxe == false || intTasksComplete[31] < 7) { boolTaskChange = true; } break;
                    case "IslandFish": if (hasIsland == false || hasRod == false) { boolTaskChange = true; } break;
                    case "IslandBug": if (hasIsland == false || hasNet == false) { boolTaskChange = true; } break;
                    case "IslandHouse": case "IslandCoconut": case "IslandFlag": if (hasIsland == false) { boolTaskChange = true; } break;
                    case "NookBells":
                    case "NookCatalog":
                    case "NookShells":
                    case "NookShirt":
                    case "NookFurn":
                    case "NookCarpet":
                    case "NookWallpaper":
                        if (TaskCheckTimeNook(now, true, false, isFirstDay) == false) { boolTaskChange = true; }
                        break;
                    case "TicketsCode": if (TaskCheckTimeNook(now, true, false, isFirstDay) == false || intTasksComplete[48] < 3) { boolTaskChange = true; } break;
                    case "IslandNote": case "IslandItem": if (TaskCheckTimeIslander(now) == false || hasIsland == false) { boolTaskChange = true; } break;
                    case "Ereader": if (hasEReader == false) { boolTaskChange = true; } break;
                    case "ShirtPattern": case "CharacterAbles": if (TaskCheckTimeAbles(now) == false) { boolTaskChange = true; } break;
                    case "Fish":
                        if (hasRod == true)
                        {
                            Tuple<FishInfo, int> fishChoose = new Tuple<FishInfo, int>(null, 0);
                            if (intFishTaskIndex == 0)
                                fishChoose = GetRandomFish(now);
                            else
                                fishChoose = FishData.GetFishCheckByIndex(intFishTaskIndex);
                            UpdateFishData(taskCodeData, fishChoose);
                        }
                        else
                            boolTaskChange = true;
                        break;
                    case "Bug":
                        if (hasNet == true)
                        {
                            Tuple<BugsInfo, int> bugChoose = new Tuple<BugsInfo, int>(null, 0);
                            if (intBugTaskIndex == 0)
                                bugChoose = GetRandomBug(now);
                            else
                                bugChoose = BugsData.GetBugCheckByIndex(intBugTaskIndex);
                            UpdateBugsData(taskCodeData, bugChoose);
                        }
                        else
                            boolTaskChange = true;
                        break;
                    case "TicketsComment": if (intTasksCompleteTotal < 50 || intTasksComplete[49] > 100 || boolBrianMp16VideosOff == true || rnd.Next(1, 5) > 2) { boolTaskChange = true; } break;
                    case "TicketsLike": if (intTasksCompleteTotal < 50 || intTasksComplete[50] > 100 || boolBrianMp16VideosOff == true || rnd.Next(1, 5) > 2) { boolTaskChange = true; } break;
                    case "TicketsFree": if (intTasksCompleteTotal < 100) { boolTaskChange = true; } break;
                }

                if (boolTaskChange == true)
                {
                    strTaskName = taskCodeData.Item1.OtherTask;
                    boolPass = false;
                }

                for (int i = 0; i < intTasksMax; i++) //Check if Task already exists on the task list
                    if (intTaskValue == intTasks[i] || intTaskValue == intTasksLast[i])
                    {
                        strTaskName = "";
                        boolPass = false;
                    }
            }

            return intTaskValue;
        }

        private void UpdateFishData(Tuple<TaskInfo, int> taskCodeData, Tuple<FishInfo, int> fishData)
        {
            taskCodeData = TaskData.GetTaskCheckByTag("Fish");
            taskCodeData.Item1.Name = fishData.Item1.Name;
            taskCodeData.Item1.Tickets = fishData.Item1.Tickets;
            intFishTaskIndex = fishData.Item1.Index;
        }

        private void UpdateBugsData(Tuple<TaskInfo, int> taskCodeData, Tuple<BugsInfo, int> bugsData)
        {
            taskCodeData = TaskData.GetTaskCheckByTag("Bug");
            taskCodeData.Item1.Name = bugsData.Item1.Name;
            taskCodeData.Item1.Tickets = bugsData.Item1.Tickets;
            intBugTaskIndex = bugsData.Item1.Index;
        }

        private Tuple<FishInfo, int> GetRandomFish(DateTime now)
        {
            while (true)
            {
                int rng = rnd.Next(1, 41);
                Tuple<FishInfo, int> fishCodeData = FishData.GetFishCheckByIndex(rng);
                try
                {
                    if (IsWithinTime(fishCodeData.Item1.MonthArray, fishCodeData.Item1.HourArray, null, fishCodeData.Item1.WeatherArray, now) == true)
                        return fishCodeData;
                }
                catch (Exception e) { MessageBox.Show(e.Message + " " + rng); }
            }

        }
        private Tuple<BugsInfo, int> GetRandomBug(DateTime now)
        {
            while (true)
            {
                Tuple<BugsInfo, int> bugsCodeData = BugsData.GetBugCheckByIndex(rnd.Next(1, 41));
                try
                {
                    if (bugsCodeData.Item1.Name == "Ant" && TaskCheckTimeNook(now, true, false, isFirstDay) == true && intWeather == 0)
                        return bugsCodeData;
                    if (IsWithinTime(bugsCodeData.Item1.MonthArray, bugsCodeData.Item1.HourArray, null, bugsCodeData.Item1.WeatherArray, now) == true)
                        return bugsCodeData;
                }
                catch (Exception e) { MessageBox.Show(e.Message); }
            }

        }

        private int GetRandomDailyTask(DateTime now)
        {
            //0 - 10 = Flowers, 11 - 17 = Fossils, 18 - 23 = Money Rock, 24 - 28 = Golden Spot, 29 - 38 = Weeds, 39 - 49 = Greet Villagers
            int rndDailyTask = rnd.Next(0, 50);
            if (rndDailyTask < 11 && TaskCheckTimeNook(now, true, false, false) == true && CheckSaleDay(now) == false && CheckCandyTime(now) == false)
                intDailyTask = 90; //Plant 2 Flowers
            else if (rndDailyTask < 18 && CheckShovelAccept(now) == true)
                intDailyTask = 91; //Dig up 3 Fossils
            else if (rndDailyTask < 24 && CheckShovelAccept(now) == true)
                intDailyTask = 92; //Money Rock
            else if (rndDailyTask < 29 && CheckShovelAccept(now) == true)
                intDailyTask = 93; //Golden Spot
            else if (rndDailyTask < 39 && hasShovel == true)
                intDailyTask = 94; //Pick Weeds
            else if (rndDailyTask < 50 && now.Hour > 8 && now.Hour < 22 || intNookStore > 2 && intNookStore < 5 && now.Hour < 4 ||
                intNookStore > 2 && intNookStore < 5 && now.Hour > 6)
                intDailyTask = 95; //Greet 2 Villagers
            else
                intDailyTask = 0;

            return intDailyTask;
        }

        private bool IsWithinTime(int?[,] MonthArray, int?[,] HourArray, int?[,] WeekdayArray, int?[,] WeatherArray, DateTime now)
        {
            //MonthArray contains 12x2 entries indicating which days within month work { {0,0} ... {0,0} }
            //Col1: 0-31 specifies which day for each month; Col2: 0=none, 1='<=', 2='>', 3='=', 4="="-1 if leap year, 5='=' & next 2 days.
            //HourArray contains 24x2 entries indicating which minutes within the hour work { {0,0} ... {0,0} }
            //Col1: 0-59 specifies which minute; Col2: 0=none, 1='<=', 2='>'
            //WeekdayArray contains 7x2 entries indicating which days within week work
            //Col1: 0-7 specifies which weekday; Col2: 0=none, 1='=', 2='='+1 day
            //WeatherArray contains 1x2 entries indicating which weather conditions are acceptable
            //Col1: 0=clear, 1=rain, 2=snow; Col2: 0=pass, 1='=', 2='!='

            bool boolPass = true; //start true and see if any conditions fail the pass
            if (MonthArray != null && boolPass == true)
            {
                switch (MonthArray[now.Month - 1, 1])
                {
                    case 0: boolPass = false; break;
                    case 1: if (now.Day > MonthArray[now.Month - 1, 0]) { boolPass = false; } break;
                    case 2: if (now.Day <= MonthArray[now.Month - 1, 0]) { boolPass = false; } break;
                    case 3: if (now.Day != MonthArray[now.Month - 1, 0]) { boolPass = false; } break;
                    case 4: if ((DateTime.IsLeapYear(now.Year) ? now.Day - 1 : now.Day) != MonthArray[now.Month - 1, 0]) { boolPass = false; } break;
                    case 5: if (now.Day < MonthArray[now.Month - 1, 0] || now.Day > MonthArray[now.Month - 1, 0] + 2) { boolPass = false; } break;
                    case 6: if (now.Day > 7 || (int)now.DayOfWeek != WeekdayArray[0, 0] + WeekdayArray[0, 1] - 1) { boolPass = false; } break;
                    case 7: if (now.Day < 8 || now.Day > 14 || (int)now.DayOfWeek != WeekdayArray[0, 0] + WeekdayArray[0, 1] - 1) { boolPass = false; } break;
                    case 8: if (now.Day < 15 || now.Day > 21 || (int)now.DayOfWeek != WeekdayArray[0, 0] + WeekdayArray[0, 1] - 1) { boolPass = false; } break;
                    case 9: if (now.Day < 21 || now.Day > 28 || (int)now.DayOfWeek != WeekdayArray[0, 0] + WeekdayArray[0, 1] - 1) { boolPass = false; } break;
                    case 10: if (CheckMoonTime(now) == false) { boolPass = false; } break;
                    default: boolPass = false; break;
                }
            }

            if (HourArray != null && boolPass == true)
            {
                switch (HourArray[now.Hour, 1])
                {
                    case 0: boolPass = false; break;
                    case 1: if (now.Minute > HourArray[now.Hour, 0] || isFirstDay == true && now.Minute > HourArray[now.Hour, 0] - 25 && hasShovel == false) { boolPass = false; } break;
                    case 2: if (now.Minute <= HourArray[now.Hour, 0]) { boolPass = false; } break;
                    default: boolPass = false; break;
                }
            }

            if (WeekdayArray != null && boolPass == true)
            {
                switch (WeekdayArray[0, 1])
                {
                    case 0: break;
                    case 1: if ((int)now.DayOfWeek != WeekdayArray[0, 0]) { boolPass = false; } break;
                    case 2: if ((int)now.DayOfWeek == WeekdayArray[0, 0]) { boolPass = false; } break;
                    default: boolPass = false; break;
                }
            }

            if (WeatherArray != null && boolPass == true)
            {
                switch (WeatherArray[0, 1])
                {
                    case 0: break;
                    case 1: if (intWeather != WeatherArray[0, 0]) { boolPass = false; } break;
                    case 2: if (intWeather == WeatherArray[0, 0]) { boolPass = false; } break;
                    default: boolPass = false; break;
                }
            }

            return boolPass;
        }

        private void CheckHolidayTime(DateTime now)
        {
            if (now.Month == 6 && (int)now.DayOfWeek == 0 && now.Hour < 17 || now.Month == 11 && (int)now.DayOfWeek == 0 && now.Hour < 17 ||
                now.Month == 6 && (int)now.DayOfWeek == 0 && now.Hour == 17 & now.Minute < 56 && isFirstDay == false ||
                now.Month == 6 && (int)now.DayOfWeek == 0 && now.Hour == 17 && now.Minute < 31 && isFirstDay == true ||
                now.Month == 11 && (int)now.DayOfWeek == 0 && now.Hour == 17 && now.Minute < 56 && isFirstDay == false ||
                now.Month == 11 && (int)now.DayOfWeek == 0 && now.Hour == 17 && now.Minute < 31 && isFirstDay == true)
                if (now.Hour > 5)
                    intDailyTask = 100; //Fishing Tourney
            if (now.Month == 10 && now.Day == 31 && now.Hour > 5 || now.Month == 11 && now.Day == 1 && now.Hour < 1)
                    intDailyTask = 101; //Halloween
            if (now.Month == 11 && now.Day >= 22 && now.Day < 29 && (int)now.DayOfWeek == 4 && now.Hour < 20 ||
                now.Month == 11 && now.Day >= 22 && now.Day < 29 && (int)now.DayOfWeek == 4 && now.Hour == 20 && now.Minute < 56 && isFirstDay == false ||
                now.Month == 11 && now.Day >= 22 && now.Day < 29 && (int)now.DayOfWeek == 4 && now.Hour == 20 && now.Minute < 31 && isFirstDay == true)
                if (now.Hour > 5)
                    intDailyTask = 102; //Thanksgiving
            if (now.Month == 11 && now.Day >= 23 && now.Day < 30 && (int)now.DayOfWeek == 5 && now.Hour < 21 ||
                now.Month == 11 && now.Day >= 23 && now.Day < 30 && (int)now.DayOfWeek == 5 && now.Hour == 21 && now.Minute < 56 && isFirstDay == false ||
                now.Month == 11 && now.Day >= 23 && now.Day < 30 && (int)now.DayOfWeek == 5 && now.Hour == 21 && now.Minute < 31 && isFirstDay == true)
                if (now.Hour > 5)
                    intDailyTask = 103; //Sale Day
            if (now.Month == 12 && now.Day == 24 && now.Hour > 5 || now.Month == 12 && now.Day == 25 && now.Hour < 1)
                    intDailyTask = 104; //Christmas

            UpdateDailyTask();
        }

        private void CheckBirthdayTime(DateTime now)
        {
            if (now.Month == birthday.Month && now.Day == birthday.Day && boolBirthdaySet == true && intTimePlayedDays > 1)
                intDailyTask = 99; // Birthday
            UpdateDailyTask();
        }

        private void CheckTortimerTime(DateTime now)
        {
            for (int i = 110; i < TortData.GetTortInfoArrayLength() + 110; i++)
            {
                Tuple<TortInfo, int> tortCodeData = TortData.GetTortCheckByIndex(i);
                if (i == 133 && now.Month == 12 && now.Day == 31 && now.Hour > 5 || i == 133 && now.Month == 1 && now.Day == 1 && now.Hour < 1)
                    intDailyTask = tortCodeData.Item1.Index; // New Years Eve
                else
                    if (IsWithinTime(tortCodeData.Item1.MonthArray, tortCodeData.Item1.HourArray, tortCodeData.Item1.WeekdayArray, tortCodeData.Item1.WeatherArray, now) == true)
                        if (now.Hour > 5)
                            intDailyTask = tortCodeData.Item1.Index;
            }
            UpdateDailyTask();
        }

        private void CheckOtherTortimerTime(DateTime now)
        {
            for (int i = 121; i < 123; i++) //Meteor Shower and Founder's Day Task Indices
            {
                Tuple<TortInfo, int> tortCodeData = TortData.GetTortCheckByIndex(i);
                if (IsWithinTime(tortCodeData.Item1.MonthArray, tortCodeData.Item1.HourArray, tortCodeData.Item1.WeekdayArray, tortCodeData.Item1.WeatherArray, now) == true)
                    if (now.Hour > 5)
                        intDailyTask = tortCodeData.Item1.Index;
            }
            UpdateDailyTask();
        }

        private void CheckMushroomTime(DateTime now)
        {
            if (now.Month == 10 && now.Day >= 15 && now.Day <= 25 && now.Hour < 8 ||
                now.Month == 10 && now.Day >= 15 && now.Day <= 25 && now.Hour == 8 && now.Minute < 45 && isFirstDay == false ||
                now.Month == 10 && now.Day >= 15 && now.Day <= 25 && now.Hour == 8 && now.Minute < 15 && isFirstDay == true)
                //if (rnd.Next(0, 100) < 80)
                if (now.Hour > 5)
                    intDailyTask = 105; //Mushroom Season
            UpdateDailyTask();
        }

        private void CheckSnowballTime(DateTime now)
        {
            if (now.Month == 12 && now.Day > 24 || now.Month == 1 || now.Month == 2 && now.Day < 25)
                if (rnd.Next(0, 100) < 80)
                    intDailyTask = 106; //Snowball Season
            UpdateDailyTask();
        }

        private void CheckLotteryTime(DateTime now)
        {
            if (now.Day == DateTime.DaysInMonth(now.Year, now.Month) && TaskCheckTimeNook(now, false, true, isFirstDay) == true && isFirstDay == false)
                intDailyTask = 107; //Nook Lottery at End of Month
            UpdateDailyTask();
        }

        private void CheckJoanTime(DateTime now)
        {
            if ((int)now.DayOfWeek == 0 && now.Hour < 11 || (int)now.DayOfWeek == 0 && now.Hour == 11 && now.Minute < 56 && isFirstDay == false ||
                (int)now.DayOfWeek == 0 && now.Hour == 11 && now.Minute < 31 && isFirstDay == true)
                //if (rnd.Next(0, 100) < 75)
                if (now.Hour > 5)
                    intDailyTask = 108; //Joan Turnips
            UpdateDailyTask();
        }


        private void CheckKKSliderTime(DateTime now)
        {
            if ((int)now.DayOfWeek == 6 && now.Hour >= 20 && now.Hour < 23 && boolKKSliderTask == false ||
                (int)now.DayOfWeek == 6 && now.Hour == 23 && now.Minute < 59 && boolKKSliderTask == false)
            {
                boolKKSliderTask = true;
                intDailyTask = 109;
            }
            UpdateDailyTask();
        }

        private bool CheckMoonTime(DateTime now)
        {
            bool boolMoonPass = false;
            int intMoonMonth = 0;
            int intMoonDay = 0;

            switch (now.Year)
            {
                case 2000: intMoonMonth = 9; intMoonDay = 12; break;
                case 2001: intMoonMonth = 10; intMoonDay = 1; break;
                case 2002: intMoonMonth = 9; intMoonDay = 21; break;
                case 2003: intMoonMonth = 9; intMoonDay = 11; break;
                case 2004: intMoonMonth = 9; intMoonDay = 28; break;
                case 2005: intMoonMonth = 9; intMoonDay = 18; break;
                case 2006: intMoonMonth = 10; intMoonDay = 6; break;
                case 2007: intMoonMonth = 9; intMoonDay = 25; break;
                case 2008: intMoonMonth = 9; intMoonDay = 14; break;
                case 2009: intMoonMonth = 10; intMoonDay = 3; break;
                case 2010: intMoonMonth = 9; intMoonDay = 22; break;
                case 2011: intMoonMonth = 9; intMoonDay = 12; break;
                case 2012: intMoonMonth = 9; intMoonDay = 30; break;
                case 2013: intMoonMonth = 9; intMoonDay = 19; break;
                case 2014: intMoonMonth = 9; intMoonDay = 9; break;
                case 2015: intMoonMonth = 9; intMoonDay = 27; break;
                case 2016: intMoonMonth = 9; intMoonDay = 15; break;
                case 2017: intMoonMonth = 10; intMoonDay = 4; break;
                case 2018: intMoonMonth = 9; intMoonDay = 24; break;
                case 2019: intMoonMonth = 9; intMoonDay = 13; break;
                case 2020: intMoonMonth = 10; intMoonDay = 1; break;
                case 2021: intMoonMonth = 9; intMoonDay = 21; break;
                case 2022: intMoonMonth = 9; intMoonDay = 10; break;
                case 2023: intMoonMonth = 9; intMoonDay = 29; break;
                case 2024: intMoonMonth = 9; intMoonDay = 17; break;
                case 2025: intMoonMonth = 10; intMoonDay = 6; break;
                case 2026: intMoonMonth = 9; intMoonDay = 25; break;
                case 2027: intMoonMonth = 9; intMoonDay = 15; break;
                case 2028: intMoonMonth = 10; intMoonDay = 3; break;
                case 2029: intMoonMonth = 9; intMoonDay = 22; break;
                case 2030: intMoonMonth = 9; intMoonDay = 12; break;
                case 2031: intMoonMonth = 10; intMoonDay = 1; break;
                case 2032: intMoonMonth = 9; intMoonDay = 19; break;
                default: break;
            }
            if (now.Month == intMoonMonth && now.Day == intMoonDay)
                boolMoonPass = true;

            return boolMoonPass;
        }

        private bool CheckSaleDay(DateTime now)
        {
            if (now.Month == 11 && now.Day >= 23 && now.Day < 30 && (int)now.DayOfWeek == 5 && TaskCheckTimeNook(now, false, false, isFirstDay) == true)
                return true;
            return false;
        }

        private bool CheckCandyTime(DateTime now)
        {
            if (now.Month == 10 && now.Day >= 15 && now.Day < 31 && TaskCheckTimeNook(now, true, false, isFirstDay) == true)
                return true;
            return false;
        }

        private bool CheckShovelAccept(DateTime now)
        {
            if (hasShovel == true || TaskCheckTimeNook(now, true, false, isFirstDay) == true && CheckSaleDay(now) == false)
                return true;
            return false;
        }

        private bool TaskCheckTimeNook(DateTime now, bool checkMorningHours, bool isRaffle, bool firstDay)
        {
            int[] intTime = SettingsGetNookHours();
            if (now.Hour < intTime[0] && checkMorningHours == true && firstDay == false || now.Hour < intTime[0] - 1 && checkMorningHours == true && firstDay == true ||
                now.Hour == intTime[0] - 1 && now.Minute < 44 && checkMorningHours == true && firstDay == true && hasShovel == false ||
                now.Hour >= intTime[1] || now.Hour == intTime[1] - 1 && now.Minute > 55 ||
                now.Hour == intTime[1] - 1 && now.Minute > 30 && firstDay == true && hasShovel == false || now.Hour < 6)
                return false;
            if (isRaffle == false & now.Day == DateTime.DaysInMonth(now.Year, now.Month))
                return false;
            return true;
        }

        private bool TaskCheckTimeAbles(DateTime now)
        {
            if (now.Hour < 7 && now.Hour >= 2 || now.Hour == 1 && now.Minute > 55)
                return false;
            return true;
        }

        private bool TaskCheckTimeIslander(DateTime now)
        {
            if (now.Hour < 10 || now.Hour >= 21 || now.Hour == 20 && now.Minute > 55)
                return false;
            return true;
        }

        private bool TaskCheckTimeIgloo(DateTime now)
        {
            if (now.Month > 2 || now.Month == 1 && now.Day == 1 || now.Month == 2 && now.Day > 24)
                return false;
            if (now.Hour < 10 || now.Hour >= 22 || now.Hour == 21 && now.Minute > 55 || now.Hour == 21 && now.Minute > 30 && isFirstDay == true && hasShovel == false)
                return false;
            return true;
        }

        private bool TaskCheckTimeCamper(DateTime now)
        {
            if (now.Month < 6 || now.Month > 8 || (int)now.DayOfWeek == 1 || (int)now.DayOfWeek == 2 || (int)now.DayOfWeek == 3 ||
                (int)now.DayOfWeek == 4 || (int)now.DayOfWeek == 5)
                return false;
            if ((int)now.DayOfWeek == 6 && now.Hour < 9 || (int)now.DayOfWeek == 0 && now.Hour >= 15 || (int)now.DayOfWeek == 0 && now.Hour == 14 && now.Minute > 55 ||
                (int)now.DayOfWeek == 0 && now.Hour == 14 && now.Minute > 30 && isFirstDay == true && hasShovel == false)
                return false;
            return true;
        }

        private int[] SettingsGetNookHours()
        {
            int[] intHours = new int[2];

            switch (intNookStore)
            {
                case 1: intHours[0] = 9; intHours[1] = 22; break;
                case 2: intHours[0] = 7; intHours[1] = 23; break;
                case 3: intHours[0] = 9; intHours[1] = 22; break;
                case 4: intHours[0] = 9; intHours[1] = 22; break;
                case 5: intHours[0] = 0; intHours[1] = 0; break;
            }

            return intHours;
        }


        private void UpdateTaskLabels()
        {
            for (int i = 0; i < intTasksMax; i++)
            {
                int intQuantity = 0;
                int intTickets = 0;

                Tuple<TaskInfo, int> taskCodeData = TaskData.GetTaskCheckByIndex(intTasks[i]);
                if (taskCodeData.Item1 != null)
                {
                    intQuantity = taskCodeData.Item1.Quantity;
                    intTickets = taskCodeData.Item1.Tickets;
                    switch (taskCodeData.Item1.Tag)
                    {
                        case "Fish": UpdateFishData(taskCodeData, FishData.GetFishCheckByIndex(intFishTaskIndex)); break;
                        case "Bug": UpdateBugsData(taskCodeData, BugsData.GetBugCheckByIndex(intBugTaskIndex)); break;
                        default: break;
                    }
                }
                Tuple<TortInfo, int> tortCodeData = TortData.GetTortCheckByIndex(intTasks[i]);
                if (tortCodeData.Item1 != null)
                {
                    intQuantity = tortCodeData.Item1.Quantity;
                    intTickets = tortCodeData.Item1.Tickets;
                }

                switch (intTasks[i])
                {
                    case 90: intQuantity = 2; intTickets = 200; break;
                    case 91: intQuantity = 3; intTickets = 250; break;
                    case 92: intQuantity = 1; intTickets = 200; break;
                    case 93: intQuantity = 1; intTickets = 200; break;
                    case 94: intQuantity = 5; intTickets = 150; break;
                    case 95: intQuantity = 3; intTickets = 250; break;
                    case 99: intQuantity = 1; intTickets = 5000; break;
                    case 100: intQuantity = 1; intTickets = 400; break;
                    case 101: intQuantity = 3; intTickets = 500; break;
                    case 102: intQuantity = 3; intTickets = 500; break;
                    case 103: intQuantity = 3; intTickets = 400; break;
                    case 104: intQuantity = 3; intTickets = 500; break;
                    case 105: intQuantity = 3; intTickets = 400; break;
                    case 106: intQuantity = 1; intTickets = 300; break;
                    case 107: intQuantity = 1; intTickets = 450; break;
                    case 108: intQuantity = 1; intTickets = 300; break;
                    case 109: intQuantity = 1; intTickets = 500; break;
                    default: break;
                }


                string strLabelQuantity = String.Format("{0}/{1}", intTasksAmount[i], intQuantity);
                switch (i)
                {
                    case 0: lblTask1.Text = strLabelQuantity; lblTickets1.Text = String.Format("{0}", intTickets == 100000 ? "100K" : intTickets == 5000 ? "5K" : intTickets); break;
                    case 1: lblTask2.Text = strLabelQuantity; lblTickets2.Text = String.Format("{0}", intTickets == 100000 ? "100K" : intTickets == 5000 ? "5K" : intTickets); break;
                    case 2: lblTask3.Text = strLabelQuantity; lblTickets3.Text = String.Format("{0}", intTickets == 100000 ? "100K" : intTickets == 5000 ? "5K" : intTickets); break;
                    case 3: lblTask4.Text = strLabelQuantity; lblTickets4.Text = String.Format("{0}", intTickets == 100000 ? "100K" : intTickets == 5000 ? "5K" : intTickets); break;
                    case 4: lblTask5.Text = strLabelQuantity; lblTickets5.Text = String.Format("{0}", intTickets == 100000 ? "100K" : intTickets == 5000 ? "5K" : intTickets); break;
                    default: break;
                }
            }
        }

        private void TaskLabelDescriptionUpdate(int intTask)
        {
            Tuple<TaskInfo, int> taskCodeData = TaskData.GetTaskCheckByIndex(intTasks[intTask]);
            if (taskCodeData.Item1 != null)
                lblTaskDescription.Text = taskCodeData.Item1.Name;

            Tuple<TortInfo, int> tortCodeData = TortData.GetTortCheckByIndex(intTasks[intTask]);
            if (tortCodeData.Item1 != null)
                lblTaskDescription.Text = "Visit Tortimer " + tortCodeData.Item1.Article + tortCodeData.Item1.Name;


            if (intTasks[intTask] >= 90 && intTasks[intTask] < 110)
            {
                switch (intTasks[intTask])
                {
                    case 90: lblTaskDescription.Text = "Plant 2 Flowers"; break;
                    case 91: lblTaskDescription.Text = "Dig up 3 Fossils"; break;
                    case 92: lblTaskDescription.Text = "Find the Money Rock"; break;
                    case 93: lblTaskDescription.Text = "Dig up the Shining Spot"; break;
                    case 94: lblTaskDescription.Text = "Pick 5 Weeds"; break;
                    case 95: lblTaskDescription.Text = "Greet 3 of your Villagers"; break;
                    case 99: lblTaskDescription.Text = String.Format("Happy Birthday, {0}! Enjoy some free Tickets!", strPlayerName); break;
                    case 100: lblTaskDescription.Text = "Give a Fish to Chip at the Fishing Tourney today"; break;
                    case 101: lblTaskDescription.Text = "Get 3 Items from Jack tonight"; break;
                    case 102: lblTaskDescription.Text = "Get 3 Harvest Items from Franklin today"; break;
                    case 103: lblTaskDescription.Text = "Open 3 Grab Bags for Sale Day today"; break;
                    case 104: lblTaskDescription.Text = "Get 3 Jingle Items from Jingle today"; break;
                    case 105: lblTaskDescription.Text = "Find 3 Mushrooms around Town"; break;
                    case 106: lblTaskDescription.Text = "Build a Snowman"; break;
                    case 107: lblTaskDescription.Text = "Win an Item from Nook's Raffle today"; break;
                    case 108: lblTaskDescription.Text = "Buy some Turnips from Joan today"; break;
                    case 109: lblTaskDescription.Text = "Listen to a K.K. Slider Concert tonight"; break;
                    default: break;
                }
            }
        }

        private void TaskLabelQuantityAdd(int intTask)
        {
            Tuple<TaskInfo, int> taskCodeData = TaskData.GetTaskCheckByIndex(intTasks[intTask]);
            if (taskCodeData.Item1 != null)
            {
                int intTasksQuantity = taskCodeData.Item1.Quantity;
                if (intTasksAmount[intTask] >= intTasksQuantity)
                {
                    TaskComplete(intTask, taskCodeData.Item1.Tag, taskCodeData.Item1.Index, taskCodeData.Item1.Tickets);
                    TaskLabelDescriptionUpdate(intTask);
                    return;
                }
                else
                {
                    intTasksAmount[intTask]++;
                    UpdateTasks();
                }

            }

            Tuple<TortInfo, int> tortCodeData = TortData.GetTortCheckByIndex(intTasks[intTask]);
            if (tortCodeData.Item1 != null)
            {
                int intTortQuantity = tortCodeData.Item1.Quantity;
                if (intTasksAmount[intTask] >= intTortQuantity)
                {
                    TaskComplete(intTask, "", tortCodeData.Item1.Index, tortCodeData.Item1.Tickets);
                    TaskLabelDescriptionUpdate(intTask);
                    return;
                }
                else
                {
                    intTasksAmount[intTask]++;
                    UpdateTasks();
                }
            }
            if (intTasks[intTask] >= 90 && intTasks[intTask] < 110)
            {
                switch (intTasks[intTask])
                {
                    case 90: if (intTasksAmount[intTask] >= 2) { TaskComplete(intTask, "", 90, 200); } else { intTasksAmount[intTask]++; UpdateTasks(); } break;
                    case 91: if (intTasksAmount[intTask] >= 3) { TaskComplete(intTask, "", 91, 250); } else { intTasksAmount[intTask]++; UpdateTasks(); } break;
                    case 92: if (intTasksAmount[intTask] >= 1) { TaskComplete(intTask, "", 92, 200); } else { intTasksAmount[intTask]++; UpdateTasks(); } break;
                    case 93: if (intTasksAmount[intTask] >= 1) { TaskComplete(intTask, "", 93, 200); } else { intTasksAmount[intTask]++; UpdateTasks(); } break;
                    case 94: if (intTasksAmount[intTask] >= 5) { TaskComplete(intTask, "", 94, 150); } else { intTasksAmount[intTask]++; UpdateTasks(); } break;
                    case 95: if (intTasksAmount[intTask] >= 3) { TaskComplete(intTask, "", 95, 250); } else { intTasksAmount[intTask]++; UpdateTasks(); } break;
                    case 99: if (intTasksAmount[intTask] >= 1) { TaskComplete(intTask, "", 99, 5000); } else { intTasksAmount[intTask]++; UpdateTasks(); } break;
                    case 100: if (intTasksAmount[intTask] >= 1) { TaskComplete(intTask, "", 100, 400); } else { intTasksAmount[intTask]++; UpdateTasks(); } break;
                    case 101: if (intTasksAmount[intTask] >= 3) { TaskComplete(intTask, "", 101, 500); } else { intTasksAmount[intTask]++; UpdateTasks(); } break;
                    case 102: if (intTasksAmount[intTask] >= 3) { TaskComplete(intTask, "", 102, 500); } else { intTasksAmount[intTask]++; UpdateTasks(); } break;
                    case 103: if (intTasksAmount[intTask] >= 3) { TaskComplete(intTask, "", 103, 400); } else { intTasksAmount[intTask]++; UpdateTasks(); } break;
                    case 104: if (intTasksAmount[intTask] >= 3) { TaskComplete(intTask, "", 104, 500); } else { intTasksAmount[intTask]++; UpdateTasks(); } break;
                    case 105: if (intTasksAmount[intTask] >= 3) { TaskComplete(intTask, "", 105, 400); } else { intTasksAmount[intTask]++; UpdateTasks(); } break;
                    case 106: if (intTasksAmount[intTask] >= 1) { TaskComplete(intTask, "", 106, 300); } else { intTasksAmount[intTask]++; UpdateTasks(); } break;
                    case 107: if (intTasksAmount[intTask] >= 1) { TaskComplete(intTask, "", 107, 450); } else { intTasksAmount[intTask]++; UpdateTasks(); } break;
                    case 108: if (intTasksAmount[intTask] >= 1) { TaskComplete(intTask, "", 108, 300); } else { intTasksAmount[intTask]++; UpdateTasks(); } break;
                    case 109: if (intTasksAmount[intTask] >= 1) { TaskComplete(intTask, "", 109, 500); } else { intTasksAmount[intTask]++; UpdateTasks(); } break;
                    default: break;
                }
            }

            TaskLabelDescriptionUpdate(intTask);
        }

        private void TaskLabelQuantitySub(int intTask)
        {
            if (intTasksAmount[intTask] > 0)
                intTasksAmount[intTask]--;
            else
            {

                boolDiscardTask = true;
                lblTaskDescription.Text = String.Format("Discard this task for {0} Tickets?", intTaskRemovePrice);
                DialogResult dr = MessageBox.Show(String.Format("Do you want to discard this task for the cost of {0} Tickets?", intTaskRemovePrice), "Discard Task?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    if (intTicketsCurrent >= intTaskRemovePrice)
                    {
                        for (int i = 0; i < intTasksMax; i++)
                        {
                            if (i > 0)
                                intTasksLast[i - 1] = intTasksLast[i];
                            if (intTasksLast[i] == 0 || i == intTasksMax - 1)
                                intTasksLast[i] = intTasks[intTask];
                        }
                        if (intTasks[intTask] < 90)
                        {
                            Tuple<TaskInfo, int> taskCodeData = TaskData.GetTaskCheckByIndex(intTasks[intTask]);
                            switch (taskCodeData.Item1.Tag)
                            {
                                case "Fish": intFishTaskIndex = 0; break;
                                case "Bug": intBugTaskIndex = 0; break;
                                default: break;
                            }
                        }
                        intTasks[intTask] = 0;
                        intTasksAmount[intTask] = 0;
                        intTicketsCurrent -= intTaskRemovePrice;
                        intTasksDiscarded++;
                        lblTaskDescription.Text = "";
                        

                        if (intTaskRemovePrice < 500)
                            intTaskRemovePrice += 50;
                    }
                    else
                        MessageBox.Show("You do not have enough Tickets to discard this task.", "Discard Task?", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                boolDiscardTask = false;
                lblTaskDescription.Text = "";
            }

            UpdateTasks();
            TaskLabelDescriptionUpdate(intTask);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            saveData();
            this.Hide();
        }

        private void btnTask1_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouse = (MouseEventArgs)e;
            if (mouse.Button == MouseButtons.Left)
                TaskLabelQuantityAdd(0);
            else if (mouse.Button == MouseButtons.Right)
                TaskLabelQuantitySub(0);
        }

        private void btnTask2_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouse = (MouseEventArgs)e;
            if (mouse.Button == MouseButtons.Left)
                TaskLabelQuantityAdd(1);
            else if (mouse.Button == MouseButtons.Right)
                TaskLabelQuantitySub(1);
        }

        private void btnTask3_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouse = (MouseEventArgs)e;
            if (mouse.Button == MouseButtons.Left)
                TaskLabelQuantityAdd(2);
            else if (mouse.Button == MouseButtons.Right)
                TaskLabelQuantitySub(2);
        }

        private void btnTask4_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouse = (MouseEventArgs)e;
            if (mouse.Button == MouseButtons.Left)
                TaskLabelQuantityAdd(3);
            else if (mouse.Button == MouseButtons.Right)
                TaskLabelQuantitySub(3);
        }

        private void btnTask5_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouse = (MouseEventArgs)e;
            if (mouse.Button == MouseButtons.Left)
                TaskLabelQuantityAdd(4);
            else if (mouse.Button == MouseButtons.Right)
                TaskLabelQuantitySub(4);
        }

        private void btnTask1_MouseEnter(object sender, EventArgs e)
        {
            TaskLabelDescriptionUpdate(0);
        }

        private void btnTask2_MouseEnter(object sender, EventArgs e)
        {
            TaskLabelDescriptionUpdate(1);
        }

        private void btnTask3_MouseEnter(object sender, EventArgs e)
        {
            TaskLabelDescriptionUpdate(2);
        }

        private void btnTask4_MouseEnter(object sender, EventArgs e)
        {
            TaskLabelDescriptionUpdate(3);
        }

        private void btnTask5_MouseEnter(object sender, EventArgs e)
        {
            TaskLabelDescriptionUpdate(4);
        }

        private void btnTask1_MouseLeave(object sender, EventArgs e)
        {
            if (boolDiscardTask == false)
                lblTaskDescription.Text = "";
        }

        private void btnTask2_MouseLeave(object sender, EventArgs e)
        {
            if (boolDiscardTask == false)
                lblTaskDescription.Text = "";
        }

        private void btnTask3_MouseLeave(object sender, EventArgs e)
        {
            if (boolDiscardTask == false)
                lblTaskDescription.Text = "";
        }

        private void btnTask4_MouseLeave(object sender, EventArgs e)
        {
            if (boolDiscardTask == false)
                lblTaskDescription.Text = "";
        }

        private void btnTask5_MouseLeave(object sender, EventArgs e)
        {
            if (boolDiscardTask == false)
                lblTaskDescription.Text = "";
        }

        private void lblTask1_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouse = (MouseEventArgs)e;
            if (mouse.Button == MouseButtons.Left)
                TaskLabelQuantityAdd(0);
            else if (mouse.Button == MouseButtons.Right)
                TaskLabelQuantitySub(0);
        }

        private void lblTask2_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouse = (MouseEventArgs)e;
            if (mouse.Button == MouseButtons.Left)
                TaskLabelQuantityAdd(1);
            else if (mouse.Button == MouseButtons.Right)
                TaskLabelQuantitySub(1);
        }

        private void lblTask3_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouse = (MouseEventArgs)e;
            if (mouse.Button == MouseButtons.Left)
                TaskLabelQuantityAdd(2);
            else if (mouse.Button == MouseButtons.Right)
                TaskLabelQuantitySub(2);
        }

        private void lblTask4_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouse = (MouseEventArgs)e;
            if (mouse.Button == MouseButtons.Left)
                TaskLabelQuantityAdd(3);
            else if (mouse.Button == MouseButtons.Right)
                TaskLabelQuantitySub(3);
        }

        private void lblTask5_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouse = (MouseEventArgs)e;
            if (mouse.Button == MouseButtons.Left)
                TaskLabelQuantityAdd(4);
            else if (mouse.Button == MouseButtons.Right)
                TaskLabelQuantitySub(4);
        }

        private void lblTask1_MouseEnter(object sender, EventArgs e)
        {
            TaskLabelDescriptionUpdate(0);
        }

        private void lblTask2_MouseEnter(object sender, EventArgs e)
        {
            TaskLabelDescriptionUpdate(1);
        }

        private void lblTask3_MouseEnter(object sender, EventArgs e)
        {
            TaskLabelDescriptionUpdate(2);
        }

        private void lblTask4_MouseEnter(object sender, EventArgs e)
        {
            TaskLabelDescriptionUpdate(3);
        }

        private void lblTask5_MouseEnter(object sender, EventArgs e)
        {
            TaskLabelDescriptionUpdate(4);
        }

        private void lblTask1_MouseLeave(object sender, EventArgs e)
        {
            if (boolDiscardTask == false)
                lblTaskDescription.Text = "";
        }

        private void lblTask2_MouseLeave(object sender, EventArgs e)
        {
            if (boolDiscardTask == false)
                lblTaskDescription.Text = "";
        }

        private void lblTask3_MouseLeave(object sender, EventArgs e)
        {
            if (boolDiscardTask == false)
                lblTaskDescription.Text = "";
        }

        private void lblTask4_MouseLeave(object sender, EventArgs e)
        {
            if (boolDiscardTask == false)
                lblTaskDescription.Text = "";
        }

        private void lblTask5_MouseLeave(object sender, EventArgs e)
        {
            if (boolDiscardTask == false)
                lblTaskDescription.Text = "";
        }

        private void Tasks_Load(object sender, EventArgs e)
        {
            //frmLoading.Hide();
        }

        private void txtTickets_TextChanged(object sender, EventArgs e)
        {
            //int intTickets = Convert.ToInt32((txtTickets.Text).Replace(",", ""));
        }

        private void Tasks_MouseEnter(object sender, EventArgs e)
        {
            if (boolDiscardTask == false)
                lblTaskDescription.Text = "";
        }
    }
}
