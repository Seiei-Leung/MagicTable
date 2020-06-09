using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormDesign.common
{
    public class PropertyOfColumn
    {
        public int id
        {
            set;get;
        }
        public int idOfModel
        {
            set;
            get;
        }
        public string nameOfTable
        {
            set;
            get;
        }
        public string title
        {
            set;
            get;
        }
        public string field
        {
            set;
            get;
        }
        public int width
        {
            set;
            get;
        }
        public bool isFreezed
        {
            set;
            get;
        }
        public string formula
        {
            set;
            get;
        }
        public string defaultValue
        {
            set;
            get;
        }
        public bool isReadOnly
        {
            set;
            get;
        }
        public bool isShow
        {
            set;
            get;
        }
    }
}
