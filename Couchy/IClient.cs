using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

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
        /// A closure-style method that sends any generic
        /// HTTP Request to the CouchDb Server.
        /// </summary>
        /// <param name="httpMethod">
        /// System.Net.Http.HttpMethod</param>
        /// <param name="url">Uri instance</param>
        /// <param name="content">HttpContent instance</param>
        /// <param name="additionalHeaders">
        /// A string:string dictionary of additional headers
        /// to send along with the request. Can be null if not used.</param>
        /// <param name="onFailure">
        /// Parameterless Action to be run when a non-successful
        /// code is encountered or an HTTP Client threw an
        /// error.</param>
        /// <returns>An HttpResponse Message</returns>
        Task<HttpResponseMessage> SendRequest(
            HttpMethod httpMethod,
            Uri url,
            HttpContent content,
            Dictionary<string, string> additionalHeaders,
            Action onFailure
        );

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
        /// <returns>Status Code</returns>
        Task<HttpStatusCode> LogIn();

        /// <summary>
        /// Ends client's session.
        /// </summary>
        /// <returns>Status Code</returns>
        Task<HttpStatusCode> LogOut();

        /// <summary>
        /// Creates a database with 
        /// </summary>
        /// <param name="name">Name of the database
        /// to be created</param>
        /// <returns>Status Code</returns>
        Task<HttpStatusCode> CreateDb(string name);

        /// <summary>
        /// Removes the database from the Server.
        /// </summary>
        /// <param name="name">Name of the database
        /// to be deleted</param>
        /// <returns>Status Code</returns>
        Task<HttpStatusCode> DeleteDb(string name);
    }
}