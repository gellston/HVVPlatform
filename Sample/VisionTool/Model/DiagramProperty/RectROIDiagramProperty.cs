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

        public double _X = 0;
        public double X
        {
            get => _X;
            set => Set(ref _X, value);
        }

        public double _Y = 0;
        public double Y
        {
            get => _Y;
            set => Set(ref _Y, value);
        }

        public double _Width = 0;
        public double Width
        {
            get => _Width;
            set => Set(ref _Width, value);
        }

        public double _Height = 0;
        public double Height
        {
            get => _Height;
            set => Set(ref _Height, value);
        }


        public override string ToString()
        {

            return this.GetType().Name;
        }

        public override string ToCode()
        {
            var code = "";
            code += "new core.rect(\"temp\"," + X + "," + Y + "," + Width + "," + Height + ")";
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
