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
           new TaskInfo("Win an item from an Igloo Villager", "Igloo", 1, 1, 100, 250, 1, 337, "Win an item from a Summer Camper", Properties.Resources.TK_Icon_Igloo),
new TaskInfo("Win an item from a Summer Camper", "SummerCamper", 2, 1, 100, 250, 338, 374, "Complete a Villager Quest", Properties.Resources.TK_Icon_SummerCamper),
new TaskInfo("Catch 5 Fish", "Fish5", 3, 5, 100, 150, 375, 749, "Kick a Ball in the water", Properties.Resources.TK_Icon_Fish5),
new TaskInfo("Catch a X(Fish)", "Fish", 4, 1, 100, 0, 750, 1124, "Kick a Ball in the water", Properties.Resources.TK_Icon_Fish),
new TaskInfo("Catch a piece of Trash", "Trash", 5, 1, 100, 250, 1125, 1199, "Kick a Ball in the water", Properties.Resources.TK_Icon_Trash),
new TaskInfo("Catch a Fish while fishing off a cliff", "FishCliff", 59, 1, 100, 200, 1200, 1274, "Kick a Ball in the water", Properties.Resources.TK_Icon_FishCliff),
new TaskInfo("Catch 5 Bugs", "Bug5", 6, 5, 100, 150, 1275, 1649, "Plant a Tree", Properties.Resources.TK_Icon_Bug5),
new TaskInfo("Catch a X(Bug)", "Bug", 7, 1, 100, 0, 1650, 2024, "Plant a Tree", Properties.Resources.TK_Icon_Bug),
new TaskInfo("See a Villager clap", "BugClap", 60, 1, 100, 200, 2025, 2099, "Plant a Tree", Properties.Resources.TK_Icon_BugClap),
new TaskInfo("Change a Furniture item inside", "HouseFurn", 8, 1, 100, 100, 2100, 2249, "Write in your Diary", Properties.Resources.TK_Icon_HouseFurn),
new TaskInfo("Change a Feng Shui item inside", "HouseFengshui", 9, 1, 100, 150, 2250, 2324, "Write in your Diary", Properties.Resources.TK_Icon_HouseFengshui),
new TaskInfo("Write in your Diary", "HouseDiary", 10, 1, 100, 100, 2325, 2424, "Write in your Diary", Properties.Resources.TK_Icon_HouseDiary),
new TaskInfo("Interact with 3 Furniture inside", "HouseFurn3", 11, 3, 100, 100, 2425, 2524, "Write in your Diary", Properties.Resources.TK_Icon_HouseFurn3),
new TaskInfo("Interact with 2 Gyroids inside", "HouseGyroid2", 12, 2, 100, 100, 2525, 2599, "Interact with 3 Furniture inside", Properties.Resources.TK_Icon_HouseGyroid2),
new TaskInfo("Complete a level in an NES game", "HouseNES", 13, 1, 100, 250, 2600, 2699, "Write in your Diary", Properties.Resources.TK_Icon_HouseNES),
new TaskInfo("Change your K.K. Slider aircheck", "HouseKKSong", 14, 1, 100, 100, 2700, 2749, "Change a Furniture item inside", Properties.Resources.TK_Icon_HouseKKSong),
new TaskInfo("Change your Gyroid Greeting", "HouseGyroid", 15, 1, 100, 100, 2750, 2799, "Write in your Diary", Properties.Resources.TK_Icon_HouseGyroid),
new TaskInfo("Change your Door Decoration", "HouseDoor", 16, 1, 100, 100, 2800, 2849, "Write in your Diary", Properties.Resources.TK_Icon_HouseDoor),
new TaskInfo("Sell 5,000 Bells at Nooks", "NookBells", 17, 5, 100, 200, 2850, 3029, "Sell something to a Villager", Properties.Resources.TK_Icon_NookBells),
new TaskInfo("Order an Item from Nook's Catalog", "NookCatalog", 18, 1, 100, 100, 3030, 3209, "Mail a Letter with a Gift", Properties.Resources.TK_Icon_NookCatalog),
new TaskInfo("Sell 5 seashells to Nook", "NookShells", 19, 5, 100, 150, 3210, 3337, "Eat something", Properties.Resources.TK_Icon_NookShells),
new TaskInfo("Sell 2 Shirts to Nook", "NookShirt", 20, 2, 100, 150, 3338, 3465, "Change your shirt", Properties.Resources.TK_Icon_NookShirt),
new TaskInfo("Sell 2 Furniture to Nook", "NookFurn", 21, 2, 100, 200, 3466, 3593, "Change a Furniture item inside", Properties.Resources.TK_Icon_NookFurn),
new TaskInfo("Sell 2 Carpets to Nook", "NookCarpet", 22, 2, 100, 200, 3594, 3670, "Sell something to a Villager", Properties.Resources.TK_Icon_NookCarpet),
new TaskInfo("Sell 2 Wallpapers to Nook", "NookWallpaper", 23, 2, 100, 200, 3671, 3747, "Sell something to a Villager", Properties.Resources.TK_Icon_NookWallpaper),
new TaskInfo("Complete a Villager Quest", "VillagerQuest", 24, 1, 100, 200, 3748, 3879, "Write in your Diary", Properties.Resources.TK_Icon_VillagerQuest),
new TaskInfo("Mail a Letter with a Gift", "VillagerLetter", 25, 1, 100, 100, 3880, 4099, "Write in your Diary", Properties.Resources.TK_Icon_VillagerLetter),
new TaskInfo("See a Villager Whistle", "VillagerWhistle", 26, 1, 100, 200, 4100, 4187, "Write in your Diary", Properties.Resources.TK_Icon_VillagerWhistle),
new TaskInfo("Give a Villager a new catchphrase", "VillagerCatchphrase", 27, 1, 100, 350, 4188, 4231, "Write in your Diary", Properties.Resources.TK_Icon_VillagerCatchphrase),
new TaskInfo("Sell something to a Villager", "VillagerSell", 28, 1, 100, 300, 4232, 4319, "Write in your Diary", Properties.Resources.TK_Icon_VillagerSell),
new TaskInfo("Buy something from a Villager", "VillagerBuy", 61, 1, 100, 300, 4320, 4407, "Write in your Diary", Properties.Resources.TK_Icon_VillagerBuy),
new TaskInfo("Have your Roof Repainted", "VillagerRoof", 29, 1, 100, 300, 4408, 4451, "Sell something to a Villager", Properties.Resources.TK_Icon_VillagerRoof),
new TaskInfo("Fall in a Pitfall", "VillagerPitfall", 30, 1, 100, 450, 4452, 4495, "Sell something to a Villager", Properties.Resources.TK_Icon_VillagerPitfall),
new TaskInfo("Plant a Tree", "TreePlant", 31, 1, 100, 100, 4496, 4686, "Shake something out of a Tree", Properties.Resources.TK_Icon_TreePlant),
new TaskInfo("Chop down a Tree", "TreeChop", 32, 1, 100, 150, 4687, 4795, "Plant a Tree", Properties.Resources.TK_Icon_TreeChop),
new TaskInfo("Shake something out of a Tree", "TreeShake", 33, 1, 100, 100, 4796, 4959, "Write in your Diary", Properties.Resources.TK_Icon_TreeShake),
new TaskInfo("Eat something", "TreeEat", 34, 1, 100, 100, 4960, 5095, "Write in your Diary", Properties.Resources.TK_Icon_TreeEat),
new TaskInfo("Design a Pattern", "ShirtPattern", 35, 1, 100, 150, 5096, 5245, "Change your shirt", Properties.Resources.TK_Icon_ShirtPattern),
new TaskInfo("Change your shirt", "ShirtChange", 36, 1, 100, 50, 5246, 5395, "Write in your Diary", Properties.Resources.TK_Icon_ShirtChange),
new TaskInfo("Change the menu background", "ShirtMenu", 37, 1, 100, 50, 5396, 5545, "Write in your Diary", Properties.Resources.TK_Icon_ShirtMenu),
new TaskInfo("Spin an Umbrella", "ShirtUmbrella", 38, 1, 100, 50, 5546, 5695, "Write in your Diary", Properties.Resources.TK_Icon_ShirtUmbrella),
new TaskInfo("Catch a Balloon in a Tree", "TownBalloon", 39, 1, 100, 450, 5696, 5749, "Run through 3 Bushes around town", Properties.Resources.TK_Icon_TownBalloon),
new TaskInfo("Write a message on the Message Board", "TownMessage", 40, 1, 100, 100, 5750, 5883, "Kick a Ball in the water", Properties.Resources.TK_Icon_TownMessage),
new TaskInfo("Change your Town Tune", "TownTune", 41, 1, 100, 100, 5884, 6044, "Kick a Ball off a cliff", Properties.Resources.TK_Icon_TownTune),
new TaskInfo("Kick a Ball in the water", "TownBall", 42, 1, 100, 150, 6045, 6124, "Kick a Ball off a cliff", Properties.Resources.TK_Icon_TownBall),
new TaskInfo("Kick a Ball off a cliff", "TownBallCliff", 62, 1, 100, 150, 6125, 6178, "Kick a Ball in the water", Properties.Resources.TK_Icon_TownBallCliff),
new TaskInfo("Run through 3 Bushes around town", "TownBushes", 63, 3, 100, 100, 6179, 6232, "Kick a Ball in the water", Properties.Resources.TK_Icon_TownBushes),
new TaskInfo("Add or move a Sign Board", "TownSign", 43, 1, 100, 100, 6233, 6366, "Write a message on the Message Board", Properties.Resources.TK_Icon_TownSign),
new TaskInfo("Confirm a Perfect Town", "TownPerfect", 44, 1, 100, 250, 6367, 6446, "Plant a Tree", Properties.Resources.TK_Icon_TownPerfect),
new TaskInfo("Talk to Copper and Booker", "CharacterPolice", 45, 2, 100, 100, 6447, 6546, "Run through 3 Bushes around town", Properties.Resources.TK_Icon_CharacterPolice),
new TaskInfo("Talk to Sable and Mabel", "CharacterAbles", 46, 2, 100, 100, 6547, 6646, "Change your shirt", Properties.Resources.TK_Icon_CharacterAbles),
new TaskInfo("Run around a Museum exhibit", "CharacterMuseum", 47, 1, 100, 100, 6647, 6746, "Run through 3 Bushes around town", Properties.Resources.TK_Icon_CharacterMuseum),
new TaskInfo("Redeem Town Tickets", "TicketsRedeem", 48, 1, 100, 200, 6747, 7196, "Write in your Diary", Properties.Resources.TK_Icon_TicketsRedeem),
new TaskInfo("Receive an Item from a Nook Code", "TicketsCode", 64, 1, 100, 150, 7197, 7396, "Mail a Letter with a Gift", Properties.Resources.TK_Icon_TicketsCode),
new TaskInfo("Comment on a BrianMp16 video", "TicketsComment", 49, 1, 100, 450, 7397, 7446, "Redeem Town Tickets", Properties.Resources.TK_Icon_TicketsComment),
new TaskInfo("Like one of BrianMp16's videos", "TicketsLike", 50, 1, 100, 350, 7447, 7496, "Redeem Town Tickets", Properties.Resources.TK_Icon_TicketsLike),
new TaskInfo("Catch a Fish on the Island", "IslandFish", 51, 1, 100, 250, 7497, 7576, "Catch 5 Fish", Properties.Resources.TK_Icon_IslandFish),
new TaskInfo("Catch a Bug on the Island", "IslandBug", 52, 1, 100, 200, 7577, 7656, "Catch 5 Bugs", Properties.Resources.TK_Icon_IslandBug),
new TaskInfo("Visit your Island House", "IslandHouse", 53, 1, 100, 150, 7657, 7736, "Change a Furniture item inside", Properties.Resources.TK_Icon_IslandHouse),
new TaskInfo("Give your Islander a new catchphrase", "IslandNote", 54, 1, 100, 300, 7737, 7784, "Write a message on the Message Board", Properties.Resources.TK_Icon_IslandNote),
new TaskInfo("Give your Islander a requested item", "IslandItem", 55, 1, 100, 400, 7785, 7832, "Sell something to a Villager", Properties.Resources.TK_Icon_IslandItem),
new TaskInfo("Plant a Coconut", "IslandCoconut", 56, 1, 100, 200, 7833, 7896, "Plant a Tree", Properties.Resources.TK_Icon_IslandCoconut),
new TaskInfo("Change your Island Flag", "IslandFlag", 57, 1, 100, 150, 7897, 7944, "Change your Gyroid Greeting", Properties.Resources.TK_Icon_IslandFlag),
new TaskInfo("Scan an E-reader card", "Ereader", 58, 1, 100, 150, 7945, 8191, "Mail a Letter with a Gift", Properties.Resources.TK_Icon_Ereader),
new TaskInfo("Free 100,000 Tickets", "TicketsFree", 89, 1, 100, 100000, 8192, 8192, "Redeem Town Tickets", Properties.Resources.TK_Icon_TicketsFree),

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

