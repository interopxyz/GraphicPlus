using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace GraphicPlus
{
    public class GraphicPlusInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "GraphicPlus";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return Properties.Resources.GraphicsPlus_24;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "A scalable vector graphics plugin for Grasshopper 3d";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("a48ac930-c378-48dc-84da-26b2af9d8302");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "David Mans";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "interopxyz@gmail.com";
            }
        }

        public override string AssemblyVersion
        {
            get
            {
                return "1.6.0.0";
            }
        }
    }
}
