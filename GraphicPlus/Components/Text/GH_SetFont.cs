using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GraphicPlus.Components.Text
{
    public class GH_SetFont : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_SetFont class.
        /// </summary>
        public GH_SetFont()
          : base("Font", "Font",
              "Applies a Font to a Text Shape",
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
            pManager.AddGenericParameter("Text Shape", "St", "A Text Shape object", GH_ParamAccess.item);
            pManager.AddTextParameter("Font Family", "F", "The font family name", GH_ParamAccess.item,"Arial");
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Font Size", "S", "The font size", GH_ParamAccess.item,8);
            pManager[2].Optional = true;
            pManager.AddBooleanParameter("Bold", "B", "Specifies if the font is bold or regular", GH_ParamAccess.item,false);
            pManager[3].Optional = true;
            pManager.AddBooleanParameter("Italic", "I", "Specifies if the font is italic or regular", GH_ParamAccess.item,false);
            pManager[4].Optional = true;
            pManager.AddBooleanParameter("Underline", "U", "Specifies if the font is underlined or regular", GH_ParamAccess.item,false);
            pManager[5].Optional = true;
            pManager.AddIntegerParameter("Justification", "J", "The horizontal and vertical text justification", GH_ParamAccess.item, 0);
            pManager[6].Optional = true;

            Param_Integer paramA = (Param_Integer)pManager[6];
            foreach (FontObject.Justifications value in Enum.GetValues(typeof(FontObject.Justifications)))
            {
                paramA.AddNamedValue(value.ToString(), (int)value);
            }

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
            if (!DA.GetData(0, ref goo)) return;
            Shape shape = goo.ToShape();

            string family = "Arial";
            DA.GetData(1, ref family);

            double size = 8;
            DA.GetData(2, ref size);

            bool isBold = false;
            DA.GetData(3, ref isBold);

            bool isItalic = false;
            DA.GetData(4, ref isItalic);

            bool isUnderline = false;
            DA.GetData(5, ref isUnderline);

            int justify = 0;
            DA.GetData(6, ref justify);

            if (shape != null) shape.Graphics.SetFont(family,size,isBold,isItalic,isUnderline, (FontObject.Justifications)justify);

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
                return Properties.Resources.GP_Graphics_Font_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("93ccac51-9dbc-4817-bb0b-2c15be81a4fc"); }
        }
    }
}