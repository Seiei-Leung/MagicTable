using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TemplateCustom
{
    public partial class FrmMasterDetail : FrmBase
    {
        public FrmMasterDetail()
        {
            InitializeComponent();
            dicGrids.Add("master", grid);
            dicGrids.Add("detail", griddl);
        }
    }
}
