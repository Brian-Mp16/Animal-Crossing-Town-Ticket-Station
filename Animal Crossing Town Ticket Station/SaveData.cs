using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Crossing_Town_Ticket_Station
{
    internal class SaveData
    {
        private static readonly SaveInfo[] SaveInfoArray =
        {
            new SaveInfo (null),
        };

        public static int GetSaveInfoArrayLength()
        {
            return SaveInfoArray.Length;
        }

        public static Tuple<SaveInfo, int> GetSaveData()
        {
            return new Tuple<SaveInfo, int>(SaveInfoArray[0], 1);
        }
    }
}
