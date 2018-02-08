using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SinjulMSBH_Blog.Data;
using SinjulMSBH_Blog.Models;
using SinjulMSBH_Blog.Services;

namespace SinjulMSBH_Blog
{
	public class Startup
	{
		public Startup ( IConfiguration configuration )
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices ( IServiceCollection services )
		{
			services.AddDbContext<ApplicationDbContext>( options =>
			     options.UseSqlServer( Configuration.GetConnectionString( "DefaultConnection" ) ) );

			services.AddIdentity<ApplicationUser , IdentityRole>( )
			    .AddEntityFrameworkStores<ApplicationDbContext>( )
			    .AddDefaultTokenProviders( );

			services.AddAntiforgery( opts => opts.HeaderName = "XSRF-TOKEN" );

			services.AddMvc( );
			services.AddResponseCompression( );

			services.AddAuthentication( )
				.AddGoogle( options =>
				{
					options.ClientId = Configuration[ "GoogleClientId" ] ?? "MissingClientId";
					options.ClientSecret = Configuration[ "GoogleClientSecret" ] ?? "MissingClientSecret";
					options.SaveTokens = true;
				} );

			//Needed for accessing action context in the tag helpers
			services.TryAddSingleton<IActionContextAccessor , ActionContextAccessor>( );

			// Add application repositories as scoped dependencies so they are shared per every request.
			services.AddScoped<IArticlesRepository , ArticlesRepository>( );

			// Add application services.
			services.AddTransient<IEmailSender , AuthMessageSender>( );
			services.AddTransient<ISmsSender , AuthMessageSender>( );
			services.AddTransient<IGooglePictureLocator , GooglePictureLocator>( );
			services.AddTransient<IRequestUserProvider , RequestUserProvider>( );
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure ( IApplicationBuilder app , IHostingEnvironment env )
		{
			if ( env.IsDevelopment( ) )
			{
				app.UseBrowserLink( );
				app.UseDeveloperExceptionPage( );
				app.UseDatabaseErrorPage( );
			}
			else
			{
				app.UseExceptionHandler( "/Home/Error" );
			}

			app.UseResponseCompression( );

			app.UseStaticFiles( );

			app.UseAuthentication( );

			app.UseMvc( routes =>
			 {
				 routes.MapRoute(
			    name: "default" ,
			    template: "{controller=Home}/{action=Index}/{id?}" );
			 } );
		}
	}
}