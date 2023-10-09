using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Crossing_Town_Ticket_Station
{
    internal class TaskInfo
    {
        public string Name;
        public readonly string Tag;
        public readonly int Index;
        public readonly int Quantity;
        public readonly int Probability;
        public int Tickets;
        public readonly int RangeMin;
        public readonly int RangeMax;
        public readonly string OtherTask;
        public readonly Image ImageName;

        public TaskInfo(string name, string tag, int index, int quantity, int probability, int tickets, int rangeMin, int rangeMax, string otherTask, Image imageName)
        {
            Name = name;
            Tag = tag;
            Index = index;
            Quantity = quantity;
            Probability = probability;
            Tickets = tickets;
            RangeMin = rangeMin;
            RangeMax = rangeMax;
            OtherTask = otherTask;
            ImageName = imageName;
        }
    }
}
