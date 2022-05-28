using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace GraphicPlus.Components.Graphics
{
    public class GH_SetEffectBlur : GH_BaseGraphics
    {
        /// <summary>
        /// Initializes a new instance of the GH_SetEffectBlur class.
        /// </summary>
        public GH_SetEffectBlur()
          : base("Blur", "Blur",
              "Applies a Blur Effect to a Shape",
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
            pManager.AddNumberParameter("Blur Radius", "R", "The radius of the blur effect", GH_ParamAccess.item);
            pManager[1].Optional = true;
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
                + Environment.NewLine + "The blur effect cannot be previewed in Rhino"
                + Environment.NewLine + "For an accurate preview use the in canvas viewer components");

            IGH_Goo goo = null;
            if (!DA.GetData(0, ref goo)) return;
            Shape shape = null;
            if (!goo.TryGetShape(ref shape)) return;

            double radius = 1.0;
            DA.GetData(1, ref radius);

            shape.Graphics.SetBlur(radius);

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
                return Properties.Resources.GP_EffectsBlur_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("ae8329c9-147c-4440-b557-2977e71e7195"); }
        }
    }
}