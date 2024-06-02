using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Formats.Asn1;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Animal_Crossing_Town_Ticket_Station
{
    public partial class Achievements : Form
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection();
        Font? fontFink14;
        Font? fontFink21;
        Font? fontFink24;
        int intTasksDiscarded = 0;
        static int intTasksTotal = 300;
        int[] intTasksComplete = new int[intTasksTotal];
        int intTasksCompleteTotal = 0;
        static string filedir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\ACTownTickets\";
        static string filepath = filedir + "SaveData.txt";
        int intTicketsCurrent = 0;
        int intTicketsTotal = 0;
        int intTicketsSpent = 0;
        int intTimePlayedDays = 0;
        double intTimePlayedSeconds = 0;
        int intFilterList = 1;
        static int intBoughtUniqueCategory = 18;
        int intBoughtTotal = 0;
        int[] intBoughtUnique = new int[intBoughtUniqueCategory];
        ImageList imgList = new ImageList();

        public Achievements()
        {
            InitializeComponent();
            CreateFonts();

            loadData();
            ChangeCheckboxes();
            CreateListView();
            PopulateListView();
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

            fontFink14 = new Font(fonts.Families[0], 14.0F);
            fontFink21 = new Font(fonts.Families[0], 21.0F);
            fontFink24 = new Font(fonts.Families[0], 24.0F);

            btnBack.Font = fontFink24;
            lstAchievements.Font = fontFink21;
            lblFilters.Font = fontFink14;
            
        }

        private void loadData()
        {
            string[] data = SaveData.GetSaveData().Item1.Data;

            intTicketsCurrent = data[12] != "" ? Convert.ToInt32(data[12]) : 0;
            intTicketsTotal = data[13] != "" ? Convert.ToInt32(data[13]) : 0;
            intTasksCompleteTotal = data[22] != "" ? Convert.ToInt32(data[22]) : 0;
            string[] strTaskData = data[23].Split(',');
            for (int i = 0; i < intTasksTotal; i++)
                intTasksComplete[i] = data[23] != "" ? Convert.ToInt32(strTaskData[i]) : 0;
            intTasksDiscarded = data[25] != "" ? Convert.ToInt32(data[25]) : 0;
            intTimePlayedDays = data[26] != "" ? Convert.ToInt32(data[26]) : 0;
            intTimePlayedSeconds = data[27] != "" ? Convert.ToInt32(data[27]) : 0;
            intFilterList = data[29] != "" ? Convert.ToInt32(data[29]) : 0;
            intBoughtTotal = data[32] != "" ? Convert.ToInt32(data[32]) : 0;
            string[] strUItemsBoughtData = data[33].Split(',');
            for (int i = 0; i < intBoughtUniqueCategory; i++)
                intBoughtUnique[i] = data[33] != "" ? Convert.ToInt32(strUItemsBoughtData[i]) : 0;
            intTicketsSpent = data[34] != "" ? Convert.ToInt32(data[34]) : 0;

        }

        private void saveData()
        {
            string[] data = SaveData.GetSaveData().Item1.Data;
            data[27] = intTimePlayedSeconds.ToString();
            data[29] = intFilterList.ToString();

            File.WriteAllLines(filepath, data);
        }


        private void ChangeCheckboxes()
        {
            switch (intFilterList)
            {
                case 0: chkFilterAll.Checked = true; break;
                case 1: chkFilterComplete.Checked = true; break;
                case 2: chkFilterIncomplete.Checked = true; break;
                default: break;
            }
            UpdateFilterLabelText();

            chkFilterAll.Appearance = Appearance.Button;
            chkFilterAll.Cursor = Cursors.Hand;
            chkFilterAll.AutoSize = false;
            chkFilterAll.Size = new Size(64, 64);
            chkFilterAll.BackgroundImage = Properties.Resources.UI_Filter_All_False;
            chkFilterAll.FlatStyle = FlatStyle.Flat;
            chkFilterAll.FlatAppearance.BorderSize = 0;
            chkFilterAll.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkFilterAll.ForeColor = Color.FromArgb(255, 244, 239, 216);
            if (chkFilterAll.Checked == true)
                chkFilterAll.BackgroundImage = Properties.Resources.UI_Filter_All;

            chkFilterComplete.Appearance = Appearance.Button;
            chkFilterComplete.Cursor = Cursors.Hand;
            chkFilterComplete.AutoSize = false;
            chkFilterComplete.Size = new Size(64, 64);
            chkFilterComplete.BackgroundImage = Properties.Resources.UI_Filter_Complete_False;
            chkFilterComplete.FlatStyle = FlatStyle.Flat;
            chkFilterComplete.FlatAppearance.BorderSize = 0;
            chkFilterComplete.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkFilterComplete.ForeColor = Color.FromArgb(255, 244, 239, 216);
            if (chkFilterComplete.Checked == true)
                chkFilterComplete.BackgroundImage = Properties.Resources.UI_Filter_Complete;

            chkFilterIncomplete.Appearance = Appearance.Button;
            chkFilterIncomplete.Cursor = Cursors.Hand;
            chkFilterIncomplete.AutoSize = false;
            chkFilterIncomplete.Size = new Size(64, 64);
            chkFilterIncomplete.BackgroundImage = Properties.Resources.UI_Filter_Incomplete_False;
            chkFilterIncomplete.FlatStyle = FlatStyle.Flat;
            chkFilterIncomplete.FlatAppearance.BorderSize = 0;
            chkFilterIncomplete.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkFilterIncomplete.ForeColor = Color.FromArgb(255, 244, 239, 216);
            if (chkFilterIncomplete.Checked == true)
                chkFilterIncomplete.BackgroundImage = Properties.Resources.UI_Filter_Incomplete;

            if (chkFilterAll.Checked == false && chkFilterComplete.Checked == false && chkFilterIncomplete.Checked == false)
                chkFilterComplete.Checked = true;
        }

        private void CreateListView()
        {
            lstAchievements.Columns.Add("Description of Achievements and Completed and", -2, HorizontalAlignment.Left);
            lstAchievements.Columns.Add("Amount", -2, HorizontalAlignment.Right);
            lstAchievements.Columns[0].Width = lstAchievements.Width - 4 - SystemInformation.VerticalScrollBarWidth;
            lstAchievements.HeaderStyle = ColumnHeaderStyle.None;
            lstAchievements.View = View.Details;
            lstAchievements.FullRowSelect = true;
            //lstAchievements.BorderStyle = BorderStyle.None;

            imgList.ImageSize = new Size(32, 32);
            imgList.ColorDepth = ColorDepth.Depth32Bit;
            imgList.Images.Add(Properties.Resources.IC_Stopwatch);
            for (int i = 1; i < TaskData.GetTaskInfoArrayLength(); i++)
            {
                Tuple<TaskInfo, int> taskCodeData = TaskData.GetTaskCheckByIndex(i);
                imgList.Images.Add(taskCodeData.Item1.ImageName);
            }
            for (int i = TaskData.GetTaskInfoArrayLength(); i < 90; i++)
                imgList.Images.Add(Properties.Resources.IC_Furniture_PG_Sprite_Upscaled);
            imgList.Images.Add(Properties.Resources.TK_Icon_Daily_Flowers);
            imgList.Images.Add(Properties.Resources.TK_Icon_Daily_Fossil);
            imgList.Images.Add(Properties.Resources.TK_Icon_Daily_MoneyRock);
            imgList.Images.Add(Properties.Resources.TK_Icon_Daily_ShinySpot);
            imgList.Images.Add(Properties.Resources.TK_Icon_Daily_Weeds);
            imgList.Images.Add(Properties.Resources.TK_Icon_Daily_Greeting);
            imgList.Images.Add(Properties.Resources.TK_Icon_Daily_Tickets);
            imgList.Images.Add(Properties.Resources.TK_Icon_Daily_Gyroids);
            imgList.Images.Add(Properties.Resources.TK_Icon_Daily_Rainbow);
            imgList.Images.Add(Properties.Resources.TK_Icon_Birthday);
            imgList.Images.Add(Properties.Resources.TK_Icon_Hol_FishingTourney);
            imgList.Images.Add(Properties.Resources.TK_Icon_Hol_Halloween);
            imgList.Images.Add(Properties.Resources.TK_Icon_Hol_Thanksgiving);
            imgList.Images.Add(Properties.Resources.TK_Icon_Hol_SaleDay);
            imgList.Images.Add(Properties.Resources.TK_Icon_Hol_Christmas);
            imgList.Images.Add(Properties.Resources.TK_Icon_Hol_Mushroom);
            imgList.Images.Add(Properties.Resources.TK_Icon_Hol_Snowman);
            imgList.Images.Add(Properties.Resources.TK_Icon_Hol_NookLottery);
            imgList.Images.Add(Properties.Resources.TK_Icon_Hol_Joan);
            imgList.Images.Add(Properties.Resources.TK_Icon_Hol_KKSlider);
            for (int i = 110; i < TortData.GetTortInfoArrayLength() + 110; i++)
            {
                Tuple<TortInfo, int> tortCodeData = TortData.GetTortCheckByIndex(i);
                imgList.Images.Add(tortCodeData.Item1.ImageName);
            }
            for (int i = TortData.GetTortInfoArrayLength() + 110; i < 199; i++)
                imgList.Images.Add(Properties.Resources.IC_Furniture_PG_Sprite_Upscaled);
            imgList.Images.Add(Properties.Resources.TK_Icon_Lighthouse);
            for (int i = 1; i < FishData.GetFishInfoArrayLength() + 1; i++)
            {
                Tuple<FishInfo, int> fishCodeData = FishData.GetFishCheckByIndex(i);
                imgList.Images.Add(fishCodeData.Item1.ImageName);
            }
            for (int i = 1; i < BugsData.GetBugsInfoArrayLength() + 1; i++)
            {
                Tuple<BugsInfo, int> bugCodeData = BugsData.GetBugCheckByIndex(i);
                imgList.Images.Add(bugCodeData.Item1.ImageName);
            }
            imgList.Images.Add(Properties.Resources.TK_Icon_Daily_Weeds);
            imgList.Images.Add(Properties.Resources.IC_TownTicket);
            imgList.Images.Add(Properties.Resources.IC_Code);
            imgList.Images.Add(Properties.Resources.IC_Furniture_PG_Sprite_Upscaled);
            imgList.Images.Add(Properties.Resources.IC_Carpet_PG_Inv_Icon_Upscaled);
            imgList.Images.Add(Properties.Resources.IC_Wallpaper_PG_Model);
            imgList.Images.Add(Properties.Resources.IC_Clothing_PG_Inv_Icon_Upscaled);
            imgList.Images.Add(Properties.Resources.IC_Tool);
            imgList.Images.Add(Properties.Resources.IC_Stationery_PG_Sprite_Upscaled);
            imgList.Images.Add(Properties.Resources.IC_Gyroid_PG_Sprite_Upscaled);
            imgList.Images.Add(Properties.Resources.IC_Fossil);
            imgList.Images.Add(Properties.Resources.IC_Aircheck_PG_Sprite_Upscaled);
            imgList.Images.Add(Properties.Resources.IC_Miscellaneous);
            imgList.Images.Add(Properties.Resources.IC_Beta);

            lstAchievements.LargeImageList = imgList;
            lstAchievements.SmallImageList = imgList;


            PopulateListView();
            ResizeListViewColumns(lstAchievements);

        }

        private void PopulateListView()
        {
            lstAchievements.BackColor = Color.FromArgb(255, 150, 255, 174);

            lstAchievements.BeginUpdate();
            lstAchievements.Items.Clear();

            TimeSpan t = TimeSpan.FromSeconds(intTimePlayedSeconds);
            int intTasksUnique = 0;
            for (int i = 0; i < intTasksTotal; i++)
                if (intTasksComplete[i] > 0)
                    intTasksUnique++;
            int intItemCodesUnique = 0;
            for (int i = 0; i < intBoughtUniqueCategory - 8; i++)
                intItemCodesUnique += intBoughtUnique[i];

            if (intFilterList == 0 || intTimePlayedSeconds > 0 && intFilterList == 1 || intTimePlayedDays > 0 && intFilterList == 1 ||
                intTasksCompleteTotal > 0 && intFilterList == 1 || intTasksUnique > 0 && intFilterList == 1 || intTicketsTotal > 0 && intFilterList == 1 ||
                intTicketsSpent > 0 && intFilterList == 1 || intBoughtTotal > 0 && intFilterList == 1 || intItemCodesUnique > 0 && intFilterList == 1 ||
                intTimePlayedSeconds <= 0 && intFilterList == 2 || intTimePlayedDays <= 0 && intFilterList == 2 || intTasksCompleteTotal <= 0 && intFilterList == 2 ||
                intTasksUnique <= 0 && intFilterList == 2 || intTicketsTotal <= 0 && intFilterList == 2 || intTicketsSpent <= 0 && intFilterList == 2 ||
                intBoughtTotal <= 0 && intFilterList == 2 || intItemCodesUnique <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Totals:                                                       ").Font = new Font("FinkHeavy", 24, FontStyle.Underline);
            if (intFilterList == 0 || intTimePlayedSeconds > 0 && intFilterList == 1 || intTimePlayedSeconds <= 0 && intFilterList == 2)
                if (t.Days < 366)
                    lstAchievements.Items.Add(" Total Time Played:", 0).SubItems.Add(string.Format("{0:D2}:{1:D2}:{2:D2} ", (t.Days * 24 + t.Hours).ToString("N0"), t.Minutes, t.Seconds));
                else
                    lstAchievements.Items.Add(" Total Time Played:", 0).SubItems.Add(string.Format("{0:D2}:{1:D2}", (t.Days * 24 + t.Hours).ToString("N0"), t.Minutes));
            if (intFilterList == 0 || intTimePlayedDays > 0 && intFilterList == 1 || intTimePlayedDays <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Unique days played:", 0).SubItems.Add(intTimePlayedDays.ToString("N0") + " ");
            if (intFilterList == 0 || intTasksCompleteTotal > 0 && intFilterList == 1 || intTasksCompleteTotal <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Total Tasks completed:", 280).SubItems.Add(intTasksCompleteTotal.ToString("N0") + " ");
            if (intFilterList == 0 || intTasksUnique > 0 && intFilterList == 1 || intTasksUnique <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Unique Tasks completed:", 280).SubItems.Add(intTasksUnique.ToString("N0") + " ");
            if (intFilterList == 0 || intTicketsTotal > 0 && intFilterList == 1 || intTicketsTotal <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Total Tickets obtained:", 281).SubItems.Add(intTicketsTotal.ToString("N0") + " ");
            if (intFilterList == 0 || intTicketsSpent > 0 && intFilterList == 1 || intTicketsSpent <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Total Tickets exchanged:", 281).SubItems.Add(intTicketsSpent.ToString("N0") + " ");
            if (intFilterList == 0 || intBoughtTotal > 0 && intFilterList == 1 || intBoughtTotal <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Total Item Codes obtained:", 282).SubItems.Add(intBoughtTotal.ToString("N0") + " ");
            if (intFilterList == 0 || intItemCodesUnique > 0 && intFilterList == 1 || intItemCodesUnique <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Unique Item Codes obtained:", 282).SubItems.Add(intItemCodesUnique.ToString("N0") + " ");
            if (intFilterList == 0 || intTimePlayedSeconds > 0 && intFilterList == 1 || intTimePlayedDays > 0 && intFilterList == 1 ||
                intTasksCompleteTotal > 0 && intFilterList == 1 || intTasksUnique > 0 && intFilterList == 1 || intTicketsTotal > 0 && intFilterList == 1 ||
                intTicketsSpent > 0 && intFilterList == 1 || intBoughtTotal > 0 && intFilterList == 1 || intItemCodesUnique > 0 && intFilterList == 1 ||
                intTimePlayedSeconds <= 0 && intFilterList == 2 || intTimePlayedDays <= 0 && intFilterList == 2 || intTasksCompleteTotal <= 0 && intFilterList == 2 ||
                intTasksUnique <= 0 && intFilterList == 2 || intTicketsTotal <= 0 && intFilterList == 2 || intTicketsSpent <= 0 && intFilterList == 2 ||
                intBoughtTotal <= 0 && intFilterList == 2 || intItemCodesUnique <= 0 && intFilterList == 2)
                lstAchievements.Items.Add("").SubItems.Add("");

            bool hasUItemsBoughtAll = true;
            bool hasUItemsBoughtNone = true;
            for (int i = 0; i < intBoughtUniqueCategory - 7; i++)
            {
                if (intBoughtUnique[i] > 0)
                    hasUItemsBoughtNone = false;
                if (intBoughtUnique[i] <= 0)
                    hasUItemsBoughtAll = false;
            }
            if (intFilterList == 0 || intFilterList == 1 && hasUItemsBoughtNone == false || intFilterList == 2 && hasUItemsBoughtAll == false)
                lstAchievements.Items.Add(" Item Codes Obtained:                               ").Font = new Font("FinkHeavy", 24, FontStyle.Underline);
            if (intFilterList == 0 || intBoughtUnique[0] > 0 && intFilterList == 1 || intBoughtUnique[0] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Furniture Codes:", 283).SubItems.Add(intBoughtUnique[0].ToString("N0") + " ");
            if (intFilterList == 0 || intBoughtUnique[1] > 0 && intFilterList == 1 || intBoughtUnique[1] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Carpet Codes:", 284).SubItems.Add(intBoughtUnique[1].ToString("N0") + " ");
            if (intFilterList == 0 || intBoughtUnique[2] > 0 && intFilterList == 1 || intBoughtUnique[2] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Wallpaper Codes:", 285).SubItems.Add(intBoughtUnique[2].ToString("N0") + " ");
            if (intFilterList == 0 || intBoughtUnique[3] > 0 && intFilterList == 1 || intBoughtUnique[3] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Clothing Codes:", 286).SubItems.Add(intBoughtUnique[3].ToString("N0") + " ");
            if (intFilterList == 0 || intBoughtUnique[4] > 0 && intFilterList == 1 || intBoughtUnique[4] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Handhelds Codes:", 287).SubItems.Add(intBoughtUnique[4].ToString("N0") + " ");
            if (intFilterList == 0 || intBoughtUnique[5] > 0 && intFilterList == 1 || intBoughtUnique[5] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Stationery Codes:", 288).SubItems.Add(intBoughtUnique[5].ToString("N0") + " ");
            if (intFilterList == 0 || intBoughtUnique[6] > 0 && intFilterList == 1 || intBoughtUnique[6] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Gyroid Codes:", 289).SubItems.Add(intBoughtUnique[6].ToString("N0") + " ");
            if (intFilterList == 0 || intBoughtUnique[7] > 0 && intFilterList == 1 || intBoughtUnique[7] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Fossil Codes:", 290).SubItems.Add(intBoughtUnique[7].ToString("N0") + " ");
            if (intFilterList == 0 || intBoughtUnique[8] > 0 && intFilterList == 1 || intBoughtUnique[8] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Music Codes:", 291).SubItems.Add(intBoughtUnique[8].ToString("N0") + " ");
            if (intFilterList == 0 || intBoughtUnique[9] > 0 && intFilterList == 1 || intBoughtUnique[9] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Miscellaneous Codes:", 292).SubItems.Add(intBoughtUnique[9].ToString("N0") + " ");
            if (intFilterList == 0 || intBoughtUnique[10] > 0 && intFilterList == 1 || intBoughtUnique[10] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Beta Item Codes:", 293).SubItems.Add(intBoughtUnique[10].ToString("N0") + " ");
            if (intFilterList == 0 || intFilterList == 1 && hasUItemsBoughtNone == false || intFilterList == 2 && hasUItemsBoughtAll == false)
                lstAchievements.Items.Add("").SubItems.Add("");

            bool hasTasksCompletedAll = true;
            bool hasTasksCompletedNone = true;
            for (int i = 0; i < intTasksTotal; i++)
            {
                if (intTasksComplete[i] > 0)
                    hasTasksCompletedNone = false;
                if (intTasksComplete[i] <= 0)
                    hasTasksCompletedAll = false;
            }
            if (intFilterList == 0 || intFilterList == 1 && hasTasksCompletedNone == false || intFilterList == 2 && hasTasksCompletedAll == false)
                lstAchievements.Items.Add(" Individual Tasks Completed:                   ").Font = new Font("FinkHeavy", 24, FontStyle.Underline);
            for (int i = 1; i < TaskData.GetTaskInfoArrayLength(); i++)
            {
                if (TaskData.GetTaskCheckByIndex(i).Item1.Tag == "Fish")
                {
                    if (intFilterList == 0 || intTasksComplete[i] > 0 && intFilterList == 1 || intTasksComplete[i] <= 0 && intFilterList == 2)
                        lstAchievements.Items.Add(" Catch a Specific Fish:", 4).SubItems.Add(intTasksComplete[i] + " ");
                }
                else if (TaskData.GetTaskCheckByIndex(i).Item1.Tag == "Bug")
                {
                    if (intFilterList == 0 || intTasksComplete[i] > 0 && intFilterList == 1 || intTasksComplete[i] <= 0 && intFilterList == 2)
                        lstAchievements.Items.Add(" Catch a Specific Bug:", 7).SubItems.Add(intTasksComplete[i] + " ");
                }
                else
                {
                    if (intFilterList == 0 || intTasksComplete[i] > 0 && intFilterList == 1 || intTasksComplete[i] <= 0 && intFilterList == 2)
                        lstAchievements.Items.Add(" " + TaskData.GetTaskCheckByIndex(i).Item1.Name + ":", i).SubItems.Add(intTasksComplete[i] + " ");
                }
            }
            if (intFilterList == 0 || intTasksComplete[90] > 0 && intFilterList == 1 || intTasksComplete[90] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Plant 2 Flowers:", 90).SubItems.Add(intTasksComplete[90] + " ");
            if (intFilterList == 0 || intTasksComplete[91] > 0 && intFilterList == 1 || intTasksComplete[91] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Dig up 3 Fossils:", 91).SubItems.Add(intTasksComplete[91] + " ");
            if (intFilterList == 0 || intTasksComplete[92] > 0 && intFilterList == 1 || intTasksComplete[92] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Find the Money Rock:", 92).SubItems.Add(intTasksComplete[92] + " ");
            if (intFilterList == 0 || intTasksComplete[93] > 0 && intFilterList == 1 || intTasksComplete[93] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Dig up the Shining Spot:", 93).SubItems.Add(intTasksComplete[93] + " ");
            if (intFilterList == 0 || intTasksComplete[94] > 0 && intFilterList == 1 || intTasksComplete[94] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Pick 5 Weeds:", 94).SubItems.Add(intTasksComplete[94] + " ");
            if (intFilterList == 0 || intTasksComplete[95] > 0 && intFilterList == 1 || intTasksComplete[95] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Greet 3 of your Villagers:", 95).SubItems.Add(intTasksComplete[95] + " ");
            if (intFilterList == 0 || intTasksComplete[96] > 0 && intFilterList == 1 || intTasksComplete[96] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Get 5 Raffle Tickets from Nook's:", 96).SubItems.Add(intTasksComplete[96] + " ");
            if (intFilterList == 0 || intTasksComplete[97] > 0 && intFilterList == 1 || intTasksComplete[97] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Dig up 2 Gyroids:", 97).SubItems.Add(intTasksComplete[97] + " ");
            if (intFilterList == 0 || intTasksComplete[98] > 0 && intFilterList == 1 || intTasksComplete[98] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Admire a Rainbow over a Waterfall:", 98).SubItems.Add(intTasksComplete[98] + " ");
            if (intFilterList == 0 || intTasksComplete[99] > 0 && intFilterList == 1 || intTasksComplete[99] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Free Tickets for your Birthday:", 99).SubItems.Add(intTasksComplete[99] + " ");
            if (intFilterList == 0 || intTasksComplete[100] > 0 && intFilterList == 1 || intTasksComplete[100] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Give a Fish to Chip:", 100).SubItems.Add(intTasksComplete[100] + " ");
            if (intFilterList == 0 || intTasksComplete[101] > 0 && intFilterList == 1 || intTasksComplete[101] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Get 3 Items from Jack:", 101).SubItems.Add(intTasksComplete[101] + " ");
            if (intFilterList == 0 || intTasksComplete[102] > 0 && intFilterList == 1 || intTasksComplete[102] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Get 3 Harvest Items from Franklin:", 102).SubItems.Add(intTasksComplete[102] + " ");
            if (intFilterList == 0 || intTasksComplete[103] > 0 && intFilterList == 1 || intTasksComplete[103] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Open 3 Grab Bags for Sale Day:", 103).SubItems.Add(intTasksComplete[103] + " ");
            if (intFilterList == 0 || intTasksComplete[104] > 0 && intFilterList == 1 || intTasksComplete[104] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Get 3 Jingle Items from Jingle:", 104).SubItems.Add(intTasksComplete[104] + " ");
            if (intFilterList == 0 || intTasksComplete[105] > 0 && intFilterList == 1 || intTasksComplete[105] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Find 3 Mushrooms around Town:", 105).SubItems.Add(intTasksComplete[105] + " ");
            if (intFilterList == 0 || intTasksComplete[106] > 0 && intFilterList == 1 || intTasksComplete[106] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Build a Snowman:", 106).SubItems.Add(intTasksComplete[106] + " ");
            if (intFilterList == 0 || intTasksComplete[107] > 0 && intFilterList == 1 || intTasksComplete[107] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Win an Item from Nook's Raffle:", 107).SubItems.Add(intTasksComplete[107] + " ");
            if (intFilterList == 0 || intTasksComplete[108] > 0 && intFilterList == 1 || intTasksComplete[108] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Buy some Turnips from Joan:", 108).SubItems.Add(intTasksComplete[108] + " ");
            if (intFilterList == 0 || intTasksComplete[109] > 0 && intFilterList == 1 || intTasksComplete[109] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Listen to a K.K. Slider Concert:", 109).SubItems.Add(intTasksComplete[109] + " ");
            for (int i = 110; i < TortData.GetTortInfoArrayLength() + 110; i++)
                if (intFilterList == 0 || intTasksComplete[i] > 0 && intFilterList == 1 || intTasksComplete[i] <= 0 && intFilterList == 2)
                    lstAchievements.Items.Add(" Visit Tortimer for " + TortData.GetTortCheckByIndex(i).Item1.Name + ":", i).SubItems.Add(intTasksComplete[i] + " ");
            if (intFilterList == 0 || intTasksComplete[199] > 0 && intFilterList == 1 || intTasksComplete[199] <= 0 && intFilterList == 2)
                lstAchievements.Items.Add(" Turn on the Lighthouse:", 199).SubItems.Add(intTasksComplete[199] + " ");
            for (int i = 201; i < FishData.GetFishInfoArrayLength() + 201; i++)
                if (intFilterList == 0 || intTasksComplete[i] > 0 && intFilterList == 1 || intTasksComplete[i] <= 0 && intFilterList == 2)
                    lstAchievements.Items.Add(" " + FishData.GetFishCheckByIndex(i - 200).Item1.Name + ":", i - 1).SubItems.Add(intTasksComplete[i] + " ");
            for (int i = 251; i < BugsData.GetBugsInfoArrayLength() + 251; i++)
                if (intFilterList == 0 || intTasksComplete[i] > 0 && intFilterList == 1 || intTasksComplete[i] <= 0 && intFilterList == 2)
                    lstAchievements.Items.Add(" " + BugsData.GetBugCheckByIndex(i - 250).Item1.Name + ":", i - 11).SubItems.Add(intTasksComplete[i] + " ");

            lstAchievements.EndUpdate();

            saveData();
        }

        private void ResizeListViewColumns(ListView lv)
        {
            foreach (ColumnHeader column in lv.Columns)
            {
                column.Width = -2;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void lstAchievements_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lblTest.Text = lstAchievements.FocusedItem.Text;
        }

        private void lstAchievements_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            //lblTest.Text = e.Item.Text;
            //picTest.Image = lstAchievements.LargeImageList.Images[e.Item.ImageIndex];
        }

        private void lstAchievements_MouseLeave(object sender, EventArgs e)
        {
            //lblTest.Text = "";
        }

        private void CheckFilters()
        {
            if (chkFilterAll.Checked == false && chkFilterComplete.Checked == false && chkFilterIncomplete.Checked == false)
            {
                switch (intFilterList)
                {
                    case 0: chkFilterAll.Checked = true; break;
                    case 1: chkFilterComplete.Checked = true; break;
                    case 2: chkFilterIncomplete.Checked = true; break;
                    default: chkFilterComplete.Checked = true; break;
                }
            }
        }

        private void chkFilterAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFilterAll.Checked == true)
            {
                chkFilterAll.BackgroundImage = Properties.Resources.UI_Filter_All;
                chkFilterComplete.Checked = false;
                chkFilterIncomplete.Checked = false;
                intFilterList = 0;
            }
            else
            {
                chkFilterAll.BackgroundImage = Properties.Resources.UI_Filter_All_False;
            }
            CheckFilters();
            PopulateListView();
        }

        private void chkFilterComplete_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFilterComplete.Checked == true)
            {
                chkFilterComplete.BackgroundImage = Properties.Resources.UI_Filter_Complete;
                chkFilterAll.Checked = false;
                chkFilterIncomplete.Checked = false;
                intFilterList = 1;
            }
            else
            {
                chkFilterComplete.BackgroundImage = Properties.Resources.UI_Filter_Complete_False;
            }
            CheckFilters();
            PopulateListView();
        }

        private void chkFilterIncomplete_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFilterIncomplete.Checked == true)
            {
                chkFilterIncomplete.BackgroundImage = Properties.Resources.UI_Filter_Incomplete;
                chkFilterComplete.Checked = false;
                chkFilterAll.Checked = false;
                intFilterList = 2;
            }
            else
            {
                chkFilterIncomplete.BackgroundImage = Properties.Resources.UI_Filter_Incomplete_False;
            }
            CheckFilters();
            PopulateListView();
        }

        private void UpdateFilterLabelText()
        {
            lblFilters.BackColor = Color.FromArgb(255, 150, 255, 174);

            switch (intFilterList)
            {
                case 0: lblFilters.Text = "Show All Achievements\n(Warning: Task Spoilers!)"; break;
                case 1: lblFilters.Text = "Completed Achievements"; break;
                case 2: lblFilters.Text = "Incomplete Achievements\n(Warning: Task Spoilers!)"; break;
                default: break;
            }
        }

        private void chkFilterAll_MouseEnter(object sender, EventArgs e)
        {
            lblFilters.Text = "Show All Achievements\n(Warning: Task Spoilers!)";
        }

        private void chkFilterComplete_MouseEnter(object sender, EventArgs e)
        {
            lblFilters.Text = "Completed Achievements";
        }

        private void chkFilterIncomplete_MouseEnter(object sender, EventArgs e)
        {
            lblFilters.Text = "Incomplete Achievements\n(Warning: Task Spoilers!)";
        }

        private void chkFilterAll_MouseLeave(object sender, EventArgs e)
        {
            UpdateFilterLabelText();
        }

        private void chkFilterComplete_MouseLeave(object sender, EventArgs e)
        {
            UpdateFilterLabelText();
        }

        private void chkFilterIncomplete_MouseLeave(object sender, EventArgs e)
        {
            UpdateFilterLabelText();
        }
    }
}
