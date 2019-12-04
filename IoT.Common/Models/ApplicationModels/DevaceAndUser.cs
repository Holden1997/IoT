using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Common.Models.ApplicationModels
{
   public class DeviceAndUser
    {
        public Guid UserId { get; set; }
        public virtual IList <Device> DeviceId { get; set; }

        public DeviceAndUser()
        {
            DeviceId = new List<Device>();
        }

    }
}
