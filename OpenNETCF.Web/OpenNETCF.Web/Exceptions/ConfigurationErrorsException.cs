#region License
// Copyright ©2017 Tacke Consulting (dba OpenNETCF)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
// and associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, 
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or 
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR 
// ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
#endregion
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
