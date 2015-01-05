using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DrawNotePad.Resources;
using DrawNotePad.ViewModel;
using DrawNotePad.Models.DB;
using DrawNotePad.Commons;

namespace DrawNotePad
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();
        }

        // Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            //button of add new note 
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Img/add.png", UriKind.Relative));
            appBarButton.Click += appBarButton_Click_add;
            appBarButton.Text = AppResources.AppBarButtonText;
            ApplicationBar.Buttons.Add(appBarButton);
            
            //setting button
            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.SettingTitle);
            appBarMenuItem.Click+=appBarMenuItem_Click_setting;
            ApplicationBar.MenuItems.Add(appBarMenuItem);
        }

         void appBarButton_Click_add(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddEditNote.xaml", UriKind.Relative));
        }

        //页面导航到Setting页面
         void appBarMenuItem_Click_setting(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Setting.xaml", UriKind.Relative));

        }

         protected override void OnNavigatedTo(NavigationEventArgs e)
         {
             MainViewModel main = new MainViewModel();
             base.DataContext = main;
             base.OnNavigatedTo(e);
         }

         //select one note then navigate to NotePreview page and pass parameters
         private void PostItControl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
         {
             Note note = (sender as StackPanel).DataContext as Note;
             NavigationHelper.NavigateExt(base.NavigationService, "/AddEditNote.xaml", "id", note.Id);
         }

         private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
         {
             (base.DataContext as MainViewModel).Read(this.GetPreferiti(), (sender as TextBox).Text);
         }

         private bool GetPreferiti()
         {
             return (this.pivot.SelectedIndex == 1);
         }
    }
}