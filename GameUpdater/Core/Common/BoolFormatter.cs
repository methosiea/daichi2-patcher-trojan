using System;

namespace GameUpdater.Core.Common
{
	// Token: 0x02000034 RID: 52
	public static class BoolFormatter
	{
		// Token: 0x06000206 RID: 518 RVA: 0x00009140 File Offset: 0x00007340
		public static bool FromString(string s)
		{
			return s == "Yes";
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00009160 File Offset: 0x00007360
		public static string ToString(bool v)
		{
			string result;
			if (v)
			{
				result = "Yes";
			}
			else
			{
				result = "No";
			}
			return result;
		}

		// Token: 0x040000B2 RID: 178
		private const string Yes = "Yes";

		// Token: 0x040000B3 RID: 179
		private const string No = "No";
	}
}
