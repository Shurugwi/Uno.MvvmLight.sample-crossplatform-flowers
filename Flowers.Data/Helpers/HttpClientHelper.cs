using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Flowers.Data.Helpers
{
	/// <summary>
	/// An <see cref="HttpClient"/> helper to create instances.
	/// </summary>
	/// <remarks>
	/// This helper is required for the Wasm target which does not
	/// provide a working <see cref="HttpClient"/> implementation.
	/// </remarks>
    public static class HttpClientHelper
    {
		/// <summary>
		/// Defines a function that will provide a new <see cref="HttpClient"/>.
		/// </summary>
		public static Func<HttpClient> HttpClientBuilder = () => new HttpClient();

		/// <summary>
		/// Creates a new HttpClient instance based on <see cref="HttpClientBuilder"/>
		/// </summary>
		public static HttpClient CreateHttpClient() => HttpClientBuilder();
	}
}
