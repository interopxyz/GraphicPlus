using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rd = Rhino.DocObjects;

namespace GraphicPlus
{
    public class FontObject
    {

        #region members

        public enum Justifications { BottomLeft, BottomMiddle, BottomRight, CenterLeft, CenterMiddle, CenterRight, TopLeft, TopMiddle, TopRight };

        protected string fontFamily = "Arial";
        protected double fontSize = 8;

        protected bool isBold = false;
        protected bool isItalic = false;
        protected bool isUnderlined = false;

        protected Justifications justification = Justifications.BottomLeft;

        #endregion

        #region constructors

        public FontObject()
        {

        }

        public FontObject(FontObject fontObject)
        {
            this.fontFamily = fontObject.fontFamily;
            this.fontSize = fontObject.fontSize;

            this.isBold = fontObject.isBold;
            this.isItalic = fontObject.isItalic;
            this.isUnderlined = fontObject.isUnderlined;

            this.justification = fontObject.justification;
        }

        public FontObject(string family, double size)
        {
            this.fontFamily = family;
            this.fontSize = size;
        }

        public FontObject(string family, double size, bool bold, bool italic, bool underline)
        {
            this.fontFamily = family;
            this.fontSize = size;

            this.isBold = bold;
            this.isItalic = italic;
            this.isUnderlined = underline;
        }

        public FontObject(string family, double size, Justifications justification)
        {
            this.fontFamily = family;
            this.fontSize = size;

            this.justification = justification;
        }

        public FontObject(string family, double size, bool bold, bool italic, bool underline, Justifications justification)
        {
            this.fontFamily = family;
            this.fontSize = size;

            this.isBold = bold;
            this.isItalic = italic;
            this.isUnderlined = underline;

            this.justification = justification;
        }


        #endregion

        #region properties

        public virtual string Family
        {
            get { return this.fontFamily; }
        }

        public virtual double Size
        {
            get { return this.fontSize; }
        }

        public virtual bool IsBold
        {
            get { return this.isBold; }
        }

        public virtual bool IsItalic
        {
            get { return this.isItalic; }
        }

        public virtual bool IsUnderlined
        {
            get { return this.isUnderlined; }
        }

        public virtual Justifications Justification
        {
            get { return this.justification; }
        }

        public virtual Rd.TextHorizontalAlignment RhHorizontalAlignment
        {
            get 
            { 
                Rd.TextHorizontalAlignment align = Rd.TextHorizontalAlignment.Left;

                switch (this.justification)
                {
                    case Justifications.BottomMiddle:
                    case Justifications.CenterMiddle:
                    case Justifications.TopMiddle:
                        align = Rd.TextHorizontalAlignment.Center;
                        break;
                    case Justifications.BottomRight:
                    case Justifications.CenterRight:
                    case Justifications.TopRight:
                        align = Rd.TextHorizontalAlignment.Right;
                        break;
                }

                return align;
            }
        }

        public virtual Rd.TextVerticalAlignment RhVerticalAlignment
        {
            get
            {
                Rd.TextVerticalAlignment align = Rd.TextVerticalAlignment.BottomOfBoundingBox;

                switch (this.justification)
                {
                    case Justifications.CenterLeft:
                    case Justifications.CenterMiddle:
                    case Justifications.CenterRight:
                        align = Rd.TextVerticalAlignment.Middle;
                        break;
                    case Justifications.TopLeft:
                    case Justifications.TopMiddle:
                    case Justifications.TopRight:
                        align = Rd.TextVerticalAlignment.Top;
                        break;
                }

                return align;
            }
        }

        #endregion

        #region methods

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 13;
                hash = (hash * 7) + (this.fontFamily.GetHashCode());
                hash = (hash * 7) + (this.fontSize.GetHashCode());
                hash = (hash * 7) + (this.isBold.GetHashCode());
                hash = (hash * 7) + (this.isItalic.GetHashCode());
                hash = (hash * 7) + (this.isUnderlined.GetHashCode());
                hash = (hash * 7) + (this.justification.GetHashCode());
                return hash;
            }
        }

        #endregion

    }
}
