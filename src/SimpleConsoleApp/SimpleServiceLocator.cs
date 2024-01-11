using CommonServiceLocator;
using System;
using System.Collections.Generic;

namespace SimpleConsoleApp
{

	public class SimpleServiceLocator : ServiceLocatorImplBase
	{
		private readonly Dictionary<Type, object> services;

		public SimpleServiceLocator()
		{
			services = new Dictionary<Type, object>();
		}

		public void Register<T>(T service)
		{
			services[typeof(T)] = service;
		}

		protected override object DoGetInstance(Type serviceType, string key)
		{
			object service;
			services.TryGetValue(serviceType, out service);
			return service;
		}

		protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
		{
			return new List<object> { DoGetInstance(serviceType, null) };
		}
	}

}