using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Popup;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Editors;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;

namespace Common
{
    public partial class CustomSearchLookUpEdit : SearchLookUpEdit
    {
        protected internal LayoutControlItem flciFindButton;

        static CustomSearchLookUpEdit()
        {
            RepositoryItemCustomSearchLookUpEdit.RegisterCustomSearchLookUpEdit();
        }

        public CustomSearchLookUpEdit()
        {
        }

        public override string EditorTypeName
        {
            get
            {
                return
                    RepositoryItemCustomSearchLookUpEdit.CustomEditName;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new RepositoryItemCustomSearchLookUpEdit Properties
        {
            get { return base.Properties as RepositoryItemCustomSearchLookUpEdit; }
        }

        LayoutControlItem GetFindButtonLayoutItem(PopupBaseForm Form)
        {
            foreach (Control FormC in Form.Controls)
            {
                if (FormC is SearchEditLookUpPopup)
                {
                    SearchEditLookUpPopup SearchPopup = FormC as SearchEditLookUpPopup;
                    foreach (Control SearchPopupC in SearchPopup.Controls)
                    {
                        if (SearchPopupC is LayoutControl)
                        {
                            LayoutControl FormLayout = SearchPopupC as LayoutControl;
                            Control Button = FormLayout.GetControlByName("btFind");
                            if (Button != null)
                            {
                                return FormLayout.GetItemByControl(Button);
                            }

                        }
                    }
                }
            }
            return null;
        }

        internal void ChangeFindButtonVisibility(bool Visible)
        {
            if (flciFindButton != null)
            {
                flciFindButton.Visibility = (Visible) ? LayoutVisibility.Always : LayoutVisibility.Never;
            }
        }

        protected override PopupBaseForm CreatePopupForm()
        {
            PopupBaseForm Popup = base.CreatePopupForm();
            flciFindButton = GetFindButtonLayoutItem(Popup);
            ChangeFindButtonVisibility(Properties.ShowFindButton);
            return Popup;
        }
    }

    [UserRepositoryItem("RegisterCustomEdit")]
    public class RepositoryItemCustomSearchLookUpEdit : RepositoryItemSearchLookUpEdit
    {
        static RepositoryItemCustomSearchLookUpEdit()
        {
            RegisterCustomSearchLookUpEdit();
        }

        public static void RegisterCustomSearchLookUpEdit()
        {
            Image img = null;
            try
            {
                img = (Bitmap)Bitmap.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("DevExpress.CustomEditors.CustomEdit.bmp"));
            }
            catch
            {
            }
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(CustomEditName, typeof(CustomSearchLookUpEdit), typeof(RepositoryItemCustomSearchLookUpEdit),
              typeof(LookUpEditViewInfo), new ButtonEditPainter(), true, img));
        }

        public new CustomSearchLookUpEdit OwnerEdit
        {
            get { return base.OwnerEdit as CustomSearchLookUpEdit; }
        }

        bool fShowFindButton;

        public RepositoryItemCustomSearchLookUpEdit()
        {
        }

        public const string CustomEditName = "CustomSearchLookUpEdit";

        [DefaultValue(true)]
        public bool ShowFindButton
        {
            get
            { return fShowFindButton; }
            set
            {
                if (fShowFindButton != value)
                {
                    fShowFindButton = value;
                    if (OwnerEdit != null)
                    {
                        OwnerEdit.ChangeFindButtonVisibility(value);
                    }
                    OnPropertiesChanged();
                }
            }
        }

        public override string EditorTypeName
        {
            get { return CustomEditName; }
        }

        public override void Assign(RepositoryItem item)
        {
            BeginUpdate();
            try
            {
                base.Assign(item);
                RepositoryItemCustomSearchLookUpEdit source = item as RepositoryItemCustomSearchLookUpEdit;
                if (source == null) return;
            }
            finally
            {
                EndUpdate();
            }
        }
    }
}
