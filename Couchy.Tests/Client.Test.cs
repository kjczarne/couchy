using NUnit.Framework;
using Couchy;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace Couchy.Tests
{
    public class TestClient
    {
        private Client _client;

        [OneTimeSetUp]
        public void SetUp()
        {
            _client = new Client(
                "http://localhost:5984",
                new BasicCredProvider(
                    "admin",
                    "password"
                )
            );
        }

        [Test, Order(1)]
        public async Task TestAuth()
        {
            var respLogIn = await _client.LogIn();
            Assert.That(HttpHandler.IsSuccessStatusCode(respLogIn));
            Assert.That(_client.IsConnected, "Client not connected!");
            Assert.That(_client.IsLoggedIn, "Client not authenticated!");
            
            var respLogOut = await _client.LogOut();
            Assert.That(HttpHandler.IsSuccessStatusCode(respLogOut));
            Assert.That(!_client.IsLoggedIn, "Log out not successful!");
        }

        [Test, Order(2)]
        public async Task TestCreateDeleteDb()
        {
            var endpointUriBuilder = new UriBuilder(new Uri(_client.ServerUrl));
            endpointUriBuilder.Path = "test_db";

            Func<Task<HttpStatusCode>> getDb = async () => { 
                var resp = await _client.SendRequest(
                    HttpMethod.Get,
                    endpointUriBuilder.Uri,
                    null,
                    null,
                    () => {}
                );
                return resp.StatusCode;
            };

            await _client.CreateDb("test_db");
            Assert.DoesNotThrowAsync(async () => await getDb(), "Getting Db after creation failed!");
            var resp1 = await getDb();
            Assert.That(HttpHandler.IsSuccessStatusCode(resp1), $"Response code from db get after creation was {resp1}");

            await _client.DeleteDb("test_db");
            var resp2 = await getDb();
            Assert.That(!HttpHandler.IsSuccessStatusCode(resp2), "Db should not exist anymore!");
        }

    }
}