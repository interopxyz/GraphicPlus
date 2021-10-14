using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicPlus
{
    public class Effect
    {

        #region members

        public enum EffectTypes { None, Blur, Shadow };
        protected EffectTypes effectType = EffectTypes.None;

        protected double radius = 0.0;
        protected double distance = 0.0;
        protected double angle = 0.0;

        #endregion

        #region constructors

        public Effect()
        {

        }

        public Effect(Effect effect)
        {
            this.effectType = effect.effectType;
            this.radius = effect.radius;
            this.distance = effect.distance;
            this.angle = effect.angle;
        }

        public Effect(double radius)
        {
            this.radius = radius;
            this.effectType = EffectTypes.Blur;
        }

        public Effect(double radius, double distance, double angle)
        {
            this.radius = radius;
            this.distance = distance;
            this.angle = angle;
            this.effectType = EffectTypes.Shadow;
        }

        #endregion

        #region properties

        public virtual double Radius
        {
            get { return radius; }
        }

        public virtual double Distance
        {
            get { return distance; }
        }

        public virtual double Angle
        {
            get { return angle; }
        }

        public virtual EffectTypes EffectType
        {
            get { return effectType; }
        }

        #endregion

        #region methods

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 13;
                hash = (hash * 7) + (this.effectType.GetHashCode());
                hash = (hash * 7) + (this.radius.GetHashCode());
                hash = (hash * 7) + (this.distance.GetHashCode());
                hash = (hash * 7) + (this.angle.GetHashCode());
                return hash;
            }
        }

        #endregion

    }
}
