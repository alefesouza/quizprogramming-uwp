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
    public sealed partial class AddQuestions : Page
    {
        SQLiteConnection objConn = new SQLiteConnection("quiz.db");
        int categoryid = 0;
        string categoryname;

        public AddQuestions()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            categoryname = e.Parameter.ToString();

            CBTitle.Text = categoryname + " - Adicionar quiz - Quiz {}";

            string getid = @"SELECT * FROM categories";
            var id = objConn.Prepare(getid);

            while (id.Step() == SQLiteResult.ROW)
            {
                categoryid = int.Parse(id[0].ToString());
            }
        }

        private void AddAlternative_Click(object sender, RoutedEventArgs e)
        {
            if (QuestionTitle.Text.Equals(""))
            {
                ShowMessage("Digite a pergunta");
            }
            else
            {
                string QuestionPhrase = @"INSERT INTO questions (categoryid, question) VALUES (" + categoryid + "," + "'" + QuestionTitle.Text + "')";
                var category = objConn.Prepare(QuestionPhrase);
                category.Step();

                string[] extra = { categoryname, QuestionTitle.Text };

                this.Frame.Navigate(typeof(AddAlternative), extra);
            }
        }

        public async void ShowMessage(string message)
        {
            MessageDialog md = new MessageDialog(message);
            await md.ShowAsync();
        }
    }
}
