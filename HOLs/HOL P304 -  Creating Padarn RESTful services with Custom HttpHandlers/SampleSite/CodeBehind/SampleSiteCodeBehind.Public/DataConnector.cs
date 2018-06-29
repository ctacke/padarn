using System;

using System.Collections.Generic;
using System.Text;
using System.Data.SqlServerCe;
using System.Reflection;
using System.IO;
using System.Xml;

namespace SampleSite
{
    internal class DataConnector : IDisposable
    {
        private static DataConnector m_instance;

        private SqlCeConnection Connection { get; set; }

        private DataConnector()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);

            Connection = new SqlCeConnection(string.Format("Data Source={0}\\Books.sdf;Persist Security Info=False", path));
            Connection.Open();
        }

        ~DataConnector()
        {
            Dispose();
        }

        public void Dispose()
        {
            if(Connection != null)
            {
                Connection.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        public static DataConnector GetInstance()
        {
            if (m_instance == null)
            {
                m_instance = new DataConnector();
            }

            return m_instance;
        }

        public Book[] GetAllBooks()
        {
            string sql = "SELECT BookID, Title, Author, Pages FROM Books";

            List<Book> books = new List<Book>();

            using(SqlCeCommand cmd = new SqlCeCommand(sql, Connection))
            using(var resultset = cmd.ExecuteResultSet(ResultSetOptions.Scrollable))
            {
                if (resultset.HasRows)
                {
                    while (resultset.Read())
                    {
                        books.Add(new Book
                        {
                            ID = resultset.GetInt32(0),
                            Title = resultset.GetString(1),
                            Author = resultset.GetString(2),
                            Pages = resultset.IsDBNull(3) ? null : (int?)resultset.GetInt32(3)
                        });
                    }
                }
            }

            return books.ToArray();
        }
        
        public void DeleteBook(int bookID)
        {
            string sql = string.Format("DELETE FROM Books WHERE BookID = {0}", bookID);

            using (SqlCeCommand cmd = new SqlCeCommand(sql, Connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void InsertBook(Book book)
        {
            string sql = string.Format("INSERT INTO Books (Title, Author, Pages) VALUES ('{0}', '{1}', {2})",
                book.Title,
                book.Author,
                book.Pages.HasValue ? book.Pages.ToString() : "NULL");

            using(SqlCeCommand cmd = new SqlCeCommand(sql, Connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateBook(Book book)
        {
            string sql = string.Format("UPDATE Books SET Title = '{0}', Author = '{1}', Pages = {2} WHERE BookID = {3}",
                book.Title,
                book.Author,
                book.Pages.HasValue ? book.Pages.ToString() : "NULL",
                book.ID);

            using (SqlCeCommand cmd = new SqlCeCommand(sql, Connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
