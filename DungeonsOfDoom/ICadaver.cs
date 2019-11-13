using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonsOfDoom
{
    public interface ICadaver
    {
     string Name { get; set; }
     int Eaten { get; set; }
    }
}
