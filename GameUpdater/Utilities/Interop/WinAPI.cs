using System;
using System.Runtime.InteropServices;

namespace GameUpdater.Utilities.Interop
{
	// Token: 0x02000010 RID: 16
	internal class WinAPI
	{
		// Token: 0x0600008B RID: 139
		[DllImport("user32")]
		public static extern int RegisterWindowMessage(string message);

		// Token: 0x0600008C RID: 140 RVA: 0x0000486C File Offset: 0x00002A6C
		public static int RegisterWindowMessage(string format, params object[] args)
		{
			return WinAPI.RegisterWindowMessage(string.Format(format, args));
		}

		// Token: 0x0600008D RID: 141
		[DllImport("user32")]
		public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

		// Token: 0x0600008E RID: 142
		[DllImport("user32.dll")]
		public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		// Token: 0x0600008F RID: 143
		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		// Token: 0x06000090 RID: 144 RVA: 0x0000488A File Offset: 0x00002A8A
		public static void ShowToFront(IntPtr window)
		{
			WinAPI.ShowWindow(window, 1);
			WinAPI.SetForegroundWindow(window);
		}

		// Token: 0x04000048 RID: 72
		public const int HWND_BROADCAST = 65535;

		// Token: 0x04000049 RID: 73
		public const int SW_SHOWNORMAL = 1;
	}
}
