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
