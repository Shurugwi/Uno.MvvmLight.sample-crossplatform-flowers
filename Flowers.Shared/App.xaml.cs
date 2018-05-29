using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Flowers.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;

namespace Flowers
{
    /// <summary>
    ///     Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        ///     Initializes the singleton application object.  This is the first line of authored code
        ///     executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
#if __ANDROID__
			if (Android.OS.Looper.MainLooper != Android.OS.Looper.MyLooper())
			{
				// This is required because the Xamarin.Inspector, where the app is being created twice.

				//	0x66 in GalaSoft.MvvmLight.Ioc.SimpleIoc.Register<GalaSoft.MvvmLight.Views.INavigationService> C#
 				//	0x3 in GalaSoft.MvvmLight.Ioc.SimpleIoc.Register<GalaSoft.MvvmLight.Views.INavigationService> C#
				//	0x60 in Flowers.App..ctor at C:\s\github\mvvmlight - sample - crossplatform - flowers\Flowers.Shared\App.xaml.cs:41,13    C#
 				//	0x1 in Flowers.Droid.Application..ctor at C:\s\github\mvvmlight - sample - crossplatform - flowers\Flowers.Uno.Droid\Main.cs:24,6 C#
 				//	0xFFFFFFFFFFFFFFFF in System.Reflection.MonoCMethod.InternalInvoke C#
 				//	0x7 in System.Reflection.MonoCMethod.InternalInvoke C#
 				//	0x7E in System.Reflection.MonoCMethod.DoInvoke C#
 				//	0x7 in System.Reflection.MonoCMethod.Invoke C#
 				//	0x12 in System.Reflection.ConstructorInfo.Invoke C#
 				//	0x6D in Java.Interop.TypeManager.CreateProxy C#
 				//	0x114 in Java.Interop.TypeManager.CreateInstance C#
 				//	0xBC in Java.Lang.Object.GetObject C#
 				//	0x23 in Java.Lang.Object._GetObject<Android.Content.Context> C#
 				//	0x2 in Java.Lang.Object.GetObject<Android.Content.Context> C#
 				//	0x31 in Android.App.Application.get_Context C#
 				//	0x14 in Android.App.SyncContext.Post C#
 				//	0x33 in Xamarin.InspectorSupport.<> c.< StartBreakdance > b__5_0 at C:\vsts\_work\5\s\Agents\Xamarin.Interactive.Android\InspectorSupport.cs:41,21  C#
 				//	0x13 in System.Threading.Timer.Scheduler.TimerCB C#
 				//	0x1B in System.Threading.QueueUserWorkItemCallback.System.Threading.IThreadPoolWorkItem.ExecuteWorkItem C#
 				//	0x75 in System.Threading.ThreadPoolWorkQueue.Dispatch C#
 				//	0x0 in System.Threading._ThreadPoolWaitCallback.PerformWaitCallback C#

				return;
			}
#endif

			var nav = new NavigationService();
            nav.Configure(
                ViewModelLocator.DetailsPageKey,
                typeof(DetailsPage));
            nav.Configure(
                ViewModelLocator.AddCommentPageKey,
                typeof(AddCommentPage));

            SimpleIoc.Default.Register<INavigationService>(() => nav);
            SimpleIoc.Default.Register<IDialogService, DialogService>();

            InitializeComponent();
            Suspending += OnSuspending;
        }

		/// <summary>
		///     Invoked when the application is launched normally by the end user.  Other entry points
		///     will be used such as when the application is launched to open a specific file.
		/// </summary>
		/// <param name="e">Details about the launch request and process.</param>
		protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            var rootFrame = Windows.UI.Xaml.Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
				// Create a Frame to act as the navigation context and navigate to the first page
				rootFrame = new Frame()
				{
					Style = Resources["NativeDefaultFrame"] as Style
				};

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

				// Place the frame in the current Window
				Windows.UI.Xaml.Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
				// Ensure the current window is active
				Windows.UI.Xaml.Window.Current.Activate();
            }
        }

        /// <summary>
        ///     Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception($"Failed to load Page {e.SourcePageType.FullName}: {e.Exception}");
        }

        /// <summary>
        ///     Invoked when application execution is being suspended.  Application state is saved
        ///     without knowing whether the application will be terminated or resumed with the contents
        ///     of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}