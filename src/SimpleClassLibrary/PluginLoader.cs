using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using PluginInterface;

namespace SimpleClassLibrary
{
	public class PluginLoader : IPluginLoader
	{
		
		public Lazy<IPlugin, IDictionary<string, object>>[] plugins;

		public string PluginLocation { get; set; }
		public IComposer Composer { get; set; }

		public int LoadAndRunPlugins(string type)
		{
			if (type == null)
			{
				return 11;
			}

			if (Composer == null)
			{
				return 12;
			}

			if (PluginLocation == null)
			{
				return 13;
			}

			plugins = Composer.ComposePlugins(this);

			var realtimePlugin = plugins.FirstOrDefault(p => p.Metadata["Type"].Equals(type));
			ErrorCode error;
			if (realtimePlugin != null)
			{
				Console.WriteLine($"Loaded plugin: {realtimePlugin.Value.Name}");
				error = realtimePlugin.Value.Run();
				Console.WriteLine($"Plugin returned {error}");
			}
			else
			{
				Console.WriteLine($"No plugins with type: {type}");
				return 14;
			}

			return (int)error;
		}
	}
}
