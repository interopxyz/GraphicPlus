using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace GraphicPlus.Components.Data
{
    public class GH_ShapeData : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_ShapeData class.
        /// </summary>
        public GH_ShapeData()
          : base("Shape Data", "Data",
              "Optionally override Id and add a series of data items to a Shape.",
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
            pManager.AddGenericParameter("Shape / Geometry", "S", "A Shape, or a Curve, Brep, Mesh", GH_ParamAccess.item);
            pManager.AddTextParameter("ID", "I", "An optional id override"+System.Environment.NewLine+"(Note: If overriding this property every value should be unique)", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddTextParameter("Keys", "K", "A list of titles to be added to the svg element as data-'key'", GH_ParamAccess.list);
            pManager[2].Optional = true;
            pManager.AddTextParameter("Values", "V", "The values coordinted with the titles to attach to the element", GH_ParamAccess.list);
            pManager[3].Optional = true;
            pManager.AddBooleanParameter("Hover", "H", "If true the key value pairs will be displayed when the mouse overs over the shape", GH_ParamAccess.item,false);
            pManager[4].Optional = true;
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

            string id = string.Empty;
            bool hasId = DA.GetData(1, ref id);

            List<string> keys = new List<string>();
            DA.GetDataList(2, keys);

            List<string> vals = new List<string>();
            DA.GetDataList(3, vals);

            bool hasTitle = false;
            DA.GetData(4, ref hasTitle);

            if (hasId) shape.SetId(id);

            int count = keys.Count;
            for (int i = vals.Count; i < count; i++) vals.Add(string.Empty);

            for (int i = 0; i < count; i++)
            {
                if (!shape.Data.ContainsKey(keys[i]))
                {
                    shape.Data.Add(keys[i], vals[i]);
                }
                else
                {
                    shape.Data[keys[i]] = vals[i];
                }
            }
            shape.HasTitle = hasTitle;

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
                return Properties.Resources.GP_SVG_Data_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("7eea3a07-f271-4f6b-8c9d-1ddc9b1fd002"); }
        }
    }
}