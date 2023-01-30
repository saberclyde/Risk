using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Risk
{
    class Game
    {
        private int playercount;
        private int cardbonus;
        private int cardbonusindex = 0;
        private List<int> cardincrement = new List<int> { 4, 6, 8, 10, 12, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60 };
        private Player turn;
        private Random dice = new Random();
        private List<Player> players = new List<Player> { };
        private string state;
        private string turnPart;
        private List<Territory> unowned = new List<Territory> { };
        private List<Card> deck = new List<Card> { };

        public List<Player> Players
        {
            get { return players; }
        }
        public int PlayerCount
        {
            get { return playercount; }
            set { playercount = value; }
        }
        public int CardBonus
        {
            get { return cardbonus; }
            set { cardbonus = value; }
        }
        public Player Turn
        {
            get { return turn; }
            set { turn = value; }
        }

        public string State
        {
            get { return state; }
            set { state = value; }
        }

        public string TurnPart
        {
            get { return turnPart; }
            set { turnPart = value; }
        }

        public Game()
        {
            cardbonus = cardincrement[0];
        }

        public void IncreaseBonus()
        {
            if (cardbonusindex == 14) return;
            else cardbonusindex++; cardbonus = cardincrement[cardbonusindex];
        }

        public int roll()
        {
            return dice.Next(1, 6);
        }

        public Player Order()
        {
            return players[dice.Next(players.Count)];
        }

        public List<Card> Deck
        {
            get { return deck; }
        }

        public void ClaimTerritory(Territory t)
        {
            unowned.Remove(t);
            t.Owner = turn;
            turn.GainTerritory(t);
            t.Troops = 1;
            turn.TroopCount--;
        }

        private void CreatePlayers(int c)
        {
            List<Color> colours = new List<Color> 
                {Color.Red, Color.BlueViolet, Color.LightSeaGreen, Color.DarkOrange, Color.Blue, Color.DeepPink, Color.ForestGreen};
            for (int i = 0; i < c; i++)
            {
                Color co = colours[dice.Next(colours.Count)];
                colours.Remove(co);
                players.Add(new Player($"Player {i + 1}", co));
            }
        }

        private void GenerateDeck(Map m)
        {
            List<Territory> tl = new List<Territory> { };
            foreach (Territory i in m.Territories) tl.Add(i);
            for (int i = 0; i < 42; i++)
            {
                Territory t = tl[dice.Next(tl.Count)];
                tl.Remove(t);
                if (i < 13) deck.Add(new Card($"{t.RefName}", "Soldier", t));
                else if (i < 27) deck.Add(new Card($"{t.RefName}", "Horse", t));
                else deck.Add(new Card($"{t.RefName}", "Cannon", t));
            }
            deck.Add(new Card("wc1", "wc1", true));
            deck.Add(new Card("wc2", "wc1", true));
        }
        
        public void SetUp(int pc, Map m)
        {
            playercount = pc;
            CreatePlayers(pc);
            Turn = Order();
            foreach (Player i in players)
            {
                i.TroopCount = 45 - (5 * playercount);
            }
            foreach (Territory i in m.Territories) unowned.Add(i);
            GenerateDeck(m);
        }

        public void GiveCard(Player p)
        {
            Card c = deck[dice.Next(deck.Count)];
            p.AddCard(c);
            deck.Remove(c);
        }

        public void ResetGame(Map m)
        {
            foreach (Territory i in m.Territories)
            {
                i.Troops = 0;
                i.Owner = null;
            }
            cardbonusindex = 0;
            cardbonus = cardincrement[0];
            deck.Clear();
            players.Clear();
            turnPart = "Fortify";
            state = "Claiming";
        }

        public void AdvanceTurn(Map m)
        {
            turnPart = "Fortify";
            if (state == "Claiming")
            {
                if (unowned.Count == 0) state = "Remaining";
            }
            else if (state == "Remaining")
            {
                foreach (Player i in players)
                {
                    if (i.TroopCount != 0) break;
                    if (i == players[playercount - 1] && i.TroopCount == 0) state = "Main";
                }
            }
            if (players.IndexOf(turn) == players.Count - 1) turn = players[0];
            else turn = players[players.IndexOf(turn) + 1];
            foreach (Player i in players)
            {
                int x = i.TerritoriesCount / 3;
                if (x < 4) i.TroopsPerTurn = 3;
                else i.TroopsPerTurn = x;
                if (m.OwnsContinent(i, m.NorthAmerica)) i.TroopsPerTurn = i.TroopsPerTurn + 5;
                if (m.OwnsContinent(i, m.SouthAmerica)) i.TroopsPerTurn = i.TroopsPerTurn + 5;
                if (m.OwnsContinent(i, m.Africa)) i.TroopsPerTurn = i.TroopsPerTurn + 5;
                if (m.OwnsContinent(i, m.Europe)) i.TroopsPerTurn = i.TroopsPerTurn + 5;
                if (m.OwnsContinent(i, m.Asia)) i.TroopsPerTurn = i.TroopsPerTurn + 5;
                if (m.OwnsContinent(i, m.Australia)) i.TroopsPerTurn = i.TroopsPerTurn + 5;
            }
            if (state == "Main") turn.TroopCount = turn.TroopCount + turn.TroopsPerTurn;
        }

        public void AdvanceTurnPart()
        {
            if (turnPart == "Fortify")
            {
                turnPart = "Attack";
            }
        }

        public void EliminatePlayer(Player p)
        {
            foreach (Card i in p.Cards) turn.AddCard(i);
            p.Cards.Clear();
            players.Remove(p);
        }
    }
}
