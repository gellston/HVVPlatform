using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Model
{
    public class Connector : DiagramObject
    {

        public Connector()
        {


        }


        private OutputSnapSpot _Start;
        public OutputSnapSpot Start
        {
            get => _Start;
            set => Set(ref _Start, value);
        }

        private InputSnapSpot _End;
        public InputSnapSpot End
        {
            get => _End;
            set {
                Set(ref _End, value);
                MidPoint.Value = new Point(((End.Location.X + Start.Location.X) / 2),
                                     ((End.Location.Y + Start.Location.Y) / 2));
            } 
        }

        private BindablePoint _MidPoint;
        public BindablePoint MidPoint
        {
            get
            {
                _MidPoint ??= new BindablePoint();
                return _MidPoint;
            }
        }

        private string _StartSnapHash;

        public string StartSnapHash
        {
            get => _StartSnapHash;
            set => Set(ref _StartSnapHash, value);
        }


        private string _OutputSnapHash;

        public string OutputSnapHash
        {
            get => _OutputSnapHash;
            set => Set(ref _OutputSnapHash, value);
        }


    }
}
