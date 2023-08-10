using CCOM.AsyncTitleServiceConsumer.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CCOM.AsyncTitleServiceConsumer.Services
{
    public class TitleAsyncService : ITitleAsyncService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        public TitleAsyncService(ILogger<TitleAsyncService> logger,IConfiguration configuration, HttpClient httpClient)
        {
            _logger = logger;
            _config = configuration;
            _httpClient = httpClient;
        }

        public void Handle(AysncTitleProviderMessage aysncTitleProviderMessage)
        {
            
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(30));
            
            var myContent = JsonConvert.SerializeObject(aysncTitleProviderMessage);            
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var uri = _config["App:Uri"];
            var response = _httpClient.PostAsync(uri, byteContent).ConfigureAwait(false);
        }
    }
}
