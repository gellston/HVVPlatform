using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace VisionTool.Model
{
    public class InputSnapSpot : DiagramObject
    {


        public InputSnapSpot()
        {

        }

        private string _DataType;
        public string DataType
        {
            get => _DataType;
            set => Set(ref _DataType, value);
        }


        private Function _Parent;
        public Function Parent
        {
            get => _Parent;
            set => Set(ref _Parent, value);
        }


        private bool _IsConnected;
        public bool IsConnected
        {
            get => _IsConnected;
            set => Set(ref _IsConnected, value);
        }

        private BindablePoint _offset;
        public BindablePoint Offset
        {
            get
            {
                _offset ??= new BindablePoint();
                return _offset;
            }
        }

        public void Recalculate()
        {
            if (Offset.X < 0)
                Offset.X = 0;
            //if (Offset.X > 1)
            //    Offset.X = 1;

            if (Offset.Y < 0)
                Offset.Y = 0;
            //if (Offset.Y > 1)
            //    Offset.Y = 1;

         
            Location.Value = Point.Add(Parent.Location.Value, new Vector(Offset.X, (Offset.Y)));
        }


        private string _ParentFunctionHash;

        public string ParentFunctionHash
        {
            get => _ParentFunctionHash;
            set => Set(ref _ParentFunctionHash, value);
        }



    }
}
