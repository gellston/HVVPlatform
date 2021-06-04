using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DiagramProperty
{
    public class RectROIDiagramProperty : BaseDiagramProperty
    {

        public RectROIDiagramProperty()
        {

        }


        public override string ToString()
        {

            return this.GetType().Name;
        }

        public override string ToCode()
        {
            var code = "";
            code += "new core.rectROI(\"temp\"," + X + "," + Y + "," + Width + "," + Height + ")";
            return code;
        }

        public override object Clone()
        {
            RectROIDiagramProperty newCopy = new RectROIDiagramProperty();
            newCopy.X = this.X;
            newCopy.Y = this.Y;
            newCopy.Width = this.Width;
            newCopy.Height = this.Height;

            return newCopy;
        }
    }
}
