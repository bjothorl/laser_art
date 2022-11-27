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
	[PluginInfo(Name = "SortByClosestNextPoint", Category = "Value")]
	#endregion PluginInfo
	public class ValueSortByClosestNextPointNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input", DefaultValue = 1.0)]
		public ISpread<double> FInput;
		
		[Input("Input colors")]  
		public ISpread<RGBAColor> FColsInput; 
		
		[Input("Black distance cutoff", DefaultValue = 0.2, IsSingle = true)]
		public ISpread<double> FDistInput;
		
		[Input("Black", IsSingle = true)]  
		public ISpread<RGBAColor> FColsBlack; 
		
		[Output("Output")]
		public ISpread<double> FOutput;
		
		[Output("Output colors")]
		public ISpread<RGBAColor> FColsOutput; 
		
		
		[Import()]
		public ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{			
			FOutput.SliceCount = SpreadMax;
			FColsOutput.SliceCount = FColsInput.SliceCount;
		
			double closestX = 0;
			double closestY = 0;
			int savedIndex = 0;
			
			FOutput[0] = FInput[0];
			FOutput[1] = FInput[1];
			
			FColsOutput[0] = FColsInput[0];
			
			for (int i = 0; i < SpreadMax-2; i+=2)
			{
				double lowestDistance = 10;		
				double x1 = FInput[i];
				double y1 = FInput[i+1];
				
				for (int j = i+2; j < FInput.SliceCount; j+=2)
				{
					double x2 = FInput[j];
					double y2 = FInput[j+1];
					double distance = Math.Sqrt((Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
					if (distance<lowestDistance)
					{
						closestX = x2;
						closestY = y2;
						savedIndex = j;
						lowestDistance = distance;
					}
				}
				
				FInput[savedIndex] = FInput[i+2];
				FInput[savedIndex+1] = FInput[i+3];
				
				FInput[i+2] = closestX;
				FInput[i+3] = closestY;
				
				FOutput[i+2] = closestX;
				FOutput[i+3] = closestY;	
				
				if (lowestDistance < FDistInput[0]) {
					FColsOutput[i/2] = FColsInput[savedIndex/2];
				}
				else {
					FColsOutput[i/2] = FColsBlack[0];
				}
			}			
		}
	}
}
