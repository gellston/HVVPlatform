using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DiagramProperty
{

    public class BaseDiagramProperty : PropertyChangedBase,  ICloneable
    {

        public BaseDiagramProperty()
        {

        }

        public double _X = 0;
        public double X
        {
            get => _X;
            set{

                if (value < 0)
                    value = 0;
                Set(ref _X, value);
            }

        }

        public double _Y = 0;
        public double Y
        {
            get => _Y;
            set
            {
                if (value < 0)
                    value = 0;
                Set(ref _Y, value);
            }

        }

        public double _Width = 0;
        public double Width
        {
            get => _Width;
            set
            {
                if (value < 5)
                    value = 5;

                Set(ref _Width, value);
            }
        }

        public double _Height = 0;
        public double Height
        {
            get => _Height;
            set
            {
                if (value < 5)
                    value = 5;

                Set(ref _Height, value);
            }
        }


        private string _Hash = "";

        public string Hash
        {
            get => _Hash;
            set => Set(ref _Hash, value);
        }

        public virtual object Clone()
        {
            BaseDiagramProperty newCopy = new BaseDiagramProperty();
            newCopy.Hash = this.Hash;
            return newCopy;
        }

        public virtual string ToCode()
        {
            return "";
        }
    }
}
