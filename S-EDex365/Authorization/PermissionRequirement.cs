using Microsoft.AspNetCore.Authorization;

namespace S_EDex365.Authorization
{
    public class PermissionRequirement: IAuthorizationRequirement
    {
        public string Permission { get; }
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
