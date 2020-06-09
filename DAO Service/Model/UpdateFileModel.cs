using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Model
{
    [DataContract]
    public class UpdateFileModel
    {
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string Loc { get; set; }
        [DataMember]
        public string DownLoadUrl { get; set; }

    }
}
