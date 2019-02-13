using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using WebApi.Models;
using WebGame.Domain;

namespace WebApi
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
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMvc(options =>
            {
                options.ReturnHttpNotAcceptable = true;
                options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                options.InputFormatters.Add(new XmlDataContractSerializerInputFormatter(options));

                //var jsonInputFormatter = setupAction.InputFormatters
                //    .OfType<JsonInputFormatter>().FirstOrDefault();
                //jsonInputFormatter?.SupportedMediaTypes
                //    .Add("application/vnd.kontur.v2+json");

                //var jsonOutputFormatter = setupAction.OutputFormatters
                //    .OfType<JsonOutputFormatter>().FirstOrDefault();
                //jsonOutputFormatter?.SupportedMediaTypes
                //    .Add("application/vnd.kontur.hateoas+json");

            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<UserEntity, UserDto>()
                    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                        $"{src.FirstName} {src.LastName}"));

                cfg.CreateMap<UserToCreate, UserEntity>()
                    .ConstructUsing(x => new UserEntity(Guid.NewGuid()));
            });

            services.AddSingleton<IUserRepository, InMemoryUserRepository>();

            AddSwaggerGen(services);
        }

        private void AddSwaggerGen(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("web-game", new OpenApiInfo
                {
                    Title = "Web Game API",
                    Version = "0.1",
                });

                c.DescribeAllEnumsAsStrings();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.EnableAnnotations();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/web-game/swagger.json", "Web Game API");
                //c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
