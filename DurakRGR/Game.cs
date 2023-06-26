using CardLib;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace DurakRGR
{
    public partial class Game
    {

        private int nextPlayer(int player)
        {
            return (player + 1) % (playersInGame.Count());
        }

        #region Functions

        private void dealPlayerHands()
        {
            pbTrumpImage.Visible = false;

            int playerToDealTo;
            PictureCard dealtCard = new PictureCard();
            for (int i = 0; i < playersInGame.Count() * playerHandSize; i++)
            {
                try
                {
                    playerToDealTo = (attacker + i) % playersInGame.Count();

                    dealtCard = dealTopCardToPlayer(playersInGame[playerToDealTo]);

                    Log.Write("Dealing card " + dealtCard.ToString() + " to player " + playersInGame[playerToDealTo].Name, false);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Debug.WriteLine(ex.ParamName.ToString());
                    break;
                }
            }

            if (gameDeckPlayer.Deck.Count() == 0)
            {
                Log.Write("Last card dealt was " + dealtCard.ToString() + " (owned by " + dealtCard.myPlayer.Name + ")", false);
                if (!dealtCard.myPlayer.IsHuman && Program.noHumanPlayer == false)
                {
                    dealtCard.Flip();
                    MessageBox.Show(dealtCard.myPlayer.Name + " has the trump card: " + dealtCard.ToString(), "Last card drawn");
                    dealtCard.Flip();
                }
                else
                {
                    MessageBox.Show((Program.noHumanPlayer ? dealtCard.myPlayer.Name + " has" : "You have") + " the trump card: " + dealtCard.ToString(), "Last card drawn");
                }
            }

        }

        void topUpPlayerHand(Player player)
        {
            int cardCount = 0;
            while (player.Deck.Count() < playerHandSize && gameDeckPlayer.Deck.Count() > 0)
            {
                dealTopCardToPlayer(player);
                cardCount++;
            }
            if (cardCount > 0)
                Log.Write("Gave " + player.Name + " " + cardCount.ToString() + " card" + (cardCount == 1 ? "" : "s") + " from the deck.", true);
        }

        void topUpPlayerHands()
        {
            topUpPlayerHand(attackingPlayer);

            foreach (Player eachPlayer in playersInGame)
            {
                if (eachPlayer != attackingPlayer && eachPlayer != defendingPlayer)
                    topUpPlayerHand(eachPlayer);
            }

            topUpPlayerHand(defendingPlayer);
        }

        void moveCardToTable(PictureCard card)
        {
            Log.Write("Moved " + card.ToString() + " to the table.", false);

            Player cardPlayer = card.myPlayer;

            cardPlayer.GiveToPlayer(card, gameTablePlayer);

            card.CardSelected -= playerCardClicked;

            card.Face = Face.Up;
        }

        void giveAllCardsOnTableToPlayer(Player unfortunatePlayer)
        {
            while (gameTablePlayer.Deck.Count() > 0)
            {
                PictureCard thisCard = gameTablePlayer.Deck[gameTablePlayer.Deck.Count() - 1];

                thisCard.Rotated = false;

                if (unfortunatePlayer.IsHuman)
                    thisCard.CardSelected += playerCardClicked;

                gameTablePlayer.GiveToPlayer(thisCard, unfortunatePlayer);

            }
        }

        private PictureCard dealTopCardToPlayer(Player thisPlayer)
        {
            if (gameDeckPlayer.Deck.Count() == 0)
                throw new ArgumentOutOfRangeException("Error: Tried to deal card from empty deck!");

            PictureCard selectedCard = gameDeckPlayer.Deck.Last();

            if (selectedCard.Rotated)
            {
                selectedCard.Rotate();
                selectedCard.Face = Face.Down;
            }

            if (thisPlayer.IsHuman)
            {
                selectedCard.Face = Face.Up;
                selectedCard.CardSelected += playerCardClicked;
            }

            gameDeckPlayer.GiveToPlayer(selectedCard, thisPlayer);

            return selectedCard;
        }

        private Boolean isValidDefendingMove(PictureCard attackingCard, PictureCard defendingCard)
        {
            return (Card)attackingCard.ToCard() < (Card)defendingCard.ToCard();
        }

        private bool isValidCardForContinuedAttack(PictureCard thisCard)
        {
            foreach (PictureCard tableCard in gameTablePlayer.Deck)
            {
                if (thisCard.Rank == tableCard.Rank)
                    return true;
            }
            return false;
        }

        private int defender
        {
            get
            {
                return nextPlayer(attacker);
            }
        }

        private bool gameDeckEmpty
        {
            get
            {
                return gameDeckPlayer.Deck.Count() == 0;
            }
        }

        internal Player attackingPlayer
        {
            get
            {
                return playersInGame[attacker];
            }
        }

        internal Player defendingPlayer
        {
            get
            {
                return playersInGame[defender];
            }
        }

        private void setContextButtonToPass()
        {
            btnContextual.Text = "Pass";
            btnContextual.Enabled = true;
            btnContextual.Click += btnContextual_PassOrTake;
            humanWantsToTakeOrPass = false;
        }

        private void setContextButtonToTake()
        {
            btnContextual.Text = "Take";
            btnContextual.Enabled = true;
            btnContextual.Click += btnContextual_PassOrTake;
            humanWantsToTakeOrPass = false;
        }

        private void LastCardRemovedFromDeckHandler(object sender, EventArgs e)
        {
            pbTrumpImage.Visible = true;
        }

        #endregion

        #region Functions: Accessors

        public int GetPlayerHandSize()
        {
            return playerHandSize;
        }

        public int playerCount()
        {
            return playersInGame.Count;
        }

        public Collection<Player> players
        {
            get
            {
                return playersInGame;
            }
        }

        #endregion
    }
}
