using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace GraphicPlus.Components.Graphics
{
    public class GH_SetEffectShadow : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_SetEffectShadow class.
        /// </summary>
        public GH_SetEffectShadow()
          : base("Drop Shadow", "Shadow",
              "Applies a Drop Shadow Effect to a Shape",
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
            pManager.AddGenericParameter("Shape / Geometry", "S", "A Shape, or a Curve, Brep, Mesh", GH_ParamAccess.item);
            pManager.AddNumberParameter("Shadow Blur Radius", "R", "The radius of the shadow effect", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Shadow Offset", "D", "The offset distance of the shadow effect", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Shadow Angle", "A", "The offset angle of the shadow effect", GH_ParamAccess.item);
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Shape", "S", "A Shape Object", GH_ParamAccess.item);
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

            double radius = 1.0;
            DA.GetData(1, ref radius);

            double distance = 1.0;
            DA.GetData(2, ref distance);

            double angle = 0.0;
            DA.GetData(3, ref angle);

            shape.Graphics.SetShadow(radius,distance,angle);

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
                return Properties.Resources.GP_EffectsShadow_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("4ea36ffe-f9ee-41b1-88a2-c2ff0b15d794"); }
        }
    }
}