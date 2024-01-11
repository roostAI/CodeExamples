using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using PluginInterface;

namespace SimpleClassLibrary
{
	public class Composer : IComposer
	{
		[ImportMany]
		public Lazy<IPlugin, IDictionary<string, object>>[] plugins;

		public Lazy<IPlugin, IDictionary<string, object>>[] ComposePlugins(IPluginLoader pluginLoader)
		{
			var catalog = new DirectoryCatalog(pluginLoader.PluginLocation);
			var container = new CompositionContainer(catalog);
			container.ComposeParts(this);

			return plugins;
		}
	}
}