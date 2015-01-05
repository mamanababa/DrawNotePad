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
using System.ComponentModel;
using DrawNotePad.ViewModel;
using DrawNotePad.Commons;
using DrawNotePad.Models;

namespace DrawNotePad
{
    public partial class DrawPage : PhoneApplicationPage
    {
        public DrawPage()
        {
            InitializeComponent();
            //添加菜单栏，用于下一页上一页操作
            BuildLocalizedApplicationBar();
        }
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // buttons for previous pages and next pages and done operation
            ApplicationBarIconButton appBarButton_pre = new ApplicationBarIconButton(new Uri("/Img/pre.png", UriKind.Relative));
            appBarButton_pre.Text = "pre";
            appBarButton_pre.Click += appBarButton_Click_pre;
            ApplicationBar.Buttons.Add(appBarButton_pre);
            
            ApplicationBarIconButton appBarButton_done = new ApplicationBarIconButton(new Uri("/Img/done.png", UriKind.Relative));
            appBarButton_done.Text = "done";
            appBarButton_done.Click += appBarButton_Click_done;
            ApplicationBar.Buttons.Add(appBarButton_done);

            ApplicationBarIconButton appBarButton_next = new ApplicationBarIconButton(new Uri("/Img/next.png", UriKind.Relative));
            appBarButton_next.Text = "next";
            appBarButton_next.Click += appBarButton_Click_next;
            ApplicationBar.Buttons.Add(appBarButton_next);

        
        }

        //previus page button
        private void appBarButton_Click_pre(object sender, EventArgs e)
        {
            draw.PreviousPage();
        }

        //next page button
        private void appBarButton_Click_next(object sender, EventArgs e)
        {
            draw.NextPage();
        }

        //done button
        void appBarButton_Click_done(object sender, EventArgs e)
        {
            (base.DataContext as DrawViewModel).CurrentList = this.draw.GetListPage().ToList<DrawModel>();
            NavigationHelper.NavigateGoBackExt(base.NavigationService, "vmNew", base.DataContext);
        }

        //取消按钮undo button
        private void Button_Click_undo(object sender, RoutedEventArgs e)
        {
            this.draw.Undo();
        }

        //恢复按钮redo button
        private void Button_Click_redo(object sender, RoutedEventArgs e)
        {
            this.draw.Redo();
        }

        //清空按钮clear button
        private void Button_Click_clear(object sender, RoutedEventArgs e)
        {
            this.draw.Clear();
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            e.Cancel = true;
            (base.DataContext as DrawViewModel).IsAbort = true;
            NavigationHelper.NavigateGoBackExt(base.NavigationService, "vmNew", base.DataContext);
        }

        //一进入文本编辑页面，就会绑定涂鸦视图模型
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DrawViewModel draw = NavigationHelper.NavigationExtGetValue<DrawViewModel>("vmNew");
            if (draw == null)
            {
                throw new Exception("set viewmodel");
            }
            base.DataContext = draw;
            base.OnNavigatedTo(e);
        }

        //load draw page ,use clone() of DrawModel, and initialize page , set the draw model data into the user control     
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.draw.SetListPage((base.DataContext as DrawViewModel).CurrentList);
        }
    }
}