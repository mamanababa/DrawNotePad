using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawNotePad.Models.DB
{
    //数据库连接信息和数据表信息
    //并在app.xaml.cs中创建对象并初始化
    public class DBContext: DataContext
    {
        public static string DBConnect = "Data Source=isostore:/DrawNotePad.sdf";

        public DBContext(string str) : base(str) { }

        public Table<Note> Notes;
        public Table<NoteDetail> NoteDetails;
    }
}
