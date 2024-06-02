﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Crossing_Town_Ticket_Station
{
    internal class TortData
    {
        private static readonly TortInfo[] TortInfoArray =
        {
            new TortInfo("today for ","New Year's Day", 110, 1, new int?[12,2]{ {1,3}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {0,0} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_TortimerNewYears),
new TortInfo("today for ","Groundhog Day", 111, 1, new int?[12,2]{ {0,0}, {2,3}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {58,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {0,0} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_Tortimer),
new TortInfo("today for ","the Spring Sports Fair", 112, 1, new int?[12,2]{ {0,0}, {0,0}, {21,4}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {0,0} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_TortimerFair),
new TortInfo("today for ","April Fool's Day", 113, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {1,3}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {58,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {0,0} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_TortimerAprilfool),
new TortInfo("today for ","the Cherry Blossom Festival", 114, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {5,5}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0} }, new int?[1,2]{ {0,0} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_TortimerBlossom),
new TortInfo("today for ","Nature Day", 115, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {22,3}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {58,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {0,0} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_Tortimer),
new TortInfo("today for ","Spring Cleaning Day", 116, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {1,3}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {58,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {0,0} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_Tortimer),
new TortInfo("today for ","Mother's Day", 117, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {31,7}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {58,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {0,1} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_Tortimer),
new TortInfo("today for ","Graduation Day", 118, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {31,7}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {58,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {5,1} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_Tortimer),
new TortInfo("today for ","Father's Day", 119, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {31,8}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {58,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {0,1} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_Tortimer),
new TortInfo("tonight for ","the Fireworks Show", 120, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {4,3}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {0,0} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_TortimerFireworks),
new TortInfo("tonight for ","the Meteor Shower", 121, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {12,3}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {0,0} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_TortimerMeteor),
new TortInfo("today for ","Founder's Day", 122, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {21,3}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {58,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {0,0} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_Tortimer),
new TortInfo("for ","the Morning Aerobics", 123, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {24,2}, {31,1}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {0,0} }, new int?[1,2]{ {1,2} }, 400, Properties.Resources.TK_Icon_Hol_TortimerAerobics),
new TortInfo("today for ","Labor Day", 124, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {31,6}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {58,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {1,1} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_Tortimer),
new TortInfo("today for ","the Fall Sports Fair", 125, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {23,4}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {0,0} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_TortimerFair),
new TortInfo("tonight for ","the Autumn Moon", 126, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {31,10}, {31,10}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {55,1}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {0,0} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_TortimerMoon),
new TortInfo("today for ","Explorer's Day", 127, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {31,7}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {58,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {1,1} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_Tortimer),
new TortInfo("today for ","Mayor's Day", 128, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {31,6}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {58,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {1,2} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_Tortimer),
new TortInfo("today for ","Officer's Day", 129, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {11,3}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {58,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {0,0} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_Tortimer),
new TortInfo("today for ","Sale Day", 130, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {31,9}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {58,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {4,2} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_Tortimer),
new TortInfo("today for ","Snow Day", 131, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {1,3} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {58,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {0,0} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_Tortimer),
new TortInfo("today for ","Toy Day", 132, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {23,3} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {58,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {0,0} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_Tortimer),
new TortInfo("tonight for","New Year's Eve", 133, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {31,3} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1} }, new int?[1,2]{ {0,0} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_TortimerFireworks),
new TortInfo("today for ","your Hometown Day", 134, 1, new int?[12,2]{ {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {31,11}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {58,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {0,0} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_Tortimer),
new TortInfo("today regarding ","his Vacation", 135, 1, new int?[12,2]{ {31,12}, {31,12}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[24,2]{ {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {60,1}, {58,1}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0}, {0,0} }, new int?[1,2]{ {0,0} }, new int?[1,2]{ {0,0} }, 400, Properties.Resources.TK_Icon_Hol_TortimerVacation),

        };

        public static int GetTortInfoArrayLength()
        {
            return TortInfoArray.Length;
        }

        public static Tuple<TortInfo, int> GetTortCheckByName(string name)
        {
            for (int i = 0; i < GetTortInfoArrayLength(); i++)
                if (TortInfoArray[i].Name.Equals(name))
                    return new Tuple<TortInfo, int>(TortInfoArray[i], 1);

            return new Tuple<TortInfo, int>(null, 0);
        }

        public static Tuple<TortInfo, int> GetTortCheckByIndex(int index)
        {
            for (int i = 0; i < GetTortInfoArrayLength(); i++)
                if (TortInfoArray[i].Index.Equals(index))
                    return new Tuple<TortInfo, int>(TortInfoArray[i], 1);

            return new Tuple<TortInfo, int>(null, 0);
        }
    }
}
