using System;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace GameUpdater.Utilities
{
	// Token: 0x0200000F RID: 15
	internal class Serialization
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00004798 File Offset: 0x00002998
		public static JavaScriptSerializer JSON
		{
			get
			{
				JavaScriptSerializer result;
				bool flag = (result = Serialization._json) == null;
				if (flag)
				{
					result = (Serialization._json = new JavaScriptSerializer());
				}
				return result;
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000047C7 File Offset: 0x000029C7
		public static void Serialize<T>(string toFile, T data)
		{
			File.WriteAllText(toFile, Serialization.JSON.Serialize(data), Encoding.UTF8);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000047E8 File Offset: 0x000029E8
		public static T DeserializeFile<T>(string filename)
		{
			T result;
			try
			{
				result = Serialization.Deserialize<T>(File.ReadAllText(filename));
			}
			catch (Exception)
			{
				result = default(T);
			}
			return result;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004828 File Offset: 0x00002A28
		public static T Deserialize<T>(string content)
		{
			object obj = null;
			try
			{
				obj = Serialization.JSON.Deserialize<T>(content);
			}
			catch (Exception)
			{
			}
			return (T)((object)obj);
		}

		// Token: 0x04000047 RID: 71
		private static JavaScriptSerializer _json;
	}
}
