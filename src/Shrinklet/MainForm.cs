using Shrinklet.ShrinksterDotCom;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;

namespace Shrinklet
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Timer m_Timer;
		private System.Windows.Forms.NotifyIcon m_NotifyIcon;
		private System.ComponentModel.IContainer components;
        private ContextMenuStrip m_ContextMenu;
        private ToolStripMenuItem m_CloseContextMenuItem;
        private TraceSwitch m_ShrinkletSwitch = new TraceSwitch("Shrinklet", string.Empty);

		public MainForm()
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.ctor().");
			}
#line default
			#endregion

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.ctor().");
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
				Trace.WriteLine("Entering MainForm.Dispose(bool).");
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
				Trace.WriteLine("Leaving MainForm.Dispose(bool).");
			}
#line default
			#endregion
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.m_Timer = new System.Windows.Forms.Timer(this.components);
            this.m_NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.m_ContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.m_CloseContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_ContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_Timer
            // 
            this.m_Timer.Enabled = true;
            this.m_Timer.Interval = 1000;
            this.m_Timer.Tick += new System.EventHandler(this.m_Timer_Tick);
            // 
            // m_NotifyIcon
            // 
            this.m_NotifyIcon.ContextMenuStrip = this.m_ContextMenu;
            this.m_NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("m_NotifyIcon.Icon")));
            this.m_NotifyIcon.Text = "Shrinklet";
            this.m_NotifyIcon.Visible = true;
            this.m_NotifyIcon.BalloonTipClicked += new System.EventHandler(this.m_NotifyIcon_BalloonClick);
            this.m_NotifyIcon.BalloonTipClosed += new System.EventHandler(this.m_NotifyIcon_BalloonTimeout);
            this.m_NotifyIcon.Click += new System.EventHandler(this.m_NotifyIcon_BalloonClick);
            // 
            // m_ContextMenu
            // 
            this.m_ContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_CloseContextMenuItem});
            this.m_ContextMenu.Name = "m_ContextMenu";
            this.m_ContextMenu.Size = new System.Drawing.Size(104, 26);
            // 
            // m_CloseContextMenuItem
            // 
            this.m_CloseContextMenuItem.Name = "m_CloseContextMenuItem";
            this.m_CloseContextMenuItem.Size = new System.Drawing.Size(103, 22);
            this.m_CloseContextMenuItem.Text = "Close";
            this.m_CloseContextMenuItem.Click += new System.EventHandler(this.m_CloseMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(346, 152);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "Shrinklet";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.m_ContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private ShrinkletState m_State = ShrinkletState.Scanning;
		
		private string GetClipboardString()
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.GetClipboardString().");
			}
#line default
			#endregion

			string clipboardString = null;

			IDataObject clipboardObject = Clipboard.GetDataObject();
			bool stringPresent = clipboardObject.GetDataPresent(typeof(string));

			if (stringPresent)
			{
				clipboardString = (string)clipboardObject.GetData(typeof(string));
			}

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.GetClipboardString().");
			}
#line default
			#endregion
			
			return clipboardString;			
		}

		private int m_RememberedClipboardStringHashCode = 0;

		private bool HasClipboardStringChanged(string clipboardString)
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.HasClipboardStringChanged(string).");
			}
#line default
			#endregion

			bool hasChanged = false;

			int clipboardStringHashCode = clipboardString.GetHashCode();

			if (clipboardStringHashCode != this.m_RememberedClipboardStringHashCode)
			{
				hasChanged = true;
				this.m_RememberedClipboardStringHashCode = clipboardStringHashCode;
			}

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.HasClipboardStringChanged(string).");
			}
#line default
			#endregion

			return hasChanged;
		}

		private const string m_ShrinkletUrlPattern = "^(http://(www.)?shrinkster.com/([a-zA-Z0-9]*))$";

		private Regex m_ShrinkletUrlRegex;

		private Regex GetShrinkletUrlRegex()
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.GetShrinkletUrlRegex.");
			}
#line default
			#endregion

			if (this.m_ShrinkletUrlRegex == null)
			{
				this.m_ShrinkletUrlRegex = new Regex(MainForm.m_ShrinkletUrlPattern, RegexOptions.Compiled);
			}

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.GetShrinkletUrlRegex.");
			}
#line default
			#endregion

			return this.m_ShrinkletUrlRegex;
		}

		private const string m_UrlPattern = @"^http(s)?://";

		private Regex m_UrlRegex;

		private Regex GetUrlRegex()
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.GetUrlRegex.");
			}
#line default
			#endregion

			if (this.m_UrlRegex == null)
			{
				this.m_UrlRegex = new Regex(MainForm.m_UrlPattern, RegexOptions.Compiled);
			}

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.GetUrlRegex.");
			}
#line default
			#endregion

			return this.m_UrlRegex;
		}

		private int m_MinimumUrlLengthToShrink = -1;

		private int GetMinimumUrlLengthToShrink()
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.GetMinimumUrlLengthToShrink.");
			}
#line default
			#endregion

			if (this.m_MinimumUrlLengthToShrink == -1)
			{
				try
				{
					this.m_MinimumUrlLengthToShrink = 50;
					string minimumUrlLengthToShrinkString = ConfigurationSettings.AppSettings["MinimumUrlLengthToShrink"];
					this.m_MinimumUrlLengthToShrink = int.Parse(minimumUrlLengthToShrinkString);

					if (this.m_MinimumUrlLengthToShrink < 0)
					{
						this.m_MinimumUrlLengthToShrink = 0;
					}
				}
				catch (Exception ex)
				{
					#region Tracing . . .
#line hidden
					if (this.m_ShrinkletSwitch.TraceError)
					{
						Trace.WriteLine(ex);
					}
#line default
					#endregion
				}
			}

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.GetMinimumUrlLengthToShrink.");
			}
#line default
			#endregion

			return this.m_MinimumUrlLengthToShrink;
		}

		private bool IsClipboardStringUrl(string clipboardString)
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.IsClipboardStringUrl(string).");
			}
#line default
			#endregion

            bool isUrl = false;
        
			int minimumUrlLengthToShrink = this.GetMinimumUrlLengthToShrink();

            if (clipboardString.Length >= minimumUrlLengthToShrink)
            {
                Regex urlRegex = this.GetUrlRegex();
                isUrl = urlRegex.IsMatch(clipboardString);
            }

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.IsClipboardStringUrl(string).");
			}
#line default
			#endregion

			return isUrl;
		}

		private void PopOfferToShrinkBalloon()
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.PopOfferToShrinkBalloon().");
			}
#line default
            #endregion

            this.m_NotifyIcon.Visible = true;

            this.m_NotifyIcon.ShowBalloonTip(
                10000,
                "URL Detected",
                "Shrinklet has detected a URL in the clipboard. Click here to replace the long URL in the clipboard with a shorter URL from Shrinkster.com",
                ToolTipIcon.Info
                );


            #region Tracing . . .
#line hidden
            if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.PopOfferToShrinkBalloon().");
			}
#line default
			#endregion
		}

		private bool IsClipboardStringShrinksterUrl(string clipboardString)
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.IsClipboardStringShrinksterUrl(string).");
			}
#line default
			#endregion

			Regex shrinksterUrlRegex = this.GetShrinkletUrlRegex();;
			bool isShrinksterUrl = shrinksterUrlRegex.IsMatch(clipboardString);

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.IsClipboardStringShrinksterUrl(string).");
			}
#line default
			#endregion

			return isShrinksterUrl;
		}

		// REVIEW: http://wiki.notgartner.com/default.aspx/WikiDotNotGartnerDotCom.BewareNestedConstructs
		private void m_Timer_Tick(object sender, System.EventArgs e)
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.m_Timer_Tick(object, EventArgs).");
			}
#line default
			#endregion

			if (this.m_State == ShrinkletState.Scanning)
			{
				string clipboardString = this.GetClipboardString();

				if (clipboardString != null)
				{
					bool clipboardStringChanged = this.HasClipboardStringChanged(clipboardString);
					bool clipboardStringIsUrl = this.IsClipboardStringUrl(clipboardString);
					bool clipboardStringIsShrinksterUrl = this.IsClipboardStringShrinksterUrl(clipboardString);

					if (clipboardStringChanged && clipboardStringIsShrinksterUrl)
					{
						this.m_State = ShrinkletState.Tracking;
						this.PopOfferToTrackShrunkUrlBalloon();
					}
					else if (clipboardStringChanged && clipboardStringIsUrl)
					{
						this.m_State = ShrinkletState.Detected;
						this.PopOfferToShrinkBalloon();
					}
				}
			}

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.m_Timer_Tick(object, EventArgs).");
			}
#line default
			#endregion
		}

		private void PlaceShrunkUrlBackInClipboard(string shrunkUrl)
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.PlaceShrunkUrlBackInClipboard(string).");
			}
#line default
			#endregion

			Clipboard.SetDataObject(shrunkUrl);
			this.m_RememberedClipboardStringHashCode = shrunkUrl.GetHashCode();

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.PlaceShrunkUrlBackInClipboard(string).");
			}
#line default
			#endregion
		}

		private void PopErrorShrinkingBalloon()
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.PopErrorShrinkingBalloon().");
			}
#line default
			#endregion

            this.m_NotifyIcon.ShowBalloonTip(
                10000,
                "Error Shrinking",
                "For some reason Shrinklet was unable to Shrink the URL detected in the clipboard. Are you connected to the Internet?",
                ToolTipIcon.Error
                );
            this.m_State = ShrinkletState.Error;

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.PopErrorShrinkingBalloon().");
			}
#line default
			#endregion
		}

		private string GetShrunkUrlFromShrinksterDotCom(string clipboardUrl)
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.GetShrunkUrlFromShrinksterDotCom(string).");
			}
#line default
			#endregion

			string shrunkUrl = null;

			try
			{
				WebService service = new WebService();
				shrunkUrl = service.ShrinkURL(clipboardUrl);
			}
			catch (Exception ex)
			{
				#region Tracing . . .
#line hidden
				if (this.m_ShrinkletSwitch.TraceWarning)
				{
					Trace.WriteLine(ex);
				}
#line default
				#endregion
			}

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.GetShrunkUrlFromShrinksterDotCom(string).");
			}
#line default
			#endregion

			return shrunkUrl;
		}

		private void ShrinkUrlInClipboard()
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.ShrinkUrlInClipboard().");
			}
#line default
			#endregion

			this.m_State = ShrinkletState.Shrinking;

			string clipboardUrl = this.GetClipboardString();
			string shrunkUrl = this.GetShrunkUrlFromShrinksterDotCom(clipboardUrl);

			if ((shrunkUrl != null) && (shrunkUrl != string.Empty))
			{
				this.PlaceShrunkUrlBackInClipboard(shrunkUrl);
				this.PopOfferToTestShrunkUrlBalloon();
			}
			else
			{
				this.PopErrorShrinkingBalloon();
			}

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.ShrinkUrlInClipboard().");
			}
#line default
			#endregion
		}

		private void PopOfferToTestShrunkUrlBalloon()
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.PopOfferToTestShrunkUrlBalloon().");
			}
#line default
			#endregion

            this.m_NotifyIcon.ShowBalloonTip(
                10000,
                "URL Shrunk",
                "Shrinklet has shrunk the URL and placed it back into the clipboard. Click here to test the URL by launching a web-browser.",
                ToolTipIcon.Info
                );

            #region Tracing . . .
#line hidden
            if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.PopOfferToTestShrunkUrlBalloon().");
			}
#line default
			#endregion
		}

		private void PopOfferToTrackShrunkUrlBalloon()
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.PopOfferToTrackShrunkUrlBalloon().");
			}
#line default
            #endregion

            this.m_NotifyIcon.ShowBalloonTip(
                10000,
                "Track URL",
                "You have a Shrinkster.com URL in the clipboard. Click here to look at the tracking information.",
                ToolTipIcon.Info
                );

            #region Tracing . . .
#line hidden
            if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.PopOfferToTrackShrunkUrlBalloon().");
			}
#line default
			#endregion
		}

		private void LaunchBrowserToUrlInClipboard()
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.LaunchBrowserToUrlInClipboard().");
			}
#line default
			#endregion

			this.m_State = ShrinkletState.Scanning;
			string clipboardUrl = this.GetClipboardString();
			Process.Start(clipboardUrl);

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.LaunchBrowserToUrlInClipboard().");
			}
#line default
			#endregion
		}

		private string GenerateTrackingUrlFromClipboardUrl(string clipboardUrl)
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.GenerateTrackingUrlFromClipboardUrl().");
			}
#line default
			#endregion

			Regex shrinksterUrlRegex = this.GetShrinkletUrlRegex();
			Match match = shrinksterUrlRegex.Match(clipboardUrl);
			string shrinksterCode = match.Groups[3].Value;

			string trackingUrl = string.Format(
				"http://www.shrinkster.com/Track.aspx?AddressID={0}",
				shrinksterCode
				);

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.GenerateTrackingUrlFromClipboardUrl().");
			}
#line default
			#endregion

			return trackingUrl;
		}

		private void LaunchBrowserToTrackShrinkletUrlInClipboard()
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.LaunchBrowserToTrackShrinkletUrlInClipboard().");
			}
#line default
			#endregion

			this.m_State = ShrinkletState.Scanning;
			string clipboardUrl = this.GetClipboardString();
			string trackingUrl = this.GenerateTrackingUrlFromClipboardUrl(clipboardUrl);
			Process.Start(trackingUrl);

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.LaunchBrowserToTrackShrinkletUrlInClipboard().");
			}
#line default
			#endregion
		}

		private void m_NotifyIcon_BalloonClick(object sender, EventArgs e)
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.m_NotifyIcon_BalloonClick(object).");
			}
#line default
			#endregion

			switch (this.m_State)
			{
				case ShrinkletState.Detected:
					this.ShrinkUrlInClipboard();
					break;

				case ShrinkletState.Shrinking:
					this.LaunchBrowserToUrlInClipboard();
					break;

				case ShrinkletState.Tracking:
					this.LaunchBrowserToTrackShrinkletUrlInClipboard();
					break;

				case ShrinkletState.Error:
					this.m_State = ShrinkletState.Scanning;
					break;
			}

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.m_NotifyIcon_BalloonClick(object).");
			}
#line default
			#endregion
		}

		private void m_NotifyIcon_BalloonTimeout(object sender, EventArgs e)
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.m_NotifyIcon_BalloonTimeout(object).");
			}
#line default
			#endregion

			this.m_State = ShrinkletState.Scanning;

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.m_NotifyIcon_BalloonTimeout(object).");
			}
#line default
			#endregion
		}

		private void m_NotifyIcon_BalloonHide(object sender, EventArgs e)
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.m_NotifyIcon_BalloonHide(object).");
			}
#line default
			#endregion

			this.m_State = ShrinkletState.Scanning;

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.m_NotifyIcon_BalloonHide(object).");
			}
#line default
			#endregion
		}

		private void m_CloseMenuItem_Click(object sender, System.EventArgs e)
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.m_CloseMenuItem_Click(object, EventArgs).");
			}
#line default
			#endregion

			this.Close();

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.m_CloseMenuItem_Click(object, EventArgs).");
			}
#line default
			#endregion
		}

		private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.MainForm_Closing(object, CancelEventArgs).");
			}
#line default
			#endregion

			this.m_NotifyIcon.Visible = false;		

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.MainForm_Closing(object, CancelEventArgs).");
			}
#line default
			#endregion
		}

		private void MainForm_Load(object sender, System.EventArgs e)
		{
			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Entering MainForm.MainForm_Load(object, EventArgs).");
			}
#line default
			#endregion

			// HACK: This is here because we need to hide the Window so when
			//       people use ALT-TAB they can't see it in the set of icons.
			this.Hide();

			#region Tracing . . .
#line hidden
			if (this.m_ShrinkletSwitch.TraceVerbose)
			{
				Trace.WriteLine("Leaving MainForm.MainForm_Load(object, EventArgs).");
			}
#line default
			#endregion
		}
	}
}
