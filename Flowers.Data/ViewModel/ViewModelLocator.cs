using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Flowers.Design;
using Flowers.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace Flowers.ViewModel
{
    public class ViewModelLocator
    {
        private const bool ForceDesignData = false;
        public const string AddCommentPageKey = "AddCommentPage";
        public const string DetailsPageKey = "DetailsPage";

        static ViewModelLocator()
        {
            Debug.WriteLine("ViewModelLocator");
            ServiceLocator.SetLocatorProvider(() => new Wrapper(SimpleIoc.Default));

            if (UseDesignData)
            {
                if (!SimpleIoc.Default.IsRegistered<INavigationService>())
                {
                    SimpleIoc.Default.Register<INavigationService, DesignNavigationService>();
                }

                SimpleIoc.Default.Register<IFlowersService, DesignFlowersService>();
            }
            else
            {
                SimpleIoc.Default.Register<IFlowersService, FlowersService>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
        }

		// This is required until the mvvmlight extras are built properly.
		private class Wrapper : IServiceLocator
		{
			private SimpleIoc _inner;

			public Wrapper(SimpleIoc @default) => this._inner = @default;

			public IEnumerable<object> GetAllInstances(Type serviceType) => _inner.GetAllInstances(serviceType);
			public IEnumerable<TService> GetAllInstances<TService>() => _inner.GetAllInstances<TService>();
			public object GetInstance(Type serviceType) => _inner.GetInstance(serviceType);
			public object GetInstance(Type serviceType, string key) => _inner.GetInstance(serviceType, key);
			public TService GetInstance<TService>() => _inner.GetInstance<TService>();
			public TService GetInstance<TService>(string key) => _inner.GetInstance<TService>(key);
			public object GetService(Type serviceType) => _inner.GetService(serviceType);
		}

		[SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                Debug.WriteLine("Main");
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        private static bool UseDesignData
        {
            get
            {
                return ViewModelBase.IsInDesignModeStatic
                       || ForceDesignData;
            }
        }
    }
}