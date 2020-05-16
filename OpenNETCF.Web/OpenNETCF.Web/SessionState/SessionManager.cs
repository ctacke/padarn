using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.Web.Configuration;
using System.Diagnostics;

namespace OpenNETCF.Web.SessionState
{
    internal class SessionManager
    {
        private static SessionManager m_instance;
        private Dictionary<string, HttpSessionState> m_sessions = new Dictionary<string, HttpSessionState>();
        private SessionConfiguration m_config;
        private SessionIDManager m_manager;

        private SessionManager()
        {
            m_config = Configuration.ServerConfig.GetConfig().Session;
            m_manager = new SessionIDManager();
        }

        internal static SessionManager Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new SessionManager();
                }

                return m_instance;
            }
        }

        public HttpSessionState GetSession(HttpContext context)
        {
            if (!m_config.AllowSessionState) return null;

            string id;
            HttpSessionState session;

            lock (m_manager)
            {
                // create or get the session
                id = m_manager.GetSessionID(context);

                if (id == null)
                {
                    id = m_manager.CreateSessionID(context);

                    bool redirected;
                    bool saved;
                    m_manager.SaveSessionID(context, id, out redirected, out saved);
                }

                if (m_sessions.ContainsKey(id))
                {
                    session = m_sessions[id];
                }
                else
                {
                    if (m_config.MaxSessions >= m_sessions.Count)
                    {
                        session = new HttpSessionState(id, context);
                        session.TimedOut += new EventHandler(session_TimedOut);
                        m_sessions.Add(id, session);
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            // set timeout from config
            session.Timeout = m_config.Timeout;

            return session;
        }

        void session_TimedOut(object sender, EventArgs e)
        {
            var session = sender as HttpSessionState;
            Debug.WriteLine(string.Format("Session '{0}' timed out", session.SessionID));

            lock (m_manager)
            {
                m_sessions.Remove(session.SessionID);
                m_manager.RemoveSessionID(session.Context);
            }
        }
    }
}
