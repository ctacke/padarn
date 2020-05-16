//                                                                   
// Copyright (c) 2007-2008 OpenNETCF Consulting, LLC                        
//                                                                     

namespace OpenNETCF.Web
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHttpModule
    {
        /// <summary>
        /// 
        /// </summary>
        void Dispose();

        /// <summary>
        /// 
        /// </summary>
        void Init(HttpContext context);
    }   
}
