using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;

namespace Model
{
    public class BindablePoint : PropertyChangedBase
    {
        public BindablePoint()
        {
            this.Value = new Point(0, 0);
        }

        [Newtonsoft.Json.JsonIgnore]
        public Action ValueChanged;


        private Point _Value;
        public Point Value
        {
            get => _Value;
            set
            {

                value.X = (Math.Round(value.X / GridStep)) * GridStep;
                value.Y = (Math.Round(value.Y / GridStep)) * GridStep;

                _Value = value;
                OnPropertyChanged("X");
                OnPropertyChanged("Y");
                OnPropertyChanged("Value");
                if (ValueChanged != null)
                    ValueChanged();
            }
        }

        public double GridStep { get; set; } = 1;

        public double X
        {
            get => Value.X;
            set
            {
                value = (Math.Round(value / GridStep)) * GridStep;
                Value = new Point(value, Value.Y);
            }
        }


        public double Y
        {
            get => Value.Y;
            set
            {
                value = (Math.Round(value / GridStep)) * GridStep;
                Value = new Point(Value.X, value);
            }
        }
    }
}
