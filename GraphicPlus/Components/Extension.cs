using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicPlus.Components
{
    public static class Extension
    {
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

        public static Shape ToShape(this IGH_Goo goo)
        {
            Shape shape = null;
            if (goo.CastTo<Shape>(out shape))
            {
                shape = new Shape(shape);
            }
            else
            {
                switch (goo.TypeName)
                {
                    case "NurbsCurve":
                        NurbsCurve nCurve = null;
                        if (goo.CastTo<NurbsCurve>(out nCurve)) shape = new Shape(nCurve);
                        break;
                    case "Curve":
                        Curve curve = null;
                        if (goo.CastTo<Curve>(out curve)) shape = new Shape(curve);
                        break;
                    case "Arc":
                        Arc arc = new Arc();
                        if (goo.CastTo<Arc>(out arc)) shape = new Shape(arc.ToNurbsCurve());
                        break;
                    case "Circle":
                        Circle circle = new Circle();
                        if (goo.CastTo<Circle>(out circle)) shape = new Shape(circle);
                        break;
                    case "Ellipse":
                        Ellipse ellipse = new Ellipse();
                        if (goo.CastTo<Ellipse>(out ellipse)) shape = new Shape(ellipse);
                        break;
                    case "Line":
                        Line line = new Line();
                        if (goo.CastTo<Line>(out line)) shape = new Shape(line.ToNurbsCurve());
                        break;
                    case "Rectangle":
                        Rectangle3d rect = new Rectangle3d();
                        if (goo.CastTo<Rectangle3d>(out rect)) shape = new Shape(rect.ToPolyline());
                        break;
                    case "Surface":
                        Surface surface = null;
                        if (goo.CastTo<Surface>(out surface)) {
                            Brep srfBrep = Brep.CreateFromSurface(surface);
                            if (goo.CastTo<Brep>(out srfBrep)) shape = new Shape(srfBrep.DuplicateBrep());
                        }
                        break;
                    case "Brep":
                        Brep brep = new Brep();
                        if (goo.CastTo<Brep>(out brep)) shape = new Shape(brep.DuplicateBrep());
                        break;
                    case "Mesh":
                        Mesh mesh = new Mesh();
                        if (goo.CastTo<Mesh>(out mesh)) shape = new Shape(mesh.DuplicateMesh());
                        break;
                }
            }

            return shape;
        }
    }
}
