using PluginInterface;

namespace SimpleConsoleApp
{
	internal class TestService : IService
	{
		public State GetState()
		{
			return State.Active;
		}
	}
}