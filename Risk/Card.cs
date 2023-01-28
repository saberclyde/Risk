using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk
{
    class Card
    {
        private string symbol;
        private Territory territory;
        private bool wild;
        private string name;

        public string Symbol
        {
            get { return symbol; }
        }

        public string Name
        {
            get { return name; }
        }

        public Territory Territory
        {
            get { return territory; }
        }

        public bool Wild
        {
            get { return wild; }
        }

        public Card(string n, string s, Territory t)
        {
            name = n;
            symbol = s;
            territory = t;
            wild = false;
        }

        public Card(string n, string s, bool w)
        {
            name = n;
            symbol = s;
            wild = true;
        }
    }
}
