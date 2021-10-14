using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicPlus
{
    public class GradientStop
    {

        #region members

        protected double parameter = 0.0;
        protected Color color = Color.Black;

        #endregion

        #region constructors

        public GradientStop()
        {

        }

        public GradientStop(double parameter, Color color)
        {
            this.parameter = parameter;
            this.color = color;
        }

        public GradientStop(GradientStop stop)
        {
            this.parameter = stop.parameter;
            this.color = stop.color;
        }

        #endregion

        #region properties

        public virtual double Parameter
        {
            get { return parameter; }
        }

        public virtual Color Color
        {
            get { return color; }
        }

        #endregion

        #region properties

        public void Set(double parameter, Color color)
        {
            this.parameter = parameter;
            this.color = color;
        }

        #endregion

        #region methods

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 13;
                hash = (hash * 7) + (this.parameter.GetHashCode());
                hash = (hash * 7) + (this.color.GetHashCode());
                return hash;
            }
        }

        #endregion

        #region overrides

        public override string ToString()
        {
            return "Gradient Stop (P:" + Math.Round(this.parameter, 2).ToString() + " C:" + this.color.ToString() + ")";
        }

        #endregion

    }
}
