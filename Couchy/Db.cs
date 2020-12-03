namespace Couchy
{
    // public class Db : IDb
    // {
    //     public async Task Create(IDocument item)
    //     {
    //         //await response.Content.ReadAsStringAsync();
    //         var itemJson = item.ToJson();
    //         await SendRequest(
    //             HttpMethod.Put,
    //             new Uri(ServerUrl + "/" + DbName + "/" + item.Id),
    //             new StringContent(itemJson),
    //             null
    //         );
    //     }

    //     public async Task<string> Read(string id, string revision)
    //     {
    //         if (id is null)
    //         {
    //             throw new System.Exception(
    //                 "Document Id to be fetched cannot be null!"
    //             );
    //         }
    //         return await Client.SendRequest(
    //             HttpMethod.Get,
    //             new Uri(ServerUrl + "/" + DbName + "/" + id),
    //             null,
    //             null
    //         );
    //     }

    //     public async Task Update(IDocument item)
    //     {

    //     }

    //     public async Task Delete(IDocument item)
    //     {
    //         // get latest revision:
    //         var json = await Client.SendRequest(
    //             HttpMethod.Get,
    //             new Uri(ServerUrl + "/" + DbName + "/" + item.Id),
    //             null,
    //             null
    //         );
    //         JObject resp = JObject.Parse(json);
    //         var revision = (string)resp["_rev"];
    //         await SendRequest(
    //             HttpMethod.Delete,
    //             new Uri(ServerUrl + "/" + DbName + "/" + item.Id + "?rev=" + revision ),
    //             null,
    //             null
    //         );
    //     }
    // }
}