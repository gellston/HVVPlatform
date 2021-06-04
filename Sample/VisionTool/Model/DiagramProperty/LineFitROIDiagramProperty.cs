using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DiagramProperty
{

    public class LineFitROIDiagramProperty : BaseDiagramProperty
    {

        public LineFitROIDiagramProperty()
        {

        }

        public double _Angle = 0;
        public double Angle
        {
            get => _Angle;
            set => Set(ref _Angle, value);
        }

        public bool _IsFlip = false;
        public bool IsFlip
        {
            get => _IsFlip;
            set => Set(ref _IsFlip, value);
        }

        public bool _IsBlack2White = false;
        public bool IsBlack2White
        {
            get => _IsBlack2White;
            set => Set(ref _IsBlack2White, value);
        }


        public override string ToCode()
        {
            var code = "";


            string _isFlip = IsFlip ? "true" : "false";
            string _isBlack2White = IsBlack2White ? "true" : "false";

            code += "new core.lineFitROI(\"temp\"," + X + "," + Y + "," + Angle + "," + Width + "," + Height + "," + _isFlip + "," + _isBlack2White + ")";
            return code;
        }

        public override object Clone()
        {
            LineFitROIDiagramProperty newCopy = new LineFitROIDiagramProperty();
            newCopy.X = this.X;
            newCopy.Y = this.Y;
            newCopy.Width = this.Width;
            newCopy.Height = this.Height;
            newCopy.Angle = this.Angle;
            newCopy.IsBlack2White = this.IsBlack2White;
            newCopy.IsFlip = this.IsFlip;
            

            return newCopy;
        }

        public override string ToString()
        {

            return this.GetType().Name;
        }
    }
}
