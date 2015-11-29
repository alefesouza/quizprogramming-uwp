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
    public sealed partial class AddCategoria : Page
    {
        SQLiteConnection objConn = new SQLiteConnection("quiz.db");

        public AddCategoria()
        {
            this.InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if(QuizTitle.Text.Equals(""))
            {
                ShowMessage("Digite a categoria do quiz");
            }
            else
            {
                string QuestionPhrase = @"INSERT INTO categories (category) VALUES ('" + QuizTitle.Text + "')";
                var category = objConn.Prepare(QuestionPhrase);
                category.Step();

                this.Frame.Navigate(typeof(AddQuestions), QuizTitle.Text);
            }
        }
        
        public async void ShowMessage(string message)
        {
            MessageDialog md = new MessageDialog(message);
            await md.ShowAsync();
        }
    }
}
