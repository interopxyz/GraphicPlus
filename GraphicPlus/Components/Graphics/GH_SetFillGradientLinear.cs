using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GraphicPlus.Components
{
    public class GH_SetFillGradientLinear : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the SetFillGradientLinear class.
        /// </summary>
        public GH_SetFillGradientLinear()
          : base("Gradient Linear Fill", "Linear Gradient",
              "Applies a Linear Gradient Fill to a Shape",
              "Display", "Graphics")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary | GH_Exposure.obscure; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Shape / Geometry", "S", "A Graphic Plus Shape, or a Curve, Brep, Mesh", GH_ParamAccess.item);
            pManager.AddColourParameter("Colors", "C", "The Gradient Stop Colors", GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Parameters", "P", "The Gradient Stop parameters", GH_ParamAccess.list);
            pManager[2].Optional = true;
            pManager.AddBooleanParameter("Vertical", "V", "Flip the direction from horizontal to vertical", GH_ParamAccess.item, false);
            pManager[3].Optional = true;
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
            IGH_Goo goo = null;
            if (!DA.GetData(0, ref goo)) return;
            Shape shape = goo.ToShape();

            List<Color> colors = new List<Color>();
            DA.GetDataList(1, colors);
            if (colors.Count < 1)
            {
                colors.Add(Color.Black);
                colors.Add(Color.White);
            }

            List<double> parameters = new List<double>();
            DA.GetDataList(2, parameters);
            if (parameters.Count < 1)
            {
                parameters.Add(0.0);
                parameters.Add(1.0);
            }

            bool flip= false;
            DA.GetData(3, ref flip);

            double angle = 0;
            if (flip) angle = 90;

            shape.Graphics.SetLinearGradient(parameters,colors,angle);

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
                return Properties.Resources.GP_Graphics_Fill_Gradient_Linear_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("388654e0-3a95-4114-bd95-00b539b71624"); }
        }
    }
}