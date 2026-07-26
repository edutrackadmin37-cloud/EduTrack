using System.Linq;
using System.Security.Principal;

namespace EduTrack.Helpers
{
    public class RolePrincipal : GenericPrincipal
    {
        private readonly string[] _roles;

        public RolePrincipal(IIdentity identity, string[] roles) : base(identity, roles)
        {
            _roles = roles ?? new string[0];
        }

        public override bool IsInRole(string role)
        {
            return _roles.Contains(role);
        }

        public bool HasPermission(string permission)
        {
            return IsInRole(permission);
        }
    }
}