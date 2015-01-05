using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawNotePad.Models
{
    //线条数据模型
    //line model incuding all lines 
    public class LineModel
    {
        //构造,初始化线条模型
        //initial model
        public LineModel(double x1, double y1, double x2, double y2, int index)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Index = index;  
        }

        public LineModel()
        {
            // TODO: Complete member initialization
        }

        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }

        //设置擦掉线条后的可见性
        //set the visibility of lines
        private bool _isVisible = true;
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
            }
        }

        //线条的先后顺序
        //order of lines
        public int Index { get; set; }

    }
}
