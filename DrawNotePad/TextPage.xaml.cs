using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DrawNotePad.ViewModel;
using System.ComponentModel;
using DrawNotePad.Commons;

namespace DrawNotePad
{
    public partial class TextPage : PhoneApplicationPage
    {
        public TextPage()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Img/save.png", UriKind.Relative));
            appBarButton.Text = "save";
            appBarButton.Click += ApplicationBarIconButton_Click_save;
            ApplicationBar.Buttons.Add(appBarButton);

            ApplicationBarIconButton appBarButton2 = new ApplicationBarIconButton(new Uri("/Img/delete.png", UriKind.Relative));
            appBarButton2.Text = "delete";
            appBarButton2.Click += ApplicationBarIconButton_Click_delete;
            ApplicationBar.Buttons.Add(appBarButton2);
        }

        //save botton is go to previous page and pass text data， NOT SAVE IN DB 
        private void ApplicationBarIconButton_Click_save(object sender, EventArgs e)
        {
            NavigationHelper.NavigateGoBackExt(base.NavigationService, "vmNew", base.DataContext);
        }

        //delete text, go to previous page and pass context 
        private void ApplicationBarIconButton_Click_delete(object sender, EventArgs e)
        {
            if (MessageBox.Show("are you sure to delete?", string.Empty, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                (base.DataContext as TextViewModel).IsDelete = true;
                NavigationHelper.NavigateGoBackExt(base.NavigationService, "vmNew", base.DataContext);
            }
        }

        //back button后退键表示没点保存就后退，
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            e.Cancel = true;
            (base.DataContext as TextViewModel).IsAbort = true;
            NavigationHelper.NavigateGoBackExt(base.NavigationService, "vmNew", base.DataContext);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //一进入文本编辑页面，就会绑定文本视图模型
            TextViewModel text = NavigationHelper.NavigationExtGetValue<TextViewModel>("vmNew");
            base.DataContext = text;
            base.OnNavigatedTo(e);
        }
    }
}