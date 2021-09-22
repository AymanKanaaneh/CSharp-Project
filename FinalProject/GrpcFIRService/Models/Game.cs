using System;
using System.Collections.Generic;

#nullable disable

namespace GrpcFIRService.Models
{
    public partial class Game
    {
        public int SerialNumber { get; set; }
        public string FirstPlayer { get; set; }
        public string SecondPlayer { get; set; }
        public string Winner { get; set; }
        public bool Draw { get; set; }
        public bool Ongoing { get; set; }
        public string StartingTime { get; set; }
        public int FirstPlayerPoints { get; set; }
        public int SecondPlayerPoints { get; set; }
    }
}
