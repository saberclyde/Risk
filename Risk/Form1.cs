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
        private List<Card> selected = new List<Card> { };
        private Territory attacker = null;
        private Territory defender = null;
        private int attackDice = 0;
        private int defendDice = 0;
        private bool getsCard = false;
        private int move = 0;
        private bool moving = false;
        private Territory lastMove;

        public Form1()
        {
            InitializeComponent();
            game.State = "Claiming";
            game.TurnPart = "Fortify";
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
            mapBox.Visible = false;
            selectedLabel.Visible = false;
            moveBox.Visible = false;
            attackOne.Visible = false;
            attackTwo.Visible = false;
            attackThree.Visible = false;
            defendOne.Visible = false;
            defendTwo.Visible = false;
            endButton.Visible = false;
            moveBox.Enabled = false;
            disableDice();
            endButton.Enabled = false;
            selectedT.Visible = false;
            turnHelpButton.Visible = false;
            diceText.Visible = false;
            foreach (Territory i in map.Territories)
            {
                ((TerritoryLabel)mapBox.Controls[$"b{i.Name.Replace(" ", "")}"]).Territory = i;
            }
        }

        private void show()
        {
            twoplayerb.Visible = false;
            threeplayerb.Visible = false;
            fourplayerb.Visible = false;
            fiveplayerb.Visible = false;
            HoverText.Visible = true;
            bonusText.Visible = true;
            turnText.Visible = true;
            cardsList.Visible = true;
            cardsHelpButton.Visible = true;
            infoBox.Visible = true;
            playersBox.Visible = true;
            mapBox.Visible = true;
            selectedLabel.Visible = true;
            moveBox.Visible = true;
            attackOne.Visible = true;
            attackTwo.Visible = true;
            attackThree.Visible = true;
            defendOne.Visible = true;
            defendTwo.Visible = true;
            endButton.Visible = true;
            turnHelpButton.Visible = true;
            selectedT.Visible = true;
        }

        private void hover(object sender, EventArgs e)
        {
            Territory c = ((TerritoryLabel)sender).Territory;
            if (c.Owner == null) HoverText.Text = c.Name + " : Unowned : " + c.Troops + " Troops";
            else 
            { 
                if (c.Troops == 1) HoverText.Text = c.Name + " : " + c.Owner.Name + " : " + c.Troops + " Troop";
                else HoverText.Text = c.Name + " : " + c.Owner.Name + " : " + c.Troops + " Troops";
            }
        }

        private void changeInfoText()
        {
            Player player = game.Turn;
            infoBox.Text = $"======= {player.Name.ToUpper()} =======\nTroops left to place - {player.TroopCount}\n" +
                $"Troops per turn - {player.TroopsPerTurn}\nTerritories - {player.TerritoriesCount}\nCards - {player.CardsCount}\n======= CARDS =======";
        }

        private void updatePlayerText()
        {
            foreach (Player i in game.Players)
            {
                playersBox.Controls[$"{i.Name.Replace(" ", "")}Label"].Text = $"{i.Name} : {i.CardsCount} Cards : {i.TroopsPerTurn} Troops per turn";
            }
        }

        private void cardsHelpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Click on three cards that are either three different suits or all the same suit to exchange them for units.\n\n" +
                "The amount of units you will gain is displayed at the top of the screen labelled as \"Card Bonus.\"\n\n" +
                "Wild cards can be substituted for any card in a pair.\n\n" +
                "If you own a card's territory then you gain two armies on that territory when you trade in the set.\n\n" +
                "Upon your defeat, the player that killed you gets all of your cards.\n\n" +
                "You can double click on a card to remove it from the selection.", "Cards Tutorial", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void turnHelpButton_Click(object sender, EventArgs e)
        {
            if (game.State == "Claiming") MessageBox.Show("Starting with a randomly chosen player, each player must claim a territory until there are no more unowned territories left.", "Set-up Tutorial", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (game.State == "Remaining") MessageBox.Show("Place the rest of your troops on territories that you own.", "Set-up Tutorial", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (game.TurnPart == "Fortify" && game.State == "Main") MessageBox.Show("During the first part of your turn you may turn in sets of cards to gain additional troops to play.\n\n" +
                "Click on a territory to add troops to it.\n\n" +
                "You must place all of your troops before you can continue your turn.", "Turn Tutorial", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void updateTroopCount(TerritoryLabel l)
        {
            Territory c = l.Territory;
            l.Text = c.Name + "\n" + c.Troops;
            if (c.Owner == null) l.ForeColor = Color.Black;
            else l.ForeColor = c.Owner.Colour;
        }

        private void updateTurnText()
        {
            turnText.Text = $"{game.Turn.Name}'s Turn ";
            turnText.ForeColor = game.Turn.Colour;
        }

        private void updateBonusText()
        {
            bonusText.Text = "Card Bonus - " + game.CardBonus;
        }

        private void updateCards()
        {
            cardsList.Controls.Clear();
            foreach (Card i in game.Turn.Cards)
            {
                CardButton button = new CardButton();
                button.Card = i;
                if (i.Wild == true) button.Text = "Wild Card";
                else button.Text = $"{i.Symbol} ({i.Territory.Name})";
                if (game.Turn.CardsCount < 15) button.Size = new Size(215, 23);
                else button.Size = new Size(200, 23);
                button.AutoSize = false;
                button.FlatStyle = FlatStyle.System;
                button.Click += new EventHandler(cardClick);
                button.DoubleClick += new EventHandler(cardDoubleClick);
                button.Margin = new Padding(1);
                cardsList.Controls.Add(button);
            }
        }

        private void updateSelectText()
        {
            selectedLabel.Text = "Selected:";
            foreach (Card i in selected)
            {
                if (i != selected[0]) selectedLabel.Text = selectedLabel.Text + ",";
                if (i.Wild == true) selectedLabel.Text = selectedLabel.Text + " Wild Card";
                else selectedLabel.Text = selectedLabel.Text + $" {i.Symbol} ({i.Territory.Name})";
            }
        }

        private void updateTSelect()
        {
            if (game.TurnPart != "Attack") diceText.Visible = false;
            else diceText.Visible = true;
            if (game.State == "Main" && game.TurnPart == "Attack")
            {
                if (attacker == null)
                {
                    if (defender == null) selectedT.Text = "Selected: ??? to ???";
                    else selectedT.Text = "Selected: ??? to " + defender.Name;
                }
                else
                {
                    if (defender == null) selectedT.Text = "Selected: " + attacker.Name + " to ???";
                    else selectedT.Text = "Selected: " + attacker.Name + " to " + defender.Name;
                }
            }
            else if (game.State == "Main" && game.TurnPart == "Fortify") selectedT.Text = "Fortify territories";
            else if (game.State == "Main" && game.TurnPart == "Move") selectedT.Text = "Transfer troops";
        }

        private void updateDiceText()
        {
            if (attackDice == 0) diceText.Text = "?";
            else diceText.Text = $"{attackDice}";
            diceText.Text = diceText.Text + " Red Dice vs. ";
            if (defendDice == 0) diceText.Text = diceText.Text + " ?";
            else diceText.Text = diceText.Text + $"{defendDice}";
            diceText.Text = diceText.Text + " Blue Dice";

        }

        private bool threeOfAKind()
        {
            if (selected[0].Symbol == selected[1].Symbol) return false;
            if (selected[0].Symbol == selected[2].Symbol) return false;
            if (selected[1].Symbol == selected[2].Symbol) return false;
            return true;
        }

        private void cardClick(object sender, EventArgs e)
        {
            Card c = ((CardButton)sender).Card;
            if (selected.Contains(c)) return;
            selected.Add(c);
            updateSelectText();
            if (selected[0].Wild == true)
            {
                Card temp = selected[0];
                selected.RemoveAt(0);
                selected.Add(temp);
            }
            if (selected[0].Wild == true)
            {
                Card temp = selected[0];
                selected.RemoveAt(0);
                selected.Add(temp);
            }
            if (selected.Count == 3)
            {
                // THREE OF A KIND
                string suit = selected[0].Symbol;
                if (selected[1].Symbol == suit || selected[1].Wild == true)
                {
                    if (selected[2].Symbol == suit || selected[2].Wild == true)
                    {
                        redeemCards();
                    }
                }
                // THREE DIFFERENT
                else if (threeOfAKind() == true)
                {
                    redeemCards();
                }
                // ELSE
                else
                {
                    selected.Clear();
                    updateSelectText();
                    return;
                }
            }
        }

        private void redeemCards()
        {
            game.Turn.TroopCount = game.Turn.TroopCount + game.CardBonus;
            game.IncreaseBonus();
            updateBonusText();
            changeInfoText();
            foreach (Card i in selected)
            {
                if (i.Territory.Owner == game.Turn)
                {
                    i.Territory.Troops = i.Territory.Troops + 2;
                    updateTroopCount(mapBox.Controls[$"b{i.Territory.RefName}"] as TerritoryLabel);
                }
            }
            game.Turn.RemoveCard(selected[0]);
            game.Turn.RemoveCard(selected[1]);
            game.Turn.RemoveCard(selected[2]);
            selected.Clear();
            updateSelectText();
            updateCards();
        }

        private void cardDoubleClick(object sender, EventArgs e)
        {
            Card c = ((CardButton)sender).Card;
            selected.Remove(c);
            updateSelectText();
            disableDice();
        }

        private void territoryClick(object sender, EventArgs e)
        {
            Territory c = ((TerritoryLabel)sender).Territory;
            if (game.State == "Claiming" && c.Owner == null)
            {
                game.ClaimTerritory(c);
                hover(sender, e);
                ((TerritoryLabel)sender).ForeColor = game.Turn.Colour;
                updateTroopCount(((TerritoryLabel)sender));
                game.AdvanceTurn(map);
                updateTurnText();
                changeInfoText();
                updateCards();
                updatePlayerText();
            }
            else if (game.State == "Remaining" && c.Owner == game.Turn)
            {
                c.Troops++;
                hover(sender, e);
                updateTroopCount(((TerritoryLabel)sender));
                game.Turn.TroopCount--;
                game.AdvanceTurn(map);
                updateTSelect();
                updateTurnText();
                changeInfoText();
                updatePlayerText();
            }
            else if (game.State == "Main")
            {
                if (game.TurnPart == "Fortify" && c.Owner == game.Turn)
                {
                    game.Turn.TroopCount--;
                    c.Troops++;
                    hover(sender, e);
                    updateTroopCount(((TerritoryLabel)sender));
                    changeInfoText();
                    if (game.Turn.TroopCount == 0)
                    {
                        game.AdvanceTurnPart();
                        updateTSelect();
                        foreach (CardButton i in cardsList.Controls) i.Enabled = false;
                        endButton.Enabled = true;
                    }
                }
                else if (game.TurnPart == "Attack")
                {
                    if (c.Owner == game.Turn)
                    {
                        if (c.Troops == 1)
                        {
                            MessageBox.Show("You must have more than one troop in a territory to use it to attack.",
                                "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (defender == null)
                        {
                            attacker = c;
                            updateTSelect();
                        }
                        else if (defender.AdjacentTerritories.Contains(c))
                        {
                            attacker = c;
                            updateTSelect();
                        }
                    }
                    else
                    {
                        if (attacker == null)
                        {
                            defender = c;
                            updateTSelect();
                        }
                        else if (attacker.AdjacentTerritories.Contains(c))
                        {
                            defender = c;
                            updateTSelect();
                        }
                    }
                    if (attacker != null && defender != null) 
                    { 
                        enableDice();
                        if (defender.Troops == 1) defendTwo.Enabled = false;
                        if (attacker.Troops == 2) { attackTwo.Enabled = false; attackThree.Enabled = false; }
                        else if (attacker.Troops == 3) attackThree.Enabled = false;
                    }
                }
                else if (game.TurnPart == "Move" && c.Owner == game.Turn)
                {
                    if (moving)
                    {
                        if (lastMove.AdjacentTerritories.Contains(c))
                        {
                            move = Convert.ToInt16(moveBox.Value);
                            moveBox.Enabled = false;
                            lastMove.Troops = lastMove.Troops - move;
                            c.Troops = c.Troops + move;
                            hover(sender, e);
                            updateTroopCount(sender as TerritoryLabel);
                            updateTroopCount(mapBox.Controls[$"b{lastMove.RefName}"] as TerritoryLabel);
                            lastMove = c;
                        }
                    }
                    else
                    {
                        if (c.Troops == 1)
                        {
                            MessageBox.Show("You must have more than one troop in a territory transfer troops from it.",
                                "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        moveBox.Maximum = c.Troops - 1;
                        moveBox.Minimum = 1;
                        moveBox.Enabled = true;
                        lastMove = c;
                        moving = true;
                    }
                }
            }
        }

        private void territoryDoubleClick(object sender, EventArgs e)
        {
            Territory c = ((TerritoryLabel)sender).Territory;
            if (game.TurnPart != "Attack") return;
            if (c == attacker)
            {
                attacker = null;
                updateTSelect();
            }
            else if (c == defender)
            {
                defender = null;
                updateTSelect();
            }
            disableDice();
        }

        private void enableDice()
        {
            attackOne.Enabled = true;
            attackTwo.Enabled = true;
            attackThree.Enabled = true;
            defendOne.Enabled = true;
            defendTwo.Enabled = true;
        }

        private void disableDice()
        {
            attackOne.Enabled = false;
            attackTwo.Enabled = false;
            attackThree.Enabled = false;
            defendOne.Enabled = false;
            defendTwo.Enabled = false;
        }

        private void diceClick(object sender, EventArgs e)
        {
            int attackLose = 0, defendLose = 0;
            
            string t = (sender as Button).Text;
            int n = Convert.ToInt16(t);
            if ((sender as Button).Tag == "a") attackDice = n;
            else defendDice = n;
            updateDiceText();
            if (attackDice != 0 && defendDice != 0)
            {
                List<int> attackD = new List<int> { };
                List<int> aDisplay = new List<int> { };
                List<int> defendD = new List<int> { };
                List<int> dDisplay = new List<int> { };
                for (int i = 0; i < attackDice; i++) attackD.Add(game.roll());
                for (int i = 0; i < defendDice; i++) defendD.Add(game.roll());
                defendD.Sort();
                defendD.Reverse();
                attackD.Sort();
                attackD.Reverse();
                aDisplay.AddRange(attackD);
                dDisplay.AddRange(defendD);
                foreach (int i in defendD)
                {
                    if (attackD.Count == 0) break;
                    if (i == 6) { attackD.RemoveAt(0); attackLose++; }
                    else if (i == attackD[0]) { attackD.RemoveAt(0); attackLose++; }
                    else if (i > attackD[0]) { attackD.RemoveAt(0); attackLose++; }
                    else if (i < attackD[0]) { attackD.RemoveAt(0); defendLose++; }
                }
                attacker.Troops = attacker.Troops - attackLose;
                defender.Troops = defender.Troops - defendLose;
                if (defender.Troops < 1)
                {
                    Player l = defender.Owner;
                    defender.Owner.LoseTerritory(defender);
                    defender.Owner = game.Turn;
                    game.Turn.GainTerritory(defender);
                    defender.Troops = 0;
                    if (l.TerritoriesCount == 0)
                    {
                        game.EliminatePlayer(l);
                        updateCards();
                        updatePlayerText();
                        changeInfoText();
                        playersBox.Controls.Remove(playersBox.Controls[$"{l.Name.Replace(" ", "")}Label"]);
                        MessageBox.Show($"{l.Name} has been eliminated.",  "Player Eliminated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (game.Players.Count == 1)
                        {
                            MessageBox.Show($"{game.Turn.Name} has won!.", "Game Ended", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            game.ResetGame(map);
                            twoplayerb.Visible = true;
                            threeplayerb.Visible = true;
                            fourplayerb.Visible = true;
                            fiveplayerb.Visible = true;
                            mapBox.Visible = false;
                            cardsList.Visible = false;
                            playersBox.Visible = false;
                            selectedLabel.Visible = false;
                            infoBox.Visible = false;
                            HoverText.Visible = false;
                            turnText.Visible = false;
                            bonusText.Visible = false;
                            diceText.Visible = false;
                            selectedT.Visible = false;
                            turnHelpButton.Visible = false;
                            cardsHelpButton.Visible = false;
                            defendOne.Visible = false;
                            defendTwo.Visible = false;
                            attackOne.Visible = false;
                            attackTwo.Visible = false;
                            attackThree.Visible = false;
                            endButton.Visible = false;
                            moveBox.Visible = false;
                            endButton.Enabled = false;
                            bonusText.Text = "Card Bonus - " + game.CardBonus;
                            HoverText.Text = "";
                            disableDice();
                            foreach (Control i in mapBox.Controls)
                            {
                                if (i.Name[0] == 'b')
                                {
                                    updateTroopCount(i as TerritoryLabel);
                                }
                            }
                            return;
                        }
                    }
                    game.TurnPart = "TakeOver";
                    moveBox.Enabled = true;
                    endButton.Enabled = true;
                    moveBox.Minimum = attackDice;
                    moveBox.Maximum = attacker.Troops - 1;
                    updateDiceText();
                    disableDice();
                    getsCard = true;
                }
                updateTroopCount(mapBox.Controls[$"b{attacker.RefName}"] as TerritoryLabel);
                updateTroopCount(mapBox.Controls[$"b{defender.RefName}"] as TerritoryLabel);
                MessageBox.Show($"Attacking roll: {String.Join(",", aDisplay)}\nDefending roll: {String.Join(",", dDisplay)}\n\nAttacker loses {attackLose} and defender loses {defendLose}.",
                    "Battle Results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                attackDice = 0;
                defendDice = 0;
                updateDiceText();
                if (defender.Troops == 1) defendTwo.Enabled = false;
                if (attacker.Troops == 2) { attackTwo.Enabled = false; attackThree.Enabled = false; }
                else if (attacker.Troops == 3) attackThree.Enabled = false;
                if (attacker.Troops == 1)
                {
                    attacker = null;
                    defender = null;
                    updateSelectText();
                    disableDice();
                }
            }
        }
        
        private void endButton_Click(object sender, EventArgs e)
        {
            if (game.TurnPart == "TakeOver")
            {
                int n = Convert.ToInt16(moveBox.Value);
                attacker.Troops = attacker.Troops - n;
                defender.Troops = n;
                updateTroopCount(mapBox.Controls[$"b{attacker.RefName}"] as TerritoryLabel);
                updateTroopCount(mapBox.Controls[$"b{defender.RefName}"] as TerritoryLabel);
                moveBox.Enabled = false;
                game.TurnPart = "Attack";
                attacker = null;
                defender = null;
                updateTSelect();
            }
            else if (game.TurnPart == "Attack")
            {
                game.TurnPart = "Move";
                attacker = null;
                defender = null;
                updateTSelect();
                disableDice();
            }
            else if (game.TurnPart == "Move")
            {
                if (getsCard == true)
                {
                    game.GiveCard(game.Turn);
                    getsCard = false;
                }
                game.AdvanceTurn(map);
                updateTSelect();
                updateTurnText();
                changeInfoText();
                updatePlayerText();
                updateCards();
                moveBox.Enabled = false;
                endButton.Enabled = false;
                moving = false;
            }
        }

        private void playersButtonCommon()
        {
            updateTurnText();
            show();
            foreach (Player i in game.Players)
            {
                Label label = new Label();
                label.Name = $"{i.Name.Replace(" ", "")}Label";
                label.ForeColor = i.Colour;
                label.Text = $"{i.Name} : {i.CardsCount} Cards : {i.TroopsPerTurn} Troops per turn";
                label.MaximumSize = new Size(0, 13);
                label.AutoSize = true;
                playersBox.Controls.Add(label);
            }
            changeInfoText();
        }
        
        private void twoplayerb_Click(object sender, EventArgs e)
        {
            game.SetUp(2, map);
            playersButtonCommon();
        }

        private void threeplayerb_Click(object sender, EventArgs e)
        {
            game.SetUp(3, map);
            playersButtonCommon();
        }

        private void fourplayerb_Click(object sender, EventArgs e)
        {
            game.SetUp(4, map);
            playersButtonCommon();
        }

        private void fiveplayerb_Click(object sender, EventArgs e)
        {
            game.SetUp(5, map);
            playersButtonCommon();
        }
    }
}