using SimpleDataManagementSystem.Shared.Common.Constants;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Helpers
{
    public static class PermissionsHelper
    {
        public static bool IsInRole(Roles requiredRole, string userRoleName)
        {
            if (!Enum.TryParse(userRoleName, out Roles role))
            {
                return false;
            }

            if (requiredRole != role)
            {
                return false;
            }

            return true;
        }

        public static bool IsResourceOwner(string resourceOwnerId, string userId)
        {
            return false;
        }
    }
}
