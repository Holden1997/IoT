using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Common.Models.ApplicationModels
{
   public class DevaceAndUser
    {
        public Guid UserId { get; set; }
        public virtual IList <Device> DevaceId { get; set; }

        public DevaceAndUser()
        {
            DevaceId = new List<Device>();
        }

    }
}
