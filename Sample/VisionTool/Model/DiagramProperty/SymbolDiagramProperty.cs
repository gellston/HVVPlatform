using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DiagramProperty
{
    public class SymbolDiagramProperty : BaseDiagramProperty
    {

        public SymbolDiagramProperty()
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
            return this.Value;
        }

        public override object Clone()
        {
            SymbolDiagramProperty newCopy = new SymbolDiagramProperty();
            newCopy.Hash = this.Hash;
            newCopy.Value = this.Value;
            return newCopy;
        }
    }
}
