using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawNotePad.ViewModel
{
    public class ViewModelBase
    {
        //abstract methods 
        public virtual void CopyViewModel()
        {
        }

        public virtual void RestoreViewModel()
        {
        }

        //判断是否没点保存就后退
        public bool IsAbort { get; set; }


        //判断是否删除了画图或文本内容
        //is delete the date of draw or text 
        public bool IsDelete { get; set; }
    }
}
