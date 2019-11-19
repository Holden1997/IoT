using IoT.Common.SharedMessages.Models;
using IoT.DevaceEnactor.Attributes;
using IoT.DevaceEnactor.Interfaces;
using MQTTnet;
using System;

using System.Linq;

namespace IoT.DevaceEnactor.Extensions
{
    public static class MqttMessageRecivedExtension
    {
        public static Message CreateMesssage(this MqttApplicationMessageReceivedEventArgs eventArgs)
        {

            string topic = eventArgs.ApplicationMessage.Topic.Split('/')[0];
            var assemblyModule = typeof(IMessageFactory).Assembly.ExportedTypes.Where(x => typeof(IMessageFactory)
            .IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(Activator.CreateInstance).Cast<IMessageFactory>()
            .ToList();

            foreach (var item in assemblyModule)
            {
                var @type = item.GetType();

                var attribute = type.CustomAttributes.FirstOrDefault(_ => _.AttributeType == typeof(TopicAttribute));
                var @value = attribute.ConstructorArguments.FirstOrDefault(_ => _.ArgumentType == typeof(TopicType));

                if ((TopicType)value.Value == (TopicType)Enum.Parse(typeof(TopicType),topic))    
                    return item.Create(eventArgs);
                
            }

            return null;
        }
    }
}
