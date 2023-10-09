using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using System.Xml.Linq;

namespace Animal_Crossing_Town_Ticket_Station
{
    internal class SaveInfo
    {
        public string[] Data = new string[50];

        public SaveInfo(string[] data)
        {
            Data = data;
        }
    }
}
