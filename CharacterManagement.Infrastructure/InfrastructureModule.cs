using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CharacterManagement.Infrastructure
{
    public class InfrastructureModule
    {
        /// <summary>
        /// This method will add all classes in this dll to a collection for dependency injection
        /// </summary>
        /// <param name="collection"></param>
        public static void RegisterServices(IServiceCollection serviceCollection)
        {
            var repositoryTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => type.Name.EndsWith("Repository"));

            var allAssemblyTypes = Assembly.GetExecutingAssembly()
                .GetReferencedAssemblies()
                .Select(Assembly.Load)
                .SelectMany(x => x.DefinedTypes);

            foreach (var type in repositoryTypes)
            {
                var interfaceType = allAssemblyTypes.Where(assemblyType => assemblyType.Name == "I" + type.Name).FirstOrDefault();
                //TODO: LOG, THEN CRASH IF DEFAULT WAS RETURNED
                serviceCollection.AddScoped(interfaceType, type);
            }
        }
    }
}
