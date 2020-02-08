using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orion.API;

namespace CoreTpl.WebApp
{
    public class Program
    {

        private static IOrionLogger _log = new OrionNLogLogger("Main");

        public static void Main(string[] args)
        {
            /* UnhandledException 只能攔截錯誤，不能阻止程式關閉 */
            AppDomain.CurrentDomain.UnhandledException += unhandledExceptionHandler;

            /* 用來攔截 Task 錯誤 */
            TaskScheduler.UnobservedTaskException += unobservedTaskException;

            /* 設定 ThreadPool 的上限 */
            ThreadPool.SetMaxThreads(400, 400);

            /* 重新指定當前目錄 */
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            var host = Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webHostBuilder => webHostBuilder
                    //.UseContentRoot(Directory.GetCurrentDirectory()) /* 指定後會造成 JS, CSS 路徑錯誤 */
                    //.UseIISIntegration()
                    .UseStartup<Startup>()
                )
                .Build();

            try
            {
                host.Run(); /* 啟動網站 */
            }
            catch (Exception ex)
            {
                _log.Error("host.Run", ex);
                throw;
            }
        }



        private static void unhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;

            _log.Fatal("執行錯誤", ex);
        }

        private static void unobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            _log.Fatal("執行錯誤", e.Exception);
            e.SetObserved();
        }

        //TODO
        //private static void cultureSetting()
        //{
        //    var culture = new CultureInfo("zh-TW");

        //    culture.DateTimeFormat.DateSeparator = "/"; /* Format 會將 / 轉換成此設定，避免問題不替換分隔符號 */
        //    culture.DateTimeFormat.MonthDayPattern = "MM-dd";
        //    culture.DateTimeFormat.LongDatePattern = "yyyy-MM-dd";
        //    culture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
        //    culture.DateTimeFormat.LongTimePattern = "HH:mm:ss";
        //    culture.DateTimeFormat.ShortTimePattern = "HH:mm:ss";

        //    Thread.CurrentThread.CurrentCulture = culture;
        //    Thread.CurrentThread.CurrentUICulture = culture;
        //}






    }
}
