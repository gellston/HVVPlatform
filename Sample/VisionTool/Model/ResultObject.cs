using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class ResultObject : PropertyChangedBase
    {

        public ResultObject()
        {

        }

        private string _Name = "";

        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }




        private HV.V1.Object _Data = null;
        public virtual HV.V1.Object Data
        {
            get => _Data;
            set => Set(ref _Data, value);
        }
    }
}
