using GH_IO.Serialization;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicPlus
{
    public class Shape
    {

        #region members

        protected string id = Guid.NewGuid().ToString();

        public enum PathTypes { None, Path, CompoundPath, Text };

        public string Hyperlink = string.Empty;
        public Dictionary<string, string> Data = new Dictionary<string, string>();
        public bool HasTitle = false;

        public enum CurveTypes { None, Polyline, Circle, Ellipse, Curve };

        protected PathTypes pathType = PathTypes.None;

        public Graphic Graphics = new Graphic();

        protected List<CurveTypes> curveTypes = new List<CurveTypes>();
        protected List<NurbsCurve> curves = new List<NurbsCurve>();

        protected string textContent = string.Empty;
        protected Plane textPlane = Plane.Unset;
        protected List<Hatch> hatches = new List<Hatch>();
        private readonly double mTol = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance * 100;

        #endregion

        #region constructors

        public Shape()
        {
        }

        public Shape(Shape shape)
        {
            this.Data = shape.Data;
            this.Hyperlink = shape.Hyperlink;
            this.HasTitle = shape.HasTitle;
            this.pathType = shape.pathType;
            this.curveTypes = shape.curveTypes;
            this.curves = shape.curves;
            this.Graphics = new Graphic(shape.Graphics);
            this.id = shape.id;
            this.textContent = shape.textContent;
            this.textPlane = shape.textPlane;
            this.hatches = shape.hatches;
        }

        public Shape(string text, Plane plane)
        {
            this.textContent = text;
            this.textPlane = plane;
            this.curves.Add(new Line(plane.Origin, plane.Origin + new Vector3d(1, 0, 0)).ToNurbsCurve());
            this.pathType = PathTypes.Text;
            this.Graphics.SetStroke(System.Drawing.Color.Transparent, 0);
            this.Graphics.SetSolidFill(System.Drawing.Color.Black);
        }

        public Shape(NurbsCurve curve, Graphic graphic)
        {
            this.pathType = PathTypes.Path;
            this.Graphics = new Graphic(graphic);
            ParseCurves(curve);
        }

        public Shape(NurbsCurve curve)
        {
            this.pathType = PathTypes.Path;
            ParseCurves(curve);
        }

        public Shape(Curve curve)
        {
            this.pathType = PathTypes.Path;
            ParseCurves(curve.ToNurbsCurve());
        }

        public Shape(Circle circle)
        {
            this.pathType = PathTypes.Path;
            ParseCurves(circle);
        }
        public Shape(Ellipse ellipse)
        {
            this.pathType = PathTypes.Path;
            ParseCurves(ellipse);
        }
        public Shape(Polyline polyline)
        {
            this.pathType = PathTypes.Path;
            ParseCurves(polyline);
        }

        public Shape(Brep brep)
        {
            this.pathType = PathTypes.CompoundPath;
            foreach (BrepLoop loop in brep.Loops)
            {
                ParseCurves(loop.To3dCurve().ToNurbsCurve());
            }
        }

        public Shape(Mesh mesh)
        {
            this.pathType = PathTypes.CompoundPath;
            Polyline[] polylines = mesh.GetNakedEdges();
            foreach (Polyline polyline in polylines)
            {
                curves.Add(polyline.ToNurbsCurve());
                curveTypes.Add(CurveTypes.Polyline);
            }
        }

        #endregion

        #region properties

        public virtual PathTypes PathType
        {
            get { return pathType; }
        }

        public virtual List<NurbsCurve> Curves
        {
            get { return curves; }
        }

        public virtual List<CurveTypes> CurveTypeSet
        {
            get { return curveTypes; }
        }

        public virtual Plane TextPlane
        {
            get { return textPlane; }
        }

        public virtual string TextContent
        {
            get { return textContent; }
        }

        public virtual bool HasData
        {
            get { return this.Data.Count > 0; }
        }

        public virtual bool HasLink
        {
            get { return this.Hyperlink!=string.Empty; }
        }

        public virtual List<Hatch> Hatches
        {
            get { return hatches; }
        }

        #endregion

        #region methods

        public void SetId(string id)
        {
            this.id = id;
        }

        protected void ParseCurves(Circle circle)
        {
            this.curveTypes.Add(CurveTypes.Circle);
            this.curves.Add(circle.ToNurbsCurve());
        }

        protected void ParseCurves(Ellipse ellipse)
        {
            this.curveTypes.Add(CurveTypes.Ellipse);
            this.curves.Add(ellipse.ToNurbsCurve());
        }

        protected void ParseCurves(Polyline polyline)
        {
            this.curveTypes.Add(CurveTypes.Polyline);
            this.curves.Add(polyline.ToNurbsCurve());
        }

        protected void ParseCurves(NurbsCurve nurbsCurve)
        {
            if (nurbsCurve.IsCircle())
            {
                this.curveTypes.Add(CurveTypes.Circle);
            }
            else if (nurbsCurve.IsEllipse())
            {
                this.curveTypes.Add(CurveTypes.Ellipse);
            }
            else if (nurbsCurve.IsPolyline())
            {
                this.curveTypes.Add(CurveTypes.Polyline);
            }
            else
            {
                this.curveTypes.Add(CurveTypes.Curve);
            }
            this.curves.Add(nurbsCurve);
                hatches = Hatch.Create(curves, 0, 0, 1, mTol).ToList();
        }

        public BoundingBox GetBoundingBox()
        {
            BoundingBox boundingBox = BoundingBox.Unset;
            if(pathType == PathTypes.Text)
            {
                boundingBox.Union(textPlane.Origin);
            }
            else
            { 
            foreach (NurbsCurve curve in curves)
            {
                boundingBox.Union(curve.GetBoundingBox(true));
            }
            }
            return boundingBox;
        }

        #endregion

        #region svg

        public string ToScript(Rectangle3d boundary, double scale = 1.0)
        {
            StringBuilder output = new StringBuilder();
            Plane plane = Plane.WorldZX;
            plane.Origin = boundary.Center;
            string close = "path";
            string flatData = "";
            if (this.HasLink) output.AppendLine("<a xlink:href=\"" + this.Hyperlink + "\" target=\"_blank\">");
            switch (pathType)
            {
                case PathTypes.Path:

                    NurbsCurve curve = curves[0].DuplicateCurve().ToNurbsCurve();
                    curve.Transform(Transform.Mirror(plane));
                    curve.Translate(new Vector3d(-boundary.Corner(0)));
                    curve.Transform(Transform.Scale(new Point3d(0, 0, 0), scale));

                    switch (curveTypes[0])
                    {
                        case CurveTypes.Circle:
                            if (curve.IsClosed)
                            {
                                Circle circle = Circle.Unset;
                                if (curve.TryGetCircle(out circle)) output.Append(circle.ToScript(id));
                                close = "circle";
                            }
                            else
                            {
                                output.Append(curve.ToScript(id));
                            }
                            break;
                        case CurveTypes.Ellipse:
                            if (curve.IsClosed)
                            {
                                Ellipse ellipse = new Ellipse();
                                if (curve.TryGetEllipse(out ellipse)) output.Append(ellipse.ToScript(id)); 
                                close = "ellipse";
                            }
                            else
                            {
                                output.Append(curve.ToScript(id));
                            }
                            break;
                        case CurveTypes.Polyline:
                            Polyline polyline = new Polyline();
                            if (curve.TryGetPolyline(out polyline)) output.Append(polyline.ToScript(id));
                            close = "polyline";
                            break;
                        case CurveTypes.Curve:
                            output.Append(curve.ToScript(id));
                            break;
                    }
                    break;
                case PathTypes.CompoundPath:
                    output.Append("<path id=\"" + id + "\" d =\"");
                    for (int i = 0; i < curves.Count; i++)
                    {
                        NurbsCurve nurbs = curves[i].DuplicateCurve().ToNurbsCurve();
                        nurbs.Transform(Transform.Mirror(plane));
                        nurbs.Translate(new Vector3d(-boundary.Corner(0)));
                        nurbs.Transform(Transform.Scale(new Point3d(0, 0, 0), scale));
                        switch (curveTypes[i])
                        {
                            case CurveTypes.Circle:
                                Circle circle = Circle.Unset;
                                if (nurbs.TryGetCircle(out circle)) output.Append(circle.ToSubScript());
                                break;
                            case CurveTypes.Ellipse:
                                Ellipse ellipse = new Ellipse();
                                if (nurbs.TryGetEllipse(out ellipse))
                                {
                                    output.Append(ellipse.ToSubScript());
                                }
                                break;
                            case CurveTypes.Polyline:
                                Polyline polyline = new Polyline();
                                if (nurbs.TryGetPolyline(out polyline)) output.Append(polyline.ToSubScript());
                                break;
                            case CurveTypes.Curve:
                                output.Append(nurbs.ToSubScript());
                                break;
                        }
                    }
                    output.Append("\" fill-rule=\"evenodd\"");
                    break;
                case PathTypes.Text:
                    Plane frame = new Plane(textPlane);
                    NurbsCurve temp = curves[0].DuplicateCurve().ToNurbsCurve();
                    temp.Transform(Transform.Mirror(plane));
                    temp.Translate(new Vector3d(-boundary.Corner(0)));
                    temp.Transform(Transform.Scale(new Point3d(0, 0, 0), scale));

                    frame.Origin = temp.PointAtStart;
                    output.Append(frame.ToTextScript(textContent, id));
                    close = "text";
                    break;
            }

            if (this.HasData)
            {
                List<string> tempText = new List<string>();
                foreach(string key in this.Data.Keys)
                {
                    string keyVal = key.Replace(';', '-');
                    keyVal = keyVal.Replace(' ', '_');
                    keyVal = keyVal.ToLower();
                    output.Append(" data-"+ keyVal +"=\""+this.Data[key]+"\"");
                    tempText.Add(key + " | " + this.Data[key]);
                }
                flatData = string.Join("\n", tempText);
            }

            output.Append(" class=\"cls-" + this.Graphics.GetHashCode() + "\" ");
            if (pathType == PathTypes.Text)
            {
                output.Append("> " + this.textContent);
            }
            else
            {
                if ((this.Graphics.FillType == Graphic.FillTypes.LinearGradient) | (this.Graphics.FillType == Graphic.FillTypes.RadialGradient)) output.Append("fill=\"url('#gr-" + this.Graphics.GetHashCode() + "')\" ");
                if ((this.Graphics.PostEffect.EffectType != Effect.EffectTypes.None)) output.Append("filter = \"url(#ef-" + this.Graphics.GetHashCode() + ")\" ");
                output.Append("> ");
            }
            if (this.HasTitle & this.HasData) output.Append(" <title>"+ flatData + "</title>");
            output.Append(" </" + close + ">");
            if (this.HasLink) output.AppendLine("</a>");
            return output.ToString();
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            string message = "Shape | Empty";
            switch (pathType)
            {
                case PathTypes.None:
                    break;
                case PathTypes.Path:
                    message = "Shape | Path: " + curveTypes[0].ToString();
                    break;
                case PathTypes.CompoundPath:
                    message = "Shape | Compound: " + curveTypes.Count + " Paths";
                    break;
            }

            return message;
        }

        #endregion

    }
}
