using System;

namespace GameUpdater.Core.Common
{
	// Token: 0x02000037 RID: 55
	public static class TimeSpanFormatter
	{
		// Token: 0x0600020A RID: 522 RVA: 0x00009274 File Offset: 0x00007474
		public static string ToString(TimeSpan ts)
		{
			bool flag = ts == TimeSpan.MaxValue;
			string result;
			if (flag)
			{
				result = "?";
			}
			else
			{
				string text = ts.ToString();
				int num = text.LastIndexOf('.');
				bool flag2 = num > 0;
				if (flag2)
				{
					result = text.Remove(num);
				}
				else
				{
					result = text;
				}
			}
			return result;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x000092CC File Offset: 0x000074CC
		public static string FormatDurationSeconds(long seconds)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)seconds);
			string str = "";
			bool flag = timeSpan.TotalHours >= 1.0;
			if (flag)
			{
				str = str + ((int)timeSpan.TotalHours).ToString() + "h ";
			}
			return str + string.Format("{0:%m}m {0:%s}s", timeSpan);
		}
	}
}
