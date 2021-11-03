using MaskCrawler.Extensions;
using MaskCrawler.Web.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MaskCrawler
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
            //services.AddRazorPages();

            services.AddOverallServices(Configuration);
            //Dapper.FluentMap.FluentMapper.Initialize(configure =>
            //{
            //    // ������ò�����Ч,��Ϊû��ʹ�� FluentMap���в�ѯ
            //    // �ڲ�ʹ�õ��� SimpleCRUD
            //    configure.AddMap(new AccountMap());
            //    configure.AddMap(new TaskMap());
            //    //configure.ForDommel(); //�����Dommel
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Error");
                app.UseSelfExceptionHandler();
            }

            app.UseOverallConfigure(env, Configuration);
        }
    }
}
