using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CoreTpl.Dao.Database;
using CoreTpl.Domain;
using CoreTpl.Service;
//using ElmahCore;
//using ElmahCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orion.API;
using Orion.API.Extensions;
using Orion.Mvc.Filters;
using Orion.Mvc.ModelBinder;
using Orion.Mvc.UI;
using Orion.Mvc.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Orion.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using CoreTpl.Enums;

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
			services.AddOptions();
			// services.AddControllersWithViews();

			services.AddHttpContextAccessor();

			services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
			services.AddDbContextPool<TplDbContext>(options =>
			{
				options.UseNpgsql(_config.GetConnectionString("TplDbContext"));
			});


			services.Configure<CookiePolicyOptions>(options =>
			{
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});


			services.AddMvc(options =>
			{
				options.ModelBinderProviders.Insert(0, new StringTrimModelBinderProvider());
				options.ModelBinderProviders.Insert(0, new WhereParamsModelBinderProvider());

				options.Filters.Add(new DevelopAuthorizationFilter<ACT>());
				options.Filters.Add(new UseViewPageActionFilter());
				options.Filters.Add(new ExceptionMessageActionFilter(typeof(OrionException)));
				options.Filters.Add(new PageParamsActionFilter("PageSize", 50)); /* 換頁參數 */


				//options.Filters.Add(new AuthorizationFilter());
				//C:\inetpub\temp\IIS Temporary Compressed Files\
				//options.Filters.Add(new AuthorizeAttribute()); /* 驗證是否登入 */

			});

			services.AddMemoryCache();
			//services.AddSession();

			services.AddHealthChecks();

			services.AddAuthorization(options =>
			{
				//options.AddPolicy("RequireAuthenticatedUserPolicy", builder => builder.RequireAuthenticatedUser());

				options.DefaultPolicy = new AuthorizationPolicyBuilder()
				  .RequireAuthenticatedUser()
				  .Build();
			});



			services
				.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					/* Cookie settings */
					options.Cookie.HttpOnly = true;
					options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

					options.LoginPath = "/Account/Login";
					options.AccessDeniedPath = "/Error/Forbidden";

					options.SlidingExpiration = true;
					
					options.Events.OnRedirectToAccessDenied = (redirectContext) => 
					{
						redirectContext.HttpContext.Response.StatusCode = 403;
						return Task.FromResult(0);
					};
				});

			services.AddSingleton<IPostConfigureOptions<CookieAuthenticationOptions>, ConfigureSessionAuthentication>();

			//services.AddAuthentication(options =>
			//{
			//	// the scheme name has to match the value we're going to use in AuthenticationBuilder.AddScheme(...)
			//	options.DefaultScheme = "Custom Scheme";
			//	//options.DefaultAuthenticateScheme = "Custom Scheme";
			//	//options.DefaultSignInScheme = "Custom Scheme";
			//	//options.DefaultSignOutScheme = "Custom Scheme";
			//	//options.DefaultChallengeScheme = "Custom Scheme";
			//})


		}




		/// <summary>ConfigureContainer is where you can register things directly with Autofac. This runs after ConfigureServices so the things here will override registrations made in ConfigureServices. Don't build the container; that gets done for you by the factory. </summary>
		public void ConfigureContainer(ContainerBuilder builder)
		{
			builder.RegisterModule<OrionNLogModule>();


			builder.Register(r => _config.GetSection("tplConfig").Get<TplConfig>());

			var menuReg = builder.Register(r => new MenuProvider("Menu.config")).As<IMenuProvider>();
			var breadReg = builder.Register(r => new BreadcrumbProvider("Breadcrumb.config")).As<IBreadcrumbProvider>();
#if !DEBUG
			/* 正式時切換成單例 */
			menuReg.SingleInstance();
			breadReg.SingleInstance();
#endif

			builder.RegisterServiceContext<IServiceContext>().InstancePerLifetimeScope();

			builder.RegisterType<OptionItemsProvider>();
			builder.Register<IOptionItemsProvider>(r => r.Resolve<OptionItemsProvider>()).GetterCacheWrap().InstancePerLifetimeScope();


			builder.RegisterType<PasswordSHA256Handle>().As<IPasswordHandle>();


			builder.RegisterAssemblyTypes(Assembly.Load("CoreTpl.Dao"))
				   .Where(t => t.Name.EndsWith("Dao"))
				   .AsImplementedInterfaces();


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
			}
			else
			{
				app.UseExceptionHandler("/Home/Error?code=500");
				//app.UseHsts(); /* HTTP 強制安全傳輸 */
			}

			//app.UseElmah();
			//app.UseSession(); //TODO ??
			app.UseCookiePolicy();

			app.UseStatusCodePagesWithReExecute("/Home/Error", "?code={0}");

			//app.UseHttpsRedirection();
			app.UseStaticFiles();


			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}"
				).RequireAuthorization();
				//endpoints.MapRazorPages(); TODO ??

				endpoints.MapHealthChecks("/healthz", new HealthCheckOptions() { });
				//endpoints.MapHealthChecks("/healthz")
				//	.RequireAuthorization(new AuthorizeAttribute() { Roles = "admin", });

			});

		}



	}
}
