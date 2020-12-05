using System.Threading.Tasks;
using System.Net;

namespace Couchy
{
    public interface IDb
    {
        /// <summary>
        /// Name of the database to operate on.
        /// </summary>
        /// <value>String name of the Db.</value>
        string Name { get; }

        /// <summary>
        /// Client instance to be used with this database.
        /// </summary>
        /// <value>An instance of a class that
        /// implements `IClient`.</value>
        IClient Client { get; }

        /// <summary>
        /// A Factory that supervises creation
        /// of the `IDocument` instances.
        /// </summary>
        /// <value>Any class instance implementing
        /// `IDocumentFactory`</value>
        IDocumentFactory DocumentFactory { get; }

        /// <summary>
        /// Creates a document from `IDocument`
        /// and sends it to the Database.
        /// </summary>
        /// <param name="item">
        /// A class instance implementing `IDocument.
        /// Any class that implements this interface
        /// can be used with the Database wrapper.</param>
        /// <returns>async Task</returns>
        Task<HttpStatusCode> Create(IDocument item);

        /// <summary>
        /// Reads an `IDocument` from the database.
        /// This is a generic method ought to be used
        /// with a class implementing `IDocument`.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="revision"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<IDocument> Read<T>(string id, string revision) where T : IDocument;

        /// <summary>
        /// Tests whether a document with a given ID
        /// exists.
        /// </summary>
        /// <param name="id">Document ID</param>
        /// <returns>Successful status code
        /// if document exists and no errors were
        /// encountered.</returns>
        Task<HttpStatusCode> Exists(string id);

        /// <summary>
        /// Updates an existing `IDocument`.
        /// </summary>
        /// <param name="item">
        /// `IDocument`-implementing class instance.</param>
        /// <returns>async Task</returns>
        Task<HttpStatusCode> Update(IDocument item);

        /// <summary>
        /// Deletes a document from the database.
        /// </summary>
        /// <param name="item">
        /// `IDocument`-implementing class instance.</param>
        /// <returns>async Task</returns>
        Task<HttpStatusCode> Delete(IDocument item);
    }
}