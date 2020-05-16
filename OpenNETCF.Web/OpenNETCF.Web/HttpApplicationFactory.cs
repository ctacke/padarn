namespace OpenNETCF.Web
{
    /// <summary>
    /// 
    /// </summary>
    internal static class HttpApplicationFactory
    {
        private static DefaultHttpHandler m_defaultHandler;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wr"></param>
        /// <returns></returns>
        public static IHttpHandler GetApplicationInstance(HttpWorkerRequest wr)
        {
            if (m_defaultHandler == null)
            {
                m_defaultHandler = new DefaultHttpHandler();
            }

            if (m_defaultHandler.IsReusable)
            {
                return m_defaultHandler;
            }

            return new DefaultHttpHandler();
        }
    }
}
