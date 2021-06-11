using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Device
{
    public class DeviceSetting
    {

        public DeviceSetting()
        {
            
        }


        public ObservableCollection<Device> DeviceCollection
        {
            get;set;
        }
    }
}
