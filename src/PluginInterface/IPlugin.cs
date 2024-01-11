using System;
using System.ComponentModel.Composition;

namespace PluginInterface
{

	public interface IPlugin
	{
		string Name { get; }
		ErrorCode Run();
	}

	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class PluginMetadataAttribute : ExportAttribute
	{
		public PluginMetadataAttribute(string type) : base(typeof(IPlugin))
		{
			Type = type;
		}

		public string Type { get; }
	}
}
