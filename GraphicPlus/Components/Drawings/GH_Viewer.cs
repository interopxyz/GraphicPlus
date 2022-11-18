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
    public class GH_Viewer : GH_BaseSave
    {
        string message = "No Image provided";
        Sd.Bitmap bitmap = Properties.Resources.GraphicsPlus_300;

        /// <summary>
        /// Initializes a new instance of the GH_Preview class.
        /// </summary>
        public GH_Viewer()
          : base("Drawing Viewer", "View",
              "Preview a Drawing in canvas."+Environment.NewLine+"Note: Right click on the component to save the image or svg",
              "Display", "Graphics")
        {
            this.Hidden = true;
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
            bitmap = Properties.Resources.GraphicsPlus_300;
            m_attributes = new Attributes_Custom(this);
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Drawings / Shapes / Geometry", "D", "A list of Graphic Plus Drawing, Shapes, or Geometry (Curves, Breps, Meshes).", GH_ParamAccess.list);
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
            List<IGH_Goo> goos = new List<IGH_Goo>();
            if (!DA.GetDataList(0, goos)) return;

            foreach(IGH_Goo goo in goos)
            {
                goo.TryGetDrawings(ref drawing);
            }

            int dpi = 96;
            if (DA.GetData(1, ref dpi)) drawing.Dpi = dpi;

            bitmap = drawing.ToBitmap();
            message = "(" + bitmap.Width + "x" + bitmap.Height + ") " + bitmap.PixelFormat.ToString();

            prevDrawing.MergeDrawing(drawing);
            UpdateMessage();
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
                return Properties.Resources.GP_Rh_Viewer4_01;
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

                int width = comp.bitmap.Width;
                int height = comp.bitmap.Height;
                Sd.Rectangle rec0 = GH_Convert.ToRectangle(Bounds);

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

                    Sd.StringFormat format = new Sd.StringFormat
                    {
                        Alignment = Sd.StringAlignment.Center,
                        LineAlignment = Sd.StringAlignment.Center
                    };

                    graphics.DrawImage(comp.bitmap, Bounds.X + 2, m_innerBounds.Y - (ButtonBounds.Height - Bounds.Height), comp.bitmap.Width - 4, comp.bitmap.Height - 2);

                    format.Dispose();
                }
            }
        }
        }
}