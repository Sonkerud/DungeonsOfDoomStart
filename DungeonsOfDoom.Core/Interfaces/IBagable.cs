using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonsOfDoom
{
    public interface IBagable
    {
     string Name { get; set; }
     int Eaten { get; set; }
    }
}
