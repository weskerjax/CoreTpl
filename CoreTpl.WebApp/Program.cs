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
            /* UnhandledException �u���d�I���~�A�������{������ */
            AppDomain.CurrentDomain.UnhandledException += unhandledExceptionHandler;

            /* �Ψ��d�I Task ���~ */
            TaskScheduler.UnobservedTaskException += unobservedTaskException;

            /* �]�w ThreadPool ���W�� */
            ThreadPool.SetMaxThreads(400, 400);

            /* ���s���w��e�ؿ� */
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            var host = Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webHostBuilder => webHostBuilder
                    //.UseContentRoot(Directory.GetCurrentDirectory()) /* ���w��|�y�� JS, CSS ���|���~ */
                    //.UseIISIntegration()
                    .UseStartup<Startup>()
                )
                .Build();

            try
            {
                host.Run(); /* �Ұʺ��� */
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

            _log.Fatal("������~", ex);
        }

        private static void unobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            _log.Fatal("������~", e.Exception);
            e.SetObserved();
        }

        //TODO
        //private static void cultureSetting()
        //{
        //    var culture = new CultureInfo("zh-TW");

        //    culture.DateTimeFormat.DateSeparator = "/"; /* Format �|�N / �ഫ�����]�w�A�קK���D���������j�Ÿ� */
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
