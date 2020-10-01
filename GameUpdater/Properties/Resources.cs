using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace GameUpdater.Properties
{
	// Token: 0x02000011 RID: 17
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	public class Resources
	{
		// Token: 0x06000092 RID: 146 RVA: 0x0000489C File Offset: 0x00002A9C
		internal Resources()
		{
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000093 RID: 147 RVA: 0x000048A8 File Offset: 0x00002AA8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static ResourceManager ResourceManager
		{
			get
			{
				bool flag = Resources.resourceMan == null;
				if (flag)
				{
					Resources.resourceMan = new ResourceManager("GameUpdater.Properties.Resources", typeof(Resources).Assembly);
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000094 RID: 148 RVA: 0x000048EC File Offset: 0x00002AEC
		// (set) Token: 0x06000095 RID: 149 RVA: 0x00004903 File Offset: 0x00002B03
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000096 RID: 150 RVA: 0x0000490C File Offset: 0x00002B0C
		public static string TITLE
		{
			get
			{
				return Resources.ResourceManager.GetString("TITLE", Resources.resourceCulture);
			}
		}

		// Token: 0x0400004A RID: 74
		private static ResourceManager resourceMan;

		// Token: 0x0400004B RID: 75
		private static CultureInfo resourceCulture;
	}
}
