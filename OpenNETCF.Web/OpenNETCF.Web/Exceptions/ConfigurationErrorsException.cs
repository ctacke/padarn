using System;
using System.Collections.Generic;
using System.Text;

#if !WindowsCE
using System.Configuration;
#endif

// disable warnings about obsoletes
#pragma warning disable 612, 618

namespace OpenNETCF.Configuration
{
    /// <summary>
    /// The current value is not one of the EnableSessionState values.
    /// </summary>
    public class ConfigurationErrorsException : ConfigurationException
    {
        private string _firstFilename;
        private int _firstLine;

        /// <summary>
        /// Initializes a new instance of a ConfigurationErrorsException class
        /// </summary>
        /// <param name="message">A message that describes why this ConfigurationErrorsException exception was thrown.</param>
        /// <param name="inner">The inner exception that caused this ConfigurationErrorsException exception to be thrown.</param>
        public ConfigurationErrorsException(string message, Exception inner)
            : this(message, inner, null, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of a ConfigurationErrorsException class
        /// </summary>
        /// <param name="message">A message that describes why this ConfigurationErrorsException exception was thrown.</param>
        /// <param name="inner">The inner exception that caused this ConfigurationErrorsException exception to be thrown.</param>
        /// <param name="filename">The line number within the configuration file at which this ConfigurationErrorsException exception was thrown.</param>
        /// <param name="line">The path to the configuration file that caused this ConfigurationErrorsException exception to be thrown.</param>
        public ConfigurationErrorsException(string message, Exception inner, string filename, int line)
            : base(message, inner)
        {
            this.Init(filename, line);
        }

        private void Init(string filename, int line)
        {
            base.HResult = -2146232062;
            if (line == -1)
            {
                line = 0;
            }
            this._firstFilename = filename;
            this._firstLine = line;
        }

 

 


    }
}
#pragma warning restore 612, 618
