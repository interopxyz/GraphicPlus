using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GraphicPlus.Components
{
    public class GH_SetStroke : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the SetStroke class.
        /// </summary>
        public GH_SetStroke()
          : base("Stroke", "Stroke",
              "Applies Stroke properties to a Shape",
              "Display", "Graphics")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {

            pManager.AddGenericParameter("Shape / Geometry", "S", "A Shape, or a Curve, Brep, Mesh", GH_ParamAccess.item);
            pManager.AddColourParameter("Color", "C", "The stroke color", GH_ParamAccess.item, Color.Black);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Weight", "W", "The stroke weight", GH_ParamAccess.item, 1);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Pattern", "P", "The stroke pattern", GH_ParamAccess.list);
            pManager[3].Optional = true;

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Shape", "S", "A Shape Object", GH_ParamAccess.item);
            pManager.HideParameter(0);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_Goo goo = null;
            if(!DA.GetData(0, ref goo))return;
            Shape shape = goo.ToShape();

            Color color = Color.Black;
            DA.GetData(1, ref color);

            double weight = 1.0;
            DA.GetData(2, ref weight);

            List<double> pattern = new List<double>();
            DA.GetDataList(3, pattern);

            shape.Graphics.SetStroke(color, weight, pattern);

            DA.SetData(0, shape);
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
                return Properties.Resources.GP_Graphics_Stroke_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("030b487b-a566-476f-96a4-a0ae2ad283af"); }
        }
    }
}