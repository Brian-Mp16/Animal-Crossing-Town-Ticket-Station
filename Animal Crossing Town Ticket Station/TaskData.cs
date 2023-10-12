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
        {new TaskInfo("Win an item from an Igloo Villager", "Igloo", 1, 1, 100, 250, 1, 348, "Win an item from a Summer Camper", Properties.Resources.TK_Icon_Igloo),
new TaskInfo("Win an item from a Summer Camper", "SummerCamper", 2, 1, 100, 250, 349, 387, "Complete a Villager Quest", Properties.Resources.TK_Icon_SummerCamper),
new TaskInfo("Catch 5 Fish", "Fish5", 3, 5, 100, 150, 388, 773, "Kick a Ball in the water", Properties.Resources.TK_Icon_Fish5),
new TaskInfo("Catch a X(Fish)", "Fish", 4, 1, 100, 0, 774, 1159, "Kick a Ball in the water", Properties.Resources.TK_Icon_Fish),
new TaskInfo("Catch a piece of Trash", "Trash", 5, 1, 100, 250, 1160, 1236, "Kick a Ball in the water", Properties.Resources.TK_Icon_Trash),
new TaskInfo("Catch a Fish while fishing off a cliff", "FishCliff", 6, 1, 100, 200, 1237, 1313, "Kick a Ball in the water", Properties.Resources.TK_Icon_FishCliff),
new TaskInfo("Catch 5 Bugs", "Bug5", 7, 5, 100, 150, 1314, 1699, "Plant a Tree", Properties.Resources.TK_Icon_Bug5),
new TaskInfo("Catch a X(Bug)", "Bug", 8, 1, 100, 0, 1700, 2085, "Plant a Tree", Properties.Resources.TK_Icon_Bug),
new TaskInfo("See a Villager clap", "BugClap", 9, 1, 100, 250, 2086, 2162, "Plant a Tree", Properties.Resources.TK_Icon_BugClap),
new TaskInfo("Change a Furniture item inside", "HouseFurn", 10, 1, 100, 100, 2163, 2322, "Write in your Diary", Properties.Resources.TK_Icon_HouseFurn),
new TaskInfo("Change a Feng Shui item inside", "HouseFengshui", 11, 1, 100, 150, 2323, 2402, "Write in your Diary", Properties.Resources.TK_Icon_HouseFengshui),
new TaskInfo("Write in your Diary", "HouseDiary", 12, 1, 100, 100, 2403, 2509, "Write in your Diary", Properties.Resources.TK_Icon_HouseDiary),
new TaskInfo("Interact with 3 Furniture inside", "HouseFurn3", 13, 3, 100, 100, 2510, 2616, "Write in your Diary", Properties.Resources.TK_Icon_HouseFurn3),
new TaskInfo("Interact with 2 Gyroids inside", "HouseGyroid2", 14, 2, 100, 100, 2617, 2696, "Interact with 3 Furniture inside", Properties.Resources.TK_Icon_HouseGyroid2),
new TaskInfo("Complete a level in an NES game", "HouseNES", 15, 1, 100, 250, 2697, 2776, "Write in your Diary", Properties.Resources.TK_Icon_HouseNES),
new TaskInfo("Change your K.K. Slider aircheck", "HouseKKSong", 16, 1, 100, 100, 2777, 2829, "Change a Furniture item inside", Properties.Resources.TK_Icon_HouseKKSong),
new TaskInfo("Change your Gyroid Greeting", "HouseGyroid", 17, 1, 100, 100, 2830, 2882, "Write in your Diary", Properties.Resources.TK_Icon_HouseGyroid),
new TaskInfo("Change your Door Decoration", "HouseDoor", 18, 1, 100, 100, 2883, 2935, "Write in your Diary", Properties.Resources.TK_Icon_HouseDoor),
new TaskInfo("Sell 5,000 Bells at Nooks", "NookBells", 19, 5, 100, 200, 2936, 3120, "Sell something to a Villager", Properties.Resources.TK_Icon_NookBells),
new TaskInfo("Order an item from your catalog", "NookCatalog", 20, 1, 100, 100, 3121, 3305, "Mail a Letter with a Gift", Properties.Resources.TK_Icon_NookCatalog),
new TaskInfo("Sell 5 seashells to Nook", "NookShells", 21, 5, 100, 150, 3306, 3437, "Eat something", Properties.Resources.TK_Icon_NookShells),
new TaskInfo("Sell 2 Shirts to Nook", "NookShirt", 22, 2, 100, 150, 3438, 3569, "Change your shirt", Properties.Resources.TK_Icon_NookShirt),
new TaskInfo("Sell 2 Furniture to Nook", "NookFurn", 23, 2, 100, 200, 3570, 3701, "Change a Furniture item inside", Properties.Resources.TK_Icon_NookFurn),
new TaskInfo("Sell 2 Carpets to Nook", "NookCarpet", 24, 2, 100, 200, 3702, 3780, "Sell something to a Villager", Properties.Resources.TK_Icon_NookCarpet),
new TaskInfo("Sell 2 Wallpapers to Nook", "NookWallpaper", 25, 2, 100, 200, 3781, 3859, "Sell something to a Villager", Properties.Resources.TK_Icon_NookWallpaper),
new TaskInfo("Complete a Villager Quest", "VillagerQuest", 26, 1, 100, 150, 3860, 4014, "Write in your Diary", Properties.Resources.TK_Icon_VillagerQuest),
new TaskInfo("Mail a Letter with a Gift", "VillagerLetter", 27, 1, 100, 100, 4015, 4169, "Write in your Diary", Properties.Resources.TK_Icon_VillagerLetter),
new TaskInfo("See a Villager Whistle", "VillagerWhistle", 28, 1, 100, 200, 4170, 4272, "Write in your Diary", Properties.Resources.TK_Icon_VillagerWhistle),
new TaskInfo("Give a Villager a new catchphrase", "VillagerCatchphrase", 29, 1, 100, 250, 4273, 4324, "Write in your Diary", Properties.Resources.TK_Icon_VillagerCatchphrase),
new TaskInfo("Sell something to a Villager", "VillagerSell", 30, 1, 100, 300, 4325, 4427, "Write in your Diary", Properties.Resources.TK_Icon_VillagerSell),
new TaskInfo("Buy something from a Villager", "VillagerBuy", 31, 1, 100, 300, 4428, 4530, "Write in your Diary", Properties.Resources.TK_Icon_VillagerBuy),
new TaskInfo("Have your Roof Repainted", "VillagerRoof", 32, 1, 100, 400, 4531, 4582, "Sell something to a Villager", Properties.Resources.TK_Icon_VillagerRoof),
new TaskInfo("Fall in a Pitfall", "VillagerPitfall", 33, 1, 100, 450, 4583, 4634, "Sell something to a Villager", Properties.Resources.TK_Icon_VillagerPitfall),
new TaskInfo("Plant a Tree", "TreePlant", 34, 1, 100, 100, 4635, 4831, "Shake something out of a Tree", Properties.Resources.TK_Icon_TreePlant),
new TaskInfo("Chop down a Tree", "TreeChop", 35, 1, 100, 150, 4832, 4943, "Plant a Tree", Properties.Resources.TK_Icon_TreeChop),
new TaskInfo("Shake something out of a Tree", "TreeShake", 36, 1, 100, 100, 4944, 5112, "Write in your Diary", Properties.Resources.TK_Icon_TreeShake),
new TaskInfo("Eat something", "TreeEat", 37, 1, 100, 100, 5113, 5253, "Write in your Diary", Properties.Resources.TK_Icon_TreeEat),
new TaskInfo("Design a Pattern", "ShirtPattern", 38, 1, 100, 150, 5254, 5369, "Change your shirt", Properties.Resources.TK_Icon_ShirtPattern),
new TaskInfo("Change your shirt", "ShirtChange", 39, 1, 100, 50, 5370, 5485, "Write in your Diary", Properties.Resources.TK_Icon_ShirtChange),
new TaskInfo("Change the menu background", "ShirtMenu", 40, 1, 100, 50, 5486, 5601, "Write in your Diary", Properties.Resources.TK_Icon_ShirtMenu),
new TaskInfo("Spin an Umbrella", "ShirtUmbrella", 41, 1, 100, 50, 5602, 5717, "Write in your Diary", Properties.Resources.TK_Icon_ShirtUmbrella),
new TaskInfo("Catch a Balloon in a Tree", "TownBalloon", 42, 1, 100, 400, 5718, 5776, "Write in your Diary", Properties.Resources.TK_Icon_TownBalloon),
new TaskInfo("Write a message on the Message Board", "TownMessage", 43, 1, 100, 100, 5777, 5925, "Write in your Diary", Properties.Resources.TK_Icon_TownMessage),
new TaskInfo("Change your Town Tune", "TownTune", 44, 1, 100, 100, 5926, 6103, "Write in your Diary", Properties.Resources.TK_Icon_TownTune),
new TaskInfo("Kick a Ball in the water", "TownBall", 45, 1, 100, 150, 6104, 6192, "Write in your Diary", Properties.Resources.TK_Icon_TownBall),
new TaskInfo("Kick a Ball off a cliff", "TownBallCliff", 46, 1, 100, 150, 6193, 6251, "Write in your Diary", Properties.Resources.TK_Icon_TownBallCliff),
new TaskInfo("Add or move a Sign Board", "TownSign", 47, 1, 100, 100, 6252, 6400, "Write a message on the Message Board", Properties.Resources.TK_Icon_TownSign),
new TaskInfo("Confirm a Perfect Town", "TownPerfect", 48, 1, 100, 250, 6401, 6489, "Plant a Tree", Properties.Resources.TK_Icon_TownPerfect),
new TaskInfo("Talk to Copper and Booker", "CharacterPolice", 49, 2, 100, 100, 6490, 6592, "Write in your Diary", Properties.Resources.TK_Icon_CharacterPolice),
new TaskInfo("Talk to Sable and Mabel", "CharacterAbles", 50, 2, 100, 100, 6593, 6695, "Change your shirt", Properties.Resources.TK_Icon_CharacterAbles),
new TaskInfo("Run around a Museum exhibit", "CharacterMuseum", 51, 1, 100, 100, 6696, 6798, "Write in your Diary", Properties.Resources.TK_Icon_CharacterMuseum),
new TaskInfo("Redeem Town Tickets", "TicketsRedeem", 52, 1, 100, 200, 6799, 7293, "Write in your Diary", Properties.Resources.TK_Icon_TicketsRedeem),
new TaskInfo("Comment on a BrianMp16 video", "TicketsComment", 53, 1, 100, 450, 7294, 7355, "Redeem Town Tickets", Properties.Resources.TK_Icon_TicketsComment),
new TaskInfo("Like one of BrianMp16's videos", "TicketsLike", 54, 1, 100, 350, 7356, 7417, "Redeem Town Tickets", Properties.Resources.TK_Icon_TicketsLike),
new TaskInfo("Catch a Fish on the Island", "IslandFish", 55, 1, 100, 250, 7418, 7500, "Catch 5 Fish", Properties.Resources.TK_Icon_IslandFish),
new TaskInfo("Catch a Bug on the Island", "IslandBug", 56, 1, 100, 200, 7501, 7583, "Catch 5 Bugs", Properties.Resources.TK_Icon_IslandBug),
new TaskInfo("Visit your Island House", "IslandHouse", 57, 1, 100, 150, 7584, 7666, "Change a Furniture item inside", Properties.Resources.TK_Icon_IslandHouse),
new TaskInfo("Give your Islander a new catchphrase", "IslandNote", 58, 1, 100, 300, 7667, 7716, "Write a message on the Message Board", Properties.Resources.TK_Icon_IslandNote),
new TaskInfo("Give your Islander a requested item", "IslandItem", 59, 1, 100, 400, 7717, 7766, "Sell something to a Villager", Properties.Resources.TK_Icon_IslandItem),
new TaskInfo("Plant a Coconut", "IslandCoconut", 60, 1, 100, 200, 7767, 7832, "Plant a Tree", Properties.Resources.TK_Icon_IslandCoconut),
new TaskInfo("Change your Island Flag", "IslandFlag", 61, 1, 100, 150, 7833, 7882, "Change your Gyroid Greeting", Properties.Resources.TK_Icon_IslandFlag),
new TaskInfo("Scan an E-reader card", "Ereader", 62, 1, 100, 150, 7883, 8191, "Mail a Letter with a Gift", Properties.Resources.TK_Icon_Ereader),
new TaskInfo("Free 100,000 Tickets", "TicketsFree", 63, 1, 100, 100000, 8192, 8192, "Redeem Town Tickets", Properties.Resources.TK_Icon_TicketsFree),
new TaskInfo("Run through Bushes", "Bushes", 64, 1, 100, 100, 8193, 8293, "Shake something out of a Tree", Properties.Resources.TK_Icon_Bushes),
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

