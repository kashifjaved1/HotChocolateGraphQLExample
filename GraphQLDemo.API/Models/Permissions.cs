using System.Collections.Generic;

namespace GraphQLDemo.API.Models
{
    public class Permissions
    {
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