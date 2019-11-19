using System;
using System.ComponentModel.DataAnnotations;

namespace IoT.Common.Models.ApplicationModels
{
    public class Device
    {
       [Key]
       public Guid SirialNumber { get; set; }
    }

}
