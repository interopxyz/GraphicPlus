using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicPlus
{
    public class Graphic
    {
        #region members

        public enum FillTypes { None,Solid,LinearGradient,RadialGradient};
        protected FillTypes fillType = FillTypes.None;

        protected Color strokeColor = Color.Black;
        protected double weight = 1.0;
        protected List<double> pattern = new List<double>();

        protected Color fillColor = Color.Transparent;
        protected Gradient gradient = new Gradient();

        protected Effect effect = new Effect();

        protected Guid guid = Guid.Empty;
        #endregion

        #region constructors

        public Graphic()
        {
            guid = Guid.NewGuid();
        }

        public Graphic(Graphic graphic)
        {
            this.guid = graphic.guid;

            this.strokeColor = graphic.strokeColor;
            this.weight = graphic.weight;
            this.pattern = graphic.pattern;

            this.fillType = graphic.fillType;
            this.fillColor = graphic.fillColor;
            this.gradient = new Gradient(graphic.gradient);
            this.effect = new Effect(graphic.effect);
        }

        public Graphic(Color fillColor, Color strokeColor)
        {
            guid = Guid.NewGuid();

            this.fillColor = fillColor;
            this.strokeColor = strokeColor;
        }

        #endregion

        #region properties

        public virtual string Id
        {
            get { return guid.ToString(); }
        }

        public virtual FillTypes FillType
        {
            get { return fillType; }
        }

        public virtual Color StrokeColor
        {
            get { return strokeColor; }
        }

        public virtual double Weight
        {
            get { return weight; }
        }

        public virtual List<double> Pattern
        {
            get { return pattern; }
        }

        public virtual Color FillColor
        {
            get { return fillColor; }
        }

        public virtual Gradient FillGradient
        {
            get { return gradient; }
        }

        public virtual Effect PostEffect
        {
            get { return effect; }
        }

        #endregion

        #region methods

        public void SetStroke(Color color, double weight, List<double> pattern = null)
        {
            this.strokeColor = color;
            this.weight = weight;

            if (pattern != null)
            {
                if(pattern.Count>0)
                {
                    this.pattern = pattern;
                }
            }
        }

        public void SetSolidFill(Color color)
        {
            this.fillType = FillTypes.Solid;
            this.fillColor = color;
        }

        public void SetLinearGradient(List<double> parameters, List<Color> colors, double angle =0)
        {
            this.fillType = FillTypes.LinearGradient;
            this.gradient = new Gradient(parameters, colors,angle);
        }

        public void SetRadialGradient(List<double> parameters, List<Color> colors, double x = 0.5, double y = 0.5)
        {
            this.fillType = FillTypes.RadialGradient;
            this.gradient = new Gradient(parameters, colors, x,y);
        }

        public void SetBlur(double radius)
        {
            this.effect = new Effect(radius);
        }

        public void SetShadow(double radius, double distance, double angle)
        {
            this.effect = new Effect(radius,distance,angle);
        }

        #endregion
    }
}
