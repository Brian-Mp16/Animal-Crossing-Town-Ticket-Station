using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Formats.Asn1;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Animal_Crossing_Town_Ticket_Station
{
    public partial class Catalog : Form
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection();
        Font? fontFink18;
        Font? fontFink20;
        Font? fontFink21;
        Font? fontFink24;
        Font? fontFink28;
        DateTime loadTime;
        TimeSpan timeOffset = new TimeSpan(0, 0, 0, 0, 0);
        ImageList imgList = new ImageList();
        static string filedir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\ACTownTickets\";
        static string filepath = filedir + "SaveData.txt";
        static int intDataLines = 50;
        string[] data = new string[intDataLines];
        int intTimePlayedSeconds = 0;
        static int intItemsTotal = 1395;
        int[] intBoughtList = new int[intItemsTotal + 1];
        string[] strCodeList = new string[intItemsTotal + 1];
        int[] intRedeemedList = new int[intItemsTotal + 1];
        static int intFilterListCatalogTotal = 20;
        int[] intFilterListCatalog = new int[intFilterListCatalogTotal];
        bool boolItemSelected = false;
        bool boolLoading = true;
        int intLastViewedDialog = 0;
        ListViewItem? lviSelectedItem = null;
        ListViewItem? previousListViewItem = null;
        Color previousListViewItemColor = Color.Black;
        bool boolFastCodeShow = false;

        public static void SetDoubleBuffering(Control control, bool value)
        {
            PropertyInfo controlProperty = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
            controlProperty.SetValue(control, value, null);
        }

        public Catalog()
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
            SetDoubleBuffering(lstShop, true);
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

            float dx = this.CreateGraphics().DpiX;
            fontFink18 = new Font(fonts.Families[0], 18.0F * 96.0F / dx);
            fontFink20 = new Font(fonts.Families[0], 20.25F * 96.0F / dx);
            fontFink21 = new Font(fonts.Families[0], 21.0F * 96.0F / dx);
            fontFink24 = new Font(fonts.Families[0], 24.0F * 96.0F / dx);
            fontFink28 = new Font(fonts.Families[0], 27.75F * 96.0F / dx);

            btnBack.Font = fontFink24;
            lstShop.Font = fontFink21;
            lblDialogTimmy.Font = fontFink28;
            lblFilterCategory.Font = fontFink18;
            lblFilterGroup.Font = fontFink18;
            lblSort.Font = fontFink18;
            lblAvailability.Font = fontFink18;
            lblFengShui.Font = fontFink24;
            lblFengShuiColor.Font = fontFink24;
            lblTimesBought.Font = fontFink24;
            btnDialogSelectYes.Font = fontFink28;
            btnDialogSelectNo.Font = fontFink28;
            txtFilterSearch.Font = fontFink18;
            lblCodeFastCheckbox.Font = fontFink20;
            lblPickup.Font = fontFink20;
            lblCodeDescription.Font = fontFink20;
        }

        private void loadData()
        {
            data = SaveData.GetSaveData().Item1.Data;

            loadTime = data[0] != "" ? Convert.ToDateTime(data[0]) : DateTime.Now;
            if (data[1] != "")
                timeOffset = TimeSpan.Parse(data[1]);
            intTimePlayedSeconds = data[27] != "" ? Convert.ToInt32(data[27]) : 0;
            string[] strFilterListData = data[31].Split(',');
            for (int i = 0; i < intFilterListCatalogTotal; i++)
                intFilterListCatalog[i] = data[31] != "" ? Convert.ToInt32(strFilterListData[i]) : 0;
            string[] strBoughtListData = data[35].Split(',');
            for (int i = 0; i < intItemsTotal + 1; i++)
                intBoughtList[i] = data[35] != "" ? Convert.ToInt32(strBoughtListData[i]) : 0;
            string[] strCodeListData = data[36].Split(',');
            for (int i = 0; i < intItemsTotal + 1; i++)
                strCodeList[i] = data[36] != "" ? strCodeListData[i] : "";
            string[] strRedeemedListData = data[40].Split(',');
            for (int i = 0; i < intItemsTotal + 1; i++)
                intRedeemedList[i] = data[40] != "" ? Convert.ToInt32(strRedeemedListData[i]) : 0;
            boolFastCodeShow = data[41].ToLower() == "true" ? true : false;

            if (boolFastCodeShow == true)
                chkCodeFast.Checked = true;
            else
                chkCodeFast.Checked = false;

        }

        private void saveData()
        {
            data = SaveData.GetSaveData().Item1.Data;
            string sb = "";

            data[1] = timeOffset.ToString();
            data[27] = intTimePlayedSeconds.ToString();
            sb = "";
            for (int i = 0; i < intFilterListCatalogTotal; i++)
                sb += intFilterListCatalog[i] + ",";
            data[31] = sb;
            sb = "";
            for (int i = 0; i < intItemsTotal + 1; i++)
                sb += strCodeList[i] + ",";
            data[36] = sb;
            sb = "";
            for (int i = 0; i < intItemsTotal + 1; i++)
                sb += intRedeemedList[i] + ",";
            data[40] = sb;
            data[41] = boolFastCodeShow.ToString();

            File.WriteAllLines(filepath, data);
        }

        private void UpdateClock(object sender, EventArgs e)
        {
            GC.Collect();

            intTimePlayedSeconds++;
            UpdateClockImages();

            if (boolLoading == true)
                PopulateListView();
            boolLoading = false;
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

        private void UpdateShopDialogLabelText(int intDialogOption, string itemName, int itemTickets)
        {
            imgShopDialog.Visible = true;

            lblFengShuiColor.Location = imgShopDialog.PointToClient(lblFengShuiColor.Parent.PointToScreen(lblFengShuiColor.Location));
            lblFengShuiColor.Parent = imgShopDialog;
            lblDialogTimmy.Location = imgShopDialog.PointToClient(lblDialogTimmy.Parent.PointToScreen(lblDialogTimmy.Location));
            lblDialogTimmy.Parent = imgShopDialog;

            lblDialogTimmy.Visible = true;

            if (boolItemSelected == false && intDialogOption == 0)
            {
                lblDialogTimmy.Text = "Welcome valued customer! This is your\r\npersonal catalog, yes, yes. Please specify\r\nwhich item you'd like to claim.";
            }

            if (boolItemSelected == true && intDialogOption == 1)
            {
                imgDialogSelect.Location = this.PointToClient(imgDialogSelect.Parent.PointToScreen(imgDialogSelect.Location));
                imgDialogSelect.Parent = this;
                imgDialogSelect.BackColor = Color.Transparent;
                btnDialogSelectYes.Location = imgDialogSelect.PointToClient(btnDialogSelectYes.Parent.PointToScreen(btnDialogSelectYes.Location));
                btnDialogSelectYes.Parent = imgDialogSelect;
                btnDialogSelectNo.Location = imgDialogSelect.PointToClient(btnDialogSelectNo.Parent.PointToScreen(btnDialogSelectNo.Location));
                btnDialogSelectNo.Parent = imgDialogSelect;

                lblCodeFast.Visible = true;
                lblCodeYours.Visible = true;
                lblCodeDescription.Visible = true;
                imgDialogSelect.Visible = true;
                imgShopItem.BackgroundImage = null;
                btnDialogSelectYes.Visible = true;
                btnDialogSelectNo.Visible = true;

                lblDialogTimmy.Text = String.Format("Your {0} is ready\r\nfor pick up! Please let me know when\r\nyou receive it in your game.", itemName);
            }

            if (boolItemSelected == false && intDialogOption == 2)
            {
                intLastViewedDialog = 4;
                lblDialogTimmy.Text = "I'm sorry, it looks like you've already\r\npicked up that item. Please visit the\r\nShop to place another order for it!";
            }

            if (boolItemSelected == false && intDialogOption == 3)
            {
                lblDialogTimmy.Text = "Thank you for your patronage!\r\nPlease let me know if you'd like to\r\nclaim anything else today.";
            }

            if (boolItemSelected == false && intDialogOption == 4)
            {
                lblDialogTimmy.Text = "Feel free to browse at your leisure!\r\nPlease let me know if you'd like to\r\nclaim anything else today.";
            }
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

        private void PopulateListView()
        {
            lstShop.BeginUpdate();
            lstShop.Items.Clear();

            for (int i = 1; i < intItemsTotal + 1; i++)
            {
                Tuple<ItemInfo, int> itemCodeData = new Tuple<ItemInfo, int>(null, 0);
                switch (intFilterListCatalog[2])
                {
                    case 0: itemCodeData = ItemData.GetItemCheckByCatalogIndex(i, out int _); break;
                    case 1: itemCodeData = ItemData.GetItemCheckByAlphaIndex(i, out int _); break;
                    case 2: itemCodeData = ItemData.GetItemCheckByTicketIndex(i, out int _); break;
                    default: itemCodeData = ItemData.GetItemCheckByCatalogIndex(i, out int _); break;
                }

                int itemIcon = 0;
                switch (itemCodeData.Item1.ItemType)
                {
                    case "Furniture": itemIcon = 0; break;
                    case "Carpet": itemIcon = 1; break;
                    case "Wallpaper": itemIcon = 2; break;
                    case "Clothing": itemIcon = 3; break;
                    case "Tool":
                        switch (itemCodeData.Item1.ItemCategory)
                        {
                            case "Tools": itemIcon = 4; break;
                            case "Umbrellas": itemIcon = 10; break;
                            case "Fans": itemIcon = 11; break;
                            case "Pinwheels": itemIcon = 12; break;
                            case "Balloons": itemIcon = 13; break;
                            default: itemIcon = 4; break;
                        }
                        break;
                    case "Stationery": itemIcon = 5; break;
                    case "Gyroid": itemIcon = 6; break;
                    case "Fossil": itemIcon = 7; break;
                    case "Music": itemIcon = 8; break;
                    case "Miscellaneous": itemIcon = 9; break;
                    default: itemIcon = 0; break;
                }

                if (CheckShopFilters(itemCodeData) == true)
                {
                    bool boolAvailable = CheckCatalogAvailability(itemCodeData);
                    if (intBoughtList[itemCodeData.Item1.CatalogIndex] > 0 && strCodeList[itemCodeData.Item1.CatalogIndex] != "")
                    {
                        if (intFilterListCatalog[1] == 1 || intFilterListCatalog[1] == 2 && boolAvailable == true || intFilterListCatalog[1] == 3)
                        {
                            if (itemCodeData.Item1.Name.ToLower().Contains(txtFilterSearch.Text.ToLower()))
                            {
                                ListViewItem item = new ListViewItem();
                                item.ForeColor = GetListViewItemColor(intBoughtList[itemCodeData.Item1.CatalogIndex], intRedeemedList[itemCodeData.Item1.CatalogIndex]);
                                item.Text = " " + itemCodeData.Item1.Name;
                                item.ImageIndex = itemIcon;
                                lstShop.Items.Add(item).SubItems.Add(itemCodeData.Item1.Tickets.ToString("N0") + " ");
                            }
                        }
                    }
                }
            }

            lstShop.EndUpdate();

            saveData();
        }

        private void ChangeCheckboxes()
        {
            switch (intFilterListCatalog[0])
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
            if (intFilterListCatalog[1] == 0)
                intFilterListCatalog[1] = 2;
            switch (intFilterListCatalog[1])
            {
                case 1: chkAvailabilityAll.Checked = true; break;
                case 2: chkAvailabilityYes.Checked = true; break;
                case 3: chkAvailabilityNo.Checked = true; break;
                default: break;
            }
            switch (intFilterListCatalog[2])
            {
                case 0: chkSortCatalog.Checked = true; break;
                case 1: chkSortAlpha.Checked = true; break;
                case 2: chkSortPrice.Checked = true; break;
                default: break;
            }

            UpdateFilterLabelText();

            chkCodeFast.Appearance = Appearance.Button;
            chkCodeFast.Cursor = Cursors.Hand;
            chkCodeFast.AutoSize = false;
            chkCodeFast.Size = new Size(52, 52);
            chkCodeFast.BackgroundImage = Properties.Resources.UI_Catalog_No;
            chkCodeFast.FlatStyle = FlatStyle.Flat;
            chkCodeFast.FlatAppearance.BorderSize = 0;
            chkCodeFast.BackColor = Color.FromArgb(255, 152, 104, 72);
            chkCodeFast.ForeColor = Color.FromArgb(255, 152, 104, 72);
            if (chkCodeFast.Checked == true)
            {
                lblCodeFastCheckbox.ForeColor = Color.LimeGreen;
                chkCodeFast.BackgroundImage = Properties.Resources.UI_Catalog_Yes;
            }
            else
            {
                lblCodeFastCheckbox.ForeColor = Color.Red;
                chkCodeFast.BackgroundImage = Properties.Resources.UI_Catalog_No;
            }

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

            if (intFilterListCatalog[9] < 2) intFilterListCatalog[9] = 2;
            if (intFilterListCatalog[10] < 0) intFilterListCatalog[10] = 0;
            if (intFilterListCatalog[11] < 2) intFilterListCatalog[11] = 2;
            if (intFilterListCatalog[12] < 2) intFilterListCatalog[12] = 2;
            if (intFilterListCatalog[13] < 1) intFilterListCatalog[13] = 1;
            if (intFilterListCatalog[14] < 0) intFilterListCatalog[14] = 0;
            if (intFilterListCatalog[15] < 2) intFilterListCatalog[15] = 2;
            if (intFilterListCatalog[16] < 1) intFilterListCatalog[16] = 1;
            if (intFilterListCatalog[17] < 3) intFilterListCatalog[17] = 3;
            if (intFilterListCatalog[18] < 3) intFilterListCatalog[18] = 3;
            if (intFilterListCatalog[19] < 0) intFilterListCatalog[19] = 0;

            UpdateFilterGroupCheckboxes();

            if (chkAvailabilityAll.Checked == false && chkAvailabilityYes.Checked == false && chkAvailabilityNo.Checked == false)
                chkAvailabilityYes.Checked = true;

            if (chkSortCatalog.Checked == false && chkSortAlpha.Checked == false && chkSortPrice.Checked == false)
                chkSortCatalog.Checked = true;

            imgFilterGroupHide.BringToFront();
        }

        private bool CheckShopFilters(Tuple<ItemInfo, int> itemCodeData)
        {
            bool boolPass = false;
            string strItemType = "";
            string strItemCategory = "";

            switch (intFilterListCatalog[0])
            {
                case 0:
                    strItemType = "Everything";
                    switch (intFilterListCatalog[9])
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
                    switch (intFilterListCatalog[10])
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
                    switch (intFilterListCatalog[11])
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
                    switch (intFilterListCatalog[12])
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
                    switch (intFilterListCatalog[13])
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
                    switch (intFilterListCatalog[14])
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
                    switch (intFilterListCatalog[15])
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
                    switch (intFilterListCatalog[16])
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
                    switch (intFilterListCatalog[17])
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
                    switch (intFilterListCatalog[18])
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
                    switch (intFilterListCatalog[19])
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
                strItemType == "Everything" && strItemCategory == "Nook" && itemCodeData.Item1.Availability == 9 ||
                strItemType == "Everything" && strItemCategory == "Nook" && itemCodeData.Item1.Availability == 8 ||
                strItemType == "Everything" && strItemCategory == "Other" && itemCodeData.Item1.ItemCategory != "Nook" && itemCodeData.Item1.ItemCategory != "Seasonal" ||
                strItemType == "Furniture" && strItemCategory == "Nook" && itemCodeData.Item1.Availability == 9 ||
                strItemType == "Clothing" && strItemCategory == "Nook" && itemCodeData.Item1.Availability == 8 ||
                strItemType == "Everything" && strItemCategory == "Seasonal" && itemCodeData.Item1.ItemCategory == "Pinwheels" ||
                strItemType == "Everything" && strItemCategory == "Seasonal" && itemCodeData.Item1.ItemCategory == "Fans" ||
                itemCodeData.Item1.ItemType == strItemType && strItemCategory == "Everything" ||
                itemCodeData.Item1.ItemType == strItemType && itemCodeData.Item1.ItemCategory == strItemCategory)
                boolPass = true;

            return boolPass;
        }

        private void UpdateFilterGroupCheckboxes()
        {
            switch (intFilterListCatalog[0])
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
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
            switch (intFilterListCatalog[0])
            {
                case 0:
                    lblFilterCategory.Text = "Category:  Everything";
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
                    switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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

            switch (intFilterListCatalog[1])
            {
                case 1: lblAvailability.Text = "Everything"; break;
                case 2: lblAvailability.Text = "Available"; break;
                case 3: lblAvailability.Text = "Unavailable"; break;
                default: break;
            }

            switch (intFilterListCatalog[2])
            {
                case 0: lblSort.Text = "Catalog Order"; break;
                case 1: lblSort.Text = "Alphabetized"; break;
                case 2: lblSort.Text = "Ticket Price"; break;
                default: break;
            }
        }

        private void CheckFiltersCategory()
        {
            if (chkFilterCategoryEverything.Checked == false && chkFilterCategoryFurniture.Checked == false && chkFilterCategoryCarpet.Checked == false &&
                chkFilterCategoryWallpaper.Checked == false && chkFilterCategoryClothing.Checked == false && chkFilterCategoryTools.Checked == false &&
                chkFilterCategoryStationery.Checked == false && chkFilterCategoryGyroid.Checked == false && chkFilterCategoryFossil.Checked == false &&
                chkFilterCategoryMusic.Checked == false && chkFilterCategoryMiscellaneous.Checked == false)
            {
                switch (intFilterListCatalog[0])
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
                switch (intFilterListCatalog[9 + intFilterListCatalog[0]])
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
                switch (intFilterListCatalog[1])
                {
                    case 1: chkAvailabilityAll.Checked = true; break;
                    case 2: chkAvailabilityYes.Checked = true; break;
                    case 3: chkAvailabilityNo.Checked = true; break;
                    default: chkAvailabilityYes.Checked = true; break;
                }
            }

            if (chkSortCatalog.Checked == false && chkSortAlpha.Checked == false && chkSortPrice.Checked == false)
            {
                switch (intFilterListCatalog[2])
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

        private Color GetListViewItemColor(int intTimesBought, int intTimesRedeemed)
        {
            Color color = Color.Black;
            if (intTimesBought - intTimesRedeemed == 0)
                color = Color.Gray;

            return color;
        }

        private void UpdateItemLabels(Tuple<ItemInfo, int> itemCodeData)
        {
            lblPickup.Visible = true;
            imgPickup.Visible = true;
            if (intBoughtList[itemCodeData.Item1.CatalogIndex] - intRedeemedList[itemCodeData.Item1.CatalogIndex] > 0)
            {
                lblPickup.ForeColor = Color.LimeGreen;
                lblPickup.Text = "Available for pick up";
                imgPickup.Image = Properties.Resources.UI_Catalog_Yes;
            }
            else
            {
                lblPickup.ForeColor = Color.Red;
                lblPickup.Text = "Item has been received";
                imgPickup.Image = Properties.Resources.UI_Catalog_No;
            }

            lblTimesBought.Text = String.Format("Times Redeemed:  {0}", intRedeemedList[itemCodeData.Item1.CatalogIndex]);

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
            boolItemSelected = false;

            txtFilterSearch.ReadOnly = false;
            txtFilterSearch.BackColor = Color.PapayaWhip;
            txtFilterSearch.Enabled = true;

            lblCodeYours.Visible = false;
            lblCodeFast.Visible = false;
            lblCodeDescription.Visible = false;
            imgPickup.Visible = false;
            lblPickup.Visible = false;

            imgDialogSelect.Visible = false;
            btnDialogSelectYes.Visible = false;
            btnDialogSelectNo.Visible = false;
            lstShop.SelectedItems.Clear();

            imgShopItemSmall.BackgroundImage = null;
            imgShopItem.Visible = true;
            imgShopItem.BackgroundImage = null;
            lstShop.SelectedItems.Clear();

            int index = lviSelectedItem.Index;

            PopulateListView();


            if (index < lstShop.Items.Count)
            {
                lstShop.Items[index].Selected = true;
                lstShop.EnsureVisible(index);

                lviSelectedItem.BackColor = Color.FromArgb(255, 193, 131, 106);
                lviSelectedItem.ForeColor = Color.FromArgb(255, 32, 32, 32);
            }

            lstShop.SelectedItems.Clear();
            lviSelectedItem = null;
        }

        private bool CheckCatalogAvailability(Tuple<ItemInfo, int> itemCodeData)
        {
            bool boolPass = false;

            if (intBoughtList[itemCodeData.Item1.CatalogIndex] - intRedeemedList[itemCodeData.Item1.CatalogIndex] > 0)
            {
                boolPass = true;
            }

            return boolPass;
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            saveData();
            this.Hide();
        }

        private void lstShop_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            if (boolItemSelected == false)
            {
                lstShop.SelectedItems.Clear();
                if (previousListViewItem != null)
                {
                    previousListViewItem.ForeColor = previousListViewItemColor;
                    previousListViewItem.BackColor = Color.FromArgb(255, 193, 131, 106);
                }

                previousListViewItemColor = e.Item.ForeColor;
                e.Item.BackColor = Color.FromArgb(255, 255, 150, 58);
                e.Item.ForeColor = Color.White;

                Tuple<ItemInfo, int> itemCodeData = ItemData.GetItemCheckByName(e.Item.Text.Substring(1, e.Item.Text.Length - 1), out int _);
                imgShopDialog.Visible = false;
                lblDialogTimmy.Visible = false;

                UpdateItemLabels(itemCodeData);

                imgShopItem.BackgroundImage = ResizeImage(itemCodeData.Item1.ImageName, 500, 500);

                previousListViewItem = e.Item;
            }
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

                lstShop.SelectedItems.Clear();
                if (previousListViewItem != null)
                {
                    previousListViewItem.ForeColor = previousListViewItemColor;
                    previousListViewItem.BackColor = Color.FromArgb(255, 193, 131, 106);
                }

                previousListViewItemColor = item.ForeColor;
                item.BackColor = Color.FromArgb(255, 255, 150, 58);
                item.ForeColor = Color.White;

                Tuple<ItemInfo, int> itemCodeData = ItemData.GetItemCheckByName(item.Text.Substring(1, item.Text.Length - 1), out int _);
                imgShopDialog.Visible = false;
                lblDialogTimmy.Visible = false;

                UpdateItemLabels(itemCodeData);

                imgShopItem.BackgroundImage = ResizeImage(itemCodeData.Item1.ImageName, 500, 500);
                imgShopItem.Refresh();
                lstShop.Refresh();

                previousListViewItem = item;
            }
        }

        private void lstShop_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (lstShop.SelectedItems.Count != 0 && lstShop.FocusedItem != null)
            {
                if (previousListViewItem != null)
                {
                    previousListViewItem.ForeColor = previousListViewItemColor;
                    previousListViewItem.BackColor = Color.FromArgb(255, 193, 131, 106);
                }
                Tuple<ItemInfo, int> itemCodeData = ItemData.GetItemCheckByName(lstShop.FocusedItem.Text.Substring(1, lstShop.FocusedItem.Text.Length - 1), out int _);

                UpdateItemLabels(itemCodeData);

                if (CheckCatalogAvailability(itemCodeData) == true)
                {
                    boolItemSelected = true;
                    lviSelectedItem = e.Item;
                    txtFilterSearch.ReadOnly = true;
                    txtFilterSearch.BackColor = Color.Gainsboro;
                    txtFilterSearch.Enabled = false;
                    imgShopItem.BackgroundImage = null;
                    imgShopItemSmall.BackgroundImage = ResizeImage(itemCodeData.Item1.ImageName, 200, 200);


                    lblCodeYours.Text = strCodeList[itemCodeData.Item1.CatalogIndex].Substring(0, 14) + "\n" + strCodeList[itemCodeData.Item1.CatalogIndex].Substring(14, 14);
                    if (chkCodeFast.Checked == true)
                    {
                        lblCodeDescription.Text = "Your\nCode:\n\nFast\nCode:";
                        if (itemCodeData.Item1.CodeFast.Length > 14)
                            lblCodeFast.Text = itemCodeData.Item1.CodeFast.Substring(0, 14) + "\n" + itemCodeData.Item1.CodeFast.Substring(14, 14);
                        else
                            lblCodeFast.Text = "\nUnavailable";
                    }
                    else
                    {
                        lblCodeDescription.Text = "Your\nCode:";
                        lblCodeFast.Text = "";
                    }

                    UpdateShopDialogLabelText(1, itemCodeData.Item1.Name, 0);
                }
                else
                {
                    imgShopItem.BackgroundImage = null;
                    imgShopItemSmall.BackgroundImage = null;
                    imgDialogSelect.Visible = false;
                    btnDialogSelectYes.Visible = false;
                    btnDialogSelectNo.Visible = false;
                    boolItemSelected = false;
                    intLastViewedDialog = 2;
                    UpdateShopDialogLabelText(2, "", 0);
                }
            }
        }

        private void lstShop_MouseLeave(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                lblPickup.Visible = false;
                imgPickup.Visible = false;

                lblFengShui.Text = "";
                lblFengShuiColor.Text = "";
                imgShopDialog.Visible = true;
                lblDialogTimmy.Visible = true;
                lblTimesBought.Text = "";
                if (previousListViewItem != null)
                {
                    previousListViewItem.ForeColor = previousListViewItemColor;
                    previousListViewItem.BackColor = Color.FromArgb(255, 193, 131, 106);
                }
                UpdateShopDialogLabelText(intLastViewedDialog, "", 0);

                imgShopItem.BackgroundImage = null;
            }
        }

        private void btnDialogSelectYes_Click(object sender, EventArgs e)
        {
            //TODO Show Codes + Fast Code
            if (boolItemSelected == true)
            {
                Tuple<ItemInfo, int> itemCodeData = ItemData.GetItemCheckByName(lviSelectedItem.Text.Substring(1, lviSelectedItem.Text.Length - 1), out int _);
                if (intBoughtList[itemCodeData.Item1.CatalogIndex] - intRedeemedList[itemCodeData.Item1.CatalogIndex] > 0)
                {
                    //strCodeList[itemCodeData.Item1.CatalogIndex] = "";
                    intRedeemedList[itemCodeData.Item1.CatalogIndex]++;

                    saveData();
                    SelectedItemOptionSelected();
                    intLastViewedDialog = 3;
                    UpdateShopDialogLabelText(3, "", 0);
                }
                else
                {
                    SelectedItemOptionSelected();
                    UpdateShopDialogLabelText(4, "", 0);
                }
            }
        }

        private void btnDialogSelectNo_Click(object sender, EventArgs e)
        {
            SelectedItemOptionSelected();
            if (intLastViewedDialog != 3)
                intLastViewedDialog = 4;
            UpdateShopDialogLabelText(intLastViewedDialog, "", 0);
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
                    intFilterListCatalog[0] = 0;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryEverything.BackgroundImage = Properties.Resources.UI_Filter_All_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
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
                    intFilterListCatalog[0] = 1;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryFurniture.BackgroundImage = Properties.Resources.UI_Filter_Furniture_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
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
                    intFilterListCatalog[0] = 2;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryCarpet.BackgroundImage = Properties.Resources.UI_Filter_Carpet_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
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
                    intFilterListCatalog[0] = 3;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryWallpaper.BackgroundImage = Properties.Resources.UI_Filter_Wallpaper_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
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
                    intFilterListCatalog[0] = 4;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryClothing.BackgroundImage = Properties.Resources.UI_Filter_Shirt_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
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
                    intFilterListCatalog[0] = 5;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryTools.BackgroundImage = Properties.Resources.UI_Filter_Tool_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
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
                    intFilterListCatalog[0] = 6;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryStationery.BackgroundImage = Properties.Resources.UI_Filter_Stationery_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
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
                    intFilterListCatalog[0] = 7;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryGyroid.BackgroundImage = Properties.Resources.UI_Filter_Gyroid_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
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
                    intFilterListCatalog[0] = 8;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryFossil.BackgroundImage = Properties.Resources.UI_Filter_Dinosaur_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
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
                    intFilterListCatalog[0] = 9;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryMusic.BackgroundImage = Properties.Resources.UI_Filter_Music_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
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
                    intFilterListCatalog[0] = 10;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                    RevealFilterGroup();
                }
                else
                {
                    chkFilterCategoryMiscellaneous.BackgroundImage = Properties.Resources.UI_Filter_Pitfall_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
            }
        }

        private void chkFilterCategoryEverything_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                lblFilterCategory.Text = "Category:  Everything";
                if (intFilterListCatalog[0] != 0)
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
                if (intFilterListCatalog[0] != 1)
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
                if (intFilterListCatalog[0] != 2)
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
                if (intFilterListCatalog[0] != 3)
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
                if (intFilterListCatalog[0] != 4)
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
                if (intFilterListCatalog[0] != 5)
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
                if (intFilterListCatalog[0] != 6)
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
                if (intFilterListCatalog[0] != 7)
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
                if (intFilterListCatalog[0] != 8)
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
                if (intFilterListCatalog[0] != 9)
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
                if (intFilterListCatalog[0] != 10)
                    HideFilterGroup();
                else
                    RevealFilterGroup();
            }
        }

        private void Catalog_MouseEnter(object sender, EventArgs e)
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
                    intFilterListCatalog[9 + intFilterListCatalog[0]] = 0;
                    UpdateFilterLabelText();
                }
                UpdateFilterGroupCheckboxes();
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
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
                    intFilterListCatalog[9 + intFilterListCatalog[0]] = 1;
                    UpdateFilterLabelText();
                }
                UpdateFilterGroupCheckboxes();
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
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
                    intFilterListCatalog[9 + intFilterListCatalog[0]] = 2;
                    UpdateFilterLabelText();
                }
                UpdateFilterGroupCheckboxes();
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
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
                    intFilterListCatalog[9 + intFilterListCatalog[0]] = 3;
                    UpdateFilterLabelText();
                }
                UpdateFilterGroupCheckboxes();
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
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
                    intFilterListCatalog[9 + intFilterListCatalog[0]] = 4;
                    UpdateFilterLabelText();
                }
                UpdateFilterGroupCheckboxes();
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
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
                    intFilterListCatalog[9 + intFilterListCatalog[0]] = 5;
                    UpdateFilterLabelText();
                }
                UpdateFilterGroupCheckboxes();
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
            }
        }

        private void chkFilterGroup1_MouseEnter(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                switch (intFilterListCatalog[0])
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
                switch (intFilterListCatalog[0])
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
                switch (intFilterListCatalog[0])
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
                switch (intFilterListCatalog[0])
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
                switch (intFilterListCatalog[0])
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
                switch (intFilterListCatalog[0])
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
                    intFilterListCatalog[1] = 1;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                }
                else
                {
                    chkAvailabilityAll.BackgroundImage = Properties.Resources.UI_Filter_All_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
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
                    intFilterListCatalog[1] = 2;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                }
                else
                {
                    chkAvailabilityYes.BackgroundImage = Properties.Resources.UI_Filter_Complete_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
            }
        }

        private void chkAvailabilityNo_CheckedChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
            {
                if (chkAvailabilityNo.Checked == true)
                {
                    chkAvailabilityNo.BackgroundImage = Properties.Resources.UI_Filter_Incomplete;
                    intFilterListCatalog[1] = 3;
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
                if (boolLoading == false) PopulateListView();
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
                    intFilterListCatalog[2] = 0;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                }
                else
                {
                    chkSortCatalog.BackgroundImage = Properties.Resources.UI_Filter_Other_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
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
                    intFilterListCatalog[2] = 1;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                }
                else
                {
                    chkSortAlpha.BackgroundImage = Properties.Resources.UI_Filter_Alphabetized_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
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
                    intFilterListCatalog[2] = 2;
                    UpdateFilterGroupCheckboxes();
                    UpdateFilterLabelText();
                }
                else
                {
                    chkSortPrice.BackgroundImage = Properties.Resources.UI_Filter_Price_False;
                }
                CheckFiltersCategory();
                if (boolLoading == false) PopulateListView();
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

        private void txtFilterSearch_TextChanged(object sender, EventArgs e)
        {
            if (boolItemSelected == false)
                PopulateListView();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCodeFast.Checked == true)
            {
                lblCodeFastCheckbox.ForeColor = Color.LimeGreen;
                boolFastCodeShow = true;
                chkCodeFast.BackgroundImage = Properties.Resources.UI_Catalog_Yes;
            }
            else
            {
                lblCodeFastCheckbox.ForeColor = Color.Red;
                boolFastCodeShow = false;
                chkCodeFast.BackgroundImage = Properties.Resources.UI_Catalog_No;
            }
            saveData();
        }
    }
}
