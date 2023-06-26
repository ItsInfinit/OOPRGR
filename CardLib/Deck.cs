using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace CardLib
{
    public class Deck : Collection<PictureCard>
    {
        public void Shuffle()
        {
            Deck newDeck = new Deck();
            Random rnd = new Random();

            while (this.Count() > 0)
            {
                newDeck.Add(this[rnd.Next(this.Count())]);
                this.Remove(newDeck.Last());
            }

            newDeck.ToList().ForEach(Add);
        }

        public void Sort()
        {
            ((List<PictureCard>)Items).Sort();
            foreach (PictureCard thisCard in this)
            {
                thisCard.cardNumber = this.IndexOf(thisCard);
            }

        }


        public void AddNewDeck(DeckSize deckSizeSelection)
        {
            int count = 0;                                                                                                             

            for (int suitVal = 0; suitVal < 4; suitVal++)                                                                         
            {
                for (int rankVal = 1; rankVal < 14; rankVal++)                                                                    
                {
                    Card tempCard = new Card();                                                                                   

                    if (deckSizeSelection == DeckSize.Durak20Deck)                                                                     
                    {
                        if (rankVal > 9 || rankVal < 2)                                                                                
                        {
                            Add(new PictureCard((Suit)suitVal, (Rank)rankVal, Face.Down));                                         
                            count++;                                                                                              
                        }
                    }
                    else if (deckSizeSelection == DeckSize.Durak36Deck)                                                                  
                    {
                        if (rankVal < 2 || rankVal > 5)                                                                             
                        {
                            Add(new PictureCard((Suit)suitVal, (Rank)rankVal, Face.Down));                                         
                            count++;                                                                                              
                        }
                    }
                    else if (deckSizeSelection == DeckSize.RegularDeck || deckSizeSelection == DeckSize.RegularDeckWithJokers)        
                    {
                        Add(new PictureCard((Suit)suitVal, (Rank)rankVal, Face.Down));                                             
                        count++;                                                                                                  
                    }
                }
            }

            if (deckSizeSelection == DeckSize.RegularDeckWithJokers)                                                                  
            {
                Add(new PictureCard(Suit.Black, Rank.Joker, Face.Down));
                Add(new PictureCard(Suit.Red, Rank.Joker, Face.Down));
            }
        }

        public void FlipAllCards()
        {
            foreach (PictureCard thisCard in this)
            {
                thisCard.Flip();
            }

        }

        public void RedisplayCards()
        {
            foreach (PictureCard thisCard in this)
            {
                thisCard.UpdateImage();
            }
        }

        public PictureCard this[PictureCard targetCard]
        {
            get
            {

                int targetHashCode = targetCard.GetHashCode();
                foreach (PictureCard eachCard in this)
                {
                    if (eachCard.GetHashCode() == targetHashCode)
                    {
                        return eachCard;
                    }
                }
                throw new KeyNotFoundException("Card not found: " + targetCard.ToString());
            }
            set
            {
                try
                {
                    int oldCardIndex = this.IndexOf(this[targetCard]);
                    this.RemoveAt(oldCardIndex);
                    this.Insert(oldCardIndex, value);
                }
                catch (KeyNotFoundException ex)
                {
                    Debug.WriteLine("Card not found in deck: " + ex.Message);
                }
            }
        }
    }
}
