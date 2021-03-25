using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace VisionTool.Model.DiagramProperty
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

        //private string _Name = "";
        //public string Name
        //{
        //    get => _Name;
        //    set => Set(ref _Name, value);
        //}


        //private string _ParentFunctionHash;

        //public string ParentFunctionHash
        //{
        //    get => _ParentFunctionHash;
        //    set => Set(ref _ParentFunctionHash, value);
        //}


        public virtual object Clone()
        {
            BaseDiagramProperty newCopy = new BaseDiagramProperty();
            //newCopy.Name = this.Name;
            newCopy.Hash = this.Hash;
            //newCopy.ParentFunctionHash = this.ParentFunctionHash;
           

            return newCopy;
        }

        public virtual string ToCode()
        {
            return "";
        }
    }
}
