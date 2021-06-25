using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Model
{
    public class DiagramObject : PropertyChangedBase
    {

        public DiagramObject()
        {

            this.Color = Color.FromRgb(255, 255, 255);
            
        }

        


        private string _Name;
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        private bool _IsNew = false;
        public bool IsNew
        {
            get => _IsNew;
            set => Set(ref _IsNew, value);
        }

        private Color _Color = Color.FromRgb(255,255,255);
        public Color Color
        {
            get => _Color;
            set => Set(ref _Color, value);
        }


        private bool _IsHighLight = false;
        public bool IsHighLight
        {
            get => _IsHighLight;
            set => Set(ref _IsHighLight, value);
        }


        private BindablePoint _Location = null;
        public BindablePoint Location
        {
            get
            {
                _Location ??= new BindablePoint();
                return _Location;
            }
        }


        private string _Hash;

        public string Hash
        {
            get => _Hash;
            set => Set(ref _Hash, value);
        }



    }
}
