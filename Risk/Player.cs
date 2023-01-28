using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Risk
{
    class Player
    {
        private Color colour;
        private int troopcount;
        private int troopsperturn;
        private string name;
        private List<Territory> territories = new List<Territory> { };
        private List<Card> cards = new List<Card> { };

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Color Colour
        {
            get { return colour; }
            set { colour = value; }
        }
        public int TroopCount
        {
            get { return troopcount; }
            set { troopcount = value; }
        }

        public int TroopsPerTurn
        {
            get { return troopsperturn; }
            set { troopsperturn = value; }
        }

        public List<Territory> Territories
        {
            get { return territories; }
        }

        public int TerritoriesCount 
        { 
            get { return territories.Count; }
        }

        public List<Card> Cards
        {
            get { return cards; }
        }

        public int CardsCount
        {
            get { return cards.Count; }
        }

        public void GainTerritory(Territory t)
        {
            territories.Add(t);
        }

        public void LoseTerritory(Territory t)
        {
            territories.Remove(t);
        }

        public void AddCard(Card c)
        {
            cards.Add(c);
        }

        public void RemoveCard(Card c)
        {
            cards.Remove(c);
        }

        public Player(string n, Color c)
        {
            name = n;
            colour = c;
            troopsperturn = 3;
        }
    }
}
