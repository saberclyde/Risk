using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Risk
{
    public partial class Form1 : Form
    {
        private Map map = new Map();
        private Game game = new Game();
        public Form1()
        {
            InitializeComponent();
            HoverText.Visible = false;
            HoverText.Text = "";
            bonusText.Visible = false;
            bonusText.Text = "Card Bonus - " + game.CardBonus;
            turnText.Visible = false;
            cardsList.Visible = false;
            cardsHelpButton.Visible = false;
            infoBox.Visible = false;
            playersBox.Visible = false;
            playersBox.Text = "";
        }

        private void show()
        {
            twoplayerb.Visible = false;
            HoverText.Visible = true;
            bonusText.Visible = true;
            turnText.Visible = true;
            cardsList.Visible = true;
            cardsHelpButton.Visible = true;
            infoBox.Visible = true;
            playersBox.Visible = true;
        }

        private void hover(object sender, EventArgs e)
        {
            Territory c = map.GetTerritory(Convert.ToInt16(((Label)sender).Tag.ToString()));
            if (c.Owner == null) HoverText.Text = c.Name + " : Unowned : " + c.Troops;
            else 
            { 
                if (c.Troops == 1) HoverText.Text = c.Name + " : " + c.Owner + " : " + c.Troops + " Troop";
                else HoverText.Text = c.Name + " : " + c.Owner + " : " + c.Troops + " Troops";
            }
        }

        private void changeInfoText()
        {
            Player player = game.Turn;
            infoBox.Text = $"======= {player.Name.ToUpper()} =======\nTroops left to place - {player.TroopCount}\n" +
                $"Troops per turn - {player.TroopsPerTurn}\nTerritories - {player.TerritoriesCount}\nCards - {player.CardsCount}\n======= CARDS =======";
        }

        private void twoplayerb_Click(object sender, EventArgs e)
        {
            game.SetUp(2);
            turnText.Text = $"{game.Turn.Name}'s Turn";
            show();
            turnText.ForeColor = game.Turn.Colour;
            for (int i = 1; i < game.PlayerCount + 1; i++)
            {
                Player player = game.Players[i - 1];
                Label label = new Label();
                    label.Name = $"player{i}Label";
                    label.ForeColor = player.Colour;
                    label.Text = $"{player.Name} : {player.CardsCount} Cards : {player.TroopsPerTurn} Troops/turn";
                    label.MaximumSize = new Size(999999999, 13);
                playersBox.Controls.Add(label);
            }
            changeInfoText();
        }

        private void cardsHelpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Click on three cards that are either three different suits or all the same suit to exchange them for units.\n\n" +
                "The amount of units you will gain is displayed at the top of the screen labelled as \"Card Bonus.\"\n\n" +
                "Wild cards can be substituted for one of the suits in a three different suits set.\n\n" +
                "If you own a card's territory then you gain two armies on that territory when you trade in the set.\n\n" +
                "Upon your defeat, the player that killed you gets all of your cards.");
        }
    }
}
