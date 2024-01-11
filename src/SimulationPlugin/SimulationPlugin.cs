using System;
using CommonServiceLocator;
using PluginInterface;

namespace SimulationPlugin
{
	[PluginMetadata("simulation")]
	public class SimulationPlugin : IPlugin
	{
		public SimulationPlugin()
		{
			_service = ServiceLocator.Current.GetInstance<IService>();
		}

		private IService _service;

		public string Name => "SimulationPlugin";

		public ErrorCode Run()
		{
			if (_service == null) { throw new Exception("service not loaded"); }

			Console.WriteLine("Running SimulationPlugin...");

			switch (_service.GetState())
			{
				case State.Active:
					return ErrorCode.NoError;
				case State.Reactive:
					return ErrorCode.Error1;
				case State.Pending:
					return ErrorCode.Error2;
				case State.Loading:
					return ErrorCode.Error3;
				case State.Initializing:
					return ErrorCode.Error4;
				case State.Restarting:
					return ErrorCode.Error5;
				case State.Starting:
					return ErrorCode.Error6;
				default:
					return ErrorCode.ErrorDefault;
			}
		}
	}
}
