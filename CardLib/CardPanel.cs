using System;
using System.Drawing;
using System.Windows.Forms;

namespace CardLib
{
    public class CardPanel : System.Windows.Forms.Panel
    {
        public Point INITIAL_POINT = new Point(10, 25);         
        public int CARD_SPACING = 20;       
        public int HOVER_PX = 30;       
        public Point startingOffset = new Point(5, 0);     
        public bool cardsHoverable = true;

        public CardPanel()
        {
            this.Dock = DockStyle.Fill;
            this.ControlAdded += new ControlEventHandler(this.Control_Changed);
            this.ControlAdded += new ControlEventHandler(this.Control_Added);
            this.ControlRemoved += new ControlEventHandler(this.Control_Changed);
            this.ControlRemoved += new ControlEventHandler(this.Control_Removed);
        }

        private void Control_Changed(object sender, ControlEventArgs e)
        {
            UpdateControlOrder();
        }

        private void Control_Added(object sender, ControlEventArgs e)
        {
            PictureCard thisControl = (PictureCard)e.Control;

            thisControl.myPictureBox.MouseEnter -= new EventHandler(Control_Hover);
            thisControl.myPictureBox.MouseLeave -= new EventHandler(Control_Unhover);

            UpdateControlOrder();

            thisControl.BringToFront();

            if (thisControl.Hoverable && cardsHoverable)
            {
                thisControl.myPictureBox.MouseEnter += new EventHandler(Control_Hover);
                thisControl.myPictureBox.MouseLeave += new EventHandler(Control_Unhover);
            }
        }

        private void Control_Removed(object sender, System.Windows.Forms.ControlEventArgs e)
        {

        }

        public void UpdateControlOrder()
        {
            foreach (PictureCard thisControl in this.Controls)
            {
                thisControl.resting_point = new Point(startingOffset.X + CARD_SPACING * thisControl.cardNumber, startingOffset.Y + HOVER_PX);
                thisControl.hovering_point = new Point(startingOffset.X + CARD_SPACING * thisControl.cardNumber, startingOffset.Y);
                thisControl.Location = thisControl.resting_point;

                if (thisControl.Rotated)
                    thisControl.Location = new System.Drawing.Point(thisControl.Location.X - 45, thisControl.Location.Y + 45);
            }
        }

        public void UpdateControlZOrder()
        {
            foreach (PictureCard thisControl in Controls)
            {
                thisControl.Name = "Card" + thisControl.cardNumber.ToString();
            }
            for (int index = 0; index < Controls.Count; index++)
            {
                Controls[Controls.IndexOfKey("Card" + index)].BringToFront();
            }
        }

        private void Control_Hover(object sender, EventArgs e)
        {
            Control myControl = (Control)((Control)sender).Parent;
            PictureCard myPC = (PictureCard)myControl;
            if (myPC.Hoverable)
                myControl.Location = myPC.hovering_point;
        }

        private void Control_Unhover(object sender, EventArgs e)
        {
            Control myControl = (Control)((Control)sender).Parent;
            PictureCard myPC = (PictureCard)myControl;
            if (myPC.Hoverable)
                myControl.Location = myPC.resting_point;
        }

        public void manuallyRaiseCard(PictureCard thisCard)
        {
            thisCard.Location = thisCard.hovering_point;
        }
    }
}
