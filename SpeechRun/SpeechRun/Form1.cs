using System;
using System.Windows.Forms;
using Microsoft.Speech.Recognition;

namespace SpeechRuntime
{
    public partial class launchForm : Form
    {
        static RichTextBox res;

        public launchForm()
        {
            InitializeComponent();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void launchForm_Load(object sender, EventArgs e)
        {
            res = resultText;

            try
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-ru");
                SpeechRecognitionEngine sre = new SpeechRecognitionEngine(ci);
                sre.SetInputToDefaultAudioDevice();

                sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Accepted);


                Choices numbers = new Choices();
                numbers.Add(new string[] { "выйти", "найди", "записать", "стоп", "помоги" });


                GrammarBuilder gb = new GrammarBuilder();
                gb.Culture = ci;
                gb.Append(numbers);


                Grammar g = new Grammar(gb);
                sre.LoadGrammar(g);
                gb.Culture = sre.RecognizerInfo.Culture;

                sre.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch(Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex) + 
                    "\n\n   Программа будет работать не корректно, пока не будет исправлена ошибка!", "Error!");
            }
        }

        static void Accepted(object sender, SpeechRecognizedEventArgs e)
        {
            try
            {
                Commands cmd = new Commands();

                if (e.Result.Confidence > 0.87)
                {
                    res.Text = e.Result.Text;

                    switch (res.Text)
                    {
                        case "выйти":
                            Application.Exit();
                            break;
                        case "записать":
                            cmd.WriteTxt();
                            break;
                        case "":

                            break;
                    }
                }
                else res.Text = "Не верно произнесенная или неопознанная команда";
            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex) + 
                    "\n\n   Программа будет работать не корректно, пока не будет исправлена ошибка!", "Error!");
            }
        }
    }
}
