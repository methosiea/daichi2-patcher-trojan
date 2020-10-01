using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GameUpdater.Controls
{
	// Token: 0x02000038 RID: 56
	public class ProgressBar : UserControl, IComponentConnector
	{
		// Token: 0x0600020C RID: 524 RVA: 0x0000933B File Offset: 0x0000753B
		public ProgressBar()
		{
			this.InitializeComponent();
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000934C File Offset: 0x0000754C
		// (set) Token: 0x0600020E RID: 526 RVA: 0x0000936E File Offset: 0x0000756E
		public long MinValue
		{
			get
			{
				return (long)base.GetValue(ProgressBar.MinProperty);
			}
			set
			{
				base.SetValue(ProgressBar.MinProperty, value);
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600020F RID: 527 RVA: 0x00009384 File Offset: 0x00007584
		// (set) Token: 0x06000210 RID: 528 RVA: 0x000093A6 File Offset: 0x000075A6
		public long MaxValue
		{
			get
			{
				return (long)base.GetValue(ProgressBar.MaxProperty);
			}
			set
			{
				base.SetValue(ProgressBar.MaxProperty, value);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000211 RID: 529 RVA: 0x000093BC File Offset: 0x000075BC
		// (set) Token: 0x06000212 RID: 530 RVA: 0x000093DE File Offset: 0x000075DE
		public long CurrentValue
		{
			get
			{
				return (long)base.GetValue(ProgressBar.ValueProperty);
			}
			set
			{
				base.SetValue(ProgressBar.ValueProperty, value);
			}
		}

		// Token: 0x06000213 RID: 531 RVA: 0x000093F4 File Offset: 0x000075F4
		private static void OnCurrentValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ProgressBar progressBar = d as ProgressBar;
			progressBar.CurrentValue = (long)e.NewValue;
			progressBar.AdjustProgressBar();
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00009424 File Offset: 0x00007624
		private static void OnMaxValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ProgressBar progressBar = d as ProgressBar;
			progressBar.MaxValue = (long)e.NewValue;
			progressBar.AdjustProgressBar();
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00009454 File Offset: 0x00007654
		private static void OnMinValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ProgressBar progressBar = d as ProgressBar;
			progressBar.MinValue = (long)e.NewValue;
			progressBar.AdjustProgressBar();
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00009483 File Offset: 0x00007683
		public void SetProgress(long min, long max)
		{
			this.MinValue = min;
			this.MaxValue = max;
			this.IncrementBy(0L);
			this.DecrementBy(0L);
			this.AdjustProgressBar();
		}

		// Token: 0x06000217 RID: 535 RVA: 0x000094AF File Offset: 0x000076AF
		public void Reset()
		{
			this.MinValue = 0L;
			this.CurrentValue = this.MinValue;
			this.MaxValue = 100L;
			this.AdjustProgressBar();
		}

		// Token: 0x06000218 RID: 536 RVA: 0x000094DC File Offset: 0x000076DC
		public void IncrementBy(long i = 1L)
		{
			this.CurrentValue += i;
			bool flag = this.CurrentValue >= this.MaxValue;
			if (flag)
			{
				this.CurrentValue = this.MaxValue;
			}
			this.AdjustProgressBar();
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00009524 File Offset: 0x00007724
		public void DecrementBy(long i = 1L)
		{
			this.CurrentValue -= i;
			bool flag = this.CurrentValue <= this.MinValue;
			if (flag)
			{
				this.CurrentValue = this.MinValue;
			}
			this.AdjustProgressBar();
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000956C File Offset: 0x0000776C
		public void AdjustProgressBar()
		{
			try
			{
				this.gFullImg.Width = base.MaxWidth / Convert.ToDouble(this.MaxValue) * Convert.ToDouble(this.CurrentValue);
			}
			catch (Exception)
			{
			}
			base.UpdateLayout();
		}

		// Token: 0x0600021B RID: 539 RVA: 0x000095C4 File Offset: 0x000077C4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GameUpdater;component/assets/controls/progressbar.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x0600021C RID: 540 RVA: 0x000095FC File Offset: 0x000077FC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			bool flag = connectionId == 1;
			if (flag)
			{
				this.gFullImg = (Grid)target;
			}
			else
			{
				this._contentLoaded = true;
			}
		}

		// Token: 0x040000B4 RID: 180
		public static readonly DependencyProperty MinProperty = DependencyProperty.Register("MinValue", typeof(long), typeof(ProgressBar), new PropertyMetadata(0L, new PropertyChangedCallback(ProgressBar.OnMinValuePropertyChanged)));

		// Token: 0x040000B5 RID: 181
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("CurrentValue", typeof(long), typeof(ProgressBar), new PropertyMetadata(0L, new PropertyChangedCallback(ProgressBar.OnCurrentValuePropertyChanged)));

		// Token: 0x040000B6 RID: 182
		public static readonly DependencyProperty MaxProperty = DependencyProperty.Register("MaxValue", typeof(long), typeof(ProgressBar), new PropertyMetadata(100L, new PropertyChangedCallback(ProgressBar.OnMaxValuePropertyChanged)));

		// Token: 0x040000B7 RID: 183
		internal Grid gFullImg;

		// Token: 0x040000B8 RID: 184
		private bool _contentLoaded;
	}
}
