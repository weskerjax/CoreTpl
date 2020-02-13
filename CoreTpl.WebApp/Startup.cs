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


			//TODO ���䴩 3.1 ����
			//services.AddElmah<XmlFileErrorLog>(options =>
			//{
			//	options.LogPath = _config.GetValue<string>("elmah:LogPath");
			//	options.FiltersConfig = getFilePath("elmah.xml");
			//});


			services.Configure<CookiePolicyOptions>(options =>
			{
				options.CheckConsentNeeded = context => false; /* ���ҥ� GDPR �o�|�y�� Cookie �L�k�g�J */
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});
					   

			services.AddMemoryCache(); /* �W�[�O����֨� */
			//services.AddSession(); /* �W�[ Session */

			//services.AddHealthChecks(); /* �W�[���d���p�ˬd */

			services.AddAuthorization(); /* �t�m�v�����ҳB�z */


			/* �t�m�n�J�̳B�z */
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

			/* �� MemoryCache �Ӭ����n�J�̸�� */
			services.AddSingleton<IPostConfigureOptions<CookieAuthenticationOptions>, ConfigureSessionAuthentication>();


			/* �t�m MVC */
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
					options.Filters.Add(new PageParamsActionFilter("PageSize", 50)); /* �����Ѽ� */
				})
#if DEBUG
				.AddRazorRuntimeCompilation() /* �ҥ� Razor Runtime �sĶ */
#endif
				.AddControllersAsServices();

		}




		/// <summary>
		/// �t�m Autofac Builder 
		/// ConfigureContainer is where you can register things directly with Autofac. This runs after ConfigureServices so the things here will override registrations made in ConfigureServices. Don't build the container; that gets done for you by the factory.
		/// </summary>
		public void ConfigureContainer(ContainerBuilder builder)
		{
			builder.RegisterModule<OrionNLogModule>();

			/* TplConfig �t�m */
			builder.Register(r => _config.GetSection("tplConfig").Get<TplConfig>());

			/* Sidebar Menu �P�ѥ]�h */
			var menuReg = builder.Register(r => new MenuProvider("Menu.config")).As<IMenuProvider>();
			var breadReg = builder.Register(r => new BreadcrumbProvider("Breadcrumb.config")).As<IBreadcrumbProvider>();
#if !DEBUG
			/* �����ɤ�������� */
			menuReg.SingleInstance();
			breadReg.SingleInstance();
#endif

			/* ServiceContext �t�m */
			builder.RegisterServiceContext<IServiceContext>().InstancePerLifetimeScope();

			/* OptionItemsProvider �t�m */
			builder.RegisterType<OptionItemsProvider>();
			builder.Register<IOptionItemsProvider>(r => r.Resolve<OptionItemsProvider>()).GetterCacheWrap().InstancePerLifetimeScope();

			/* �K�X�B�z */
			builder.RegisterType<PasswordSHA256Handle>().As<IPasswordHandle>();


			/* DAO �t�m */
			builder.RegisterAssemblyTypes(Assembly.Load("CoreTpl.Dao"))
				   .Where(t => t.Name.EndsWith("Dao"))
				   .AsImplementedInterfaces();


			/* Service �t�m */
			builder.RegisterAssemblyTypes(Assembly.Load("CoreTpl.Service.Impl"))
				   .Where(t => t.Name.EndsWith("Service"))
				   .AsImplementedInterfaces();


			/* ���U�����W�٬� Controller �� class */
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
				//app.UseHsts(); /* HTTP �j��w���ǿ� */
			}

			//app.UseElmah(); //TODO ���䴩 3.1 ����
			//app.UseSession(); /* �ҥ� Session */
			app.UseCookiePolicy();

			app.UseStatusCodePagesWithReExecute("/Home/Error");

			//app.UseHttpsRedirection();


			/* �h��y���]�w */
			var supportedCultures = getSupportedCultures().ToArray();

			app.UseRequestLocalization(options =>
			{
				options.DefaultRequestCulture = new RequestCulture(supportedCultures[0].Name);
				options.SupportedCultures = supportedCultures; /* Formatting numbers, dates, etc. */
				options.SupportedUICultures = supportedCultures; /* UI strings that we have localized. */
			});


			app.UseRouting(); /* �ҥθ��� */
			app.UseStaticFiles(); /* �R�A�ɮ� (JS, CSS) */
			app.UseAuthentication(); /* �ҥέ������v���ˬd */
			app.UseAuthorization(); /* �ҥεn�J�̸�ƳB�z */


			/* MVC ���Ѱt�m */
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}"
				).RequireAuthorization();
				
				/* �t�m���d���p�ˬd�� Routing */
				//endpoints.MapHealthChecks("/healthz", new HealthCheckOptions() { });

			});
		}



		/// <summary>�h��y���M��</summary>
		private IEnumerable<CultureInfo> getSupportedCultures() 
		{
			var zhTW = new CultureInfo("zh-TW");

			zhTW.DateTimeFormat.DateSeparator = "/"; /* Format �|�N / �ഫ�����]�w�A�קK���D���������j�Ÿ� */
			zhTW.DateTimeFormat.MonthDayPattern = "MM-dd";
			zhTW.DateTimeFormat.LongDatePattern = "yyyy-MM-dd";
			zhTW.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
			zhTW.DateTimeFormat.LongTimePattern = "HH:mm:ss";
			zhTW.DateTimeFormat.ShortTimePattern = "HH:mm:ss";

			yield return zhTW;

		}





	}





}
