using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Couchy
{
    public interface IClient
    {
        /// <summary>
        /// `HttpClient` instance internal to the
        /// Database client.
        /// </summary>
        /// <value>`System.Net.Http.HttpClient` instance</value>
        HttpClient WebClient { get; }

        /// <summary>
        /// TODO: Credential injection.
        /// </summary>
        /// <value></value>
        ICredProvider Credentials { get; }

        CookieContainer CookieJar { get; }

        /// <summary>
        /// URL to the CouchDb Server.
        /// </summary>
        /// <value>`string`</value>
        string ServerUrl { get; set; }

        /// <summary>
        /// Property that can be used to determine
        /// whether existing instance of the client
        /// managed to connect to the server.
        /// </summary>
        /// <value>`true` if a valid response from
        /// the server has been received. `false`
        /// otherwise.</value>
        bool IsConnected { get; }

        /// <summary>
        /// Property used to determine whether an
        /// authorized session has been opened by
        /// the client. Doesn't guarantee that the
        /// session hasn't expired.
        /// </summary>
        /// <value>`true` if the user's session
        /// is active, `false` otherwise</value>
        bool IsLoggedIn { get; }

        /// <summary>
        /// Authorizes the client for a session.
        /// </summary>
        /// <returns>async Task</returns>
        Task<HttpStatusCode> LogIn();

        /// <summary>
        /// Ends client's session.
        /// </summary>
        /// <returns>async Task</returns>
        Task<HttpStatusCode> LogOut();

        /// <summary>
        /// Creates a database with 
        /// </summary>
        /// <param name="name">Name of the database
        /// to be created</param>
        /// <returns>`true` if operation succeeded,
        /// `false` otherwise.</returns>
        Task<HttpStatusCode> CreateDb(string name);

        /// <summary>
        /// Removes the database from the Server.
        /// </summary>
        /// <param name="name">Name of the database
        /// to be deleted</param>
        /// <returns>`true` if operation was
        /// successful, `false` otherwise</returns>
        Task<HttpStatusCode> DeleteDb(string name);
    }
}