using System.Collections.Generic;
using System.Linq;
using HollypocketBackend.Models;

namespace HollypocketBackend.Utils
{
    public static class ExtensionMethods
    {
        public static IEnumerable<Account> WithoutPasswords(this IEnumerable<Account> users)
        {
            return users.Select(x => x.WithoutPassword());
        }

        public static Account WithoutPassword(this Account user)
        {
            user.Password = null;
            return user;
        }
   }
}
