using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SampleClient
{
    public class BookClient
    {
        protected const string XML_HEADER = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
        private const string ENTITY_NAME = "books";

        private RestConnector Connector { get; set; }

        public BookClient(RestConnector connector)
        {
            Connector = connector;
        }

        public Book[] GetAllBooks()
        {
            List<Book> books = new List<Book>();

            string xml = Connector.Get(ENTITY_NAME);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            foreach (XmlNode node in doc.SelectNodes("books/book"))
            {
                books.Add(Book.FromXml(node.OuterXml));
            }

            return books.ToArray();
        }

        public void AddNewBook(string title, string author, int? pages)
        {
            Book book = new Book
            {
                Title = title,
                Author = author,
                Pages = pages
            };

            string xml = string.Format("{0}{1}", XML_HEADER, book.AsXml());
            Connector.Post(ENTITY_NAME, xml);
        }

        public void UpdateBook(int id, string title, string author, int? pages)
        {
            Book book = new Book
            {
                ID = id,
                Title = title,
                Author = author,
                Pages = pages
            };

            string xml = string.Format("{0}{1}", XML_HEADER, book.AsXml());
            Connector.Put(ENTITY_NAME, xml);
        }

        public void DeleteBook(int id)
        {
            string address = string.Format("{0}/{1}/", ENTITY_NAME, id);
            Connector.Delete(address);
        }
    }
}
