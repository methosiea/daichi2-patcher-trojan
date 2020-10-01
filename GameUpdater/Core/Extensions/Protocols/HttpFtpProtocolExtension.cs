using System;

namespace GameUpdater.Core.Extensions.Protocols
{
	// Token: 0x0200002D RID: 45
	public class HttpFtpProtocolExtension : IExtension
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00008B74 File Offset: 0x00006D74
		public string Name
		{
			get
			{
				return "HTTP/FTP";
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00008B8B File Offset: 0x00006D8B
		public HttpFtpProtocolExtension() : this(new HttpFtpProtocolParametersSettingsProxy())
		{
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00008B9C File Offset: 0x00006D9C
		public HttpFtpProtocolExtension(IHttpFtpProtocolParameters parameters)
		{
			bool flag = parameters == null;
			if (flag)
			{
				throw new ArgumentNullException("parameters");
			}
			bool flag2 = HttpFtpProtocolExtension.parameters != null;
			if (flag2)
			{
				throw new InvalidOperationException("The type HttpFtpProtocolExtension is already initialized.");
			}
			HttpFtpProtocolExtension.parameters = parameters;
			ProtocolProviderFactory.RegisterProtocolHandler("http", typeof(HttpProtocolProvider));
			ProtocolProviderFactory.RegisterProtocolHandler("https", typeof(HttpProtocolProvider));
			ProtocolProviderFactory.RegisterProtocolHandler("ftp", typeof(FtpProtocolProvider));
		}

		// Token: 0x040000AC RID: 172
		internal static IHttpFtpProtocolParameters parameters;
	}
}
