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
using System.Threading;

namespace IoT.WebApi.Controllers.v1
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DevaceController : ControllerBase
    {
        private readonly IDeviceService _devaceService;
        private readonly IMessageSession _message;
        public DevaceController(IMessageSession message, IDeviceService devaceService)
        {
            _message = message;
            _devaceService = devaceService;
        }

        [HttpPost("")]
        public async Task<IActionResult> Devace([FromHeader] Guid serialNumber)
        {
            if (!ModelState.IsValid)
                return Content("Serial number is not valid");

            var result = await _devaceService.AddDeviceToUser(HttpContext.ClientId(), serialNumber);

            if (result == false) return Content("Error");

            return StatusCode(201);
        }

        [HttpGet("")]
        public async  Task<IActionResult> Devace()
        {
            var id =  Thread.CurrentThread.ManagedThreadId;
            var clientId =  HttpContext.ClientId();
            var devices = await _devaceService.GetAllDevices(clientId);
            if (devices.Count == 0)
                return NoContent();
            var id2 = Thread.CurrentThread.ManagedThreadId;
            List<Guid> serialNamber = new List<Guid>();
         
            for (int i = 0; i < devices.Count; i++)
                serialNamber.Add(devices[i].SirialNumber);

            var response = await _message.Request<DeviceCallback>(new UserMessage(serialNamber));
            string jsonData = JsonConvert.SerializeObject(response.Devices);

            return Content(jsonData);
        }

    }
}