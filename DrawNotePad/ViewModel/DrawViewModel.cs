using DrawNotePad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawNotePad.ViewModel
{
    //view model of draw page 
    //inherit ViewModelBase and implement all abstract methods , similiar with TextViewModel
    public class DrawViewModel:ViewModelBase
    {
        public DrawViewModel()
        {
            this.OriginalList = new List<DrawModel>();
            this.CurrentList = new List<DrawModel>();
        }　

        public override void CopyViewModel()
        {
            this.OriginalList.Clear();
            foreach (DrawModel draw in this.CurrentList)
            {
                this.OriginalList.Add(draw.Clone());
            }
        }

        public override void RestoreViewModel()
        {
            this.CurrentList.Clear();
            foreach (DrawModel draw in this.OriginalList)
            {
                this.CurrentList.Add(draw.Clone());
            }
        }

        public List<DrawModel> CurrentList { get; set; }

        public List<DrawModel> OriginalList { get; set; }

    }
}
