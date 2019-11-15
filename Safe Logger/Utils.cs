using System;
using System.IO;
using System.Windows.Forms;

namespace Safe_Logger
{
    class Utils
    {
        public static void exit()
        {
            Environment.Exit(0);
        }

        public static void log(string txt)
        {
            MessageBox.Show(txt, "Safe Logger");
            Voice.speak(txt);
        }

        public static string license = "";

        public static bool initFirstTime()
        {
            bool f = false;

            if (!System.IO.File.Exists(Directory.GetCurrentDirectory() + "/options/1.txt"))
            {
                f = true;
                var file = System.IO.File.Create(Directory.GetCurrentDirectory() + "/options/1.txt");
                file.Close();
            }

            return f;
        }
    }
}
