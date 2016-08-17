using SgqSystem.ViewModels;
using System.Collections.Generic;
using System.Web;

namespace SgqSystem.Secirity
{
    public static class SessionPersister
    {
        
        static string userNameSessionVar = "username";

        public static string Username
        {
            get
            {
                if (HttpContext.Current == null)
                    return string.Empty;
                var sessionaVar = HttpContext.Current.Session[userNameSessionVar];
                if (sessionaVar != null)
                    return sessionaVar as string;
                return null;
            }
            set
            {
                HttpContext.Current.Session[userNameSessionVar] = value;
            }
        }

    }

}