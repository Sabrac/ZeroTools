using MergeIniResource.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeIniResource
{
    class IniObject
    {
        public string name { get; set; }
        public Dictionary<string, GraphicObject> lsGraphic { get; set; }
        public int count
        {
            get
            {
                return this.lsGraphic.Count;
            }
        }

        public IniObject()
        {
            this.name = "";
            this.lsGraphic = new Dictionary<string, GraphicObject>();
        }

        public IniObject(string name)
        {
            this.name = name;
            this.lsGraphic = new Dictionary<string, GraphicObject>();
        }

        public IniObject(string name, Dictionary<string, GraphicObject> lsGraphic)
        {
            this.name = name;
            this.lsGraphic = lsGraphic;
        }
    }
}
