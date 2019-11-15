using System;
using System.Speech.Synthesis;
using System.Threading;
using System.Timers;

namespace Safe_Logger
{
    class Voice
    {
        static SpeechSynthesizer synth;

        public static bool useVoice = true;

        public static void init()
        {
            synth = new SpeechSynthesizer();
            synth.Volume = 100;
            synth.Rate = -1;
        }

        static bool speaking = false;

        public static void speak(string txt)
        {
            if (!useVoice)
                return;

            if (speaking)
                return;

            speaking = true;
            synth.Speak(txt);

            System.Timers.Timer t = new System.Timers.Timer();
            t.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            t.Interval = 5000;
            t.Enabled = true;
            t = null;
        }

        static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            speaking = false;
        }
    }
}
