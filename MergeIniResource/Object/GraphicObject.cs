using MergeIniResource.EnumType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeIniResource.Object
{
    class GraphicObject
    {
        public string id { get; set; }
        public string value { get; set; }
        public FileType type { get; set; }

        public GraphicObject()
        {
            this.id = "";
            this.value = "";
            this.type = FileType.OTHER;
        }

        public GraphicObject(string id, string value)
        {
            this.id = id;
            this.value = value;
        }

        public GraphicObject(string id, string value, FileType type)
        {
            this.id = id;
            this.value = value;
            this.type = type;
        }

        public GraphicObject(string id, string value, object o)
        {
            this.id = id;
            this.value = value;
            this.type = (FileType) Enum.Parse(typeof(FileType), Enum.GetName(typeof(FileType), o));
        }
    }
}
