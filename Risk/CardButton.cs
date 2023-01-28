using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Risk
{
    class CardButton : Button
    {
        private Card card;

        public Card Card
        {
            get { return card; }
            set { card = value; }
        }
        public CardButton()
        {
            SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, true);
        }
    }
}
