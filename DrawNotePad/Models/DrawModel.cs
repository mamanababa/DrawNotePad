using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawNotePad.Models
{
    //涂鸦数据模型，包含条线集合，可序列化，涂鸦图形和数据模型互相转换
    // draw data model , including list of lines
    public class DrawModel
    {
        public DrawModel(int page, double width, double height)
        {
            PageNo = page;
            Width = width;
            Height = height;
            _lineList = new List<LineModel>();
        }

        //读取数据库的值，反序列化为涂鸦数据模型对象
        //get string from database and deserialize to drwa data model object
        public DrawModel(string value)
        {
            Deserialize(value);
        }

        //包含线条的List
        //list of lines
        private List<LineModel> _lineList;
        public List<LineModel> LineList
        {
            get
            {
                return _lineList;
            }
        }

        //图片宽高度
        // width and height of draw pictures
        public double Width { get; set; }
        public double Height { get; set; }
        //图片页码
        //page number of draw pictures
        public int PageNo { get; set; }
        //线条顺序索引
        //index of lines
        public int ProgressIndex { get; set; }


        //把所有属性序列化为string， 用于存到独立存储
        //Serialize all data to string 
        public string Serialize()
        {
            //字符串拼接
            //append string
            StringBuilder sb = new StringBuilder();
            //冒号隔开每个属性
            string format = "{0}:";
            sb.AppendFormat(format, new Object[] { PageNo });
            sb.AppendFormat(format, new Object[] { ProgressIndex });
            sb.AppendFormat(format, new Object[] { Width });
            sb.AppendFormat(format, new Object[] { Height });
            _lineList.ForEach(item =>
                {
                    sb.AppendFormat(format, new Object[] { item.Index});
                    sb.AppendFormat(format, new Object[] { item.IsVisible });
                    sb.AppendFormat(format, new Object[] { item.X1 });
                    sb.AppendFormat(format, new Object[] { item.Y1 });
                    sb.AppendFormat(format, new Object[] { item.X2 });
                    sb.AppendFormat(format, new Object[] { item.Y2 });
                });
            sb.Append(";");
            return sb.ToString();
        }

        // 之后再反序列化,用于从独立存储恢复
        //Deserialize all string 
        public void Deserialize(string value)
        {
            try
            {
                if (value != "")
                {
                    string[] strArray = value.Split(':');
                    if (strArray != null && strArray.Length > 0)
                    {
                        if (_lineList == null)
                        {
                            _lineList = new List<LineModel>();
                        }
                        else
                        {
                            _lineList.Clear();
                        }
                        PageNo = Convert.ToInt32(strArray[0]);
                        ProgressIndex = Convert.ToInt32(strArray[1]);
                        Width = Convert.ToDouble(strArray[2]);
                        Height = Convert.ToDouble(strArray[3]);
                        for (int i = 4; i < strArray.Length; i += 6)
                        {
                            if (strArray[i].ToString() == ";")
                            {
                                return;
                            }
                            LineModel item = new LineModel(0, 0, 0, 0, 0)
                            {
                                Index = Convert.ToInt32(strArray[i]),
                                IsVisible = Convert.ToBoolean(strArray[i + 1]),
                                X1 = Convert.ToDouble(strArray[i + 2]),
                                Y1 = Convert.ToDouble(strArray[i + 3]),
                                X2 = Convert.ToDouble(strArray[i + 4]),
                                Y2 = Convert.ToDouble(strArray[i + 5])
                            };
                            LineList.Add(item);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        //通过 克隆 涂鸦数据模型对象，重新创建对象 
        //create new draw data model object by clone
        public DrawModel Clone()
        {
            DrawModel drawModel = new DrawModel(PageNo, Width, Height)
            {
                ProgressIndex = this.ProgressIndex
            };

            this.LineList.ForEach(item =>
            {
                LineModel line = new LineModel()
                {
                    Index = item.Index,
                    IsVisible = item.IsVisible,
                    X1 = item.X1,
                    Y1 = item.Y1,
                    X2 = item.X2,
                    Y2 = item.Y2
                };
                drawModel.LineList.Add(line);
            });
            return drawModel;
        }

        //传入线条索引，通过linq语句，获取某一条线，用于恢复或取消功能
        //use linq to get lines from LineList by index and put them into Enum
        public IEnumerable<LineModel> GetByIndex(int index)
        {
            //linq
            return from item in this.LineList where item.Index == index select item;
        }

        //获取第一条不可见的线, 用于恢复擦除的线
        //get the first invisible line（line that was erased）
        public LineModel GetFirstInvisible()
        {
            //遍历线条list,查找第一条可见线条
            foreach (var item in LineList)
            {
                if (!item.IsVisible) 
                    return item;
            }
            return null;
        }

        //获取最后一条可见的线
        //get the last visible line,  invoked by Undo()
        public LineModel GetLastVisible()
        {
            //遍历线条list,查找最后第一条可见线条
            for (int i = LineList.Count - 1; i >= 0; i--)
            {
                if (LineList[i].IsVisible) 
                  return LineList[i];
            }
            return null;
        }

        //清除不可见线条
        //get all visable lines and clear all invisible lines and reset list 
        public void ResetInvisible()
        {
            //linq查询出可见的
            var items = from item in LineList where item.IsVisible select item;

            if (items != null)
            {
                var listItems = items.ToList();
                LineList.Clear();
                LineList.AddRange(listItems);
            }
        }
    }
}