using AppAny.HotChocolate.FluentValidation;
using FluentValidation.AspNetCore;
using GraphQLDemo.API.GraphQL.Mutations;
using GraphQLDemo.API.GraphQL.Queries;
using GraphQLDemo.API.GraphQL.Types;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Repositories;
using GraphQLDemo.API.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GraphQLDemo.API
{
    public static class ServiceExtensions
    {
        public static void ProjectSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFluentValidation();
            services.AddTransient<CourseTypeInputValidator>();

            services.AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                //.AddSubscriptionType<Subscription>()
                .AddType<InstructorType>()
                .AddType<CourseType>()
                .AddTypeExtension<CourseQuery>() // use registering extended/extension types
                .AddFiltering() // for adding data filteration
                .AddSorting() // for sorting
                .AddProjections() // for avoiding over-fetching
                .AddAuthorization()
                .AddFluentValidation(o =>
                 {
                     o.UseDefaultErrorMapper();
                 }); // registering Appany fluentValidation extensions method

            // services.AddInMemorySubscriptions();

            string connectionString = configuration.GetConnectionString("default");
            services.AddPooledDbContextFactory<SchoolDbContext>(o =>
                o.UseSqlite(connectionString)
                .LogTo(Console.WriteLine) // to log db queries to console.
            ); // added dbcontext pool to avoid collision of parallel running graphQL resolvers.

            // services.AddPooledDbContextFactory<SchoolDbContext>(o => o.UseSqlServer(connectionString));

            services.AddScoped<CourseRepository>();
            services.AddScoped<InstructorRepository>();

            services.AddAuthentication();
        }
    }
}
