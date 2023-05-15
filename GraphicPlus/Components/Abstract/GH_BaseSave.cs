using System;
using System.IO;
using System.Collections.Generic;
using Sd = System.Drawing;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System.Windows.Forms;
using gp = GraphicPlus;

namespace GraphicPlus.Components
{
    public abstract class GH_BaseSave : GH_Component
    {
        protected gp.Drawing prevDrawing = new gp.Drawing();

        /// <summary>
        /// Initializes a new instance of the GH_SaveBase class.
        /// </summary>
        public GH_BaseSave()
          : base("GH_SaveBase", "Nickname",
              "Description",
              "Category", "Subcategory")
        {
        }

        public GH_BaseSave(string Name, string NickName, string Description, string Category, string Subcategory) : base(Name, NickName, Description, Category, Subcategory)
        {
        }

        protected override void BeforeSolveInstance()
        {
            prevDrawing = new gp.Drawing();
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalMenuItems(menu);
            Menu_AppendSeparator(menu);
            Menu_AppendItem(menu, "Save Image", SaveImage, true, false);
            Menu_AppendItem(menu, "Save SVG", SaveSVG, true, false);

        }

        public void SaveImage(Object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "JPEG Image|*.jpg|PNG Image|*.png|BMP Image|*.bmp|TIFF Image|*.tiff",
                Title = "Save an Image"
            };
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                FileStream fs = (FileStream)saveFileDialog1.OpenFile();        
                
                Sd.Image img = prevDrawing.ToBitmap();

                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        img.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        img.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Png);
                        break;

                    case 3:
                        img.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 4:
                        img.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Tiff);
                        break;
                }

                fs.Close();

                this.ExpireSolution(true);
            }
        }

        public void SaveSVG(Object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog2 = new SaveFileDialog
            {
                Filter = "Scalable Vector Graphics|*.svg",
                Title = "Save a SVG"
            };
            saveFileDialog2.ShowDialog();

            if (saveFileDialog2.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog2.OpenFile();

                switch (saveFileDialog2.FilterIndex)
                {
                    case 1:
                        StreamWriter m_WriterParameter = new StreamWriter(fs);
                        m_WriterParameter.BaseStream.Seek(0, SeekOrigin.End);
                        m_WriterParameter.Write(prevDrawing.ToScript());
                        m_WriterParameter.Flush();
                        m_WriterParameter.Close();
                        break;
                }
                fs.Close();
            }

            this.ExpireSolution(true);
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("0f5914de-afb1-40e0-8faa-e68a42261fba"); }
        }
    }
}