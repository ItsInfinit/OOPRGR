using CardLib;

namespace DurakRGR
{
    public partial class Game
    {
        PictureCard getAIDefenseCardFromHand(PictureCard attackingCard, Deck aiDeck)
        {
            PictureCard bestCardSoFar = null;

            foreach (PictureCard thisCard in aiDeck)
            {
                if ((thisCard.Suit == attackingCard.Suit || (gameDeckEmpty && thisCard.Suit == Card.trump)) && thisCard.ToCard() > attackingCard.ToCard())
                {
                    if (bestCardSoFar == null || (thisCard.ToCard() < bestCardSoFar.ToCard()))
                        bestCardSoFar = thisCard;
                    else if (thisCard.ToCard() < bestCardSoFar.ToCard())
                        bestCardSoFar = thisCard;
                }
            }

            return bestCardSoFar;
        }

        PictureCard getAIAttackCardFromHand(Deck aiDeck)
        {
            PictureCard bestCardSoFar = null;

            foreach (PictureCard thisCard in aiDeck)
            {
                if (bestCardSoFar == null || thisCard.Suit != Card.trump && (thisCard.Rank == Rank.Ace ? Rank.Joker : thisCard.Rank) < (bestCardSoFar.Rank == Rank.Ace ? Rank.Joker : bestCardSoFar.Rank))
                    bestCardSoFar = thisCard;
            }

            if (bestCardSoFar == null)
            {
                foreach (PictureCard thisCard in aiDeck)
                {
                    if (bestCardSoFar == null || thisCard.ToCard() < bestCardSoFar.ToCard())
                        bestCardSoFar = thisCard;
                }
            }
            return bestCardSoFar;
        }

        PictureCard getAIContinuedAttackCardFromHand(Deck aiDeck)
        {
            PictureCard bestCardSoFar = null;

            foreach (PictureCard thisCard in aiDeck)
            {
                if (bestCardSoFar == null || thisCard.Suit != Card.trump && (thisCard.Rank == Rank.Ace ? Rank.Joker : thisCard.Rank) < (bestCardSoFar.Rank == Rank.Ace ? Rank.Joker : bestCardSoFar.Rank))
                    if (isValidCardForContinuedAttack(thisCard))
                        bestCardSoFar = thisCard;
            }

            if (bestCardSoFar == null && gameDeckEmpty)
            {
                foreach (PictureCard thisCard in aiDeck)
                {
                    if (bestCardSoFar == null || thisCard.ToCard() < bestCardSoFar.ToCard())
                        if (isValidCardForContinuedAttack(thisCard))
                            bestCardSoFar = thisCard;
                }
            }
            return bestCardSoFar;
        }
    }
}
