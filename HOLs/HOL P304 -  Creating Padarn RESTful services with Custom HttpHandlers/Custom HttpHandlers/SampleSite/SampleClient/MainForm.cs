using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SampleClient
{
    public delegate void ErrorEventHandler(object sender, Exception ex);

    public partial class MainForm : Form
    {
        private Book[] m_books;

        private RestConnector Connector { get; set; }
        private BookClient BookClient { get; set; }

        public event EventHandler BooksChanged;
        public event ErrorEventHandler ServiceError;

        private Book[] Books 
        {
            get { return m_books; }
            set
            {
                m_books = value;
                if (BooksChanged != null)
                {
                    BooksChanged(this, null);
                }
            }
        }

        public MainForm()
        {
            InitializeComponent();

            Connector = new DigestRestConnector(deviceIP.Text, "adminuser", "adminpass", false);
            BookClient = new BookClient(Connector);

            deviceIP.Validated += new EventHandler(deviceIP_Validated);

            BooksChanged += new EventHandler(MainForm_BooksChanged);
            ServiceError += new ErrorEventHandler(MainForm_ServiceError);
            
            BeginGetBooks();
        }

        void MainForm_ServiceError(object sender, Exception ex)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ErrorEventHandler(MainForm_ServiceError), new object[] { sender, ex });
                return;
            }

            status.Text = ex.Message;
            status.Visible = true;
        }

        void MainForm_BooksChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler(MainForm_BooksChanged), new object[] { sender, e });
                return;
            }

            RefreshBookList();
            status.Visible = false;
        }

        void deviceIP_Validated(object sender, EventArgs e)
        {
            Connector.DeviceAddress = deviceIP.Text;
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            BeginGetBooks();
        }

        private void BeginGetBooks()
        {
            status.Text = "Getting book list...";
            status.Visible = true;

            ThreadPool.QueueUserWorkItem(delegate
                {
                    try
                    {
                        Books = BookClient.GetAllBooks();
                    }
                    catch(Exception ex)
                    {
                        if (ServiceError != null)
                        {
                            ServiceError(this, ex);
                        }
                        else
                        {
                            throw;
                        }
                    }
                });
        }

        private void RefreshBookList()
        {
            bookList.Items.Clear();

            foreach (Book b in Books)
            {
                ListViewItem lvi = new ListViewItem(new string[] 
            {
                b.ID.ToString(),
                b.Title,
                b.Author,
                b.Pages.HasValue ? b.Pages.ToString() : "<null>"
            });
                lvi.Tag = b;

                bookList.Items.Add(lvi);
            }
        }

        private void add_Click(object sender, EventArgs e)
        {
            EditForm f = new EditForm();
            f.Text = "Add New Book";

            if (f.ShowDialog() == DialogResult.OK)
            {
                BookClient.AddNewBook(f.Title, f.Author, f.Pages);
                BeginGetBooks();
            }

            f.Dispose();
        }

        private void modify_Click(object sender, EventArgs e)
        {
            if (bookList.SelectedItems.Count == 0) return;
            Book existing = bookList.SelectedItems[0].Tag as Book;

            EditForm f = new EditForm(existing);
            f.Text = "Update Book";

            if (f.ShowDialog() == DialogResult.OK)
            {
                BookClient.UpdateBook(existing.ID, f.Title, f.Author, f.Pages);
                BeginGetBooks();
            }

            f.Dispose();
        }

        private void delete_Click(object sender, EventArgs e)
        {
            if (bookList.SelectedItems.Count == 0) return;
            Book existing = bookList.SelectedItems[0].Tag as Book;

            if (MessageBox.Show(string.Format("Delete book '{0}'?", existing.Title), 
                "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                BookClient.DeleteBook(existing.ID);
                BeginGetBooks();
            }
        }
    }
}
