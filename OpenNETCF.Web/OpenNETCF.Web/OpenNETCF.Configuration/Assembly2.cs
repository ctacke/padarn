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
using System.Reflection;
using System.Runtime.InteropServices;

namespace OpenNETCF.Reflection
{
	/// <summary>
	/// Contains helper functions for the <see cref="System.Reflection.Assembly"/> class.
	/// </summary>
	/// <seealso cref="System.Reflection.Assembly"/>
	internal static class Assembly2
	{
		/// <summary>
		/// Gets the process executable.
		/// </summary>
		/// <returns>The <see cref="Assembly"/> that is the process executable.</returns>
		public static Assembly GetEntryAssembly()
		{
			byte[] buffer = new byte[256 * Marshal.SystemDefaultCharSize];
			int chars = GetModuleFileName(IntPtr.Zero, buffer, 255);

			if(chars > 0)
			{
				if(chars > 255)
				{
					throw new System.IO.PathTooLongException("Assembly name is longer than MAX_PATH characters.");
				}

				string assemblyPath = System.Text.Encoding.Unicode.GetString(buffer, 0, chars * Marshal.SystemDefaultCharSize);

				return Assembly.LoadFrom(assemblyPath);
			}
			else
			{
				return null;
			}

		}

		[DllImport("coredll.dll", SetLastError=true)]
		private static extern int GetModuleFileName(IntPtr hModule, byte[] lpFilename, int nSize);

	}
}
