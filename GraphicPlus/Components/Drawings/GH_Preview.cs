using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace GraphicPlus.Components
{
    public class GH_Preview : GH_BaseGraphics
    {
        /// <summary>
        /// Initializes a new instance of the GH_Preview class.
        /// </summary>
        public GH_Preview()
          : base("Shape Preview Beta", "ShpPrev",
              "A beta fill and stroke preview in Rhino."+Environment.NewLine+ "WARNING: This does not reflect most of the graphic settings in Graphic Plus." + Environment.NewLine + "For an accurate preview use the in canvas viewer components",
              "Display", "Graphics")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary | GH_Exposure.obscure; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Drawing", "D", "A Drawing object", GH_ParamAccess.item);
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
            Drawing drawing = new Drawing();
            IGH_Goo goo = null;
            if (!DA.GetData(0, ref goo)) return;
            if (goo.CastTo<Drawing>(out drawing))
            {
                drawing = new Drawing(drawing);
            }

            foreach(Shape shape in drawing.Shapes)
            {
                prevShapes.Add(shape);
            }

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
                return Properties.Resources.GP_Preview_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("35f36ea6-aee3-498e-9aaf-6028fed9e74f"); }
        }
    }
}