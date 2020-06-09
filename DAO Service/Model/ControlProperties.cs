using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ControlProperties
    {
        private string morD;

        public string MorD
        {
            get { return morD; }
        }
        private string controlType;

        public string ControlType
        {
            get { return controlType; }
        }
        private string properties;

        public string Properties
        {
            get { return properties; }
        }

        public ControlProperties(string properties, string morD, string controlType)
        {
            this.morD = morD;
            this.controlType = controlType;
            this.properties = properties;
        }
    }
}
