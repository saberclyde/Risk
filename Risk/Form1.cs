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
            // initial set-up
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
            // Assign territories to labels
            foreach (Territory i in map.Territories)
            {
                ((TerritoryLabel)mapBox.Controls[$"b{i.Name.Replace(" ", "")}"]).Territory = i;
            }
        }

        private void show()
        {
            // hide the title screen and show the normal view
            twoplayerb.Visible = false;
            threeplayerb.Visible = false;
            fourplayerb.Visible = false;
            fiveplayerb.Visible = false;
            sixplayerb.Visible = false;
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
            titlePic.Visible = false;
        }

        private void hover(object sender, EventArgs e)
        {
            Territory c = ((TerritoryLabel)sender).Territory;
            // change the hover text to show information on a territory
            if (c.Owner == null) HoverText.Text = c.Name + " : Unowned : " + c.Troops + " Troops";
            else 
            { 
                if (c.Troops == 1) HoverText.Text = c.Name + " : " + c.Owner.Name + " : " + c.Troops + " Troop";
                else HoverText.Text = c.Name + " : " + c.Owner.Name + " : " + c.Troops + " Troops";
            }
        }

        private void changeInfoText()
        {
            // update the side box with a player's info during their turn or switch it
            Player player = game.Turn;
            infoBox.Text = $"======= {player.Name.ToUpper()} =======\nTroops left to place - {player.TroopCount}\n" +
                $"Troops per turn - {player.TroopsPerTurn}\nTerritories - {player.TerritoriesCount}\nCards - {player.CardsCount}\n======= CARDS =======";
        }

        private void updatePlayerText()
        {
            // update the player labels 
            foreach (Player i in game.Players)
            {
                playersBox.Controls[$"{i.Name.Replace(" ", "")}Label"].Text = $"{i.Name} : {i.CardsCount} Cards : {i.TroopsPerTurn} Troops per turn";
            }
        }

        private void cardsHelpButton_Click(object sender, EventArgs e)
        {
            // help with cards
            MessageBox.Show("Click on three cards that are either three different suits or all the same suit to exchange them for units.\n\n" +
                "The amount of units you will gain is displayed at the top of the screen labelled as \"Card Bonus.\"\n\n" +
                "Wild cards can be substituted for any card in a pair.\n\n" +
                "If you own a card's territory then you gain two armies on that territory when you trade in the set.\n\n" +
                "Upon your defeat, the player that killed you gets all of your cards.\n\n" +
                "You can double click on a card to remove it from the selection.", "Cards Tutorial", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void turnHelpButton_Click(object sender, EventArgs e)
        {
            // show a help diaglogue depending on what part the game is at
            if (game.State == "Claiming") MessageBox.Show("Starting with a randomly chosen player, each player must claim a territory until there are no more unowned territories left.", "Claiming Tutorial", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (game.State == "Remaining") MessageBox.Show("Place the rest of your troops on territories that you own.", "Claiming Tutorial", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (game.State == "Main")
            {
                if (game.TurnPart == "Fortify") MessageBox.Show("During the first part of your turn, you may turn in sets of cards to gain additional troops to play.\n\n" +
                    "Click on a territory to add troops to it.\n\n" +
                    "You must place all of your troops before you can continue your turn.", "Fortifying Tutorial", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (game.TurnPart == "Attack") MessageBox.Show("Click on a territory you own with more than one troop in it to attack from it.\n\n" +
                    "Click on an opponents territory to choose to attack it\n\n" +
                    "You must have 1 more troop in the attacking territory than the number of dice you decide to roll\n\n" +
                    "If you manage to conquer at least 1 territory during your turn you are awarded with a card.\n\n" +
                    "If you conquer the rest of a player's territories during your turn you are awarded with all of that player's cards. The game ends when you are the only player remaining." +
                    "You may skip this part of your turn by simply pressing the end button", "Attacking Tutorial", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (game.TurnPart == "Move") MessageBox.Show("Click on a territory to transfer troops to a different territory connected to that territory.\n\n" +
                    "Specify a number of troops using the input box and then click along a path of your territories to where you wish to move to. Pressing the end button will end your turn and place the troops there.\n\n" +
                    "You may skip this part of your turn by simply pressing the end button", "Moving Tutorial", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (game.TurnPart == "TakeOver") MessageBox.Show("Specify how many troops you wish to move onto this territory.\n\n" +
                    "You must move a minumum of 1 troop for every attack dice rolled.", "Attacking Tutorial", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void updateTroopCount(TerritoryLabel l)
        {
            // update a territorylabel's color and troop count
            Territory c = l.Territory;
            l.Text = c.Name + "\n" + c.Troops;
            if (c.Owner == null) l.ForeColor = Color.Black;
            else l.ForeColor = c.Owner.Colour;
        }

        private void updateTurnText()
        {
            // update the turn text's text and colour to the current player's
            turnText.Text = $"{game.Turn.Name}'s Turn ";
            turnText.ForeColor = game.Turn.Colour;
        }

        private void updateBonusText()
        {
            // update the card bonus text
            bonusText.Text = "Card Bonus - " + game.CardBonus;
        }

        private void updateCards()
        {
            // is called whenever a card is removed and upon switching turns
            // creates a list of cards for the player to interact with
            cardsList.Controls.Clear();
            foreach (Card i in game.Turn.Cards)
            {
                // create new button for every card
                CardButton button = new CardButton();
                button.Card = i;
                if (i.Wild == true) button.Text = "Wild Card"; // Wild Card
                else button.Text = $"{i.Symbol} ({i.Territory.Name})"; // Horse (Middle East)
                if (game.Turn.CardsCount < 15) button.Size = new Size(215, 23); // without scrollbar
                else button.Size = new Size(200, 23); // with scrollbar (resizes so it fits)
                button.AutoSize = false;
                button.FlatStyle = FlatStyle.System;
                button.Click += new EventHandler(cardClick); // select card
                button.DoubleClick += new EventHandler(cardDoubleClick); // deselect card
                button.Margin = new Padding(1);
                cardsList.Controls.Add(button); // add card button
            }
        }

        private void updateSelectText()
        {
            // update the card selection text
            selectedLabel.Text = "Selected:";
            foreach (Card i in selected)
            {
                if (i != selected[0]) selectedLabel.Text = selectedLabel.Text + ","; // make it so the commas go in correctly
                if (i.Wild == true) selectedLabel.Text = selectedLabel.Text + " Wild Card"; // Wild card
                else selectedLabel.Text = selectedLabel.Text + $" {i.Symbol} ({i.Territory.Name})";
            }
        }

        private void updateTurnPartText()
        {
            // updates the text above the dice to add a visual thing so it's more clear what part of ur turn you're on
            if (game.TurnPart != "Attack") diceText.Visible = false; // dice text only visible when attacking
            else diceText.Visible = true; // dice text only visible when attacking
            if (game.State == "Main" && game.TurnPart == "Attack") // show selected two countries during the attack part of ur turn
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
            else if (game.State == "Main" && game.TurnPart == "Fortify") selectedT.Text = "Fortify territories"; // Fortify
            else if (game.State == "Main" && game.TurnPart == "Move") selectedT.Text = "Move troops"; // Move
        }

        private void updateDiceText()
        {
            // show the number of dice each player is using
            if (attackDice == 0) diceText.Text = "?";
            else diceText.Text = $"{attackDice}";
            diceText.Text = diceText.Text + " Red Dice vs. ";
            if (defendDice == 0) diceText.Text = diceText.Text + " ?";
            else diceText.Text = diceText.Text + $"{defendDice}";
            diceText.Text = diceText.Text + " Blue Dice";

        }

        private bool threeDifferentCards()
        {
            // if any cards are the same symbol then return false, otherwise return true
            if (selected[0].Symbol == selected[1].Symbol) return false;
            if (selected[0].Symbol == selected[2].Symbol) return false;
            if (selected[1].Symbol == selected[2].Symbol) return false;
            return true;
        }

        private void cardClick(object sender, EventArgs e)
        {
            Card c = ((CardButton)sender).Card;
            if (selected.Contains(c)) return; // do nothing if the card is already selected
            selected.Add(c);
            updateSelectText();

            // do this part twice so that no matter what the first card in the order is ALWAYS a non-wild card
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

            // when 3 are selected do this part
            // check if it's a pair
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
                else if (threeDifferentCards() == true)
                {
                    redeemCards();
                }
                // ELSE / NO PAIR
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
            game.Turn.TroopCount = game.Turn.TroopCount + game.CardBonus; // increase troops
            game.IncreaseBonus(); // increase the card bonus
            updateBonusText();
            changeInfoText();

            // put 2 troops on the card's territory if the player redeeming the card owns it
            foreach (Card i in selected)
            {
                if (i.Territory.Owner == game.Turn)
                {
                    i.Territory.Troops = i.Territory.Troops + 2;
                    updateTroopCount(mapBox.Controls[$"b{i.Territory.RefName}"] as TerritoryLabel);
                }
            }

            // take away redeemed cards
            game.Turn.RemoveCard(selected[0]);
            game.Turn.RemoveCard(selected[1]);
            game.Turn.RemoveCard(selected[2]);
            selected.Clear();
            updateSelectText();
            updateCards();
        }

        private void cardDoubleClick(object sender, EventArgs e)
        {
            // deselect a card when double clicked
            Card c = ((CardButton)sender).Card;
            selected.Remove(c);
            updateSelectText();
            disableDice();
        }

        private void territoryClick(object sender, EventArgs e)
        {
            Territory c = ((TerritoryLabel)sender).Territory;
            // CLAIMING
            if (game.State == "Claiming" && c.Owner == null)
            {
                // add troop to territory
                game.ClaimTerritory(c);
                hover(sender, e);
                updateTroopCount(((TerritoryLabel)sender));
                // advance turns
                game.AdvanceTurn(map);
                // update all the text
                updateTurnText();
                changeInfoText();
                updateCards();
                updatePlayerText();
            }
            else if (game.State == "Remaining" && c.Owner == game.Turn)
            {
                // add troop to territory
                c.Troops++;
                hover(sender, e);
                updateTroopCount(((TerritoryLabel)sender));
                game.Turn.TroopCount--;
                // advance turns
                game.AdvanceTurn(map);
                // update all the text
                updateTurnPartText();
                updateTurnText();
                changeInfoText();
                updatePlayerText();
            }
            else if (game.State == "Main")
            {
                if (game.TurnPart == "Fortify" && c.Owner == game.Turn)
                {
                    // add troop to territory
                    game.Turn.TroopCount--;
                    c.Troops++;
                    hover(sender, e);
                    updateTroopCount(((TerritoryLabel)sender));
                    changeInfoText();
                    // advance to the attacking stage if you've placed all ur troops
                    if (game.Turn.TroopCount == 0)
                    {
                        game.AdvanceTurnPart();
                        updateTurnPartText();
                        foreach (CardButton i in cardsList.Controls) i.Enabled = false;
                        endButton.Enabled = true;
                    }
                }
                else if (game.TurnPart == "Attack")
                {
                    if (c.Owner == game.Turn)
                    {
                        if (c.Troops == 1) // cannot use it to attack if there's only 1 troop in it
                        {
                            MessageBox.Show("You must have more than one troop in a territory to use it to attack.",
                                "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (defender == null) // set attacker
                        {
                            attacker = c;
                            updateTurnPartText();
                        }
                        else if (defender.AdjacentTerritories.Contains(c)) // if a defender has been set then make sure the attacker is adjacent the defender
                        {
                            attacker = c;
                            updateTurnPartText();
                        }
                    }
                    else
                    {
                        if (attacker == null) // set defender
                        {
                            defender = c;
                            updateTurnPartText();
                        }
                        else if (attacker.AdjacentTerritories.Contains(c)) // if an attacker has been set then make sure the defender is adjecent to the attacker
                        {
                            defender = c;
                            updateTurnPartText();
                        }
                    }
                    if (attacker != null && defender != null) // if both attacker and defender are chosen then let them roll
                    { 
                        enableDice();
                        // disable dice depending on how many troops there are
                        if (defender.Troops == 1) defendTwo.Enabled = false;
                        if (attacker.Troops == 2) { attackTwo.Enabled = false; attackThree.Enabled = false; }
                        else if (attacker.Troops == 3) attackThree.Enabled = false;
                    }
                }
                else if (game.TurnPart == "Move" && c.Owner == game.Turn)
                {
                    if (moving)
                    {
                        // if moving then move the selected amount of troops to the adjacent territory
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
                        // need a minimum of 2 troops to transfer troops
                        if (c.Troops == 1)
                        {
                            MessageBox.Show("You must have more than one troop in a territory transfer troops from it.",
                                "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // set mins and max of move box
                        moveBox.Maximum = c.Troops - 1;
                        moveBox.Minimum = 1;
                        moveBox.Enabled = true;
                        lastMove = c;
                        moving = true; // yes movig
                    }
                }
            }
        }

        private void territoryDoubleClick(object sender, EventArgs e)
        {
            Territory c = ((TerritoryLabel)sender).Territory;
            if (game.TurnPart != "Attack") return;
            // remove attacker
            if (c == attacker)
            {
                attacker = null;
                updateTurnPartText();
            }
            // remove defender
            else if (c == defender)
            {
                defender = null;
                updateTurnPartText();
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
            // is it red/attack dice or blue/defend dice
            if ((sender as Button).Tag == "a") attackDice = n;
            else defendDice = n;
            updateDiceText();
            // if both dice are chosen
            if (attackDice != 0 && defendDice != 0)
            {
                // roll lists
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

                // calculate wins and losses
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

                // check if the territory has been conquered
                if (defender.Troops < 1)
                {
                    Player l = defender.Owner;
                    defender.Owner.LoseTerritory(defender);
                    defender.Owner = game.Turn;
                    game.Turn.GainTerritory(defender);
                    defender.Troops = 0;
                    // has defender player been eliminated
                    if (l.TerritoriesCount == 0)
                    {
                        game.EliminatePlayer(l);
                        updateCards();
                        updatePlayerText();
                        changeInfoText();
                        playersBox.Controls.Remove(playersBox.Controls[$"{l.Name.Replace(" ", "")}Label"]);
                        MessageBox.Show($"{l.Name} has been eliminated.",  "Player Eliminated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // have all other players been eliminated
                        if (game.Players.Count == 1)
                        {
                            MessageBox.Show($"{game.Turn.Name} has won!.", "Game Ended", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // reset game
                            game.ResetGame(map);
                            twoplayerb.Visible = true;
                            threeplayerb.Visible = true;
                            fourplayerb.Visible = true;
                            fiveplayerb.Visible = true;
                            sixplayerb.Visible = true;
                            titlePic.Visible = true;
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
                    // claim the territory
                    game.TurnPart = "TakeOver";
                    moveBox.Enabled = true;
                    endButton.Enabled = true;
                    moveBox.Minimum = attackDice;
                    moveBox.Maximum = attacker.Troops - 1;
                    updateDiceText();
                    disableDice();
                    getsCard = true; // player gets a card if they've conquered this turn
                }
                // display battle results
                updateTroopCount(mapBox.Controls[$"b{attacker.RefName}"] as TerritoryLabel);
                updateTroopCount(mapBox.Controls[$"b{defender.RefName}"] as TerritoryLabel);
                MessageBox.Show($"Attacking roll: {String.Join(",", aDisplay)}\nDefending roll: {String.Join(",", dDisplay)}\n\nAttacker loses {attackLose} and defender loses {defendLose}.",
                    "Battle Results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                attackDice = 0;
                defendDice = 0;
                updateDiceText();
                // recheck dice
                if (defender.Troops == 1) defendTwo.Enabled = false;
                if (attacker.Troops == 2) { attackTwo.Enabled = false; attackThree.Enabled = false; }
                else if (attacker.Troops == 3) attackThree.Enabled = false;
                // cant attack if only one
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
                // move troops into the conquered territory
                int n = Convert.ToInt16(moveBox.Value);
                attacker.Troops = attacker.Troops - n;
                defender.Troops = n;
                updateTroopCount(mapBox.Controls[$"b{attacker.RefName}"] as TerritoryLabel);
                updateTroopCount(mapBox.Controls[$"b{defender.RefName}"] as TerritoryLabel);
                moveBox.Enabled = false;
                game.TurnPart = "Attack";
                attacker = null;
                defender = null;
                updateTurnPartText();
            }
            else if (game.TurnPart == "Attack")
            {
                // end attacking
                game.TurnPart = "Move";
                attacker = null;
                defender = null;
                updateTurnPartText();
                disableDice();
            }
            else if (game.TurnPart == "Move")
            {
                // end moving
                // give player their card
                if (getsCard == true)
                {
                    game.GiveCard(game.Turn);
                    getsCard = false;
                }
                // end turn
                game.AdvanceTurn(map);
                updateTurnPartText();
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
            // create player labels
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

        private void sixplayerb_Click(object sender, EventArgs e)
        {
            game.SetUp(6, map);
            playersButtonCommon();
        }
    }
}