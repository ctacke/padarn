// 
// Copyright (c) 2007-2010 OpenNETCF Consulting, LLC                        
//                                                                     
namespace OpenNETCF.Web
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Security;
    using System.Text;
    using Configuration;
    using System.Diagnostics;

    /// <summary>
    /// Provides access to specific file types.
    /// </summary>
    internal class StaticFileHandler : IHttpHandler
    {
        private readonly string m_localFile;
        private readonly string m_mime;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="mimeType"></param>
        internal StaticFileHandler(string filePath, string mimeType)
        {
            m_localFile = filePath;
            m_mime = mimeType;
        }

        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// Processes the incoming HTTP request
        /// </summary>
        /// <param name="context">The HttpContext for the request</param>
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;

            string filename = m_localFile; // TODO: Re-instate request.PhysicalPath;

            byte[] content;
            using (FileStream fs = new FileStream(m_localFile, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    FileInfo requestedFile;

                    try
                    {
                        requestedFile = new FileInfo(m_localFile);
                    }
                    catch (IOException)
                    {
                        throw new HttpException(404, Resources.HttpEnumeratorError);
                    }
                    catch (SecurityException)
                    {
                        throw new HttpException(401, Resources.HttpEnumeratorDenied);
                    }

                    string ims = request.Headers["HTTP_IF_MODIFIED_SINCE"];
                    if ((ims != null) && (ims.Length > 0))
                    {
                        try
                        {
                            DateTime t = DateTime.Parse(ims, DateTimeFormatInfo.CurrentInfo);
                            if (requestedFile.LastWriteTime.Subtract(t.ToUniversalTime()).TotalSeconds < 1)
                            {
                                HttpContext.Current.Response.SendStatus(304, "Not Modified: " + m_mime, true);
                                return;
                            }
                        }
                        catch
                        {
                            Debug.WriteLine("Unable to parse header 'HTTP_IF_MODIFIED_SINCE' value: " + ims);
                            // ignore and continue, we just won't return a 304
                        }
                    }

                    if ((requestedFile.Attributes & FileAttributes.Hidden) != 0)
                    {
                        throw new HttpException(404, Resources.HttpFileHidden);
                    }

                    if (filename[filename.Length - 1].Equals('.'))
                    {
                        throw new HttpException(404, Resources.HttpFileNotFound);
                    }

                    DateTime lastModTime = requestedFile.LastWriteTime;

                    if (lastModTime > DateTime.Now)
                    {
                        lastModTime = DateTime.Now;
                    }

                    string strETag = GenerateETag(context, lastModTime);

                    try
                    {
                        BuildFileItemResponse(context, filename, requestedFile.Length, lastModTime, strETag, requestedFile.LastWriteTime);
                    }
                    catch (Exception)
                    {
                        throw new HttpException(401, Resources.HttpAccessForbidden);
                    }


                    HttpContext.Current.Response.ForcedContentLength = fs.Length;
                    HttpContext.Current.Response.ContentType = m_mime;

                    // packetize output to prevent OOMs on serving large files
                    using (BinaryReader r = new BinaryReader(fs))
                    {
                        var totalSent = 0;

                        try
                        {
                            do
                            {
                                // TODO: allow this to be configurable
                                content = r.ReadBytes(0x40000); // 256k

                                if (content.Length > 0)
                                {
                                    // check for a broken connection (i.e. a cancelled download)
                                    if (!HttpContext.Current.Response.IsClientConnected)
                                    {
                                        return;
                                    }

                                    HttpContext.Current.Response.Write(content);
                                    HttpContext.Current.Response.Flush();
                                }

                                totalSent += content.Length;
                            } while (totalSent < fs.Length);
                        }
                        finally
                        {
                            r.Close();

                            // if we sent more than 1MB, clean up
                            if (totalSent > 0x100000)
                            {
                                GC.Collect();
                            }
                        }
                    }
                }
                finally
                {
                    fs.Close();
                }
            }
        }

        private static DateTimeFormatInfo m_dtfi = new DateTimeFormatInfo();
        private static void BuildFileItemResponse(HttpContext context, string fileName, long fileSize,
                                                  DateTime lastModifiedTime, string strETag, DateTime lastChange)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;

            response.AppendHeader(Resources.HeaderAcceptRanges, Resources.HeaderAcceptRangesBytes);
            response.AppendHeader("Last-modified", lastChange.ToUniversalTime().ToString(m_dtfi.RFC1123Pattern));
            //string rangeHeader = request.Headers["Range"];

            // TODO: Complete implementation of BuildFileItemResponse
        }

        internal static string GenerateETag(HttpContext context, DateTime lastModTime)
        {
            StringBuilder builder = new StringBuilder();
            long num = DateTime.Now.ToFileTime();
            long num2 = lastModTime.ToFileTime();
            builder.Append("\"");
            builder.Append(num2.ToString("X8", CultureInfo.InvariantCulture));
            builder.Append(":");
            builder.Append(num.ToString("X8", CultureInfo.InvariantCulture));
            builder.Append("\"");
            if ((DateTime.Now.ToFileTime() - num2) <= 0x1c9c380)
            {
                return ("W/" + builder.ToString());
            }
            return builder.ToString();
        }


    }
}
