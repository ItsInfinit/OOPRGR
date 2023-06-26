
using System;
using System.Linq;

namespace CardLib
{
    public class Player
    {
        #region Variable Declarations

        private Deck playerDeck = new Deck();
        public CardPanel playerPanel = new CardPanel();

        private string playerName = "";
        private PlayerType thisPlayerType;
        public int originalIndex = -1;
        public static bool ALL_CARDS_FACE_UP = false;
        public event EventHandler<EventArgs> LastCardRemoved;


        public String Name
        {
            get { return playerName; }
            set { playerName = value; }
        }

        public bool IsAI
        {
            get
            {
                if (thisPlayerType == PlayerType.AI)
                    return true;
                else
                    return false;
            }
        }

        public bool IsHuman
        {
            get
            {
                if (thisPlayerType == PlayerType.Human)
                    return true;
                else
                    return false;
            }
        }

        public PlayerType PlayerType
        {
            get
            {
                return thisPlayerType;
            }
            set
            {
                thisPlayerType = value;
            }
        }

        #endregion

        #region Functions

        public void UpdateCard(PictureCard thisCard)
        {
            thisCard.cardNumber = playerDeck.IndexOf(thisCard);
            thisCard.myPlayer = this;
        }

        public void Add(PictureCard thisCard)
        {
            thisCard.CardSelected += ThisPlayerCardSelected;
            playerDeck.Add(thisCard);
            UpdateCard(thisCard);
            playerPanel.Controls.Add(playerDeck.Last());
        }

        public void GiveToPlayer(PictureCard thisCard, Player otherPlayer)
        {
            if (otherPlayer.thisPlayerType == PlayerType.Human)
            {
                thisCard.Hoverable = true;
                thisCard.Face = Face.Up;
            }
            else
            {
                thisCard.Hoverable = false;
                thisCard.Face = Face.Down;
            }

            if (ALL_CARDS_FACE_UP)
                thisCard.Face = Face.Up;

            Remove(thisCard);
            if (this.PlayerType != PlayerType.None)
            {
                playerDeck.Sort();
                UpdatePlayerPanelOrder();
                playerPanel.UpdateControlOrder();
            }

            otherPlayer.Add(thisCard);
            if (otherPlayer.thisPlayerType != PlayerType.None)
            {
                otherPlayer.Deck.Sort();
                otherPlayer.UpdatePlayerPanelOrder();
                otherPlayer.playerPanel.UpdateControlOrder();
                otherPlayer.playerPanel.UpdateControlZOrder();
            }
        }

        public void Remove(PictureCard thisCard)
        {
            if (playerDeck.Contains(thisCard))
            {
                thisCard.CardSelected -= ThisPlayerCardSelected;
                playerDeck.Remove(thisCard);
                playerPanel.Controls.Remove(thisCard);
                UpdatePlayerPanelOrder();
                if (playerDeck.Count() == 0)
                    if (LastCardRemoved != null)
                        LastCardRemoved(this, new EventArgs());
            }
            else
            {
                throw new Exception("Card not found");
            }
        }

        public Deck Deck
        {
            get
            {
                return playerDeck;
            }
        }

        public void UpdatePlayerPanelOrder()
        {
            foreach (PictureCard thisCard in playerDeck)
            {
                thisCard.cardNumber = playerDeck.IndexOf(thisCard);
            }
        }

        public void sortHand()
        {
            playerDeck.Sort();
        }

        #endregion

        #region Constructors

        public Player()
        {
        }

        public Player(String newName)
        {
            playerName = newName;
        }

        public Player(String newName, PlayerType newPlayerType)
        {
            playerName = newName;
            thisPlayerType = newPlayerType;
        }

        #endregion

        #region Events

        private void ThisPlayerCardSelected(object sender, CardEventArgs e)
        {
        }

        #endregion
    }
}
