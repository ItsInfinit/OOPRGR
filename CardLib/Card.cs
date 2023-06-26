using System;


namespace CardLib
{
    public class Card : ICloneable
    {
        #region Class level variables/declarations

        public Suit suit;
        public Rank rank;
        public Face face;
        private bool rotated = false;

        public static Suit trump;  
        public static bool useTrumps = false;
        public static bool isAceHigh = true;

        #endregion

        #region Constructors

        public Card()
        {
        }

        public Card(Suit newSuit, Rank newRank)
        {
            suit = newSuit;                                                   
            rank = newRank;                                                   
            face = Face.Down;                                                   
            fixJokers();
        }

        public Card(Suit newSuit, Rank newRank, Face newFace)
        {
            suit = newSuit;
            rank = newRank;
            face = newFace;                                                     

            fixJokers();
        }

        #endregion

        #region Internal methods

        internal void fixJokers()
        {
            if (rank == Rank.Joker)
                suit = (suit == Suit.Clubs || suit == Suit.Spades) ? Suit.Black : (suit == Suit.Diamonds || suit == Suit.Hearts) ? Suit.Red : suit;
            else
                suit = (suit == Suit.Red || suit == Suit.Black) ? Suit.Spades : suit;

        }


        #endregion

        #region Public methods

        public void Flip()
        {
            if (this.face == Face.Down)
                this.face = Face.Up;
            else
                this.face = Face.Down;
        }

        public void Rotate()
        {
            Rotated = !Rotated;
        }

        public override string ToString()
        {
            return "The " + rank + " of " + suit;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

        #region Properties

        public bool Rotated
        {
            get
            {
                return rotated;
            }
            set
            {
                rotated = value;
            }
        }

        #endregion

        #region Operator overloads

        public static bool operator ==(Card card1, Card card2)
        {
            return (card1.suit == card2.suit) && (card1.rank == card2.rank);
        }

        public static bool operator !=(Card card1, Card card2)
        {
            return !(card1 == card2);
        }

        public override bool Equals(object card)
        {
            return this == (Card)card;
        }

        public static bool operator >(Card card1, Card card2)
        {
            if (card1.suit == card2.suit)
            {
                if (isAceHigh)
                {
                    if (card1.rank == Rank.Ace)
                    {
                        if (card2.rank == Rank.Ace)
                            return false;
                        else
                            return true;
                    }
                    else
                    {
                        if (card2.rank == Rank.Ace)
                            return false;
                        else
                            return (card1.rank > card2.rank);
                    }
                }
                else
                {
                    return (card1.rank > card2.rank);
                }
            }
            else
            {
                if (useTrumps && (card2.suit == Card.trump))
                    return false;
                else
                    return true;
            }
        }

        public static bool operator <(Card card1, Card card2)
        {
            return !(card1 >= card2);
        }

        public static bool operator >=(Card card1, Card card2)
        {
            if (card1.suit == card2.suit)
            {
                if (isAceHigh)
                {
                    if (card1.rank == Rank.Ace)
                    {
                        return true;
                    }
                    else
                    {
                        if (card2.rank == Rank.Ace)
                            return false;
                        else
                            return (card1.rank >= card2.rank);
                    }
                }
                else
                {
                    return (card1.rank >= card2.rank);
                }
            }
            else
            {
                if (useTrumps && (card2.suit == Card.trump))
                    return false;
                else
                    return true;
            }
        }

        public static bool operator <=(Card card1, Card card2)
        {
            return !(card1 > card2);
        }

        public override int GetHashCode()
        {
            int mySuit;
            int myRank;
            if (isAceHigh && rank == Rank.Ace)
                myRank = 14;
            else
                myRank = (int)rank;
            if (useTrumps && suit == trump)
                mySuit = (int)suit + 4;
            else
                mySuit = (int)suit;

            return 13 * mySuit + myRank;
        }

        #endregion
    }
}
