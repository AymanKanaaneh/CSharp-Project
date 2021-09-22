using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourInRowCreateDB {
    public class Game {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SerialNumber { get; set; }
        public string FirstPlayer { get; set; }
        public string SecondPlayer { get; set; }
        public string Winner{ get; set; }
        public bool draw{ get; set; }
        public bool ongoing{ get; set; }
        public string StartingTime { get; set; }
        public int FirstPlayerPoints { get; set; }
        public int SecondPlayerPoints { get; set; }

    }
}
