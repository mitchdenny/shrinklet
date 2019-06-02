using System;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Reflection;

namespace Shrinklet
{
	/// <summary>
	/// Summary description for ShrinkletInstaller.
	/// </summary>
	[RunInstaller(true)]
	public class ShrinkletInstaller : System.Configuration.Install.Installer
	{
		private TraceSwitch m_ShrinkletSwitch = new TraceSwitch("Shrinklet", string.Empty);

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ShrinkletInstaller()
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering ShrinkletInstaller.ctor().");
			}
#line default
			#endregion

			// This call is required by the Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving ShrinkletInstaller.ctor().");
			}
#line default
			#endregion
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering ShrinkletInstaller.Dispose(bool).");
			}
#line default
			#endregion

			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving ShrinkletInstaller.Dispose(bool).");
			}
#line default
			#endregion
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// ShrinkletInstaller
			// 
			this.BeforeUninstall += new System.Configuration.Install.InstallEventHandler(this.ShrinkletInstaller_BeforeUninstall);
			this.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.ShrinkletInstaller_AfterInstall);

		}
		#endregion

		private void StartShrinkletProcess()
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering ShrinkletInstaller.StartShrinkletProcess().");
			}
#line default
			#endregion

			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string executablePath = executingAssembly.Location;
			Process.Start(executablePath);

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving ShrinkletInstaller.StartShrinkletProcess().");
			}
#line default
			#endregion
		}

		private void ShrinkletInstaller_AfterInstall(object sender, System.Configuration.Install.InstallEventArgs e)
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering ShrinkletInstaller.ShrinkletInstaller_AfterInstall(object, InstallEventArgs).");
			}
#line default
			#endregion

			this.StartShrinkletProcess();

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving ShrinkletInstaller.ShrinkletInstaller_AfterInstall(object, InstallEventArgs).");
			}
#line default
			#endregion
		}

		private void KillRunningShrinkletInstances()
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering ShrinkletInstaller.KillRunningShrinkletInstances().");
			}
#line default
			#endregion

			Process[] processes = Process.GetProcessesByName("Shrinklet");			
			foreach (Process process in processes)
			{
				process.Kill();
			}

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving ShrinkletInstaller.KillRunningShrinkletInstances().");
			}
#line default
			#endregion
		}

		private void ShrinkletInstaller_BeforeUninstall(object sender, System.Configuration.Install.InstallEventArgs e)
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering ShrinkletInstaller.ShrinkletInstaller_BeforeUninstall(object, InstallEventArgs).");
			}
#line default
			#endregion

			this.KillRunningShrinkletInstances();

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving ShrinkletInstaller.ShrinkletInstaller_BeforeUninstall(object, InstallEventArgs).");
			}
#line default
			#endregion
		}
	}
}
