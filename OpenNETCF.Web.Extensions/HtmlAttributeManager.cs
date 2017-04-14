using System;
using OpenNETCF.Web.UI;
using System.Text;

namespace OpenNETCF.Web.UI
{
    /// <summary>
    /// Handles the writing of attributes to the HtmlTextWriter specified
    /// in the constructor.
    /// </summary>
    public class HtmlAttributeManager
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="writer">The writer to use for writing.</param>
        public HtmlAttributeManager(HtmlTextWriter writer)
        {
            Validate.Begin()
                .IsNotNull(writer)
                .Check();

            this.Writer = writer;
        }

        /// <summary>
        /// The writer this manager writes to.
        /// </summary>
        public HtmlTextWriter Writer { get; private set; }

        /// <summary>
        /// Applies the value to the specified attribute to the HtmlTextWriter
        /// this instance contains.
        /// </summary>
        /// <param name="attribute">The attribute to set.</param>
        /// <param name="value">The value to set to the attribute.</param>
        /// <returns>The attribute manager.</returns>
        public HtmlAttributeManager this[HtmlTextWriterAttribute attribute, string value] 
        {
            get 
            {
                this.Writer.AddAttribute(attribute, value);
                return this;
            }
        }

        /// <summary>
        /// Applies the value to the specified attribute to the HtmlTextWriter
        /// this instance contains.
        /// </summary>
        /// <param name="attribute">The attribute to set.</param>
        /// <param name="value">The value to set to the attribute.</param>
        /// <returns>The attribute manager.</returns>
        public HtmlAttributeManager this[string attribute, string value]
        {
            get
            {
                this.Writer.AddAttribute(attribute, value);
                return this;
            }
        }

        /// <summary>
        /// Applies specified attribute (with no value) to the HtmlTextWriter
        /// this instance contains.
        /// </summary>
        /// <param name="attribute">The attribute to set.</param>
        /// <returns>The attribute manager.</returns>
        public HtmlAttributeManager this[HtmlTextWriterAttribute attribute]
        {
            get
            {
                this.Writer.AddAttribute(attribute, null);
                return this;
            }
        }

        /// <summary>
        /// Applies specified attribute (with no value) to the HtmlTextWriter
        /// this instance contains.
        /// </summary>
        /// <param name="attribute">The attribute to set.</param>
        /// <returns>The attribute manager.</returns>
        public HtmlAttributeManager this[string attribute]
        {
            get
            {
                this.Writer.AddAttribute(attribute, null);
                return this;
            }
        }

        /// <summary>
        /// Adds the class attribute to the tag being rendered.
        /// </summary>
        /// <param name="className">The name of the class.</param>
        /// <returns>The attribute manager.</returns>
        public HtmlAttributeManager Class(string className)
        {
            return this[HtmlTextWriterAttribute.Class, className];
        }

        /// <summary>
        /// Adds the class attribute to the tag being rendered.
        /// </summary>
        /// <param name="classNames">The names of the classes to set to the attribute.</param>
        /// <returns>The attribute manager.</returns>
        public HtmlAttributeManager Class(params string[] classNames)
        {
            var namesString = new StringBuilder();

            foreach (var name in classNames)
            {
                if (namesString.Length > 0)
                {
                    namesString.Append(" ");
                }

                namesString.Append(name);
            }

            return this[HtmlTextWriterAttribute.Class, namesString.ToString()];
        }

        /// <summary>
        /// Adds the id attribute to the tag being rendered.
        /// </summary>
        /// <param name="className">The id to set.</param>
        /// <returns>The attribute manager.</returns>
        public HtmlAttributeManager Id(string elementId)
        {
            return this[HtmlTextWriterAttribute.Id, elementId];
        }

        /// <summary>
        /// Adds the name attribute to the tag being rendered.
        /// </summary>
        /// <param name="className">The name to set.</param>
        /// <returns>The attribute manager.</returns>
        public HtmlAttributeManager Name(string elementName)
        {
            return this[HtmlTextWriterAttribute.Name, elementName];
        }


    }
}
