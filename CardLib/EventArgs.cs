using System;

namespace CardLib
{
    public class CardEventArgs : EventArgs
    {
        private PictureCard myCard;
        public PictureCard CardSelected
        {
            get
            {
                return myCard;
            }
        }
        public CardEventArgs(PictureCard selectedCard)
        {
            myCard = selectedCard;
        }
    }
}
