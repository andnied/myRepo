using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Shopper
    {
        private readonly ICard _card;

        public Shopper(ICard card)
        {
            _card = card;
        }

        public void Charge()
        {
            _card.Charge();
        }
    }
}
