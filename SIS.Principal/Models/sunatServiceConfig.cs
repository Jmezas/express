using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO; 
using System.Net;  
using System.Xml;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace SIS.Principal.Models
{
    public class sunatServiceConfig
    {
        private readonly HttpClient _httpClient;
        public sunatServiceConfig()
        {

            _httpClient = new HttpClient { BaseAddress = new Uri(ConfigurationManager.AppSettings["UrlApiRestDoc"]) };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
        }
        public async Task PostAsync(string endpoint, object data, string args = null)
        {

            var payload = GetPayload(data);
            await _httpClient.PostAsync($"{endpoint}?{args}", payload);
        }


        private static StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public async Task<HttpResponseMessage> PostAsync(string endpoint, object data)
        {

            var payload = GetPayload(data);
            // string accessToken = await GenerateAccessToken();
            // _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.PostAsync($"{endpoint}", payload);
            return response;
        }

        public async Task<HttpResponseMessage> PostAuthAsync(string endpoint, object data)
        {
            var payload = GetPayload(data);
            var response = await _httpClient.PostAsync($"{endpoint}", payload);
            return response;
        }

        public async Task<HttpResponseMessage> PutAsync(string endpoint, object data)
        {

            var payload = GetPayload(data);
            var response = await _httpClient.PutAsync($"{endpoint}", payload);
            return response;
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint, string args = null)
        {
            // string accessToken = await GenerateAccessToken();
            // _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync($"{endpoint}?{args}");
            //if (!response.IsSuccessStatusCode)
            //    throw new NotImplementedException();

            return response;
        }



        public async Task<string> GenerateAccessToken()
        {

            try
            {
                var parameters = new Dictionary<string, string>()
                {
                    //{"username",users},
                    //{"password",password}
                };

                HttpResponseMessage responseMessage = await this.PostAuthAsync("account/login", parameters);
                if (responseMessage.IsSuccessStatusCode)
                {
                    string token = responseMessage.Content.ReadAsStringAsync().Result.Replace("\"", "");
                    return token;
                }

                return string.Empty;
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }

        public HttpResponseMessage Get(string endpoint, string args = null)
        {
            var response = _httpClient.GetAsync($"{endpoint}?{args}").Result;
            if (!response.IsSuccessStatusCode)
                throw new NotImplementedException();

            return response;
        }
    }
}