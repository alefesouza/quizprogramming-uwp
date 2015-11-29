using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLitePCL;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.System;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Quiz
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => { HomeViewModel = DataContext as ViewModels.HomeViewModel; };
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        public ViewModels.HomeViewModel HomeViewModel { get; set; }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddCategoria));
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private void ListCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListCategories.SelectedIndex != -1)
            {
                Models.Home a = e.AddedItems[0] as Models.Home;
                this.Frame.Navigate(typeof(QuizPage), a.Id);
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

                ListCategories.SelectedIndex = -1;
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            ShowMessage("Aplicativo desenvolvido por Alefe Souza");
        }

        public async void ShowMessage(string message)
        {
            MessageDialog md = new MessageDialog(message);
            md.Commands.Add(new UICommand("Meu GitHub", new UICommandInvokedHandler(CommandHandlers)) { Id = 0 });
            md.Commands.Add(new UICommand("Fechar") { Id = 1 });

            md.DefaultCommandIndex = 0;
            md.CancelCommandIndex = 1;
            await md.ShowAsync();
        }

        public void CommandHandlers(IUICommand commandLabel)
        {
            var Actions = commandLabel.Label;
            switch (Actions)
            {
                case "Meu GitHub":
                    openBrowser();
                    break;
            }
        }

        public async void openBrowser()
        {
            await Launcher.LaunchUriAsync(new Uri("http://github.com/alefesouza"));
        }
    }
}
