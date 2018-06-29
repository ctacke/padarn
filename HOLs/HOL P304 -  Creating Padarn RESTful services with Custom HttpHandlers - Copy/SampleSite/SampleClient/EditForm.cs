using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SampleClient
{
    public partial class EditForm : Form
    {
        public EditForm()
        {
            InitializeComponent();
        }

        public EditForm(Book book)
        {
            InitializeComponent();

            if (book == null) return;

            title.Text = book.Title;
            author.Text = book.Author;

            if (book.Pages.HasValue)
            {
                pages.Text = book.Pages.ToString();
            }
        }

        public string Title
        {
            get { return title.Text; }
        }

        public string Author
        {
            get { return author.Text; }
        }

        public int? Pages
        {
            get 
            {
                if (pages.Text == string.Empty) return null;

                return int.Parse(pages.Text);
            }
        }
    }
}
