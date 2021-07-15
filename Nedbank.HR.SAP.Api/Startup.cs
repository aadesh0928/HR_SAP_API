using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Nedbank.EAPI.SDK.Builder;
using Nedbank.EAPI.SDK.DependencyInjection;
using Nedbank.HR.SAP.BAL.Business;
using Nedbank.HR.SAP.BAL.Filters;
using Nedbank.HR.SAP.BAL.Interfaces;
using Nedbank.HR.SAP.BAL.ViewModels;
using Nedbank.HR.SAP.DAL.Interfaces;
using Nedbank.HR.SAP.DAL.Repository;
using Nedbank.HR.SAP.Service.Authentcation;
using Nedbank.HR.SAP.Service.Exception;
using Nedbank.HR.SAP.Shared.Interfaces;
using Nedbank.HR.SAP.Shared.Models;

namespace Nedbank.HR.SAP.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //CoE framework configuration.
            ServiceConfiguration.ConfigureServices(services, Configuration);

            services
            .AddMvc(options =>
            {
               // options.Filters.Add(typeof(StaffAuthenticationFilter));
                options.Filters.Add(typeof(HRSAPExceptionFilter));
            });
            services.AddSingleton<IConfiguration>(Configuration);

            services.Configure<AuthorizationSettings>(
                Configuration.GetSection(nameof(AuthorizationSettings)));
            services.AddSingleton<IAuthorizationSettings>(setting =>
            setting.GetRequiredService<IOptions<AuthorizationSettings>>().Value);

            services.Configure<SearchFilterSettings>(
                Configuration.GetSection(nameof(SearchFilterSettings)));
            services.AddSingleton<ISearchFilterSettings>(sf =>
                sf.GetRequiredService<IOptions<SearchFilterSettings>>().Value);

            services.Configure<BulkUpdateSettings>(
                Configuration.GetSection(nameof(BulkUpdateSettings)));
            services.AddSingleton<IBulkUpdateSettings>(sf =>
                sf.GetRequiredService<IOptions<BulkUpdateSettings>>().Value);

            services.Configure<DatabaseSettings>(
            Configuration.GetSection(nameof(DatabaseSettings)));

            services.AddSingleton<IDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
            services.AddSingleton(typeof(IMongoConnector<>), typeof(MongoConnector<>));
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IResultSet, ResultSet>();
            services.AddScoped<IMetaDataResult, MetaDataResult>();
            services.AddScoped<IEmployeeBusiness, EmployeeBusiness>();
            services.AddScoped<IProductBusiness, ProductBusiness>();
            services.AddScoped(typeof(IValidationResult<>), typeof(ValidationResult<>));
            services.AddScoped(typeof(IExpressionFilter<>), typeof(ExpressionFilter<>));
            services.AddScoped(typeof(IAugmentedResult<>), typeof(AugmentedResult<>));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddAutoMapper(option => option.AddProfile(new AutoMapperProfile()));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            PipelineConfiguration.Configure(app, env, Configuration);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            } else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
