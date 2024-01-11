namespace SimpleClassLibrary
{
	public interface IPluginLoader
	{
		string PluginLocation { get; set; }

		int LoadAndRunPlugins(string type);
	}
}