using NUnit.Framework;
using System.Threading.Tasks;
using Couchy;
using Newtonsoft.Json;

namespace Couchy.Tests
{
    public class TestDoc : IDocument
    {
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }
        public TestDoc(string id, string content)
        {
            Id = id;
            Content = content;
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class TestDocFactory : IDocumentFactory
    {
        public TestDocFactory()
        {

        }

        public TestDoc CreateDocument<TestDoc>(string json) where TestDoc : IDocument
        {
            return JsonConvert.DeserializeObject<TestDoc>(json);
        }
    }

    public class TestDb
    {
        private Client _client;
        private Db _db;
        private TestDoc _doc;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            _doc = new TestDoc(
                "lol",
                "rotflmao"
            );

            _client = new Client(
                "http://localhost:5984",
                new BasicCredProvider(
                    "admin",
                    "password"
                )
            );

            await _client.LogIn();
            await _client.CreateDb("test_couchy");

            _db = new Db(
                "test_couchy",
                _client,
                new TestDocFactory()
            );
        }
        
        [OneTimeTearDown]
        public async Task TearDown()
        {
            await _client.DeleteDb("test_couchy");
            await _client.LogOut();
        }

        [Test, Order(1)]
        public async Task TestDocCreate()
        {
            var response = await _db.Create(_doc);
            Assert.That(HttpHandler.IsSuccessStatusCode(response));
        }

        [Test, Order(2)]
        public async Task TestDocRead()
        {
            var response = await _db.Read<TestDoc>("lol");
            Assert.That(response.Id == "lol");
        }

        [Test, Order(3)]
        public async Task TestDocUpdate()
        {
            var newDoc = new TestDoc(
                "lol",
                "lulz"
            );

            var response = await _db.Update(newDoc);
            Assert.That(HttpHandler.IsSuccessStatusCode(response));
            var readResponse = await _db.Read<TestDoc>("lol");
            Assert.That(((TestDoc)readResponse).Content == "lulz");
        }

        [Test, Order(4)]
        public async Task TestDocDelete()
        {
            var response = await _db.Delete(_doc);
            Assert.That(HttpHandler.IsSuccessStatusCode(response));
            var readResponse = await _db.Exists("lol");
            Assert.That(!HttpHandler.IsSuccessStatusCode(readResponse));
        }

        
    }
}