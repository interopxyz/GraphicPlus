using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicPlus
{
    public static class ToSvg
    {

        #region compound

        public static string ToSubScript(this Circle input, int digits = 4)
        {
            StringBuilder output = new StringBuilder();
            Point3d center = input.Plane.Origin;
            double X = Math.Round(center.X, digits);
            double Y = Math.Round(center.Y, digits);
            double R = Math.Round(input.Radius, digits);

            output.Append("M " + X + " " + (Y - R) + " ");
            output.Append("A " + R + " " + R + " 0 1 0 " + X + " " + (Y + R) + " ");
            output.Append("A " + R + " " + R + " 0 1 0 " + X + " " + (Y - R) + " ");
            output.Append("Z ");

            return output.ToString();
        }

        public static string ToSubScript(this Ellipse input, int digits = 4)
        {
            return input.ToNurbsCurve().ToSubScript();
            //StringBuilder output = new StringBuilder();
            //Point3d center = input.Plane.Origin;
            //double X = Math.Round(center.X, digits);
            //double Y = Math.Round(center.Y, digits);
            //double RA = Math.Round(input.Radius1/2.0, digits);
            //double RB = Math.Round(input.Radius2/2.0, digits);

            //output.Append("M " + X + " " + (Y - RA) + " ");
            //output.Append("A " + RA + " " + RB + " 0 1 0 " + X + " " + (Y + RA) + " ");
            //output.Append("A " + RA + " " + RB + " 0 1 0 " + X + " " + (Y - RA) + " ");
            //output.Append("Z ");

            //return output.ToString();
        }

        public static string ToSubScript(this Polyline input, int digits = 4)
        {
            StringBuilder output = new StringBuilder();

            output.Append("M "+input[0].ToScript());
            for (int i = 1; i < input.Count; i++)
            {
                output.Append("L " + input[i].ToScript());
            }
            output.Append("Z ");

            return output.ToString();
        }

        public static string ToSubScript(this NurbsCurve input, int digits = 4)
        {
            StringBuilder output = new StringBuilder();

            double mt = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
            BezierCurve[] beziers = BezierCurve.CreateCubicBeziers(input, mt, mt);

            output.Append("M " + input.PointAtStart.ToScript());
            foreach (BezierCurve bezier in beziers)
            {
                output.Append(" C " + bezier.GetControlVertex3d(1).ToScript() + bezier.GetControlVertex3d(2).ToScript() + bezier.GetControlVertex3d(3).ToScript());
            }
            output.Append("Z ");

            return output.ToString();
        }
        #endregion

        #region geometry

        public static string ToScript(this Circle input, string id, int digits = 4)
        {
            StringBuilder output = new StringBuilder();
            Point3d center = input.Plane.Origin;
            double X = Math.Round(center.X,digits);
            double Y = Math.Round(center.Y, digits);
            double R = Math.Round(input.Radius, digits);

            output.Append("<circle id=\"" + id + "\" ");
            output.Append("cx=\"" + X + "\" ");
            output.Append("cy=\"" + Y + "\" ");
            output.Append("r=\"" + R + "\" ");

            return output.ToString();
        }

        public static string ToScript(this Ellipse input, string id, int digits = 4)
        {
            StringBuilder output = new StringBuilder();
            double angle = Math.Round(Vector3d.VectorAngle(input.Plane.XAxis, Vector3d.XAxis, Plane.WorldXY) / Math.PI* 180.00, 6);

            double X = Math.Round(input.Plane.Origin.X, digits);
            double Y = Math.Round(input.Plane.Origin.Y, digits);

            output.Append("<ellipse id=\"" + id + "\" ");
            output.Append("cx=\"" + X + "\" ");
            output.Append("cy=\"" + Y + "\" ");
            output.Append("rx=\"" + Math.Round(input.Radius1, digits) + "\" ");
            output.Append("ry=\"" + Math.Round(input.Radius2, digits) + "\" ");
            output.Append("transform = \"rotate(-"+angle+" "+X+" "+Y+")\"");

            return output.ToString();
        }

        public static string ToScript(this Polyline input, string id, int digits = 4)
        {
            StringBuilder output = new StringBuilder();

                     output.Append("<polyline id=\"" + id + "\" ");
            output.Append("points=\"");
            foreach(Point3d point in input)
            {
                output.Append(point.ToScript());
            }
            output.Append("\" ");

            return output.ToString();
        }

        public static string ToScript(this NurbsCurve input, string id, int digits = 4)
        {
            StringBuilder output = new StringBuilder();

            double mt = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
            BezierCurve[] beziers = BezierCurve.CreateCubicBeziers(input, mt, mt);

            output.Append("<path id =\""+id+"\" ");
            output.Append("d = \" ");
            output.Append("M " + input.PointAtStart.ToScript());
            foreach (BezierCurve bezier in beziers)
            {
                output.Append(" C " + bezier.GetControlVertex3d(1).ToScript() + bezier.GetControlVertex3d(2).ToScript() + bezier.GetControlVertex3d(3).ToScript());
            }
            output.Append("\" ");

            return output.ToString();
        }

        public static string ToScript(this Point3d input, int digits = 4)
        {
            return Math.Round(input.X, digits) + " " + Math.Round(input.Y, digits) + " ";
        }

        #endregion

        #region text

        public static string ToTextScript(this Plane input, string content, string id, int digits = 4)
        {
            StringBuilder output = new StringBuilder();
            double angle = Math.Round(Vector3d.VectorAngle(input.XAxis, Vector3d.XAxis, Plane.WorldXY) / Math.PI * 180.00, 6);

            double X = Math.Round(input.Origin.X, digits);
            double Y = Math.Round(input.Origin.Y, digits);

            output.Append("<text id=\"" + id + "\" ");
            output.Append("x=\"" + X + "\" ");
            output.Append("y=\"" + Y + "\" ");
            output.Append("transform = \"rotate(" + angle + " " + X + " " + Y + ")\"");

            return output.ToString();

        }

        #endregion

        #region graphics

        public static string ToSVG(this Color input)
        {
            return ColorTranslator.ToHtml(input)+" ";
        }

        public static string ToScript(this Graphic input, int digits = 6, bool isText = false)
        {
            StringBuilder output = new StringBuilder();

            output.Append("<style>");
            output.Append(" .cls-" + input.GetHashCode() + "{");
            if ((input.StrokeColor.A > 0) & (input.Weight != 0))
            {
                output.Append("stroke:" + input.StrokeColor.ToSVG() + "; ");
                output.Append("stroke-opacity:" + Math.Round(input.StrokeColor.A / 255.0, digits) + "; ");
                output.Append("stroke-width:" + input.Weight + "; ");
                if (input.Pattern.Count>0) output.Append("stroke-dasharray:" + String.Join(", ",input.Pattern) + "; ");
            }
            else
            {
                output.Append("stroke:none; ");
            }

            switch (input.EndCap)
            {
                case Graphic.Caps.Square:
                    output.Append("stroke-linecap:square; ");
                    output.Append("stroke-linejoin:bevel; ");
                    break;
                case Graphic.Caps.Round:
                    output.Append("stroke-linecap:round; ");
                    output.Append("stroke-linejoin:round; ");
                    break;
                default:
                    break;
            }

            switch (input.FillType)
            {
                case Graphic.FillTypes.None:
                case Graphic.FillTypes.Solid:
                    if (input.FillColor.A > 0)
                    {
                        output.Append("fill:" + input.FillColor.ToSVG() + "; ");
                        output.Append("fill-opacity:" + Math.Round(input.FillColor.A / 255.0, digits) + "; ");
                    }
                    else
                    {
                        output.Append("fill:none; ");
                    }
                    break;
            }

            if (isText)
            {
                output.Append("font-family: " + input.Font.Family + "; ");
                output.Append("font-size:" + input.Font.Size + "px; ");

                if (input.Font.IsBold) output.Append("font-weight:bold; ");
                if (input.Font.IsItalic) output.Append("font-style:italic; ");
                if (input.Font.IsUnderlined) output.Append("text-decoration:underline; ");

                switch (input.Font.Justification)
                {
                    case FontObject.Justifications.BottomMiddle:
                    case FontObject.Justifications.CenterMiddle:
                    case FontObject.Justifications.TopMiddle:
                        output.Append("text-anchor:middle; ");
                        break;
                    case FontObject.Justifications.BottomRight:
                    case FontObject.Justifications.CenterRight:
                    case FontObject.Justifications.TopRight:
                        output.Append("text-anchor:end; ");
                        break;
                }

                switch (input.Font.Justification)
                {
                    case FontObject.Justifications.TopLeft:
                    case FontObject.Justifications.TopMiddle:
                    case FontObject.Justifications.TopRight:
                        output.Append("dominant-baseline:hanging; ");
                        break;
                    case FontObject.Justifications.CenterLeft:
                    case FontObject.Justifications.CenterMiddle:
                    case FontObject.Justifications.CenterRight:
                        output.Append("dominant-baseline:middle; ");
                        break;
                }
            }

            output.Append("}");
            output.Append(" </style>");

            return output.ToString();
        }

        public static string GetLinearGradientScript(this Graphic input, int digits =4)
        {
            StringBuilder output = new StringBuilder();

            Gradient gradient = input.FillGradient;

            output.Append("<linearGradient id=\"gr-" + input.GetHashCode() + "\" ");
            output.Append("gradientTransform=\"rotate("+ Math.Round(gradient.Angle,digits) + ")\" ");
            output.AppendLine("gradientUnits=\"objectBoundingBox\" >");

            for (int i = 0; i < gradient.Colors.Count; i++)
            {
                output.AppendLine("<stop offset=\"" + (gradient.Stops[i].Parameter * 100.0) + "%\" style=\"stop-color:" + gradient.Stops[i].Color.ToSVG() + "; stop-opacity:" + (gradient.Stops[i].Color.A / 255.0) + "\" />");
            }

            output.AppendLine("</linearGradient>");

            return output.ToString();
        }

        public static string GetRadialGradientScript(this Graphic input, int digits = 4)
        {
            StringBuilder output = new StringBuilder();

            Gradient gradient = input.FillGradient;

            output.Append("<radialGradient id=\"gr-" + input.GetHashCode() + "\" ");
            output.Append("gradientTransform=\"rotate(" + Math.Round(gradient.Angle, digits) + ")\" ");
            output.Append("cx=\"" + Math.Round(gradient.X * 100,digits) + "%\" cy=\"" + Math.Round(gradient.Y * 100, digits) + "%\" ");
            output.AppendLine("gradientUnits=\"objectBoundingBox\" >");

            for (int i = 0; i < gradient.Colors.Count; i++)
            {
                output.AppendLine("<stop offset=\"" + (gradient.Stops[i].Parameter * 100.0) + "%\" style=\"stop-color:" + gradient.Stops[i].Color.ToSVG() + "; stop-opacity:" + (gradient.Stops[i].Color.A / 255.0) + "\" />");
            }

            output.AppendLine("</radialGradient>");

            return output.ToString();
        }

        #endregion

        #region effects

        public static string ToSVGEffect(this Graphic input)
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("<filter id=\"ef-" + input.GetHashCode() + "\" x=\"-50%\" width=\"200%\" y =\"-50%\" height=\"200%\" >");
            switch (input.PostEffect.EffectType)
            {
                case Effect.EffectTypes.Blur:
                    output.AppendLine(input.PostEffect.GetBlurEffect());
                    break;
                case Effect.EffectTypes.Shadow:
                    output.AppendLine(input.PostEffect.GetShadowEffect());
                    break;
                case Effect.EffectTypes.InnerGlow:
                    output.AppendLine(input.PostEffect.GetInnerGlowEffect());
                    break;
                case Effect.EffectTypes.OuterGlow:
                    output.AppendLine(input.PostEffect.GeOuterGlowEffect());
                    break;
            }
            output.Append("</filter>");
            return output.ToString();
        }

        public static string GetInnerGlowEffect(this Effect input)
        {
            return "<feFlood flood-color=\""+input.Color.ToSVG() + "\"/>" +
            " <feComposite in2=\"SourceAlpha\" operator= \"out\" />" + 
                " <feGaussianBlur stdDeviation=\""+ Math.Round(input.Radius / 3.0, 4) + "\" result =\"blur\" />"+
                " <feComposite operator=\"atop\" in2=\"SourceGraphic\"/>";
        }

        public static string GetBlurEffect(this Effect input)
        {
            return "<feGaussianBlur result=\"blurOut\" in=\"SourceGraphic\" stdDeviation=\"" + Math.Round(input.Radius/3.0,4) + "\" />";
        }

        public static string GeOuterGlowEffect(this Effect input)
        {
            return
            "<feFlood flood-color=\"" + input.Color.ToSVG() + "\"  result =\"fillOut\"/>" +
            " <feComposite in=\"fillOut\"  in2=\"SourceAlpha\" operator= \"in\" result =\"clipOut\" />"+
            "<feOffset result=\"offOut\" in=\"clipOut\" dx=\"0\" dy=\"0\"/>" +
            "<feGaussianBlur in=\"offOut\" stdDeviation=\"" + Math.Round(input.Radius/3.0 , 4) + "\" result =\"blurOut\" />" +
            "<feMerge> "+
            "<feMergeNode in=\"blurOut\" />"+
            "<feMergeNode in=\"blurOut\" />" +
            "<feMergeNode in=\"SourceGraphic\" />" +
            "</feMerge>";
        }

        public static string GetShadowEffect(this Effect input)
        {
            double radians = (input.Angle + 180) / 180 * Math.PI;
            string output = "<feOffset result=\"offOut\" in=\"SourceAlpha\" dx=\""+ Math.Round(input.Distance * Math.Sin(radians), 4) + "\" dy=\"" + Math.Round(input.Distance * Math.Cos(radians), 4) + "\" />" +
                "<feGaussianBlur in=\"offOut\" stdDeviation=\"" + Math.Round(input.Radius / 3.0, 4) + "\" result =\"blurOut\" />" +
                "<feBlend in=\"SourceGraphic\" in2=\"blurOut\" mode=\"normal\" />" + Environment.NewLine;
            return output;
        }

        #endregion
    }
}
