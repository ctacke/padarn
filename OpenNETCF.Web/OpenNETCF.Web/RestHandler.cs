using System;

using System.Collections.Generic;
using System.Text;
using System.Reflection;
using OpenNETCF.Web.Configuration;
using OpenNETCF.Web.Rest;

namespace OpenNETCF.Web
{
    internal class RestHandler : IHttpHandler
    {

        static Dictionary<RestMethodKey, MethodInfo> _restTypes = new Dictionary<RestMethodKey, MethodInfo>();

        static RestHandler ()
	    {
            InitializeTypes();
    	}

        public bool IsReusable
        {
            get { return true; }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            HttpMethod method = (HttpMethod)Enum.Parse(typeof(HttpMethod), context.Request.HttpMethod, true);
            var m = GetMethodForRequest(context.Request.Path, method);
            object o = Activator.CreateInstance(m.ReflectedType);
            m.Invoke(o, null);
        }

        Type GetTypeForRequest(string path)
        {
            throw new NotImplementedException();
            //Type returnValue = null;
            //string lowerPath = path.ToLower();
            //if (_restTypes.ContainsKey(lowerPath))
            //{
            //    returnValue = _restTypes[lowerPath];
            //}
            //return returnValue;
        }

        MethodInfo GetMethodForRequest(string path, HttpMethod method)
        {
            MethodInfo returnValue = null;
            var t = GetTypeForRequest(path);
            foreach (var m in t.GetMethods())
            {
                object[] restMethods = m.GetCustomAttributes(typeof(WebGetAttribute), true);
                foreach (WebGetAttribute a in restMethods)
                {
                    //if (a.Uri == method)
                    //{
                    //    returnValue = m;
                    //    break;
                    //}
                }
                if (returnValue != null)
                {
                    break;
                }
            }
            return returnValue;
        }

        static void InitializeTypes()
        {
            foreach (var a in ServerConfig.GetConfig().Rest)
            {
                foreach (var t in a.GetTypes())
                {
                    object[] restAttributes = t.GetCustomAttributes(typeof(ServiceContractAttribute), true);

                    if (restAttributes.Length > 0)
                    {
                        foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                        {
                            object[] webGet = m.GetCustomAttributes(typeof(WebGetAttribute), true);
                            foreach (WebGetAttribute g in webGet)
                            {
                                _restTypes.Add(new RestMethodKey { UriTemplate = g.UriTemplate }, m);
                            }
                        }

                    }
                }
            }

        }
    }
}
