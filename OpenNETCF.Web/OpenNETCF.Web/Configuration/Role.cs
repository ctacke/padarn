using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Web.Configuration
{
  internal sealed class Role
  {
    private string m_name;

    public string Name
    {
      get { return m_name; }
      internal set { m_name = value; }
    }
  }
}
