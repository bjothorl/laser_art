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
	[PluginInfo(Name = "InsertDwellVertices", Category = "Value", Version = "1.0")]
	#endregion PluginInfo
	public class C1_0ValueInsertDwellVerticesNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input", DefaultValue = 1.0)]
		public ISpread<Vector2D> FInput;
		
		[Input("Extra Vertex Factor", DefaultValue = 1.0, IsSingle = true)]
		public ISpread<int> FInputFactor;

		[Output("Output")]
		public ISpread<Vector2D> FOutput;
		
		[Output("Angles")]
		public ISpread<double> FOutputAngle;

		[Output("Extra Points Pr. Vertex")]
		public ISpread<double> FOutputExtraPoints;
		
		[Import()]
		public ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			FOutputAngle.SliceCount = SpreadMax;	
			FOutputExtraPoints.SliceCount = SpreadMax;
			FOutput.SliceCount = FInput.SliceCount;
			
			double angle = 0.0;
			int j = 0;
			
			for (int i = 0; i < FInput.SliceCount; i++)
			{				
				FOutput[j] = FInput[i];
				j++;
				
				angle = GetAngle(FInput[i],FInput[i+1],FInput[i+2]);	
				FOutputAngle[i] = angle;
				
				if (angle<100)
				{
					
				}
			}
		}
		
		public double GetAngle(Vector2D vec1, Vector2D vec2, Vector2D vec3)
	    {
	        double lenghtA = Math.Sqrt(Math.Pow(vec2.x - vec1.x, 2) + Math.Pow(vec2.y - vec1.y,2));
	        double lenghtB = Math.Sqrt(Math.Pow(vec3.x - vec2.x,2) + Math.Pow(vec3.y - vec2.y, 2));
	        double lenghtC = Math.Sqrt(Math.Pow(vec3.x - vec1.x,2) + Math.Pow(vec3.y - vec1.y, 2));
	
	        double calc = ((lenghtA * lenghtA) + (lenghtB * lenghtB) - (lenghtC * lenghtC)) / (2 * lenghtA * lenghtB);
	
	        return Math.Acos(calc) * 57.295;
			
	    }
	}
}
