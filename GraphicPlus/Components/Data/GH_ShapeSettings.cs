using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace GraphicPlus.Components.Data
{
    public class GH_ShapeSettings : GH_BaseSave
    {
        /// <summary>
        /// Initializes a new instance of the GH_ShapeData class.
        /// </summary>
        public GH_ShapeSettings()
          : base("Shape Settings", "Settings",
              "Optionally override Id and set a group / layer.",
              "Display", "Graphics")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.tertiary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Shape / Geometry", "S", "A Graphic Plus Shape, or a Curve, Brep, Mesh", GH_ParamAccess.item);
            pManager.AddTextParameter("ID", "I", "An optional id override" + System.Environment.NewLine + "(Note: If overriding this property every value should be unique)", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddTextParameter("Layer", "L", "An optional layer name" + System.Environment.NewLine + "Uses Rhino's Layer structure for nesting ( Parent :: Child :: GrandChild ) ", GH_ParamAccess.item);
            pManager[2].Optional = true;
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
            Shape shape = null;
            if (!goo.TryGetShape(ref shape)) return;

            string id = string.Empty;
            if(DA.GetData(1, ref id)) shape.SetId(id);

            string layer = string.Empty;
            if (DA.GetData(2, ref layer)) shape.Layer = layer;

            DA.SetData(0, shape);
            prevDrawing.MergeDrawing(shape);
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
                return Properties.Resources.GP_SVG_Layers2_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("e888a1fe-0929-49a2-95df-9c74fcc194b4"); }
        }
    }
}