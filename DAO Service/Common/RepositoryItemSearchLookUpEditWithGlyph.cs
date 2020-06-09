using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.ViewInfo;
using System.Drawing;
using DevExpress.XtraExport;
using DevExpress.XtraEditors;
using System.ComponentModel;

namespace Common
{
    public class RepositoryItemSearchLookUpEditWithGlyph : RepositoryItemSearchLookUpEdit
    {
        static RepositoryItemSearchLookUpEditWithGlyph()
        {
            RegisterSearchLookUpEditWithGlyph();
        }
        public event ImageByValueEventHandler GetImageByValue;
        public object GetImage(ImageByValueEventArgs e)
        {
            if (GetImageByValue != null)
                GetImageByValue(this, e);
            return e.Image;
        }
        public override string EditorTypeName { get { return "SearchLookUpEditWithGlyph"; } }
        public static void RegisterSearchLookUpEditWithGlyph()
        {
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo("SearchLookUpEditWithGlyph", typeof(SearchLookUpEditWithGlyph), typeof(RepositoryItemSearchLookUpEditWithGlyph),
                typeof(SearchLookUpEditWithGlyphBaseViewInfo), new SearchEditPainterWithGlyph(), false));
        }
        public override void Assign(RepositoryItem item)
        {
            RepositoryItemSearchLookUpEditWithGlyph li = item as RepositoryItemSearchLookUpEditWithGlyph;
            if (li != null)
            {
                this.GetImageByValue += li.GetImageByValue;
            }
            base.Assign(item);
        }
        protected override void ClearClick()
        {
            base.ClearClick();
            SearchLookUpEditWithGlyph edit = OwnerEdit as SearchLookUpEditWithGlyph;
            if (edit != null)
            {
                edit.Image = null;
                edit.DoValidate();
            }
        }
    }
    public class SearchEditPainterWithGlyph : ButtonEditPainter
    {
        protected override void DrawGlyphCore(ControlGraphicsInfoArgs info, ButtonEditViewInfo be)
        {
            SearchLookUpEditWithGlyphBaseViewInfo vi = be as SearchLookUpEditWithGlyphBaseViewInfo;
            if (vi.Image == null) return;
            if (vi.Image.Size.Width > SearchLookUpEditWithGlyphBaseViewInfo.DefaultImageSize.Width * 2)
                info.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            info.Paint.DrawImage(info.Graphics, vi.Image, vi.GlyphBounds, new Rectangle(Point.Empty, vi.Image.Size), vi.State != DevExpress.Utils.Drawing.ObjectState.Disabled);
        }
    }
    public class SearchLookUpEditWithGlyphBaseViewInfo : SearchLookUpEditBaseViewInfo
    {
        Image image;
        public static Size DefaultImageSize = new Size(16, 16);
        public SearchLookUpEditWithGlyphBaseViewInfo(RepositoryItem item) : base(item) { }
        public new SearchLookUpEditWithGlyph OwnerEdit { get { return base.OwnerEdit as SearchLookUpEditWithGlyph; } }
        public new RepositoryItemSearchLookUpEditWithGlyph Item { get { return base.Item as RepositoryItemSearchLookUpEditWithGlyph; } }
        public override bool IsExistImage { get { return true; } }
        public override Size ImageSize { get { return Image == null ? Size.Empty : DefaultImageSize; } }
        public override DevExpress.Utils.Drawing.TextGlyphDrawModeEnum GlyphDrawMode
        {
            get
            {
                if (ImageSize.IsEmpty) return DevExpress.Utils.Drawing.TextGlyphDrawModeEnum.Text;
                return DevExpress.Utils.Drawing.TextGlyphDrawModeEnum.TextGlyph;
            }
        }
        bool requireUpdateImage = false;
        protected override void OnEditValueChanged()
        {
            base.OnEditValueChanged();
            if (Bounds.IsEmpty)
            {
                this.requireUpdateImage = true;
                return;
            }
            UpdateImage();
        }
        public override void CalcViewInfo(Graphics g)
        {
            if (requireUpdateImage || OwnerEdit != null && OwnerEdit.Image != null) UpdateImage();
            base.CalcViewInfo(g);
        }
        public override void Reset()
        {
            base.Reset();
            this.image = null;
        }

        void UpdateImage()
        {
            this.requireUpdateImage = false;
            this.image = GetImageCore(EditValue);
        }
        Image GetImageCore(object editValue)
        {
            object ret = null;
            if (OwnerEdit != null) ret = OwnerEdit.Image;
            if (ret != null) return (Image)ret;

            if (Item != null && editValue != nullValue)
                ret = Item.GetImage(new ImageByValueEventArgs(editValue));
            if (ret == null) return null;
            if (ret is Image) return (Image)ret;
            return ByteImageConverter.FromByteArray(ByteImageConverter.ToByteArray(ret));
        }
        public Image Image
        {
            get
            {
                return image;
            }
        }
    }
    public class SearchLookUpEditWithGlyph : SearchLookUpEdit
    {
        Image image;
        static SearchLookUpEditWithGlyph()
        {
            RepositoryItemSearchLookUpEditWithGlyph.RegisterSearchLookUpEditWithGlyph();
        }
        public override string EditorTypeName { get { return "SearchLookUpEditWithGlyph"; } }
        protected override void Dispose(bool disposing)
        {
            if (disposing && image != null) image.Dispose();
            this.image = null;
            base.Dispose(disposing);
        }
        [DefaultValue(null)]
        public Image Image
        {
            get { return image; }
            set
            {
                image = value;
                LayoutChanged();
            }
        }
    }
    public delegate void ImageByValueEventHandler(object sender, ImageByValueEventArgs e);
    public class ImageByValueEventArgs : EventArgs
    {
        object value;
        object image;
        public ImageByValueEventArgs(object value)
        {
            this.value = value;
        }
        public object Value { get { return value; } }
        public object Image
        {
            get { return image; }
            set { image = value; }
        }
    }
}
