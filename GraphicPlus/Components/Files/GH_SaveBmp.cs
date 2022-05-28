using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace GraphicPlus.Components.Drawings
{
    public class GH_SaveBmp : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GH_SaveBmp class.
        /// </summary>
        public GH_SaveBmp()
          : base("Save Bitmap", "SaveBmp",
              "Save a Bitmap file of a Drawing.",
              "Display", "Graphics")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.senary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Drawings / Shapes / Geometry", "D", "A list of Graphic Plus Drawing, Shapes, or Geometry (Curves, Breps, Meshes).", GH_ParamAccess.list);
            pManager.AddTextParameter("Folder Path", "F", "The folder path to save the file", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddTextParameter("File Name", "N", "The file name for the bitmap", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("Extension", "E", "File type extension", GH_ParamAccess.item, 0);
            pManager[3].Optional = true;
            pManager.AddIntegerParameter("Resolution", "R", "The PPI (Pixels Per Inch) resolution for the image which must be greater than or equal to 72.", GH_ParamAccess.item, 96);
            pManager[4].Optional = true;
            pManager.AddBooleanParameter("Save", "S", "If true, save image file", GH_ParamAccess.item, false);
            pManager[5].Optional = true;

            Param_Integer param = (Param_Integer)pManager[3];
            param.AddNamedValue("png", 0);
            param.AddNamedValue("jpeg", 1);
            param.AddNamedValue("bmp", 2);
            param.AddNamedValue("tiff", 3);
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
            Drawing drawing = new Drawing();
            List<IGH_Goo> goos = new List<IGH_Goo>();
            if (!DA.GetDataList(0, goos)) return;

            foreach (IGH_Goo goo in goos)
            {
                goo.TryGetDrawings(ref drawing);
            }

            string path = "C:\\Users\\Public\\Documents\\";
            bool hasPath = DA.GetData(1, ref path);

            string name = DateTime.UtcNow.ToString("yyyy-dd-M_HH-mm-ss");
             DA.GetData(2, ref name);

            int extension = 0;
            DA.GetData(3, ref extension);
            if (extension < 0) extension = 0;
            if (extension > 3) extension = 3;
            int ppi = 96;
            DA.GetData(4, ref ppi);
            if (ppi < 72) ppi = 72;

            Bitmap bitmap = drawing.ToBitmap(ppi);

            bool save = false;
            DA.GetData(5, ref save);

            if (!hasPath)
            {
                if (this.OnPingDocument().FilePath != null)
                {
                    path = Path.GetDirectoryName(this.OnPingDocument().FilePath) + "\\";
                }
            }

            if (path.Last() != '\\') path += "\\";

            name = name.StripExtension();

            if (!Directory.Exists(path))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "The provided folder path does not exist. Please verify this is a valid path.");
                return;
            }


            string ext = ".png";
            System.Drawing.Imaging.ImageFormat encoding = System.Drawing.Imaging.ImageFormat.Png;
            switch (extension)
            {
                case 1:
                    ext = ".jpeg";
                    encoding = System.Drawing.Imaging.ImageFormat.Jpeg;
                    break;
                case 2:
                    ext = ".bmp";
                    encoding = System.Drawing.Imaging.ImageFormat.Bmp;
                    break;
                case 3:
                    ext = ".tiff";
                    encoding = System.Drawing.Imaging.ImageFormat.Tiff;
                    break;
            }

            Bitmap bmp = (Bitmap)bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            string filepath = path + name + ext;

            if (save)
            {
                bmp.Save(filepath, encoding);
                bmp.Dispose();

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
                return Properties.Resources.GP_Bmp_Save_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("33104752-99ac-4ed7-a11e-a6f6d9502462"); }
        }
    }
}