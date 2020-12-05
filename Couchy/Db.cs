using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System;
using Newtonsoft.Json.Linq;
namespace Couchy
{
    public class Db : IDb
    {
        public IClient Client { get; private set; }

        public IDocumentFactory DocumentFactory { get; private set; }

        public string Name { get; private set; }

        public Db(string name, IClient client, IDocumentFactory documentFactory)
        {
            Name = name;
            Client = client;
            DocumentFactory = documentFactory;
        }

        private async Task<HttpResponseMessage> _getDocResponse(string id)
        {
            if (id is null)
            {
                throw new System.Exception(
                    "Document Id to be fetched cannot be null!"
                );
            }
            var response = await Client.SendRequest(
                HttpMethod.Get,
                new Uri(Client.ServerUrl + "/" + Name + "/" + id),
                null,
                null,
                () => {}
            );
            return response;
        }

        private async Task<string> _getRevisionFromResponse(HttpResponseMessage response)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            
            JObject jObj = JObject.Parse(jsonString);
            var revision = (string)jObj["_rev"];
            return revision;
        }

        public async Task<HttpStatusCode> Create(IDocument item)
        {
            //await response.Content.ReadAsStringAsync();
            var itemJson = item.ToJson();
            var response = await Client.SendRequest(
                HttpMethod.Put,
                new Uri(Client.ServerUrl + "/" + Name + "/" + item.Id),
                new StringContent(itemJson),
                null,
                () => {}
            );
            return response.StatusCode;
        }

        public async Task<IDocument> Read<T>(string id, string revision) where T : IDocument
        {
            var response = await _getDocResponse(id);
            var jsonString = await response.Content.ReadAsStringAsync();
            return DocumentFactory.CreateDocument<T>(jsonString);
        }

        public async Task<HttpStatusCode> Exists(string id)
        {
            var response = await _getDocResponse(id);
            return response.StatusCode;
        }

        public async Task<HttpStatusCode> Update(IDocument item)
        {
            var response = await _getDocResponse(item.Id);
            if (!HttpHandler.IsSuccessStatusCode(response.StatusCode))
            {
                throw new Exception($"Getting doc revision before update failed: {response.StatusCode}");
            }

            var revision = await _getRevisionFromResponse(response);

            var itemJson = item.ToJson();
            var updateResponse = await Client.SendRequest(
                HttpMethod.Put,
                new Uri(Client.ServerUrl + "/" + Name + "/" + item.Id + "?rev=" + revision ),
                new StringContent(itemJson),
                null,
                () => {}
            );

            return updateResponse.StatusCode;

        }

        public async Task<HttpStatusCode> Delete(IDocument item)
        {
            // get latest revision:
            var response = await _getDocResponse(item.Id);
            if (!HttpHandler.IsSuccessStatusCode(response.StatusCode))
            {
                throw new Exception($"Getting doc revision before delete failed: {response.StatusCode}");
            }
            
            var revision = await _getRevisionFromResponse(response);

            var delResponse = await Client.SendRequest(
                HttpMethod.Delete,
                new Uri(Client.ServerUrl + "/" + Name + "/" + item.Id + "?rev=" + revision ),
                null,
                null,
                () => {}
            );

            return delResponse.StatusCode;
        }
    }
}