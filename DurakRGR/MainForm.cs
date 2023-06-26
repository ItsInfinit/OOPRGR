using CardLib;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DurakRGR
{
    public partial class frmMainForm : Form
    {
        public Game game = new Game();
        private bool firstTimeLoad = false;

        public frmMainForm()
        {
            InitializeComponent();
        }

        public frmMainForm(bool isFirstTimeLoad)
        {
            firstTimeLoad = isFirstTimeLoad;
            InitializeComponent();
        }

        #region Events

        private void frmMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Write("Main form closed.", false);
        }

        private void PlayForm_Load(object sender, EventArgs e)
        {
            if (firstTimeLoad)
                btnOptions_Click(this, EventArgs.Empty);

            game = new Game(Program.optionGamePlayers);

            game.pbTrumpImage = picTrump;
            picTrump.Visible = false;

            game.startGame();
            initFormControls(Program.optionGamePlayers);

            this.Activate();

            Log.Write("Main form loaded.", false);

        }

        #endregion

        #region Functions

        private void mainLoop(object sender, EventArgs e)
        {
            txtGameMessages.Text = Game.userMessageLog;
            lblAttackOrDefend.Text = "";

            while (game.isHumanTurn == false && game.gameStarted)
            {
                game.Go();

                txtGameMessages.Text = Game.userMessageLog;
                txtGameMessages.SelectionStart = txtGameMessages.Text.Length;
                txtGameMessages.ScrollToCaret();
            }

            if (game.gameStarted == false)
            {
                MessageBox.Show("Game over!\r\n\r\n" + game.endOfGameMessage, "Game Over");
                btnNewGame.Visible = true;
            }
            else
            {
                if (game.attackingPlayer.IsHuman)
                    lblAttackOrDefend.Text = "Attacking";
                else if (game.defendingPlayer.IsHuman)
                    lblAttackOrDefend.Text = "Defending";
                else
                    lblAttackOrDefend.Text = "";

                Log.Write("Broke out of game loop. Waiting for user action.", false);
            }
        }

        public void initFormControls(int numberOfPlayers)
        {
            if (numberOfPlayers < 2)
                throw new Exception("Must have at least two players in the game");

            grpDeck.Controls.Add(game.gameDeckPlayer.playerPanel);

            grpPlayField.Controls.Add(game.gameTablePlayer.playerPanel);

            grpPlayer1.Controls.Add(game.players[0].playerPanel);
            grpPlayer2.Controls.Add(game.players[1].playerPanel);

            if (numberOfPlayers >= 3)
                grpPlayer3.Controls.Add(game.players[2].playerPanel);
            if (numberOfPlayers >= 4)
                grpPlayer4.Controls.Add(game.players[3].playerPanel);
            if (numberOfPlayers >= 5)
                grpPlayer5.Controls.Add(game.players[4].playerPanel);
            if (numberOfPlayers == 6)
                grpPlayer6.Controls.Add(game.players[5].playerPanel);
            if (numberOfPlayers > 6)
                throw new Exception("This game cannot support more than six players.");

            if (Card.trump == Suit.Black || Card.trump == Suit.Clubs)
                picTrump.Image = (Image)Properties.Resources.club;
            else if (Card.trump == Suit.Spades)
                picTrump.Image = (Image)Properties.Resources.spade;
            else if (Card.trump == Suit.Red || Card.trump == Suit.Hearts)
                picTrump.Image = (Image)Properties.Resources.heart;
            else if (Card.trump == Suit.Diamonds)
                picTrump.Image = (Image)Properties.Resources.diamond;

            game.btnContextual = btnContextual;

            game.ResumeLoop += mainLoop;
        }

        #endregion

        private void frmMainForm_Shown(object sender, EventArgs e)
        {
            mainLoop(sender, e);
        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            frmOptions formOptions = new frmOptions();

            formOptions.ShowDialog();

            formOptions.Dispose();
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            Program.startAgain = true;
            this.Close();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }


    }
}
