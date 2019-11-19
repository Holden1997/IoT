using System;
using System.Threading.Tasks;
using IoT.Common.SharedMessages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

namespace IoT.WebApi.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class TopicController : ControllerBase
    {
        private readonly IMessageSession _message;
        public TopicController(IMessageSession message)
        {
            _message = message;
        }
        [HttpPut("led/{id}")]
        public async  Task<IActionResult> Topic(Guid id, [FromBody] string payload)
        {
        
            string topic = "led/publish" + id.ToString();
            var command = new DevicesPublishCommand(topic, payload);

            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("IoT.DeviceEnactor");

            await _message.Send(command,sendOptions)
                .ConfigureAwait(false);

            return StatusCode(200);
        }
    }
}