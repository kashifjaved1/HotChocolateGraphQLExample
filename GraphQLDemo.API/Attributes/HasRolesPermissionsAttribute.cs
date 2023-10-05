using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQLDemo.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class HasRolesPermissionsAttribute : ObjectFieldDescriptorAttribute
    {
        private readonly string[] _roles;
        private readonly string[] _permissions;

        public HasRolesPermissionsAttribute(params string[] rolesAndPermissions)
        {
            _roles = rolesAndPermissions.Where(r => !r.StartsWith("Permissions.")).ToArray();
            _permissions = rolesAndPermissions.Where(p => p.StartsWith("Permissions.")).ToArray();
        }

        public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
        {
            descriptor.Use(next => async context =>
            {
                // Check roles
                var httpContextAccessor = context.Services.GetRequiredService<IHttpContextAccessor>();
                var userRoles = _roles.Where(role => httpContextAccessor.HttpContext.User.IsInRole(role)).ToArray();
                var missingRoles = _roles.Except(userRoles).ToArray();

                // Check permissions only if user has any of the specified roles
                if (userRoles.Any())
                {
                    var userClaims = httpContextAccessor.HttpContext.User.Claims;
                    var permissionsClaim = userClaims
                    .Where(c => c.Type == "Permissions")
                    .SelectMany(c => c.Value.Split(','))
                    .ToList();

                    var requiredPermissions = _permissions.Select(p => p);
                    var missingPermissions = requiredPermissions
                        .Where(permission => !permissionsClaim.Contains(permission))
                        .ToArray();

                    if (missingPermissions.Any())
                    {
                        missingPermissions = missingPermissions.Select(p => p.Replace("Permissions.", "")).ToArray();
                        context.ReportError($"User does not have the required permission(s): {string.Join(", ", missingPermissions)}");
                        return;
                    }
                }
                else
                {
                    // Report missing roles only if user has none of the specified roles
                    if (missingRoles.Any())
                    {
                        context.ReportError($"User does not have the required role(s): {string.Join(", ", missingRoles)}");
                        return;
                    }
                }

                await next(context);
            });
        }
    }
}
