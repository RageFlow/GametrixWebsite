using H3WebAPI.Controllers;
using H3WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace H3WebAPI.Models.Extensions;
[ApiController]
public class RiotAPIControllerExtension : ControllerBase
{
    internal readonly RiotService riotService;
    internal readonly DataService dataService;
    internal readonly ILogger<RiotAPIControllerExtension> logger;

    public RiotAPIControllerExtension(RiotService riotService, ILogger<RiotAPIControllerExtension> logger, DataService dataService)
    {
        this.riotService = riotService;
        this.logger = logger;
        this.dataService = dataService;
    }

    /// <summary>
    /// Simple function for checking value and returning an API Response
    /// </summary>
    /// <param name="value"></param>
    /// <returns>OK, NotFound</returns>
    internal IActionResult ReturnMessage(object? value)
    {
        if (value != null)
        {
            return Ok(value);
        }
        else
        {
            return NotFound(value);
        }
    }
}

