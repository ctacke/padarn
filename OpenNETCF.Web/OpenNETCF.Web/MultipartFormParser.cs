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
using System.Collections;

namespace OpenNETCF.Web
{
    internal sealed class MultipartFormParser
    {
        // Fields
        private byte[] _boundary;
        private HttpRawRequestContent _data;
        private List<MultipartContentItem> _elements = new List<MultipartContentItem>();
        private Encoding _encoding;
        private bool _lastBoundaryFound;
        private int _length;
        private int _lineLength = -1;
        private int _lineStart = -1;
        private string _partContentType;
        private int _partDataLength = -1;
        private int _partDataStart = -1;
        private string _partFilename;
        private string _partName;
        private int _pos;

        // Methods
        private MultipartFormParser(HttpRawRequestContent data, int length, byte[] boundary, Encoding encoding)
        {
            this._data = data;
            this._length = length;
            this._boundary = boundary;
            this._encoding = encoding;
        }

        private bool AtBoundaryLine()
        {
            int length = this._boundary.Length;
            if ((this._lineLength != length) && (this._lineLength != (length + 2)))
            {
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                if (this._data[this._lineStart + i] != this._boundary[i])
                {
                    return false;
                }
            }
            if (this._lineLength != length)
            {
                if ((this._data[this._lineStart + length] != 0x2d) || (this._data[(this._lineStart + length) + 1] != 0x2d))
                {
                    return false;
                }
                this._lastBoundaryFound = true;
            }
            return true;
        }

        private bool AtEndOfData()
        {
            if (this._pos < this._length)
            {
                return this._lastBoundaryFound;
            }
            return true;
        }

        private string ExtractValueFromContentDispositionHeader(string l, int pos, string name)
        {
            string str = name + "=\"";
            int startIndex = l.IndexOf(str, pos, StringComparison.OrdinalIgnoreCase);
            if (startIndex < 0)
            {
                return null;
            }
            startIndex += str.Length;
            int index = l.IndexOf('"', startIndex);
            if (index < 0)
            {
                return null;
            }
            if (index == startIndex)
            {
                return string.Empty;
            }
            return l.Substring(startIndex, index - startIndex);
        }

        private bool GetNextLine()
        {
            int num = this._pos;
            this._lineStart = -1;
            while (num < this._length)
            {
                if (this._data[num] == 10)
                {
                    this._lineStart = this._pos;
                    this._lineLength = num - this._pos;
                    this._pos = num + 1;
                    if ((this._lineLength > 0) && (this._data[num - 1] == 13))
                    {
                        this._lineLength--;
                    }
                    break;
                }
                if (++num == this._length)
                {
                    this._lineStart = this._pos;
                    this._lineLength = num - this._pos;
                    this._pos = this._length;
                }
            }
            return (this._lineStart >= 0);
        }

        internal static List<MultipartContentItem> Parse(HttpRawRequestContent data, int length, byte[] boundary, Encoding encoding)
        {
            MultipartFormParser parser = new MultipartFormParser(data, length, boundary, encoding);
            parser.ParseIntoElementList();
            return parser._elements;
            //return (MultipartContentItem[])parser._elements.ToArray(typeof(MultipartContentItem));
        }

        private void ParseIntoElementList()
        {
            while (this.GetNextLine())
            {
                if (this.AtBoundaryLine())
                {
                    break;
                }
            }
            if (this.AtEndOfData())
            {
                return;
            }
        Label_001B:
            this.ParsePartHeaders();
            if (!this.AtEndOfData())
            {
                this.ParsePartData();
                if (this._partDataLength != -1)
                {
                    if (this._partName != null)
                    {
                        this._elements.Add(new MultipartContentItem(this._partName, this._partFilename, this._partContentType, this._data, this._partDataStart, this._partDataLength));
                    }
                    if (!this.AtEndOfData())
                    {
                        goto Label_001B;
                    }
                }
            }
        }

        private void ParsePartData()
        {
            this._partDataStart = this._pos;
            this._partDataLength = -1;
            while (this.GetNextLine())
            {
                if (this.AtBoundaryLine())
                {
                    int num = this._lineStart - 1;
                    if (this._data[num] == 10)
                    {
                        num--;
                    }
                    if (this._data[num] == 13)
                    {
                        num--;
                    }
                    this._partDataLength = (num - this._partDataStart) + 1;
                    return;
                }
            }
        }

        private void ParsePartHeaders()
        {
            this._partName = null;
            this._partFilename = null;
            this._partContentType = null;
            while (this.GetNextLine())
            {
                if (this._lineLength == 0)
                {
                    return;
                }
                byte[] buffer = new byte[this._lineLength];
                this._data.CopyBytes(this._lineStart, buffer, 0, this._lineLength);
                string l = this._encoding.GetString(buffer,0,buffer.Length);
                int index = l.IndexOf(':');
                if (index >= 0)
                {
                    string str2 = l.Substring(0, index);
                    if (str2.Equals("Content-Disposition",StringComparison.OrdinalIgnoreCase))
                    {
                        this._partName = this.ExtractValueFromContentDispositionHeader(l, index + 1, "name");
                        this._partFilename = this.ExtractValueFromContentDispositionHeader(l, index + 1, "filename");
                    }
                    else if (str2.Equals("Content-Type",StringComparison.OrdinalIgnoreCase))
                    {
                        this._partContentType = l.Substring(index + 1).Trim();
                    }
                }
            }
        }
    }


}
