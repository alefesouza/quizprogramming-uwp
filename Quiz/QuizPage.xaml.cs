using NotificationsExtensions.Tiles;
using Quiz.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Notifications;
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
    public sealed partial class QuizPage : Page
    {
        SQLiteConnection objConn = new SQLiteConnection("quiz.db");
        string id;
        int questiontotal = 0, actualquestion = 1, thisquestioncorrectindex = 0;
        bool canchange = true;
        ISQLiteStatement question;

        public QuizPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs ne)
        {
            id = ne.Parameter.ToString();

            string CategoryName = @"SELECT * FROM categories WHERE id=" + id;

            var title = objConn.Prepare(CategoryName);
            title.Step();
            CBTitle.Text = title[1] + " - Quiz {}";

            string QuestionPhrase = @"SELECT * FROM questions WHERE categoryid=" + id;
            question = objConn.Prepare(QuestionPhrase);

            while (question.Step() == SQLiteResult.ROW)
            {
                questiontotal++;
            }

            question.Reset();

            ChangeQuestion();
        }

        private void ListAlternatives_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListAlternatives.SelectedIndex != -1)
            {
                if (canchange)
                {
                    Questions a = e.AddedItems[0] as Questions;
                    if (a.IsCorrect == 1)
                    {
                        CorretasCount.Text = (int.Parse(CorretasCount.Text) + 1).ToString();
                    }
                    else
                    {
                        ErradasCount.Text = (int.Parse(ErradasCount.Text) + 1).ToString();
                    }

                    canchange = false;
                    ListAlternatives.SelectedIndex = thisquestioncorrectindex;

                    Next.IsEnabled = true;

                    if (actualquestion == questiontotal)
                    {
                        Close.IsEnabled = true;
                    }
                }
                else
                {
                    ListAlternatives.SelectedIndex = thisquestioncorrectindex;
                }
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            canchange = true;
            ListAlternatives.SelectedIndex = -1;

            actualquestion++;

            ChangeQuestion();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        public void ChangeQuestion()
        {
            QuestaoCount.Text = actualquestion + "/" + questiontotal;

            question.Step();

            AlternativeText.Text = question[2].ToString();

            string strA = @"SELECT * FROM alternatives WHERE questionid=" + question[0];
            var alternative = objConn.Prepare(strA);

            ObservableCollection<Questions> Items = new ObservableCollection<Questions>();

            int i = 0;
            while (alternative.Step() == SQLiteResult.ROW)
            {
                Items.Add(new Questions(int.Parse(alternative[1].ToString()), alternative[2].ToString(), int.Parse(alternative[3].ToString())));

                if (int.Parse(alternative[3].ToString()) == 1)
                {
                    thisquestioncorrectindex = i;
                }
                i++;
            }

            Binding myBinding = new Binding();
            myBinding.Source = Items;
            ListAlternatives.SetBinding(ItemsControl.ItemsSourceProperty, myBinding);

            Next.IsEnabled = false;

            if (actualquestion == questiontotal)
            {
                Next.Visibility = Visibility.Collapsed;
                Close.Visibility = Visibility.Visible;

                ModifyTile("Quiz {}", CBTitle.Text, "Acertou " + CorretasCount.Text + "/" + questiontotal + " questões");
            }
        }

        public void ModifyTile(string from, string subject, string body)
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();

            TileContent content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileMedium = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                            {
                                new TileText()
                                {
                                    Text = from
                                },

                                new TileText()
                                {
                                    Text = subject,
                                    Style = TileTextStyle.CaptionSubtle
                                },

                                new TileText()
                                {
                                    Text = body,
                                    Style = TileTextStyle.CaptionSubtle
                                }
                            }
                        }
                    },

                    TileWide = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                            {
                                new TileText()
                                {
                                    Text = from,
                                    Style = TileTextStyle.Subtitle
                                },

                                new TileText()
                                {
                                    Text = subject,
                                    Style = TileTextStyle.CaptionSubtle
                                },

                                new TileText()
                                {
                                    Text = body,
                                    Style = TileTextStyle.CaptionSubtle
                                }
                            }
                        }
                    },

                    TileLarge = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                            {
                                new TileText()
                                {
                                    Text = from,
                                    Style = TileTextStyle.Subtitle
                                },

                                new TileText()
                                {
                                    Text = subject,
                                    Style = TileTextStyle.CaptionSubtle
                                },

                                new TileText()
                                {
                                    Text = body,
                                    Style = TileTextStyle.CaptionSubtle
                                }
                            }
                        }
                    }
                }
            };

            var notification = new TileNotification(content.GetXml());

            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
        }
    }
}
