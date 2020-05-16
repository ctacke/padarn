using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Security
{
    internal abstract class Authentication : IHttpModule
    {
        private readonly string authMethod;

        /// <summary>
        /// 
        /// </summary>
        public string AuthenticationMethod
        {
            get { return authMethod; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authenticationMethod"></param>
        public Authentication(string authenticationMethod)
        {
            authMethod = authenticationMethod;
        }

        protected bool AuthenticationRequired
        {
            get { return (string.Compare(AuthenticationMethod, Configuration.ServerConfig.GetConfig().Authentication.Mode, true) == 0); }
        }

        public virtual void Dispose()
        {
        }

        public void Init(HttpContext context)
        {
        }

        protected void DenyAccess(HttpContext context)
        {
            context.Response.SendStatus(401, "Access Denied", true);
            context.Response.Write("401 Access Denied.");
        }

        protected string Authorization(HttpContext context, string authenticationMethod)
        {
            string requestedAuthMethod = context.Request.Headers["Authorization"];
            if ((requestedAuthMethod == null) || (authenticationMethod.Length == 0))
                return null; // Anonymous request 

            if (requestedAuthMethod.ToLower().StartsWith(authenticationMethod.ToLower()))
                return requestedAuthMethod.Substring(authenticationMethod.Length + 1);
            
            return null;
        }

        public virtual void OnAuthenticateRequest(object sender, EventArgs e)
        {
            if (!AuthenticationRequired)
                return;
            HttpContext context = (HttpContext) sender;

            string authData = Authorization(context, AuthenticationMethod);
            if( String.IsNullOrEmpty(authData) || !AcceptCredentials(context, authData))
            {
                DenyAccess(context);
                return;
            }
        }

        public abstract void OnEndRequest(object sender, EventArgs e);

        public abstract bool AcceptCredentials(HttpContext context, string authentication);

        internal abstract string User { get; }
    }
}
