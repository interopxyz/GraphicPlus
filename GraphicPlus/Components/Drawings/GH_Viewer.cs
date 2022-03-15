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

namespace GraphicPlus.Components.Drawings
{
    public class GH_Viewer : GH_Component
    {
        public Sd.Image img = null;
        public string svg = string.Empty;
        string message = "No Image provided";

        /// <summary>
        /// Initializes a new instance of the GH_Preview class.
        /// </summary>
        public GH_Viewer()
          : base("Drawing Viewer", "View",
              "Preview a Drawing in canvas."+Environment.NewLine+"Note: Right click on the component to save the image or svg",
              "Display", "Graphics")
        {
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.quinary; }
        }

        public override void CreateAttributes()
        {
            img = Properties.Resources.GraphicsPlus_300;
            m_attributes = new Attributes_Custom(this);
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Drawing", "D", "A Graphic Plus Drawing object", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Resolution", "R", "The PPI (Pixels Per Inch) resolution for the image which must be greater than or equal to 72.", GH_ParamAccess.item, 96);
            pManager[1].Optional = true;
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
            Drawing drawing = new Drawing();
            IGH_Goo goo = null;
            if (!DA.GetData(0, ref goo)) return;
            if (goo.CastTo<Drawing>(out drawing))
            {
                drawing = new Drawing(drawing);
            }

            int ppi = 96;
            DA.GetData(1, ref ppi);
            if (ppi < 72) ppi = 72;

            Sd.Bitmap bitmap = drawing.ToBitmap(ppi);
            img = bitmap;
            svg = drawing.ToScript();
            message = "(" + bitmap.Width + "x" + bitmap.Height + ") " + bitmap.PixelFormat.ToString();
            UpdateMessage();
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

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPEG Image|*.jpg|PNG Image|*.png|BMP Image|*.bmp|TIFF Image|*.tiff";
            saveFileDialog1.Title = "Save an Image";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs =
                    (System.IO.FileStream)saveFileDialog1.OpenFile();

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

            SaveFileDialog saveFileDialog2 = new SaveFileDialog();
            saveFileDialog2.Filter = "Scalable Vector Graphics|*.svg";
            saveFileDialog2.Title = "Save a SVG";
            saveFileDialog2.ShowDialog();

            if (saveFileDialog2.FileName != "")
            {
                System.IO.FileStream fs =
                    (System.IO.FileStream)saveFileDialog2.OpenFile();

                switch (saveFileDialog2.FilterIndex)
                {
                    case 1:
                        StreamWriter m_WriterParameter = new StreamWriter(fs);
                        m_WriterParameter.BaseStream.Seek(0, SeekOrigin.End);
                        m_WriterParameter.Write(svg);
                        m_WriterParameter.Flush();
                        m_WriterParameter.Close();
                        break;                
                }
                fs.Close();
            }

            this.ExpireSolution(true);
            }
        

        private void UpdateMessage()
        {
            Message = message;
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
                return Properties.Resources.GP_Viewer_01;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("fc076e15-dcb0-4d11-bf04-f5c79fc3d200"); }
        }


        public class Attributes_Custom : GH_ComponentAttributes
        {
            public Attributes_Custom(GH_Component owner) : base(owner) { }

            private Sd.Rectangle ButtonBounds { get; set; }
            protected override void Layout()
            {
                base.Layout();
                GH_Viewer comp = Owner as GH_Viewer;

                int width = comp.img.Width;
                int height = comp.img.Height;
                Sd.Rectangle rec0 = GH_Convert.ToRectangle(Bounds);

                int cWidth = rec0.Width;
                int cHeight = rec0.Height;

                rec0.Width = width;
                rec0.Height += height;

                Sd.Rectangle rec1 = rec0;
                rec1.Y = rec1.Bottom - height;
                rec1.Height = height;
                rec1.Width = width;

                Bounds = rec0;
                ButtonBounds = rec1;

            }

            protected override void Render(GH_Canvas canvas, Sd.Graphics graphics, GH_CanvasChannel channel)
            {
                base.Render(canvas, graphics, channel);
                GH_Viewer comp = Owner as GH_Viewer;

                if (channel == GH_CanvasChannel.Objects)
                {
                    GH_Capsule capsule = GH_Capsule.CreateCapsule(ButtonBounds, GH_Palette.Normal, 0, 0);
                    capsule.Render(graphics, Selected, Owner.Locked, true);
                    capsule.AddOutputGrip(this.OutputGrip.Y);
                    capsule.Dispose();
                    capsule = null;

                    Sd.StringFormat format = new Sd.StringFormat();
                    format.Alignment = Sd.StringAlignment.Center;
                    format.LineAlignment = Sd.StringAlignment.Center;

                    Sd.RectangleF textRectangle = ButtonBounds;

                    graphics.DrawImage(comp.img, Bounds.X + 2, m_innerBounds.Y - (ButtonBounds.Height - Bounds.Height), comp.img.Width - 4, comp.img.Height - 2);

                    format.Dispose();
                }
            }
        }
        }
}