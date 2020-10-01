using System;

namespace GameUpdater.Core.Common
{
	// Token: 0x02000035 RID: 53
	public static class ByteFormatter
	{
		// Token: 0x06000208 RID: 520 RVA: 0x00009188 File Offset: 0x00007388
		public static string ToString(long size)
		{
			string[] array = new string[]
			{
				"GB",
				"MB",
				"KB",
				"Bytes"
			};
			long num = (long)Math.Pow(1024.0, (double)(array.Length - 1));
			foreach (string arg in array)
			{
				bool flag = size > num;
				if (flag)
				{
					return string.Format("{0:##.00} {1}", decimal.Divide(size, num), arg);
				}
				num /= 1024L;
			}
			return "0 Bytes";
		}
	}
}
