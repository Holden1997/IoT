using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IoT.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IoT.WebApi.Extentions;
using NServiceBus;
using IoT.Common.SharedMessages.Models;
using Newtonsoft.Json;

namespace IoT.WebApi.Controllers.v1
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DevaceController : ControllerBase
    {
        private readonly IDevaceService _devaceService;
        private readonly IMessageSession _message;
        public DevaceController(IMessageSession message, IDevaceService devaceService)
        {
            _message = message;
            _devaceService = devaceService;
        }

        [HttpPost("")]
        public async Task<IActionResult> Devace([FromHeader] Guid serialNumber)
        {
            if (!ModelState.IsValid)
                return Content("Serial number is not valid");

           var result =  await _devaceService.AddDevaceToUser(HttpContext.ClientId(),serialNumber )
                .ConfigureAwait(false);

            if (result == false) return Content("Error");

            return StatusCode(201);
        }

        [HttpGet("")]
        public async  Task<string> Devace()
        {
            var clientId =  HttpContext.ClientId();
            var devaces = await  _devaceService.GetAllDevaces(clientId).ConfigureAwait(false);

            List<Guid> serialNamber = new List<Guid>();

            for (int i = 0; i < devaces.Count; i++)
                serialNamber.Add(devaces[i].SirialNumber);

            var response = await _message.Request<DeviceCallback>(new UserMessage(serialNamber)).ConfigureAwait(false);
            string jsonData = JsonConvert.SerializeObject(response.Devices);

            return jsonData;
        }

    }
}