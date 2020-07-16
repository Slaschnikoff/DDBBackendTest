using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharacterManagement.Contracts.Entities;
using CharacterManagement.Domain;
using CharacterManagement.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CharacterManagement.API
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
            services.AddControllers();
            Infrastructure.InfrastructureModule.RegisterServices(services);
            Domain.DomainModule.RegisterServices(services);

            //for the purposes of a demo i am creating my first character here on startup. A bit messy with DI, typically i might have something in the 
            //  infrastructure layer that did any initlizing, with entity framework there is version seeding for instance
            List<Character> characters = new List<Character>();
            CharacterRepository characterRepository = new CharacterRepository();
            CharacterHealthService characterHealthService = new CharacterHealthService(characterRepository);
            characters.Add(DemoStartup.InitilizeDemoCharacter(characterHealthService));
            CharacterRepository.InitilizeCharacterArray(characters);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
