#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "ContinueValueThroughSpread", Category = "Value")]
	#endregion PluginInfo
	public class ValueContinueValueThroughSpreadNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input", DefaultValue = 1.0, IsSingle = true)]
		public ISpread<double> FInput;
		
		[Input("spread n", DefaultValue = 1.0, IsSingle = true)]
		public ISpread<int> FSpreadN;

		[Input("multiplier", DefaultValue = 1.0, IsSingle = true)]
		public ISpread<double> FMult;
		
		[Output("Output")]
		public ISpread<double> FOutput;

		[Import()]
		public ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			int slices = FSpreadN[0];
			FOutput.SliceCount = slices;

			for (int i = 0; i < slices; i++) {
				double val = FInput[0]-i;
//				val += FMult[0]*(i+1);
				FOutput[i] = Math.Max(Math.Min(val,1),0);
//				FOutput[i] = val;
			}

			//FLogger.Log(LogType.Debug, "hi tty!");
		}
	}
}
