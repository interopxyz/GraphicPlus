using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GraphicPlus.Components
{
    public class GH_SetStroke : GH_BaseGraphics
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

            pManager.AddGenericParameter("Shape / Geometry", "S", "A Graphic Plus Shape, or a Curve, Brep, Mesh", GH_ParamAccess.item);
            pManager.AddColourParameter("Color", "C", "The stroke color", GH_ParamAccess.item, Color.Black);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Weight", "W", "The stroke weight", GH_ParamAccess.item, 1);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Pattern", "P", "The stroke pattern", GH_ParamAccess.list);
            pManager[3].Optional = true;
            pManager.AddIntegerParameter("End Cap", "E", "The shape to be used at the end of open path", GH_ParamAccess.item, 0);
            pManager[4].Optional = true;

            Param_Integer paramA = (Param_Integer)pManager[4];
            paramA.AddNamedValue("Flat", 0);
            paramA.AddNamedValue("Square", 1);
            paramA.AddNamedValue("Round", 2);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Shape", "S", "A Graphic Plus Shape Object", GH_ParamAccess.item);
            pManager.HideParameter(0);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            this.AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "NOTE: Rhino Previews are limited and do not directly match final outputs."
                + Environment.NewLine + "Lineweight previews are an approximation and scale according to the zoom level."
                + Environment.NewLine + "For an accurate preview use the in canvas viewer components");

            IGH_Goo goo = null;
            if(!DA.GetData(0, ref goo))return;
            Shape shape = null;
            if (!goo.TryGetShape(ref shape)) return;

            Color color = Color.Black;
            DA.GetData(1, ref color);

            double weight = 1.0;
            DA.GetData(2, ref weight);

            List<double> pattern = new List<double>();
            DA.GetDataList(3, pattern);

            int cap = 0;
            DA.GetData(4, ref cap);

            shape.Graphics.SetStroke(color, weight, (Graphic.Caps)cap, pattern);

            DA.SetData(0, shape);
            SetPreview(shape);
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