using ACPasswordLibrary.Core.AnimalCrossing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACPassword = ACPasswordLibrary.Core.AnimalCrossing;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace Animal_Crossing_Town_Ticket_Station
{
    public partial class ShopAround : Form
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection();
        Font? fontFink18;
        Font? fontFink21;
        Font? fontFink24;
        Font? fontFink28;
        Font? fontFink36;
        Font? fontFink42;
        static string filedir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\ACTownTickets\";
        static string filepath = filedir + "SaveData.txt";
        static int intDataLines = 50;
        string[] data = new string[intDataLines];
        DateTime loadTime;
        TimeSpan timeOffset = new TimeSpan(0, 0, 0, 0, 0);
        int intTicketsCurrent = 0;
        int intTicketsTotal = 0;
        int intTicketsSpent = 0;
        static int intItemsTotal = 1395;
        int[] intBoughtList = new int[intItemsTotal + 1];
        string[] strCodeList = new string[intItemsTotal + 1];
        static int intFilterListTotal = 20;
        int[] intFilterList = new int[intFilterListTotal];
        static int intBoughtUniqueCategory = 18;
        int intBoughtTotal = 0;
        int[] intBoughtUnique = new int[intBoughtUniqueCategory];
        ImageList imgList = new ImageList();
        bool boolLoading = true;
        ListViewItem? previousListViewItem = null;
        Tuple<ItemInfo, int> previousItem;
        Color previousListViewItemColor = Color.Black;
        bool boolItemSelected = false;
        int intLastViewedDialog = 0;
        ListViewItem? lviSelectedItem = null;
        int intTimePlayedSeconds = 0;
        string strNESinQueue = "";
        bool boolHasNESGame = false;
        static ushort intNESStart = 0x1DA8;
        static ushort intNESEnd = 0x1DE4;
        bool boolNESDialog = false;
        Point pntDialogYes;
        string strPlayerName = "";
        string strTownName = "";
        bool boolBirthdaySet = false;
        DateTime birthday;
        static int intTasksTotal = 300;
        int[] intTasksComplete = new int[intTasksTotal];
        private List<ListViewItem> listShopItemListViewItem;
        private List<ListViewItem> filteredList = null;
        bool bInvalidItemSelected = false;
        ListViewItem? invalidSelectedItem = null;
        static readonly ListViewItem emptyViewItem = new();

        public static void SetDoubleBuffering(Control control, bool value)
        {
            PropertyInfo controlProperty = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
            controlProperty.SetValue(control, value, null);
        }

        public ShopAround()
        {
            InitializeComponent();
            CreateFonts();

            loadData();
            UpdateClockImages();
            CreateListView();
            UpdateShopDialogLabelText(0, "", 0);
            ChangeCheckboxes();

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Tick += new EventHandler(UpdateClock);
            timer.Start();

            UpdateFilterLabelText();
            UpdateCheatLabel();

            InitializeShopListView();
            lstShop_UpdateFilterAndSort("");
            SetDoubleBuffering(lstShop, true); // double buffering prevents flickering

            lstShop.VirtualMode = true;
            //lstShop.VirtualListSize = intItemsTotal;
            lstShop.RetrieveVirtualItem += lstShop_RetrieveVirtualItem;
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

            fontFink18 = new Font(fonts.Families[0], 18.0F);
            fontFink21 = new Font(fonts.Families[0], 21.0F);
            fontFink24 = new Font(fonts.Families[0], 24.0F);
            fontFink28 = new Font(fonts.Families[0], 27.75F);
            fontFink36 = new Font(fonts.Families[0], 36.0F);
            fontFink42 = new Font(fonts.Families[0], 42.0F);

            btnBack.Font = fontFink24;
            lstShop.Font = fontFink21;
            lblDialogTimmy.Font = fontFink28;
            lblDialogTommy.Font = fontFink21;
            lblFilterCategory.Font = fontFink18;
            lblFilterGroup.Font = fontFink18;
            lblSort.Font = fontFink18;
            lblAvailability.Font = fontFink18;
            lblTickets.Font = fontFink42;
            btnTicketsPrice.Font = fontFink36;
            lblFengShui.Font = fontFink24;
            lblFengShuiColor.Font = fontFink24;
            lblTimesBought.Font = fontFink24;
            lblUnlock.Font = fontFink18;
            btnDialogSelectYes.Font = fontFink28;
            btnDialogSelectNo.Font = fontFink28;
            txtFilterSearch.Font = fontFink18;
        }

        private void UpdateClock(object sender, EventArgs e)
        {
            GC.Collect();
            UpdateClockImages();

            intTimePlayedSeconds++;
            DateTime now = DateTime.Now + timeOffset;

            if (loadTime.Day != now.Day && strNESinQueue != "" || loadTime.Month != now.Month && strNESinQueue != "" || loadTime.Year != now.Year && strNESinQueue != "")
            {
                Tuple<ItemInfo, int> itemCodeData = new Tuple<ItemInfo, int>(null, 0);
                itemCodeData = ItemData.GetItemCheckByName(strNESinQueue, out int _);

                boolItemSelected = true;
                txtFilterSearch.ReadOnly = true;
                txtFilterSearch.BackColor = Color.Gainsboro;
                txtFilterSearch.Enabled = false;
                imgShopItem.BackgroundImage = null;
                imgShopItemSmall.BackgroundImage = ResizeImage(itemCodeData.Item1.ImageName, 140, 140);

                UpdateShopDialogLabelText(8, strNESinQueue, 0);
                loadTime = now;
            }

            if (boolLoading == true && boolNESDialog == false)
                lstShop_UpdateFilterAndSort(txtFilterSearch.Text); //PopulateListView();
            boolLoading = false;
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

        private void loadData()
        {
            data = SaveData.GetSaveData().Item1.Data;

            string[] strTaskCompleteData = new string[intTasksTotal];

            loadTime = data[0] != "" ? Convert.ToDateTime(data[0]) : DateTime.Now;
            if (data[1] != "")
                timeOffset = TimeSpan.Parse(data[1]);
            strPlayerName = data[2] == "" ? "Not Set" : data[2];
            strTownName = data[3] == "" ? "Not Set" : data[3];
            intTicketsCurrent = data[12] != "" ? Convert.ToInt32(data[12]) : 0;
            intTimePlayedSeconds = data[27] != "" ? Convert.ToInt32(data[27]) : 0;
            intTicketsTotal = data[13] != "" ? Convert.ToInt32(data[13]) : 0;
            strTaskCompleteData = data[23].Split(',');
            for (int i = 0; i < intTasksTotal; i++)
                intTasksComplete[i] = data[23] != "" ? Convert.ToInt32(strTaskCompleteData[i]) : 0;
            boolHasNESGame = data[38] != "" ? Convert.ToBoolean(data[38]) : false;
            intTicketsSpent = data[34] != "" ? Convert.ToInt32(data[34]) : 0;
            intBoughtTotal = data[32] != "" ? Convert.ToInt32(data[32]) : 0;
            string[] strFilterListData = data[30].Split(',');
            for (int i = 0; i < intFilterListTotal; i++)
                intFilterList[i] = data[30] != "" ? Convert.ToInt32(strFilterListData[i]) : 0;
            string[] strUItemsBoughtData = data[33].Split(',');
            for (int i = 0; i < intBoughtUniqueCategory; i++)
                intBoughtUnique[i] = data[33] != "" ? Convert.ToInt32(strUItemsBoughtData[i]) : 0;
            string[] strBoughtListData = data[35].Split(',');
            for (int i = 0; i < intItemsTotal + 1; i++)
                intBoughtList[i] = data[35] != "" ? Convert.ToInt32(strBoughtListData[i]) : 0;
            string[] strCodeListData = data[36].Split(',');
            for (int i = 0; i < intItemsTotal + 1; i++)
                strCodeList[i] = data[36] != "" ? strCodeListData[i] : "";
            strNESinQueue = data[37] != "" ? data[37] : "";
            birthday = data[28] != "" ? Convert.ToDateTime(data[28]) : DateTime.Now;
            boolBirthdaySet = data[39].ToLower() == "true" ? true : false;
        }

        private void saveData()
        {
            data = SaveData.GetSaveData().Item1.Data;
            string sb = "";

            data[0] = (DateTime.Now + timeOffset).ToString();
            data[12] = intTicketsCurrent.ToString();
            data[27] = intTimePlayedSeconds.ToString();
            data[13] = intTicketsTotal.ToString();
            data[34] = intTicketsSpent.ToString();
            data[32] = intBoughtTotal.ToString();
            sb = "";
            for (int i = 0; i < intFilterListTotal; i++)
                sb += intFilterList[i] + ",";
            data[30] = sb;
            sb = "";
            for (int i = 0; i < intBoughtUniqueCategory; i++)
                sb += intBoughtUnique[i] + ",";
            data[33] = sb;
            sb = "";
            for (int i = 0; i < intItemsTotal + 1; i++)
                sb += intBoughtList[i] + ",";
            data[35] = sb;
            sb = "";
            for (int i = 0; i < intItemsTotal + 1; i++)
                sb += strCodeList[i] + ",";
            data[36] = sb;
            data[37] = strNESinQueue;
            data[38] = boolHasNESGame.ToString();

            File.WriteAllLines(filepath, data);
        }

        private string GeneratePassword(Tuple<ItemInfo, int> itemCodeData)
        {
            byte[] passwordBytes = new byte[28];
            Span<byte> passwordSpan = passwordBytes.AsSpan();
            CodeType codeType = itemCodeData.Item1.CodeType;
            byte[] str1 = String2Bytes("!", 8); //Town Name is first thing Nook says
            byte[] str0 = String2Bytes("!", 8); //Player Name in second thing Nook says
            uint hitRateIndex = 1; // 4 for magazine, 1 for famicom and user
            ushort itemIndex = itemCodeData.Item1.ID;
            byte npcType = 0x01; // 0x01 for Magazine, 0x01 or 0x00 for User or Famicom
            byte npcCode = 0xFF; // anything

            string resourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames().Single(str => str.EndsWith("banned_words_list.txt"));
            string[] strBadWords = ReadResource(resourceName).Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            if (codeType == CodeType.Magazine)
            {
                str1 = String2Bytes("Town T", 8);
                str0 = String2Bytes("ickets", 8);
                hitRateIndex = 4;
            }
            else
            {
                str1 = String2Bytes(strTownName, 8);
                str0 = String2Bytes(strPlayerName, 8);
                hitRateIndex = 1;
            }

            bool boolPass = false;
            int counterPass = 0;
            Random rnd = new Random();
            string newPassword = "";

            while (boolPass == false && counterPass < 100)
            {
                boolPass = true;
                counterPass++;
                npcCode = Convert.ToByte(rnd.Next(0, 255));
                ACPassword.Encoder.MakePasswordForced(ref passwordSpan, codeType, hitRateIndex, str0, str1, itemIndex, npcType, npcCode);
                newPassword = BytesToString(passwordBytes);

                foreach (var str in strBadWords)
                    if (newPassword.ToLower().Contains(str))
                        boolPass = false;
            }

            return newPassword;
        }

        private static readonly Dictionary<string, char> acChars = new()
        {
            { "🌢", '\x3B' },
            { "💢", '\x5C' },
            { "a̱", '\x99' },
            { "o̱", '\x9A' },
            { "🌬", '\xAA' },
            { "🗙", '\xB1' },
            { "🍀", '\xB8' },
            { "💀", '\xBA' },
            { "😮", '\xBB' },
            { "😄", '\xBC' },
            { "😣", '\xBD' },
            { "😠", '\xBE' },
            { "😃", '\xBF' },
            { "🔨", '\xC2' },
            { "🎀", '\xC3' },
            { "💰", '\xC5' },
            { "🐾", '\xC6' },
            { "🐶", '\xC7' },
            { "🐱", '\xC8' },
            { "🐰", '\xC9' },
            { "🐦", '\xCA' },
            { "🐮", '\xCB' },
            { "🐷", '\xCC' },
            { "🐟", '\xCE' },
            { "🐞", '\xCF' },
        };

        private static byte[] String2Bytes(in string s, int length)
        {
            var bytes = new byte[length];
            StringInfo stringInfo = new(s);
            int len = stringInfo.LengthInTextElements;

            for (int i = 0; i < length; i++)
            {
                if (i < len)
                {
                    string sub = stringInfo.SubstringByTextElements(i, 1);
                    if (sub.Length == 1)
                    {
                        bytes[i] = (byte)Array.IndexOf(Common.CharacterSet, sub[0]);
                    }
                    else if (acChars.ContainsKey(sub))
                    {
                        bytes[i] = (byte)acChars[sub];
                    }
                    else
                    {
                        bytes[i] = 0x20;
                    }
                }
                else
                {
                    bytes[i] = 0x20;
                }
            }

            return bytes;

        }

        public static string BytesToString(in byte[] data) => data.Aggregate("", (current, b) => current += Common.CharacterSet[b]);

        public string ReadResource(string name)
        {
            // Determine path
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(name))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private void ChangeCheckboxes()
        {
            switch (intFilterList[0])
            {
                case 0: chkFilterCategoryEverything.Checked = true; break;
                case 1: chkFilterCategoryFurniture.Checked = true; break;
                case 2: chkFilterCategoryCarpet.Checked = true; break;
                case 3: chkFilterCategoryWallpaper.Checked = true; break;
                case 4: chkFilterCategoryClothing.Checked = true; break;
                case 5: chkFilterCategoryTools.Checked = true; break;
                case 6: chkFilterCategoryStationery.Checked = true; break;
                case 7: chkFilterCategoryGyroid.Checked = true; break;
                case 8: chkFilterCategoryFossil.Checked = true; break;
                case 9: chkFilterCategoryMusic.Checked = true; break;
                case 10: chkFilterCategoryMiscellaneous.Checked = true; break;
                default: break;
            }
            if (intFilterList[1] == 0)
                intFilterList[1] = 2;
            switch (intFilterList[1])
            {
                case 1: chkAvailabilityAll.Checked = true; break;
                case 2: chkAvailabilityYes.Checked = true; break;
                case 3: chkAvailabilityNo.Checked = true; break;
                default: break;
            }
            switch (intFilterList[2])
            {
                case 0: chkSortCatalog.Checked = true; break;
                case 1: chkSortAlpha.Checked = true; break;
                case 2: chkSortPrice.Checked = true; break;
                default: break;
            }

            UpdateFilterLabelText();

            chkFilterCategoryEverything.Appearance = Appearance.Button;
            chkFilterCategoryEverything.Cursor = Cursors.Hand;
            chkFilterCategoryEverything.AutoSize = false;
            chkFilterCategoryEverything.Size = new Size(64, 64);
            chkFilterCategoryEverything.BackgroundImage = Properties.Resources.UI_Filter_All_False;
            chkFilterCategoryEverything.FlatStyle = FlatStyle.Flat;
            chkFilterCategoryEverything.FlatAppearance.BorderSize = 0;
            chkFilterCategoryEverything.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkFilterCategoryEverything.ForeColor = Color.FromArgb(255, 244, 239, 216);
            if (chkFilterCategoryEverything.Checked == true)
                chkFilterCategoryEverything.BackgroundImage = Properties.Resources.UI_Filter_All;

            chkFilterCategoryFurniture.Appearance = Appearance.Button;
            chkFilterCategoryFurniture.Cursor = Cursors.Hand;
            chkFilterCategoryFurniture.AutoSize = false;
            chkFilterCategoryFurniture.Size = new Size(64, 64);
            chkFilterCategoryFurniture.BackgroundImage = Properties.Resources.UI_Filter_Furniture_False;
            chkFilterCategoryFurniture.FlatStyle = FlatStyle.Flat;
            chkFilterCategoryFurniture.FlatAppearance.BorderSize = 0;
            chkFilterCategoryFurniture.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkFilterCategoryFurniture.ForeColor = Color.FromArgb(255, 244, 239, 216);
            if (chkFilterCategoryFurniture.Checked == true)
                chkFilterCategoryFurniture.BackgroundImage = Properties.Resources.UI_Filter_Furniture;

            chkFilterCategoryCarpet.Appearance = Appearance.Button;
            chkFilterCategoryCarpet.Cursor = Cursors.Hand;
            chkFilterCategoryCarpet.AutoSize = false;
            chkFilterCategoryCarpet.Size = new Size(64, 64);
            chkFilterCategoryCarpet.BackgroundImage = Properties.Resources.UI_Filter_Carpet_False;
            chkFilterCategoryCarpet.FlatStyle = FlatStyle.Flat;
            chkFilterCategoryCarpet.FlatAppearance.BorderSize = 0;
            chkFilterCategoryCarpet.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkFilterCategoryCarpet.ForeColor = Color.FromArgb(255, 244, 239, 216);
            if (chkFilterCategoryCarpet.Checked == true)
                chkFilterCategoryCarpet.BackgroundImage = Properties.Resources.UI_Filter_Carpet;

            chkFilterCategoryWallpaper.Appearance = Appearance.Button;
            chkFilterCategoryWallpaper.Cursor = Cursors.Hand;
            chkFilterCategoryWallpaper.AutoSize = false;
            chkFilterCategoryWallpaper.Size = new Size(64, 64);
            chkFilterCategoryWallpaper.BackgroundImage = Properties.Resources.UI_Filter_Wallpaper_False;
            chkFilterCategoryWallpaper.FlatStyle = FlatStyle.Flat;
            chkFilterCategoryWallpaper.FlatAppearance.BorderSize = 0;
            chkFilterCategoryWallpaper.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkFilterCategoryWallpaper.ForeColor = Color.FromArgb(255, 244, 239, 216);
            if (chkFilterCategoryWallpaper.Checked == true)
                chkFilterCategoryWallpaper.BackgroundImage = Properties.Resources.UI_Filter_Wallpaper;

            chkFilterCategoryClothing.Appearance = Appearance.Button;
            chkFilterCategoryClothing.Cursor = Cursors.Hand;
            chkFilterCategoryClothing.AutoSize = false;
            chkFilterCategoryClothing.Size = new Size(64, 64);
            chkFilterCategoryClothing.BackgroundImage = Properties.Resources.UI_Filter_Shirt_False;
            chkFilterCategoryClothing.FlatStyle = FlatStyle.Flat;
            chkFilterCategoryClothing.FlatAppearance.BorderSize = 0;
            chkFilterCategoryClothing.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkFilterCategoryClothing.ForeColor = Color.FromArgb(255, 244, 239, 216);
            if (chkFilterCategoryClothing.Checked == true)
                chkFilterCategoryClothing.BackgroundImage = Properties.Resources.UI_Filter_Shirt;

            chkFilterCategoryTools.Appearance = Appearance.Button;
            chkFilterCategoryTools.Cursor = Cursors.Hand;
            chkFilterCategoryTools.AutoSize = false;
            chkFilterCategoryTools.Size = new Size(64, 64);
            chkFilterCategoryTools.BackgroundImage = Properties.Resources.UI_Filter_Tool_False;
            chkFilterCategoryTools.FlatStyle = FlatStyle.Flat;
            chkFilterCategoryTools.FlatAppearance.BorderSize = 0;
            chkFilterCategoryTools.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkFilterCategoryTools.ForeColor = Color.FromArgb(255, 244, 239, 216);
            if (chkFilterCategoryTools.Checked == true)
                chkFilterCategoryTools.BackgroundImage = Properties.Resources.UI_Filter_Tool;

            chkFilterCategoryStationery.Appearance = Appearance.Button;
            chkFilterCategoryStationery.Cursor = Cursors.Hand;
            chkFilterCategoryStationery.AutoSize = false;
            chkFilterCategoryStationery.Size = new Size(64, 64);
            chkFilterCategoryStationery.BackgroundImage = Properties.Resources.UI_Filter_Stationery_False;
            chkFilterCategoryStationery.FlatStyle = FlatStyle.Flat;
            chkFilterCategoryStationery.FlatAppearance.BorderSize = 0;
            chkFilterCategoryStationery.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkFilterCategoryStationery.ForeColor = Color.FromArgb(255, 244, 239, 216);
            if (chkFilterCategoryStationery.Checked == true)
                chkFilterCategoryStationery.BackgroundImage = Properties.Resources.UI_Filter_Stationery;

            chkFilterCategoryGyroid.Appearance = Appearance.Button;
            chkFilterCategoryGyroid.Cursor = Cursors.Hand;
            chkFilterCategoryGyroid.AutoSize = false;
            chkFilterCategoryGyroid.Size = new Size(64, 64);
            chkFilterCategoryGyroid.BackgroundImage = Properties.Resources.UI_Filter_Gyroid_False;
            chkFilterCategoryGyroid.FlatStyle = FlatStyle.Flat;
            chkFilterCategoryGyroid.FlatAppearance.BorderSize = 0;
            chkFilterCategoryGyroid.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkFilterCategoryGyroid.ForeColor = Color.FromArgb(255, 244, 239, 216);
            if (chkFilterCategoryGyroid.Checked == true)
                chkFilterCategoryGyroid.BackgroundImage = Properties.Resources.UI_Filter_Gyroid;

            chkFilterCategoryFossil.Appearance = Appearance.Button;
            chkFilterCategoryFossil.Appearance = Appearance.Button;
            chkFilterCategoryFossil.Cursor = Cursors.Hand;
            chkFilterCategoryFossil.AutoSize = false;
            chkFilterCategoryFossil.Size = new Size(64, 64);
            chkFilterCategoryFossil.BackgroundImage = Properties.Resources.UI_Filter_Dinosaur_False;
            chkFilterCategoryFossil.FlatStyle = FlatStyle.Flat;
            chkFilterCategoryFossil.FlatAppearance.BorderSize = 0;
            chkFilterCategoryFossil.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkFilterCategoryFossil.ForeColor = Color.FromArgb(255, 244, 239, 216);
            if (chkFilterCategoryFossil.Checked == true)
                chkFilterCategoryFossil.BackgroundImage = Properties.Resources.UI_Filter_Dinosaur;

            chkFilterCategoryMusic.Appearance = Appearance.Button;
            chkFilterCategoryMusic.Cursor = Cursors.Hand;
            chkFilterCategoryMusic.AutoSize = false;
            chkFilterCategoryMusic.Size = new Size(64, 64);
            chkFilterCategoryMusic.BackgroundImage = Properties.Resources.UI_Filter_Music_False;
            chkFilterCategoryMusic.FlatStyle = FlatStyle.Flat;
            chkFilterCategoryMusic.FlatAppearance.BorderSize = 0;
            chkFilterCategoryMusic.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkFilterCategoryMusic.ForeColor = Color.FromArgb(255, 244, 239, 216);
            if (chkFilterCategoryMusic.Checked == true)
                chkFilterCategoryMusic.BackgroundImage = Properties.Resources.UI_Filter_Music;

            chkFilterCategoryMiscellaneous.Appearance = Appearance.Button;
            chkFilterCategoryMiscellaneous.Cursor = Cursors.Hand;
            chkFilterCategoryMiscellaneous.AutoSize = false;
            chkFilterCategoryMiscellaneous.Size = new Size(64, 64);
            chkFilterCategoryMiscellaneous.BackgroundImage = Properties.Resources.UI_Filter_Pitfall_False;
            chkFilterCategoryMiscellaneous.FlatStyle = FlatStyle.Flat;
            chkFilterCategoryMiscellaneous.FlatAppearance.BorderSize = 0;
            chkFilterCategoryMiscellaneous.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkFilterCategoryMiscellaneous.ForeColor = Color.FromArgb(255, 244, 239, 216);
            if (chkFilterCategoryMiscellaneous.Checked == true)
                chkFilterCategoryMiscellaneous.BackgroundImage = Properties.Resources.UI_Filter_Pitfall;

            chkFilterGroup1.Appearance = Appearance.Button;
            chkFilterGroup1.Cursor = Cursors.Hand;
            chkFilterGroup1.AutoSize = false;
            chkFilterGroup1.Size = new Size(64, 64);
            chkFilterGroup1.BackgroundImage = Properties.Resources.UI_Filter_BG_Null;
            chkFilterGroup1.FlatStyle = FlatStyle.Flat;
            chkFilterGroup1.FlatAppearance.BorderSize = 0;
            chkFilterGroup1.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkFilterGroup1.ForeColor = Color.FromArgb(255, 244, 239, 216);

            chkFilterGroup2.Appearance = Appearance.Button;
            chkFilterGroup2.Cursor = Cursors.Hand;
            chkFilterGroup2.AutoSize = false;
            chkFilterGroup2.Size = new Size(64, 64);
            chkFilterGroup2.BackgroundImage = Properties.Resources.UI_Filter_BG_Null;
            chkFilterGroup2.FlatStyle = FlatStyle.Flat;
            chkFilterGroup2.FlatAppearance.BorderSize = 0;
            chkFilterGroup2.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkFilterGroup2.ForeColor = Color.FromArgb(255, 244, 239, 216);

            chkFilterGroup3.Appearance = Appearance.Button;
            chkFilterGroup3.Cursor = Cursors.Hand;
            chkFilterGroup3.AutoSize = false;
            chkFilterGroup3.Size = new Size(64, 64);
            chkFilterGroup3.BackgroundImage = Properties.Resources.UI_Filter_BG_Null;
            chkFilterGroup3.FlatStyle = FlatStyle.Flat;
            chkFilterGroup3.FlatAppearance.BorderSize = 0;
            chkFilterGroup3.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkFilterGroup3.ForeColor = Color.FromArgb(255, 244, 239, 216);

            chkFilterGroup4.Appearance = Appearance.Button;
            chkFilterGroup4.Cursor = Cursors.Hand;
            chkFilterGroup4.AutoSize = false;
            chkFilterGroup4.Size = new Size(64, 64);
            chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_BG_Null;
            chkFilterGroup4.FlatStyle = FlatStyle.Flat;
            chkFilterGroup4.FlatAppearance.BorderSize = 0;
            chkFilterGroup4.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkFilterGroup4.ForeColor = Color.FromArgb(255, 244, 239, 216);

            chkFilterGroup5.Appearance = Appearance.Button;
            chkFilterGroup5.Cursor = Cursors.Hand;
            chkFilterGroup5.AutoSize = false;
            chkFilterGroup5.Size = new Size(64, 64);
            chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_BG_Null;
            chkFilterGroup5.FlatStyle = FlatStyle.Flat;
            chkFilterGroup5.FlatAppearance.BorderSize = 0;
            chkFilterGroup5.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkFilterGroup5.ForeColor = Color.FromArgb(255, 244, 239, 216);

            chkFilterGroup6.Appearance = Appearance.Button;
            chkFilterGroup6.Cursor = Cursors.Hand;
            chkFilterGroup6.AutoSize = false;
            chkFilterGroup6.Size = new Size(64, 64);
            chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_BG_Null;
            chkFilterGroup6.FlatStyle = FlatStyle.Flat;
            chkFilterGroup6.FlatAppearance.BorderSize = 0;
            chkFilterGroup6.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkFilterGroup6.ForeColor = Color.FromArgb(255, 244, 239, 216);

            chkAvailabilityAll.Appearance = Appearance.Button;
            chkAvailabilityAll.Cursor = Cursors.Hand;
            chkAvailabilityAll.AutoSize = false;
            chkAvailabilityAll.Size = new Size(64, 64);
            chkAvailabilityAll.BackgroundImage = Properties.Resources.UI_Filter_All_False;
            chkAvailabilityAll.FlatStyle = FlatStyle.Flat;
            chkAvailabilityAll.FlatAppearance.BorderSize = 0;
            chkAvailabilityAll.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkAvailabilityAll.ForeColor = Color.FromArgb(255, 244, 239, 216);
            if (chkAvailabilityAll.Checked == true)
                chkAvailabilityAll.BackgroundImage = Properties.Resources.UI_Filter_All;

            chkAvailabilityYes.Appearance = Appearance.Button;
            chkAvailabilityYes.Cursor = Cursors.Hand;
            chkAvailabilityYes.AutoSize = false;
            chkAvailabilityYes.Size = new Size(64, 64);
            chkAvailabilityYes.BackgroundImage = Properties.Resources.UI_Filter_Complete_False;
            chkAvailabilityYes.FlatStyle = FlatStyle.Flat;
            chkAvailabilityYes.FlatAppearance.BorderSize = 0;
            chkAvailabilityYes.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkAvailabilityYes.ForeColor = Color.FromArgb(255, 244, 239, 216);
            if (chkAvailabilityYes.Checked == true)
                chkAvailabilityYes.BackgroundImage = Properties.Resources.UI_Filter_Complete;

            chkAvailabilityNo.Appearance = Appearance.Button;
            chkAvailabilityNo.Cursor = Cursors.Hand;
            chkAvailabilityNo.AutoSize = false;
            chkAvailabilityNo.Size = new Size(64, 64);
            chkAvailabilityNo.BackgroundImage = Properties.Resources.UI_Filter_Incomplete_False;
            chkAvailabilityNo.FlatStyle = FlatStyle.Flat;
            chkAvailabilityNo.FlatAppearance.BorderSize = 0;
            chkAvailabilityNo.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkAvailabilityNo.ForeColor = Color.FromArgb(255, 244, 239, 216);
            if (chkAvailabilityNo.Checked == true)
                chkAvailabilityNo.BackgroundImage = Properties.Resources.UI_Filter_Incomplete;

            chkSortCatalog.Appearance = Appearance.Button;
            chkSortCatalog.Cursor = Cursors.Hand;
            chkSortCatalog.AutoSize = false;
            chkSortCatalog.Size = new Size(64, 64);
            chkSortCatalog.BackgroundImage = Properties.Resources.UI_Filter_Other_False;
            chkSortCatalog.FlatStyle = FlatStyle.Flat;
            chkSortCatalog.FlatAppearance.BorderSize = 0;
            chkSortCatalog.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkSortCatalog.ForeColor = Color.FromArgb(255, 244, 239, 216);
            if (chkSortCatalog.Checked == true)
                chkSortCatalog.BackgroundImage = Properties.Resources.UI_Filter_Other;

            chkSortAlpha.Appearance = Appearance.Button;
            chkSortAlpha.Cursor = Cursors.Hand;
            chkSortAlpha.AutoSize = false;
            chkSortAlpha.Size = new Size(64, 64);
            chkSortAlpha.BackgroundImage = Properties.Resources.UI_Filter_Alphabetized_False;
            chkSortAlpha.FlatStyle = FlatStyle.Flat;
            chkSortAlpha.FlatAppearance.BorderSize = 0;
            chkSortAlpha.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkSortAlpha.ForeColor = Color.FromArgb(255, 244, 239, 216);
            if (chkSortAlpha.Checked == true)
                chkSortAlpha.BackgroundImage = Properties.Resources.UI_Filter_Alphabetized;

            chkSortPrice.Appearance = Appearance.Button;
            chkSortPrice.Cursor = Cursors.Hand;
            chkSortPrice.AutoSize = false;
            chkSortPrice.Size = new Size(64, 64);
            chkSortPrice.BackgroundImage = Properties.Resources.UI_Filter_Price_False;
            chkSortPrice.FlatStyle = FlatStyle.Flat;
            chkSortPrice.FlatAppearance.BorderSize = 0;
            chkSortPrice.BackColor = Color.FromArgb(255, 244, 239, 216);
            chkSortPrice.ForeColor = Color.FromArgb(255, 244, 239, 216);
            if (chkSortPrice.Checked == true)
                chkSortPrice.BackgroundImage = Properties.Resources.UI_Filter_Price;


            if (chkFilterCategoryEverything.Checked == false && chkFilterCategoryFurniture.Checked == false && chkFilterCategoryCarpet.Checked == false &&
                chkFilterCategoryWallpaper.Checked == false && chkFilterCategoryClothing.Checked == false && chkFilterCategoryTools.Checked == false &&
                chkFilterCategoryStationery.Checked == false && chkFilterCategoryGyroid.Checked == false && chkFilterCategoryFossil.Checked == false &&
                chkFilterCategoryMusic.Checked == false && chkFilterCategoryMiscellaneous.Checked == false)
                chkFilterCategoryEverything.Checked = true;

            if (intFilterList[9] < 2) intFilterList[9] = 2;
            if (intFilterList[10] < 0) intFilterList[10] = 0;
            if (intFilterList[11] < 2) intFilterList[11] = 2;
            if (intFilterList[12] < 2) intFilterList[12] = 2;
            if (intFilterList[13] < 1) intFilterList[13] = 1;
            if (intFilterList[14] < 0) intFilterList[14] = 0;
            if (intFilterList[15] < 2) intFilterList[15] = 2;
            if (intFilterList[16] < 1) intFilterList[16] = 1;
            if (intFilterList[17] < 3) intFilterList[17] = 3;
            if (intFilterList[18] < 3) intFilterList[18] = 3;
            if (intFilterList[19] < 0) intFilterList[19] = 0;

            UpdateFilterGroupCheckboxes();

            if (chkAvailabilityAll.Checked == false && chkAvailabilityYes.Checked == false && chkAvailabilityNo.Checked == false)
                chkAvailabilityYes.Checked = true;

            if (chkSortCatalog.Checked == false && chkSortAlpha.Checked == false && chkSortPrice.Checked == false)
                chkSortCatalog.Checked = true;

            imgFilterGroupHide.BringToFront();
        }

        private void CreateListView()
        {
            lstShop.Columns.Add("Description of Items", -2, HorizontalAlignment.Left);
            lstShop.Columns.Add("Amount Price", 150, HorizontalAlignment.Right);
            lstShop.Columns[0].Width = lstShop.Width - lstShop.Columns[1].Width - SystemInformation.VerticalScrollBarWidth;
            lstShop.HeaderStyle = ColumnHeaderStyle.None;
            lstShop.View = View.Details;
            lstShop.FullRowSelect = true;
            //lstShop.VirtualMode = true;

            imgList.ImageSize = new Size(32, 32);
            imgList.ColorDepth = ColorDepth.Depth32Bit;

            imgList.Images.Add(Properties.Resources.IC_Furniture_PG_Sprite_Upscaled);
            imgList.Images.Add(Properties.Resources.IC_Carpet_PG_Inv_Icon_Upscaled);
            imgList.Images.Add(Properties.Resources.IC_Wallpaper_PG_Model);
            imgList.Images.Add(Properties.Resources.IC_Clothing_PG_Inv_Icon_Upscaled);
            imgList.Images.Add(Properties.Resources.IC_Tool);
            imgList.Images.Add(Properties.Resources.IC_Stationery_PG_Sprite_Upscaled);
            imgList.Images.Add(Properties.Resources.IC_Gyroid_PG_Sprite_Upscaled);
            imgList.Images.Add(Properties.Resources.IC_Fossil_PG_Inv_Icon_Upscaled);
            imgList.Images.Add(Properties.Resources.IC_Aircheck_PG_Sprite_Upscaled);
            imgList.Images.Add(Properties.Resources.IC_Item_Bag_PG_Sprite_Upscaled);
            imgList.Images.Add(Properties.Resources.IC_Umbrella_PG_Sprite_Upscaled);
            imgList.Images.Add(Properties.Resources.IC_Fan_PG_Sprite_Upscaled);
            imgList.Images.Add(Properties.Resources.IC_Pinwheel_PG_Sprite_Upscaled);
            imgList.Images.Add(Properties.Resources.IC_Red_Balloon_PG_Model);

            lstShop.LargeImageList = imgList;
            lstShop.SmallImageList = imgList;
        }

        private void InitializeShopListView()
        {
            listShopItemListViewItem = new(intItemsTotal); // allocate list view items
            filteredList = new(intItemsTotal); // allocate filter list space

            for (int i = 1; i < intItemsTotal + 1; i++)
            {
                Tuple<ItemInfo, int> itemCodeData = ItemData.GetItemCheckByCatalogIndex(i, out int _);

                int itemIcon = itemCodeData.Item1.ItemType switch
                {
                    "Furniture" => 0,
                    "Carpet" => 1,
                    "Wallpaper" => 2,
                    "Clothing" => 3,
                    "Tool" => itemCodeData.Item1.ItemCategory switch
                    {
                        "Tools" => 4,
                        "Umbrellas" => 10,
                        "Fans" => 11,
                        "Pinwheels" => 12,
                        "Balloons" => 13,
                        _ => 4,
                    },
                    "Stationery" => 5,
                    "Gyroid" => 6,
                    "Fossil" => 7,
                    "Music" => 8,
                    "Miscellaneous" => 9,
                    _ => 0,
                };

                //TODO: Unlock bool
                bool boolAvailable = CheckShopAvailability(itemCodeData);
                bool boolUnlocked = CheckItemUnlock(itemCodeData);

                ListViewItem item = new()
                {
                    ForeColor = GetListViewItemColor(boolAvailable, boolUnlocked, intBoughtList[itemCodeData.Item1.CatalogIndex]),
                    Text = " " + itemCodeData.Item1.Name,
                    ImageIndex = itemIcon,
                    Tag = i
                };

                item.SubItems.Add(itemCodeData.Item1.Tickets.ToString("N0") + " "); // Add ticket cost text
                listShopItemListViewItem.Add(item); // Add item to all items list
            }
        }

        private void lstShop_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (filteredList != null && e.ItemIndex < filteredList.Count)
            {
                e.Item = filteredList[e.ItemIndex]; // pull the item from our filtered list
            }
            else
            {
                e.Item = emptyViewItem;
            }
        }

        private void lstShop_UpdateItemStatus(int i)
        {
            Tuple<ItemInfo, int> itemCodeData = ItemData.GetItemCheckByIndex(i - 1);

            bool boolAvailable = CheckShopAvailability(itemCodeData);
            bool boolUnlocked = CheckItemUnlock(itemCodeData);

            ListViewItem item = listShopItemListViewItem[i - 1];
            item.ForeColor = GetListViewItemColor(boolAvailable, boolUnlocked, intBoughtList[itemCodeData.Item1.CatalogIndex]);
        }

        private void lstShop_UpdateFilterAndSort(string filter = "")
        {
            lstShop.BeginUpdate();
            filteredList = new();
            lstShop.VirtualListSize = 0;
            lstShop.Refresh();
            lstShop.EndUpdate();

            filteredList = new(listShopItemListViewItem.Count); // allocate list

            if (!string.IsNullOrWhiteSpace(filter))
            {
                filter = filter.Trim().ToLower();
                for (int i = 1; i < intItemsTotal + 1; i++)
                {
                    int idx;

                    /* Go through our items in current shop sort mode */
                    Tuple<ItemInfo, int> itemCodeData = intFilterList[2] switch
                    {
                        0 => ItemData.GetItemCheckByCatalogIndex(i, out idx),
                        1 => ItemData.GetItemCheckByAlphaIndex(i, out idx),
                        2 => ItemData.GetItemCheckByTicketIndex(i, out idx),
                        _ => ItemData.GetItemCheckByCatalogIndex(i, out idx),
                    };

                    if (idx != -1 && itemCodeData.Item1.Name.ToLower().Contains(filter) && CheckShopFilters(itemCodeData) == true)
                    {
                        bool boolAvailable = CheckShopAvailability(itemCodeData);
                        bool boolUnlocked = CheckItemUnlock(itemCodeData);
                        if (intFilterList[1] == 1 || intFilterList[1] == 2 && boolUnlocked == true && boolAvailable == true || intFilterList[1] == 3 && boolUnlocked == false || intFilterList[1] == 3 && boolAvailable == false)
                        {
                            filteredList.Add(listShopItemListViewItem[idx]); // idx is the item index in our program
                        }
                    }
                }
            }
            else // filter is empty, just update sort
            {
                for (int i = 1; i < intItemsTotal + 1; i++)
                {
                    int idx;

                    /* Go through our items in current shop sort mode */
                    Tuple<ItemInfo, int> itemCodeData = intFilterList[2] switch
                    {
                        0 => ItemData.GetItemCheckByCatalogIndex(i, out idx),
                        1 => ItemData.GetItemCheckByAlphaIndex(i, out idx),
                        2 => ItemData.GetItemCheckByTicketIndex(i, out idx),
                        _ => ItemData.GetItemCheckByCatalogIndex(i, out idx),
                    };

                    if (CheckShopFilters(itemCodeData) == true)
                    {
                        bool boolAvailable = CheckShopAvailability(itemCodeData);
                        bool boolUnlocked = CheckItemUnlock(itemCodeData);
                        if (intFilterList[1] == 1 || intFilterList[1] == 2 && boolUnlocked == true && boolAvailable == true || intFilterList[1] == 3 && boolUnlocked == false || intFilterList[1] == 3 && boolAvailable == false)
                        {
                            filteredList.Add(listShopItemListViewItem[idx]);
                        }
                    }
                }
            }

            lstShop.BeginUpdate();

            for (int i = 1; i < intItemsTotal + 1; i++)
            {
                lstShop_UpdateItemStatus(i);
            }

            if (previousListViewItem != null)
            {
                previousListViewItem.ForeColor = GetListViewItemColor(CheckShopAvailability(previousItem), CheckItemUnlock(previousItem), intBoughtList[previousItem.Item1.CatalogIndex]);
                previousListViewItem = null;
                previousItem = null;
            }

            boolItemSelected = false;
            lviSelectedItem = null;
            lstShop.FocusedItem = null;
            //for (int i = lstShop.SelectedIndices.Count - 1; i >= 0; i--)
            //{
            //    lstShop.SelectedItems[lstShop.SelectedIndices[i]].Selected = false;
            //}

            lstShop.VirtualListSize = filteredList.Count;
            lstShop.Refresh();
            //lstShop.Items.Clear();
            //lstShop.Items.AddRange(filteredList.ToArray());
            lstShop.EndUpdate();
        }

        private bool CheckShopFilters(Tuple<ItemInfo, int> itemCodeData)
        {
            bool boolPass = false;
            string strItemType = "";
            string strItemCategory = "";

            switch (intFilterList[0])
            {
                case 0:
                    strItemType = "Everything";
                    switch (intFilterList[9])
                    {
                        case 0: strItemCategory = ""; break;
                        case 1: strItemCategory = ""; break;
                        case 2: strItemCategory = "Everything"; break;
                        case 3: strItemCategory = "Nook"; break;
                        case 4: strItemCategory = "Seasonal"; break;
                        case 5: strItemCategory = "Other"; break;
                        default: break;
                    }
                    break;
                case 1:
                    strItemType = "Furniture";
                    switch (intFilterList[10])
                    {
                        case 0: strItemCategory = "Everything"; break;
                        case 1: strItemCategory = "Nook"; break;
                        case 2: strItemCategory = "Crazy Redd"; break;
                        case 3: strItemCategory = "Raffle"; break;
                        case 4: strItemCategory = "Seasonal"; break;
                        case 5: strItemCategory = "Other"; break;
                        default: break;
                    }
                    break;
                case 2:
                    strItemType = "Carpet";
                    switch (intFilterList[11])
                    {
                        case 0: strItemCategory = ""; break;
                        case 1: strItemCategory = ""; break;
                        case 2: strItemCategory = "Everything"; break;
                        case 3: strItemCategory = "Nook"; break;
                        case 4: strItemCategory = "Saharah"; break;
                        case 5: strItemCategory = "Seasonal"; break;
                        default: break;
                    }
                    break;
                case 3:
                    strItemType = "Wallpaper";
                    switch (intFilterList[12])
                    {
                        case 0: strItemCategory = ""; break;
                        case 1: strItemCategory = ""; break;
                        case 2: strItemCategory = "Everything"; break;
                        case 3: strItemCategory = "Nook"; break;
                        case 4: strItemCategory = "Wendell"; break;
                        case 5: strItemCategory = "Seasonal"; break;
                        default: break;
                    }
                    break;
                case 4:
                    strItemType = "Clothing";
                    switch (intFilterList[13])
                    {
                        case 0: strItemCategory = ""; break;
                        case 1: strItemCategory = "Everything"; break;
                        case 2: strItemCategory = "Nook"; break;
                        case 3: strItemCategory = "Gracie"; break;
                        case 4: strItemCategory = "Seasonal"; break;
                        case 5: strItemCategory = "Beta"; break;
                        default: break;
                    }
                    break;
                case 5:
                    strItemType = "Tool";
                    switch (intFilterList[14])
                    {
                        case 0: strItemCategory = "Everything"; break;
                        case 1: strItemCategory = "Tools"; break;
                        case 2: strItemCategory = "Umbrellas"; break;
                        case 3: strItemCategory = "Fans"; break;
                        case 4: strItemCategory = "Pinwheels"; break;
                        case 5: strItemCategory = "Balloons"; break;
                        default: break;
                    }
                    break;
                case 6:
                    strItemType = "Stationery";
                    switch (intFilterList[15])
                    {
                        case 0: strItemCategory = ""; break;
                        case 1: strItemCategory = ""; break;
                        case 2: strItemCategory = "Everything"; break;
                        case 3: strItemCategory = "Nook"; break;
                        case 4: strItemCategory = "Seasonal"; break;
                        case 5: strItemCategory = "Other"; break;
                        default: break;
                    }
                    break;
                case 7:
                    strItemType = "Gyroid";
                    switch (intFilterList[16])
                    {
                        case 0: strItemCategory = ""; break;
                        case 1: strItemCategory = "Everything"; break;
                        case 2: strItemCategory = "Mini Gyroids"; break;
                        case 3: strItemCategory = "Gyroids"; break;
                        case 4: strItemCategory = "Mega Gyroids"; break;
                        case 5: strItemCategory = "Tall Gyroids"; break;
                        default: break;
                    }
                    break;
                case 8:
                    strItemType = "Fossil";
                    switch (intFilterList[17])
                    {
                        case 0: strItemCategory = ""; break;
                        case 1: strItemCategory = ""; break;
                        case 2: strItemCategory = ""; break;
                        case 3: strItemCategory = "Everything"; break;
                        case 4: strItemCategory = "Common"; break;
                        case 5: strItemCategory = "Dinosaurs"; break;
                        default: break;
                    }
                    break;
                case 9:
                    strItemType = "Music";
                    switch (intFilterList[18])
                    {
                        case 0: strItemCategory = ""; break;
                        case 1: strItemCategory = ""; break;
                        case 2: strItemCategory = ""; break;
                        case 3: strItemCategory = "Everything"; break;
                        case 4: strItemCategory = "Common"; break;
                        case 5: strItemCategory = "Secret"; break;
                        default: break;
                    }
                    break;
                case 10:
                    strItemType = "Miscellaneous";
                    switch (intFilterList[19])
                    {
                        case 0: strItemCategory = "Everything"; break;
                        case 1: strItemCategory = "Bells"; break;
                        case 2: strItemCategory = "Nature"; break;
                        case 3: strItemCategory = "Seasonal"; break;
                        case 4: strItemCategory = "Other"; break;
                        case 5: strItemCategory = "Beta"; break;
                        default: break;
                    }
                    break;
                default: break;
            }

            if (strItemType == "Everything" && strItemCategory == "Everything" || strItemType == "Everything" && strItemCategory == itemCodeData.Item1.ItemCategory ||
                strItemType == "Everything" && strItemCategory == "Other" && itemCodeData.Item1.ItemCategory != "Nook" && itemCodeData.Item1.ItemCategory != "Seasonal" ||
                itemCodeData.Item1.ItemType == strItemType && strItemCategory == "Everything" ||
                itemCodeData.Item1.ItemType == strItemType && itemCodeData.Item1.ItemCategory == strItemCategory)
                boolPass = true;

            return boolPass;
        }

        private bool CheckShopAvailability(Tuple<ItemInfo, int> itemCodeData)
        {
            bool boolPass = false;
            DateTime now = DateTime.Now + timeOffset;
            int intMonth = itemCodeData.Item1.Month;
            int intDay = itemCodeData.Item1.Day;
            int intBirthdayMonth = 0;
            if (boolBirthdaySet == true)
                intBirthdayMonth = birthday.Month;

            switch (itemCodeData.Item1.Availability)
            {
                case 0: boolPass = true; break;
                case 1: if (now.Month == intMonth && now.Day >= intDay && now.Day < intDay + 7 || now.Month == intMonth + 1 && intDay == 31 && now.Day < 7) boolPass = true; break;
                case 2: if (now.Month == intMonth && now.Day >= intDay - 3 && now.Day <= intDay + 3 || now.Month == intMonth + 1 && intDay == 31 && now.Day <= 3) boolPass = true; break;
                case 3: if (now.Month == intBirthdayMonth) boolPass = true; break;
                case 4: if (now.Month == intMonth) boolPass = true; break;
                case 5: if (now.Month == intMonth && now.Day <= intDay || now.Month == intMonth - 1) boolPass = true; break;
                case 6: if (now.Month == intMonth && now.Day >= intDay || now.Month == intMonth + 1) boolPass = true; break;
                case 7: if (now.Month >= intMonth && now.Month <= intMonth + 2) boolPass = true; break;
                case 8:
                    if (intMonth != 11 && now.Month == intMonth && now.Day >= intDay || now.Month != 11 && now.Month > intMonth && now.Month <= intMonth + 2 ||
                        intMonth == 11 && now.Month == 11 && now.Day >= intDay || intMonth == 11 && now.Month > 11 || intMonth == 11 && now.Month <= 2) boolPass = true; break;
                default: boolPass = false; break;
            }

            return boolPass;
        }

        private bool CheckItemUnlock(Tuple<ItemInfo, int> itemCodeData)
        {
            bool boolPass = true;

            switch (itemCodeData.Item1.UnlockReq)
            {
                case 1: if (intBoughtUnique[11] < 50) boolPass = false; break;
                case 2: if (intBoughtUnique[11] < 100) boolPass = false; break;
                case 3: if (intBoughtUnique[12] < 20) boolPass = false; break;
                case 4: if (intBoughtUnique[13] < 10) boolPass = false; break;
                case 5: if (intBoughtUnique[14] < 10) boolPass = false; break;
                case 6: if (intBoughtUnique[6] < 30) boolPass = false; break;
                case 7: if (intBoughtUnique[3] < 50) boolPass = false; break;
                case 8: if (intBoughtUnique[1] < 20) boolPass = false; break;
                case 9: if (intBoughtUnique[2] < 20) boolPass = false; break;
                case 10: if (intTasksComplete[109] < 1) boolPass = false; break;
                case 11: if (intBoughtUnique[8] < 20) boolPass = false; break;
                case 12: if (intBoughtUnique[15] < 10) boolPass = false; break;
                case 13: if (intBoughtUnique[5] < 60) boolPass = false; break;
                case 14: if (intBoughtUnique[4] < 60) boolPass = false; break;
                case 15: if (intBoughtUnique[0] < 500) boolPass = false; break;
                case 16: if (intBoughtUnique[0] < 600) boolPass = false; break;
                case 17: if (intBoughtUnique[17] < 1372) boolPass = false; break;
                default: break;
            }

            return boolPass;
        }

        private bool CheckNESAvailability(Tuple<ItemInfo, int> itemCodeData)
        {
            bool boolPass = true;

            if (itemCodeData.Item1.ID >= intNESStart && itemCodeData.Item1.ID < intNESEnd && strNESinQueue != "")
                boolPass = false;

            return boolPass;
        }

        private void IncrementItemsBought(Tuple<ItemInfo, int> itemCodeData)
        {
            if (intBoughtList[itemCodeData.Item1.CatalogIndex] < 1)
            {
                intBoughtUnique[17]++;

                if (itemCodeData.Item1.ItemCategory == "Beta")
                    intBoughtUnique[10]++;

                switch (itemCodeData.Item1.ItemType)
                {
                    case "Carpet": intBoughtUnique[1]++; break;
                    case "Wallpaper": intBoughtUnique[2]++; break;
                    case "Clothing": intBoughtUnique[3]++; break;
                    case "Tool": intBoughtUnique[4]++; break;
                    case "Stationery": intBoughtUnique[5]++; break;
                    case "Gyroid": intBoughtUnique[6]++; break;
                    case "Fossil": intBoughtUnique[7]++; break;
                    case "Music": intBoughtUnique[8]++; break;
                    case "Miscellaneous": intBoughtUnique[9]++; break;

                    case "Furniture":
                        intBoughtUnique[0]++;

                        if (itemCodeData.Item1.ItemCategory != "Seasonal")
                            intBoughtUnique[16]++;

                        switch (itemCodeData.Item1.ItemCategory)
                        {
                            case "Nook": intBoughtUnique[11]++; break;
                            case "Crazy Redd": intBoughtUnique[12]++; break;
                            case "Raffle": intBoughtUnique[13]++; break;
                            default: break;
                        }

                        switch (itemCodeData.Item1.Name)
                        {
                            case "Arc De Triomphe":
                            case "Chinese Lion":
                            case "Chinese Lioness":
                            case "Compass":
                            case "Fishing Bear":
                            case "Lady Liberty":
                            case "Manekin Pis":
                            case "Matryoshka":
                            case "Merlion":
                            case "Mermaid Statue":
                            case "Moai Statue":
                            case "Mouth of Truth":
                            case "Pagoda":
                            case "Plate Armor":
                            case "Shogi Piece":
                            case "Stone Coin":
                            case "Tiger Bobblehead":
                            case "Tokyo Tower":
                            case "Tower of Pisa":
                            case "Tribal Mask": intBoughtUnique[14]++; break;
                            case "Balloon Fight":
                            case "Clu Clu Land":
                            case "Golf":
                            case "Pinball":
                            case "DK Jr Math":
                            case "Donkey Kong":
                            case "Excitebike":
                            case "Tennis":
                            case "Wario's Woods":
                            case "Baseball":
                            case "Soccer":
                            case "Clu Clu Land D":
                            case "Donkey Kong 3":
                            case "Donkey Kong Jr":
                            case "Punchout": intBoughtUnique[15]++; break;
                            default: break;
                        }
                        break;
                }
            }

            intBoughtList[itemCodeData.Item1.CatalogIndex]++;
            intBoughtTotal++;
            intTicketsCurrent -= itemCodeData.Item1.Tickets;
            intTicketsSpent += itemCodeData.Item1.Tickets;

            if (itemCodeData.Item1.ID < intNESStart || itemCodeData.Item1.ID >= intNESEnd)
                AddPasswordToArray(itemCodeData);
        }

        private void AddPasswordToArray(Tuple<ItemInfo, int> itemCodeData)
        {
            string password = GeneratePassword(ItemData.GetItemCheckByName(itemCodeData.Item1.Name, out int _));
            strCodeList[itemCodeData.Item1.CatalogIndex] = password;
        }

        private Color GetListViewItemColor(bool boolAvailable, bool boolUnlocked, int intTimesBought)
        {
            Color color = Color.Black;
            if (boolAvailable == true && intTimesBought == 0)
                color = Color.Black;
            if (boolAvailable == true && intTimesBought > 0)
                color = Color.FromArgb(255, 92, 106, 194);
            if ((boolAvailable == false && intTimesBought == 0) || (boolUnlocked == false && intTimesBought == 0))
                color = Color.LightGray;
            if ((boolAvailable == false && intTimesBought > 0) || (boolUnlocked == false && intTimesBought > 0))
                color = Color.FromArgb(255, 129, 150, 194);

            return color;
        }

        private void UpdateShopDialogLabelText(int intDialogOption, string itemName, int itemTickets)
        {
            imgShopDialog.Visible = true;

            lblFengShuiColor.Location = imgShopDialog.PointToClient(lblFengShuiColor.Parent.PointToScreen(lblFengShuiColor.Location));
            lblFengShuiColor.Parent = imgShopDialog;
            lblDialogTimmy.Location = imgShopDialog.PointToClient(lblDialogTimmy.Parent.PointToScreen(lblDialogTimmy.Location));
            lblDialogTimmy.Parent = imgShopDialog;
            lblDialogTommy.Location = lblDialogTimmy.PointToClient(lblDialogTommy.Parent.PointToScreen(lblDialogTommy.Location));
            lblDialogTommy.Parent = lblDialogTimmy;

            lblDialogTimmy.Visible = true;
            lblDialogTommy.Visible = true;

            if (boolItemSelected == false && intDialogOption == 0)
            {
                lblDialogTimmy.Text = "Welcome to the Ticket Exchange Center!\r\nYou may filter your selection up above.\r\nPlease have a look around!";
                lblDialogTommy.Text = "                         … around!";
            }

            if (boolItemSelected == true && intDialogOption == 1)
            {
                imgDialogSelect.Location = this.PointToClient(imgDialogSelect.Parent.PointToScreen(imgDialogSelect.Location));
                imgDialogSelect.Parent = this;
                imgDialogSelect.BackColor = Color.Transparent;
                pntDialogYes = imgDialogSelect.PointToClient(btnDialogSelectYes.Parent.PointToScreen(btnDialogSelectYes.Location));
                btnDialogSelectYes.Location = pntDialogYes;
                btnDialogSelectYes.Parent = imgDialogSelect;
                btnDialogSelectNo.Location = imgDialogSelect.PointToClient(btnDialogSelectNo.Parent.PointToScreen(btnDialogSelectNo.Location));
                btnDialogSelectNo.Parent = imgDialogSelect;

                imgDialogSelect.Visible = true;
                imgShopItem.BackgroundImage = null;
                btnDialogSelectYes.Visible = true;
                btnDialogSelectNo.Visible = true;

                lblDialogTimmy.Text = String.Format("That's a fine {0}!\r\nThe total comes to {1} Tickets.\r\nWhat do you say?", itemName, itemTickets.ToString("N0"));
                lblDialogTommy.Text = "       … say?";
            }

            if (boolItemSelected == false && intDialogOption == 2)
            {
                intLastViewedDialog = 4;
                lblDialogTimmy.Text = "We're sorry, that item isn't in stock at\r\nthe moment. Please check below when\r\nit will be available again!";
                lblDialogTommy.Text = "                       … again!";
            }

            if (boolItemSelected == false && intDialogOption == 3)
            {
                lblDialogTimmy.Text = "Thank you for your patronage!\r\nPlease let us know if you see anything\r\nelse you'd like!";
                lblDialogTommy.Text = "… like!";
            }

            if (boolItemSelected == false && intDialogOption == 4)
            {
                lblDialogTimmy.Text = "Feel free to browse at your leisure!\r\nPlease let us know if you see anything\r\nelse you'd like!";
                lblDialogTommy.Text = "… like!";
            }

            if (boolItemSelected == true && intDialogOption == 5)
            {
                imgDialogSelect.Location = this.PointToClient(imgDialogSelect.Parent.PointToScreen(imgDialogSelect.Location));
                imgDialogSelect.Parent = this;
                imgDialogSelect.BackColor = Color.Transparent;
                pntDialogYes = imgDialogSelect.PointToClient(btnDialogSelectYes.Parent.PointToScreen(btnDialogSelectYes.Location));
                btnDialogSelectYes.Location = pntDialogYes;
                btnDialogSelectYes.Parent = imgDialogSelect;
                btnDialogSelectNo.Location = imgDialogSelect.PointToClient(btnDialogSelectNo.Parent.PointToScreen(btnDialogSelectNo.Location));
                btnDialogSelectNo.Parent = imgDialogSelect;

                imgDialogSelect.Visible = true;
                imgShopItem.BackgroundImage = null;
                btnDialogSelectYes.Visible = true;
                btnDialogSelectNo.Visible = true;

                lblDialogTimmy.Text = String.Format("Ah yes, {0}! For {1}\r\nTickets, you may enter an NES contest.\r\nWhat do you say?", itemName, itemTickets.ToString("N0"));
                lblDialogTommy.Text = "       … say?";
            }

            if (boolItemSelected == false && intDialogOption == 6)
            {
                lblDialogTimmy.Text = "Thank you for entering the NES contest!\r\nPlease check back tomorrow to receive\r\nyour NES Game prize!";
                lblDialogTommy.Text = "               … prize!";
            }

            if (boolItemSelected == false && intDialogOption == 7)
            {
                lblDialogTimmy.Text = "We're sorry, you may only enter your\r\nname in an NES contest once per day.\r\nPlease try again later!";
                lblDialogTommy.Text = "                 … later!";
            }

            if (boolItemSelected == true && intDialogOption == 8)
            {
                boolNESDialog = true;

                lstShop.Visible = false;
                imgSpecialDelivery.Visible = true;
                imgDialogSelect.Location = this.PointToClient(imgDialogSelect.Parent.PointToScreen(imgDialogSelect.Location));
                imgDialogSelect.Parent = this;
                imgDialogSelect.BackColor = Color.Transparent;
                pntDialogYes = imgDialogSelect.PointToClient(btnDialogSelectYes.Parent.PointToScreen(btnDialogSelectYes.Location));
                btnDialogSelectYes.Location = new Point(70, 50);
                btnDialogSelectYes.Parent = imgDialogSelect;

                imgDialogSelect.Visible = true;
                imgShopItem.BackgroundImage = null;
                btnDialogSelectYes.Visible = true;
                btnDialogSelectYes.Text = "Okay!";

                lblDialogTimmy.Text = String.Format("Delivery! Your {0} has arrived!\r\nPlease check your catalog for delivery\r\ninstructions. Thank you!", itemName);
                lblDialogTommy.Text = "                     … you!";
            }

            if (boolItemSelected == false && intDialogOption == 9)
            {
                lblDialogTimmy.Text = "Thank you for your patronage!\r\nPlease check your catalog for delivery\r\ninstructions. Thank you!";
                lblDialogTommy.Text = "                     … you!";
            }

            if (boolItemSelected == false && intDialogOption == 10)
            {
                intLastViewedDialog = 4;
                lblDialogTimmy.Text = "Oh, no! You don't have enough Tickets\r\nfor this item. We're terribly sorry,\r\nplease try again later!";
                lblDialogTommy.Text = "                … later!";
            }
        }

        private string GetAvailabilityText(Tuple<ItemInfo, int> itemCodeData)
        {
            //TODO: Unlock requirement text
            string strAvailability = "";
            DateTime now = DateTime.Now + timeOffset;
            int intMonth = itemCodeData.Item1.Month;
            int intMonth2 = itemCodeData.Item1.Month;
            int intDay = itemCodeData.Item1.Day;
            int intDay2 = 0;
            string strDayAppend = "";
            string strDayAppend2 = "";


            switch (itemCodeData.Item1.Availability)
            {
                case 0: break;
                case 1:
                    if (intDay == 31)
                    {
                        intDay2 = 6;
                        intMonth2 = intMonth + 1;
                    }
                    else
                        intDay2 = intDay + 6;
                    break;
                case 2:
                    if (intDay == 31)
                    {
                        intDay2 = 3;
                        intDay -= 3;
                        intMonth2 = intMonth + 1;
                    }
                    else
                    {
                        intDay2 = intDay + 3;
                        intDay -= 3;
                    }
                    break;
                case 3:
                    if (boolBirthdaySet == true) { intDay = 1; intDay2 = DateTime.DaysInMonth(now.Year, now.Month); intMonth = birthday.Month; intMonth2 = birthday.Month; }
                    else { intDay = 1; intDay2 = 31; intMonth = 1; intMonth2 = 12; }
                    break;
                case 4: intDay = 1; intDay2 = DateTime.DaysInMonth(now.Year, intMonth); break;
                case 5: intDay2 = intDay; intMonth2 = intMonth; intDay = 1; intMonth -= 1; break;
                case 6: intDay2 = DateTime.DaysInMonth(now.Year, intMonth + 1); intMonth2 = intMonth + 1; break;
                case 7: intDay = 1; intDay2 = DateTime.DaysInMonth(now.Year, intMonth + 2); intMonth2 = intMonth + 2; break;
                case 8:
                    if (intMonth == 11) { intDay2 = DateTime.DaysInMonth(now.Year, 2); intMonth2 = 2; }
                    else { intDay2 = DateTime.DaysInMonth(now.Year, intMonth + 3); intMonth2 = intMonth + 3; }
                    break;
                default: break;
            }

            switch (intDay)
            {
                case 1: case 21: case 31: strDayAppend = "st"; break;
                case 2: case 22: strDayAppend = "nd"; break;
                case 3: case 23: strDayAppend = "rd"; break;
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                case 30: strDayAppend = "th"; break;
                default: break;
            }
            switch (intDay2)
            {
                case 1: case 21: case 31: strDayAppend2 = "st"; break;
                case 2: case 22: strDayAppend2 = "nd"; break;
                case 3: case 23: strDayAppend2 = "rd"; break;
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                case 30: strDayAppend2 = "th"; break;
                default: break;
            }

            if (itemCodeData.Item1.Availability == 0)
                strAvailability = "Available:  Any Time";
            else
                strAvailability = String.Format("Available:  {0} {1}{2} - {3} {4}{5}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(intMonth), intDay, strDayAppend,
                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(intMonth2), intDay2, strDayAppend2);
            //if (CheckItemUnlock(itemCodeData).Item1 == false)
            if (itemCodeData.Item1.UnlockReq > 0)
            {
                int intUnlockAmount = 0;
                int intUnlockHave = 0;
                string strUnlockText = "";
                switch (itemCodeData.Item1.UnlockReq)
                {
                    case 1: intUnlockAmount = 50; intUnlockHave = intBoughtUnique[11]; strUnlockText = "50 unique Nook Furniture Codes"; break;
                    case 2: intUnlockAmount = 100; intUnlockHave = intBoughtUnique[11]; strUnlockText = "100 unique Nook Furniture Codes"; break;
                    case 3: intUnlockAmount = 20; intUnlockHave = intBoughtUnique[12]; strUnlockText = "20 unique Crazy Redd Item Codes"; break;
                    case 4: intUnlockAmount = 10; intUnlockHave = intBoughtUnique[13]; strUnlockText = "10 unique Raffle Item Codes"; break;
                    case 5: intUnlockAmount = 10; intUnlockHave = intBoughtUnique[14]; strUnlockText = "10 unique Gulliver Item Codes"; break;
                    case 6: intUnlockAmount = 30; intUnlockHave = intBoughtUnique[6]; strUnlockText = "30 unique Gyroid Codes"; break;
                    case 7: intUnlockAmount = 50; intUnlockHave = intBoughtUnique[3]; strUnlockText = "50 unique Clothing Codes"; break;
                    case 8: intUnlockAmount = 20; intUnlockHave = intBoughtUnique[1]; strUnlockText = "20 unique Carpet Codes"; break;
                    case 9: intUnlockAmount = 20; intUnlockHave = intBoughtUnique[2]; strUnlockText = "20 unique Wallpaper Codes"; break;
                    case 10: intUnlockAmount = 1; intUnlockHave = intTasksComplete[109]; strUnlockText = "Completing a K.K. Slider Concert Task"; break;
                    case 11: intUnlockAmount = 20; intUnlockHave = intBoughtUnique[8]; strUnlockText = "20 unique K.K. Slider Aircheck Codes"; break;
                    case 12: intUnlockAmount = 10; intUnlockHave = intBoughtUnique[15]; strUnlockText = "10 unique NES Game Codes"; break;
                    case 13: intUnlockAmount = 60; intUnlockHave = intBoughtUnique[5]; strUnlockText = "60 unique Stationery Codes"; break;
                    case 14: intUnlockAmount = 60; intUnlockHave = intBoughtUnique[4]; strUnlockText = "60 unique Handheld Item Codes"; break;
                    case 15: intUnlockAmount = 500; intUnlockHave = intBoughtUnique[0]; strUnlockText = "500 unique Furniture Codes"; break;
                    case 16: intUnlockAmount = 600; intUnlockHave = intBoughtUnique[0]; strUnlockText = "600 unique Furniture Codes"; break;
                    case 17: intUnlockAmount = 1372; intUnlockHave = intBoughtUnique[17]; strUnlockText = "All other Item Codes"; break;
                    default: break;
                }

                strAvailability = String.Format("Available after:  {0} - ({1}/{2})", strUnlockText, intUnlockHave, intUnlockAmount);
            }
            if (itemCodeData.Item1.Name == "Birthday Cake" && boolBirthdaySet == false)
                strAvailability = "Available after:  Setting your Birthday";


            return strAvailability;
        }

        private void UpdateFilterGroupCheckboxes()
        {
            switch (intFilterList[0])
            {
                case 0:
                    chkFilterGroup1.Visible = false; chkFilterGroup2.Visible = false; chkFilterGroup3.Visible = true;
                    chkFilterGroup4.Visible = true; chkFilterGroup5.Visible = true; chkFilterGroup6.Visible = true;
                    chkFilterGroup1.BackgroundImage = null;
                    chkFilterGroup2.BackgroundImage = null;
                    chkFilterGroup3.BackgroundImage = Properties.Resources.UI_Filter_All_False;
                    chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_Nook_False;
                    chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_Seasonal_False;
                    chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_Other_False;
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: chkFilterGroup1.BackgroundImage = null; break;
                        case 1: chkFilterGroup2.BackgroundImage = null; break;
                        case 2: chkFilterGroup3.BackgroundImage = Properties.Resources.UI_Filter_All; break;
                        case 3: chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_Nook; break;
                        case 4: chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_Seasonal; break;
                        case 5: chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_Other; break;
                    }
                    break;
                case 1:
                    chkFilterGroup1.Visible = true; chkFilterGroup2.Visible = true; chkFilterGroup3.Visible = true;
                    chkFilterGroup4.Visible = true; chkFilterGroup5.Visible = true; chkFilterGroup6.Visible = true;
                    chkFilterGroup1.BackgroundImage = Properties.Resources.UI_Filter_All_False;
                    chkFilterGroup2.BackgroundImage = Properties.Resources.UI_Filter_Nook_False;
                    chkFilterGroup3.BackgroundImage = Properties.Resources.UI_Filter_Redd_False;
                    chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_Raffle_False;
                    chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_Seasonal_False;
                    chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_Miscellaneous_False;
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: chkFilterGroup1.BackgroundImage = Properties.Resources.UI_Filter_All; break;
                        case 1: chkFilterGroup2.BackgroundImage = Properties.Resources.UI_Filter_Nook; break;
                        case 2: chkFilterGroup3.BackgroundImage = Properties.Resources.UI_Filter_Redd; break;
                        case 3: chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_Raffle; break;
                        case 4: chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_Seasonal; break;
                        case 5: chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_Miscellaneous; break;
                    }
                    break;
                case 2:
                    chkFilterGroup1.Visible = false; chkFilterGroup2.Visible = false; chkFilterGroup3.Visible = true;
                    chkFilterGroup4.Visible = true; chkFilterGroup5.Visible = true; chkFilterGroup6.Visible = true;
                    chkFilterGroup1.BackgroundImage = null;
                    chkFilterGroup2.BackgroundImage = null;
                    chkFilterGroup3.BackgroundImage = Properties.Resources.UI_Filter_All_False;
                    chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_Nook_False;
                    chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_Saharah_False;
                    chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_Seasonal_False;
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: chkFilterGroup1.BackgroundImage = null; break;
                        case 1: chkFilterGroup2.BackgroundImage = null; break;
                        case 2: chkFilterGroup3.BackgroundImage = Properties.Resources.UI_Filter_All; break;
                        case 3: chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_Nook; break;
                        case 4: chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_Saharah; break;
                        case 5: chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_Seasonal; break;
                    }
                    break;
                case 3:
                    chkFilterGroup1.Visible = false; chkFilterGroup2.Visible = false; chkFilterGroup3.Visible = true;
                    chkFilterGroup4.Visible = true; chkFilterGroup5.Visible = true; chkFilterGroup6.Visible = true;
                    chkFilterGroup1.BackgroundImage = null;
                    chkFilterGroup2.BackgroundImage = null;
                    chkFilterGroup3.BackgroundImage = Properties.Resources.UI_Filter_All_False;
                    chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_Nook_False;
                    chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_Wendell_False;
                    chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_Seasonal_False;
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: chkFilterGroup1.BackgroundImage = null; break;
                        case 1: chkFilterGroup2.BackgroundImage = null; break;
                        case 2: chkFilterGroup3.BackgroundImage = Properties.Resources.UI_Filter_All; break;
                        case 3: chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_Nook; break;
                        case 4: chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_Wendell; break;
                        case 5: chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_Seasonal; break;
                    }
                    break;
                case 4:
                    chkFilterGroup1.Visible = false; chkFilterGroup2.Visible = true; chkFilterGroup3.Visible = true;
                    chkFilterGroup4.Visible = true; chkFilterGroup5.Visible = true; chkFilterGroup6.Visible = true;
                    chkFilterGroup1.BackgroundImage = null;
                    chkFilterGroup2.BackgroundImage = Properties.Resources.UI_Filter_All_False;
                    chkFilterGroup3.BackgroundImage = Properties.Resources.UI_Filter_Nook_False;
                    chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_Gracie_False;
                    chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_Seasonal_False;
                    chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_Beta_False;
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: chkFilterGroup1.BackgroundImage = null; break;
                        case 1: chkFilterGroup2.BackgroundImage = Properties.Resources.UI_Filter_All; break;
                        case 2: chkFilterGroup3.BackgroundImage = Properties.Resources.UI_Filter_Nook; break;
                        case 3: chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_Gracie; break;
                        case 4: chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_Seasonal; break;
                        case 5: chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_Beta; break;
                    }
                    break;
                case 5:
                    chkFilterGroup1.Visible = true; chkFilterGroup2.Visible = true; chkFilterGroup3.Visible = true;
                    chkFilterGroup4.Visible = true; chkFilterGroup5.Visible = true; chkFilterGroup6.Visible = true;
                    chkFilterGroup1.BackgroundImage = Properties.Resources.UI_Filter_All_False;
                    chkFilterGroup2.BackgroundImage = Properties.Resources.UI_Filter_Tool_False;
                    chkFilterGroup3.BackgroundImage = Properties.Resources.UI_Filter_Umbrella_False;
                    chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_Fan_False;
                    chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_Pinwheel_False;
                    chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_Balloon_False;
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: chkFilterGroup1.BackgroundImage = Properties.Resources.UI_Filter_All; break;
                        case 1: chkFilterGroup2.BackgroundImage = Properties.Resources.UI_Filter_Tool; break;
                        case 2: chkFilterGroup3.BackgroundImage = Properties.Resources.UI_Filter_Umbrella; break;
                        case 3: chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_Fan; break;
                        case 4: chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_Pinwheel; break;
                        case 5: chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_Balloon; break;
                    }
                    break;
                case 6:
                    chkFilterGroup1.Visible = false; chkFilterGroup2.Visible = false; chkFilterGroup3.Visible = true;
                    chkFilterGroup4.Visible = true; chkFilterGroup5.Visible = true; chkFilterGroup6.Visible = true;
                    chkFilterGroup1.BackgroundImage = null;
                    chkFilterGroup2.BackgroundImage = null;
                    chkFilterGroup3.BackgroundImage = Properties.Resources.UI_Filter_All_False;
                    chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_Nook_False;
                    chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_Seasonal_False;
                    chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_Miscellaneous_False;
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: chkFilterGroup1.BackgroundImage = null; break;
                        case 1: chkFilterGroup2.BackgroundImage = null; break;
                        case 2: chkFilterGroup3.BackgroundImage = Properties.Resources.UI_Filter_All; break;
                        case 3: chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_Nook; break;
                        case 4: chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_Seasonal; break;
                        case 5: chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_Miscellaneous; break;
                    }
                    break;
                case 7:
                    chkFilterGroup1.Visible = false; chkFilterGroup2.Visible = true; chkFilterGroup3.Visible = true;
                    chkFilterGroup4.Visible = true; chkFilterGroup5.Visible = true; chkFilterGroup6.Visible = true;
                    chkFilterGroup1.BackgroundImage = null;
                    chkFilterGroup2.BackgroundImage = Properties.Resources.UI_Filter_All_False;
                    chkFilterGroup3.BackgroundImage = Properties.Resources.UI_Filter_GyroidMini_False;
                    chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_Gyroid_False;
                    chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_GyroidMega_False;
                    chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_GyroidTall_False;
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: chkFilterGroup1.BackgroundImage = null; break;
                        case 1: chkFilterGroup2.BackgroundImage = Properties.Resources.UI_Filter_All; break;
                        case 2: chkFilterGroup3.BackgroundImage = Properties.Resources.UI_Filter_GyroidMini; break;
                        case 3: chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_Gyroid; break;
                        case 4: chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_GyroidMega; break;
                        case 5: chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_GyroidTall; break;
                    }
                    break;
                case 8:
                    chkFilterGroup1.Visible = false; chkFilterGroup2.Visible = false; chkFilterGroup3.Visible = false;
                    chkFilterGroup4.Visible = true; chkFilterGroup5.Visible = true; chkFilterGroup6.Visible = true;
                    chkFilterGroup1.BackgroundImage = null;
                    chkFilterGroup2.BackgroundImage = null;
                    chkFilterGroup3.BackgroundImage = null;
                    chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_All_False;
                    chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_Fossil_False;
                    chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_Dinosaur_False;
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: chkFilterGroup1.BackgroundImage = null; break;
                        case 1: chkFilterGroup2.BackgroundImage = null; break;
                        case 2: chkFilterGroup3.BackgroundImage = null; break;
                        case 3: chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_All; break;
                        case 4: chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_Fossil; break;
                        case 5: chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_Dinosaur; break;
                    }
                    break;
                case 9:
                    chkFilterGroup1.Visible = false; chkFilterGroup2.Visible = false; chkFilterGroup3.Visible = false;
                    chkFilterGroup4.Visible = true; chkFilterGroup5.Visible = true; chkFilterGroup6.Visible = true;
                    chkFilterGroup1.BackgroundImage = null;
                    chkFilterGroup2.BackgroundImage = null;
                    chkFilterGroup3.BackgroundImage = null;
                    chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_All_False;
                    chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_Music_False;
                    chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_MusicSecret_False;
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: chkFilterGroup1.BackgroundImage = null; break;
                        case 1: chkFilterGroup2.BackgroundImage = null; break;
                        case 2: chkFilterGroup3.BackgroundImage = null; break;
                        case 3: chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_All; break;
                        case 4: chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_Music; break;
                        case 5: chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_MusicSecret; break;
                    }
                    break;
                case 10:
                    chkFilterGroup1.Visible = true; chkFilterGroup2.Visible = true; chkFilterGroup3.Visible = true;
                    chkFilterGroup4.Visible = true; chkFilterGroup5.Visible = true; chkFilterGroup6.Visible = true;
                    chkFilterGroup1.BackgroundImage = Properties.Resources.UI_Filter_All_False;
                    chkFilterGroup2.BackgroundImage = Properties.Resources.UI_Filter_Bells_False;
                    chkFilterGroup3.BackgroundImage = Properties.Resources.UI_Filter_Nature_False;
                    chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_Seasonal_False;
                    chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_Miscellaneous_False;
                    chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_Beta_False;
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: chkFilterGroup1.BackgroundImage = Properties.Resources.UI_Filter_All; break;
                        case 1: chkFilterGroup2.BackgroundImage = Properties.Resources.UI_Filter_Bells; break;
                        case 2: chkFilterGroup3.BackgroundImage = Properties.Resources.UI_Filter_Nature; break;
                        case 3: chkFilterGroup4.BackgroundImage = Properties.Resources.UI_Filter_Seasonal; break;
                        case 4: chkFilterGroup5.BackgroundImage = Properties.Resources.UI_Filter_Miscellaneous; break;
                        case 5: chkFilterGroup6.BackgroundImage = Properties.Resources.UI_Filter_Beta; break;
                    }
                    break;
            }
        }

        private void UpdateFilterLabelText()
        {
            switch (intFilterList[0])
            {
                case 0:
                    lblFilterCategory.Text = "Category:  Everything";
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: lblFilterGroup.Text = ""; break;
                        case 1: lblFilterGroup.Text = ""; break;
                        case 2: lblFilterGroup.Text = "Group:  Everything"; break;
                        case 3: lblFilterGroup.Text = "Group:  Nook"; break;
                        case 4: lblFilterGroup.Text = "Group:  Seasonal"; break;
                        case 5: lblFilterGroup.Text = "Group:  Other"; break;
                        default: break;
                    }
                    break;
                case 1:
                    lblFilterCategory.Text = "Category:  Furniture";
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: lblFilterGroup.Text = "Group:  Everything"; break;
                        case 1: lblFilterGroup.Text = "Group:  Nook"; break;
                        case 2: lblFilterGroup.Text = "Group:  Crazy Redd"; break;
                        case 3: lblFilterGroup.Text = "Group:  Raffle"; break;
                        case 4: lblFilterGroup.Text = "Group:  Seasonal"; break;
                        case 5: lblFilterGroup.Text = "Group:  Other"; break;
                        default: break;
                    }
                    break;
                case 2:
                    lblFilterCategory.Text = "Category:  Carpets";
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: lblFilterGroup.Text = ""; break;
                        case 1: lblFilterGroup.Text = ""; break;
                        case 2: lblFilterGroup.Text = "Group:  Everything"; break;
                        case 3: lblFilterGroup.Text = "Group:  Nook"; break;
                        case 4: lblFilterGroup.Text = "Group:  Saharah"; break;
                        case 5: lblFilterGroup.Text = "Group:  Seasonal"; break;
                        default: break;
                    }
                    break;
                case 3:
                    lblFilterCategory.Text = "Category:  Wallpaper";
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: lblFilterGroup.Text = ""; break;
                        case 1: lblFilterGroup.Text = ""; break;
                        case 2: lblFilterGroup.Text = "Group:  Everything"; break;
                        case 3: lblFilterGroup.Text = "Group:  Nook"; break;
                        case 4: lblFilterGroup.Text = "Group:  Wendell"; break;
                        case 5: lblFilterGroup.Text = "Group:  Seasonal"; break;
                        default: break;
                    }
                    break;
                case 4:
                    lblFilterCategory.Text = "Category:  Clothing";
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: lblFilterGroup.Text = ""; break;
                        case 1: lblFilterGroup.Text = "Group:  Everything"; break;
                        case 2: lblFilterGroup.Text = "Group:  Nook"; break;
                        case 3: lblFilterGroup.Text = "Group:  Gracie"; break;
                        case 4: lblFilterGroup.Text = "Group:  Seasonal"; break;
                        case 5: lblFilterGroup.Text = "Group:  Beta"; break;
                        default: break;
                    }
                    break;
                case 5:
                    lblFilterCategory.Text = "Category:  Handhelds";
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: lblFilterGroup.Text = "Group:  Everything"; break;
                        case 1: lblFilterGroup.Text = "Group:  Tools"; break;
                        case 2: lblFilterGroup.Text = "Group:  Umbrellas"; break;
                        case 3: lblFilterGroup.Text = "Group:  Fans"; break;
                        case 4: lblFilterGroup.Text = "Group:  Pinwheels"; break;
                        case 5: lblFilterGroup.Text = "Group:  Balloons"; break;
                        default: break;
                    }
                    break;
                case 6:
                    lblFilterCategory.Text = "Category:  Stationery";
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: lblFilterGroup.Text = ""; break;
                        case 1: lblFilterGroup.Text = ""; break;
                        case 2: lblFilterGroup.Text = "Group:  Everything"; break;
                        case 3: lblFilterGroup.Text = "Group:  Nook"; break;
                        case 4: lblFilterGroup.Text = "Group:  Seasonal"; break;
                        case 5: lblFilterGroup.Text = "Group:  Other"; break;
                        default: break;
                    }
                    break;
                case 7:
                    lblFilterCategory.Text = "Category:  Gyroids";
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: lblFilterGroup.Text = ""; break;
                        case 1: lblFilterGroup.Text = "Group:  Everything"; break;
                        case 2: lblFilterGroup.Text = "Group:  Mini Gyroids"; break;
                        case 3: lblFilterGroup.Text = "Group:  Gyroids"; break;
                        case 4: lblFilterGroup.Text = "Group:  Mega Gyroids"; break;
                        case 5: lblFilterGroup.Text = "Group:  Tall Gyroids"; break;
                        default: break;
                    }
                    break;
                case 8:
                    lblFilterCategory.Text = "Category:  Fossils";
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: lblFilterGroup.Text = ""; break;
                        case 1: lblFilterGroup.Text = ""; break;
                        case 2: lblFilterGroup.Text = ""; break;
                        case 3: lblFilterGroup.Text = "Group:  Everything"; break;
                        case 4: lblFilterGroup.Text = "Group:  Common"; break;
                        case 5: lblFilterGroup.Text = "Group:  Dinosaurs"; break;
                        default: break;
                    }
                    break;
                case 9:
                    lblFilterCategory.Text = "Category:  Music";
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: lblFilterGroup.Text = ""; break;
                        case 1: lblFilterGroup.Text = ""; break;
                        case 2: lblFilterGroup.Text = ""; break;
                        case 3: lblFilterGroup.Text = "Group:  Everything"; break;
                        case 4: lblFilterGroup.Text = "Group:  Common"; break;
                        case 5: lblFilterGroup.Text = "Group:  Secret"; break;
                        default: break;
                    }
                    break;
                case 10:
                    lblFilterCategory.Text = "Category:  Miscellaneous";
                    switch (intFilterList[9 + intFilterList[0]])
                    {
                        case 0: lblFilterGroup.Text = "Group:  Everything"; break;
                        case 1: lblFilterGroup.Text = "Group:  Bells"; break;
                        case 2: lblFilterGroup.Text = "Group:  Nature"; break;
                        case 3: lblFilterGroup.Text = "Group:  Seasonal"; break;
                        case 4: lblFilterGroup.Text = "Group:  Other"; break;
                        case 5: lblFilterGroup.Text = "Group:  Beta"; break;
                        default: break;
                    }
                    break;
                default: break;
            }

            switch (intFilterList[1])
            {
                case 1: lblAvailability.Text = "Everything"; break;
                case 2: lblAvailability.Text = "Available"; break;
                case 3: lblAvailability.Text = "Unavailable"; break;
                default: break;
            }

            switch (intFilterList[2])
            {
                case 0: lblSort.Text = "Catalog Order"; break;
                case 1: lblSort.Text = "Alphabetized"; break;
                case 2: lblSort.Text = "Ticket Price"; break;
                default: break;
            }
        }

        private void CheckFiltersCategory()
        {
            //intSelectionCounter = 0;
            if (chkFilterCategoryEverything.Checked == false && chkFilterCategoryFurniture.Checked == false && chkFilterCategoryCarpet.Checked == false &&
                chkFilterCategoryWallpaper.Checked == false && chkFilterCategoryClothing.Checked == false && chkFilterCategoryTools.Checked == false &&
                chkFilterCategoryStationery.Checked == false && chkFilterCategoryGyroid.Checked == false && chkFilterCategoryFossil.Checked == false &&
                chkFilterCategoryMusic.Checked == false && chkFilterCategoryMiscellaneous.Checked == false)
            {
                switch (intFilterList[0])
                {
                    case 0: chkFilterCategoryEverything.Checked = true; break;
                    case 1: chkFilterCategoryFurniture.Checked = true; break;
                    case 2: chkFilterCategoryCarpet.Checked = true; break;
                    case 3: chkFilterCategoryWallpaper.Checked = true; break;
                    case 4: chkFilterCategoryClothing.Checked = true; break;
                    case 5: chkFilterCategoryTools.Checked = true; break;
                    case 6: chkFilterCategoryStationery.Checked = true; break;
                    case 7: chkFilterCategoryGyroid.Checked = true; break;
                    case 8: chkFilterCategoryFossil.Checked = true; break;
                    case 9: chkFilterCategoryMusic.Checked = true; break;
                    case 10: chkFilterCategoryMiscellaneous.Checked = true; break;
                    default: chkFilterCategoryEverything.Checked = true; break;
                }
            }

            if (chkFilterGroup1.Checked == false && chkFilterGroup2.Checked == false && chkFilterGroup3.Checked == false &&
                chkFilterGroup4.Checked == false && chkFilterGroup5.Checked == false && chkFilterGroup6.Checked == false)
            {
                switch (intFilterList[9 + intFilterList[0]])
                {
                    case 0: chkFilterGroup1.Checked = true; break;
                    case 1: chkFilterGroup2.Checked = true; break;
                    case 2: chkFilterGroup3.Checked = true; break;
                    case 3: chkFilterGroup4.Checked = true; break;
                    case 4: chkFilterGroup5.Checked = true; break;
                    case 5: chkFilterGroup6.Checked = true; break;
                    default: chkFilterGroup1.Checked = true; break;
                }
            }

            if (chkAvailabilityAll.Checked == false && chkAvailabilityYes.Checked == false && chkAvailabilityNo.Checked == false)
            {
                switch (intFilterList[1])
                {
                    case 1: chkAvailabilityAll.Checked = true; break;
                    case 2: chkAvailabilityYes.Checked = true; break;
                    case 3: chkAvailabilityNo.Checked = true; break;
                    default: chkAvailabilityYes.Checked = true; break;
                }
            }

            if (chkSortCatalog.Checked == false && chkSortAlpha.Checked == false && chkSortPrice.Checked == false)
            {
                switch (intFilterList[2])
                {
                    case 0: chkSortCatalog.Checked = true; break;
                    case 1: chkSortAlpha.Checked = true; break;
                    case 2: chkSortPrice.Checked = true; break;
                    default: chkSortCatalog.Checked = true; break;
                }
            }
        }

        private void HideFilterGroup()
        {
            imgFilterGroupHide.BringToFront();
            imgFilterGroupHide.Visible = true;
            lblFilterGroup.Visible = false;
        }

        private void RevealFilterGroup()
        {
            imgFilterGroupHide.Visible = false;
            lblFilterGroup.Visible = true;
        }

        private void UpdateCheatLabel()
        {
            lblTickets.Text = intTicketsCurrent.ToString("N0");
        }

        private void UpdateItemLabels(Tuple<ItemInfo, int> itemCodeData)
        {
            lblUnlock.Text = String.Format(GetAvailabilityText(itemCodeData));
            lblTimesBought.Text = String.Format("Times Bought:  {0}", intBoughtList[itemCodeData.Item1.CatalogIndex]);
            btnTicketsPrice.Text = itemCodeData.Item1.Tickets.ToString("N0");

            if (imgShopDialog.Visible == true)
            {
                lblFengShuiColor.Location = imgShopDialog.PointToClient(lblFengShuiColor.Parent.PointToScreen(lblFengShuiColor.Location));
                lblFengShuiColor.Parent = imgShopDialog;
            }
            else
            {
                lblFengShuiColor.Location = this.PointToClient(lblFengShuiColor.Parent.PointToScreen(lblFengShuiColor.Location));
                lblFengShuiColor.Parent = this;
            }
            switch (itemCodeData.Item1.FengShui)
            {
                case "None": lblFengShuiColor.ForeColor = Color.Black; break;
                case "Red": lblFengShuiColor.ForeColor = Color.Red; break;
                case "Orange": lblFengShuiColor.ForeColor = Color.Orange; break;
                case "Green": lblFengShuiColor.ForeColor = Color.Green; break;
                case "Yellow": lblFengShuiColor.ForeColor = Color.Yellow; break;
                case "Special": lblFengShuiColor.ForeColor = Color.FromArgb(255, 0, 192, 192); break;
                default: lblFengShuiColor.ForeColor = Color.Black; break;
            }
            lblFengShui.Text = "Feng Shui:";
            lblFengShuiColor.Text = itemCodeData.Item1.FengShui.ToString();
        }

        private void SelectedItemOptionSelected()
        {
            lblFengShui.Text = string.Empty;
            lblFengShuiColor.Text = string.Empty;
            btnTicketsPrice.Visible = false;
            lblTickets.Text = intTicketsCurrent.ToString("N0");
            boolItemSelected = false;

            txtFilterSearch.ReadOnly = false;
            txtFilterSearch.BackColor = Color.PapayaWhip;
            txtFilterSearch.Enabled = true;

            imgSpecialDelivery.Visible = false;
            imgDialogSelect.Visible = false;
            btnDialogSelectYes.Visible = false;
            btnDialogSelectNo.Visible = false;
            btnDialogSelectYes.Location = pntDialogYes;
            btnDialogSelectYes.Text = "Please!";
            //lstShop.SelectedItems.Clear();

            imgShopItemSmall.BackgroundImage = null;
            imgShopItem.Visible = true;
            imgShopItem.BackgroundImage = null;
            //lstShop.SelectedItems.Clear();

            if (boolNESDialog == false && lviSelectedItem != null)
            {
                int index = lviSelectedItem.Index;

                //PopulateListView();


                if (index < lstShop.Items.Count)
                {
                    lstShop.Items[index].Selected = true;
                    lstShop.EnsureVisible(index);

                    lviSelectedItem.BackColor = Color.FromArgb(255, 255, 215, 179);
                    lviSelectedItem.ForeColor = Color.FromArgb(255, 32, 32, 32);
                }

                //lstShop.SelectedItems.Clear();
                lviSelectedItem = null;
            }
            boolNESDialog = false;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void lstShop_MouseMove(object sender, MouseEventArgs e)
        {
            if (boolItemSelected == false)
            {
                ListViewItem? item = lstShop.GetItemAt(e.X, e.Y); // get the list view item at the mouse location, much faster then waiting for the hover event

                if (item == null)
                {
                    return;
                }

                lstShop.BeginUpdate();

                //lstShop.SelectedItems.Clear();
                if (previousListViewItem != null)
                {
                    previousListViewItem.ForeColor = GetListViewItemColor(CheckShopAvailability(previousItem), CheckItemUnlock(previousItem), intBoughtList[previousItem.Item1.CatalogIndex]);
                    previousListViewItem.BackColor = Color.FromArgb(255, 255, 215, 179);
                }

                previousListViewItemColor = item.ForeColor;
                item.BackColor = Color.FromArgb(255, 255, 150, 58);
                item.ForeColor = Color.White;

                Tuple<ItemInfo, int> itemCodeData = ItemData.GetItemCheckByName(item.Text.Substring(1, item.Text.Length - 1), out int _);
                previousItem = itemCodeData;
                btnTicketsPrice.Visible = true;

                UpdateItemLabels(itemCodeData);

                if (bInvalidItemSelected == false || invalidSelectedItem != item)
                {
                    imgShopDialog.Visible = false;
                    lblDialogTimmy.Visible = false;
                    lblDialogTommy.Visible = false;
                    imgShopItem.Visible = true;
                }
                else
                {
                    imgShopDialog.Visible = true;
                    lblDialogTimmy.Visible = true;
                    lblDialogTommy.Visible = true;
                    imgShopItem.Visible = false;
                }

                imgShopItem.BackgroundImage = ResizeImage(itemCodeData.Item1.ImageName, 500, 500);
                imgShopItem.Refresh();
                previousListViewItem = item;

                lstShop.EndUpdate();
                lstShop.Refresh();
            }
        }

        private void lstShop_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {

            if (lstShop.FocusedItem != null)
            {
                if (previousListViewItem != null)
                {
                    previousListViewItem.ForeColor = previousListViewItemColor;
                    previousListViewItem.BackColor = Color.FromArgb(255, 255, 215, 179);
                }
                Tuple<ItemInfo, int> itemCodeData = ItemData.GetItemCheckByName(lstShop.FocusedItem.Text.Substring(1, lstShop.FocusedItem.Text.Length - 1), out int _);

                UpdateItemLabels(itemCodeData);

                if (CheckShopAvailability(itemCodeData) == true && CheckItemUnlock(itemCodeData) == true && CheckNESAvailability(itemCodeData) == true)
                {
                    boolItemSelected = true;
                    lviSelectedItem = e.Item;
                    txtFilterSearch.ReadOnly = true;
                    txtFilterSearch.BackColor = Color.Gainsboro;
                    txtFilterSearch.Enabled = false;
                    imgShopItem.BackgroundImage = null;
                    imgShopItemSmall.BackgroundImage = ResizeImage(itemCodeData.Item1.ImageName, 140, 140);
                    bInvalidItemSelected = false;
                    invalidSelectedItem = null;
                    if (itemCodeData.Item1.ID < intNESStart || itemCodeData.Item1.ID >= intNESEnd)
                        UpdateShopDialogLabelText(1, itemCodeData.Item1.Name, itemCodeData.Item1.Tickets);
                    else
                        UpdateShopDialogLabelText(5, itemCodeData.Item1.Name, itemCodeData.Item1.Tickets);
                }
                else
                {
                    imgShopItem.BackgroundImage = null;
                    imgShopItemSmall.Visible = false;
                    imgDialogSelect.Visible = false;
                    btnDialogSelectYes.Visible = false;
                    btnDialogSelectNo.Visible = false;
                    boolItemSelected = false;
                    bInvalidItemSelected = true;
                    invalidSelectedItem = e.Item;
                    if (itemCodeData.Item1.ID >= intNESStart && itemCodeData.Item1.ID < intNESEnd && CheckItemUnlock(itemCodeData) == true) //TODO: distinguish between available NES and already purchased NES
                    {
                        intLastViewedDialog = 7;
                        UpdateShopDialogLabelText(7, "", 0);
                    }
                    else
                    {
                        intLastViewedDialog = 2;
                        UpdateShopDialogLabelText(2, "", 0);
                    }
                }
            }

        }

        private void lstShop_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstShop_MouseLeave(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                lblFengShui.Text = "";
                lblFengShuiColor.Text = "";
                imgShopDialog.Visible = true;
                lblDialogTimmy.Visible = true;
                lblDialogTommy.Visible = true;
                btnTicketsPrice.Visible = false;
                lblUnlock.Text = "";
                lblTimesBought.Text = "";
                if (previousListViewItem != null)
                {
                    previousListViewItem.ForeColor = previousListViewItemColor;
                    previousListViewItem.BackColor = Color.FromArgb(255, 255, 215, 179);
                }
                UpdateShopDialogLabelText(intLastViewedDialog, "", 0);

                imgShopItem.BackgroundImage = null;
            }
        }

        private void btnDialogSelectYes_Click(object sender, EventArgs e)
        {
            if (boolNESDialog == false)
            {
                if (boolItemSelected == true)
                {
                    Tuple<ItemInfo, int> itemCodeData = ItemData.GetItemCheckByName(lviSelectedItem.Text.Substring(1, lviSelectedItem.Text.Length - 1), out int _);

                    if (intTicketsCurrent >= itemCodeData.Item1.Tickets)
                    {
                        int i = (int)lviSelectedItem.Tag;

                        IncrementItemsBought(itemCodeData);
                        saveData();
                        SelectedItemOptionSelected();

                        if (itemCodeData.Item1.ID < intNESStart || itemCodeData.Item1.ID >= intNESEnd)
                        {
                            AddPasswordToArray(ItemData.GetItemCheckByName(itemCodeData.Item1.Name, out int _));
                            saveData();
                            intLastViewedDialog = 9;
                            UpdateShopDialogLabelText(9, "", 0);
                        }
                        else
                        {
                            if (itemCodeData.Item1.Name == "Grab Bag (w/ NES Games)")
                                boolHasNESGame = true;
                            strNESinQueue = itemCodeData.Item1.Name;
                            saveData();
                            intLastViewedDialog = 6;
                            UpdateShopDialogLabelText(6, "", 0);
                        }

                        lstShop_UpdateItemStatus(i);
                    }
                    else
                    {
                        SelectedItemOptionSelected();
                        UpdateShopDialogLabelText(10, "", 0);
                    }
                }
            }
            else
            {
                lstShop.Visible = true;
                boolHasNESGame = true;
                AddPasswordToArray(ItemData.GetItemCheckByName(strNESinQueue, out int _));
                strNESinQueue = "";
                SelectedItemOptionSelected();
                intLastViewedDialog = 4;
                UpdateShopDialogLabelText(4, "", 0);
                //PopulateListView();
                saveData();
            }
        }

        private void btnDialogSelectNo_Click(object sender, EventArgs e)
        {
            SelectedItemOptionSelected();
            if (intLastViewedDialog != 3)
                intLastViewedDialog = 4;
            UpdateShopDialogLabelText(intLastViewedDialog, "", 0);
        }

        private void btnTicketsAdd_Click(object sender, EventArgs e)
        {
            intTicketsCurrent += Convert.ToInt32(txtTicketsExchange.Text);
            UpdateCheatLabel();
            saveData();
        }

        private void btnTicketsSubtract_Click(object sender, EventArgs e)
        {
            intTicketsCurrent -= Convert.ToInt32(txtTicketsExchange.Text);
            UpdateCheatLabel();
            saveData();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            saveData();
            this.Hide();
        }

        private void btnCheat_Click(object sender, EventArgs e)
        {
            if (txtTicketsExchange.Visible == true)
            {
                txtTicketsExchange.Visible = false;
                btnTicketsAdd.Visible = false;
                btnTicketsSubtract.Visible = false;
            }
            else
            {
                txtTicketsExchange.Visible = true;
                btnTicketsAdd.Visible = true;
                btnTicketsSubtract.Visible = true;
            }
        }

        private void chkFilterCategoryEverything_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkFilterCategoryEverything.Checked == true)
                {
                    imgFilterGroup.BackgroundImage = Properties.Resources.UI_Filter_BG_4;
                    chkFilterCategoryEverything.BackgroundImage = Properties.Resources.UI_Filter_All;
                    chkFilterCategoryFurniture.Checked = false;
                    chkFilterCategoryCarpet.Checked = false;
                    chkFilterCategoryWallpaper.Checked = false;
                    chkFilterCategoryClothing.Checked = false;
                    chkFilterCategoryTools.Checked = false;
                    chkFilterCategoryStationery.Checked = false;
                    chkFilterCategoryGyroid.Checked = false;
                    chkFilterCategoryFossil.Checked = false;
                    chkFilterCategoryMiscellaneous.Checked = false;
                    intFilterList[0] = 0;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryEverything.BackgroundImage = Properties.Resources.UI_Filter_All_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); //PopulateListView();
            }
        }

        private void chkFilterCategoryFurniture_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkFilterCategoryFurniture.Checked == true)
                {
                    imgFilterGroup.BackgroundImage = Properties.Resources.UI_Filter_BG_6;
                    chkFilterCategoryFurniture.BackgroundImage = Properties.Resources.UI_Filter_Furniture;
                    chkFilterCategoryEverything.Checked = false;
                    chkFilterCategoryCarpet.Checked = false;
                    chkFilterCategoryWallpaper.Checked = false;
                    chkFilterCategoryClothing.Checked = false;
                    chkFilterCategoryTools.Checked = false;
                    chkFilterCategoryStationery.Checked = false;
                    chkFilterCategoryGyroid.Checked = false;
                    chkFilterCategoryFossil.Checked = false;
                    chkFilterCategoryMusic.Checked = false;
                    chkFilterCategoryMiscellaneous.Checked = false;
                    intFilterList[0] = 1;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryFurniture.BackgroundImage = Properties.Resources.UI_Filter_Furniture_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); //PopulateListView();
            }
        }

        private void chkFilterCategoryCarpet_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkFilterCategoryCarpet.Checked == true)
                {
                    imgFilterGroup.BackgroundImage = Properties.Resources.UI_Filter_BG_4;
                    chkFilterCategoryCarpet.BackgroundImage = Properties.Resources.UI_Filter_Carpet;
                    chkFilterCategoryFurniture.Checked = false;
                    chkFilterCategoryEverything.Checked = false;
                    chkFilterCategoryWallpaper.Checked = false;
                    chkFilterCategoryClothing.Checked = false;
                    chkFilterCategoryTools.Checked = false;
                    chkFilterCategoryStationery.Checked = false;
                    chkFilterCategoryGyroid.Checked = false;
                    chkFilterCategoryFossil.Checked = false;
                    chkFilterCategoryMusic.Checked = false;
                    chkFilterCategoryMiscellaneous.Checked = false;
                    intFilterList[0] = 2;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryCarpet.BackgroundImage = Properties.Resources.UI_Filter_Carpet_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); //PopulateListView();
            }
        }

        private void chkFilterCategoryWallpaper_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkFilterCategoryWallpaper.Checked == true)
                {
                    imgFilterGroup.BackgroundImage = Properties.Resources.UI_Filter_BG_4;
                    chkFilterCategoryWallpaper.BackgroundImage = Properties.Resources.UI_Filter_Wallpaper;
                    chkFilterCategoryFurniture.Checked = false;
                    chkFilterCategoryEverything.Checked = false;
                    chkFilterCategoryCarpet.Checked = false;
                    chkFilterCategoryClothing.Checked = false;
                    chkFilterCategoryTools.Checked = false;
                    chkFilterCategoryStationery.Checked = false;
                    chkFilterCategoryGyroid.Checked = false;
                    chkFilterCategoryFossil.Checked = false;
                    chkFilterCategoryMusic.Checked = false;
                    chkFilterCategoryMiscellaneous.Checked = false;
                    intFilterList[0] = 3;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryWallpaper.BackgroundImage = Properties.Resources.UI_Filter_Wallpaper_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); // PopulateListView();
            }
        }

        private void chkFilterCategoryClothing_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkFilterCategoryClothing.Checked == true)
                {
                    imgFilterGroup.BackgroundImage = Properties.Resources.UI_Filter_BG_5;
                    chkFilterCategoryClothing.BackgroundImage = Properties.Resources.UI_Filter_Shirt;
                    chkFilterCategoryFurniture.Checked = false;
                    chkFilterCategoryEverything.Checked = false;
                    chkFilterCategoryCarpet.Checked = false;
                    chkFilterCategoryWallpaper.Checked = false;
                    chkFilterCategoryTools.Checked = false;
                    chkFilterCategoryStationery.Checked = false;
                    chkFilterCategoryGyroid.Checked = false;
                    chkFilterCategoryFossil.Checked = false;
                    chkFilterCategoryMusic.Checked = false;
                    chkFilterCategoryMiscellaneous.Checked = false;
                    intFilterList[0] = 4;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryClothing.BackgroundImage = Properties.Resources.UI_Filter_Shirt_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); // PopulateListView();
            }
        }

        private void chkFilterCategoryTools_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkFilterCategoryTools.Checked == true)
                {
                    imgFilterGroup.BackgroundImage = Properties.Resources.UI_Filter_BG_6;
                    chkFilterCategoryTools.BackgroundImage = Properties.Resources.UI_Filter_Tool;
                    chkFilterCategoryFurniture.Checked = false;
                    chkFilterCategoryEverything.Checked = false;
                    chkFilterCategoryCarpet.Checked = false;
                    chkFilterCategoryWallpaper.Checked = false;
                    chkFilterCategoryClothing.Checked = false;
                    chkFilterCategoryStationery.Checked = false;
                    chkFilterCategoryGyroid.Checked = false;
                    chkFilterCategoryFossil.Checked = false;
                    chkFilterCategoryMusic.Checked = false;
                    chkFilterCategoryMiscellaneous.Checked = false;
                    intFilterList[0] = 5;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryTools.BackgroundImage = Properties.Resources.UI_Filter_Tool_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); // PopulateListView();
            }
        }

        private void chkFilterCategoryStationery_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkFilterCategoryStationery.Checked == true)
                {
                    imgFilterGroup.BackgroundImage = Properties.Resources.UI_Filter_BG_4;
                    chkFilterCategoryStationery.BackgroundImage = Properties.Resources.UI_Filter_Stationery;
                    chkFilterCategoryFurniture.Checked = false;
                    chkFilterCategoryEverything.Checked = false;
                    chkFilterCategoryCarpet.Checked = false;
                    chkFilterCategoryWallpaper.Checked = false;
                    chkFilterCategoryClothing.Checked = false;
                    chkFilterCategoryTools.Checked = false;
                    chkFilterCategoryGyroid.Checked = false;
                    chkFilterCategoryFossil.Checked = false;
                    chkFilterCategoryMusic.Checked = false;
                    chkFilterCategoryMiscellaneous.Checked = false;
                    intFilterList[0] = 6;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryStationery.BackgroundImage = Properties.Resources.UI_Filter_Stationery_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); // PopulateListView();
            }
        }

        private void chkFilterCategoryGyroid_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkFilterCategoryGyroid.Checked == true)
                {
                    imgFilterGroup.BackgroundImage = Properties.Resources.UI_Filter_BG_5;
                    chkFilterCategoryGyroid.BackgroundImage = Properties.Resources.UI_Filter_Gyroid;
                    chkFilterCategoryFurniture.Checked = false;
                    chkFilterCategoryEverything.Checked = false;
                    chkFilterCategoryCarpet.Checked = false;
                    chkFilterCategoryWallpaper.Checked = false;
                    chkFilterCategoryClothing.Checked = false;
                    chkFilterCategoryTools.Checked = false;
                    chkFilterCategoryStationery.Checked = false;
                    chkFilterCategoryFossil.Checked = false;
                    chkFilterCategoryMusic.Checked = false;
                    chkFilterCategoryMiscellaneous.Checked = false;
                    intFilterList[0] = 7;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryGyroid.BackgroundImage = Properties.Resources.UI_Filter_Gyroid_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); // PopulateListView();
            }
        }

        private void chkFilterCategoryFossil_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkFilterCategoryFossil.Checked == true)
                {
                    imgFilterGroup.BackgroundImage = Properties.Resources.UI_Filter_BG_3;
                    chkFilterCategoryFossil.BackgroundImage = Properties.Resources.UI_Filter_Dinosaur;
                    chkFilterCategoryFurniture.Checked = false;
                    chkFilterCategoryEverything.Checked = false;
                    chkFilterCategoryCarpet.Checked = false;
                    chkFilterCategoryWallpaper.Checked = false;
                    chkFilterCategoryClothing.Checked = false;
                    chkFilterCategoryTools.Checked = false;
                    chkFilterCategoryStationery.Checked = false;
                    chkFilterCategoryGyroid.Checked = false;
                    chkFilterCategoryMusic.Checked = false;
                    chkFilterCategoryMiscellaneous.Checked = false;
                    intFilterList[0] = 8;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryFossil.BackgroundImage = Properties.Resources.UI_Filter_Dinosaur_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); // PopulateListView();
            }
        }

        private void chkFilterCategoryMusic_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkFilterCategoryMusic.Checked == true)
                {
                    imgFilterGroup.BackgroundImage = Properties.Resources.UI_Filter_BG_3;
                    chkFilterCategoryMusic.BackgroundImage = Properties.Resources.UI_Filter_Music;
                    chkFilterCategoryFurniture.Checked = false;
                    chkFilterCategoryEverything.Checked = false;
                    chkFilterCategoryCarpet.Checked = false;
                    chkFilterCategoryWallpaper.Checked = false;
                    chkFilterCategoryClothing.Checked = false;
                    chkFilterCategoryTools.Checked = false;
                    chkFilterCategoryStationery.Checked = false;
                    chkFilterCategoryGyroid.Checked = false;
                    chkFilterCategoryFossil.Checked = false;
                    chkFilterCategoryMiscellaneous.Checked = false;
                    intFilterList[0] = 9;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryMusic.BackgroundImage = Properties.Resources.UI_Filter_Music_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); // PopulateListView();
            }
        }

        private void chkFilterCategoryMiscellaneous_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkFilterCategoryMiscellaneous.Checked == true)
                {
                    imgFilterGroup.BackgroundImage = Properties.Resources.UI_Filter_BG_6;
                    chkFilterCategoryMiscellaneous.BackgroundImage = Properties.Resources.UI_Filter_Pitfall;
                    chkFilterCategoryFurniture.Checked = false;
                    chkFilterCategoryEverything.Checked = false;
                    chkFilterCategoryCarpet.Checked = false;
                    chkFilterCategoryWallpaper.Checked = false;
                    chkFilterCategoryClothing.Checked = false;
                    chkFilterCategoryTools.Checked = false;
                    chkFilterCategoryStationery.Checked = false;
                    chkFilterCategoryGyroid.Checked = false;
                    chkFilterCategoryFossil.Checked = false;
                    chkFilterCategoryMusic.Checked = false;
                    intFilterList[0] = 10;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryMiscellaneous.BackgroundImage = Properties.Resources.UI_Filter_Pitfall_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); // PopulateListView();
            }
        }

        private void chkFilterCategoryEverything_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                lblFilterCategory.Text = "Category:  Everything";
                if (intFilterList[0] != 0)
                    HideFilterGroup();
                else
                    RevealFilterGroup();
            }
        }

        private void chkFilterCategoryFurniture_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                lblFilterCategory.Text = "Category:  Furniture";
                if (intFilterList[0] != 1)
                    HideFilterGroup();
                else
                    RevealFilterGroup();
            }
        }

        private void chkFilterCategoryCarpet_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                lblFilterCategory.Text = "Category:  Carpets";
                if (intFilterList[0] != 2)
                    HideFilterGroup();
                else
                    RevealFilterGroup();
            }
        }

        private void chkFilterCategoryWallpaper_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                lblFilterCategory.Text = "Category:  Wallpaper";
                if (intFilterList[0] != 3)
                    HideFilterGroup();
                else
                    RevealFilterGroup();
            }
        }

        private void chkFilterCategoryClothing_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                lblFilterCategory.Text = "Category:  Clothing";
                if (intFilterList[0] != 4)
                    HideFilterGroup();
                else
                    RevealFilterGroup();
            }
        }

        private void chkFilterCategoryTools_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                lblFilterCategory.Text = "Category:  Handhelds";
                if (intFilterList[0] != 5)
                    HideFilterGroup();
                else
                    RevealFilterGroup();
            }
        }

        private void chkFilterCategoryStationery_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                lblFilterCategory.Text = "Category:  Stationery";
                if (intFilterList[0] != 6)
                    HideFilterGroup();
                else
                    RevealFilterGroup();
            }
        }

        private void chkFilterCategoryGyroid_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                lblFilterCategory.Text = "Category:  Gyroids";
                if (intFilterList[0] != 7)
                    HideFilterGroup();
                else
                    RevealFilterGroup();
            }
        }

        private void chkFilterCategoryFossil_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                lblFilterCategory.Text = "Category:  Fossils";
                if (intFilterList[0] != 8)
                    HideFilterGroup();
                else
                    RevealFilterGroup();
            }
        }

        private void chkFilterCategoryMusic_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                lblFilterCategory.Text = "Category:  Music";
                if (intFilterList[0] != 9)
                    HideFilterGroup();
                else
                    RevealFilterGroup();
            }
        }

        private void chkFilterCategoryMiscellaneous_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                lblFilterCategory.Text = "Category:  Miscellaneous";
                if (intFilterList[0] != 10)
                    HideFilterGroup();
                else
                    RevealFilterGroup();
            }
        }

        private void ShopAround_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                RevealFilterGroup();
                UpdateFilterLabelText();
            }
        }

        private void chkFilterGroup1_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkFilterGroup1.Checked == true)
                {
                    chkFilterGroup2.Checked = false;
                    chkFilterGroup3.Checked = false;
                    chkFilterGroup4.Checked = false;
                    chkFilterGroup5.Checked = false;
                    chkFilterGroup6.Checked = false;
                    intFilterList[9 + intFilterList[0]] = 0;
                    UpdateFilterLabelText();
                }
                UpdateFilterGroupCheckboxes();
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); // PopulateListView();
            }
        }

        private void chkFilterGroup2_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkFilterGroup2.Checked == true)
                {
                    chkFilterGroup1.Checked = false;
                    chkFilterGroup3.Checked = false;
                    chkFilterGroup4.Checked = false;
                    chkFilterGroup5.Checked = false;
                    chkFilterGroup6.Checked = false;
                    intFilterList[9 + intFilterList[0]] = 1;
                    UpdateFilterLabelText();
                }
                UpdateFilterGroupCheckboxes();
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); // PopulateListView();
            }
        }

        private void chkFilterGroup3_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkFilterGroup3.Checked == true)
                {
                    chkFilterGroup2.Checked = false;
                    chkFilterGroup1.Checked = false;
                    chkFilterGroup4.Checked = false;
                    chkFilterGroup5.Checked = false;
                    chkFilterGroup6.Checked = false;
                    intFilterList[9 + intFilterList[0]] = 2;
                    UpdateFilterLabelText();
                }
                UpdateFilterGroupCheckboxes();
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); // PopulateListView();
            }
        }

        private void chkFilterGroup4_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkFilterGroup4.Checked == true)
                {
                    chkFilterGroup2.Checked = false;
                    chkFilterGroup3.Checked = false;
                    chkFilterGroup1.Checked = false;
                    chkFilterGroup5.Checked = false;
                    chkFilterGroup6.Checked = false;
                    intFilterList[9 + intFilterList[0]] = 3;
                    UpdateFilterLabelText();
                }
                UpdateFilterGroupCheckboxes();
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); // PopulateListView();
            }
        }

        private void chkFilterGroup5_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkFilterGroup5.Checked == true)
                {
                    chkFilterGroup2.Checked = false;
                    chkFilterGroup3.Checked = false;
                    chkFilterGroup4.Checked = false;
                    chkFilterGroup1.Checked = false;
                    chkFilterGroup6.Checked = false;
                    intFilterList[9 + intFilterList[0]] = 4;
                    UpdateFilterLabelText();
                }
                UpdateFilterGroupCheckboxes();
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); // PopulateListView();
            }
        }

        private void chkFilterGroup6_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkFilterGroup6.Checked == true)
                {
                    chkFilterGroup2.Checked = false;
                    chkFilterGroup3.Checked = false;
                    chkFilterGroup4.Checked = false;
                    chkFilterGroup5.Checked = false;
                    chkFilterGroup1.Checked = false;
                    intFilterList[9 + intFilterList[0]] = 5;
                    UpdateFilterLabelText();
                }
                UpdateFilterGroupCheckboxes();
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); // PopulateListView();
            }
        }

        private void chkFilterGroup1_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                switch (intFilterList[0])
                {
                    case 0: lblFilterGroup.Text = ""; break;
                    case 1: lblFilterGroup.Text = "Group:  Everything"; break;
                    case 2: lblFilterGroup.Text = ""; break;
                    case 3: lblFilterGroup.Text = ""; break;
                    case 4: lblFilterGroup.Text = ""; break;
                    case 5: lblFilterGroup.Text = "Group:  Everything"; break;
                    case 6: lblFilterGroup.Text = ""; break;
                    case 7: lblFilterGroup.Text = ""; break;
                    case 8: lblFilterGroup.Text = ""; break;
                    case 9: lblFilterGroup.Text = ""; break;
                    case 10: lblFilterGroup.Text = "Group:  Everything"; break;
                }
            }
        }

        private void chkFilterGroup2_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                switch (intFilterList[0])
                {
                    case 0: lblFilterGroup.Text = ""; break;
                    case 1: lblFilterGroup.Text = "Group:  Nook"; break;
                    case 2: lblFilterGroup.Text = ""; break;
                    case 3: lblFilterGroup.Text = ""; break;
                    case 4: lblFilterGroup.Text = "Group:  Everything"; break;
                    case 5: lblFilterGroup.Text = "Group:  Tools"; break;
                    case 6: lblFilterGroup.Text = ""; break;
                    case 7: lblFilterGroup.Text = "Group:  Everything"; break;
                    case 8: lblFilterGroup.Text = ""; break;
                    case 9: lblFilterGroup.Text = ""; break;
                    case 10: lblFilterGroup.Text = "Group:  Bells"; break;
                }
            }
        }

        private void chkFilterGroup3_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                switch (intFilterList[0])
                {
                    case 0: lblFilterGroup.Text = "Group:  Everything"; break;
                    case 1: lblFilterGroup.Text = "Group:  Redd"; break;
                    case 2: lblFilterGroup.Text = "Group:  Everything"; break;
                    case 3: lblFilterGroup.Text = "Group:  Everything"; break;
                    case 4: lblFilterGroup.Text = "Group:  Nook"; break;
                    case 5: lblFilterGroup.Text = "Group:  Umbrellas"; break;
                    case 6: lblFilterGroup.Text = "Group:  Everything"; break;
                    case 7: lblFilterGroup.Text = "Group:  Mini Gyroids"; break;
                    case 8: lblFilterGroup.Text = ""; break;
                    case 9: lblFilterGroup.Text = ""; break;
                    case 10: lblFilterGroup.Text = "Group:  Nature"; break;
                }
            }
        }

        private void chkFilterGroup4_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                switch (intFilterList[0])
                {
                    case 0: lblFilterGroup.Text = "Group:  Nook"; break;
                    case 1: lblFilterGroup.Text = "Group:  Raffle"; break;
                    case 2: lblFilterGroup.Text = "Group:  Nook"; break;
                    case 3: lblFilterGroup.Text = "Group:  Nook"; break;
                    case 4: lblFilterGroup.Text = "Group:  Gracie"; break;
                    case 5: lblFilterGroup.Text = "Group:  Fans"; break;
                    case 6: lblFilterGroup.Text = "Group:  Nook"; break;
                    case 7: lblFilterGroup.Text = "Group:  Gyroids"; break;
                    case 8: lblFilterGroup.Text = "Group:  Everything"; break;
                    case 9: lblFilterGroup.Text = "Group:  Everything"; break;
                    case 10: lblFilterGroup.Text = "Group:  Seasonal"; break;
                }
            }
        }

        private void chkFilterGroup5_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                switch (intFilterList[0])
                {
                    case 0: lblFilterGroup.Text = "Group:  Seasonal"; break;
                    case 1: lblFilterGroup.Text = "Group:  Seasonal"; break;
                    case 2: lblFilterGroup.Text = "Group:  Saharah"; break;
                    case 3: lblFilterGroup.Text = "Group:  Wendell"; break;
                    case 4: lblFilterGroup.Text = "Group:  Seasonal"; break;
                    case 5: lblFilterGroup.Text = "Group:  Pinwheels"; break;
                    case 6: lblFilterGroup.Text = "Group:  Seasonal"; break;
                    case 7: lblFilterGroup.Text = "Group:  Mega Gyroids"; break;
                    case 8: lblFilterGroup.Text = "Group:  Common"; break;
                    case 9: lblFilterGroup.Text = "Group:  Common"; break;
                    case 10: lblFilterGroup.Text = "Group:  Other"; break;
                }
            }
        }

        private void chkFilterGroup6_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                switch (intFilterList[0])
                {
                    case 0: lblFilterGroup.Text = "Group:  Other"; break;
                    case 1: lblFilterGroup.Text = "Group:  Other"; break;
                    case 2: lblFilterGroup.Text = "Group:  Seasonal"; break;
                    case 3: lblFilterGroup.Text = "Group:  Seasonal"; break;
                    case 4: lblFilterGroup.Text = "Group:  Beta"; break;
                    case 5: lblFilterGroup.Text = "Group:  Balloons"; break;
                    case 6: lblFilterGroup.Text = "Group:  Other"; break;
                    case 7: lblFilterGroup.Text = "Group:  Tall Gyroids"; break;
                    case 8: lblFilterGroup.Text = "Group:  Dinosaurs"; break;
                    case 9: lblFilterGroup.Text = "Group:  Secret"; break;
                    case 10: lblFilterGroup.Text = "Group:  Beta"; break;
                }
            }
        }

        private void chkAvailabilityAll_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkAvailabilityAll.Checked == true)
                {
                    chkAvailabilityAll.BackgroundImage = Properties.Resources.UI_Filter_All;
                    chkAvailabilityYes.Checked = false;
                    chkAvailabilityNo.Checked = false;
                    intFilterList[1] = 1;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                }
                else
                {
                    chkAvailabilityAll.BackgroundImage = Properties.Resources.UI_Filter_All_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); // PopulateListView();
            }
        }

        private void chkAvailabilityYes_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkAvailabilityYes.Checked == true)
                {
                    chkAvailabilityYes.BackgroundImage = Properties.Resources.UI_Filter_Complete;
                    chkAvailabilityAll.Checked = false;
                    chkAvailabilityNo.Checked = false;
                    intFilterList[1] = 2;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                }
                else
                {
                    chkAvailabilityYes.BackgroundImage = Properties.Resources.UI_Filter_Complete_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); // PopulateListView();
            }
        }

        private void chkAvailabilityNo_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkAvailabilityNo.Checked == true)
                {
                    chkAvailabilityNo.BackgroundImage = Properties.Resources.UI_Filter_Incomplete;
                    intFilterList[1] = 3;
                    chkAvailabilityAll.Checked = false;
                    chkAvailabilityYes.Checked = false;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                }
                else
                {
                    chkAvailabilityNo.BackgroundImage = Properties.Resources.UI_Filter_Incomplete_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); // PopulateListView();
            }
        }

        private void chkSortCatalog_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkSortCatalog.Checked == true)
                {
                    chkSortCatalog.BackgroundImage = Properties.Resources.UI_Filter_Other;
                    chkSortAlpha.Checked = false;
                    chkSortPrice.Checked = false;
                    intFilterList[2] = 0;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                }
                else
                {
                    chkSortCatalog.BackgroundImage = Properties.Resources.UI_Filter_Other_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); // PopulateListView();
            }
        }

        private void chkSortAlpha_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkSortAlpha.Checked == true)
                {
                    chkSortAlpha.BackgroundImage = Properties.Resources.UI_Filter_Alphabetized;
                    chkSortCatalog.Checked = false;
                    chkSortPrice.Checked = false;
                    intFilterList[2] = 1;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                }
                else
                {
                    chkSortAlpha.BackgroundImage = Properties.Resources.UI_Filter_Alphabetized_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); // PopulateListView();
            }
        }

        private void chkSortPrice_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkSortPrice.Checked == true)
                {
                    chkSortPrice.BackgroundImage = Properties.Resources.UI_Filter_Price;
                    chkSortCatalog.Checked = false;
                    chkSortAlpha.Checked = false;
                    intFilterList[2] = 2;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                }
                else
                {
                    chkSortPrice.BackgroundImage = Properties.Resources.UI_Filter_Price_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) lstShop_UpdateFilterAndSort(txtFilterSearch.Text); // PopulateListView();
            }
        }

        private void chkAvailabilityAll_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
                lblAvailability.Text = "Everything";
        }

        private void chkAvailabilityYes_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
                lblAvailability.Text = "Available";
        }

        private void chkAvailabilityNo_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
                lblAvailability.Text = "Unavailable";
        }

        private void chkSortCatalog_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
                lblSort.Text = "Catalog Order";
        }

        private void chkSortAlpha_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
                lblSort.Text = "Alphabetized";
        }

        private void chkSortPrice_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
                lblSort.Text = "Ticket Price";
        }

        private void btnDialogSelectYes_MouseEnter(object sender, EventArgs e)
        {
            btnDialogSelectYes.ForeColor = Color.Blue;
        }

        private void btnDialogSelectNo_MouseEnter(object sender, EventArgs e)
        {
            btnDialogSelectNo.ForeColor = Color.Blue;
        }

        private void btnDialogSelectYes_MouseLeave(object sender, EventArgs e)
        {
            btnDialogSelectYes.ForeColor = Color.Orange;
        }

        private void btnDialogSelectNo_MouseLeave(object sender, EventArgs e)
        {
            btnDialogSelectNo.ForeColor = Color.Orange;
        }

        private void ShopAround_Load(object sender, EventArgs e)
        {

        }

        private void txtFilterSearch_TextChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
                lstShop_UpdateFilterAndSort(txtFilterSearch.Text); // PopulateListView();
        }
    }
}

