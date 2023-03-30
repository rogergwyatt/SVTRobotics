using System;
namespace SVTTestProject.Models
{
	public class LoadAssignment
	{
		public string? robotId { get; set; }
		public double distanceToGoal { get; set; }
		public int batteryLevel { get; set; }
	}
}

