using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Risk
{
    class TerritoryLabel : Label
    {
        private Territory territory;

        public Territory Territory
        {
            get { return territory; }
            set { territory = value; }
        }
    }
}
