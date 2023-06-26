using System;
using System.Drawing;
using System.Windows.Forms;

namespace CardLib
{
    public partial class PictureCard : UserControl, IEquatable<PictureCard>, IComparable<PictureCard>
    {
        #region Class level variables/declarations

        private Card card;
        public PictureBox myPictureBox = new PictureBox();
        public static int backgroundSelection = 1;

        public Size originalCardSize = new Size(100, 145);

        private bool hoverable = true;

        public int cardNumber;

        public Point resting_point;
        public Point hovering_point;

        public Player myPlayer;

        #endregion

        #region Constructors

        public PictureCard()
        {
            InitThis();
            InitializeComponent();
            card = new Card(Suit.Spades, Rank.Ace, Face.Down);
        }

        public PictureCard(Suit newSuit, Rank newRank, Face newFace)
        {
            InitThis();
            InitializeComponent();
            card = new Card(newSuit, newRank, newFace);
        }

        public PictureCard(Card newCard)
        {
            InitThis();
            InitializeComponent();
            card = newCard;
        }

        #endregion

        #region Properties

        public Suit Suit
        {
            get
            {
                return card.suit;
            }
            set
            {
                if (value != card.suit)
                {
                    card.suit = value;
                    UpdateImage();
                }
            }
        }

        public Rank Rank
        {
            get
            {
                return card.rank;
            }
            set
            {
                if (value != card.rank)
                {
                    card.rank = value;
                    UpdateImage();
                }
            }
        }

        public Face Face
        {
            get
            {
                return card.face;
            }
            set
            {
                if (value != card.face)
                {
                    card.face = value;
                    UpdateImage();
                }
            }
        }

        public bool Rotated
        {
            get
            {
                return card.Rotated;
            }

            set
            {
                if (value != card.Rotated)
                {
                    card.Rotated = value;
                    UpdateImage();
                }
            }
        }

        public bool Hoverable
        {
            get
            {
                return hoverable;
            }
            set
            {
                hoverable = value;
            }
        }

        private string filename
        {
            get
            {
                String filenameBuilder = card.suit.ToString().ToLower() + "_";
                if (card.face == Face.Down)
                {
                    filenameBuilder = "card_back_" + backgroundSelection.ToString();
                    if (Properties.Resources.ResourceManager.GetObject(filenameBuilder).Equals(null))
                        filenameBuilder = "card_back_1";  
                }
                else if (1 < (int)card.rank && (int)card.rank <= 10)
                    filenameBuilder += (int)card.rank;
                else
                    filenameBuilder += card.rank.ToString().ToLower();

                return filenameBuilder;
            }
        }

        private Bitmap myBitmap
        {
            get
            {
                try
                {
                    Bitmap thisBitmap = new Bitmap((Image)Properties.Resources.ResourceManager.GetObject(filename));

                    if (card.Rotated)
                        thisBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);

                    return thisBitmap;

                }
                catch (NullReferenceException)
                {
                    throw new Exception("Card file not found. Check references.");
                }
            }
        }

        #endregion

        #region Private methods

        private void InitThis()
        {
            myPictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            myPictureBox.Click += PictureCard_Click;
        }

        internal void UpdateImage()
        {
            card.fixJokers();

            myPictureBox.Image = myBitmap;

            if (Rotated)
                myPictureBox.Size = new Size(originalCardSize.Height, originalCardSize.Width);
            else
                myPictureBox.Size = new Size(originalCardSize.Width, originalCardSize.Height);

            Size = new Size(myPictureBox.Size.Width, myPictureBox.Size.Height);
        }

        #endregion

        #region Public Methods

        public void Flip()
        {
            card.Flip();
            UpdateImage();
            if (CardFlipped != null)
                CardFlipped(this, new CardEventArgs(this));

        }

        public void Rotate()
        {
            Rotated = !Rotated;
        }

        public override string ToString()
        {
            return card.ToString();
        }

        public Card ToCard()
        {
            return card;
        }

        #endregion

        #region Events

        public event EventHandler<CardEventArgs> CardSelected;
        public event EventHandler<CardEventArgs> CardFlipped;

        void PictureCard_Click(object sender, EventArgs e)
        {
            if (CardSelected != null)
                CardSelected(this, new CardEventArgs(this));
        }


        private void PictureCard_Load(object sender, EventArgs e)
        {

            this.Controls.Add(myPictureBox);

            UpdateImage();
        }

        #endregion

        #region Operators

        public static explicit operator Card(PictureCard pictureCard)
        {
            return pictureCard.ToCard();
        }

        public static explicit operator PictureCard(Card regularCard)
        {
            return new PictureCard(regularCard);
        }

        public bool Equals(PictureCard other)
        {
            if (other == null) return false;
            return (this.card.rank.Equals(other.card.rank) && this.card.suit.Equals(other.card.suit));
        }

        public override bool Equals(Object o)
        {
            return (o.GetHashCode() == this.GetHashCode());
        }

        public static bool operator ==(PictureCard card1, PictureCard card2)
        {
            if ((object)card1 == null || ((object)card2) == null)
                return Object.Equals(card1, card2);

            return card1.Equals(card2);
        }

        public static bool operator !=(PictureCard card1, PictureCard card2)
        {
            return !card1.Equals(card2);
        }

        public override int GetHashCode()
        {
            return ((Card)this).GetHashCode();
        }

        public int CompareTo(PictureCard compareCard)
        {
            if (compareCard == null)
                return 1;
            else
                return this.GetHashCode().CompareTo(compareCard.GetHashCode());
        }


        #endregion
    }
}
