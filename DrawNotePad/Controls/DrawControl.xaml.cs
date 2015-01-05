using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using DrawNotePad.Models;
using System.Windows.Shapes;
using System.Windows.Input;

namespace DrawNotePad.Controls
{
    public partial class DrawControl : UserControl
    {
        //初始化操作  画刷默认黑色 initialize brush to defualt color (black)  
        public DrawControl()
        {
            this.InitializeComponent();
            this._brush = new SolidColorBrush(Colors.Black);
        }

        //加载控件 调用初始化首次画图页面，并在xaml中绑定
        //load user control and initialize draw pages
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.InizializeFirstPage();
        }
        public void InizializeFirstPage()
        {
            //如果线条集合为空则创建并作为第一个页面
            //create new draw pages list if it's null, 
            //then clear canvas make current page as the first page
            if (this.ListPage == null)
            {
                this.ListPage = new List<DrawModel>();
                this._currentPage = new DrawModel(this.ListPage.Count, this.panel.ActualWidth, this.panel.ActualHeight);
                this.ListPage.Add(this._currentPage);
                this.panel.Children.Clear();
                this.CurrentPageCount = 1;
            }
            //若线条集合不为空且页面数不为零，则取出第一个页面，并加载数据
            //get the first page if the draw pages list is not null and count of draw pages > 0, 
            //then load the data of the firstpage
            else if (this.ListPage.Count > 0)
            {
                this._currentPage = this.ListPage[0];
                this.CurrentPageCount = 1;
                this.ReloadData();
            }
            //if there just have one draw page in page list, create draw model object
            else if (this.ListPage.Count == 0)
            {
                this._currentPage = new DrawModel(this.ListPage.Count, this.panel.ActualWidth, this.panel.ActualHeight);
                this.ListPage.Add(this._currentPage);
                this.panel.Children.Clear();
                this.CurrentPageCount = 1;
            }
        }

        //load data of draw pages then show them on UI
        private void ReloadData()
        {
            try
            {
                if (this._currentPage != null)
                {
                    //线条模型数据转换为系统的可以显示在UI上的线条对象，
                    //make line model object as System.Windows.Shapes.Line object which can be shown on UI
                    foreach (LineModel line in this._currentPage.LineList)
                    {
                        if (line.IsVisible)
                        {
                            Line line2 = new Line
                            {
                                X1 = line.X1,
                                Y1 = line.Y1,
                                X2 = line.X2,
                                Y2 = line.Y2,
                                Stroke = this._brush,
                                StrokeThickness = this._strokeThickness,
                                StrokeLineJoin = PenLineJoin.Round,
                                StrokeStartLineCap = PenLineCap.Round,
                                StrokeEndLineCap = PenLineCap.Round
                            };
                            this.panel.Children.Add(line2);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
   

        //画笔颜色  color of brush
        private SolidColorBrush _brush;
        //当前涂鸦页面  current draw page object 
        private DrawModel _currentPage;
        //temorary lines 
        private Line _linetemp;
        //线条粗细 thickness of lines
        private double _strokeThickness = 7.0;

        //当前页面  current draw page        
        public static readonly DependencyProperty CurrentPageCountProperty = DependencyProperty.Register("CurrentPageCount", typeof(int), typeof(DrawControl), new PropertyMetadata(0));
        public int CurrentPageCount
        {
            get
            {
                return (int)base.GetValue(CurrentPageCountProperty);
            }
            set
            {
                base.SetValue(CurrentPageCountProperty, value);
            }
        }

        //涂鸦数据模型的集合 ，表示多张涂鸦页面
        //list of all draw pages (draw model object)
        private List<DrawModel> ListPage { get; set; }


        //画图触摸 flag of touch the screen to draw
        private bool _isManipulation;

        //触摸开始事件  start draw event
        private void UC_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            //判断是否触摸了xaml的画布<canvas>或者线
            //if the user touch the panel of <canvas> in xaml or touch on a exsit line , then start to draw
            if (e.OriginalSource.Equals(this.panel) || (e.OriginalSource is Line))
            {
                this._linetemp = null;
                //触摸事件的容器设置为xaml中名为panel的<canvas>，按照相对<canvas>的坐标进行计算
                e.ManipulationContainer = this.panel;
                //put all visable lines to draw model
                this._currentPage.ResetInvisible();
                //create lines
                this._currentPage.ProgressIndex++;
                Line line = new Line
                {
                    //赋值坐标 coordinate
                    X1 = e.ManipulationOrigin.X,
                    Y1 = e.ManipulationOrigin.Y,
                    //赋值颜色和粗细
                    Stroke = this._brush,
                    StrokeThickness = this._strokeThickness,
                    //线条属性 ，线条开始和结束的点是半圆形
                    StrokeLineJoin = PenLineJoin.Round,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round
                };
                this._linetemp = line;
                //画图开始的标志 set the start flag
                this._isManipulation = true;
            }
        }

        //触摸的过程事件 during draw event
        private void UC_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (this._isManipulation)
            {
                //控制边界  control the scope

                //if flag is true, is out of canvas
                bool flag = false;
                //若坐标处于canvas以外，则不捕获轨迹
                //situations of out of canvas
                if (e.ManipulationOrigin.X < 0.0)
                {
                    flag = true;
                }
                if (e.ManipulationOrigin.Y < 0.0)
                {
                    flag = true;
                }
                if (e.ManipulationOrigin.X > e.ManipulationContainer.RenderSize.Width)
                {
                    flag = true;
                }
                if (e.ManipulationOrigin.Y > e.ManipulationContainer.RenderSize.Height)
                {
                    flag = true;
                }   
                if (flag)
                {
                    this._linetemp = null;
                    e.Handled = true;
                }
                //else is in the scope of canvas, then create lines 
                //if temorary lines are null then use the current coordinate to create lines
                //线条对象为空时就用当前坐标创建线条
                else if (this._linetemp == null)
                {
                    Line line2 = new Line
                    {
                        X1 = e.ManipulationOrigin.X,
                        Y1 = e.ManipulationOrigin.Y,
                        Stroke = this._brush,
                        StrokeThickness = this._strokeThickness,
                        StrokeLineJoin = PenLineJoin.Round,
                        StrokeStartLineCap = PenLineCap.Round,
                        StrokeEndLineCap = PenLineCap.Round
                    };
                    this._linetemp = line2;
                }
                
                else
                {
                //否则就用已有的线条对象linetemp，赋值坐标，再添加到线条集合
                    //else use current line object 
                    this._linetemp.X2 = e.ManipulationOrigin.X;
                    this._linetemp.Y2 = e.ManipulationOrigin.Y;
                    
                    this._currentPage.LineList.Add(new LineModel(this._linetemp.X1, this._linetemp.Y1, this._linetemp.X2, this._linetemp.Y2, this._currentPage.ProgressIndex));
                    //并在画板上显示，然后临时线条设为空，然后创新线条 
                    //show on canvas, then make linetemp null, then create new line
                    this.panel.Children.Add(this._linetemp); 
                    this._linetemp = null;
                    
                    Line line = new Line
                    {
                        //前一坐标 last coordinate
                        X1 = e.ManipulationOrigin.X,
                        Y1 = e.ManipulationOrigin.Y,
                        Stroke = this._brush,
                        StrokeThickness = this._strokeThickness,
                        StrokeLineJoin = PenLineJoin.Round,
                        StrokeStartLineCap = PenLineCap.Round,
                        StrokeEndLineCap = PenLineCap.Round
                    };
                    this._linetemp = line;
                }
            }
        }

        //触摸结束事件 draw end event
        private void UC_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            this._isManipulation = false;
        }

        //获取涂鸦模型数据的集合
        //get the list of DrawModel object
        public IEnumerable<DrawModel> GetListPage()
        {
            return this.ListPage.Where<DrawModel>(delegate(DrawModel item)
            {
                item.ResetInvisible();
                return (item.LineList.Count > 0);
            });
        }

        //把涂鸦模型数据克隆到list,再调用初始化页面方法，加载数据到控件
        //use clone() of DrawModel, and initialize page , set the draw model data into the user control
        public void SetListPage(List<DrawModel> lst)
        {
            if (lst != null)
            {
                if (this.ListPage != null)
                {
                    this.ListPage.Clear();
                }
                this.ListPage = new List<DrawModel>();
                this.ListPage.AddRange(from item in lst select item.Clone());
                this.InizializeFirstPage();
            }
        }

        
        //操作画板相关方法，下一页，上一页，清除页面，取消，恢复等操作
        //methods of control the draw pages

        //next draw page if the page contains data and there has one more pages 
        public void NextPage()
        {
            if ((this._currentPage != null) && (this._currentPage.LineList.Count != 0))
            {
                //get current page index
                int index = this.ListPage.IndexOf(this._currentPage);
                this._linetemp = null;
                //当前索引小于最后一页索引就获取下一页数据
                // if current index < the index of last page , get data of next page 
                if (index < (this.ListPage.Count - 1))
                {
                    this._currentPage = this.ListPage[index + 1];
                    this.panel.Children.Clear();
                    this.ReloadData();
                }
                //if current page is the last page， create new drawmodel object 
                else if (index >= (this.ListPage.Count - 1))
                {
                    this._currentPage = new DrawModel(this.ListPage.Count, this.panel.ActualWidth, this.panel.ActualHeight);
                    this.ListPage.Add(this._currentPage);
                    this.panel.Children.Clear();
                    this.ReloadData();
                }
                if (this._currentPage != null)
                {
                    this.CurrentPageCount = this.ListPage.IndexOf(this._currentPage) + 1;
                }
            }
        }

        //previous draw page smiliar witj next page
        public void PreviousPage()
        {
            if (this._currentPage != null)
            {
                int index = this.ListPage.IndexOf(this._currentPage);
                this._linetemp = null;
                if (index > 0)
                {
                    this._currentPage = this.ListPage[index - 1];
                    this.panel.Children.Clear();
                    this.ReloadData();
                }
                if (this._currentPage != null)
                {
                    this.CurrentPageCount = this.ListPage.IndexOf(this._currentPage) + 1;
                }
            }
        }

        //clear whole data of pgae on list and canvas
        public void Clear()
        {
            this._currentPage.LineList.Clear();
            this.panel.Children.Clear();
        }

        //恢复操作
        //make invisible lines(lines that were erased) visible
        public void Redo()
        {
            if (!this._isManipulation)
            {
                //get the first invisible(erased) line then add to canvas replace the data of  line list
                LineModel firstInvisible = this._currentPage.GetFirstInvisible();
                if (firstInvisible != null)
                {
                    foreach (LineModel line2 in this._currentPage.GetByIndex(firstInvisible.Index))
                    {
                        line2.IsVisible = true;
                        Line line3 = new Line
                        {
                            X1 = line2.X1,
                            Y1 = line2.Y1,
                            X2 = line2.X2,
                            Y2 = line2.Y2,
                            Stroke = this._brush,
                            StrokeThickness = this._strokeThickness,
                            StrokeLineJoin = PenLineJoin.Round,
                            StrokeStartLineCap = PenLineCap.Round,
                            StrokeEndLineCap = PenLineCap.Round
                        };
                        this.panel.Children.Insert(this._currentPage.LineList.IndexOf(line2), line3);
                    }
                }
            }
        }

        //取消操作
        //undo operation , get last visible line from Draw model then return line model 
        public void Undo()
        {
            if (!this._isManipulation)
            {
                //get last visible line 
                LineModel lastVisible = this._currentPage.GetLastVisible();
                if (lastVisible != null)
                {
                    IEnumerable<LineModel> byIndex = this._currentPage.GetByIndex(lastVisible.Index);
                    if (byIndex.Count<LineModel>() > 0)
                    {
                        LineModel[] lineArray = byIndex.ToArray<LineModel>();
                       //remove the last visible line from line list
                        for (int i = lineArray.Length - 1; i >= 0; i--)
                        {
                            int index = this._currentPage.LineList.IndexOf(lineArray[i]);
                            lineArray[i].IsVisible = false;
                            this.panel.Children.RemoveAt(index);
                        }
                    }
                }
            }
        }
    }
}