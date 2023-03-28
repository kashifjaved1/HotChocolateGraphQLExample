using AppAny.HotChocolate.FluentValidation;
using FluentValidation.AspNetCore;
using GraphQLDemo.API.GraphQL.Mutations;
using GraphQLDemo.API.GraphQL.Queries;
using GraphQLDemo.API.GraphQL.Types;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Models.Entities;
using GraphQLDemo.API.Repositories.Implementation;
using GraphQLDemo.API.Services.Helpers;
using GraphQLDemo.API.Services.Implementation;
using GraphQLDemo.API.Services.Interfaces;
using GraphQLDemo.API.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace GraphQLDemo.API
{
    public static class ServiceExtensions
    {
        public static void ProjectSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFluentValidation();
            services.AddTransient<CourseTypeInputValidator>();

            services.AddCors(corsPolicy =>
            {
                corsPolicy.AddPolicy("AllowAll", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });

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

            services.AddScoped<CourseRepository>();
            services.AddScoped<InstructorRepository>();

            services.AddScoped<IAuthManager, AuthManager>();

            services.AddIdentity<ApiUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<SchoolDbContext>();

            services.AddDbContext<SchoolDbContext>(o => o.UseSqlite(connectionString));

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 1;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.Lockout.MaxFailedAccessAttempts = 2;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(300);
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
            });

            // configuring jwt
            var issuer = configuration["JWT:Issuer"];
            var audiance = configuration["JWT:Audiance"];
            var key = configuration["JWT:KEY"];

            services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            )
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidAudience = audiance,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                };
            });
            services.AddAuthorization();

            // setting forgot, reset and change password token TTL
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
               opt.TokenLifespan = TimeSpan.FromHours(2));

            // configuring email here.
            var emailConfig = configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailSender, EmailSender>();
        }
    }
}
