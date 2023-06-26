using CardLib;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;


namespace DurakRGR
{
    public partial class Game
    {
        #region Class Variables

        private const int PLAYER_COUNT_DEFAULT = 6;
        private const int PLAYER_COUNT_MAX = 6;
        private const int PLAYER_COUNT_MIN = 1;

        private int playerHandSize = 6;
        public Deck gameDeck = new Deck();
        public Player gameDeckPlayer = new Player("Fake player (Deck)", PlayerType.None);
        public Player gameTablePlayer = new Player("Fake player (Table)", PlayerType.None);
        public Player discardPilePlayer = new Player();

        public readonly string[] DEFAULT_PLAYER_NAMES = { "Player 1 (Human)", "Player 2 (AI)", "Player 3 (AI)", "Player 4 (AI)", "Player 5 (AI)", "Player 6 (AI)" };
        public PictureBox pbTrumpImage;
        public static String userMessageLog;
        private Random rng = new Random();
        private int dealer;
        private int attacker;
        private int nextAttacker;
        private int gameActionOrder = 0;
        public event EventHandler ResumeLoop;
        internal bool gameStarted = true;
        internal bool isHumanTurn = false;
        private PictureCard selectedCard;
        private PictureCard attackingCard;
        internal Button btnContextual;
        private bool humanWantsToTakeOrPass = false;
        internal string endOfGameMessage = "";

        private DeckSize deckSize;

        private Collection<Player> playersInGame = new Collection<Player>();

        #endregion

        #region Constructors

        public Game()
        {
        }

        public Game(int numberOfPlayers)
        {
            AddPlayersToGame(numberOfPlayers);
        }

        #endregion


        public void startGame()
        {
            Log.Write("New game started. Setting up.", false);

            deckSize = Program.optionGameDeckSize;
            if (Program.optionGameDeckSize == DeckSize.Durak20Deck && playersInGame.Count() > 2)
                playerHandSize = 20 / playersInGame.Count();

            SetupNewGameDeck();

            Log.Write("Game setup complete.", false);

            gameActionOrder = 0;
        }

        private void AddPlayersToGame(int numberOfPlayers)
        {
            if (numberOfPlayers < 2)
                throw new Exception("Must have at least two players in the game");
            else if (numberOfPlayers > 6)
                throw new Exception("This game cannot support more than six players.");

            playersInGame.Add(new Player(Program.noHumanPlayer ? "Player 1 (AI)" : DEFAULT_PLAYER_NAMES[0], Program.noHumanPlayer ? PlayerType.AI : PlayerType.Human));
            playersInGame.Add(new Player(DEFAULT_PLAYER_NAMES[1], PlayerType.AI));

            Player.ALL_CARDS_FACE_UP = Program.noHumanPlayer;

            if (numberOfPlayers >= 3)
                playersInGame.Add(new Player(DEFAULT_PLAYER_NAMES[2], PlayerType.AI));
            if (numberOfPlayers >= 4)
                playersInGame.Add(new Player(DEFAULT_PLAYER_NAMES[3], PlayerType.AI));
            if (numberOfPlayers >= 5)
                playersInGame.Add(new Player(DEFAULT_PLAYER_NAMES[4], PlayerType.AI));
            if (numberOfPlayers == 6)
                playersInGame.Add(new Player(DEFAULT_PLAYER_NAMES[5], PlayerType.AI));

            foreach (Player thisPlayer in playersInGame)
            {
                thisPlayer.originalIndex = playersInGame.IndexOf(thisPlayer);
            }
        }

        private void SetupNewGameDeck()
        {
            gameDeck = new Deck();
            gameDeck.AddNewDeck(deckSize);

            gameDeck.Shuffle();

            gameDeckPlayer.playerPanel.CARD_SPACING = 0;

            gameDeckPlayer.playerPanel.cardsHoverable = false;

            gameDeckPlayer.playerPanel.startingOffset = new System.Drawing.Point(45, 5);

            foreach (PictureCard thisCard in gameDeck)
                gameDeckPlayer.Add(thisCard);

            gameDeckPlayer.LastCardRemoved += LastCardRemovedFromDeckHandler;

            Card.trump = gameDeckPlayer.Deck.First().ToCard().suit;
            Card.useTrumps = true;

            gameDeckPlayer.Deck.First().Face = Face.Up;

            gameDeckPlayer.Deck.First().Rotate();
        }

    }
}
