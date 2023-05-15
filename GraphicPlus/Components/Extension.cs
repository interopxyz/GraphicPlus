using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gp = GraphicPlus;


namespace GraphicPlus
{
    public static class Extension
    {
        public static bool TryGetDrawings(this IGH_Goo input, ref gp.Drawing drawing)
        {
            bool status = false;
            gp.Drawing newDrawing;
            if (input.CastTo<gp.Drawing>(out newDrawing))
            {
                drawing.MergeDrawing(newDrawing);
                status = true;
            }
            else
            {
                gp.Shape shape = null;
                if (input.TryGetShape(ref shape))
                {
                    drawing.MergeDrawing(new gp.Drawing(shape));
                    status = true;
                }
            }

            return status;
        }


        public static Rectangle3d ToRectangle(this BoundingBox input)
        {
            return new Rectangle3d(Plane.WorldXY, new Point3d(input.Min.X, input.Min.Y, 0), new Point3d(input.Max.X, input.Max.Y, 0));
        }

        public static string StripExtension(this string name)
        {
            string[] parts = name.Split('.');
            if (parts.Count() > 1)
            {
                string extension = parts[parts.Count() - 1];
                extension = extension.ToLower();
                bool hasExtension = false;
                switch (extension)
                {
                    case "png":
                        hasExtension = true;
                        break;
                    case "jpg":
                        hasExtension = true;
                        break;
                    case "jpeg":
                        hasExtension = true;
                        break;
                    case "tif":
                        hasExtension = true;
                        break;
                    case "tiff":
                        hasExtension = true;
                        break;
                    case "bmp":
                        hasExtension = true;
                        break;
                }

                if (hasExtension) parts = parts.Take(parts.Length - 1).ToArray();

                return String.Join(".", parts);
            }
            return name;
        }

        public static bool TryGetShape(this IGH_Goo goo, ref Shape shape)
        {
            bool isValid = false;

            if (goo.CastTo<Shape>(out shape))
            {
                shape = new Shape(shape);
                isValid = true;
            }
            else
            {
                switch (goo.TypeName)
                {
                    case "NurbsCurve":
                        NurbsCurve nCurve;
                        if (goo.CastTo<NurbsCurve>(out nCurve))
                        {
                            shape = new Shape(nCurve);
                            isValid = true;
                        }
                        break;
                    case "Curve":
                        Curve curve;
                        if (goo.CastTo<Curve>(out curve))
                        {
                            shape = new Shape(curve);
                            isValid = true;
                        }
                        break;
                    case "Arc":
                        Arc arc;
                        if (goo.CastTo<Arc>(out arc))
                        {
                            shape = new Shape(arc.ToNurbsCurve());
                            isValid = true;
                        }
                        break;
                    case "Circle":
                        Circle circle;
                        if (goo.CastTo<Circle>(out circle))
                        {
                            shape = new Shape(circle);
                            isValid = true;
                        }
                        break;
                    case "Ellipse":
                        Ellipse ellipse;
                        if (goo.CastTo<Ellipse>(out ellipse))
                        {
                            shape = new Shape(ellipse);
                            isValid = true;
                        }
                        break;
                    case "Line":
                        Line line;
                        if (goo.CastTo<Line>(out line))
                        {
                            shape = new Shape(line.ToNurbsCurve());
                            isValid = true;
                        }
                        break;
                    case "Rectangle":
                        Rectangle3d rect;
                        if (goo.CastTo<Rectangle3d>(out rect))
                        {
                            shape = new Shape(rect.ToPolyline());
                            isValid = true;
                        }
                        break;
                    case "Surface":
                        Surface surface;
                        if (goo.CastTo<Surface>(out surface))
                        {
                            Brep srfBrep = Brep.CreateFromSurface(surface);
                            if (goo.CastTo<Brep>(out srfBrep)) shape = new Shape(srfBrep.DuplicateBrep());
                            shape = new Shape(srfBrep.DuplicateBrep());
                            isValid = true;
                        }
                        break;
                    case "Brep":
                        Brep brep;
                        if (goo.CastTo<Brep>(out brep))
                        {
                            shape = new Shape(brep.DuplicateBrep());
                            isValid = true;
                        }
                        break;
                    case "Mesh":
                        Mesh mesh;
                        if (goo.CastTo<Mesh>(out mesh))
                        {
                            shape = new Shape(mesh.DuplicateMesh());
                            isValid = true;
                        }
                        break;
                }
            }

            return isValid;
        }
    }
}
