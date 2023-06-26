using System;
using System.Linq;

namespace DurakRGR
{
    public partial class Game
    {

        public void Go()
        {
            Log.Write("Game action order " + gameActionOrder.ToString());

            switch (gameActionOrder)
            {
                case 0:
                    Log.Write("GA0: Variable init", false);

                    gameActionOrder = 1;
                    break;

                case 1:
                    Log.Write("GA1: Game setup", false);

                    if (Program.lastFool == -1 || Program.lastFool > playersInGame.Count() - 1)
                        dealer = rng.Next(playersInGame.Count());
                    else
                        dealer = Program.lastFool;

                    Log.Write(playersInGame[dealer].Name + " is dealing (" + (Program.lastFool == -1 ? "no fool last game" : "fool from last game") + ").", true);

                    attacker = nextPlayer(dealer);
                    nextAttacker = -1;

                    dealPlayerHands();

                    gameActionOrder = 2;

                    break;
                case 2:
                    Log.Write("GA2: Start of turn", false);

                    humanWantsToTakeOrPass = false;

                    if (gameDeckPlayer.Deck.Count() > 0)
                    {
                        Log.Write("Deck has " + gameDeckPlayer.Deck.Count().ToString() + " cards. Topping up player hands.", false);

                        topUpPlayerHands();
                    }
                    else
                    {
                        Log.Write("Deck is empty!", false);

                        for (int playerIndex = playersInGame.Count(); playerIndex > 0; playerIndex--)
                        {
                            if (playersInGame[playerIndex - 1].Deck.Count() == 0)
                            {
                                Log.Write(playersInGame[playerIndex - 1].Name + " ran out of cards and exits the game!", true);
                                playersInGame.RemoveAt(playerIndex - 1);

                                if (playersInGame.Count() == 0)
                                {
                                    gameActionOrder = 6;
                                    break;
                                }

                                if (playerIndex - 1 < nextAttacker)
                                    nextAttacker--;
                                else if (playerIndex - 1 == nextAttacker)
                                    nextAttacker = nextPlayer(nextAttacker);
                            }
                        }

                        if (playersInGame.Count() <= 1)
                        {
                            gameActionOrder = 6;
                            break;
                        }
                    }

                    if (nextAttacker != -1)
                        attacker = nextAttacker;

                    gameActionOrder = 3;
                    break;
                case 3:
                    Log.Write("GA3: Attacker's first move", false);

                    if (selectedCard == null)
                    {
                        if (attackingPlayer.IsHuman)
                        {
                            isHumanTurn = true;
                            Log.Write("Make your move, human.", true);
                        }
                        else
                        {
                            selectedCard = getAIAttackCardFromHand(attackingPlayer.Deck);

                            if (selectedCard == null)
                            {
                                if (attackingPlayer.Deck.Count() == 0)
                                    Log.Write("AI is out of cards! This should be checked for earlier..", true);
                                else
                                    throw new NotImplementedException();
                            }
                        }

                        break;
                    }
                    else
                    {
                        Log.Write(attackingPlayer.Name + " attacks " + defendingPlayer.Name + " with " + selectedCard.ToString() + ".", true);

                        moveCardToTable(selectedCard);
                        attackingCard = selectedCard;

                        selectedCard = null;

                        gameActionOrder = 4;
                    }

                    break;
                case 4:
                    Log.Write("GA4: Defender's move", false);

                    if (selectedCard == null)
                    {
                        if (defendingPlayer.IsHuman)
                        {
                            if (humanWantsToTakeOrPass)
                            {
                                Log.Write(defendingPlayer.Name + " takes " + gameTablePlayer.Deck.Count() + " card" + (gameTablePlayer.Deck.Count() == 1 ? "" : "s") + "!", true);
                                giveAllCardsOnTableToPlayer(defendingPlayer);

                                nextAttacker = nextPlayer(defender);

                                gameActionOrder = 2;
                                break;
                            }
                            else
                            {
                                Log.Write("Human " + playersInGame[defender].Name + " is defending against " + attackingCard.ToString(), false);
                                Log.Write("Defend, human. Play a card or take.", false);

                                setContextButtonToTake();

                                isHumanTurn = true;
                                break;
                            }
                        }
                        else if (defendingPlayer.IsAI)
                        {
                            Log.Write("AI " + defendingPlayer.Name + " is defending against " + attackingCard.ToString(), false);

                            selectedCard = getAIDefenseCardFromHand(attackingCard, defendingPlayer.Deck);

                            if (selectedCard == null)
                            {
                                Log.Write(defendingPlayer.Name + " takes " + gameTablePlayer.Deck.Count() + " card" + (gameTablePlayer.Deck.Count() == 1 ? "" : "s") + "!", true);

                                giveAllCardsOnTableToPlayer(defendingPlayer);

                                nextAttacker = nextPlayer(defender);

                                gameActionOrder = 2;
                                break;
                            }

                        }
                    }
                    else
                    {
                        if (!isValidDefendingMove(attackingCard, selectedCard))
                        {
                            Log.Write(defendingPlayer.Name + " tried to make an invalid move by defending with " + selectedCard.ToString() + "! Try again!", true);

                            selectedCard = null;

                            break;
                        }
                        else
                        {
                            Log.Write(defendingPlayer.Name + " defends successfully with " + selectedCard.ToString() + "!", true);

                            moveCardToTable(selectedCard);

                            selectedCard = null;
                            gameActionOrder = 5;
                        }
                    }

                    break;
                case 5:
                    Log.Write("GA5: Attacker's continuation", false);

                    if (defendingPlayer.Deck.Count() == 0)
                    {
                        Log.Write(defendingPlayer.Name + " ran out of cards! Attack over!", true);

                        giveAllCardsOnTableToPlayer(discardPilePlayer);
                        nextAttacker = defender;

                        gameActionOrder = 2;
                        break;
                    }

                    if (selectedCard == null)
                    {
                        if (humanWantsToTakeOrPass && attackingPlayer.IsHuman)
                        {
                            Log.Write(attackingPlayer.Name + " passes their attack.", true);

                            giveAllCardsOnTableToPlayer(discardPilePlayer);

                            nextAttacker = defender;

                            gameActionOrder = 2;
                            break;
                        }
                        else if (attackingPlayer.IsHuman)
                        {
                            isHumanTurn = true;
                            setContextButtonToPass();
                            Log.Write("Make your move, human. You get to attack or pass.", true);
                            break;
                        }
                        else
                        {
                            selectedCard = getAIContinuedAttackCardFromHand(attackingPlayer.Deck);

                            if (selectedCard == null)
                            {
                                Log.Write(attackingPlayer.Name + " passes their attack.", false);

                                giveAllCardsOnTableToPlayer(discardPilePlayer);
                                nextAttacker = defender;
                                gameActionOrder = 2;
                                break;
                            }
                            else
                                break;
                        }
                    }
                    else
                    {
                        attackingCard = selectedCard;

                        if (!isValidCardForContinuedAttack(selectedCard))
                        {
                            Log.Write(attackingPlayer.Name + " tried to continue attack with " + selectedCard.ToString() + " but couldn't!", true);
                            selectedCard = null;
                            break;
                        }
                        else
                        {
                            Log.Write(attackingPlayer.Name + " continues attack with " + selectedCard.ToString(), true);

                            attackingCard = selectedCard;
                            selectedCard = null;

                            moveCardToTable(attackingCard);

                            gameActionOrder = 4;
                            break;
                        }
                    }
                case 6:
                    if (playersInGame.Count() == 0)
                    {
                        endOfGameMessage = "Tie game!";
                        Program.lastFool = -1;
                    }
                    else if (playersInGame.Count() == 1)
                    {
                        endOfGameMessage = playersInGame[0].Name + " is the fool!";
                        Program.lastFool = playersInGame[0].originalIndex;
                    }

                    Log.Write("Game over! " + endOfGameMessage, true);

                    gameStarted = false;
                    break;
                case 99:
                    Log.Write("Game stopped for unknown reasons.", false);
                    gameStarted = false;
                    break;
                case 100:
                    throw new NotImplementedException();
                default:
                    gameActionOrder++;
                    break;

            }
        }
    }
}
