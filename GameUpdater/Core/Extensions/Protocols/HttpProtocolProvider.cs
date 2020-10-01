using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace GameUpdater.Core.Extensions.Protocols
{
	// Token: 0x0200002F RID: 47
	public class HttpProtocolProvider : BaseProtocolProvider, IProtocolProvider
	{
		// Token: 0x060001DB RID: 475 RVA: 0x00008D57 File Offset: 0x00006F57
		static HttpProtocolProvider()
		{
			ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(HttpProtocolProvider.certificateCallBack);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00008D6C File Offset: 0x00006F6C
		private static bool certificateCallBack(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00008D80 File Offset: 0x00006F80
		private void FillCredentials(HttpWebRequest request, ResourceLocation rl)
		{
			bool authenticate = rl.Authenticate;
			if (authenticate)
			{
				string text = rl.Login;
				string domain = string.Empty;
				int num = text.IndexOf('\\');
				bool flag = num >= 0;
				if (flag)
				{
					domain = text.Substring(0, num);
					text = text.Substring(num + 1);
				}
				request.Credentials = new NetworkCredential(text, rl.Password)
				{
					Domain = domain
				};
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000028E4 File Offset: 0x00000AE4
		public virtual void Initialize(Downloader downloader)
		{
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00008DF0 File Offset: 0x00006FF0
		public virtual Stream CreateStream(ResourceLocation rl, long initialPosition, long endPosition)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)base.GetRequest(rl);
			this.FillCredentials(httpWebRequest, rl);
			bool flag = initialPosition != 0L;
			if (flag)
			{
				bool flag2 = endPosition == 0L;
				if (flag2)
				{
					httpWebRequest.AddRange(initialPosition);
				}
				else
				{
					httpWebRequest.AddRange(initialPosition, endPosition);
				}
			}
			httpWebRequest.KeepAlive = true;
			httpWebRequest.Pipelined = true;
			return httpWebRequest.GetResponse().GetResponseStream();
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00008E60 File Offset: 0x00007060
		public virtual RemoteFileInfo GetFileInfo(ResourceLocation rl, out Stream stream)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)base.GetRequest(rl);
			this.FillCredentials(httpWebRequest, rl);
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			RemoteFileInfo remoteFileInfo = new RemoteFileInfo();
			remoteFileInfo.MimeType = httpWebResponse.ContentType;
			remoteFileInfo.LastModified = httpWebResponse.LastModified;
			remoteFileInfo.FileSize = httpWebResponse.ContentLength;
			remoteFileInfo.AcceptRanges = (string.Compare(httpWebResponse.Headers["Accept-Ranges"], "bytes", true) == 0);
			stream = httpWebResponse.GetResponseStream();
			return remoteFileInfo;
		}
	}
}
