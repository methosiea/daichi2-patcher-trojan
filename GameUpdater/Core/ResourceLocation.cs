using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GameUpdater.Core
{
	// Token: 0x02000022 RID: 34
	[Serializable]
	public class ResourceLocation
	{
		// Token: 0x06000173 RID: 371 RVA: 0x00007EE0 File Offset: 0x000060E0
		public static ResourceLocation FromURL(string url)
		{
			return new ResourceLocation
			{
				URL = url
			};
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00007F00 File Offset: 0x00006100
		public static ResourceLocation[] FromURLArray(string[] urls)
		{
			List<ResourceLocation> list = new List<ResourceLocation>();
			for (int i = 0; i < urls.Length; i++)
			{
				bool flag = ResourceLocation.IsURL(urls[i]);
				if (flag)
				{
					list.Add(ResourceLocation.FromURL(urls[i]));
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00007F54 File Offset: 0x00006154
		public static ResourceLocation FromURL(string url, bool authenticate, string login, string password)
		{
			return new ResourceLocation
			{
				URL = url,
				Authenticate = authenticate,
				Login = login,
				Password = password
			};
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00007F8C File Offset: 0x0000618C
		// (set) Token: 0x06000177 RID: 375 RVA: 0x00007FA4 File Offset: 0x000061A4
		public string URL
		{
			get
			{
				return this.url;
			}
			set
			{
				this.url = value;
				this.BindProtocolProviderType();
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00007FB8 File Offset: 0x000061B8
		// (set) Token: 0x06000179 RID: 377 RVA: 0x00007FD0 File Offset: 0x000061D0
		public bool Authenticate
		{
			get
			{
				return this.authenticate;
			}
			set
			{
				this.authenticate = value;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00007FDC File Offset: 0x000061DC
		// (set) Token: 0x0600017B RID: 379 RVA: 0x00007FF4 File Offset: 0x000061F4
		public string Login
		{
			get
			{
				return this.login;
			}
			set
			{
				this.login = value;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00008000 File Offset: 0x00006200
		// (set) Token: 0x0600017D RID: 381 RVA: 0x00008018 File Offset: 0x00006218
		public string Password
		{
			get
			{
				return this.password;
			}
			set
			{
				this.password = value;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00008024 File Offset: 0x00006224
		// (set) Token: 0x0600017F RID: 383 RVA: 0x00008058 File Offset: 0x00006258
		public string ProtocolProviderType
		{
			get
			{
				bool flag = this.protocolProviderType == null;
				string result;
				if (flag)
				{
					result = null;
				}
				else
				{
					result = this.protocolProviderType.AssemblyQualifiedName;
				}
				return result;
			}
			set
			{
				bool flag = value == null;
				if (flag)
				{
					this.BindProtocolProviderType();
				}
				else
				{
					this.protocolProviderType = Type.GetType(value);
				}
			}
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00008084 File Offset: 0x00006284
		public IProtocolProvider GetProtocolProvider(Downloader downloader)
		{
			return this.BindProtocolProviderInstance(downloader);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000080A0 File Offset: 0x000062A0
		public void BindProtocolProviderType()
		{
			this.provider = null;
			bool flag = !string.IsNullOrEmpty(this.URL);
			if (flag)
			{
				this.protocolProviderType = ProtocolProviderFactory.GetProviderType(this.URL);
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000080DC File Offset: 0x000062DC
		public IProtocolProvider BindProtocolProviderInstance(Downloader downloader)
		{
			bool flag = this.protocolProviderType == null;
			if (flag)
			{
				this.BindProtocolProviderType();
			}
			bool flag2 = this.provider == null;
			if (flag2)
			{
				this.provider = ProtocolProviderFactory.CreateProvider(this.protocolProviderType, downloader);
			}
			return this.provider;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00008130 File Offset: 0x00006330
		public ResourceLocation Clone()
		{
			return (ResourceLocation)base.MemberwiseClone();
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00008150 File Offset: 0x00006350
		public override string ToString()
		{
			return this.URL;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00008168 File Offset: 0x00006368
		public static bool IsURL(string url)
		{
			return Regex.Match(url, "(?<Protocol>\\w+):\\/\\/(?<Domain>[\\w.]+\\/?)\\S*").ToString() != string.Empty;
		}

		// Token: 0x04000089 RID: 137
		private string url;

		// Token: 0x0400008A RID: 138
		private bool authenticate;

		// Token: 0x0400008B RID: 139
		private string login;

		// Token: 0x0400008C RID: 140
		private string password;

		// Token: 0x0400008D RID: 141
		private Type protocolProviderType;

		// Token: 0x0400008E RID: 142
		private IProtocolProvider provider;
	}
}
