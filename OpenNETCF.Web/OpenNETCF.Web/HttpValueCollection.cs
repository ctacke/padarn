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
using System.Collections.Specialized;
using System.Collections;

namespace OpenNETCF.Web
{
    internal class HttpValueCollection : NameValueCollection
    {
        internal HttpValueCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        internal HttpValueCollection(int capacity)
            : base(capacity, (IEqualityComparer)StringComparer.OrdinalIgnoreCase)
        {
        }

        internal void FillFromEncodedBytes(byte[] bytes, Encoding encoding, bool isQueryString)
        {
            if (bytes.Length <= 0) return;
            bool datastart = false;

            string[] list = encoding.GetString(bytes, 0, bytes.Length).Split(new char[] { '\n' } );

            foreach (string item in list)
            {
                string val = item.Trim();
                // find the line break before the post data
                if ((!isQueryString) && (!datastart) && (!string.IsNullOrEmpty(val))) continue;
                datastart = true;

                int end = val.IndexOf("=");
                if (end < 1)
                {
                    // key but no value
                    var name = HttpUtility.UrlDecode(val);
                    base.Add(name, null);
                }
                else
                {
                    int start = 0;
                    while (end >= 0)
                    {
                        string name = HttpUtility.UrlDecode(val.Substring(start, end - start));
                        start = end + 1;
                        end = val.IndexOf('&', start);
                        string value;
                        if (end >= 0)
                        {
                            value = HttpUtility.UrlDecode(val.Substring(start, end - start));
                        }
                        else
                        {
                            value = HttpUtility.UrlDecode(val.Substring(start));
                        }

                        base.Add(name, value);

                        if (end >= 0)
                        {
                            start = end + 1;
                            end = val.IndexOf("=", start);
                        }
                    }
                }
            }
        }

        internal void FillFromString(string s)
        {
            this.FillFromString(s, false, null);
        }

        internal void FillFromString(string s, bool urlencoded, Encoding encoding)
        {
            int length = (s != null) ? s.Length : 0;
            for (int i = 0; i < length; i++)
            {
                int startIndex = i;
                int endIndex = -1;
                while (i < length)
                {
                    char ch = s[i];
                    if (ch == '=')
                    {
                        if (endIndex < 0)
                        {
                            endIndex = i;
                        }
                    }
                    else if (ch == '&')
                    {
                        break;
                    }
                    i++;
                }
                string name = null;
                string value = null;
                if (endIndex >= 0)
                {
                    name = s.Substring(startIndex, endIndex - startIndex);
                    value = s.Substring(endIndex + 1, (i - endIndex) - 1);
                }
                else
                {
                    value = s.Substring(startIndex, i - startIndex);
                }
                if (urlencoded)
                {
                    base.Add(HttpUtility.UrlDecode(name, encoding), HttpUtility.UrlDecode(value, encoding));
                }
                else
                {
                    base.Add(name, value);
                }
                if ((i == (length - 1)) && (s[i] == '&'))
                {
                    base.Add(null, string.Empty);
                }
            }
        }

        internal void MakeReadOnly()
        {
            base.IsReadOnly = true;
        }

        internal void MakeReadWrite()
        {
            base.IsReadOnly = false;
        }

        internal void Reset()
        {
            base.Clear();
        }

        public override string ToString()
        {
            return this.ToString(true);
        }

        internal virtual string ToString(bool urlencoded)
        {
            return this.ToString(urlencoded, null);
        }

        internal virtual string ToString(bool urlencoded, IDictionary excludeKeys)
        {
            int count = this.Count;
            if (count == 0)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            bool flag = (excludeKeys != null);
            for (int i = 0; i < count; i++)
            {
                string key = this.GetKey(i);
                if (((!flag || (key == null)) || (((excludeKeys == null) || (key == null)) || (excludeKeys[key] == null))))
                {
                    string str3;
                    if (urlencoded)
                    {
                        key = HttpUtility.UrlEncodeUnicode(key);
                    }
                    string str2 = !string.IsNullOrEmpty(key) ? (key + "=") : string.Empty;
                    ArrayList list = (ArrayList)base.BaseGet(i);
                    int num3 = (list != null) ? list.Count : 0;
                    if (builder.Length > 0)
                    {
                        builder.Append('&');
                    }
                    if (num3 == 1)
                    {
                        builder.Append(str2);
                        str3 = (string)list[0];
                        if (urlencoded)
                        {
                            str3 = HttpUtility.UrlEncodeUnicode(str3);
                        }
                        builder.Append(str3);
                    }
                    else if (num3 == 0)
                    {
                        builder.Append(str2);
                    }
                    else
                    {
                        for (int j = 0; j < num3; j++)
                        {
                            if (j > 0)
                            {
                                builder.Append('&');
                            }
                            builder.Append(str2);
                            str3 = (string)list[j];
                            if (urlencoded)
                            {
                                str3 = HttpUtility.UrlEncodeUnicode(str3);
                            }
                            builder.Append(str3);
                        }
                    }
                }
            }
            return builder.ToString();
        }
    }

}
