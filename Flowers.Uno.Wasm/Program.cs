using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Uno.Extensions;

namespace Flowers.Wasm
{
	public class Program
	{
		private static App _app;

		static void Main(string[] args)
		{
			ConfigureFilters(LogExtensionPoint.AmbientLoggerFactory);

			// Override the default http client builder to use the Uno Wasm HttpHandler
			// which mono does not yet provide.
			Data.Helpers.HttpClientHelper.HttpClientBuilder =
				() => new HttpClient(new Uno.UI.Wasm.WasmHttpHandler());

			_app = new App();
		}

		static void ConfigureFilters(ILoggerFactory factory)
		{
			factory
				.WithFilter(new FilterLoggerSettings
					{
						{ "Uno", LogLevel.Warning },
						{ "Windows", LogLevel.Warning },
						{ "SampleControl.Presentation", LogLevel.Debug },

						// Generic Xaml events
						// { "Windows.UI.Xaml", LogLevel.Debug },

						// { "Uno.UI.Controls.AsyncValuePresenter", LogLevel.Debug },
						// { "Uno.UI.Controls.IfDataContext", LogLevel.Debug },
						   
						// Layouter specific messages
						// { "Windows.UI.Xaml.Controls", LogLevel.Debug },
						//{ "Windows.UI.Xaml.Controls.Layouter", LogLevel.Debug },
						//{ "Windows.UI.Xaml.Controls.Panel", LogLevel.Debug },
						   
						// Binding related messages
						// { "Windows.UI.Xaml.Data", LogLevel.Debug },
						// { "Windows.UI.Xaml.Data", LogLevel.Debug },
						   
						//  Binder memory references tracking
						// { "ReferenceHolder", LogLevel.Debug },
					}
				)
				.AddConsole(LogLevel.Debug);
		}

	}
}
