using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace GraphicPlus.Components.Drawings
{
    public class GH_DrawingToText : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_DrawingToScript class.
        /// </summary>
        public GH_DrawingToText()
          : base("SVG Text", "SVG txt",
              "Converts a Drawing to SVG txt"+Environment.NewLine+"( WARNING: Complex drawings can crash Grasshopper's UI. Use the Save Svg component if this is the case.)",
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
            pManager.AddTextParameter("SVG Text", "T", "The Svg text", GH_ParamAccess.item);
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

            DA.SetData(0, drawing.ToScript());
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
                return Properties.Resources.GP_SVG_Text_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("005ccb43-e524-4da8-ae64-8f5eea6402d6"); }
        }
    }
}