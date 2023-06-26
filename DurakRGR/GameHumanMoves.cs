using CardLib;
using System;

namespace DurakRGR
{
    public partial class Game
    {
        void playerCardClicked(object sender, CardEventArgs e)
        {
            if (isHumanTurn)
            {
                Log.Write("You selected " + e.CardSelected.ToString() + ".", false);

                selectedCard = e.CardSelected;

                OnResumeLoop(EventArgs.Empty);
            }
        }

        protected virtual void OnResumeLoop(EventArgs e)
        {
            if (ResumeLoop != null)
            {
                isHumanTurn = false;
                btnContextual.Enabled = false;
                btnContextual.Text = "...";
                btnContextual.Click -= btnContextual_PassOrTake;

                ResumeLoop(this, e);
            }
        }

        private void btnContextual_PassOrTake(object sender, EventArgs e)
        {
            if (isHumanTurn)
            {
                if (attackingPlayer.IsHuman)
                {
                    humanWantsToTakeOrPass = true;
                    Log.Write("Human wants to pass", false);
                }
                else if (defendingPlayer.IsHuman)
                {
                    humanWantsToTakeOrPass = true;
                    Log.Write("Human wants to take", false);
                }
                else
                {
                    throw new NotImplementedException();
                }

                OnResumeLoop(EventArgs.Empty);
            }
        }

    }
}
