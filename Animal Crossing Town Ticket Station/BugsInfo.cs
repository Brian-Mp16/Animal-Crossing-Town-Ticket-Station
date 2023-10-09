using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Crossing_Town_Ticket_Station
{
    internal class BugsInfo
    {
        public readonly string Name;
        public readonly int Index;
        public readonly int?[,] MonthArray;
        public readonly int?[,] HourArray;
        public readonly int?[,] WeekdayArray;
        public readonly int?[,] WeatherArray;
        public readonly int Tickets;
        public readonly Image ImageName;


        public BugsInfo(string name, int index, int?[,] monthArray, int?[,] hourArray, int?[,] weekdayArray, int?[,] weatherArray, int tickets, Image imageName)
        {
            Name = name;
            Index = index;
            MonthArray = monthArray;
            HourArray = hourArray;
            WeekdayArray = weekdayArray;
            WeatherArray = weatherArray;
            Tickets = tickets;
            ImageName = imageName;
        }
    }
}
