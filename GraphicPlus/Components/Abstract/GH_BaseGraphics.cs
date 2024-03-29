﻿using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GraphicPlus.Components
{
    public abstract class GH_BaseGraphics : GH_BaseSave
    {

        /// <summary>
        /// Initializes a new instance of the GH_BaseGraphics class.
        /// </summary>
        public GH_BaseGraphics()
          : base("GH_BaseGraphics", "Nickname",
              "Description",
              "Category", "Subcategory")
        {
        }

        public GH_BaseGraphics(string Name, string NickName, string Description, string Category, string Subcategory) : base(Name, NickName, Description, Category, Subcategory)
        {
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

        protected void SetPreview(Drawing drawing)
        {
            prevDrawing.MergeDrawing(drawing);
        }

        protected void SetPreview(Shape shape)
        {
            prevDrawing.MergeDrawing(shape);
        }

        protected void SetPreview(List<Shape> shapes)
        {
            prevDrawing.MergeDrawing(shapes);
        }

        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            double mTol = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;

            // Surfaces
            foreach (Shape shape in prevDrawing.Shapes)
            {
                if (shape.TextContent == string.Empty)
                {
                    if (shape.Graphics.FillType != Graphic.FillTypes.None)
                    {

                        foreach (Hatch hatch in shape.Hatches)
                        {
                            args.Display.DrawHatch(hatch, shape.Graphics.FillColor, Color.Transparent);
                        }
                    }

                    Transform xform = args.Viewport.GetTransform(Rhino.DocObjects.CoordinateSystem.World, Rhino.DocObjects.CoordinateSystem.Screen);
                    foreach (Curve curve in shape.Curves)
                    {
                        args.Display.DrawCurve(curve, shape.Graphics.StrokeColor, (int)shape.Graphics.Weight);
                    }
                }
                else
                {
                    args.Display.Draw3dText(shape.TextContent, shape.Graphics.FillColor, shape.TextPlane, shape.Graphics.Font.Size/2.0, shape.Graphics.Font.Family, shape.Graphics.Font.IsBold, shape.Graphics.Font.IsItalic, shape.Graphics.Font.RhHorizontalAlignment, shape.Graphics.Font.RhVerticalAlignment);
                }
            }


            // Set Display Override
            base.DrawViewportWires(args);
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
            get { return new Guid("99a9d69f-fc5a-4f37-bcb6-720f66bc373d"); }
        }
    }
}