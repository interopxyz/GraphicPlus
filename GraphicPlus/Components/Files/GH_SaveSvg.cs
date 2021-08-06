using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GraphicPlus.Components.Drawings
{
    public class GH_SaveSvg : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_SaveSvg class.
        /// </summary>
        public GH_SaveSvg()
          : base("Save Svg", "Save Svg",
              "Save a SVG file of a Drawing.",
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
            pManager.AddGenericParameter("Drawing", "D", "A Drawing Object", GH_ParamAccess.item);
            pManager.AddTextParameter("Folder Path", "F", "The folderpath to save the file", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddTextParameter("File Name", "N", "The filename for the Svg export", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddBooleanParameter("Save", "S", "If true, the new file will be writter or overwritten", GH_ParamAccess.item, false);
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Filepath", "P", "The full path to the new file", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_Goo goo = null;

            if (!DA.GetData(0, ref goo)) return;
            Drawing drawing = new Drawing();
            if (!goo.CastTo<Drawing>(out drawing)) return; ;

            string path = "C:\\Users\\Public\\Documents\\";
            bool hasPath = DA.GetData(1, ref path);

            string name = DateTime.UtcNow.ToString("yyyy-dd-M_HH-mm-ss")+".svg";
            bool hasName = DA.GetData(2, ref name);

            bool save = false;
            if (!DA.GetData(3, ref save)) return;

            if (!hasPath) 
            { 
                if (this.OnPingDocument().FilePath != null) 
                { 
                    path = Path.GetDirectoryName(this.OnPingDocument().FilePath) + "\\"; 
                } 
            }

            string filepath = path + name;

            if (hasName)
            {
                string[] segments = path.Split('.');
                if (segments[segments.Count() - 1].ToLower() != "svg")
                {
                    filepath += ".svg";
                }
            }

            if (!Directory.Exists(path))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "The provided folder path does not exist. Please verify this is a valid path.");
                return;
            }

            string data = drawing.ToScript();

            if (save)
            {
                File.WriteAllText(filepath, data);
                DA.SetData(0, filepath);
            }

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
                return Properties.Resources.GP_SVG_Save_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("e6d4e3dd-2058-40b0-958a-53b8bb6c13ae"); }
        }
    }
}