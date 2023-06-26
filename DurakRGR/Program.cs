using CardLib;
using System;
using System.Windows.Forms;

namespace DurakRGR
{
    static class Program
    {
        public static bool startAgain = false;
        private const bool SHOW_OPTIONS_ON_FIRST_START = true;
        private const bool SAVE_AND_LOAD_USER_SETTINGS = true;

        public static int optionGamePlayers = 2;
        public static DeckSize optionGameDeckSize = DeckSize.Durak36Deck;
        public static int optionSelectedBack = 1;
        public static string optionLogPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static bool noHumanPlayer = false;
        public static int lastFool = -1;
        internal static frmMainForm mainFormInstance;

        [STAThread]
        static void Main()
        {
            if (SAVE_AND_LOAD_USER_SETTINGS)
            {
                if (System.IO.Directory.Exists(DurakRGR.Properties.Settings.Default.LogPath))
                    optionLogPath = DurakRGR.Properties.Settings.Default.LogPath;
                optionGamePlayers = DurakRGR.Properties.Settings.Default.GamePlayers;
                optionGameDeckSize = DurakRGR.Properties.Settings.Default.GameDeckSize;
                optionSelectedBack = DurakRGR.Properties.Settings.Default.SelectedCardBack;
            }
            else
            {
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainFormInstance = new frmMainForm(SHOW_OPTIONS_ON_FIRST_START);
            Application.Run(mainFormInstance);
            while (startAgain)
            {
                mainFormInstance = new frmMainForm();
                startAgain = false;
                Application.Run(mainFormInstance);
            }

            DurakRGR.Properties.Settings.Default.LogPath = optionLogPath;
            DurakRGR.Properties.Settings.Default.GamePlayers = optionGamePlayers;
            DurakRGR.Properties.Settings.Default.GameDeckSize = optionGameDeckSize;
            DurakRGR.Properties.Settings.Default.SelectedCardBack = optionSelectedBack;
            if (SAVE_AND_LOAD_USER_SETTINGS)
                DurakRGR.Properties.Settings.Default.Save();
        }
    }
}
