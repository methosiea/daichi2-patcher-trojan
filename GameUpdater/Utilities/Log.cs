using System;
using System.Collections.Generic;
using System.IO;

namespace GameUpdater.Utilities
{
	// Token: 0x0200000E RID: 14
	internal class Log
	{
		// Token: 0x06000081 RID: 129 RVA: 0x0000469C File Offset: 0x0000289C
		public static void Clear()
		{
			try
			{
				File.Delete(Settings.Default.LogFile);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000046D4 File Offset: 0x000028D4
		public static void Write(string content, bool buffer = false)
		{
			if (buffer)
			{
				List<string> lineBuffer = Log.LineBuffer;
				List<string> obj = lineBuffer;
				lock (obj)
				{
					Log.LineBuffer.Add(string.Format("{0} :: {1}", DateTime.Now, content));
					return;
				}
			}
			File.AppendAllText(Settings.Default.LogFile, string.Format("{0} :: {1}", DateTime.Now, content) + Environment.NewLine);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004768 File Offset: 0x00002968
		public static void FlushLineBuffer()
		{
			File.AppendAllLines(Settings.Default.LogFile, Log.LineBuffer);
			Log.LineBuffer.Clear();
		}

		// Token: 0x04000046 RID: 70
		public static List<string> LineBuffer = new List<string>();
	}
}
