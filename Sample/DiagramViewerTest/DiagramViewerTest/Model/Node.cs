using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DiagramViewerTest.Model
{
    public class Node : DiagramObject
    {
        
        public Node()
        {

            this.Size.ValueChanged = RecalculateSnaps;
            this.Location.ValueChanged = RecalculateSnaps;


            double offset = 30;
            for(int index =0; index < 5; index++)
            {
                var snap = new SnapSpot(this)
                {
                    IsConnected = false,
                    IsNew = true,
                    IsHighlighted = false,
                    
                };
                snap.Offset.Y = index * offset + 15;
                snap.Offset.X = 15;
                snap.Name = "Input_" + index;
                this.InputCollection.Add(snap);
            }

            Name = "Function A";
            Type = "Function";
            IsNew = true;
            this.Size.X = 300;
            this.Size.Y = 300;

        }

        private void RecalculateSnaps()
        {
            InputCollection.ToList().ForEach(x => x.Recalculate());
        }

        private BindablePoint _size = null;
        public BindablePoint Size
        {
            get
            {
                _size ??= new BindablePoint();
                return _size;
            }

        }

        private ObservableCollection<SnapSpot> _InputCollection = null;
        public ObservableCollection<SnapSpot> InputCollection
        {
            get
            {
                _InputCollection ??= new ObservableCollection<SnapSpot>();
                return _InputCollection;
            }
        }

        public void Activate()
        {
            this.IsNew = false;
            this.InputCollection.ToList().ForEach(x => x.IsNew = false);
        }

    }
}
