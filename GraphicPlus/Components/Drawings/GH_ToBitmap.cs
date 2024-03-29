﻿using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
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
            get { return GH_Exposure.senary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Drawings / Shapes / Geometry", "D", "A list of Graphic Plus Drawing, Shapes, or Geometry (Curves, Breps, Meshes).", GH_ParamAccess.list);
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
            List<IGH_Goo> goos = new List<IGH_Goo>();
            if (!DA.GetDataList(0, goos)) return;

            foreach (IGH_Goo goo in goos)
            {
                goo.TryGetDrawings(ref drawing);
            }


            int dpi = 96;
            if (DA.GetData(1, ref dpi)) drawing.Dpi = dpi;

            DA.SetData(0, drawing.ToBitmap());
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