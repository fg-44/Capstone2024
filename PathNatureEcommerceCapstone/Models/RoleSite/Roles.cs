using PathNatureEcommerceCapstone.Models.DatabaseCapstone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace PathNatureEcommerceCapstone.Models.RoleSite
{
    public class Roles : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            using (var context = new PathNatureDbEcommerce())
            {
                // Utilizza la relazione tra le tabelle per ottenere il ruolo associato all'utente
                var userRole = context.Members
                    .Where(u => u.Username == username)
                    .Select(u => u.Roles.RoleName)  // Utilizza la relazione per ottenere il ruolo
                    .FirstOrDefault();

                if (userRole != null)
                {
                    switch (userRole)
                    {
                        case "User":
                            return new[] { "BasicAccess" }; // Permission for regular users
                        case "Client":
                            return new[] { "ClientAccess" }; // Permission for clients
                        case "Admin":
                            return new[] { "AdminAccess" }; // Permission for administrators
                        default:
                            // Handle unrecognized roles
                            break;
                    }
                }

                // Handle the case where user role is not found
                return new string[] { }; // Return empty array if user role not found
            }
        }



        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}