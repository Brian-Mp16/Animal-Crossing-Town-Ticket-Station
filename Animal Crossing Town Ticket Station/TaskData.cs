using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using System.Windows.Forms;

namespace Animal_Crossing_Town_Ticket_Station
{
    internal class TaskData
    {
        private static readonly TaskInfo[] TaskInfoArray =
        {
           new TaskInfo("Win an item from an Igloo Villager", "Igloo", 1, 1, 100, 250, 1, 402, "Win an item from a Summer Camper", Properties.Resources.TK_Icon_Igloo),
new TaskInfo("Win an item from a Summer Camper", "SummerCamper", 2, 1, 100, 250, 403, 447, "Complete a Villager Quest", Properties.Resources.TK_Icon_SummerCamper),
new TaskInfo("Catch 5 Fish", "Fish5", 3, 5, 100, 150, 448, 853, "Kick a Ball in the water", Properties.Resources.TK_Icon_Fish5),
new TaskInfo("Catch a X(Fish)", "Fish", 4, 1, 100, 0, 854, 1259, "Kick a Ball in the water", Properties.Resources.TK_Icon_Fish),
new TaskInfo("Catch a piece of Trash", "Trash", 5, 1, 100, 250, 1260, 1340, "Kick a Ball in the water", Properties.Resources.TK_Icon_Trash),
new TaskInfo("Catch 5 Bugs", "Bug5", 6, 5, 100, 150, 1341, 1787, "Plant a Tree", Properties.Resources.TK_Icon_Bug5),
new TaskInfo("Catch a X(Bug)", "Bug", 7, 1, 100, 0, 1788, 2234, "Plant a Tree", Properties.Resources.TK_Icon_Bug),
new TaskInfo("Change a Furniture item inside", "HouseFurn", 8, 1, 100, 100, 2235, 2408, "Write in your Diary", Properties.Resources.TK_Icon_HouseFurn),
new TaskInfo("Change a Feng Shui item inside", "HouseFengshui", 9, 1, 100, 150, 2409, 2482, "Write in your Diary", Properties.Resources.TK_Icon_HouseFengshui),
new TaskInfo("Write in your Diary", "HouseDiary", 10, 1, 100, 100, 2483, 2581, "Write in your Diary", Properties.Resources.TK_Icon_HouseDiary),
new TaskInfo("Interact with 3 Furniture inside", "HouseFurn3", 11, 3, 100, 100, 2582, 2680, "Write in your Diary", Properties.Resources.TK_Icon_HouseFurn3),
new TaskInfo("Interact with 2 Gyroids inside", "HouseGyroid2", 12, 2, 100, 100, 2681, 2754, "Interact with 3 Furniture inside", Properties.Resources.TK_Icon_HouseGyroid2),
new TaskInfo("Complete a level in an NES game", "HouseNES", 13, 1, 100, 250, 2755, 2828, "Write in your Diary", Properties.Resources.TK_Icon_HouseNES),
new TaskInfo("Change your K.K. Slider aircheck", "HouseKKSong", 14, 1, 100, 100, 2829, 2878, "Change a Furniture item inside", Properties.Resources.TK_Icon_HouseKKSong),
new TaskInfo("Change your Gyroid Greeting", "HouseGyroid", 15, 1, 100, 100, 2879, 2928, "Write in your Diary", Properties.Resources.TK_Icon_HouseGyroid),
new TaskInfo("Change your Door Decoration", "HouseDoor", 16, 1, 100, 100, 2929, 2978, "Write in your Diary", Properties.Resources.TK_Icon_HouseDoor),
new TaskInfo("Sell 5,000 Bells at Nooks", "NookBells", 17, 5, 100, 200, 2979, 3168, "Sell an item to a Villager", Properties.Resources.TK_Icon_NookBells),
new TaskInfo("Order an item from your catalog", "NookCatalog", 18, 1, 100, 100, 3169, 3303, "Mail a Letter with a Gift ", Properties.Resources.TK_Icon_NookCatalog),
new TaskInfo("Sell 5 seashells to Nook", "NookShells", 19, 5, 100, 150, 3304, 3438, "Eat something", Properties.Resources.TK_Icon_NookShells),
new TaskInfo("Sell 2 Shirts to Nook", "NookShirt", 20, 2, 100, 150, 3439, 3573, "Change your shirt", Properties.Resources.TK_Icon_NookShirt),
new TaskInfo("Sell 2 Furniture to Nook", "NookFurn", 21, 2, 100, 200, 3574, 3708, "Change a Furniture item inside", Properties.Resources.TK_Icon_NookFurn),
new TaskInfo("Sell 2 Carpets to Nook", "NookCarpet", 22, 2, 100, 200, 3709, 3789, "Sell an item to a Villager", Properties.Resources.TK_Icon_NookCarpet),
new TaskInfo("Sell 2 Wallpapers to Nook", "NookWallpaper", 23, 2, 100, 200, 3790, 3870, "Sell an item to a Villager", Properties.Resources.TK_Icon_NookWallpaper),
new TaskInfo("Complete a Villager Quest", "VillagerQuest", 24, 1, 100, 150, 3871, 4076, "Write in your Diary", Properties.Resources.TK_Icon_VillagerQuest),
new TaskInfo("Mail a Letter with a Gift ", "VillagerLetter", 25, 1, 100, 100, 4077, 4282, "Write in your Diary", Properties.Resources.TK_Icon_VillagerLetter),
new TaskInfo("See a Villager Whistle", "VillagerWhistle", 26, 1, 100, 200, 4283, 4419, "Write in your Diary", Properties.Resources.TK_Icon_VillagerWhistle),
new TaskInfo("Give a Villager a new catchphrase", "VillagerCatchphrase", 27, 1, 100, 250, 4420, 4488, "Write in your Diary", Properties.Resources.TK_Icon_VillagerCatchphrase),
new TaskInfo("Sell an item to a Villager", "VillagerSell", 28, 1, 100, 300, 4489, 4625, "Write in your Diary", Properties.Resources.TK_Icon_VillagerSell),
new TaskInfo("Have your Roof Repainted", "VillagerRoof", 29, 1, 100, 400, 4626, 4694, "Sell an item to a Villager", Properties.Resources.TK_Icon_VillagerRoof),
new TaskInfo("Fall in a Pitfall", "VillagerPitfall", 30, 1, 100, 450, 4695, 4763, "Sell an item to a Villager", Properties.Resources.TK_Icon_VillagerPitfall),
new TaskInfo("Plant a Tree", "TreePlant", 31, 1, 100, 100, 4764, 4953, "Shake something out of a Tree", Properties.Resources.TK_Icon_TreePlant),
new TaskInfo("Chop down a Tree", "TreeChop", 32, 1, 100, 150, 4954, 5034, "Plant a Tree", Properties.Resources.TK_Icon_TreeChop),
new TaskInfo("Shake something out of a Tree", "TreeShake", 33, 1, 100, 100, 5035, 5224, "Write in your Diary", Properties.Resources.TK_Icon_TreeShake),
new TaskInfo("Eat something", "TreeEat", 34, 1, 100, 100, 5225, 5359, "Write in your Diary", Properties.Resources.TK_Icon_TreeEat),
new TaskInfo("Design a Pattern", "ShirtPattern", 35, 1, 100, 150, 5360, 5471, "Change your shirt", Properties.Resources.TK_Icon_ShirtPattern),
new TaskInfo("Change your shirt", "ShirtChange", 36, 1, 100, 50, 5472, 5583, "Write in your Diary", Properties.Resources.TK_Icon_ShirtChange),
new TaskInfo("Change the menu background", "ShirtMenu", 37, 1, 100, 50, 5584, 5695, "Write in your Diary", Properties.Resources.TK_Icon_ShirtMenu),
new TaskInfo("Spin an Umbrella", "ShirtUmbrella", 38, 1, 100, 50, 5696, 5807, "Write in your Diary", Properties.Resources.TK_Icon_ShirtUmbrella),
new TaskInfo("Catch a Balloon in a Tree", "TownBalloon", 39, 1, 100, 400, 5808, 5860, "Write in your Diary", Properties.Resources.TK_Icon_TownBalloon),
new TaskInfo("Write a message on the Message Board", "TownMessage", 40, 1, 100, 100, 5861, 5993, "Write in your Diary", Properties.Resources.TK_Icon_TownMessage),
new TaskInfo("Change your Town Tune", "TownTune", 41, 1, 100, 100, 5994, 6179, "Write in your Diary", Properties.Resources.TK_Icon_TownTune),
new TaskInfo("Kick a Ball in the water", "TownBall", 42, 1, 100, 150, 6180, 6339, "Write in your Diary", Properties.Resources.TK_Icon_TownBall),
new TaskInfo("Add or move a Sign Board", "TownSign", 43, 1, 100, 100, 6340, 6472, "Write a message on the Message Board", Properties.Resources.TK_Icon_TownSign),
new TaskInfo("Confirm a Perfect Town", "TownPerfect", 44, 1, 100, 300, 6473, 6552, "Plant a Tree", Properties.Resources.TK_Icon_TownPerfect),
new TaskInfo("Talk to Copper and Booker", "CharacterPolice", 45, 2, 100, 100, 6553, 6651, "Write in your Diary", Properties.Resources.TK_Icon_CharacterPolice),
new TaskInfo("Talk to Sable and Mabel", "CharacterAbles", 46, 2, 100, 100, 6652, 6750, "Change your shirt", Properties.Resources.TK_Icon_CharacterAbles),
new TaskInfo("Run around a Museum exhibit", "CharacterMuseum", 47, 1, 100, 100, 6751, 6849, "Write in your Diary", Properties.Resources.TK_Icon_CharacterMuseum),
new TaskInfo("Redeem Town Tickets", "TicketsRedeem", 48, 1, 100, 200, 6850, 7445, "Write in your Diary", Properties.Resources.TK_Icon_TicketsRedeem),
new TaskInfo("Comment on a BrianMp16 video", "TicketsComment", 49, 1, 100, 450, 7446, 7519, "Redeem Town Tickets", Properties.Resources.TK_Icon_TicketsComment),
new TaskInfo("Like one of BrianMp16's videos", "TicketsLike", 50, 1, 100, 350, 7520, 7593, "Redeem Town Tickets", Properties.Resources.TK_Icon_TicketsLike),
new TaskInfo("Catch a Fish on the Island", "IslandFish", 51, 1, 100, 250, 7594, 7676, "Catch 5 Fish", Properties.Resources.TK_Icon_IslandFish),
new TaskInfo("Catch a Bug on the Island", "IslandBug", 52, 1, 100, 200, 7677, 7759, "Catch 5 Bugs", Properties.Resources.TK_Icon_IslandBug),
new TaskInfo("Visit your Island House", "IslandHouse", 53, 1, 100, 150, 7760, 7842, "Change a Furniture item inside", Properties.Resources.TK_Icon_IslandHouse),
new TaskInfo("Write a note to your Islander", "IslandNote", 54, 1, 100, 300, 7843, 7892, "Write a message on the Message Board", Properties.Resources.TK_Icon_IslandNote),
new TaskInfo("Give an item to your Islander", "IslandItem", 55, 1, 100, 400, 7893, 7942, "Sell an item to a Villager", Properties.Resources.TK_Icon_IslandItem),
new TaskInfo("Plant a Coconut", "IslandCoconut", 56, 1, 100, 200, 7943, 7992, "Plant a Tree", Properties.Resources.TK_Icon_IslandCoconut),
new TaskInfo("Change your Island Flag", "IslandFlag", 57, 1, 100, 150, 7993, 8042, "Change your Gyroid Greeting", Properties.Resources.TK_Icon_IslandFlag),
new TaskInfo("Scan an E-reader card", "Ereader", 58, 1, 100, 100, 8043, 8191, "Mail a Letter with a Gift ", Properties.Resources.TK_Icon_Ereader),
new TaskInfo("Free 100,000 Tickets", "TicketsFree", 59, 1, 100, 100000, 8192, 8192, "Redeem Town Tickets", Properties.Resources.TK_Icon_TicketsFree),

 };

        public static int GetTaskInfoArrayLength()
        {
            return TaskInfoArray.Length;
        }

        public static Tuple<TaskInfo, int> GetTaskCheckByName(string name)
        {
            for (int i = 0; i < GetTaskInfoArrayLength(); i++)
                if (TaskInfoArray[i].Name.Equals(name))
                    return new Tuple<TaskInfo, int>(TaskInfoArray[i], 1);

            return new Tuple<TaskInfo, int>(null, 0);
        }

        public static Tuple<TaskInfo, int> GetTaskCheckByTag(string tag)
        {
            for (int i = 0; i < GetTaskInfoArrayLength(); i++)
                if (TaskInfoArray[i].Tag.Equals(tag))
                    return new Tuple<TaskInfo, int>(TaskInfoArray[i], 1);

            return new Tuple<TaskInfo, int>(null, 0);
        }

        public static Tuple<TaskInfo, int> GetTaskCheckByRNGValue(int rngValue)
        {
            for (int i = 0; i < GetTaskInfoArrayLength(); i++)
                if (rngValue >= TaskInfoArray[i].RangeMin && rngValue <= TaskInfoArray[i].RangeMax)
                    return new Tuple<TaskInfo, int>(TaskInfoArray[i], 1);

            return new Tuple<TaskInfo, int>(null, 0);
        }

        public static Tuple<TaskInfo, int> GetTaskCheckByIndex(int index)
        {
            for (int i = 0; i < GetTaskInfoArrayLength(); i++)
                if (TaskInfoArray[i].Index.Equals(index))
                    return new Tuple<TaskInfo, int>(TaskInfoArray[i], 1);

            return new Tuple<TaskInfo, int>(null, 0);
        }

    }
}

