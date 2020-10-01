using System;
using System.IO;

namespace GameUpdater.Core.Common
{
	// Token: 0x02000036 RID: 54
	public static class PathHelper
	{
		// Token: 0x06000209 RID: 521 RVA: 0x00009230 File Offset: 0x00007430
		public static string GetWithBackslash(string path)
		{
			bool flag = !path.EndsWith(Path.DirectorySeparatorChar.ToString());
			if (flag)
			{
				path += Path.DirectorySeparatorChar.ToString();
			}
			return path;
		}
	}
}
