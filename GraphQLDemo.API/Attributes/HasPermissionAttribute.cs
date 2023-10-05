// For Rest API 
//using Microsoft.AspNetCore.Authorization;
//using System.Collections.Generic;

//namespace GraphQLDemo.API.Attributes
//{
//    public sealed class HasPermissionAttribute : AuthorizeAttribute
//    {
//        public HasPermissionAttribute(string permission) : base(policy: permission.ToString())
//        {
//        }

//        public HasPermissionAttribute(List<string> permissions) : base(policy: permissions.ToString())
//        {
//        }
//    }
//}

// For GraphQL API
using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
public class HasPermissionAttribute : ObjectFieldDescriptorAttribute
{
    private readonly string _requiredPermission;

    public HasPermissionAttribute(string requiredPermission)
    {
        _requiredPermission = requiredPermission;
    }

    public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
    {
        descriptor.Use(next => async context =>
        {
            var httpContextAccessor = context.Services.GetRequiredService<IHttpContextAccessor>();
            var userClaims = httpContextAccessor.HttpContext.User.Claims;
            var permissionsClaim = userClaims.FirstOrDefault(c => c.Type == "Permissions")?.Value;

            if (string.IsNullOrEmpty(permissionsClaim) || !permissionsClaim.Contains(_requiredPermission))
            {
                context.ReportError($"User does not have the required permission: {_requiredPermission}");
                return;
            }

            await next(context);
        });
    }
}
