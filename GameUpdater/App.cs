using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using GameUpdater.Core.Extensions;
using GameUpdater.Core.Extensions.Protocols;
using GameUpdater.Localization;
using GameUpdater.Properties;
using GameUpdater.Utilities;
using GameUpdater.Utilities.Interop;

namespace GameUpdater
{
	// Token: 0x02000002 RID: 2
	public class App : Application
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static string AssemblyGuid
		{
			get
			{
				return ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), true)[0]).Value;
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002084 File Offset: 0x00000284
		private App()
		{
			Locale.Initialize();
			Log.Clear();
			HttpWebRequest.DefaultCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
			this.extensions = new List<IExtension>();
			this.extensions.Add(new HttpFtpProtocolExtension());
			this.InitExtensions();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020D4 File Offset: 0x000002D4
		private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			string directoryName = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
			string text = string.Format("{0}{1}_{2}{3}{4}{5}{6}.backtrace", new object[]
			{
				Locale.GetString("TITLE").Replace(" ", ""),
				((MainWindow)Application.Current.MainWindow).CurrentVersion,
				DateTime.Now.Day,
				DateTime.Now.Month,
				DateTime.Now.Year,
				DateTime.Now.Hour,
				DateTime.Now.Minute
			});
			MessageBox.Show(e.Exception.Message + Environment.NewLine + string.Format(Locale.GetString("ERROR_UNHANDLED_EXCEPTION"), text), "", MessageBoxButton.OK, MessageBoxImage.Hand);
			try
			{
				File.WriteAllText(Path.Combine(directoryName, text), e.Exception.ToString(), Encoding.UTF8);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, GameUpdater.Properties.Resources.TITLE, MessageBoxButton.OK, MessageBoxImage.Hand);
			}
			e.Handled = true;
			base.Shutdown();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002230 File Offset: 0x00000430
		protected override void OnStartup(StartupEventArgs e)
		{
			App.WM_SHOWFIRSTINSTANCE = WinAPI.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", new object[]
			{
				App.AssemblyGuid
			});
			bool flag;
			App._mutex = new Mutex(true, "Global\\" + App.AssemblyGuid, out flag);
			bool flag2 = !flag;
			if (flag2)
			{
				WinAPI.PostMessage((IntPtr)65535, App.WM_SHOWFIRSTINSTANCE, IntPtr.Zero, IntPtr.Zero);
				base.Shutdown();
			}
			else
			{
				bool flag3 = e.Args.Length != 0 && File.Exists(e.Args[0]);
				if (flag3)
				{
					try
					{
						Log.Write(string.Format("Deleting old patcher {0}", e.Args[0]), false);
						File.Delete(e.Args[0]);
						Log.Write(string.Format("  - Success", new object[0]), false);
					}
					catch (Exception ex)
					{
						Log.Write(string.Format("Unable to remove old patcher... Error: {0}", ex.Message), false);
					}
				}
				base.OnStartup(e);
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002340 File Offset: 0x00000540
		public List<IExtension> Extensions
		{
			get
			{
				return this.extensions;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002358 File Offset: 0x00000558
		public IExtension GetExtensionByType(Type type)
		{
			for (int i = 0; i < this.extensions.Count; i++)
			{
				bool flag = this.extensions[i].GetType() == type;
				if (flag)
				{
					return this.extensions[i];
				}
			}
			return null;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000023B4 File Offset: 0x000005B4
		public void InitExtensions()
		{
			for (int i = 0; i < this.Extensions.Count; i++)
			{
				bool flag = this.Extensions[i] is IInitializable;
				if (flag)
				{
					((IInitializable)this.Extensions[i]).Init();
				}
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002410 File Offset: 0x00000610
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				base.DispatcherUnhandledException += this.Application_DispatcherUnhandledException;
				base.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
				Uri resourceLocator = new Uri("/GameUpdater;component/app.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000246C File Offset: 0x0000066C
		[STAThread]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public static void Main()
		{
			App app = new App();
			app.InitializeComponent();
			app.Run();
		}

		// Token: 0x04000001 RID: 1
		public static int WM_SHOWFIRSTINSTANCE;

		// Token: 0x04000002 RID: 2
		private static Mutex _mutex;

		// Token: 0x04000003 RID: 3
		private List<IExtension> extensions;

		// Token: 0x04000004 RID: 4
		private bool _contentLoaded;
	}
}
