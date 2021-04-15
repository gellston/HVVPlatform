using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Model
{
    public class Connector : DiagramObject
    {

        public Connector() : base()
        {


        }


        private OutputSnapSpot _Start = null;
        public OutputSnapSpot Start
        {
            get => _Start;
            set => Set(ref _Start, value);
        }

        private InputSnapSpot _End = null;
        public InputSnapSpot End
        {
            get => _End;
            set {
                Set(ref _End, value);

                if (Start == null)
                {
                    MidPoint.Value = new Point(_End.Location.X, _End.Location.Y);
                }
                else
                {
                    MidPoint.Value = new Point(((End.Location.X + Start.Location.X) / 2),
                                     ((End.Location.Y + Start.Location.Y) / 2));
                }
                    
            } 
        }

        private BindablePoint _MidPoint = null;
        public BindablePoint MidPoint
        {
            get
            {
                _MidPoint ??= new BindablePoint();
                return _MidPoint;
            }
        }

        private string _StartSnapHash = "";

        public string StartSnapHash
        {
            get => _StartSnapHash;
            set => Set(ref _StartSnapHash, value);
        }


        private string _EndSnapHash = "";

        public string EndSnapHash
        {
            get => _EndSnapHash;
            set => Set(ref _EndSnapHash, value);
        }


    }
}
