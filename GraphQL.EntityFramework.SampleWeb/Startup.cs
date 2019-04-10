using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphiQl;
using GraphQL;
using GraphQL.EntityFramework;
using GraphQL.Server;
using GraphQL.Subscription;
using GraphQL.Types;
using GraphQL.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApplication1.DataContexts;
using WebApplication1.GraphTypes;
using ExecutionContext = GraphQL.Execution.ExecutionContext;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            GraphTypeTypeRegistry.Register<Book, BookGraph>();
            GraphTypeTypeRegistry.Register<Author, AuthorGraph>();
            services.AddDbContext<BookDbContext>(options =>
            options.UseSqlServer(@"Server=.\SQLEXPRESS;Initial Catalog = TestApp; Integrated Security = True;"));

            services.AddDbContext<AuthorDbContext>(options =>
            options.UseSqlServer(@"Server=.\SQLEXPRESS;Initial Catalog = TestApp2; Integrated Security = True;"));

            var serviceProvider = services.BuildServiceProvider();
            var bookBuilder = new BookContextBuilder(serviceProvider);
            var authorBuilder = new AuthorContextBuilder(serviceProvider);

            EfGraphQLConventions.RegisterInContainer(services, authorBuilder.Model);
            EfGraphQLConventions.RegisterInContainer(services, bookBuilder.Model);

            foreach (var type in GetGraphQlTypes())
            {
                services.AddSingleton(type);
            }

            services.AddGraphQL(options => options.ExposeExceptions = true)
                    .AddWebSockets();

            services.AddSingleton<IDocumentExecuter, EfDocumentExecuter>();
            services.AddSingleton<IDependencyResolver>(
                provider => new FuncDependencyResolver(provider.GetRequiredService));
            services.AddSingleton<ISchema, Schema>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        private static IEnumerable<Type> GetGraphQlTypes()
        {
            return typeof(Startup).Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract &&
                            (typeof(IObjectGraphType).IsAssignableFrom(x) ||
                             typeof(IInputObjectGraphType).IsAssignableFrom(x)));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.UseGraphQLWebSockets<ISchema>();
            app.UseGraphiQl("/graphiql", "/graphql");
            app.UseMvc();
        }
    }

    public class BookContextBuilder
    {
        public BookContextBuilder(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<BookDbContext>();
            this.Model = context.Model;
        }

        public IModel Model;
    }

    public class AuthorContextBuilder
    {
        public AuthorContextBuilder(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<AuthorDbContext>();
            this.Model = context.Model;
        }

        public IModel Model;
    }
}
