using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicPlus
{
    public class Gradient
    {
        #region members

        protected List<GradientStop> stops = new List<GradientStop>();
        public double Angle = 0;

        protected double x = 0.5;
        protected double y = 0.5;

        #endregion

        #region constructors

        public Gradient()
        {
            stops.Add(new GradientStop(0, Color.White));
            stops.Add(new GradientStop(1, Color.Black));
        }

        public Gradient(Gradient gradient)
        {
            foreach(GradientStop stop in gradient.Stops)
            {
                this.stops.Add(new GradientStop(stop));
            }
            this.Angle = gradient.Angle;
            this.x = gradient.x;
            this.y = gradient.y;
        }

        public Gradient(List<double> parameters, List<Color> colors, double angle = 0)
        {
            int count = parameters.Count;
            for (int i = colors.Count;i<count;i++)
            {
                colors.Add(colors[colors.Count - 1]);
            }

            for (int i = 0; i < count; i++)
            {
                stops.Add(new GradientStop(parameters[i], colors[i]));
            }
            this.Angle = angle;
        }

        public Gradient(List<double> parameters, List<Color> colors, double x = 0, double y = 0)
        {
            int count = parameters.Count;
            for (int i = colors.Count; i < count; i++)
            {
                colors.Add(colors[colors.Count - 1]);
            }

            for (int i = 0; i < count; i++)
            {
                stops.Add(new GradientStop(parameters[i], colors[i]));
            }
            this.X = x;
            this.Y = y;
        }

        #endregion

        #region properties

        public virtual List<GradientStop> Stops
        {
            get { return stops; }
        }

        public virtual List<double> Parameters
        {
            get
            {
                List<double> parameters = new List<double>();
                foreach (GradientStop stop in this.stops)
                {
                    parameters.Add(stop.Parameter);
                }
                return parameters;
            }
        }

        public virtual List<Color> Colors
        {
            get {
                List<Color> colors = new List<Color>();
                foreach(GradientStop stop in this.stops)
                {
                    colors.Add(stop.Color);
                }
                return colors; 
            }
        }

        public virtual double X
        {
            get { return x; }
            set {
                if (value > 1)
                {
                    x = 1;
                }
                else if( value<0)
                {
                    x = 0;
                }
                else
                {
                    x = value;
                }
            }
        }

        public virtual double Y
        {
            get { return y; }
            set
            {
                if (value > 1)
                {
                    y = 1;
                }
                else if (value < 0)
                {
                    y = 0;
                }
                else
                {
                    y = value;
                }
            }
        }

        #endregion

        #region methods

        #region methods

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 13;
                hash = (hash * 7) + (this.x.GetHashCode());
                hash = (hash * 7) + (this.y.GetHashCode());
                foreach(GradientStop stp in stops)
                {
                    hash = (hash * 7) + (stp.GetHashCode());
                }
                return hash;
            }
        }

        #endregion

        #endregion

        #region overrides

        public override string ToString()
        {
            return "Gradient (S:" +stops.Count + ")";
        }

        #endregion


    }
}
