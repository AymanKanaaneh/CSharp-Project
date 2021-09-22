using System;
using System.Collections.Generic;

#nullable disable

namespace GrpcFIRService.Models
{
    public partial class User
    {
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public int NumOfGames { get; set; }
        public int NumOfWins { get; set; }
        public int Points { get; set; }
        public int NumOfLoses { get; set; }
    }
}
