using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Visa : ICard
    {
        public string CarHolderName { get; set; }

        public void Charge()
        {
            Console.WriteLine("Swiping Visa with " + CarHolderName);
        }
    }
}
