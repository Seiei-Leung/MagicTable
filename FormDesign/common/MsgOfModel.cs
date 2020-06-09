using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormDesign.common
{
    public class MsgOfModel
    {
        public MsgOfModel(string id, string titleOfModel, string classNameOfModel, string nameOfMainTable, string nameOfDetailTable, bool typeOfModel, string relationField)
        {
            this.id = id;
            this.titleOfModel = titleOfModel;
            this.classNameOfModel = classNameOfModel;
            this.nameOfMainTable = nameOfMainTable;
            this.nameOfDetailTable = nameOfDetailTable;
            this.typeOfModel = typeOfModel;
            this.relationField = relationField;
        }

        public string id
        {
            set;
            get;
        }
        public string titleOfModel
        {
            set;
            get;
        }
        public string classNameOfModel
        {
            set;
            get;
        }
        public string nameOfMainTable
        {
            set;
            get;
        }
        public string nameOfDetailTable
        {
            set;
            get;
        }
        public bool typeOfModel
        {
            set;
            get;
        }
        public string relationField
        {
            set;
            get;
        }

        

    }
}
