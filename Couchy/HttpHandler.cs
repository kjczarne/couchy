using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Couchy
{
    public static class HttpHandler
    {
        public static async Task<HttpResponseMessage> SendRequest(
            HttpClient webClient,
            HttpMethod httpMethod,
            Uri url,
            HttpContent content,
            Dictionary<string, string> additionalHeaders,
            Action onFailure
        )
        {
            try	
            {
                // var conc = Credentials.Username + ":" + Credentials.Password;
                // var byteArray = Encoding.ASCII.GetBytes(conc);

                var request = new HttpRequestMessage {
                    Method = httpMethod,
                    RequestUri = url,
                    Content = content,
                };

                request.Headers.Add("Host", url.Host);
                request.Headers.Add("Accept", "application/json");
                // request.Headers.Add(HttpRequestHeader.Authorization.ToString(), $"Basic {Convert.ToBase64String(byteArray)}");

                if (!(additionalHeaders is null))
                {
                    foreach (var (key, value) in additionalHeaders)
                    {
                        if (key.StartsWith("Content-"))
                        {
                            request.Content.Headers.Add(key, value);
                        } else
                        {
                            request.Headers.Add(key, value);   
                        }
                    }   
                }

                HttpResponseMessage response = await webClient.SendAsync(request);
                // response.EnsureSuccessStatusCode();

                if (!HttpHandler.IsSuccessStatusCode(response.StatusCode))
                {
                    onFailure();
                }
                return response;
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ",e.Message);
                if (!(onFailure is null))
                {
                    onFailure();
                }
            }
            return null;
        }

        public static bool IsSuccessStatusCode(HttpStatusCode code)
        {
            if ((int)code >= 200 && (int)code <= 299)
            {
                return true;
            } else
            {
                return false;
            }
        }

    }
}