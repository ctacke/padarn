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
using System.Collections.Specialized;
using System.Globalization;

namespace OpenNETCF.Configuration
{
	/// <summary>
	/// Provides a method for reading values of a particular type from the .config file.
	/// </summary>
	internal class AppSettingsReader
	{
		private NameValueCollection map;
		private static Type stringType = typeof(string);
		private static Type[] paramsArray = new Type[]{stringType};
		private static string NullString = "None";

		/// <summary>
		/// 
		/// </summary>
		public AppSettingsReader()
		{
          map = ConfigurationSettings.AppSettings;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public object GetValue(string key, Type type)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			string keyVal = map[key];
			if (keyVal == null)
			{
				throw new InvalidOperationException("No Key: " + key);
			}
			if (type == stringType)
			{
				int i = GetNoneNesting(keyVal);
				if (i == 0)
				{
					return keyVal;
				}
				if (i == 1)
				{
					return null;
				}
				else
				{
					return keyVal.Substring(1, keyVal.Length - 2);
				}
			}
			try
			{
				return Convert.ChangeType(keyVal, type, null);
			}
			catch (Exception)
			{
				string exceptionVal = (keyVal.Length != 0) ? keyVal : "AppSettingsReaderEmptyString";
				throw new InvalidOperationException("Can't Parse " +  exceptionVal + " for key " + key + " of type " + type.ToString());
			}
		}

		private int GetNoneNesting(string val)
		{
			int i = 0;
			int j = val.Length;
			char[] chars = val.ToCharArray();
			if (j > 1)
			{
				for (i++; chars[i] == '(' && chars[j - i - 1] == ')'; i++)
				{
				}
				if (i > 0 && String.Compare(NullString, 0, val, i, j - 2 * i, false, CultureInfo.InvariantCulture) != 0)
				{
					i = 0;
				}
			}
			return i;
		}
	}
}
