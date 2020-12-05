using System;
using System.Net;
using System.Data;
using System.Text;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace Couchy
{
    public class Client : IClient
    {
        private CookieContainer _cookieJar = new CookieContainer();
        public CookieContainer CookieJar { get => _cookieJar; }
        public HttpClient WebClient { get; set; }
        public ICredProvider Credentials { get; set; }

        public string ServerUrl { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool IsConnected { get; set; }

        public Client(string url, ICredProvider provider)
        {
            Credentials = provider;
            ServerUrl = url;
            WebClient = new HttpClient(
                new HttpClientHandler() { CookieContainer = CookieJar }
            );
        }

        public async Task<HttpResponseMessage> SendRequest(
            HttpMethod httpMethod,
            Uri url,
            HttpContent content,
            Dictionary<string, string> additionalHeaders,
            Action onFailure
        )
        {
            var resp = await HttpHandler.SendRequest(
                WebClient,
                httpMethod,
                url,
                content,
                additionalHeaders,
                onFailure);
            
            return resp;
        }

        public async Task<HttpStatusCode> LogIn()
        {
            var endpointUriBuilder = new UriBuilder(new Uri(ServerUrl));
            endpointUriBuilder.Path = "_session";
            var c = JsonContent.Create(
                "{\"name\": \"" + Credentials.Username + "\", \"password\": \"" + Credentials.Password + "\"}");
            var response = await SendRequest(
                HttpMethod.Post,
                endpointUriBuilder.Uri,
                new StringContent(
                    "{\"name\": \"" + Credentials.Username + "\", \"password\": \"" + Credentials.Password + "\"}",
                    Encoding.UTF8, 
                    "application/json"
                ),
                // new Dictionary<string, string> { { "Content-Type", "application/json" } },
                null,
                () => { IsLoggedIn = false; IsConnected = false; }
            );

            IsLoggedIn = true; 
            IsConnected = true;
            return response.StatusCode;
        }

        public async Task<HttpStatusCode> LogOut()
        {
            // fake method because deleting cookies only
            // works for the client, cookies on the server are stateless
            IsLoggedIn = false; 
            IsConnected = false;
            // clear cookie container:
            _cookieJar = new CookieContainer();
            return HttpStatusCode.Accepted;
        }

        public async Task<HttpStatusCode> CreateDb(string name)
        {
            var response = await SendRequest(
                HttpMethod.Put,
                new Uri(ServerUrl + "/" + name),
                null,
                null,
                () => {}
            );
            return response.StatusCode;
        }

        public async Task<HttpStatusCode> DeleteDb(string name)
        {
            var response = await SendRequest(
                HttpMethod.Delete,
                new Uri(ServerUrl + "/" + name),
                null,
                null,
                () => {}
            );
            return response.StatusCode;
        }

    }
}
