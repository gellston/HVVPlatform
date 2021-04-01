using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Model
{
    public class BindableSize : PropertyChangedBase
    {


        public BindableSize()
        {

        }

        public Action ValueChanged;


        private Size _Value;
        public Size Value
        {
            get => _Value;
            set
            {
                _Value = value;
                OnPropertyChanged("Width");
                OnPropertyChanged("Height");
                OnPropertyChanged("Value");
                if (ValueChanged != null)
                    ValueChanged();
            }
        }

        public double Width
        {
            get => Value.Width;
            set
            {
                Value = new Size(value, Value.Height);
            }
        }


        public double Height
        {
            get => Value.Height;
            set
            {
                Value = new Size(Value.Width, value);
            }
        }
    }
}
