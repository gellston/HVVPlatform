using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DiagramProperty
{

    public class CircleFitROIDiagramProperty : BaseDiagramProperty
    {

        public CircleFitROIDiagramProperty()
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
                var startRadius = radius * this.StartRatio;
                var endRadius = radius * this.EndRatio;
                var finalRadius = startRadius >= endRadius ? startRadius : endRadius;
                finalRadius = finalRadius >= radius ? finalRadius : radius;


                if (finalRadius > 0)
                {
                    this.Width = finalRadius * 2;
                    this.Height = finalRadius * 2;
                }

                Set(ref _Radius, value);
            }
        }

        public double _StartRatio = 0;
        public double StartRatio
        {
            get => _StartRatio;
            set
            {
                if (value < 0) value = 0;

                var radius = this.Radius;
                var startRadius = radius * value;
                var endRadius = radius * this.EndRatio;
                var finalRadius = startRadius >= endRadius ? startRadius : endRadius;
                finalRadius = finalRadius >= radius ? finalRadius : radius;


                if (finalRadius > 0)
                {
                    this.Width = finalRadius * 2;
                    this.Height = finalRadius * 2;
                }

                Set(ref _StartRatio, value);
            }
        }

        public double _EndRatio = 0;
        public double EndRatio
        {
            get => _EndRatio;
            set
            {
                if (value < 0) value = 0;

                var radius = this.Radius;
                var startRadius = radius * this.StartRatio;
                var endRadius = radius * value;
                var finalRadius = startRadius >= endRadius ? startRadius : endRadius;
                finalRadius = finalRadius >= radius ? finalRadius : radius;


                if (finalRadius > 0)
                {
                    this.Width = finalRadius * 2;
                    this.Height = finalRadius * 2;
                }

                Set(ref _EndRatio, value);
            }
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



            string _isBlack2White = IsBlack2White ? "true" : "false";

            code += "new core.circleFitROI(\"temp\"," + X + "," + Y + "," + Radius + "," + StartRatio + "," + EndRatio + "," + _isBlack2White + ")";
            return code;
        }

        public override object Clone()
        {
            CircleFitROIDiagramProperty newCopy = new CircleFitROIDiagramProperty();
            newCopy.X = this.X;
            newCopy.Y = this.Y;
            newCopy.Width = this.Width;
            newCopy.Height = this.Height;
            newCopy.Radius = this.Radius;
            newCopy.StartRatio = this.StartRatio;
            newCopy.EndRatio = this.EndRatio;
            newCopy.IsBlack2White = this.IsBlack2White;


            return newCopy;
        }

        public override string ToString()
        {

            return this.GetType().Name;
        }
    }
}
