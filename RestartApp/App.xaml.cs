using Saylor.CommonTool.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RestartApp
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        Process currentProcess;//当前进程
        DispatcherTimer timer = new DispatcherTimer();

        public App()
        {
            currentProcess = Process.GetCurrentProcess();
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        /// <summary>
        /// 定时操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Restart();
            Log4NetHelper.WriteInfoLog($"重启新应用");
            Application.Current.Shutdown();
            Log4NetHelper.WriteInfoLog($"关闭当前应用");
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            KillExeExceptSelf(currentProcess.ProcessName,currentProcess.Id);
            Log4NetHelper.WriteInfoLog($"启动新进程：{currentProcess.Id}");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            timer.Stop();
            timer = null;
            Log4NetHelper.WriteInfoLog($"OnExit:{currentProcess.Id}");

        }


        /// <summary>
        /// 关闭除了自身以外的所有同名进程
        /// </summary>
        /// <param name="exePath"></param>
        /// <param name="exceptId"></param>
        public  void KillExeExceptSelf(string exePath ,int exceptId )
        {
            try
            {
                foreach (var item in Process.GetProcessesByName(exePath).Where(p=>p.Id != exceptId))
                {
                    //item.Kill();
                    item.CloseMainWindow();
                    Log4NetHelper.WriteInfoLog($"KillProcessId:{item.Id}");
                }
            }
            catch (Exception ex)
            {
                
            }
        }

    }
}
