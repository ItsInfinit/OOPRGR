using System;
using System.Diagnostics;
using System.IO;

namespace DurakRGR
{
    class Log
    {
        #region Functions

        public static bool Write(string message, bool isPublic)
        {
            try
            {
                DateTime timestamp = DateTime.Now;

                string output = Convert.ToString(timestamp) + ": " + message + "\r\n";

                File.AppendAllText(Program.optionLogPath + @"\G4Durak.log", output);

                Debug.WriteLine(output);

                if (isPublic)
                    Game.userMessageLog += message + "\r\n";

                return true;
            }
            catch (Exception ex)
            {
                Console.Beep();

                System.Windows.Forms.MessageBox.Show("ERROR: Unable to log,\"" + message + "\".\n\n" + ex);

                return false;
            }
        }
        public static bool Write(string message)
        {
            return Write(message, false);
        }

        #endregion
    }
}
