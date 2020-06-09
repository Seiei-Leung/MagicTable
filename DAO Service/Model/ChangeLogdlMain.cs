using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ChangeLogdlMain
    {
        public string Id { get; set; }
        public string FormClassName { get; set; }
        public string FormCaption { get; set; }
        public string TableName { get; set; }
        public string Serialno	{ get; set; }
        public string FieldName	{ get; set; }
        public string FieldTitle { get; set; }
        public string BFFieldValue { get; set; }
        public string AFFieldValue { get; set; }
        public string Buser{ get; set; }
        public DateTime BTime	{ get; set; }
    }
}
