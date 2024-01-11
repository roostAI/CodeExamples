using PluginInterface;
using System.Collections.Generic;
using System;

namespace SimpleClassLibrary
{
	public interface IComposer
	{
		Lazy<IPlugin, IDictionary<string, object>>[] ComposePlugins(IPluginLoader pluginLoader);
	}
}