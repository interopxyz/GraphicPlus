using System;
using Sd = System.Drawing;
using Sw = System.Windows;
using Sh = System.Windows.Shapes;
using Sm = System.Windows.Media;
using Si = System.Windows.Media.Imaging;
using Se = System.Windows.Media.Effects;

using System.Linq;

using Rg = Rhino.Geometry;
using System.IO;
using System.Collections.Generic;

using gp = GraphicPlus;

namespace GraphicPlus
{
    public static class ToDotNet
    {

        #region To Windows Point

        /// <summary>
        /// Returns a Windows Point from a Rhino.Geometry.Point3d
        /// </summary>
        /// <param name="input">Rhinocommon Point3d</param>
        /// <returns>System Windows Point</returns>
        public static Sw.Point ToWindowsPoint(this Rg.Point3d input)
        {
            return new Sw.Point(input.X, input.Y);
        }

        /// <summary>
        /// Returns a Windows Point from a Rhino.Geometry.Point3f
        /// </summary>
        /// <param name="input">Rhinocommon Point3f</param>
        /// <returns>System Windows Point</returns>
        public static Sw.Point ToWindowsPoint(this Rg.Point3f input)
        {
            return new Sw.Point(input.X, input.Y);
        }

        /// <summary>
        /// Returns a Windows Point from a Rhino.Geometry.Point2d
        /// </summary>
        /// <param name="input">Rhinocommon Point2d</param>
        /// <returns>System Windows Point</returns>
        public static Sw.Point ToWindowsPoint(this Rg.Point2d input)
        {
            return new Sw.Point(input.X, input.Y);
        }

        /// <summary>
        /// Returns a Windows Point from a Rhino.Geometry.Point2f
        /// </summary>
        /// <param name="input">Rhinocommon Point2f</param>
        /// <returns>System Windows Point</returns>
        public static Sw.Point ToWindowsPoint(this Rg.Point2f input)
        {
            return new Sw.Point(input.X, input.Y);
        }

        #endregion

        #region To Windows Vector

        /// <summary>
        /// Returns a Windows Vector from a Rhino.Geometry.Vector3d
        /// </summary>
        /// <param name="input">Rhinocommon Vector3d</param>
        /// <returns>System Windows Vector</returns>
        public static Sw.Vector ToWindowsVector(this Rg.Vector3d input)
        {
            return new Sw.Vector(input.X, input.Y);
        }

        /// <summary>
        /// Returns a Windows Point from a Rhino.Geometry.Vector3f
        /// </summary>
        /// <param name="input">Rhinocommon Vector3f</param>
        /// <returns>System Windows Vector</returns>
        public static Sw.Vector ToWindowsVector(this Rg.Vector3f input)
        {
            return new Sw.Vector(input.X, input.Y);
        }

        /// <summary>
        /// Returns a Windows Point from a Rhino.Geometry.Vector2d
        /// </summary>
        /// <param name="input">Rhinocommon Vector2d</param>
        /// <returns>System Windows Vector</returns>
        public static Sw.Vector ToWindowsVector(this Rg.Vector2d input)
        {
            return new Sw.Vector(input.X, input.Y);
        }

        /// <summary>
        /// Returns a Windows Point from a Rhino.Geometry.Vector2f
        /// </summary>
        /// <param name="input">Rhinocommon Vector2f</param>
        /// <returns>System Windows Vector</returns>
        public static Sw.Vector ToWindowsVector(this Rg.Vector2f input)
        {
            return new Sw.Vector(input.X, input.Y);
        }

        #endregion

        #region To Windows Media Curve Geometry

        /// <summary>
        /// Returns a Windows Media Line from a Rhinocommon Line
        /// </summary>
        /// <param name="input">Rhinocommon Line</param>
        /// <returns>System Windows Media Line</returns>
        public static Sm.LineGeometry ToGeometry(this Rg.Line input)
        {
            return new Sm.LineGeometry(input.From.ToWindowsPoint(), input.To.ToWindowsPoint());
        }

        /// <summary>
        /// Returns a Windows Media Rect from a Rhinocommon Rectangle
        /// </summary>
        /// <param name="input">Rhinocommon Rectangle</param>
        /// <returns>System Windows Media Rect</returns>
        public static Sm.RectangleGeometry ToGeometry(this Rg.Rectangle3d input)
        {
            Sw.Point center = input.Center.ToWindowsPoint();
            double angle = Rg.Vector3d.VectorAngle(Rg.Vector3d.YAxis, input.Plane.YAxis, Rg.Plane.WorldXY);
            Sm.Transform xform = new Sm.RotateTransform(angle, center.X, center.Y);

            Sw.Rect rect = new Sw.Rect(center.X - input.Width / 2, center.Y + input.Height / 2, input.Width, input.Height);
            return new Sm.RectangleGeometry(rect, 0, 0, xform);
        }

        /// <summary>
        /// Returns a Windows Media Ellipse from a Rhinocommon Circle
        /// </summary>
        /// <param name="input">Rhinocommon Circle</param>
        /// <returns>System Windows Media Circle</returns>
        public static Sm.EllipseGeometry ToGeometry(this Rg.Circle input)
        {
            return new Sm.EllipseGeometry(input.Center.ToWindowsPoint(), input.Radius, input.Radius);
        }

        /// <summary>
        /// Returns a Windows Media Ellipse from a Rhinocommon Ellipse
        /// </summary>
        /// <param name="input">Rhinocommon Ellipse</param>
        /// <returns>System Windows Media Ellipse</returns>
        public static Sm.EllipseGeometry ToGeometry(this Rg.Ellipse input)
        {
            Sw.Point origin = input.Plane.Origin.ToWindowsPoint();
            double angle = Rg.Vector3d.VectorAngle(Rg.Vector3d.YAxis, input.Plane.YAxis, Rg.Plane.WorldXY);
            angle = angle / System.Math.PI * 180;
            Sm.Transform xform = new Sm.RotateTransform(angle, origin.X, origin.Y);

            return new Sm.EllipseGeometry(origin, input.Radius1, input.Radius2, xform);
        }

        /// <summary>
        /// Returns a Windows Media Path Geometry from a Rhinocommon Arc
        /// </summary>
        /// <param name="input">Rhinocommon Arc</param>
        /// <returns>System Windows Media Path Geometry</returns>
        public static Sm.PathGeometry ToGeometry(this Rg.Arc input)
        {
            Sm.ArcSegment arc = new Sm.ArcSegment();
            Sm.PathFigure figure = new Sm.PathFigure();
            Sm.PathGeometry geometry = new Sm.PathGeometry();
            Sm.PathFigureCollection figureCollection = new Sm.PathFigureCollection();
            Sm.PathSegmentCollection segmentCollection = new Sm.PathSegmentCollection();

            figure.StartPoint = input.StartPoint.ToWindowsPoint();

            arc.Point = input.EndPoint.ToWindowsPoint();
            arc.Size = new Sw.Size(input.Radius, input.Radius);
            if (Rg.Vector3d.VectorAngle(input.Plane.Normal, Rg.Vector3d.ZAxis) > 0) { arc.SweepDirection = Sm.SweepDirection.Counterclockwise; } else { arc.SweepDirection = Sm.SweepDirection.Clockwise; }
            arc.IsLargeArc = (input.Angle > Math.PI);

            segmentCollection.Add(arc);
            figure.Segments = segmentCollection;
            figureCollection.Add(figure);
            geometry.Figures = figureCollection;

            return geometry;
        }

        /// <summary>
        /// Returns a Windows Media Path Geometry from a Rhinocommon Polyline
        /// </summary>
        /// <param name="input">Rhinocommon Polyline</param>
        /// <returns>System Windows Media Path Geometry</returns>
        public static Sm.PathGeometry ToGeometry(this Rg.Polyline input)
        {
            Sm.PathFigure figure = new Sm.PathFigure();
            Sm.PathGeometry geometry = new Sm.PathGeometry();
            Sm.PathFigureCollection figureCollection = new Sm.PathFigureCollection();
            Sm.PathSegmentCollection segmentCollection = new Sm.PathSegmentCollection();

            figure.StartPoint = input[0].ToWindowsPoint();
            for (int i = 1; i < input.Count; i++)
            {
                Sm.LineSegment line = new Sm.LineSegment(input[i].ToWindowsPoint(), true);
                segmentCollection.Add(line);
            }

            figure.Segments = segmentCollection;
            figure.IsClosed = input.IsClosed;
            figureCollection.Add(figure);
            geometry.Figures = figureCollection;

            return geometry;
        }

        /// <summary>
        /// Returns a Windows Media Bezier Spline Path Geometry from a Rhinocommon Curve
        /// </summary>
        /// <param name="input">Rhinocommon Curve</param>
        /// <returns>System Windows Media Bezier Curve Path Geometry </returns>
        public static Sm.PathGeometry ToGeometry(this Rg.Curve input)
        {
            Rg.NurbsCurve nurbsCurve = input.ToNurbsCurve();
            nurbsCurve.MakePiecewiseBezier(true);
            Rg.BezierCurve[] bezier = Rg.BezierCurve.CreateCubicBeziers(nurbsCurve, 0, 0);

            Sm.PathFigure figure = new Sm.PathFigure();
            Sm.PathGeometry geometry = new Sm.PathGeometry();
            Sm.PathFigureCollection figureCollection = new Sm.PathFigureCollection();
            Sm.PathSegmentCollection segmentCollection = new Sm.PathSegmentCollection();

            figure.StartPoint = bezier[0].GetControlVertex3d(0).ToWindowsPoint();
            for (int i = 0; i < bezier.Count(); i++)
            {
                Sm.BezierSegment segment = new Sm.BezierSegment(bezier[i].GetControlVertex3d(1).ToWindowsPoint(), bezier[i].GetControlVertex3d(2).ToWindowsPoint(), bezier[i].GetControlVertex3d(3).ToWindowsPoint(), true);
                segmentCollection.Add(segment);
            }

            figure.Segments = segmentCollection;
            figure.IsClosed = input.IsClosed;
            figureCollection.Add(figure);
            geometry.Figures = figureCollection;

            return geometry;
        }

        #endregion

        #region To Windows Media Path

        /// <summary>
        /// Returns a Windows Media Line from a Rhinocommon Line
        /// </summary>
        /// <param name="input">Rhinocommon Vector3d</param>
        /// <returns>System Windows Vector</returns>
        public static Sh.Path ToPath(this Rg.Line input)
        {
            Sh.Path path = new Sh.Path
            {
                Data = input.ToGeometry()
            };

            return path;
        }

        /// <summary>
        /// Returns a Windows Shapes Path from a Rhinocommon Circle
        /// </summary>
        /// <param name="input">Rhinocommon Circle</param>
        /// <returns>System Windows Shapes Path</returns>
        public static Sh.Path ToPath(this Rg.Circle input)
        {
            Sh.Path path = new Sh.Path
            {
                Data = input.ToGeometry()
            };

            return path;
        }

        /// <summary>
        /// Returns a Windows Shapes Path from a Rhinocommon Ellipse
        /// </summary>
        /// <param name="input">Rhinocommon Ellipse</param>
        /// <returns>System Windows Shapes Path</returns>
        public static Sh.Path ToPath(this Rg.Ellipse input)
        {
            Sh.Path path = new Sh.Path
            {
                Data = input.ToGeometry()
            };

            return path;
        }

        /// <summary>
        /// Returns a Windows Media Path from a Rhinocommon Arc
        /// </summary>
        /// <param name="input">Rhinocommon Arc</param>
        /// <returns>System Windows Shapes Path</returns>
        public static Sh.Path ToPath(this Rg.Arc input)
        {
            Sh.Path path = new Sh.Path
            {
                Data = input.ToGeometry()
            };

            return path;
        }

        /// <summary>
        /// Returns a Windows Shapes Path from a Rhinocommon Rectangle
        /// </summary>
        /// <param name="input">Rhinocommon Rectangle</param>
        /// <returns>System Windows Shapes Path</returns>
        public static Sh.Path ToPath(this Rg.Rectangle3d input)
        {
            Sh.Path path = new Sh.Path
            {
                Data = input.ToGeometry()
            };

            return path;
        }

        /// <summary>
        /// Returns a Windows Shapes Path from a Rhinocommon Polyline
        /// </summary>
        /// <param name="input">Rhinocommon Polyline</param>
        /// <returns>System Windows Shapes Path</returns>
        public static Sh.Path ToPath(this Rg.Polyline input)
        {
            Sh.Path path = new Sh.Path
            {
                Data = input.ToGeometry()
            };

            return path;
        }

        /// <summary>
        /// Returns a Windows Media Shapes Bezier Path from a Rhinocommon Curve
        /// </summary>
        /// <param name="input">Rhinocommon Curve</param>
        /// <returns>System Windows Shapes Path</returns>
        public static Sh.Path ToPath(this Rg.Curve input)
        {
            Sh.Path path = new Sh.Path
            {
                Data = input.ToGeometry()
            };

            return path;
        }

        #endregion

        #region To Geometry Visual

        public static Sm.DrawingVisual ToVisualDrawing(this Shape input, gp.Drawing drawing)
        {
            Rg.Rectangle3d boundary = drawing.Boundary;
            double Scale = drawing.Scale;
            double X = drawing.TranslateX;
            double Y = drawing.TranslateY;

            Rg.Plane plane = Rg.Plane.WorldZX;
            plane.Origin = boundary.Center;
            Rg.Vector3d vector = new Rg.Vector3d(-boundary.Corner(0));
            vector.X -= X / Scale;
            vector.Y -= Y / Scale;

            Sm.DrawingVisual drawingVisual = new Sm.DrawingVisual();
            Sm.DrawingContext drawingContext = drawingVisual.RenderOpen();
            Sm.GeometryGroup dwg = new Sm.GeometryGroup();

            if(input.PathType != Shape.PathTypes.Text)
            { 
                for (int i = 0; i < input.Curves.Count;i++)
            {
                Rg.NurbsCurve curve = input.Curves[i].DuplicateCurve().ToNurbsCurve();
                curve.Transform(Rg.Transform.Mirror(plane));
                curve.Translate(vector);
                curve.Transform(Rg.Transform.Scale(new Rg.Point3d(0, 0, 0), Scale));

                Sm.Geometry geometry = curve.ToGeometry(input.CurveTypeSet[i]);
                dwg.Children.Add(geometry);
                }

            drawingContext.DrawGeometry(input.Graphics.ToMediaBrush(), input.Graphics.ToMediaPen(), dwg);
            }
            else
            {
                Rg.NurbsCurve line = input.Curves[0].DuplicateCurve().ToNurbsCurve();
                line.Transform(Rg.Transform.Mirror(plane));
                line.Translate(vector);
                line.Transform(Rg.Transform.Scale(new Rg.Point3d(0, 0, 0), Scale));

                Sw.Point point = line.PointAtStart.ToWindowsPoint();

                Sm.FormattedText text = new Sm.FormattedText(
                    input.TextContent,
                    System.Globalization.CultureInfo.GetCultureInfo("en-us"),
                    Sw.FlowDirection.LeftToRight,
                    new Sm.Typeface(input.Graphics.Font.Family),
                    input.Graphics.Font.Size,
                    new Sm.SolidColorBrush(input.Graphics.FillColor.ToMediaColor()));
                if(input.Graphics.Font.IsBold)text.SetFontWeight(Sw.FontWeights.Bold);
                if (input.Graphics.Font.IsItalic) text.SetFontStyle(Sw.FontStyles.Italic);
                if (input.Graphics.Font.IsUnderlined) text.SetTextDecorations(Sw.TextDecorations.Underline);


                switch (input.Graphics.Font.Justification)
                {
                    case FontObject.Justifications.BottomMiddle:
                    case FontObject.Justifications.CenterMiddle:
                    case FontObject.Justifications.TopMiddle:
                        text.TextAlignment = Sw.TextAlignment.Center;
                        break;
                    case FontObject.Justifications.BottomRight:
                    case FontObject.Justifications.CenterRight:
                    case FontObject.Justifications.TopRight:
                        text.TextAlignment = Sw.TextAlignment.Right;
                        break;
                }

                double height = 0;
                switch (input.Graphics.Font.Justification)
                {
                    case FontObject.Justifications.BottomLeft:
                    case FontObject.Justifications.BottomMiddle:
                    case FontObject.Justifications.BottomRight:
                        height = text.Height;
                        break;
                    case FontObject.Justifications.CenterLeft:
                    case FontObject.Justifications.CenterMiddle:
                    case FontObject.Justifications.CenterRight:
                        height = text.Height/2.0;
                        break;
                }

                Sm.TranslateTransform xformM = new Sm.TranslateTransform(0, -height);
                
                double angle = Rg.Vector3d.VectorAngle(input.TextPlane.XAxis, Rg.Vector3d.XAxis, Rg.Plane.WorldXY) / Math.PI * 180.00;
                Sm.RotateTransform xformR = new Sm.RotateTransform(angle, point.X, point.Y);

                drawingContext.PushTransform(xformR);
                drawingContext.PushTransform(xformM);

                drawingContext.DrawText(text, point);
                drawingContext.Pop();
            }

            drawingContext.Close();

            switch (input.Graphics.PostEffect.EffectType)
            {
                case Effect.EffectTypes.Blur:
                    drawingVisual.Effect = input.Graphics.PostEffect.ToMediaBlurEffect();
                    break;
                case Effect.EffectTypes.Shadow:
                    drawingVisual.Effect = input.Graphics.PostEffect.ToMediaShadowEffect();
                    break;
                case Effect.EffectTypes.OuterGlow:
                    drawingVisual.Effect = input.Graphics.PostEffect.ToMediaOuterGlowEffect();
                    break;
            }

            return drawingVisual;
        }

        public static Sm.DrawingVisual ToGeometryVisual(this gp.Drawing input)
        {
            Sm.DrawingVisual drawing = new Sm.DrawingVisual();
            Rg.Point3d origin = input.Boundary.Corner(0);
            double rX = input.Boundary.Width * input.Scale;
            double rY = input.Boundary.Height * input.Scale;
            origin.X -= (input.Width / rX);
            origin.Y += (input.Height / rY);

            Sm.DrawingVisual background = new Sm.DrawingVisual();
            Sm.DrawingContext bgC = background.RenderOpen();
            Sw.Rect rect = new Sw.Rect(0, 0, input.Width, input.Height);
            bgC.DrawRectangle(new Sm.SolidColorBrush(input.Background.ToMediaColor()),null, new Sw.Rect(0, 0, input.Width, input.Height));
            bgC.Close();
            
            drawing.Children.Add(background);

            foreach (Shape shape in input.Shapes)
            {
                drawing.Children.Add(shape.ToVisualDrawing(input));
            }

            return drawing;
        }

        public static Sm.Geometry ToGeometry(this Rg.NurbsCurve curve, Shape.CurveTypes type)
        {
            Sm.Geometry geometry = null;
            switch (type)
            {
                case  Shape.CurveTypes.Circle:
                    Rg.Circle circle = new Rg.Circle();
                    curve.TryGetCircle(out circle);
                    geometry = circle.ToGeometry();
                    break;
                case Shape.CurveTypes.Ellipse:
                    Rg.Ellipse ellipse = new Rg.Ellipse();
                    curve.TryGetEllipse(out ellipse);
                    geometry = ellipse.ToGeometry();
                    break;
                case Shape.CurveTypes.Polyline:
                    Rg.Polyline polyline = new Rg.Polyline();
                    curve.TryGetPolyline(out polyline);
                    geometry = polyline.ToGeometry();
                    break;
                default:
                    geometry = curve.ToGeometry();
                    break;
            }
            return geometry;
        }

        #endregion

        #region Media Graphics

        public static Sm.Color ToMediaColor(this Sd.Color input)
        {
            return Sm.Color.FromArgb((byte)input.A, (byte)input.R, (byte)input.G, (byte)input.B);
        }

        public static Sm.Pen ToMediaPen(this Graphic input)
        {
            List<double> pattern = new List<double>();
            foreach(double item in input.Pattern)
            {
                pattern.Add(item / input.Weight);
            }

            Sm.Pen pen = new Sm.Pen(new Sm.SolidColorBrush(input.StrokeColor.ToMediaColor()), input.Weight);

            if (input.Pattern.Count>0) pen.DashStyle = new Sm.DashStyle(pattern, 0);

            pen.DashCap = Sm.PenLineCap.Flat;
            pen.StartLineCap = Sm.PenLineCap.Flat;
            pen.EndLineCap = Sm.PenLineCap.Flat;

            return pen;
        }

        public static Sm.Brush ToMediaBrush(this Graphic input)
        {
            Sm.Brush brush;
            switch (input.FillType)
            {
                default:
                    brush = new Sm.SolidColorBrush(input.FillColor.ToMediaColor());
                    break;
                case Graphic.FillTypes.LinearGradient:
                    brush = input.FillGradient.ToMediaLinearGradientBrush();
                    break;
                case Graphic.FillTypes.RadialGradient:
                    brush = input.FillGradient.ToMediaRadialGradientBrush();
                    break;
            }

            return brush;
        }

        public static Sm.LinearGradientBrush ToMediaLinearGradientBrush(this Gradient input)
        {
            Sm.LinearGradientBrush brush = new Sm.LinearGradientBrush();

            foreach (GradientStop gradStop in input.Stops)
            {
                brush.GradientStops.Add(new Sm.GradientStop(gradStop.Color.ToMediaColor(), gradStop.Parameter));
            }

            brush.SpreadMethod = Sm.GradientSpreadMethod.Pad;
            brush.MappingMode = Sm.BrushMappingMode.RelativeToBoundingBox;

            double radians = (270-input.Angle) / 180.0*Math.PI;
            double XA = (0.5 + Math.Sin(radians) * 0.5);
            double YA = (0.5 + Math.Cos(radians) * 0.5);
            double XB = (0.5 + Math.Sin(radians + Math.PI) * 0.5);
            double YB = (0.5 + Math.Cos(radians + Math.PI) * 0.5);

            brush.StartPoint = new Sw.Point(XA, YA);
            brush.EndPoint = new Sw.Point(XB, YB);

            return brush;
        }

        public static Sm.RadialGradientBrush ToMediaRadialGradientBrush(this Gradient input)
        {
            Sm.RadialGradientBrush brush = new Sm.RadialGradientBrush();

            foreach (GradientStop gradStop in input.Stops)
            {
                brush.GradientStops.Add(new Sm.GradientStop(gradStop.Color.ToMediaColor(), gradStop.Parameter));
            }

            brush.SpreadMethod = Sm.GradientSpreadMethod.Pad;
            brush.MappingMode = Sm.BrushMappingMode.RelativeToBoundingBox;

            brush.Center = new Sw.Point(input.X, input.Y);

            return brush;
        }

        #endregion

        #region Drawing to Bitmap

        // Bitmap from Drawing
        public static Sd.Bitmap ToBitmap(this gp.Drawing drawing)
        {
            return drawing.ToGeometryVisual().ToBitmap(drawing.Width, drawing.Height, drawing.Dpi, new Si.PngBitmapEncoder());
        }

        // Bitmap from Visual, Width, and Height
        public static Sd.Bitmap ToBitmap(this Sm.DrawingVisual drawing, double width, double height)
        {
            return drawing.ToBitmap(width, height, 96, new Si.PngBitmapEncoder());
        }

        // Bitmap from Visual, Width, Height, Encoder
        public static Sd.Bitmap ToBitmap(this Sm.DrawingVisual drawing, double width, double height, int dpi, Si.BitmapEncoder encoder)
        {

            var bitmap = new Si.RenderTargetBitmap((int)(width / 96 * dpi), (int)(height / 96 * dpi), dpi, dpi, Sm.PixelFormats.Pbgra32);

            bitmap.Render(drawing);

            MemoryStream stream = new MemoryStream();
            encoder.Frames.Add(Si.BitmapFrame.Create(bitmap));
            encoder.Save(stream);

            return new Sd.Bitmap(stream);
        }

        #endregion

        #region Media Effect

        public static Se.BlurEffect ToMediaBlurEffect(this Effect input)
        {
            Se.BlurEffect output = new Se.BlurEffect
            {
                Radius = input.Radius
            };
            return output;
        }

        public static Se.DropShadowEffect ToMediaOuterGlowEffect(this Effect input)
        {
            Se.DropShadowEffect output = new Se.DropShadowEffect()
            {
                BlurRadius = input.Radius,
                Direction = input.Angle + 90,
                ShadowDepth = input.Distance,
                Color = input.Color.ToMediaColor()
            };
            return output;
        }

        public static Se.DropShadowEffect ToMediaShadowEffect(this Effect input)
        {
            Se.DropShadowEffect output = new Se.DropShadowEffect
            {
                BlurRadius = input.Radius,
                Direction = input.Angle + 90,
                ShadowDepth = 0
            };

            return output;
        }

        #endregion

    }
}
