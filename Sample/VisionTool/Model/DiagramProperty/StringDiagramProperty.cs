using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DiagramProperty
{
    public class StringDiagramProperty : BaseDiagramProperty
    {

        public StringDiagramProperty()
        {

        }

        public string _Value = "";
        public string Value
        {
            get => _Value;
            set => Set(ref _Value, value);
        }

        public override string ToString()
        {

            return this.GetType().Name;
        }

        public override string ToCode()
        {
            if (this.Value == "")
                return "\"\"";
            else return "\"" + this.Value  + "\"";
        }

        public override object Clone()
        {
            StringDiagramProperty newCopy = new StringDiagramProperty();
            newCopy.Hash = this.Hash;
            newCopy.Value = this.Value;
            return newCopy;
        }
    }
}
