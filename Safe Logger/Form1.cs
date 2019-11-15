using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Safe_Logger
{
    public partial class Form1 : Form
    {
        public Form1()
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

            string license = licenseInput.Text.Trim();

            if (string.IsNullOrEmpty(license))
            {
                Utils.log("Please enter a valid license key!");
                licenseInput.Text = "";
                return;
            }

            if (LicenseChecker.getInstance.exists(license))
            {
                Main m = new Main();
                m.Show();
                this.Hide();
            }
            else
            {
                Utils.log("Please enter a valid license key!");
                licenseInput.Text = "";
            }
        }

        string checkBoxS = "is set to ";
        string trueS = "true";
        string falseS = "false";

        private void Form1_Load(object sender, EventArgs e)
        {
            licenseInput.UseSystemPasswordChar = true;
            LicenseChecker.init();
            LanguageManager.init();
            Voice.init();

            if (Utils.initFirstTime())
            {
                //Program opened for first time
            }

            string[] ui = LanguageManager.getInstance.updateLanguage(1, LanguageManager.getInstance.loadLanguage());
            label2.Text = ui[0];
            button2.Text = ui[1];
            button1.Text = ui[2];
            checkBox1.Text = ui[3];
            checkBoxS = ui[4];
            trueS = ui[5];
            falseS = ui[6];
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Voice.speak(label2.Text);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Voice.speak(checkBox1.Text);
            int i = 0;
            if (checkBox1.Checked) i = 1;
            string g = "";
            if (i == 1) g = trueS; else g = falseS;
            Voice.speak(checkBoxS + g);
        }
    }
}
