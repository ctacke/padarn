using System.Xml;
using System.Text;

public class Book
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int? Pages { get; set; }

    public static Book FromXml(string xml)
    {
        Book b = new Book();

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);

        XmlNode node = doc.SelectSingleNode("book/id");
        b.ID = int.Parse(node.InnerText);

        node = doc.SelectSingleNode("book/title");
        b.Title = node.InnerText;

        node = doc.SelectSingleNode("book/author");
        b.Author = node.InnerText;

        node = doc.SelectSingleNode("book/pages");
        if (node == null)
        {
            b.Pages = null;
        }
        else
        {
            b.Pages = int.Parse(node.InnerText);
        }

        return b;
    }

    public string AsXml()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<book>");
        sb.Append(string.Format("<id>{0}</id>", this.ID));
        sb.Append(string.Format("<title>{0}</title>", this.Title));
        sb.Append(string.Format("<author>{0}</author>", this.Author));
        if (Pages.HasValue)
        {
            sb.Append(string.Format("<pages>{0}</pages>", this.Pages));
        }
        sb.Append("</book>");

        return sb.ToString();
    }
}
