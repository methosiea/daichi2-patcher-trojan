using System;
using System.IO;
using System.Security.Cryptography;

namespace GameUpdater.Utilities
{
	// Token: 0x0200000A RID: 10
	internal class Crypto
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00004400 File Offset: 0x00002600
		public static MD5CryptoServiceProvider MD5
		{
			get
			{
				MD5CryptoServiceProvider result;
				bool flag = (result = Crypto._md5) == null;
				if (flag)
				{
					result = (Crypto._md5 = new MD5CryptoServiceProvider());
				}
				return result;
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004430 File Offset: 0x00002630
		public static string GetMD5Hash(string pathName)
		{
			try
			{
				using (FileStream fileStream = new FileStream(pathName, FileMode.Open, FileAccess.Read, FileShare.Read, 2097152))
				{
					return BitConverter.ToString(Crypto.MD5.ComputeHash(fileStream)).Replace("-", "").ToUpper();
				}
			}
			catch (Exception)
			{
			}
			return "";
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000044AC File Offset: 0x000026AC
		public static string GetMD5Hash(ref byte[] buffer, int readSize)
		{
			string result;
			using (MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider())
			{
				md5CryptoServiceProvider.Initialize();
				result = BitConverter.ToString(md5CryptoServiceProvider.ComputeHash(buffer, 0, readSize)).Replace("-", "").ToUpper();
			}
			return result;
		}

		// Token: 0x04000040 RID: 64
		private static MD5CryptoServiceProvider _md5;
	}
}
