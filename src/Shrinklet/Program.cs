using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Shrinklet
{
	public class Program
	{
		private static TraceSwitch m_ShrinkletSwitch = new TraceSwitch("Shrinklet", string.Empty);

		private static bool IsAlreadyRunning()
		{
			#region Tracing . . .
#line hidden
			if (Program.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering Program.IsAlreadyRunning().");
			}
#line default
			#endregion

			bool alreadyRunning = false;

			Process[] processes = Process.GetProcessesByName("Shrinklet");
			
			if (processes.Length > 1)
			{
				alreadyRunning = true;
			}

			#region Tracing . . .
#line hidden
			if (Program.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving Program.IsAlreadyRunning().");
			}
#line default
			#endregion

			return alreadyRunning;
		}

		[STAThread()]
		public static void Main()
		{
			#region Tracing . . .
#line hidden
			if (Program.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering Program.Main().");
			}
#line default
			#endregion

			bool alreadyRunning = IsAlreadyRunning();

			if (!alreadyRunning)
			{
				MainForm mainForm = new MainForm();

				Application.EnableVisualStyles();
				Application.Run(mainForm);
			}

			#region Tracing . . .
#line hidden
			if (Program.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving Program.Main().");
			}
#line default
			#endregion
		}
	}
}
