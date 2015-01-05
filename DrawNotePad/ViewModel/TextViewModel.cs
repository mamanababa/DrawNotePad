using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawNotePad.ViewModel
{
    //view model of text input page 
    //inherit ViewModelBase and implement all abstract methods 
    public class TextViewModel:ViewModelBase
    {
        private string _orgText = string.Empty;
        private string _text = string.Empty;

        //拷贝视图模型，编辑完的当前文本作为第一次进入文本编辑的内容
        //copy CurrentText to OriginalText
        public override void CopyViewModel()
        {
            this.OriginalText = this.CurrentText;
        }

        //恢复视图模型， 把第一次进入文本编辑的内容作为编辑完后的当前文本内容
        //recovre text to original
        public override void RestoreViewModel()
        { 
            this.CurrentText = this.OriginalText;
        }

        //编辑完的当前文本内容
        //the data after input
        public string CurrentText
        {
            get
            {
                return this._text;
            }
            set
            {
                this._text = value;
            }
        }

        //第一次进入文本编辑的内容
        //the data of fisrt time get into the text input page
        public string OriginalText
        {
            get
            {
                return this._orgText;
            }
            set
            {
                this._orgText = value;
            }
        }
    }
}