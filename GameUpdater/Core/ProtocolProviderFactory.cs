using System;
using System.Collections;
using System.Diagnostics;

namespace GameUpdater.Core
{
	// Token: 0x0200001F RID: 31
	public static class ProtocolProviderFactory
	{
		// Token: 0x1400000E RID: 14
		// (add) Token: 0x0600015C RID: 348 RVA: 0x00007C18 File Offset: 0x00005E18
		// (remove) Token: 0x0600015D RID: 349 RVA: 0x00007C4C File Offset: 0x00005E4C
		// [DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event EventHandler<ResolvingProtocolProviderEventArgs> ResolvingProtocolProvider;

		// Token: 0x0600015E RID: 350 RVA: 0x00007C7F File Offset: 0x00005E7F
		public static void RegisterProtocolHandler(string prefix, Type protocolProvider)
		{
			ProtocolProviderFactory.protocolHandlers[prefix] = protocolProvider;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00007C90 File Offset: 0x00005E90
		public static IProtocolProvider CreateProvider(string uri, Downloader downloader)
		{
			IProtocolProvider protocolProvider = ProtocolProviderFactory.InternalGetProvider(uri);
			bool flag = downloader != null;
			if (flag)
			{
				protocolProvider.Initialize(downloader);
			}
			return protocolProvider;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00007CBC File Offset: 0x00005EBC
		public static IProtocolProvider GetProvider(string uri)
		{
			return ProtocolProviderFactory.InternalGetProvider(uri);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00007CD4 File Offset: 0x00005ED4
		public static Type GetProviderType(string uri)
		{
			int num = uri.IndexOf("://");
			bool flag = num > 0;
			Type result;
			if (flag)
			{
				string key = uri.Substring(0, num);
				result = (ProtocolProviderFactory.protocolHandlers[key] as Type);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00007D18 File Offset: 0x00005F18
		public static IProtocolProvider CreateProvider(Type providerType, Downloader downloader)
		{
			IProtocolProvider protocolProvider = ProtocolProviderFactory.CreateFromType(providerType);
			bool flag = ProtocolProviderFactory.ResolvingProtocolProvider != null;
			if (flag)
			{
				ResolvingProtocolProviderEventArgs resolvingProtocolProviderEventArgs = new ResolvingProtocolProviderEventArgs(protocolProvider, null);
				ProtocolProviderFactory.ResolvingProtocolProvider(null, resolvingProtocolProviderEventArgs);
				protocolProvider = resolvingProtocolProviderEventArgs.ProtocolProvider;
			}
			bool flag2 = downloader != null;
			if (flag2)
			{
				protocolProvider.Initialize(downloader);
			}
			return protocolProvider;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00007D70 File Offset: 0x00005F70
		private static IProtocolProvider InternalGetProvider(string uri)
		{
			IProtocolProvider protocolProvider = ProtocolProviderFactory.CreateFromType(ProtocolProviderFactory.GetProviderType(uri));
			bool flag = ProtocolProviderFactory.ResolvingProtocolProvider != null;
			if (flag)
			{
				ResolvingProtocolProviderEventArgs resolvingProtocolProviderEventArgs = new ResolvingProtocolProviderEventArgs(protocolProvider, uri);
				ProtocolProviderFactory.ResolvingProtocolProvider(null, resolvingProtocolProviderEventArgs);
				protocolProvider = resolvingProtocolProviderEventArgs.ProtocolProvider;
			}
			return protocolProvider;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00007DBC File Offset: 0x00005FBC
		private static IProtocolProvider CreateFromType(Type type)
		{
			return (IProtocolProvider)Activator.CreateInstance(type);
		}

		// Token: 0x04000082 RID: 130
		private static Hashtable protocolHandlers = new Hashtable();
	}
}
