using CommonServiceLocator;
using PluginInterface;
using SimpleClassLibrary;

namespace SimpleConsoleApp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			IService testService = new TestService(); // Your implementation of IService

			var serviceLocator = new SimpleServiceLocator();
			serviceLocator.Register<IService>(new TestService());

			ServiceLocator.SetLocatorProvider(() => serviceLocator);

			var loader = new PluginLoader();

			loader.PluginLocation = @"..\..\..\Plugins\Debug";

			loader.Composer = new Composer();

			loader.LoadAndRunPlugins("realtime");

			loader.LoadAndRunPlugins("simulation");
		}
	}
}
