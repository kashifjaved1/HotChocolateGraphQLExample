using GraphQLDemo.API.Models;
using GraphQLDemo.API.Repositories;
using GraphQLDemo.API.GraphQL.Mutations;
using GraphQLDemo.API.GraphQL.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using GraphQLDemo.API.GraphQL.Types;

namespace GraphQLDemo.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                //.AddSubscriptionType<Subscription>()
                .AddType<InstructorType>()
                .AddType<CourseType>()
                .AddTypeExtension<CourseQuery>() // use registering extended/extension types
                .AddFiltering() // for adding data filteration
                .AddSorting() // for sorting
                .AddProjections()
                .AddAuthorization(); // for avoiding over-fetching

            // services.AddInMemorySubscriptions();

            string connectionString = _configuration.GetConnectionString("default");
            services.AddPooledDbContextFactory<SchoolDbContext>(o => 
                o.UseSqlite(connectionString)
                .LogTo(Console.WriteLine) // to log db queries to console.
            ); // added dbcontext pool to avoid collision of parallel running graphQL resolvers.

            // services.AddPooledDbContextFactory<SchoolDbContext>(o => o.UseSqlServer(connectionString));

            services.AddScoped<CourseRepository>();
            services.AddScoped<InstructorRepository>();

            services.AddAuthentication();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();

            app.UseWebSockets();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
