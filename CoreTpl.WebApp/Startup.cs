using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using CoreTpl.Dao.Database;
using CoreTpl.Domain;
using CoreTpl.Enums;
using CoreTpl.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Orion.API;
using Orion.API.Extensions;
using Orion.Mvc.Filters;
using Orion.Mvc.ModelBinder;
using Orion.Mvc.UI;

namespace CoreTpl.WebApp
{
	public class Startup
	{
		private readonly IConfigurationRoot _config;

		public Startup(IWebHostEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();

			_config = builder.Build();
		}




		/// <summary>ConfigureServices is where you register dependencies. This gets called by the runtime before the ConfigureContainer method, below.</summary>
		public void ConfigureServices(IServiceCollection services)
		{
			//services.AddControllers();
			//services.AddControllersWithViews();
			//services.AddRazorPages();

			services.AddOptions();

			services.AddHttpContextAccessor();

			services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
			services.AddDbContextPool<TplDbContext>(options =>
			{
				options.UseNpgsql(_config.GetConnectionString("TplDbContext"));
			});


			//TODO 等支援 3.1 版本
			//services.AddElmah<XmlFileErrorLog>(options =>
			//{
			//	options.LogPath = _config.GetValue<string>("elmah:LogPath");
			//	options.FiltersConfig = getFilePath("elmah.xml");
			//});


			services.Configure<CookiePolicyOptions>(options =>
			{
				options.CheckConsentNeeded = context => false; /* 不啟用 GDPR 這會造成 Cookie 無法寫入 */
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});
					   

			services.AddMemoryCache(); /* 增加記憶體快取 */
			//services.AddSession(); /* 增加 Session */

			//services.AddHealthChecks(); /* 增加健康情況檢查 */

			services.AddAuthorization(); /* 配置權限驗證處理 */


			/* 配置登入者處理 */
			services
				.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.LoginPath = "/Account/Login";
					options.SlidingExpiration = true;
					options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
					options.Cookie.HttpOnly = true; /* Cookie settings */

					options.Events.OnRedirectToAccessDenied = (redirectContext) => 
					{
						redirectContext.HttpContext.Response.StatusCode = 403;
						return Task.FromResult(0);
					};
				});

			/* 用 MemoryCache 來紀錄登入者資料 */
			services.AddSingleton<IPostConfigureOptions<CookieAuthenticationOptions>, ConfigureSessionAuthentication>();


			/* 配置 MVC */
			services
				.AddMvc(options =>
				{
					options.ModelBinderProviders.Insert(0, new StringTrimModelBinderProvider());
					options.ModelBinderProviders.Insert(0, new WhereParamsModelBinderProvider());
#if DEBUG
					options.Filters.Insert(0, new DevelopAuthorizationFilter<ACT>());
#endif
					options.Filters.Add(new UseViewPageActionFilter());
					options.Filters.Add(new ExceptionMessageActionFilter());
					options.Filters.Add(new PageParamsActionFilter("PageSize", 50)); /* 換頁參數 */
				})
#if DEBUG
				.AddRazorRuntimeCompilation() /* 啟用 Razor Runtime 編譯 */
#endif
				.AddControllersAsServices();

		}




		/// <summary>
		/// 配置 Autofac Builder 
		/// ConfigureContainer is where you can register things directly with Autofac. This runs after ConfigureServices so the things here will override registrations made in ConfigureServices. Don't build the container; that gets done for you by the factory.
		/// </summary>
		public void ConfigureContainer(ContainerBuilder builder)
		{
			builder.RegisterModule<OrionNLogModule>();

			/* TplConfig 配置 */
			builder.Register(r => _config.GetSection("tplConfig").Get<TplConfig>());

			/* Sidebar Menu 與麵包屑 */
			var menuReg = builder.Register(r => new MenuProvider("Menu.config")).As<IMenuProvider>();
			var breadReg = builder.Register(r => new BreadcrumbProvider("Breadcrumb.config")).As<IBreadcrumbProvider>();
#if !DEBUG
			/* 正式時切換成單例 */
			menuReg.SingleInstance();
			breadReg.SingleInstance();
#endif

			/* ServiceContext 配置 */
			builder.RegisterServiceContext<IServiceContext>().InstancePerLifetimeScope();

			/* OptionItemsProvider 配置 */
			builder.RegisterType<OptionItemsProvider>();
			builder.Register<IOptionItemsProvider>(r => r.Resolve<OptionItemsProvider>()).GetterCacheWrap().InstancePerLifetimeScope();

			/* 密碼處理 */
			builder.RegisterType<PasswordSHA256Handle>().As<IPasswordHandle>();


			/* DAO 配置 */
			builder.RegisterAssemblyTypes(Assembly.Load("CoreTpl.Dao"))
				   .Where(t => t.Name.EndsWith("Dao"))
				   .AsImplementedInterfaces();


			/* Service 配置 */
			builder.RegisterAssemblyTypes(Assembly.Load("CoreTpl.Service.Impl"))
				   .Where(t => t.Name.EndsWith("Service"))
				   .AsImplementedInterfaces();


			/* 註冊結尾名稱為 Controller 的 class */
			builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
				   .Where(t => t.IsAssignableTo<Controller>())
				   .PropertiesAutowired();

		}



		/// <summary>Configure is where you add middleware. This is called after ConfigureContainer. You can use IApplicationBuilder.ApplicationServices here if you need to resolve things from the container.</summary>
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				//app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				//app.UseHsts(); /* HTTP 強制安全傳輸 */
			}

			//app.UseElmah(); //TODO 等支援 3.1 版本
			//app.UseSession(); /* 啟用 Session */
			app.UseCookiePolicy();

			app.UseStatusCodePagesWithReExecute("/Home/Error");

			//app.UseHttpsRedirection();


			/* 多國語言設定 */
			var supportedCultures = getSupportedCultures().ToArray();

			app.UseRequestLocalization(options =>
			{
				options.DefaultRequestCulture = new RequestCulture(supportedCultures[0].Name);
				options.SupportedCultures = supportedCultures; /* Formatting numbers, dates, etc. */
				options.SupportedUICultures = supportedCultures; /* UI strings that we have localized. */
			});


			app.UseRouting(); /* 啟用路由 */
			app.UseStaticFiles(); /* 靜態檔案 (JS, CSS) */
			app.UseAuthentication(); /* 啟用頁面的權限檢查 */
			app.UseAuthorization(); /* 啟用登入者資料處理 */


			/* MVC 路由配置 */
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}"
				).RequireAuthorization();
				
				/* 配置健康情況檢查的 Routing */
				//endpoints.MapHealthChecks("/healthz", new HealthCheckOptions() { });

			});
		}



		/// <summary>多國語言清單</summary>
		private IEnumerable<CultureInfo> getSupportedCultures() 
		{
			var zhTW = new CultureInfo("zh-TW");

			zhTW.DateTimeFormat.DateSeparator = "/"; /* Format 會將 / 轉換成此設定，避免問題不替換分隔符號 */
			zhTW.DateTimeFormat.MonthDayPattern = "MM-dd";
			zhTW.DateTimeFormat.LongDatePattern = "yyyy-MM-dd";
			zhTW.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
			zhTW.DateTimeFormat.LongTimePattern = "HH:mm:ss";
			zhTW.DateTimeFormat.ShortTimePattern = "HH:mm:ss";

			yield return zhTW;

		}





	}





}
