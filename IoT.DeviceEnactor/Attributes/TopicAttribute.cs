using IoT.Common.SharedMessages.Models;
using System;

namespace IoT.DevaceEnactor.Attributes
{
    public class TopicAttribute : Attribute
    {
        public TopicAttribute(TopicType type) { }
    }
}
