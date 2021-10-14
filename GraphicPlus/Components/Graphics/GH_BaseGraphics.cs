using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GraphicPlus.Components
{
    public abstract class GH_BaseGraphics : GH_Component
    {
        protected List<Shape> prevShapes = new List<Shape>();

        /// <summary>
        /// Initializes a new instance of the GH_BaseGraphics class.
        /// </summary>
        public GH_BaseGraphics()
          : base("GH_BaseGraphics", "Nickname",
              "Description",
              "Category", "Subcategory")
        {
        }

        public GH_BaseGraphics(string Name, string NickName, string Description, string Category, string Subcategory) : base(Name, NickName, Description, Category, Subcategory)
        {
        }

        protected override void BeforeSolveInstance()
        {
            prevShapes = new List<Shape>();
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }

        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            double mTol = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
            double aTol = Rhino.RhinoDoc.ActiveDoc.ModelAngleToleranceRadians;
            //args.Display.ZBiasMode = Rhino.Display.ZBiasMode.TowardsCamera;

            // Surfaces
            foreach (Shape shape in prevShapes)
            {
                if (shape.Graphics.FillType != Graphic.FillTypes.None) {
                    Hatch[] hatches = Hatch.Create(shape.Curves, 0, 0, 1, mTol);

                    foreach(Hatch hatch in hatches)
                    {
                        args.Display.DrawHatch(hatch, shape.Graphics.FillColor, Color.Transparent);
                    }

                }

                Transform xform = args.Viewport.GetTransform(Rhino.DocObjects.CoordinateSystem.World, Rhino.DocObjects.CoordinateSystem.Screen);
                foreach (Curve curve in shape.Curves)
                {
                    //Polyline polyline = curve.ToPolyline(mTol, aTol, mTol*100, 1000000).ToPolyline();
                    //Line[] lines = polyline.GetSegments();
                    //foreach(Line line in lines)
                    //{

                    //    Point3d ptA = new Point3d(line.From);
                    //    ptA.Transform(xform);

                    //    Point3d ptB = new Point3d(line.To);
                    //    ptB.Transform(xform);

                    //    args.Display.Draw2dLine(new PointF((float)ptA.X, (float)ptA.Y), new PointF((float)ptB.X, (float)ptB.Y), shape.Graphics.StrokeColor, (float)(shape.Graphics.Weight*2.0));
                    //}

                    args.Display.DrawCurve(curve, shape.Graphics.StrokeColor, (int)shape.Graphics.Weight);
                }
            }


            // Set Display Override
            base.DrawViewportWires(args);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("99a9d69f-fc5a-4f37-bcb6-720f66bc373d"); }
        }
    }
}