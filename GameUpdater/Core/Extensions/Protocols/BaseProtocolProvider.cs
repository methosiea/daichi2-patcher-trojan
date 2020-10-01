using System;
using System.Net;

namespace GameUpdater.Core.Extensions.Protocols
{
	// Token: 0x0200002B RID: 43
	public class BaseProtocolProvider
	{
		// Token: 0x060001C0 RID: 448 RVA: 0x000088F5 File Offset: 0x00006AF5
		static BaseProtocolProvider()
		{
			ServicePointManager.DefaultConnectionLimit = int.MaxValue;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00008904 File Offset: 0x00006B04
		protected WebRequest GetRequest(ResourceLocation location)
		{
			WebRequest webRequest = WebRequest.Create(location.URL);
			webRequest.Timeout = 10000;
			this.SetProxy(webRequest);
			return webRequest;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00008938 File Offset: 0x00006B38
		protected void SetProxy(WebRequest request)
		{
			bool useProxy = HttpFtpProtocolExtension.parameters.UseProxy;
			if (useProxy)
			{
				request.Proxy = new WebProxy(HttpFtpProtocolExtension.parameters.ProxyAddress, HttpFtpProtocolExtension.parameters.ProxyPort)
				{
					BypassProxyOnLocal = HttpFtpProtocolExtension.parameters.ProxyByPassOnLocal
				};
				bool flag = !string.IsNullOrEmpty(HttpFtpProtocolExtension.parameters.ProxyUserName);
				if (flag)
				{
					request.Proxy.Credentials = new NetworkCredential(HttpFtpProtocolExtension.parameters.ProxyUserName, HttpFtpProtocolExtension.parameters.ProxyPassword, HttpFtpProtocolExtension.parameters.ProxyDomain);
				}
			}
		}
	}
}
