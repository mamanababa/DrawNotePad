using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawNotePad.Models.DB
{
    //[Table]
    //public class NoteDetail
    //{
    //    //initialization,
    //    public NoteDetail()
    //    {
    //        _draw = SerializeListDrawSub();
    //    }

    //    //primary key for NoteData table 
    //    [Column(IsDbGenerated = true, IsPrimaryKey = true)]
    //    public int Id { get; set; }

    //    private int? _parentId = null;
    //    [Column(Storage = "ParentId")]
    //    public int? ParentId
    //    {
    //        get
    //        {
    //            return _parentId;
    //        }
    //        set
    //        {
    //            _parentId = value;
    //        }

    //    }

    //    //reference of parent table(Note)
    //    private EntityRef<Note> parentRef = new EntityRef<Note>();
    //    [Association(Name = "FK_Note_NoteDetail", Storage = "parentRef", ThisKey = "ParentId", OtherKey = "Id", IsForeignKey = true)]
    //    public Note Parent
    //    {
    //        get
    //        {
    //            return parentRef.Entity;
    //        }
    //        set
    //        {
    //            Note note = parentRef.Entity;
    //            if (note != value || parentRef.HasLoadedOrAssignedValue)
    //            {
    //                parentRef.Entity = value;
    //                if (value == null)
    //                {
    //                    ParentId = null;
    //                }
    //                else
    //                {
    //                    value.Body.Add(this);
    //                    ParentId = new int?(value.Id);
    //                }

    //            }
    //        }
    //    }

    //    //纯文本显示
    //    [Column]
    //    public string Text { get; set; }

    //    //涂鸦显示，从数据库把string反序列化为涂鸦数据模型对象集合
    //    //derialize string to draw model object list
    //    private string _draw = "";
    //    [Column(DbType = "ntext", UpdateCheck = UpdateCheck.Never)]
    //    public string Draw
    //    {
    //        get
    //        {
    //            return _draw;
    //        }
    //        set
    //        {
    //            _draw = value;
    //            //create new list if it is null or clear the list
    //            if (ListPageDraw == null)
    //            {
    //                ListPageDraw = new List<DrawModel>();
    //            }
    //            else
    //            {
    //                ListPageDraw.Clear();
    //            }
    //            // then derialize string with "#" splitor, then add to draw page list as draw model object
    //            if (_draw != "")
    //            {
    //                string[] strArray = _draw.Split('#');
    //                if (strArray != null && strArray.Length > 0)
    //                {
    //                    for (int i = 0; i < strArray.Length; i++)
    //                    {
    //                        if (strArray[i] != "")
    //                        {
    //                            ListPageDraw.Add(new DrawModel(strArray[i]));
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    private List<DrawModel> _listPageDraw;
    //    public List<DrawModel> ListPageDraw
    //    {
    //        get
    //        {
    //            return _listPageDraw;
    //        }
    //        set
    //        {
    //            _listPageDraw = value;
    //        }
    //    }

    //    public void SerializeListDraw()
    //    {
    //        this._draw = this.SerializeListDrawSub();
    //    }

    //    //把涂鸦数据模型对象集合序列化为string存入数据库
    //    //serialize draw model list to string to storage in DB       
    //    private string SerializeListDrawSub()
    //    {
    //        if (ListPageDraw != null && ListPageDraw.Count > 0)
    //        {
    //            //存入数据库要分隔符来分割
    //            //splitor to split string of draw model list
    //            StringBuilder str = new StringBuilder();
    //            string fomat = "{0}#";
    //            ListPageDraw.ForEach(item => str.AppendFormat(fomat, new object[] { item.Serialize() }));
    //            return str.ToString();
    //        }
    //        return "";
    //    }
    //}
    [Table]
    public class NoteDetail
    {
        //initialization,
        public NoteDetail()
        {
            _draw = SerializeListDrawSub();
        }

        //primary key for NoteData table 
        [Column(IsDbGenerated = true, IsPrimaryKey = true)]
        public int Id { get; set; }

        private int? _parentId = null;
        [Column(Storage = "ParentId")]
        public int? ParentId
        {
            get
            {
                return _parentId;
            }
            set
            {
                _parentId = value;
            }

        }

        //reference of parent table(Note)
        private EntityRef<Note> parentRef = new EntityRef<Note>();
        [Association(Name = "FK_Note_NoteDetail", Storage = "parentRef", ThisKey = "ParentId", OtherKey = "Id", IsForeignKey = true)]
        public Note Parent
        {
            get
            {
                return parentRef.Entity;
            }
            set
            {
                Note note = parentRef.Entity;
                if (note != value || parentRef.HasLoadedOrAssignedValue)
                {
                    parentRef.Entity = value;
                    if (value == null)
                    {
                        ParentId = null;
                    }
                    else
                    {
                        value.Body.Add(this);
                        ParentId = new int?(value.Id);
                    }

                }
            }
        }

        //纯文本显示
        [Column]
        public string Text { get; set; }

        //涂鸦显示，从数据库把string反序列化为涂鸦数据模型对象集合
        //derialize string to draw model object list
        private string _draw = "";
        [Column(DbType = "ntext", UpdateCheck = UpdateCheck.Never)]
        public string Draw
        {
            get
            {
                return _draw;
            }
            set
            {
                _draw = value;
                //create new list if it is null or clear the list
                if (ListPageDraw == null)
                {
                    ListPageDraw = new List<DrawModel>();
                }
                else
                {
                    ListPageDraw.Clear();
                }
                // then derialize string with "#" splitor, then add to draw page list as draw model object
                if (_draw != "")
                {
                    string[] strArray = _draw.Split('#');
                    if (strArray != null && strArray.Length > 0)
                    {
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            if (strArray[i] != "")
                            {
                                ListPageDraw.Add(new DrawModel(strArray[i]));
                            }
                        }
                    }
                }
            }
        }

        private List<DrawModel> _listPageDraw;
        public List<DrawModel> ListPageDraw
        {
            get
            {
                return _listPageDraw;
            }
            set
            {
                _listPageDraw = value;
            }
        }

        public void SerializeListDraw()
        {
            this._draw = this.SerializeListDrawSub();
        }

        //把涂鸦数据模型对象集合序列化为string存入数据库
        //serialize draw model list to string to storage in DB       
        private string SerializeListDrawSub()
        {
            if (ListPageDraw != null && ListPageDraw.Count > 0)
            {
                //存入数据库要分隔符来分割
                //splitor to split string of draw model list
                StringBuilder str = new StringBuilder();
                string fomat = "{0}#";
                ListPageDraw.ForEach(item => str.AppendFormat(fomat, new object[] { item.Serialize() }));
                return str.ToString();
            }
            return "";
        }
    }
}
