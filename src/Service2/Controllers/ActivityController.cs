﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IHttpClientFactory HttpClient;

        public ActivityController(IHttpClientFactory httpClient)
        {
            HttpClient = httpClient;
        }

        [HttpGet]
        [Route("Start")]
        public async Task<IActionResult> GetAsync()
        {
            var result = JsonSerializer.Deserialize<object>(await SendAsync("Service1", "/api/Activity/Status"));
            return Ok(result);
        }

        #region HttpClient Factory
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<string> SendAsync(string name, string requestUrl)
        {
            var httpClient = HttpClient.CreateClient(name);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            var response = await httpClient.SendAsync(
             request, HttpCompletionOption.ResponseHeadersRead).
             ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        #endregion
    }
}
