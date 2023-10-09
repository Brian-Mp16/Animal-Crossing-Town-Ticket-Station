using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACPassword = ACPasswordLibrary.Core.AnimalCrossing;

namespace Animal_Crossing_Town_Ticket_Station
{
    internal class ItemInfo
    {
        public readonly string Name;
        public readonly ushort ID;
        public readonly int CatalogIndex;
        public readonly int AlphaIndex;
        public readonly int TicketIndex;
        public readonly int Month;
        public readonly int Day;
        public readonly int Availability;
        public readonly int UnlockReq;
        public readonly string ItemType;
        public readonly string ItemCategory;
        public readonly string FengShui;
        public readonly int Tickets;
        public readonly ACPassword.CodeType CodeType;
        public readonly string CodeFast;
        public readonly Image ImageName;

        public ItemInfo(string name, ushort id, int catalogIndex, int alphaIndex, int ticketIndex, int month, int day, int availability, int unlockReq, string itemType, string itemCategory, string fengShui, int tickets, ACPassword.CodeType codeType, string codeFast, Image imageName)
        {
            Name = name;
            ID = id;
            CatalogIndex = catalogIndex;
            AlphaIndex = alphaIndex;
            TicketIndex = ticketIndex;
            Month = month;
            Day = day;
            Availability = availability;
            UnlockReq = unlockReq;
            ItemType = itemType;
            ItemCategory = itemCategory;
            FengShui = fengShui;
            Tickets = tickets;
            CodeType = codeType;
            CodeFast = codeFast;
            ImageName = imageName;
        }
    }
}
