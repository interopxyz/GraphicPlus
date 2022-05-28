using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace GraphicPlus.Components.Text
{
    public class GH_Text : GH_BaseGraphics
    {
        /// <summary>
        /// Initializes a new instance of the GH_Text class.
        /// </summary>
        public GH_Text()
          : base("Text Shape", "Text",
              "Construct a Text Shape",
              "Display", "Graphics")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text","T","The text to display",GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane","P","The plane which sets the location and rotation",GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Text Shape", "St", "A Graphic Plus Text Shape Object", GH_ParamAccess.item);
            pManager.HideParameter(0);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            this.AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "NOTE: Rhino Previews are limited and do not directly match final outputs."
                + Environment.NewLine + "For an accurate preview use the in canvas viewer components");

            string text = string.Empty;
            if (!DA.GetData(0, ref text))return;

            Plane plane = Plane.Unset;
            if (!DA.GetData(1, ref plane)) return;

            Shape shape = new Shape(text, plane);


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
                return Properties.Resources.GP_Graphics_Text_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("f19b5802-6361-49dd-885b-d0066ecb43b0"); }
        }
    }
}