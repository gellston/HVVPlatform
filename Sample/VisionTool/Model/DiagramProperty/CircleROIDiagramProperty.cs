using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DiagramProperty
{

    public class CircleROIDiagramProperty : BaseDiagramProperty
    {

        public CircleROIDiagramProperty()
        {

        }

        public double _Radius = 0;
        public double Radius
        {
            get => _Radius;
            set
            {
                if (value < 0) value = 0;

                var radius = value;


                if (radius > 0)
                {
                    this.Width = radius * 2;
                    this.Height = radius * 2;
                }

                Set(ref _Radius, value);
            }
        }


        public override string ToCode()
        {
            var code = "";
            code += "new core.circleROI(\"temp\"," + X + "," + Y + "," + Radius + ")";
            return code;
        }

        public override object Clone()
        {
            CircleROIDiagramProperty newCopy = new CircleROIDiagramProperty();
            newCopy.X = this.X;
            newCopy.Y = this.Y;
            newCopy.Width = this.Width;
            newCopy.Height = this.Height;
            newCopy.Radius = this.Radius;


            return newCopy;
        }

        public override string ToString()
        {

            return this.GetType().Name;
        }
    }
}
