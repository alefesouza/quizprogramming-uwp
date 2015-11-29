using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Quiz
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddAlternative : Page
    {
        SQLiteConnection objConn = new SQLiteConnection("quiz.db");
        int questionid = 0;
        string categoryname;

        public AddAlternative()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string[] extra = e.Parameter as string[];
            categoryname = extra[0];

            CBTitle.Text = categoryname + " - Adicionar quiz - Quiz {}";
            Question.Text = "Pergunta: " + extra[1];

            string getid = @"SELECT * FROM questions";
            var id = objConn.Prepare(getid);

            while (id.Step() == SQLiteResult.ROW)
            {
                questionid = int.Parse(id[0].ToString());
            }
        }

        public void Add()
        {
            int iscorrect = 0;
            if (IsCorrect.IsChecked == true)
            {
                iscorrect = 1;
            }
            else
            {
                iscorrect = 0;
            }

            string alternativesql = @"INSERT INTO alternatives (questionid, alternative, iscorrect) VALUES (" + questionid + ", '" + AlternativeText.Text + "', " + iscorrect + ")";
            var alternative = objConn.Prepare(alternativesql);
            alternative.Step();
        }

        private void AddAlternativ_Click(object sender, RoutedEventArgs e)
        {
            if (AlternativeText.Text.Equals(""))
            {
                ShowMessage("Digite o texto da alternativa");
            }
            else
            {
                Add();

                AlternativeText.Text = "";
                IsCorrect.IsChecked = false;
            }
        }

        private void AddQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (AlternativeText.Text.Equals(""))
            {
                ShowMessage("Digite o texto da alternativa");
            }
            else
            {
                Add();
                this.Frame.Navigate(typeof(AddQuestions), categoryname);
            }
        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            if (AlternativeText.Text.Equals(""))
            {
                ShowMessage("Digite o texto da alternativa");
            }
            else
            {
                Add();
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        public async void ShowMessage(string message)
        {
            MessageDialog md = new MessageDialog(message);
            await md.ShowAsync();
        }
    }
}
