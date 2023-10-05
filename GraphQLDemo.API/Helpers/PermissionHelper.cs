using GraphQLDemo.API.Data;
using GraphQLDemo.API.Data.Entities;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Helpers
{
    public class PermissionHelper
    {
        private static SchoolDbContext _context;

        // Static method to initialize the DbContext
        public static void InitializeContext(SchoolDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public static async Task<List<string>> GetPermissionsFromDB()
        {
            if (_context == null)
            {
                throw new InvalidOperationException("DbContext has not been initialized. Call InitializeContext before using GetPermissionsFromDB.");
            }
            List<string> permissions = new();
            var permsFromDb = await _context.Permissions.ToListAsync();
            foreach (var permission in permsFromDb)
            {
                permissions.Add(permission.Name);
            }

            return permissions;
        }

        public static List<string> GetAllPermissions() => new()
        {
            "Permissions.Create",
            "Permissions.View",
            "Permissions.Edit",
            "Permissions.Delete",
        };

        public static List<string> GeneratePermissionsForModule(string module) => new()
        {
            $"Permissions.{module}.Create",
            $"Permissions.{module}.View",
            $"Permissions.{module}.Edit",
            $"Permissions.{module}.Delete",
        };
    }
}