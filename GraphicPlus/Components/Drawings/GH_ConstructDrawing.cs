using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GraphicPlus.Components.Drawings
{
    public class GH_ConstructDrawing : GH_BaseGraphics
    {
        /// <summary>
        /// Initializes a new instance of the GH_ConstructDrawing class.
        /// </summary>
        public GH_ConstructDrawing()
          : base("Construct Drawing", "Drawing",
              "Constructs a Drawing from a list of Shapes",
              "Display", "Graphics")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.quarternary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {

            pManager.AddGenericParameter("Shapes / Geometry", "S", "A list of Graphic Plus Shapes, or Curves, Breps, Meshes", GH_ParamAccess.list);
            pManager.AddRectangleParameter("Boundary", "B", "An optional frame for the drawing. If blank, the shapes bounding box will be used", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Width", "W", "The width of the output drawing", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Height", "H", "The height of the output drawing", GH_ParamAccess.item);
            pManager[3].Optional = true;
            pManager.AddColourParameter("Color", "C", "An optional background color", GH_ParamAccess.item, Color.Transparent);
            pManager[4].Optional = true;
            pManager.AddIntegerParameter("Units", "U", "An optional background color", GH_ParamAccess.item, 0);
            pManager[5].Optional = true;

            Param_Integer paramA = (Param_Integer)pManager[5];
            foreach (Drawing.DocumentUnits value in Enum.GetValues(typeof(Drawing.DocumentUnits))) paramA.AddNamedValue(value.ToString(), (int)value);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Drawing", "D", "A Graphic Plus Drawing Object", GH_ParamAccess.item);
            pManager.AddRectangleParameter("Boundary", "B", "The bounding rectangle", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            this.AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "NOTE: Rhino Previews are limited and do not directly match final outputs."
                + Environment.NewLine + "For an accurate preview use the in canvas viewer components");

            List<Shape> shapes = new List<Shape>();
            List <IGH_Goo> goos = new List<IGH_Goo>();
            if (!DA.GetDataList(0, goos)) return;

            BoundingBox box = BoundingBox.Unset;

            foreach (IGH_Goo goo in goos)
            {
                Shape shape = null;
                if (goo.TryGetShape(ref shape))
                {
                    box.Union(shape.GetBoundingBox());
                    shapes.Add(shape);
                }
            }

            Rectangle3d boundary = Rectangle3d.Unset;
            if(!DA.GetData(1, ref boundary))
            {
                boundary = new Rectangle3d(Plane.WorldXY, new Point3d(box.Min.X, box.Min.Y, 0), new Point3d(box.Max.X, box.Max.Y, 0));
            }

            double width = 0;
            DA.GetData(2, ref width);
            if (width <= 0) width = boundary.Width;

            double height = 0;
            DA.GetData(3, ref height);
            if (height <= 0) height = boundary.Height;

            Color background = Color.Transparent;
            DA.GetData(4, ref background);

            int units = 0;
            DA.GetData(5, ref units);

            Drawing drawing = new Drawing(shapes, boundary, width, height)
            {
                Background = background,
                Units = (Drawing.DocumentUnits)units
        }; 

            DA.SetData(0, drawing);
            DA.SetData(1, boundary);
            SetPreview(drawing);
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
                return Properties.Resources.GP_DrawingA_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("f3220ce3-0aeb-41b4-bfb9-435838423791"); }
        }
    }
}