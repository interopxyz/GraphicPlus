using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GraphicPlus.Components.Graphics
{
    public class GH_SetEffectGlow : GH_BaseGraphics
    {
        /// <summary>
        /// Initializes a new instance of the GH_SetEffectInnerGlow class.
        /// </summary>
        public GH_SetEffectGlow()
          : base("Glow", "Glow",
              "Applies an Inner or Outer Glow Effect to a Shape",
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
            pManager.AddNumberParameter("Glow Radius", "R", "The radius of the glow effect", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddColourParameter("Glow Color", "C", "The radius of the glow effect", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddBooleanParameter("Inner", "I", "If true the glow will be interior to the object, if false it will be exterior", GH_ParamAccess.item);
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
            this.AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "NOTE: Rhino Previews are limited and do not directly match final outputs."
                + Environment.NewLine + "The glow effect cannot be previewed in Rhino"
                + Environment.NewLine + "Outer glow can be previewd in the canvas viewer components, but inner glow cannot");

            IGH_Goo goo = null;
            if (!DA.GetData(0, ref goo)) return;
            Shape shape = null;
            if (!goo.TryGetShape(ref shape)) return;

            double radius = 1.0;
            DA.GetData(1, ref radius);

            Color color = Color.LightGray;
            DA.GetData(2, ref color);

            bool inner = true;
            DA.GetData(3, ref inner);

            shape.Graphics.SetGlow(radius,color,inner);

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
                return Properties.Resources.GP_EffectsGlow_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("56c82327-20a9-47f1-85a4-b3d23d460481"); }
        }
    }
}