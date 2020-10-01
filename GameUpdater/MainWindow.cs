#define ADDON

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shell;
using System.Windows.Threading;
using GameUpdater.Core;
using GameUpdater.Core.Common;
using GameUpdater.Localization;
using GameUpdater.Utilities;
using GameUpdater.Utilities.Interop;

namespace GameUpdater
{
    // Token: 0x02000005 RID: 5
    public class MainWindow : Window, IComponentConnector
    {
        // Token: 0x17000003 RID: 3
        // (get) Token: 0x0600000A RID: 10 RVA: 0x00002490 File Offset: 0x00000690
        public TaskbarItemInfo TaskbarInfo
        {
            get
            {
                return ((MainWindow)System.Windows.Application.Current.MainWindow).TaskbarItemInfo;
            }
        }

        // Token: 0x0600000B RID: 11 RVA: 0x000024B8 File Offset: 0x000006B8
        public MainWindow()
        {
#if ADDON
            Addon.Initialize(RealVersion, CurrentVersion);
#endif

            this.InitializeComponent();
            this.InitTrayIcon();
            this.InitializeInstallationPath();
            this.m_rateTimer = new System.Threading.Timer(new TimerCallback(this.Timer_RefreshSpeedRate), null, 0, 500);
            this.m_progressTimer = new System.Threading.Timer(new TimerCallback(this.Timer_ProgressUpdater), null, 0, 400);
            this.m_updateCheckTimer = new System.Threading.Timer(new TimerCallback(this.Timer_UpdateCheck), null, 0, 10800000);
            base.MouseLeftButtonDown += this.MainWindow_MouseLeftButtonDown;
            base.Loaded += this.MainWindow_Loaded;
            base.Closing += this.MainWindow_Closing;
            DownloadManager.Instance.DownloadAdded += this.Instance_DownloadAdded;
            DownloadManager.Instance.DownloadEnded += this.Instance_DownloadEnded;
            this.OnStateChange(EState.NONE);
        }

        // Token: 0x0600000C RID: 12 RVA: 0x000025EE File Offset: 0x000007EE
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            this.m_isClosing = true;
            DownloadManager.Instance.PauseAll();
            FileSystem.DeleteFiles(this.m_appPath, Settings.Default.TempFileExtensionPattern);
        }

        // Token: 0x0600000D RID: 13 RVA: 0x00002619 File Offset: 0x00000819
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Log.Write(string.Format("Starting {0} v{1}", Locale.GetString("TITLE"), this.CurrentVersion), false);
            this.StartAnimation();
            this.OnStateChange(EState.SERVER_LIST);
        }

        // Token: 0x0600000E RID: 14 RVA: 0x0000264C File Offset: 0x0000084C
        private void InitializeInstallationPath()
        {
            this.m_appPath = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            CUpdaterConfig cupdaterConfig = Serialization.DeserializeFile<CUpdaterConfig>("updater.cfg");
            bool flag = cupdaterConfig != null && !Directory.Exists(cupdaterConfig.InstallPath);
            if (flag)
            {
                cupdaterConfig = null;
            }
            bool flag2 = cupdaterConfig == null;
            if (flag2)
            {
                bool flag3 = !File.Exists("metin2client.exe");
                if (flag3)
                {
                    try
                    {
                        Directory.CreateDirectory(Locale.GetString("INSTALL_DIR_NAME"));
                    }
                    catch (Exception ex)
                    {
                        Log.Write(string.Format("Unable to create installation directory - Error: {0}", ex.Message), false);
                        this.TaskbarInfo.ProgressState = TaskbarItemProgressState.Error;
                        System.Windows.MessageBox.Show(Locale.GetString("ERROR_CREATE_INSTALL_PATH"), Locale.GetString("TITLE"), MessageBoxButton.OK, MessageBoxImage.Hand);
                        return;
                    }
                    DirectoryInfo directoryInfo = new DirectoryInfo(Locale.GetString("INSTALL_DIR_NAME"));
                    this.m_appPath = directoryInfo.FullName;
                }
                cupdaterConfig = new CUpdaterConfig();
                cupdaterConfig.InstallPath = this.m_appPath;
                try
                {
                    File.Copy(Assembly.GetExecutingAssembly().Location, Path.Combine(this.m_appPath, string.Format("{0}.exe", Locale.GetString("TITLE"))));
                    goto IL_14B;
                }
                catch (Exception ex2)
                {
                    Log.Write(string.Format("Unable to copy patcher inside installation directory - Error: {0}", ex2.Message), false);
                    goto IL_14B;
                }
            }
            this.m_appPath = cupdaterConfig.InstallPath;
        IL_14B:
            bool flag4 = !this.m_appPath.EndsWith("\\");
            if (flag4)
            {
                this.m_appPath += "\\";
            }
            Serialization.Serialize<CUpdaterConfig>(Path.Combine(this.m_appPath, "updater.cfg"), cupdaterConfig);
        }

        // Token: 0x0600000F RID: 15 RVA: 0x0000280C File Offset: 0x00000A0C
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            (PresentationSource.FromVisual(this) as HwndSource).AddHook(new HwndSourceHook(this.WndProc));
        }

        // Token: 0x06000010 RID: 16 RVA: 0x00002834 File Offset: 0x00000A34
        protected IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            bool flag = msg == App.WM_SHOWFIRSTINSTANCE;
            if (flag)
            {
                base.WindowState = WindowState.Normal;
                base.Show();
                WinAPI.ShowToFront(new WindowInteropHelper(this).Handle);
            }
            return IntPtr.Zero;
        }

        // Token: 0x06000011 RID: 17 RVA: 0x00002879 File Offset: 0x00000A79
        private void StartAnimation()
        {
            ((Storyboard)base.Resources["LoadingOverlayAnimation"]).Begin();
        }

        // Token: 0x06000012 RID: 18 RVA: 0x00002897 File Offset: 0x00000A97
        private void StopAnimation()
        {
            ((Storyboard)base.Resources["LoadingOverlayAnimation"]).Pause();
        }

        // Token: 0x06000013 RID: 19 RVA: 0x000028B8 File Offset: 0x00000AB8
        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool flag = e.ButtonState == MouseButtonState.Pressed;
            if (flag)
            {
                base.DragMove();
                e.Handled = true;
            }
        }

        // Token: 0x06000014 RID: 20 RVA: 0x000028E4 File Offset: 0x00000AE4
        private void btnStart_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        // Token: 0x06000015 RID: 21 RVA: 0x000028E4 File Offset: 0x00000AE4
        private void btnStart_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        // Token: 0x06000016 RID: 22 RVA: 0x000028E7 File Offset: 0x00000AE7
        private void btnStart_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.LaunchGame();
        }

        // Token: 0x06000017 RID: 23 RVA: 0x000028F1 File Offset: 0x00000AF1
        private void btnStart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        // Token: 0x06000018 RID: 24 RVA: 0x000028FC File Offset: 0x00000AFC
        private void btnClose_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.btnClose.Source = new BitmapImage(new Uri("Assets/Images/btn_quit_hover.png", UriKind.Relative));
        }

        // Token: 0x06000019 RID: 25 RVA: 0x0000291B File Offset: 0x00000B1B
        private void btnClose_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.btnClose.Source = new BitmapImage(new Uri("Assets/Images/btn_quit_normal.png", UriKind.Relative));
        }

        // Token: 0x0600001A RID: 26 RVA: 0x0000293A File Offset: 0x00000B3A
        private void btnClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.btnClose.Source = new BitmapImage(new Uri("Assets/Images/btn_quit_press.png", UriKind.Relative));
            e.Handled = true;
        }

        // Token: 0x0600001B RID: 27 RVA: 0x00002961 File Offset: 0x00000B61
        private void btnMinimize_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.btnMinimize.Source = new BitmapImage(new Uri("Assets/Images/btn_minimize_press.png", UriKind.Relative));
        }

        // Token: 0x0600001C RID: 28 RVA: 0x00002980 File Offset: 0x00000B80
        private void btnMinimize_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.btnMinimize.Source = new BitmapImage(new Uri("Assets/Images/btn_minimize_normal.png", UriKind.Relative));
        }

        // Token: 0x0600001D RID: 29 RVA: 0x0000299F File Offset: 0x00000B9F
        private void btnMinimize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.btnMinimize.Source = new BitmapImage(new Uri("Assets/Images/btn_minimize_press.png", UriKind.Relative));
            e.Handled = true;
        }

        // Token: 0x0600001E RID: 30 RVA: 0x000029C6 File Offset: 0x00000BC6
        private void btnMinimize_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.btnMinimize.Source = new BitmapImage(new Uri("Assets/Images/btn_minimize_normal.png", UriKind.Relative));
            e.Handled = true;
            base.WindowState = WindowState.Minimized;
        }

        // Token: 0x0600001F RID: 31 RVA: 0x000029F5 File Offset: 0x00000BF5
        private void btnClose_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.btnClose.Source = new BitmapImage(new Uri("Assets/Images/btn_quit_up.png", UriKind.Relative));
            System.Windows.Application.Current.Shutdown();
        }

        // Token: 0x06000020 RID: 32 RVA: 0x00002A20 File Offset: 0x00000C20
        private void TxtHomeLink_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                bool flag = this.m_patchList != null;
                if (flag)
                {
                    Process.Start(this.m_patchList.WebsiteURL);
                }
                else
                {
                    Process.Start(Settings.Default.WebsiteURL);
                }
            }
            catch (Exception)
            {
            }
        }

        // Token: 0x06000021 RID: 33 RVA: 0x00002A7C File Offset: 0x00000C7C
        private void TxtRegisterLink_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                bool flag = this.m_patchList != null;
                if (flag)
                {
                    Process.Start(this.m_patchList.ForumURL);
                }
                else
                {
                    Process.Start(Settings.Default.ForumURL);
                }
            }
            catch (Exception)
            {
            }
        }

        // Token: 0x06000022 RID: 34 RVA: 0x00002AD8 File Offset: 0x00000CD8
        private void TxtSupportLink_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                bool flag = this.m_patchList != null;
                if (flag)
                {
                    Process.Start(this.m_patchList.SupportURL);
                }
                else
                {
                    Process.Start(Settings.Default.SupportURL);
                }
            }
            catch (Exception)
            {
            }
        }

        // Token: 0x06000023 RID: 35 RVA: 0x00002B34 File Offset: 0x00000D34
        private void BtnConfig_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.btnConfig.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Images/btn_config_hover.png"));
        }

        // Token: 0x06000024 RID: 36 RVA: 0x00002B52 File Offset: 0x00000D52
        private void BtnConfig_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.btnConfig.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Images/btn_config_norm.png"));
        }

        // Token: 0x06000025 RID: 37 RVA: 0x00002B70 File Offset: 0x00000D70
        private void BtnConfig_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.btnConfig.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Images/btn_config_press.png"));
            this.LaunchConfig();
        }

#if ADDON
        private const string RealVersion = "1.0.0.5" + " ";
#endif

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000026 RID: 38 RVA: 0x00002B98 File Offset: 0x00000D98
        public string CurrentVersion
        {
            get
            {
#if ADDON
                // Added invisible sign so that the version does not look different from the original.
                return "1.0.0.5";
#else
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
#endif
            }
        }

        // Token: 0x06000027 RID: 39 RVA: 0x00002BC0 File Offset: 0x00000DC0
        private void Timer_RefreshSpeedRate(object data)
        {
            bool flag = this.m_appState != EState.DOWNLOAD_FILES;
            if (!flag)
            {
                this.txtDLSpeed.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate ()
                {
                    this.txtDLSpeed.Visibility = Visibility.Visible;
                    this.m_currentDownloadRate.Add((long)DownloadManager.Instance.TotalDownloadRate);
                    bool flag2 = this.m_currentDownloadRate.Count > 5;
                    if (flag2)
                    {
                        this.m_currentDownloadRate.RemoveAt(0);
                    }
                    string text = ByteFormatter.ToString((long)this.m_currentDownloadRate.Average());
                    this.txtDLSpeed.Text = string.Format("{0} {1}", text, (!text.Contains("Bytes")) ? "/s" : "");
                }));
            }
        }

        // Token: 0x06000028 RID: 40 RVA: 0x00002C00 File Offset: 0x00000E00
        private void Timer_ProgressUpdater(object data)
        {
            bool flag = this.m_appState != EState.DOWNLOAD_FILES;
            if (!flag)
            {
                base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate ()
                {
                    this.txtDLProgress.Visibility = Visibility.Visible;
                    this.txtDLProgress.Text = string.Format("{0}%", (int)(DownloadManager.Instance.TotalDownloadedBytes / (double)this.m_totalDownloadSize * 100.0));
                    this.UpdateTaskbarProgressState(DownloadManager.Instance.TotalDownloadedBytes, (double)this.m_totalDownloadSize);
                    long seconds = (long)((double)this.m_totalDownloadSize - DownloadManager.Instance.TotalDownloadedBytes) / ((long)this.m_currentDownloadRate.Average() + 1L);
                    this.txtRemainingTime.Text = string.Format(Locale.GetString("REMAINING_TIME"), ByteFormatter.ToString(Math.Max(0L, this.m_totalDownloadSize - (long)((int)DownloadManager.Instance.TotalDownloadedBytes))), TimeSpanFormatter.FormatDurationSeconds(seconds));
                }));
            }
        }

        // Token: 0x06000029 RID: 41 RVA: 0x00002C3C File Offset: 0x00000E3C
        private void Timer_UpdateCheck(object data)
        {
            bool flag = this.m_appState != EState.COMPLETE;
            if (!flag)
            {
                this.OnStateChange(EState.CHECK_UPDATE);
            }
        }

        // Token: 0x0600002A RID: 42 RVA: 0x00002C68 File Offset: 0x00000E68
        private void UpdateTaskbarProgressState(double current, double max)
        {
            double num = ((current == 0.0) ? 1.0 : current) / max;
            this.TaskbarInfo.ProgressValue = ((num < 0.1) ? 0.1 : num);
        }

        // Token: 0x0600002B RID: 43 RVA: 0x00002CB5 File Offset: 0x00000EB5
        private void Instance_DownloadAdded(object sender, DownloaderEventArgs e)
        {
            base.Dispatcher.Invoke(new Action(delegate ()
            {
                EState appState = this.m_appState;
            }), new object[0]);
        }

        // Token: 0x0600002C RID: 44 RVA: 0x00002CD8 File Offset: 0x00000ED8
        private void Instance_DownloadEnded(object sender, DownloaderEventArgs e)
        {
            switch (this.m_appState)
            {
                case EState.SERVER_LIST:
                    {
                        string localFile = e.Downloader.LocalFile;
                        Log.Write(string.Format("Serverlist saved to {0}", localFile), false);
                        this.m_serverList = Serialization.DeserializeFile<CServerList>(localFile);
                        FileSystem.DeleteFile(localFile);
                        bool flag = this.m_serverList == null;
                        if (flag)
                        {
                            Log.Write("Error deserializing serverlist.", false);
                            System.Windows.MessageBox.Show(string.Format(Locale.GetString("ERROR_DESERIALIZE_SERVERLIST"), Environment.NewLine), Locale.GetString("TITLE"), MessageBoxButton.OK, MessageBoxImage.Hand);
                            this.OnStateChange(EState.COMPLETE);
                            this.TaskbarInfo.ProgressState = TaskbarItemProgressState.Error;
                        }
                        else
                        {
                            Log.Write("Serverlist deserialization completed.", false);
                            DownloadManager.Instance.ClearEnded();
                            this.OnStateChange(EState.CHECK_PATCHER);
                        }
                        break;
                    }
                case EState.UPDATE_PATCHER:
                    {
                        Process process = new Process();
                        ProcessStartInfo processStartInfo = new ProcessStartInfo();
                        processStartInfo.WorkingDirectory = this.m_appPath;
                        processStartInfo.FileName = e.Downloader.LocalFile;
                        processStartInfo.Arguments = Assembly.GetExecutingAssembly().Location;
                        process.StartInfo = processStartInfo;
                        process.Start();
                        Log.Write(string.Format("Download completed. Launching new updater - path: {0}", Path.Combine(this.m_appPath, processStartInfo.FileName)), false);
                        base.Dispatcher.Invoke(new Action(delegate ()
                        {
                            System.Windows.Application.Current.Shutdown();
                        }), new object[0]);
                        break;
                    }
                case EState.FETCH_PATCHLIST:
                    {
                        string localFile2 = e.Downloader.LocalFile;
                        Log.Write(string.Format("Patchlist downloaded to {0}", localFile2), false);
                        this.m_patchList = Serialization.DeserializeFile<CPatchList>(localFile2);
                        FileSystem.DeleteFile(localFile2);
                        bool flag2 = this.m_patchList == null;
                        if (flag2)
                        {
                            Log.Write("Patchlist deserialization failed.", false);
                            System.Windows.MessageBox.Show(string.Format(Locale.GetString("ERROR_DESERIALIZE_PATCHLIST"), Environment.NewLine), Locale.GetString("TITLE"), MessageBoxButton.OK, MessageBoxImage.Hand);
                            this.OnStateChange(EState.COMPLETE);
                            this.TaskbarInfo.ProgressState = TaskbarItemProgressState.Error;
                        }
                        else
                        {
                            Log.Write("Patchlist deserialization completed.", false);
                            DownloadManager.Instance.ClearEnded();
                            this.OnStateChange(EState.CHECK_FILES);
                        }
                        break;
                    }
                case EState.DOWNLOAD_FILES:
                    {
                        bool flag3 = e.Downloader != null && !this.m_isClosing;
                        if (flag3)
                        {
                            string localFile3 = e.Downloader.LocalFile;
                            Log.Write(string.Format("  File {0} completed", localFile3.Replace(Settings.Default.TempFileExtension, "")), true);
                            FileSystem.Rename(Path.Combine(this.m_appPath, localFile3), Path.Combine(this.m_appPath, localFile3.Replace(Settings.Default.TempFileExtension, "")));
                        }
                        bool flag4 = e.Downloader == null || DownloadManager.Instance.TotalDownloads == DownloadManager.Instance.CompletedDownloads + DownloadManager.Instance.FailedDownloads;
                        if (flag4)
                        {
                            Log.FlushLineBuffer();
                            this.OnStateChange(EState.CLEAR_FILES);
                        }
                        break;
                    }
                case EState.CHECK_UPDATE:
                    {
                        string localFile4 = e.Downloader.LocalFile;
                        Log.Write(string.Format("Serverlist saved to {0}", localFile4), false);
                        this.m_serverList = Serialization.DeserializeFile<CServerList>(localFile4);
                        FileSystem.DeleteFile(localFile4);
                        bool flag5 = this.m_serverList == null;
                        if (flag5)
                        {
                            Log.Write("Error deserializing serverlist.", false);
                            System.Windows.MessageBox.Show(string.Format(Locale.GetString("ERROR_DESERIALIZE_SERVERLIST"), Environment.NewLine), Locale.GetString("TITLE"), MessageBoxButton.OK, MessageBoxImage.Hand);
                            this.OnStateChange(EState.COMPLETE);
                            this.TaskbarInfo.ProgressState = TaskbarItemProgressState.Error;
                        }
                        else
                        {
                            Log.Write("Serverlist deserialization completed.", false);
                            DownloadManager.Instance.ClearEnded();
                            this.OnStateChange(EState.CHECK_PATCHER);
                        }
                        break;
                    }
            }
        }

        // Token: 0x0600002D RID: 45 RVA: 0x000030C0 File Offset: 0x000012C0
        public void OnStateChange(EState state)
        {
            base.Dispatcher.Invoke(new Action(delegate ()
            {
                this.m_appState = state;
                this.txtDLProgress.Visibility = Visibility.Collapsed;
                this.txtDLSpeed.Visibility = Visibility.Collapsed;
                this.txtCurrentAction.Visibility = Visibility.Collapsed;
                this.btnStart.Visibility = Visibility.Collapsed;
                this.txtRemainingTime.Visibility = Visibility.Collapsed;
                this.m_currentDownloadRate.Clear();
                this.m_currentDownloadRate = new List<long>
                {
                    0L,
                    0L,
                    0L,
                    0L,
                    0L
                };
                switch (this.m_appState)
                {
                    case EState.SERVER_LIST:
                        DownloadManager.Instance.ClearAll();
                        this.txtCurrentAction.Visibility = Visibility.Visible;
                        this.txtCurrentAction.Text = Locale.GetString("ACTION_CONNECTING");
                        DownloadManager.Instance.Add(GameUpdater.Core.ResourceLocation.FromURL(Settings.Default.ServerHost + Settings.Default.ServerList), null, Path.GetTempFileName(), 1, true);
                        Log.Write("Retrieving serverlist...", false);
                        break;
                    case EState.CHECK_PATCHER:
                        {
                            Log.Write("Checking patcher version...", false);
                            Log.Write(string.Format("   Current: {0}", this.CurrentVersion), false);
                            Log.Write(string.Format("   Latest: {0}", this.m_serverList.UpdaterVersion), false);
#if ADDON
                            bool flag = this.m_serverList.UpdaterVersion != null && this.CurrentVersion != this.m_serverList.UpdaterVersion && MainWindow.RealVersion != this.m_serverList.UpdaterVersion;
#else
                            bool flag = this.m_serverList.UpdaterVersion != null && this.CurrentVersion != this.m_serverList.UpdaterVersion;
#endif
                            if (flag)
                            {
                                Log.Write("Patcher version mismatch...", false);
                                this.OnStateChange(EState.UPDATE_PATCHER);
                            }
                            else
                            {
                                this.OnStateChange(EState.FETCH_PATCHLIST);
                            }
                            break;
                        }
                    case EState.UPDATE_PATCHER:
                        {
                            bool flag2 = !this.m_isCheckingOnly;
                            if (flag2)
                            {
                                Log.Write(string.Format("Preparing download for Updater v{0} from URL {1}", this.m_serverList.UpdaterVersion, this.m_serverList.RemoteUpdaterURL), false);
                                this.txtCurrentAction.Visibility = Visibility.Visible;
                                this.txtCurrentAction.Text = string.Format(Locale.GetString("ACTION_DOWNLOADING_PATCHER_UPDATE"), this.m_serverList.UpdaterVersion);
                                DownloadManager.Instance.Add(GameUpdater.Core.ResourceLocation.FromURL(this.m_serverList.RemoteUpdaterURL), null, string.Format(Settings.Default.PatcherUpdaterName, this.m_serverList.UpdaterVersion.ToString()), 1, true);
                            }
                            else
                            {
                                Log.Write(string.Format("Pending download for new Updater v{0}", this.m_serverList.UpdaterVersion), false);
                                this.txtCurrentAction.Visibility = Visibility.Visible;
                                this.txtCurrentAction.Text = string.Format(Locale.GetString("ACTION_PENDING_PATCHER_UPDATE"), this.m_serverList.UpdaterVersion);
                                this.SetTrayIconState(ETrayState.NEW_PATCHER_AVAILABLE);
                                bool flag3 = System.Windows.MessageBox.Show(Locale.GetString("PROMPT_NEW_PATCHER_VERSION_AVAILABLE"), Locale.GetString("TITLE"), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
                                if (flag3)
                                {
                                    this.m_isCheckingOnly = false;
                                    this.OnStateChange(EState.UPDATE_PATCHER);
                                }
                            }
                            break;
                        }
                    case EState.FETCH_PATCHLIST:
                        Log.Write(string.Format("Preparing patchlist download...", new object[0]), false);
                        this.txtCurrentAction.Visibility = Visibility.Visible;
                        this.txtCurrentAction.Text = Locale.GetString("ACTION_FETCHING_PATCHLIST");
                        DownloadManager.Instance.Add(GameUpdater.Core.ResourceLocation.FromURL(Settings.Default.ServerHost + this.m_serverList.PatchListFile), null, Path.GetTempFileName(), 1, true);
                        break;
                    case EState.CHECK_FILES:
                        this.txtCurrentAction.Visibility = Visibility.Visible;
                        this.txtCurrentAction.Text = Locale.GetString("ACTION_CHECKING_CLIENT_FILES");
                        this.TaskbarInfo.ProgressState = TaskbarItemProgressState.Normal;
                        Log.Write("Initializing client integrity check...", false);
                        Log.Write(string.Format("   Threads: {0}", Environment.ProcessorCount), false);
                        Log.Write(string.Format("   Files: {0}", this.m_patchList.Files.Count), false);
                        this.CheckClientFiles();
                        break;
                    case EState.PREPARE_DOWNLOAD:
                        {
                            bool flag4 = !this.m_isCheckingOnly;
                            if (flag4)
                            {
                                this.txtCurrentAction.Visibility = Visibility.Visible;
                                this.txtCurrentAction.Text = Locale.GetString("ACTION_PREPARING_DOWNLOAD");
                                this.TaskbarInfo.ProgressState = TaskbarItemProgressState.Normal;
                                Log.Write(string.Format("Preparing download of {0} files. Total bytes to download: {1}", DownloadManager.Instance.Downloads.Count, this.m_totalDownloadSize), false);
                                this.OnStateChange(EState.DOWNLOAD_FILES);
                            }
                            else
                            {
                                bool flag5 = this.m_totalDownloadSize == 0L;
                                if (flag5)
                                {
                                    this.OnStateChange(EState.COMPLETE);
                                }
                                else
                                {
                                    this.txtCurrentAction.Visibility = Visibility.Visible;
                                    this.txtCurrentAction.Text = Locale.GetString("ACTION_DOWNLOAD_PENDING");
                                    this.SetTrayIconState(ETrayState.NEW_FILES_AVAILABLE);
                                    bool flag6 = System.Windows.MessageBox.Show(Locale.GetString("PROMPT_NEW_UPDATES_AVAILABLE"), Locale.GetString("TITLE"), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
                                    if (flag6)
                                    {
                                        this.m_isCheckingOnly = false;
                                        this.OnStateChange(EState.PREPARE_DOWNLOAD);
                                    }
                                }
                            }
                            break;
                        }
                    case EState.DOWNLOAD_FILES:
                        this.txtRemainingTime.Visibility = Visibility.Visible;
                        this.txtCurrentAction.Visibility = Visibility.Visible;
                        this.txtCurrentAction.Text = Locale.GetString("ACTION_DOWNLOADING");
                        this.TaskbarInfo.ProgressState = TaskbarItemProgressState.Normal;
                        Log.Write("Download started...", false);
                        DownloadManager.Instance.StartAll();
                        break;
                    case EState.CLEAR_FILES:
                        this.txtCurrentAction.Visibility = Visibility.Visible;
                        this.txtCurrentAction.Text = Locale.GetString("ACTION_CLEANING_CLIENT_FILES");
                        this.TaskbarInfo.ProgressState = TaskbarItemProgressState.Normal;
                        this.TaskbarInfo.ProgressValue = 0.0;
                        Log.Write("Deleting unneeded client files...", false);
                        this.CleanClientFiles();
                        break;
                    case EState.CHECK_UPDATE:
                        this.m_isCheckingOnly = true;
                        this.txtCurrentAction.Visibility = Visibility.Visible;
                        this.txtCurrentAction.Text = Locale.GetString("ACTION_CHECKING_UPDATES");
                        DownloadManager.Instance.Add(GameUpdater.Core.ResourceLocation.FromURL(Settings.Default.ServerHost + Settings.Default.ServerList), null, Path.GetTempFileName(), 1, true);
                        break;
                    case EState.COMPLETE:
                        {
                            this.TaskbarInfo.ProgressValue = 1.0;
                            this.txtCurrentAction.Visibility = Visibility.Visible;
                            this.txtCurrentAction.Text = Locale.GetString("ACTION_CLIENT_READY");
                            this.btnStart.Visibility = Visibility.Visible;
                            bool flag7 = !this.m_isCheckingOnly;
                            if (flag7)
                            {
                                this.SetTrayIconState(ETrayState.UPDATE_COMPLETED);
                            }
                            this.m_isCheckingOnly = false;
                            bool flag8 = DownloadManager.Instance.FailedDownloads > 0;
                            if (flag8)
                            {
                                Log.Write(string.Format("Update completed - {0} files couldn't be downloaded.", DownloadManager.Instance.FailedDownloads), false);
                                this.TaskbarInfo.ProgressState = TaskbarItemProgressState.Error;
                                bool flag9 = System.Windows.MessageBox.Show(Locale.GetString("PROMPT_REDOWNLOAD_MISSING_UPDATES"), Locale.GetString("TITLE"), MessageBoxButton.YesNo, MessageBoxImage.Hand) == MessageBoxResult.Yes;
                                if (flag9)
                                {
                                    this.OnStateChange(EState.SERVER_LIST);
                                }
                            }
                            else
                            {
                                this.TaskbarInfo.ProgressState = TaskbarItemProgressState.Normal;
                                Log.Write(string.Format("Update completed without errors", new object[0]), false);
                            }
                            break;
                        }
                }
            }), new object[0]);
        }

        // Token: 0x0600002E RID: 46 RVA: 0x00003100 File Offset: 0x00001300
        public void IncrementProgressBar()
        {
            base.Dispatcher.Invoke(new Action(delegate ()
            {
                this.m_CompletedFileCheckCount += 1.0;
                this.UpdateTaskbarProgressState(this.m_CompletedFileCheckCount, this.m_totalFileCheckCount);
            }), new object[0]);
        }

        // Token: 0x0600002F RID: 47 RVA: 0x00003124 File Offset: 0x00001324
        public void ThreadedFileCheck(object data)
        {
            CPatchFile cpatchFile = data as CPatchFile;
            FileInfo fileInfo = new FileInfo(Path.Combine(this.m_appPath, cpatchFile.FilePath));
            bool exists = fileInfo.Exists;
            if (exists)
            {
                bool flag = (long)cpatchFile.Size != fileInfo.Length;
                if (flag)
                {
                    DownloadManager.Instance.Add(cpatchFile.FilePath, this.m_serverList.MirrorList, this.m_appPath + cpatchFile.FilePath + Settings.Default.TempFileExtension, this.m_serverList.MirrorList.Count, false);
                    this.IncrementProgressBar();
                    this.m_totalDownloadSize += (long)cpatchFile.Size;
                    Log.Write(string.Format("  Check {0}, Size {1}, Expected Size {2} - Mismatch", cpatchFile.FilePath, fileInfo.Length, cpatchFile.Size), true);
                }
                else
                {
                    bool flag2 = true;
                    using (FileStream fileStream = fileInfo.OpenRead())
                    {
                        foreach (CFileChunk cfileChunk in cpatchFile.Checksum)
                        {
                            byte[] buffer = new byte[cfileChunk.Size];
                            fileStream.Seek((long)cfileChunk.StartIndex, SeekOrigin.Begin);
                            int readSize = fileStream.Read(buffer, 0, cfileChunk.Size);
                            string md5Hash = Crypto.GetMD5Hash(ref buffer, readSize);
                            bool flag3 = md5Hash != cfileChunk.Hash;
                            if (flag3)
                            {
                                DownloadManager.Instance.Add(cpatchFile.FilePath, this.m_serverList.MirrorList, this.m_appPath + cpatchFile.FilePath + Settings.Default.TempFileExtension, this.m_serverList.MirrorList.Count, false);
                                flag2 = false;
                                Log.Write(string.Format("  Check {0}, Size {1}, Chunk {2} Length {3} - Checksum mismatch (Expected {4}, Found {5})", new object[]
                                {
                                    cpatchFile.FilePath,
                                    fileInfo.Length,
                                    cfileChunk.StartIndex,
                                    cfileChunk.Size,
                                    cfileChunk.Hash,
                                    md5Hash
                                }), true);
                                this.m_totalDownloadSize += (long)cpatchFile.Size;
                                break;
                            }
                        }
                    }
                    bool flag4 = flag2;
                    if (flag4)
                    {
                        Log.Write(string.Format("  Check {0}, Size {1} - Checksum OK", cpatchFile.FilePath, fileInfo.Length), true);
                    }
                    this.IncrementProgressBar();
                }
            }
            else
            {
                DownloadManager.Instance.Add(cpatchFile.FilePath, this.m_serverList.MirrorList, this.m_appPath + cpatchFile.FilePath + Settings.Default.TempFileExtension, this.m_serverList.MirrorList.Count, false);
                Log.Write(string.Format("  Check {0}, Size {1} - File not found", cpatchFile.FilePath, cpatchFile.Size), true);
                this.IncrementProgressBar();
                this.m_totalDownloadSize += (long)cpatchFile.Size;
            }
            bool isCheckingOnly = this.m_isCheckingOnly;
            if (isCheckingOnly)
            {
                Thread.Sleep(250);
            }
            bool flag5 = this.m_totalFileCheckCount == this.m_CompletedFileCheckCount;
            if (flag5)
            {
                Log.Write("Client integrity check completed.", true);
                Log.FlushLineBuffer();
                this.OnStateChange(EState.PREPARE_DOWNLOAD);
            }
        }

        // Token: 0x06000030 RID: 48 RVA: 0x000034B8 File Offset: 0x000016B8
        public void CheckClientFiles()
        {
            this.m_totalDownloadSize = 0L;
            this.m_totalFileCheckCount = (double)this.m_patchList.Files.Count;
            foreach (CPatchFile state in this.m_patchList.Files)
            {
                ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.ThreadedFileCheck), state);
            }
        }

        // Token: 0x06000031 RID: 49 RVA: 0x00003540 File Offset: 0x00001740
        public void CleanClientFiles()
        {
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>
            {
                {
                    "FILES",
                    new List<string>
                    {
                        "updater.cfg",
                        "syserr.txt",
                        "updater.log",
                        "metin2.cfg",
                        "locale.cfg",
                        "mouse.cfg",
                        new FileInfo(Assembly.GetExecutingAssembly().Location).Name.ToLower(),
                        string.Format("{0} v{1}.exe", Locale.GetString("TITLE"), this.CurrentVersion).ToLower(),
#if ADDON
                        string.Format("{0} v{1}.exe", Locale.GetString("TITLE"), MainWindow.RealVersion).ToLower()
#endif
                    }
                },
                {
                    "DIRS",
                    new List<string>
                    {
                        "mark/",
                        "settings/",
                        "upload/",
                        "screenshot/"
                    }
                }
            };
            foreach (FileInfo fileInfo in new DirectoryInfo(this.m_appPath).GetFiles("*.*", SearchOption.AllDirectories))
            {
                string text = fileInfo.FullName.Replace(this.m_appPath, "").Replace("\\", "/").ToLower();
                bool flag = false;
                bool flag2 = !(fileInfo.Extension == ".backtrace") && !dictionary["FILES"].Contains(text) && (!text.Contains("/") || !dictionary["DIRS"].Contains(text.Substring(0, text.IndexOf("/") + 1)));
                if (flag2)
                {
                    foreach (CPatchFile cpatchFile in this.m_patchList.Files)
                    {
                        bool flag3 = cpatchFile.FilePath.ToLower() == text;
                        if (flag3)
                        {
                            flag = true;
                            break;
                        }
                    }
                    bool flag4 = !flag;
                    if (flag4)
                    {
                        bool flag5 = FileSystem.DeleteFile(fileInfo.FullName);
                        if (flag5)
                        {
                            Log.Write(string.Format("  Deleting {0} - OK", fileInfo.FullName), false);
                        }
                        else
                        {
                            Log.Write(string.Format("  Deleting {0} - Failed", fileInfo.FullName), false);
                        }
                    }
                }
            }
            this.OnStateChange(EState.COMPLETE);
        }

        // Token: 0x06000032 RID: 50 RVA: 0x000037C8 File Offset: 0x000019C8
        public void LaunchGame()
        {
            bool flag = !File.Exists(Path.Combine(this.m_appPath, "metin2.cfg"));
            if (flag)
            {
                this.LaunchConfig();
            }
            else
            {
                bool flag2 = !File.Exists(Path.Combine(this.m_appPath, "locale.cfg"));
                if (flag2)
                {
                    try
                    {
                        File.WriteAllText(Path.Combine(this.m_appPath, "locale.cfg"), string.Format("10000 65001 {0}", Locale.GetLocale()));
                    }
                    catch (Exception ex)
                    {
                        Log.Write(string.Format("Unable to create locale.cfg - Error: {0}", ex.Message), false);
                    }
                }
                try
                {
                    new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            WorkingDirectory = this.m_appPath,
                            FileName = this.m_patchList.LauncherFileName,
                            Arguments = "--exec-manager"
                        }
                    }.Start();
                }
                catch (Exception ex2)
                {
                    Log.Write(string.Format("Unable to launch game instance - Error: {0}", ex2.Message), false);
                    this.TaskbarInfo.ProgressState = TaskbarItemProgressState.Error;
                    System.Windows.MessageBox.Show(Locale.GetString("ERROR_CANNOT_LAUNCH_GAME"), Locale.GetString("TITLE"), MessageBoxButton.OK, MessageBoxImage.Hand);
                    return;
                }
                this.TaskbarInfo.ProgressState = TaskbarItemProgressState.None;
                System.Windows.Application.Current.Shutdown();
            }
        }

        // Token: 0x06000033 RID: 51 RVA: 0x00003924 File Offset: 0x00001B24
        public void LaunchConfig()
        {
            try
            {
                new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        WorkingDirectory = this.m_appPath,
                        FileName = "config.exe"
                    }
                }.Start();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show(Locale.GetString("ERROR_CANNOT_LAUNCH_CONFIG"), Locale.GetString("TITLE"), MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        // Token: 0x06000034 RID: 52 RVA: 0x00003998 File Offset: 0x00001B98
        private void InitTrayIcon()
        {
            this.m_notifyIcon = new NotifyIcon();
            this.m_notifyIcon.BalloonTipClicked += this.m_notifyIcon_Click;
            this.m_notifyIcon.BalloonTipTitle = Locale.GetString("TITLE");
            this.m_notifyIcon.Text = Locale.GetString("TITLE");
            this.m_notifyIcon.Click += this.m_notifyIcon_Click;
            using (Stream stream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/;component/icon.ico")).Stream)
            {
                this.m_notifyIcon.Icon = new Icon(stream);
            }
            this.SetTrayIconState(ETrayState.DEFAULT);
            base.Closing += this.OnClearTrayIcon;
            base.StateChanged += this.OnStateChanged;
            base.IsVisibleChanged += this.OnIsVisibleChanged;
        }

        // Token: 0x06000035 RID: 53 RVA: 0x00003A94 File Offset: 0x00001C94
        private void SetTrayIconState(ETrayState state)
        {
            this.m_trayState = state;
            switch (this.m_trayState)
            {
                case ETrayState.DEFAULT:
                    this.m_notifyIcon.BalloonTipText = Locale.GetString("TRAY_INFO_DEFAULT");
                    break;
                case ETrayState.UPDATE_COMPLETED:
                    {
                        bool flag = base.WindowState != WindowState.Minimized;
                        if (!flag)
                        {
                            this.m_notifyIcon.BalloonTipText = Locale.GetString("TRAY_INFO_UPDATE_COMPLETED");
                            this.m_notifyIcon.ShowBalloonTip(2000);
                        }
                        break;
                    }
                case ETrayState.NEW_PATCHER_AVAILABLE:
                    {
                        bool flag2 = base.WindowState != WindowState.Minimized;
                        if (!flag2)
                        {
                            this.m_notifyIcon.BalloonTipText = Locale.GetString("TRAY_INFO_NEW_PATCHER_AVAILABLE");
                            this.m_notifyIcon.ShowBalloonTip(2000);
                        }
                        break;
                    }
                case ETrayState.NEW_FILES_AVAILABLE:
                    {
                        bool flag3 = base.WindowState != WindowState.Minimized;
                        if (!flag3)
                        {
                            this.m_notifyIcon.BalloonTipText = Locale.GetString("TRAY_INFO_NEW_UPDATES_AVAILABLE");
                            this.m_notifyIcon.ShowBalloonTip(2000);
                        }
                        break;
                    }
            }
        }

        // Token: 0x06000036 RID: 54 RVA: 0x00003BA3 File Offset: 0x00001DA3
        private void OnClearTrayIcon(object sender, CancelEventArgs args)
        {
            this.m_notifyIcon.Dispose();
            this.m_notifyIcon = null;
        }

        // Token: 0x06000037 RID: 55 RVA: 0x00003BBC File Offset: 0x00001DBC
        private void OnStateChanged(object sender, EventArgs args)
        {
            bool flag = base.WindowState == WindowState.Minimized;
            if (flag)
            {
                base.Hide();
                bool flag2 = this.m_notifyIcon != null;
                if (flag2)
                {
                    this.m_notifyIcon.ShowBalloonTip(2000);
                }
            }
            else
            {
                this.m_storedWindowState = base.WindowState;
                this.SetTrayIconState(ETrayState.DEFAULT);
            }
        }

        // Token: 0x06000038 RID: 56 RVA: 0x00003C19 File Offset: 0x00001E19
        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            this.CheckTrayIcon();
        }

        // Token: 0x06000039 RID: 57 RVA: 0x00003C23 File Offset: 0x00001E23
        private void m_notifyIcon_Click(object sender, EventArgs e)
        {
            base.Show();
            base.WindowState = this.m_storedWindowState;
        }

        // Token: 0x0600003A RID: 58 RVA: 0x00003C3A File Offset: 0x00001E3A
        private void CheckTrayIcon()
        {
            this.ShowTrayIcon(!base.IsVisible);
        }

        // Token: 0x0600003B RID: 59 RVA: 0x00003C50 File Offset: 0x00001E50
        private void ShowTrayIcon(bool show)
        {
            bool flag = this.m_notifyIcon != null;
            if (flag)
            {
                this.m_notifyIcon.Visible = show;
            }
        }

        // Token: 0x0600003C RID: 60 RVA: 0x00003C7C File Offset: 0x00001E7C
        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            bool contentLoaded = this._contentLoaded;
            if (!contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/GameUpdater;component/mainwindow.xaml", UriKind.Relative);
                System.Windows.Application.LoadComponent(this, resourceLocator);
            }
        }

        // Token: 0x0600003D RID: 61 RVA: 0x00003CB4 File Offset: 0x00001EB4
        [DebuggerNonUserCode]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.imgLoadingOverlay = (System.Windows.Controls.Image)target;
                    break;
                case 2:
                    this.txtDLProgress = (TextBlock)target;
                    break;
                case 3:
                    this.txtDLSpeed = (TextBlock)target;
                    break;
                case 4:
                    this.txtCurrentAction = (TextBlock)target;
                    break;
                case 5:
                    this.txtRemainingTime = (TextBlock)target;
                    break;
                case 6:
                    this.btnStart = (Border)target;
                    this.btnStart.MouseEnter += this.btnStart_MouseEnter;
                    this.btnStart.MouseLeave += this.btnStart_MouseLeave;
                    this.btnStart.MouseLeftButtonUp += this.btnStart_MouseLeftButtonUp;
                    this.btnStart.MouseLeftButtonDown += this.btnStart_MouseLeftButtonDown;
                    break;
                case 7:
                    this.btnMinimize = (System.Windows.Controls.Image)target;
                    this.btnMinimize.MouseEnter += this.btnMinimize_MouseEnter;
                    this.btnMinimize.MouseLeave += this.btnMinimize_MouseLeave;
                    this.btnMinimize.MouseLeftButtonDown += this.btnMinimize_MouseLeftButtonDown;
                    this.btnMinimize.MouseLeftButtonUp += this.btnMinimize_MouseLeftButtonUp;
                    break;
                case 8:
                    this.btnClose = (System.Windows.Controls.Image)target;
                    this.btnClose.MouseEnter += this.btnClose_MouseEnter;
                    this.btnClose.MouseLeave += this.btnClose_MouseLeave;
                    this.btnClose.MouseLeftButtonDown += this.btnClose_MouseLeftButtonDown;
                    this.btnClose.MouseLeftButtonUp += this.btnClose_MouseLeftButtonUp;
                    break;
                case 9:
                    this.txtHomeLink = (System.Windows.Controls.Label)target;
                    this.txtHomeLink.PreviewMouseLeftButtonDown += this.TxtHomeLink_MouseLeftButtonDown;
                    break;
                case 10:
                    this.txtRegisterLink = (System.Windows.Controls.Label)target;
                    this.txtRegisterLink.PreviewMouseLeftButtonDown += this.TxtRegisterLink_MouseLeftButtonDown;
                    break;
                case 11:
                    this.txtSupportLink = (System.Windows.Controls.Label)target;
                    this.txtSupportLink.PreviewMouseLeftButtonDown += this.TxtSupportLink_MouseLeftButtonDown;
                    break;
                case 12:
                    this.btnConfig = (System.Windows.Controls.Image)target;
                    this.btnConfig.MouseEnter += this.BtnConfig_MouseEnter;
                    this.btnConfig.MouseLeave += this.BtnConfig_MouseLeave;
                    this.btnConfig.MouseLeftButtonDown += this.BtnConfig_MouseLeftButtonDown;
                    break;
                default:
                    this._contentLoaded = true;
                    break;
            }
        }

        // Token: 0x04000016 RID: 22
        private System.Threading.Timer m_rateTimer;

        // Token: 0x04000017 RID: 23
        private System.Threading.Timer m_progressTimer;

        // Token: 0x04000018 RID: 24
        private System.Threading.Timer m_updateCheckTimer;

        // Token: 0x04000019 RID: 25
        private EState m_appState;

        // Token: 0x0400001A RID: 26
        private string m_appPath = "";

        // Token: 0x0400001B RID: 27
        private CServerList m_serverList;

        // Token: 0x0400001C RID: 28
        private CPatchList m_patchList;

        // Token: 0x0400001D RID: 29
        private bool m_isClosing;

        // Token: 0x0400001E RID: 30
        private bool m_isCheckingOnly;

        // Token: 0x0400001F RID: 31
        private List<long> m_currentDownloadRate = new List<long>
        {
            0L,
            0L,
            0L,
            0L,
            0L
        };

        // Token: 0x04000020 RID: 32
        private long m_totalDownloadSize;

        // Token: 0x04000021 RID: 33
        private double m_totalFileCheckCount;

        // Token: 0x04000022 RID: 34
        private double m_CompletedFileCheckCount;

        // Token: 0x04000023 RID: 35
        private NotifyIcon m_notifyIcon;

        // Token: 0x04000024 RID: 36
        private WindowState m_storedWindowState;

        // Token: 0x04000025 RID: 37
        private ETrayState m_trayState;

        // Token: 0x04000026 RID: 38
        internal System.Windows.Controls.Image imgLoadingOverlay;

        // Token: 0x04000027 RID: 39
        internal TextBlock txtDLProgress;

        // Token: 0x04000028 RID: 40
        internal TextBlock txtDLSpeed;

        // Token: 0x04000029 RID: 41
        internal TextBlock txtCurrentAction;

        // Token: 0x0400002A RID: 42
        internal TextBlock txtRemainingTime;

        // Token: 0x0400002B RID: 43
        internal Border btnStart;

        // Token: 0x0400002C RID: 44
        internal System.Windows.Controls.Image btnMinimize;

        // Token: 0x0400002D RID: 45
        internal System.Windows.Controls.Image btnClose;

        // Token: 0x0400002E RID: 46
        internal System.Windows.Controls.Label txtHomeLink;

        // Token: 0x0400002F RID: 47
        internal System.Windows.Controls.Label txtRegisterLink;

        // Token: 0x04000030 RID: 48
        internal System.Windows.Controls.Label txtSupportLink;

        // Token: 0x04000031 RID: 49
        internal System.Windows.Controls.Image btnConfig;

        // Token: 0x04000032 RID: 50
        private bool _contentLoaded;

        // Token: 0x02000039 RID: 57
        // (Invoke) Token: 0x0600021F RID: 543
        public delegate void WaitCallback(object state);
    }
}
