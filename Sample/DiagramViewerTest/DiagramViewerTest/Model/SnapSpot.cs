using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace DiagramViewerTest.Model
{
    public class SnapSpot : DiagramObject
    {

        private Node _parent;
        public Node Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
            }
        }

        private bool _isConnected;
        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                OnPropertyChanged("IsConnected");
            }
        }

        public SnapSpot(Node parent)
        {
            Parent = parent;
            Offset.ValueChanged = Recalculate;
        }



        private BindablePoint _offset;
        public BindablePoint Offset
        {
            get {
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
    }
}
