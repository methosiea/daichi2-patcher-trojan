using System;
using System.IO;
using System.Net;

namespace GameUpdater.Core.Extensions.Protocols
{
	// Token: 0x0200002C RID: 44
	public class FtpProtocolProvider : BaseProtocolProvider, IProtocolProvider
	{
		// Token: 0x060001C4 RID: 452 RVA: 0x000089D0 File Offset: 0x00006BD0
		private void FillCredentials(FtpWebRequest request, ResourceLocation rl)
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

		// Token: 0x060001C5 RID: 453 RVA: 0x000028E4 File Offset: 0x00000AE4
		public void Initialize(Downloader downloader)
		{
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00008A40 File Offset: 0x00006C40
		public Stream CreateStream(ResourceLocation rl, long initialPosition, long endPosition)
		{
			FtpWebRequest ftpWebRequest = (FtpWebRequest)base.GetRequest(rl);
			this.FillCredentials(ftpWebRequest, rl);
			ftpWebRequest.Method = "RETR";
			ftpWebRequest.ContentOffset = initialPosition;
			ftpWebRequest.KeepAlive = true;
			return ftpWebRequest.GetResponse().GetResponseStream();
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00008A90 File Offset: 0x00006C90
		public RemoteFileInfo GetFileInfo(ResourceLocation rl, out Stream stream)
		{
			RemoteFileInfo remoteFileInfo = new RemoteFileInfo();
			remoteFileInfo.AcceptRanges = true;
			stream = null;
			FtpWebRequest ftpWebRequest = (FtpWebRequest)base.GetRequest(rl);
			ftpWebRequest.Method = "SIZE";
			this.FillCredentials(ftpWebRequest, rl);
			using (FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse())
			{
				remoteFileInfo.FileSize = ftpWebResponse.ContentLength;
			}
			ftpWebRequest = (FtpWebRequest)base.GetRequest(rl);
			ftpWebRequest.Method = "MDTM";
			this.FillCredentials(ftpWebRequest, rl);
			using (FtpWebResponse ftpWebResponse2 = (FtpWebResponse)ftpWebRequest.GetResponse())
			{
				remoteFileInfo.LastModified = ftpWebResponse2.LastModified;
			}
			return remoteFileInfo;
		}
	}
}
