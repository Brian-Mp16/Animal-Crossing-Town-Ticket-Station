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
           new TaskInfo("Win an item from an Igloo Villager", "Igloo", 1, 1, 100, 200, 1, 269, "Win an item from a Summer Camper", Properties.Resources.TK_Icon_Igloo),
new TaskInfo("Win an item from a Summer Camper", "SummerCamper", 2, 1, 100, 200, 270, 299, "Complete a Villager Quest", Properties.Resources.TK_Icon_SummerCamper),
new TaskInfo("Catch 5 Fish", "Fish5", 3, 5, 100, 150, 300, 673, "Kick a Ball in the water", Properties.Resources.TK_Icon_Fish5),
new TaskInfo("Catch a X(Fish)", "Fish", 4, 1, 100, 0, 674, 1047, "Kick a Ball in the water", Properties.Resources.TK_Icon_Fish),
new TaskInfo("Catch a piece of Trash", "Trash", 5, 1, 100, 250, 1048, 1122, "Kick a Ball in the water", Properties.Resources.TK_Icon_Trash),
new TaskInfo("Catch a Fish while fishing off a cliff", "FishCliff", 59, 1, 100, 200, 1123, 1197, "Kick a Ball in the water", Properties.Resources.TK_Icon_FishCliff),
new TaskInfo("Catch 5 Bugs", "Bug5", 6, 5, 100, 150, 1198, 1571, "Plant a Tree", Properties.Resources.TK_Icon_Bug5),
new TaskInfo("Catch a X(Bug)", "Bug", 7, 1, 100, 0, 1572, 1945, "Plant a Tree", Properties.Resources.TK_Icon_Bug),
new TaskInfo("See a Villager clap", "BugClap", 60, 1, 100, 200, 1946, 2020, "Plant a Tree", Properties.Resources.TK_Icon_BugClap),
new TaskInfo("Add or Move a Furniture item inside", "HouseFurn", 8, 1, 100, 100, 2021, 2076, "Write in your Diary", Properties.Resources.TK_Icon_HouseFurn),
new TaskInfo("Add or Move a Feng Shui item inside", "HouseFengshui", 9, 1, 100, 150, 2077, 2132, "Write in your Diary", Properties.Resources.TK_Icon_HouseFengshui),
new TaskInfo("Write in your Diary", "HouseDiary", 10, 1, 100, 100, 2133, 2216, "Write in your Diary", Properties.Resources.TK_Icon_HouseDiary),
new TaskInfo("Interact with 3 Furniture inside", "HouseFurn3", 11, 3, 100, 100, 2217, 2272, "Write in your Diary", Properties.Resources.TK_Icon_HouseFurn3),
new TaskInfo("Interact with 2 Gyroids inside", "HouseGyroid2", 12, 2, 100, 100, 2273, 2356, "Interact with 3 Furniture inside", Properties.Resources.TK_Icon_HouseGyroid2),
new TaskInfo("Complete a level in an NES game", "HouseNES", 13, 1, 100, 250, 2357, 2468, "Write in your Diary", Properties.Resources.TK_Icon_HouseNES),
new TaskInfo("Change your K.K. Slider aircheck", "HouseKKSong", 14, 1, 100, 100, 2469, 2524, "Add or Move a Furniture item inside", Properties.Resources.TK_Icon_HouseKKSong),
new TaskInfo("Change your Gyroid Greeting", "HouseGyroid", 15, 1, 100, 100, 2525, 2580, "Write in your Diary", Properties.Resources.TK_Icon_HouseGyroid),
new TaskInfo("Change your Door Decoration", "HouseDoor", 16, 1, 100, 100, 2581, 2636, "Write in your Diary", Properties.Resources.TK_Icon_HouseDoor),
new TaskInfo("Change your Carpet or Wallpaper inside", "HouseWallFloor", 65, 1, 100, 150, 2637, 2678, "Add or Move a Furniture item inside", Properties.Resources.TK_Icon_HouseWallFloor),
new TaskInfo("Stretch using an Aerobics Radio", "HouseAerobics", 66, 1, 100, 200, 2679, 2734, "Add or Move a Furniture item inside", Properties.Resources.TK_Icon_HouseAerobics),
new TaskInfo("Dial-a-Psychic using a Lovely Phone", "HousePhone", 67, 1, 100, 250, 2735, 2776, "Interact with 3 Furniture inside", Properties.Resources.TK_Icon_HousePhone),
new TaskInfo("Throw 50 Bells into a Piggy Bank", "HousePiggyBank", 68, 5, 100, 500, 2777, 2804, "Interact with 3 Furniture inside", Properties.Resources.TK_Icon_HousePiggyBank),
new TaskInfo("Ring a Judge's Bell", "HouseJudgesBell", 69, 1, 100, 200, 2805, 2832, "Add or Move a Furniture item inside", Properties.Resources.TK_Icon_HouseJudgesBell),
new TaskInfo("Activate Star Power with a Starman", "HouseStarman", 70, 1, 100, 300, 2833, 2874, "Interact with 3 Furniture inside", Properties.Resources.TK_Icon_HouseStarman),
new TaskInfo("Read a Wish from a Tanabata Palm", "HouseTanabataPalm", 71, 1, 100, 350, 2875, 2916, "Add or Move a Feng Shui item inside", Properties.Resources.TK_Icon_HouseTanabataPalm),
new TaskInfo("Sell 5,000 Bells at Nook's", "NookBells", 17, 5, 100, 200, 2917, 3096, "Sell something to a Villager", Properties.Resources.TK_Icon_NookBells),
new TaskInfo("Order an Item from Nook's Catalog", "NookCatalog", 18, 1, 100, 100, 3097, 3276, "Mail a Letter with a Gift", Properties.Resources.TK_Icon_NookCatalog),
new TaskInfo("Sell 5 seashells to Nook", "NookShells", 19, 5, 100, 150, 3277, 3404, "Eat something", Properties.Resources.TK_Icon_NookShells),
new TaskInfo("Sell 2 Shirts to Nook", "NookShirt", 20, 2, 100, 150, 3405, 3532, "Change your shirt", Properties.Resources.TK_Icon_NookShirt),
new TaskInfo("Sell 2 Furniture to Nook", "NookFurn", 21, 2, 100, 200, 3533, 3660, "Add or Move a Furniture item inside", Properties.Resources.TK_Icon_NookFurn),
new TaskInfo("Sell 2 Carpets to Nook", "NookCarpet", 22, 2, 100, 200, 3661, 3737, "Sell something to a Villager", Properties.Resources.TK_Icon_NookCarpet),
new TaskInfo("Sell 2 Wallpapers to Nook", "NookWallpaper", 23, 2, 100, 200, 3738, 3814, "Sell something to a Villager", Properties.Resources.TK_Icon_NookWallpaper),
new TaskInfo("Complete a Villager Quest", "VillagerQuest", 24, 1, 100, 200, 3815, 3946, "Write in your Diary", Properties.Resources.TK_Icon_VillagerQuest),
new TaskInfo("Mail a Letter with a Gift", "VillagerLetter", 25, 1, 100, 100, 3947, 4166, "Write in your Diary", Properties.Resources.TK_Icon_VillagerLetter),
new TaskInfo("See a Villager Whistle", "VillagerWhistle", 26, 1, 100, 200, 4167, 4254, "Write in your Diary", Properties.Resources.TK_Icon_VillagerWhistle),
new TaskInfo("Give a Villager a new catchphrase", "VillagerCatchphrase", 27, 1, 100, 350, 4255, 4298, "Write in your Diary", Properties.Resources.TK_Icon_VillagerCatchphrase),
new TaskInfo("Sell something to a Villager", "VillagerSell", 28, 1, 100, 300, 4299, 4386, "Write in your Diary", Properties.Resources.TK_Icon_VillagerSell),
new TaskInfo("Buy something from a Villager", "VillagerBuy", 61, 1, 100, 300, 4387, 4474, "Write in your Diary", Properties.Resources.TK_Icon_VillagerBuy),
new TaskInfo("Have your Roof Repainted", "VillagerRoof", 29, 1, 100, 300, 4475, 4518, "Sell something to a Villager", Properties.Resources.TK_Icon_VillagerRoof),
new TaskInfo("Fall in a Pitfall", "VillagerPitfall", 30, 1, 100, 450, 4519, 4562, "Sell something to a Villager", Properties.Resources.TK_Icon_VillagerPitfall),
new TaskInfo("Plant a Tree", "TreePlant", 31, 1, 100, 100, 4563, 4752, "Shake something out of a Tree", Properties.Resources.TK_Icon_TreePlant),
new TaskInfo("Chop down a Tree", "TreeChop", 32, 1, 100, 150, 4753, 4861, "Plant a Tree", Properties.Resources.TK_Icon_TreeChop),
new TaskInfo("Shake something out of a Tree", "TreeShake", 33, 1, 100, 100, 4862, 5024, "Write in your Diary", Properties.Resources.TK_Icon_TreeShake),
new TaskInfo("Eat something", "TreeEat", 34, 1, 100, 100, 5025, 5160, "Write in your Diary", Properties.Resources.TK_Icon_TreeEat),
new TaskInfo("Design a Pattern", "ShirtPattern", 35, 1, 100, 150, 5161, 5294, "Change your shirt", Properties.Resources.TK_Icon_ShirtPattern),
new TaskInfo("Change your shirt", "ShirtChange", 36, 1, 100, 50, 5295, 5428, "Write in your Diary", Properties.Resources.TK_Icon_ShirtChange),
new TaskInfo("Change the menu background", "ShirtMenu", 37, 1, 100, 50, 5429, 5562, "Write in your Diary", Properties.Resources.TK_Icon_ShirtMenu),
new TaskInfo("Spin an Umbrella", "ShirtUmbrella", 38, 1, 100, 100, 5563, 5589, "Run through 3 Bushes around town", Properties.Resources.TK_Icon_ShirtUmbrella),
new TaskInfo("Fan yourself with a Fan", "ShirtFan", 72, 1, 100, 100, 5590, 5616, "Spin an Umbrella", Properties.Resources.TK_Icon_ShirtFan),
new TaskInfo("Run around holding a Pinwheel", "ShirtPinwheel", 73, 1, 100, 100, 5617, 5723, "Fan yourself with a Fan", Properties.Resources.TK_Icon_ShirtPinwheel),
new TaskInfo("Catch a Balloon in a Tree", "TownBalloon", 39, 1, 100, 450, 5724, 5776, "Run through 3 Bushes around town", Properties.Resources.TK_Icon_TownBalloon),
new TaskInfo("Write a message on the Message Board", "TownMessage", 40, 1, 100, 100, 5777, 5910, "Kick a Ball in the water", Properties.Resources.TK_Icon_TownMessage),
new TaskInfo("Change your Town Tune", "TownTune", 41, 1, 100, 100, 5911, 6070, "Kick a Ball off a cliff", Properties.Resources.TK_Icon_TownTune),
new TaskInfo("Kick a Ball in the water", "TownBall", 42, 1, 100, 150, 6071, 6150, "Kick a Ball off a cliff", Properties.Resources.TK_Icon_TownBall),
new TaskInfo("Kick a Ball off a cliff", "TownBallCliff", 62, 1, 100, 150, 6151, 6203, "Kick a Ball in the water", Properties.Resources.TK_Icon_TownBallCliff),
new TaskInfo("Run through 3 Bushes around town", "TownBushes", 63, 3, 100, 100, 6204, 6256, "Kick a Ball in the water", Properties.Resources.TK_Icon_TownBushes),
new TaskInfo("Add, Move, or Change a Sign Board", "TownSign", 43, 1, 100, 100, 6257, 6390, "Write a message on the Message Board", Properties.Resources.TK_Icon_TownSign),
new TaskInfo("Confirm a Perfect Town", "TownPerfect", 44, 1, 100, 250, 6391, 6470, "Plant a Tree", Properties.Resources.TK_Icon_TownPerfect),
new TaskInfo("Talk to Copper and Booker", "CharacterPolice", 45, 2, 100, 100, 6471, 6570, "Run through 3 Bushes around town", Properties.Resources.TK_Icon_CharacterPolice),
new TaskInfo("Talk to Sable and Mabel", "CharacterAbles", 46, 2, 100, 100, 6571, 6670, "Change your shirt", Properties.Resources.TK_Icon_CharacterAbles),
new TaskInfo("Run around a Museum exhibit", "CharacterMuseum", 47, 1, 100, 100, 6671, 6770, "Run through 3 Bushes around town", Properties.Resources.TK_Icon_CharacterMuseum),
new TaskInfo("Redeem Town Tickets", "TicketsRedeem", 48, 1, 100, 200, 6771, 7219, "Write in your Diary", Properties.Resources.TK_Icon_TicketsRedeem),
new TaskInfo("Receive an Item from a Nook Code", "TicketsCode", 64, 1, 100, 150, 7220, 7419, "Mail a Letter with a Gift", Properties.Resources.TK_Icon_TicketsCode),
new TaskInfo("Comment on a BrianMp16 video", "TicketsComment", 49, 1, 100, 450, 7420, 7469, "Redeem Town Tickets", Properties.Resources.TK_Icon_TicketsComment),
new TaskInfo("Like one of BrianMp16's videos", "TicketsLike", 50, 1, 100, 350, 7470, 7519, "Redeem Town Tickets", Properties.Resources.TK_Icon_TicketsLike),
new TaskInfo("Catch a Fish on the Island", "IslandFish", 51, 1, 100, 250, 7520, 7599, "Catch 5 Fish", Properties.Resources.TK_Icon_IslandFish),
new TaskInfo("Catch a Bug on the Island", "IslandBug", 52, 1, 100, 200, 7600, 7679, "Catch 5 Bugs", Properties.Resources.TK_Icon_IslandBug),
new TaskInfo("Visit your Island House", "IslandHouse", 53, 1, 100, 150, 7680, 7759, "Add or Move a Furniture item inside", Properties.Resources.TK_Icon_IslandHouse),
new TaskInfo("Give your Islander a new catchphrase", "IslandNote", 54, 1, 100, 300, 7760, 7807, "Write a message on the Message Board", Properties.Resources.TK_Icon_IslandNote),
new TaskInfo("Give your Islander a requested item", "IslandItem", 55, 1, 100, 400, 7808, 7855, "Sell something to a Villager", Properties.Resources.TK_Icon_IslandItem),
new TaskInfo("Plant a Coconut", "IslandCoconut", 56, 1, 100, 200, 7856, 7919, "Plant a Tree", Properties.Resources.TK_Icon_IslandCoconut),
new TaskInfo("Change your Island Flag", "IslandFlag", 57, 1, 100, 150, 7920, 7967, "Change your Gyroid Greeting", Properties.Resources.TK_Icon_IslandFlag),
new TaskInfo("Scan an E-reader card", "Ereader", 58, 1, 100, 150, 7968, 8191, "Mail a Letter with a Gift", Properties.Resources.TK_Icon_Ereader),
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

