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
           new TaskInfo("Win an item from an Igloo Villager", "Igloo", 1, 1, 100, 200, 1, 207, "Win an item from a Summer Camper", Properties.Resources.TK_Icon_Igloo),
new TaskInfo("Win an item from a Summer Camper", "SummerCamper", 2, 1, 100, 200, 208, 230, "Complete a Villager Quest", Properties.Resources.TK_Icon_SummerCamper),
new TaskInfo("Catch 5 Fish", "Fish5", 3, 5, 100, 150, 231, 613, "Kick a Ball in the water", Properties.Resources.TK_Icon_Fish5),
new TaskInfo("Catch a X(Fish)", "Fish", 4, 1, 100, 0, 614, 996, "Kick a Ball in the water", Properties.Resources.TK_Icon_Fish),
new TaskInfo("Catch a piece of Trash", "Trash", 5, 1, 100, 250, 997, 1073, "Kick a Ball in the water", Properties.Resources.TK_Icon_Trash),
new TaskInfo("Catch a Fish while fishing off a cliff", "FishCliff", 59, 1, 100, 200, 1074, 1150, "Kick a Ball in the water", Properties.Resources.TK_Icon_FishCliff),
new TaskInfo("Catch 5 Bugs", "Bug5", 6, 5, 100, 150, 1151, 1533, "Plant a Tree", Properties.Resources.TK_Icon_Bug5),
new TaskInfo("Catch a X(Bug)", "Bug", 7, 1, 100, 0, 1534, 1916, "Plant a Tree", Properties.Resources.TK_Icon_Bug),
new TaskInfo("See a Villager clap", "BugClap", 60, 1, 100, 200, 1917, 1993, "Plant a Tree", Properties.Resources.TK_Icon_BugClap),
new TaskInfo("Add or Move a Furniture item inside", "HouseFurn", 8, 1, 100, 100, 1994, 2050, "Write in your Diary", Properties.Resources.TK_Icon_HouseFurn),
new TaskInfo("Add or Move a Feng Shui item inside", "HouseFengshui", 9, 1, 100, 150, 2051, 2107, "Write in your Diary", Properties.Resources.TK_Icon_HouseFengshui),
new TaskInfo("Write in your Diary", "HouseDiary", 10, 1, 100, 100, 2108, 2193, "Write in your Diary", Properties.Resources.TK_Icon_HouseDiary),
new TaskInfo("Interact with 3 Furniture inside", "HouseFurn3", 11, 3, 100, 100, 2194, 2250, "Write in your Diary", Properties.Resources.TK_Icon_HouseFurn3),
new TaskInfo("Interact with 2 Gyroids inside", "HouseGyroid2", 12, 2, 100, 100, 2251, 2336, "Interact with 3 Furniture inside", Properties.Resources.TK_Icon_HouseGyroid2),
new TaskInfo("Complete a level in an NES game", "HouseNES", 13, 1, 100, 250, 2337, 2451, "Write in your Diary", Properties.Resources.TK_Icon_HouseNES),
new TaskInfo("Change your K.K. Slider aircheck", "HouseKKSong", 14, 1, 100, 100, 2452, 2508, "Add or Move a Furniture item inside", Properties.Resources.TK_Icon_HouseKKSong),
new TaskInfo("Change your Gyroid Greeting", "HouseGyroid", 15, 1, 100, 100, 2509, 2565, "Write in your Diary", Properties.Resources.TK_Icon_HouseGyroid),
new TaskInfo("Change your Door Decoration", "HouseDoor", 16, 1, 100, 100, 2566, 2622, "Write in your Diary", Properties.Resources.TK_Icon_HouseDoor),
new TaskInfo("Change your Carpet or Wallpaper inside", "HouseWallFloor", 65, 1, 100, 150, 2623, 2665, "Add or Move a Furniture item inside", Properties.Resources.TK_Icon_HouseWallFloor),
new TaskInfo("Stretch using an Aerobics Radio", "HouseAerobics", 66, 1, 100, 200, 2666, 2722, "Add or Move a Furniture item inside", Properties.Resources.TK_Icon_HouseAerobics),
new TaskInfo("Dial-a-Psychic using a Lovely Phone", "HousePhone", 67, 1, 100, 250, 2723, 2765, "Interact with 3 Furniture inside", Properties.Resources.TK_Icon_HousePhone),
new TaskInfo("Throw 50 Bells into a Piggy Bank", "HousePiggyBank", 68, 5, 100, 500, 2766, 2794, "Interact with 3 Furniture inside", Properties.Resources.TK_Icon_HousePiggyBank),
new TaskInfo("Ring a Judge's Bell", "HouseJudgesBell", 69, 1, 100, 200, 2795, 2823, "Add or Move a Furniture item inside", Properties.Resources.TK_Icon_HouseJudgesBell),
new TaskInfo("Activate Star Power with a Starman", "HouseStarman", 70, 1, 100, 300, 2824, 2866, "Interact with 3 Furniture inside", Properties.Resources.TK_Icon_HouseStarman),
new TaskInfo("Read a Wish from a Tanabata Palm", "HouseTanabataPalm", 71, 1, 100, 350, 2867, 2909, "Add or Move a Feng Shui item inside", Properties.Resources.TK_Icon_HouseTanabataPalm),
new TaskInfo("Sell 5,000 Bells at Nook's", "NookBells", 17, 5, 100, 200, 2910, 3062, "Sell something to a Villager", Properties.Resources.TK_Icon_NookBells),
new TaskInfo("Order an Item from Nook's Catalog", "NookCatalog", 18, 1, 100, 100, 3063, 3215, "Mail a Letter with a Gift", Properties.Resources.TK_Icon_NookCatalog),
new TaskInfo("Sell 5 seashells to Nook", "NookShells", 19, 5, 100, 150, 3216, 3324, "Eat something", Properties.Resources.TK_Icon_NookShells),
new TaskInfo("Sell 2 Shirts to Nook", "NookShirt", 20, 2, 100, 150, 3325, 3433, "Change your shirt", Properties.Resources.TK_Icon_NookShirt),
new TaskInfo("Sell 2 Furniture to Nook", "NookFurn", 21, 2, 100, 200, 3434, 3542, "Add or Move a Furniture item inside", Properties.Resources.TK_Icon_NookFurn),
new TaskInfo("Sell 2 Carpets to Nook", "NookCarpet", 22, 2, 100, 200, 3543, 3608, "Sell something to a Villager", Properties.Resources.TK_Icon_NookCarpet),
new TaskInfo("Sell 2 Wallpapers to Nook", "NookWallpaper", 23, 2, 100, 200, 3609, 3674, "Sell something to a Villager", Properties.Resources.TK_Icon_NookWallpaper),
new TaskInfo("Complete a Villager Quest", "VillagerQuest", 24, 1, 100, 200, 3675, 3809, "Write in your Diary", Properties.Resources.TK_Icon_VillagerQuest),
new TaskInfo("Mail a Letter with a Gift", "VillagerLetter", 25, 1, 100, 100, 3810, 4034, "Write in your Diary", Properties.Resources.TK_Icon_VillagerLetter),
new TaskInfo("See a Villager Whistle", "VillagerWhistle", 26, 1, 100, 200, 4035, 4124, "Write in your Diary", Properties.Resources.TK_Icon_VillagerWhistle),
new TaskInfo("Give a Villager a new catchphrase", "VillagerCatchphrase", 27, 1, 100, 350, 4125, 4169, "Write in your Diary", Properties.Resources.TK_Icon_VillagerCatchphrase),
new TaskInfo("Sell something to a Villager", "VillagerSell", 28, 1, 100, 300, 4170, 4259, "Write in your Diary", Properties.Resources.TK_Icon_VillagerSell),
new TaskInfo("Buy something from a Villager", "VillagerBuy", 61, 1, 100, 300, 4260, 4349, "Write in your Diary", Properties.Resources.TK_Icon_VillagerBuy),
new TaskInfo("Have your Roof Repainted", "VillagerRoof", 29, 1, 100, 300, 4350, 4394, "Sell something to a Villager", Properties.Resources.TK_Icon_VillagerRoof),
new TaskInfo("Fall in a Pitfall", "VillagerPitfall", 30, 1, 100, 450, 4395, 4439, "Sell something to a Villager", Properties.Resources.TK_Icon_VillagerPitfall),
new TaskInfo("Plant a Tree", "TreePlant", 31, 1, 100, 100, 4440, 4634, "Shake something out of a Tree", Properties.Resources.TK_Icon_TreePlant),
new TaskInfo("Chop down a Tree", "TreeChop", 32, 1, 100, 150, 4635, 4745, "Plant a Tree", Properties.Resources.TK_Icon_TreeChop),
new TaskInfo("Shake something out of a Tree", "TreeShake", 33, 1, 100, 100, 4746, 4912, "Write in your Diary", Properties.Resources.TK_Icon_TreeShake),
new TaskInfo("Eat something", "TreeEat", 34, 1, 100, 100, 4913, 5051, "Write in your Diary", Properties.Resources.TK_Icon_TreeEat),
new TaskInfo("Design a Pattern", "ShirtPattern", 35, 1, 100, 150, 5052, 5197, "Change your shirt", Properties.Resources.TK_Icon_ShirtPattern),
new TaskInfo("Change your shirt", "ShirtChange", 36, 1, 100, 50, 5198, 5343, "Write in your Diary", Properties.Resources.TK_Icon_ShirtChange),
new TaskInfo("Change the menu background", "ShirtMenu", 37, 1, 100, 50, 5344, 5489, "Write in your Diary", Properties.Resources.TK_Icon_ShirtMenu),
new TaskInfo("Spin an Umbrella", "ShirtUmbrella", 38, 1, 100, 100, 5490, 5518, "Run through 3 Bushes around town", Properties.Resources.TK_Icon_ShirtUmbrella),
new TaskInfo("Fan yourself with a Fan", "ShirtFan", 72, 1, 100, 100, 5519, 5547, "Spin an Umbrella", Properties.Resources.TK_Icon_ShirtFan),
new TaskInfo("Run around holding a Pinwheel", "ShirtPinwheel", 73, 1, 100, 100, 5548, 5664, "Fan yourself with a Fan", Properties.Resources.TK_Icon_ShirtPinwheel),
new TaskInfo("Catch a Balloon in a Tree", "TownBalloon", 39, 1, 100, 450, 5665, 5719, "Run through 3 Bushes around town", Properties.Resources.TK_Icon_TownBalloon),
new TaskInfo("Write a message on the Message Board", "TownMessage", 40, 1, 100, 100, 5720, 5856, "Kick a Ball in the water", Properties.Resources.TK_Icon_TownMessage),
new TaskInfo("Change your Town Tune", "TownTune", 41, 1, 100, 100, 5857, 6020, "Kick a Ball off a cliff", Properties.Resources.TK_Icon_TownTune),
new TaskInfo("Kick a Ball in the water", "TownBall", 42, 1, 100, 150, 6021, 6102, "Kick a Ball off a cliff", Properties.Resources.TK_Icon_TownBall),
new TaskInfo("Kick a Ball off a cliff", "TownBallCliff", 62, 1, 100, 150, 6103, 6157, "Kick a Ball in the water", Properties.Resources.TK_Icon_TownBallCliff),
new TaskInfo("Run through 3 Bushes around town", "TownBushes", 63, 3, 100, 100, 6158, 6212, "Kick a Ball in the water", Properties.Resources.TK_Icon_TownBushes),
new TaskInfo("Add, Move, or Change a Sign Board", "TownSign", 43, 1, 100, 100, 6213, 6349, "Write a message on the Message Board", Properties.Resources.TK_Icon_TownSign),
new TaskInfo("Confirm a Perfect Town", "TownPerfect", 44, 1, 100, 250, 6350, 6431, "Plant a Tree", Properties.Resources.TK_Icon_TownPerfect),
new TaskInfo("Talk to Copper and Booker", "CharacterPolice", 45, 2, 100, 100, 6432, 6533, "Run through 3 Bushes around town", Properties.Resources.TK_Icon_CharacterPolice),
new TaskInfo("Talk to Sable and Mabel", "CharacterAbles", 46, 2, 100, 100, 6534, 6635, "Change your shirt", Properties.Resources.TK_Icon_CharacterAbles),
new TaskInfo("Run around a Museum exhibit", "CharacterMuseum", 47, 1, 100, 100, 6636, 6737, "Run through 3 Bushes around town", Properties.Resources.TK_Icon_CharacterMuseum),
new TaskInfo("Redeem Town Tickets", "TicketsRedeem", 48, 1, 100, 200, 6738, 7196, "Write in your Diary", Properties.Resources.TK_Icon_TicketsRedeem),
new TaskInfo("Receive an Item from a Nook Code", "TicketsCode", 64, 1, 100, 150, 7197, 7400, "Mail a Letter with a Gift", Properties.Resources.TK_Icon_TicketsCode),
new TaskInfo("Comment on a BrianMp16 video", "TicketsComment", 49, 1, 100, 450, 7401, 7451, "Redeem Town Tickets", Properties.Resources.TK_Icon_TicketsComment),
new TaskInfo("Like one of BrianMp16's videos", "TicketsLike", 50, 1, 100, 350, 7452, 7502, "Redeem Town Tickets", Properties.Resources.TK_Icon_TicketsLike),
new TaskInfo("Catch a Fish on the Island", "IslandFish", 51, 1, 100, 250, 7503, 7584, "Catch 5 Fish", Properties.Resources.TK_Icon_IslandFish),
new TaskInfo("Catch a Bug on the Island", "IslandBug", 52, 1, 100, 200, 7585, 7666, "Catch 5 Bugs", Properties.Resources.TK_Icon_IslandBug),
new TaskInfo("Visit your Island House", "IslandHouse", 53, 1, 100, 150, 7667, 7748, "Add or Move a Furniture item inside", Properties.Resources.TK_Icon_IslandHouse),
new TaskInfo("Give your Islander a new catchphrase", "IslandNote", 54, 1, 100, 300, 7749, 7797, "Write a message on the Message Board", Properties.Resources.TK_Icon_IslandNote),
new TaskInfo("Give your Islander a requested item", "IslandItem", 55, 1, 100, 400, 7798, 7846, "Sell something to a Villager", Properties.Resources.TK_Icon_IslandItem),
new TaskInfo("Plant a Coconut", "IslandCoconut", 56, 1, 100, 200, 7847, 7912, "Plant a Tree", Properties.Resources.TK_Icon_IslandCoconut),
new TaskInfo("Change your Island Flag", "IslandFlag", 57, 1, 100, 150, 7913, 7961, "Change your Gyroid Greeting", Properties.Resources.TK_Icon_IslandFlag),
new TaskInfo("Scan an E-reader card", "Ereader", 58, 1, 100, 150, 7962, 8191, "Mail a Letter with a Gift", Properties.Resources.TK_Icon_Ereader),
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

