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
using DrawNotePad.Commons;
using DrawNotePad.Models.DB;
using System.ComponentModel;
using System.Windows.Media;
using DrawNotePad.Models;
using DrawNotePad.Controls;

namespace DrawNotePad
{
    public partial class AddEditNote : PhoneApplicationPage
    {
        public AddEditNote()
        {
            this.InitializeComponent();
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

        //save button to save data to db
        private void ApplicationBarIconButton_Click_save(object sender, EventArgs e)
        {
            //if it is update, navigate to pervious page and pass parameters. or just  navigate to pervious page
            AddEditNoteViewModel new2 = base.DataContext as AddEditNoteViewModel;
            if (new2.Save())
            {
                if (new2.StateModel == StateModel.Update)
                {
                    NavigationHelper.NavigateGoBackExt(base.NavigationService, "id", new2.Note.Id);
                }
                else
                {
                    NavigationHelper.NavigateGoBack(base.NavigationService);
                }
            }
        }

        //delete data then navigate to pervious page
        private void ApplicationBarIconButton_Click_delete(object sender, EventArgs e)
        {
            if (MessageBox.Show("are you sure to delete?", "alert", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                AddEditNoteViewModel new2 = base.DataContext as AddEditNoteViewModel;
                new2.Delete();
                if (new2.StateModel == StateModel.Update)
                {
                    this.DeleteTile(new2.Note);
                    base.NavigationService.RemoveBackEntry();
                }
                NavigationHelper.NavigateGoBack(base.NavigationService, true);
            }
        }

        //跳转到添加记事页面时，取出相关数据
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (base.DataContext == null)
            {
                int id = NavigationHelper.NavigationExtGetIntValue("id");
                if (id > 0)
                {
                    base.DataContext = new AddEditNoteViewModel(id);
                    this.AppendAllControlsBody();
                }
                else if (id == 0)
                {
                    AddEditNoteViewModel new2 = new AddEditNoteViewModel
                    {
                        Note = { Favorite = NavigationHelper.NavigationExtGetBoolValue("favorite") }
                    };
                    base.DataContext = new2;
                }
            }
            ViewModelBase vm = NavigationHelper.NavigationExtGetValue<ViewModelBase>("vmNew");
            if (vm != null)
            {
                this.ArrangeBody(vm);
            }
        }

        //back press
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            e.Cancel = true;
            AddEditNoteViewModel new2 = base.DataContext as AddEditNoteViewModel;
            if (new2.StateModel == StateModel.Update)
            {
                NavigationHelper.NavigateGoBackExt(base.NavigationService, "id", new2.Note.Id);
            }
            else
            {
                NavigationHelper.NavigateGoBack(base.NavigationService);
            }
            base.OnBackKeyPress(e);
        }

        //event of select one of drawing notes to edit on StackPanel of add/edit page
        //then navigate to draw page to edit
        private void sd_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            int index = this.body.Children.IndexOf(sender as UIElement);
            ViewModelBase vm = (base.DataContext as AddEditNoteViewModel).ListViewModels[index];
            vm.CopyViewModel();
            this.NavigateDraw(vm);
        }

        //event of select one of text notes to edit on StackPanel of add/edit page
        //then navigate to text page to edit
        private void txt_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock block = sender as TextBlock;
            int index = this.body.Children.IndexOf(block.Parent as UIElement);
            ViewModelBase vm = (base.DataContext as AddEditNoteViewModel).ListViewModels[index];
            vm.CopyViewModel();
            this.NavigateText(vm);
        }

        //add new text button
        private void Button_Click_text(object sender, RoutedEventArgs e)
        {
            this.NavigateText(new TextViewModel());
        }

        //add new darwing button
        private void Button_Click_draw(object sender, RoutedEventArgs e)
        {
            this.NavigateDraw(new DrawViewModel());
        }

        //读取viewmodel数据，并添加到用户界面
        private void AppendAllControlsBody()
        {
            //读取文本viewmodel或涂鸦viewmodel
            AddEditNoteViewModel dataContext = base.DataContext as AddEditNoteViewModel;
            foreach (ViewModelBase base2 in dataContext.ListViewModels)
            {
                //读取文本viewmodel或涂鸦viewmodel
                if (base2 is TextViewModel)
                {
                    this.CreateTextBlock(base2);
                }
                else if (base2 is DrawViewModel)
                {
                    this.CreateDrawPreview(base2);
                }
            }
        }

        //添加编辑好的涂鸦或文本到 添加/编辑页面的body
        //add the text or drawing to the StackPanel of add/edit page 
        private void ArrangeBody(ViewModelBase vm)
        {
            int index = (base.DataContext as AddEditNoteViewModel).ListViewModels.IndexOf(vm);
            if (((index == -1) && !vm.IsAbort) && !vm.IsDelete)
            {
                (base.DataContext as AddEditNoteViewModel).ListViewModels.Add(vm);
                if (vm is TextViewModel)
                {
                    this.CreateTextBlock(vm);
                }
                else if (vm is DrawViewModel)
                {
                    this.CreateDrawPreview(vm);
                }
            }
            else if (index >= 0)
            {
                if (vm.IsDelete)
                {
                    (base.DataContext as AddEditNoteViewModel).ListViewModels.RemoveAt(index);
                    this.body.Children.RemoveAt(index);
                }
                else
                {
                    if (vm.IsAbort)
                    {
                        vm.IsAbort = false;
                        vm.RestoreViewModel();
                    }
                    if (vm is TextViewModel)
                    {
                        Border border = this.body.Children[index] as Border;
                        (border.Child as TextBlock).Text = (vm as TextViewModel).CurrentText;
                    }
                    else if (vm is DrawViewModel)
                    {
                        ShowDrawingsControl drawings = this.body.Children[index] as ShowDrawingsControl;
                        drawings.DataSource = (vm as DrawViewModel).CurrentList;
                    }
                }
            }
        }

        //
        private void CreateDrawPreview(ViewModelBase vm)
        {
            ShowDrawingsControl drawings2 = new ShowDrawingsControl
            {
                Margin = new Thickness(5.0)
            };
            ShowDrawingsControl drawings = drawings2;
            this.body.Children.Add(drawings);
            drawings.Tag = vm;
            drawings.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(this.sd_Tap);
            drawings.HorizontalAlignment = HorizontalAlignment.Stretch;
            drawings.VerticalAlignment = VerticalAlignment.Stretch;
            drawings.Background = new SolidColorBrush(Colors.White);
            drawings.DataSource = (vm as DrawViewModel).CurrentList;
        }

        //
        private void CreateTextBlock(ViewModelBase vm)
        {
            TextBlock block2 = new TextBlock
            {
                Text = (vm as TextViewModel).CurrentText,
                FontSize = 46.0,
                Foreground = new SolidColorBrush(Colors.Black),
                TextWrapping = TextWrapping.Wrap
            };
            TextBlock block = block2;
            block.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(this.txt_Tap);
            Border border2 = new Border
            {
                Margin = new Thickness(5.0)
            };
            Border border = border2;
            border.Child = block;
            this.body.Children.Add(border);
        }

        // delete tile after delete note data 
        private void DeleteTile(Note nota)
        {
            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault<ShellTile>(x => x.NavigationUri.ToString().Contains(string.Format("idNote={0}", nota.Id)));
            if (tile != null)
            {
                tile.Delete();
            }
        }

        //navigate to draw page
        private void NavigateDraw(ViewModelBase vm)
        {
            NavigationHelper.NavigateExt(base.NavigationService, "/DrawPage.xaml", "vmNew", vm);
        }

        //navigate to text page
        private void NavigateText(ViewModelBase vm)
        {
            NavigationHelper.NavigateExt(base.NavigationService, "/TextPage.xaml", "vmNew", vm);
        }
    }
}