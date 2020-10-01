using System;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace GameUpdater.Localization
{
	// Token: 0x02000013 RID: 19
	internal class Locale
	{
		// Token: 0x0600009A RID: 154 RVA: 0x00004964 File Offset: 0x00002B64
		public static void Initialize()
		{
			string name = CultureInfo.CurrentCulture.Parent.Name;
			bool flag = name != null && name == "de";
			if (flag)
			{
				Locale.resMgr = new ResourceManager("GameUpdater.Localization.Languages.de", Assembly.GetExecutingAssembly());
			}
			else
			{
				Locale.resMgr = new ResourceManager("GameUpdater.Localization.Languages.en", Assembly.GetExecutingAssembly());
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000049C4 File Offset: 0x00002BC4
		public static string GetLocale()
		{
			string name = CultureInfo.CurrentCulture.Parent.Name;
			bool flag = name != null && name == "de";
			string result;
			if (flag)
			{
				result = "de";
			}
			else
			{
				result = "en";
			}
			return result;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004A0C File Offset: 0x00002C0C
		public static string GetString(string key)
		{
			return Locale.resMgr.GetString(key);
		}

		// Token: 0x0400004D RID: 77
		private static ResourceManager resMgr;
	}
}
