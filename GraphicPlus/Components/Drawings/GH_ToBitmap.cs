using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace GraphicPlus.Components.Drawings
{
    public class GH_ToBitmap : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_Viewer class.
        /// </summary>
        public GH_ToBitmap()
          : base("Drawing to Bitmap", "DrawingToBmp",
              "Create a Bitmap file of a Drawing.",
              "Display", "Graphics")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.quinary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Drawing", "D", "A Graphic Plus Drawing object", GH_ParamAccess.item);
            pManager.AddIntegerParameter("PPI", "S", "The pixel per inch value acts as a scalar multiplier. Must be 96 or above", GH_ParamAccess.item, 96);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bitmap", "B", "A Bitmap Object of the Drawing. (System.Drawing.Bitmap)", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            Drawing drawing = new Drawing();
            if (!DA.GetData<Drawing>(0, ref drawing)) return;

            int dpi = 96;
            DA.GetData(1, ref dpi);
            if (dpi < 96) dpi = 96;

            DA.SetData(0, drawing.ToBitmap(dpi));
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
                return Properties.Resources.GP_Bitmap_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("afda75ca-c2fa-424c-83e5-56fed962ce6b"); }
        }
    }
}