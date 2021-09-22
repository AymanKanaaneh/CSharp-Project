using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FourInRowCreateDB {
    class Program {
        static async Task Main(string[] args) {
            try {
                await Create();
            }
            catch (Exception ex) {

                Console.WriteLine(ex.Message);
               // Debug.Print(ex.Message + " ++++ ");
            }
        }

        static async Task Create() {
            using (var ctx = new FourInRowContext()) {
                bool created = await ctx.Database.EnsureCreatedAsync();
                Console.WriteLine(created == true ? "database created" : "database exists");
            }
        }
    }
}
