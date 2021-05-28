using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DiagramProperty
{
    public class EmptyDiagramProperty : BaseDiagramProperty
    {

        public EmptyDiagramProperty()
        {

        }

        public override object Clone()
        {
            EmptyDiagramProperty newCopy = new EmptyDiagramProperty();
            newCopy.Hash = this.Hash;
            return newCopy;
        }

        public override string ToString()
        {

            return this.GetType().Name;
        }

        public override string ToCode()
        {
            return base.ToCode();
        }
    }
}
