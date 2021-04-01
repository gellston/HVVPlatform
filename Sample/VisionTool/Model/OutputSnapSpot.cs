using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Model
{
    public class OutputSnapSpot : DiagramObject
    {

        public OutputSnapSpot(string _Name, string _DataType)
        {
            this.Name = _Name;
            this.DataType = _DataType;

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



        private BindablePoint _offset;
        public BindablePoint Offset
        {
            set => _offset = value;
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
