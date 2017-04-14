#region License
// Copyright ©2017 Tacke Consulting (dba OpenNETCF)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
// and associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, 
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or 
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR 
// ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
#endregion
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
