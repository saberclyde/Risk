using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Risk
{
    class Continent
    {
        private List<Territory> territories = new List<Territory> { };
        private string name;

        public List<Territory> Territories
        {
            get { return territories; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Continent(string n, List<Territory> t)
        {
            name = n;
            territories.AddRange(t);
        }
    }
}
