using System;
using System.Windows.Forms;
using OpenNETCF.Web.Server;
using System.Net;
using System.Net.Sockets;

namespace ocfhttpd
{
  public partial class Form1 : Form
  {
    private WebServer m_padarnServer = new WebServer();

    public Form1()
    {
      InitializeComponent();

      startButton.Click += new EventHandler(startButton_Click);
      stopButton.Click += new EventHandler(stopButton_Click);

      IPHostEntry localHost = Dns.GetHostEntry(Dns.GetHostName());
      for (int x = 0; x < localHost.AddressList.Length; x++)
      {
        if (localHost.AddressList[x].AddressFamily == AddressFamily.InterNetwork)
        {
          ipLabel.Text = localHost.AddressList[x].ToString();
          break;
        }
      }
    }

    void stopButton_Click(object sender, EventArgs e)
    {
      if (m_padarnServer.Running)
      {
        m_padarnServer.Stop();
      }
    }

    void startButton_Click(object sender, EventArgs e)
    {
      if (!m_padarnServer.Running)
      {
        try
        {
          m_padarnServer.Start();
        }
        catch (SocketException)
        {
          MessageBox.Show("Port 80 is already in use.  Ensure no other web server is running.");
        }
      }
    }
  }
}