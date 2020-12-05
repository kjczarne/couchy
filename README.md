# Couchy

Couchy is yet another CouchDb wrapper for .NET. Because I didn't like any of the existing ones.

Implementation here is fairly loosely coupled and the whole project is close enough to bare-metal CouchDb, while providing minimum syntactic sugar for basic CRUD operations.

At the moment Couchy supports single-doc CRUD, database creation and destruction. Credentials are username and password but after the first query, a session is started and Cookies are used instead.

## How to install

I'd strongly recommend using NuGet packages. This project is built originally on .NET 5 but I made sure to provide a secondary project for .NET Standard 2.1.

If your project is using .NET Standard 2.1:
`dotnet add package Couchy.NetStandard2.1`

If your project is using .NET 5.0:
`dotnet add package Couchy`

Since .NET is finally heading the right direction and Xamarin morphs into MAUI which will make use of .NET 6, I will remove the .NET Standard 2.1 variant when .NET 6 LTS is released.

## How to use

For every type of document you will need to create a class that implements the `IDocument` interface. If you're working with a simple one-doc schema you're in luck.

`IDocument` implementation should include serialization logic, while `IDocumentFactory` should include deserialization logic.

The following example uses good old `Newtonsoft.Json` as the Serializer/Deserializer:

```csharp
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
```

You can then open a connection to the server and start working with databases:

```csharp
var doc = new TestDoc(
    "lol",
    "rotflmao"
);

var client = new Client(
    "http://localhost:5984",
    new BasicCredProvider(
        "admin",
        "password"
    )
);

await client.LogIn();
await client.CreateDb("test_couchy");

var db = new Db(
    "test_couchy",
    _client,
    new TestDocFactory()
);

await _db.Create(_doc);
```

All CRUD methods except `Read` pipe through their HTTP Status Codes for easier debugging.

While we're speaking of `Read` there's one important thing to mention:

```csharp
var readDoc = await _db.Read<TestDoc>("lol", null);
((TestDoc)readResponse).Content == "lulz"
```

`Read` is a generic method that creates your injected Document type (in this case `TestDoc`) and immediately casts it to `IDocument`. Using it this way makes it easier to work with the remaining methods seamlessly and whenever you need your original class just cast it as I've shown above.

Under the hood `Read` uses your implementation of `IDocumentFactory` (in this case `TestDocFactory`) to create the document, which you pass to the constructor of the database wrapper.

## Unit tests

A dilligent reader might notice that all the above examples are pulled out of `Couchy.Tests` project. Yeah, it's 1:30 AM when I'm writing this and I have hard time coming up with other examples.

Unit test project is using `NUnit` Framework. You don't need to know much to run them, assumption is that you roll out a basic CouchDb (perhaps on Docker? save yourself some time, mate) on `localhost:5984` with credentials `admin` and `password`. I might generalize this in the future if need be but until somebody starts using this project except from me, why bother?

One caveat with the tests. Both the Client and Database tests are ordered and the whole fixture should be executed at once to ensure that all tests pass. Yes, some tests depend on the results of other tests. Yes, I know this is not optimal. But I genuinely believe that as long as one knows *how* the tests work and that they *do* work, one should not spend time overengineering there.