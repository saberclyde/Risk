using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Risk
{
    class Territory
    {
        private int troops;
        private Continent continent;
        private Player owner;
        private string name;
        private List<Territory> adjacent = new List<Territory> { };
        public Territory(string n, Continent c)
        {
            owner = null;
            troops = 0;
            name = n;
            continent = c;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        
        public int Troops
        {
            get { return troops; }
            set { troops = value; }
        }

        public Player Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        public Continent Continent
        {
            get { return continent; }
            set { continent = value; }
        }

        public string RefName
        {
            get { return name.ToLower().Replace(" ", ""); }
        }

        public List<Territory> AdjacentTerritories
        {
            get { return adjacent; }
            set { adjacent.Clear(); adjacent.AddRange(value); }
        }
    }
}
