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
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using OpenNETCF.Configuration;
using OpenNETCF.Web.Configuration;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;

namespace OpenNETCF.Web
{
    ///<summary>
    /// Provides access to detailed information about the capabilities of the client's browser.
    /// </summary>
    public class HttpCapabilitiesBase
    {
        #region Fields
        private NameValueCollection m_headers;
        private Dictionary<string, string> m_capabilities = null;
        private static XmlDocument CombinedBrowserDocument = null;
        private static object CombinedBrowserDocumentSyncRoot = new object();

        private List<string> _browsers;
        private static object _staticLock = new object();

        #region variables to see if the properties have been retreived
        private volatile bool _hasBackButton;
        private volatile bool _haveactivexcontrols;
        private volatile bool _haveaol;
        private volatile bool _havebackgroundsounds;
        private volatile bool _havebeta;
        private volatile bool _havebrowser;
        private volatile bool _haveCanCombineFormsInDeck;
        private volatile bool _haveCanInitiateVoiceCall;
        private volatile bool _haveCanRenderAfterInputOrSelectElement;
        private volatile bool _haveCanRenderEmptySelects;
        private volatile bool _haveCanRenderInputAndSelectElementsTogether;
        private volatile bool _haveCanRenderMixedSelects;
        private volatile bool _haveCanRenderOneventAndPrevElementsTogether;
        private volatile bool _haveCanRenderPostBackCards;
        private volatile bool _haveCanRenderSetvarZeroWithMultiSelectionList;
        private volatile bool _haveCanSendMail;
        private volatile bool _havecdf;
        private volatile bool _havecookies;
        private volatile bool _havecrawler;
        private volatile bool _haveDefaultSubmitButtonLimit;
        private volatile bool _haveecmascriptversion;
        private volatile bool _haveframes;
        private volatile bool _haveGatewayMajorVersion;
        private volatile bool _haveGatewayMinorVersion;
        private volatile bool _haveGatewayVersion;
        private volatile bool _haveHasBackButton;
        private volatile bool _haveHidesRightAlignedMultiselectScrollbars;
        private volatile bool _haveInputType;
        private volatile bool _haveIsColor;
        private volatile bool _haveIsMobileDevice;
        private volatile bool _havejavaapplets;
        private volatile bool _havejavascript;
        private volatile bool _havejscriptversion;
        private volatile bool _havemajorversion;
        private volatile bool _haveMaximumHrefLength;
        private volatile bool _haveMaximumRenderedPageSize;
        private volatile bool _haveMaximumSoftkeyLabelLength;
        private volatile bool _haveminorversion;
        private volatile bool _haveMobileDeviceManufacturer;
        private volatile bool _haveMobileDeviceModel;
        private volatile bool _havemsdomversion;
        private volatile bool _haveNumberOfSoftkeys;
        private volatile bool _haveplatform;
        private volatile bool _havePreferredImageMime;
        private volatile bool _havePreferredRenderingMime;
        private volatile bool _havePreferredRenderingType;
        private volatile bool _havePreferredRequestEncoding;
        private volatile bool _havePreferredResponseEncoding;
        private volatile bool _haveRendersBreakBeforeWmlSelectAndInput;
        private volatile bool _haveRendersBreaksAfterHtmlLists;
        private volatile bool _haveRendersBreaksAfterWmlAnchor;
        private volatile bool _haveRendersBreaksAfterWmlInput;
        private volatile bool _haveRendersWmlDoAcceptsInline;
        private volatile bool _haveRendersWmlSelectsAsMenuCards;
        private volatile bool _haveRequiredMetaTagNameValue;
        private volatile bool _haveRequiresAttributeColonSubstitution;
        private volatile bool _haveRequiresContentTypeMetaTag;
        private volatile bool _haverequiresControlStateInSession;
        private volatile bool _haveRequiresDBCSCharacter;
        private volatile bool _haveRequiresHtmlAdaptiveErrorReporting;
        private volatile bool _haveRequiresLeadingPageBreak;
        private volatile bool _haveRequiresNoBreakInFormatting;
        private volatile bool _haveRequiresOutputOptimization;
        private volatile bool _haveRequiresPhoneNumbersAsPlainText;
        private volatile bool _haveRequiresSpecialViewStateEncoding;
        private volatile bool _haveRequiresUniqueFilePathSuffix;
        private volatile bool _haveRequiresUniqueHtmlCheckboxNames;
        private volatile bool _haveRequiresUniqueHtmlInputNames;
        private volatile bool _haveRequiresUrlEncodedPostfieldValues;
        private volatile bool _haveScreenBitDepth;
        private volatile bool _haveScreenCharactersHeight;
        private volatile bool _haveScreenCharactersWidth;
        private volatile bool _haveScreenPixelsHeight;
        private volatile bool _haveScreenPixelsWidth;
        private volatile bool _haveSupportsAccesskeyAttribute;
        private volatile bool _haveSupportsBodyColor;
        private volatile bool _haveSupportsBold;
        private volatile bool _haveSupportsCacheControlMetaTag;
        private volatile bool _haveSupportsCallback;
        private volatile bool _haveSupportsCss;
        private volatile bool _haveSupportsDivAlign;
        private volatile bool _haveSupportsDivNoWrap;
        private volatile bool _haveSupportsEmptyStringInCookieValue;
        private volatile bool _haveSupportsFontColor;
        private volatile bool _haveSupportsFontName;
        private volatile bool _haveSupportsFontSize;
        private volatile bool _haveSupportsImageSubmit;
        private volatile bool _haveSupportsIModeSymbols;
        private volatile bool _haveSupportsInputIStyle;
        private volatile bool _haveSupportsInputMode;
        private volatile bool _haveSupportsItalic;
        private volatile bool _haveSupportsJPhoneMultiMediaAttributes;
        private volatile bool _haveSupportsJPhoneSymbols;
        private volatile bool _haveSupportsMaintainScrollPositionOnPostback;
        private volatile bool _haveSupportsQueryStringInFormAction;
        private volatile bool _haveSupportsRedirectWithCookie;
        private volatile bool _haveSupportsSelectMultiple;
        private volatile bool _haveSupportsUncheck;
        private volatile bool _haveSupportsXmlHttp;
        private volatile bool _havetables;
        //private volatile bool _havetagwriter;
        private volatile bool _havetype;
        private volatile bool _havevbscript;
        private volatile bool _haveversion;
        private volatile bool _havew3cdomversion;
        private volatile bool _havewin16;
        private volatile bool _havewin32;
        #endregion

        #region Properties from Default.browser
        private volatile bool _activexcontrols;
        private volatile bool _aol;
        private volatile bool _backgroundsounds;
        private volatile bool _beta;
        private volatile string _browser;
        private volatile bool _canCombineFormsInDeck;
        private volatile bool _canInitiateVoiceCall;
        private volatile bool _canRenderAfterInputOrSelectElement;
        private volatile bool _canRenderEmptySelects;
        private volatile bool _canRenderInputAndSelectElementsTogether;
        private volatile bool _canRenderMixedSelects;
        private volatile bool _canRenderOneventAndPrevElementsTogether;
        private volatile bool _canRenderPostBackCards;
        private volatile bool _canRenderSetvarZeroWithMultiSelectionList;
        private volatile bool _canSendMail;
        private volatile bool _cdf;
        private volatile bool _cookies;
        private volatile bool _crawler;
        private volatile int _defaultSubmitButtonLimit;
        private volatile Version _ecmascriptversion;
        private volatile bool _frames;
        private volatile int _gatewayMajorVersion;
        private double _gatewayMinorVersion;
        private volatile string _gatewayVersion;
        private volatile bool _hidesRightAlignedMultiselectScrollbars;
        private string _htmlTextWriter;
        private volatile string _inputType;
        private volatile bool _isColor;
        private volatile bool _isMobileDevice;
        private volatile bool _javaapplets;
        private volatile bool _javascript;
        private volatile Version _jscriptversion;
        private volatile int _majorversion;
        private volatile int _maximumHrefLength;
        private volatile int _maximumRenderedPageSize;
        private volatile int _maximumSoftkeyLabelLength;
        private double _minorversion;
        private volatile string _mobileDeviceManufacturer;
        private volatile string _mobileDeviceModel;
        private volatile Version _msdomversion;
        private volatile int _numberOfSoftkeys;
        private volatile string _platform;
        private volatile string _preferredImageMime;
        private volatile string _preferredRenderingMime;
        private volatile string _preferredRenderingType;
        private volatile string _preferredRequestEncoding;
        private volatile string _preferredResponseEncoding;
        private volatile bool _rendersBreakBeforeWmlSelectAndInput;
        private volatile bool _rendersBreaksAfterHtmlLists;
        private volatile bool _rendersBreaksAfterWmlAnchor;
        private volatile bool _rendersBreaksAfterWmlInput;
        private volatile bool _rendersWmlDoAcceptsInline;
        private volatile bool _rendersWmlSelectsAsMenuCards;
        private volatile string _requiredMetaTagNameValue;
        private volatile bool _requiresAttributeColonSubstitution;
        private volatile bool _requiresContentTypeMetaTag;
        private volatile bool _requiresControlStateInSession;
        private volatile bool _requiresDBCSCharacter;
        private volatile bool _requiresHtmlAdaptiveErrorReporting;
        private volatile bool _requiresLeadingPageBreak;
        private volatile bool _requiresNoBreakInFormatting;
        private volatile bool _requiresOutputOptimization;
        private volatile bool _requiresPhoneNumbersAsPlainText;
        private volatile bool _requiresSpecialViewStateEncoding;
        private volatile bool _requiresUniqueFilePathSuffix;
        private volatile bool _requiresUniqueHtmlCheckboxNames;
        private volatile bool _requiresUniqueHtmlInputNames;
        private volatile bool _requiresUrlEncodedPostfieldValues;
        private volatile int _screenBitDepth;
        private volatile int _screenCharactersHeight;
        private volatile int _screenCharactersWidth;
        private volatile int _screenPixelsHeight;
        private volatile int _screenPixelsWidth;
        private volatile bool _supportsAccesskeyAttribute;
        private volatile bool _supportsBodyColor;
        private volatile bool _supportsBold;
        private volatile bool _supportsCacheControlMetaTag;
        private volatile bool _supportsCallback;
        private volatile bool _supportsCss;
        private volatile bool _supportsDivAlign;
        private volatile bool _supportsDivNoWrap;
        private volatile bool _supportsEmptyStringInCookieValue;
        private volatile bool _supportsFontColor;
        private volatile bool _supportsFontName;
        private volatile bool _supportsFontSize;
        private volatile bool _supportsImageSubmit;
        private volatile bool _supportsIModeSymbols;
        private volatile bool _supportsInputIStyle;
        private volatile bool _supportsInputMode;
        private volatile bool _supportsItalic;
        private volatile bool _supportsJPhoneMultiMediaAttributes;
        private volatile bool _supportsJPhoneSymbols;
        private volatile bool _supportsMaintainScrollPositionOnPostback;
        private volatile bool _supportsQueryStringInFormAction;
        private volatile bool _supportsRedirectWithCookie;
        private volatile bool _supportsSelectMultiple;
        private volatile bool _supportsUncheck;
        private volatile bool _supportsXmlHttp;
        private volatile bool _tables;
        private volatile string _type;
        private volatile bool _vbscript;
        private volatile string _version;
        private volatile Version _w3cdomversion;
        private volatile bool _win16;
        private volatile bool _win32;
        #endregion
        #endregion

        ///<summary>
        /// Creates a new instance of the HttpCapabilitiesBase class.
        /// </summary>
        /// <param name="headers"></param>
        public HttpCapabilitiesBase(NameValueCollection headers)
        {
            m_headers = headers;
            IDictionary dic = Capabilities;
        }

        #region Public Methods
        ///<summary>
        /// Gets the value of the specified browser capability. In C#, this property is the indexer for the class.
        /// </summary>
        /// <param name="key">The name of the browser capability to retrieve.</param>
        ///<returns>The browser capability with the specified key name.</returns>
        public virtual string this[string key]
        {
            get
            {
                if (m_capabilities.ContainsKey(key))
                    return (string)this.m_capabilities[key];
                else
                    return null;
            }
        }

        ///<summary>
        /// Used internally to get the defined capabilities of the browser.
        /// </summary>
        ///<returns>The defined capabilities of the browser</returns>
        public IDictionary Capabilities
        {
            get
            {
                if (m_capabilities == null)
                {
                    ResolveCapabilities();
                }
                return m_capabilities;
            }
        }

        ///<summary>
        /// Used internally to add an entry to the internal collection of browsers for which capabilities are recognized.
        /// </summary>
        /// <param name="browserName">The name of the browser to add.</param>
        public void AddBrowser(string browserName)
        {
            if (this._browsers == null)
            {
                lock (_staticLock)
                {
                    if (this._browsers == null)
                    {
                        this._browsers = new List<string>();
                    }
                }
            }
            this._browsers.Add(browserName.ToLower(CultureInfo.InvariantCulture));
        }

        ///<summary>
        /// Gets the version of the .NET Framework that is installed on the client.
        /// </summary>
        ///<returns>The common language runtime Version.</returns>
        public Version[] GetClrVersions()
        {
            string userAgent = m_headers["HTTP_USER_AGENT"] as string;
            if (string.IsNullOrEmpty(userAgent))
            {
                return null;
            }
            MatchCollection matchs = new Regex(@"\.NET CLR (?'clrVersion'[0-9\.]*)").Matches(userAgent);
            if (matchs.Count == 0)
            {
                return new Version[] { new Version(0, 0) };
            }
            ArrayList list = new ArrayList();
            foreach (Match match in matchs)
            {
                try
                {
                    Version version = new Version(match.Groups["clrVersion"].Value);
                    list.Add(version);
                    continue;
                }
                catch (FormatException)
                {
                    continue;
                }
            }
            list.Sort();
            return (Version[])list.ToArray(typeof(Version));
        }

        ///<summary>
        /// Gets a value indicating whether the client browser is the same as the specified browser.
        /// </summary>
        /// <param name="browserName">The specified browser.</param>
        ///<returns>true if the client browser is the same as the specified browser; otherwise, false. The default is false.</returns>
        public bool IsBrowser(string browserName)
        {
            if (!string.IsNullOrEmpty(browserName))
            {
                if (this._browsers == null)
                {
                    return false;
                }
                for (int i = 0; i < this._browsers.Count; i++)
                {
                    if (string.Equals(browserName, (string)this._browsers[i], StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region Private Methods
        private bool CapsParseBool(string capsKey)
        {
            bool flag = false;
            try
            {
                flag = bool.Parse(this[capsKey]);
            }
            catch /*(FormatException exception)*/
            {
                //throw this.BuildParseError(exception, capsKey);
            }
            return flag;
        }

        private bool CapsParseBoolDefault(string capsKey, bool defaultValue)
        {
            string str = this[capsKey];
            if (str == null)
            {
                return defaultValue;
            }
            try
            {
                return bool.Parse(str);
            }
            catch (FormatException)
            {
                return defaultValue;
            }
        }

        private void ResolveCapabilities()
        {
            //Create a new m_capabilitiies
            m_capabilities = new Dictionary<string, string>();

            //Get the server configuration
            ServerConfig cfg = ServerConfig.GetConfig();

            //Make sure the directory exists incase a customer has not deployed
            if (!Directory.Exists(cfg.BrowserDefinitions))
                return;

            //Get the userAgent string
            string userAgent = m_headers["HTTP_USER_AGENT"] as string;
            if (userAgent == null)
                return;

            //Create a browserID variable to track the browser
            string browserID = string.Empty;

            //Get the config information
            string tempName = null;

            //Load the combineed XmlDocument
            XmlDocument browserDoc = GetCombinedBrowserXmlDoc;
            if (browserDoc == null)
                return;

            try
            {
                //Load the default definition file
                XmlNode browserNode = browserDoc.SelectSingleNode("browsers");
                //Start with parentID of Default
                //browserID = "Default";
                if (browserNode != null)
                {
                    do
                    {
                        tempName = GetConfigInfo(userAgent, browserNode, m_capabilities, browserID);
                        if (tempName != null)
                        {
                            browserID = tempName;
                        }
                    } while (tempName != null);
                }
            }
            catch (Exception e)
            {
                string s = e.Message;
                if (System.Diagnostics.Debugger.IsAttached)
                    System.Diagnostics.Debugger.Break();
            }

            foreach (string key in m_capabilities.Keys)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", key, m_capabilities[key]));
            }
        }

        private XmlDocument GetCombinedBrowserXmlDoc
        {
            get
            {
                if (CombinedBrowserDocument != null)
                    return CombinedBrowserDocument;

                //Make sure the directory exists
                if (!Directory.Exists(ServerConfig.GetConfig().BrowserDefinitions))
                    return null;

                lock (CombinedBrowserDocumentSyncRoot)
                {
                    try
                    {
                        //First get all the data from the files
                        StringBuilder xml = new StringBuilder();
                        xml.Append("<browsers>");
                        string[] browserFiles = Directory.GetFiles(ServerConfig.GetConfig().BrowserDefinitions, "*.browser");
                        StreamReader sr;
                        string tempLine;
                        foreach (string browserFile in browserFiles)
                        {
                            sr = new StreamReader(browserFile);
                            tempLine = sr.ReadToEnd().Replace("<browsers>", "");
                            tempLine = tempLine.Replace("</browsers>", "");
                            xml.Append(tempLine);
                            //GetBrowserNodes(sr, xml);
                            sr.Close();
                        }
                        xml.Append("</browsers>");
                        //StreamWriter sw = new StreamWriter("\\windows\\Inetpub\\browsers.xml");
                        //sw.Write(xml.ToString());
                        //sw.Close();
                        CombinedBrowserDocument = new XmlDocument();
                        CombinedBrowserDocument.LoadXml(xml.ToString());
                        return CombinedBrowserDocument;
                    }
                    catch (Exception e)
                    {
                        string s = e.Message;
                        if (System.Diagnostics.Debugger.IsAttached)
                            System.Diagnostics.Debugger.Break();
                        CombinedBrowserDocument = null;
                        return null;
                    }
                }
            }
        }

        private string GetConfigInfo(string userAgent, XmlNode section, Dictionary<string, string> capabilities, string parentID)
        {
            XmlNodeList browsers;
            string browserID = null;
            XmlNode activeNode = null;
            Dictionary<string, string> captures = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(parentID))
            {
                // first look, so find the parent browser
                browsers = section.SelectNodes("browser[@id=\"Default\"]");
            }
            else
            {
                browsers = section.SelectNodes(string.Format("browser[@parentID='{0}']", parentID));
            }


            foreach (XmlNode node in browsers)
            {
                activeNode = node;
                bool nodeMatches = false;
                if (node.Attributes["id"].Value == "Default")
                {
                    browserID = node.Attributes["id"].Value;
                }
                else
                {
                    // get <identification><useragent match="???"> attribute
                    XmlNode idNode = node.SelectSingleNode("identification");
                    if (idNode != null)
                    {
                        XmlAttribute attrib;

                        //monitors if we should just do the next browsernode incase we hit a nonMatch item
                        bool nonMatch = false;

                        //Find the nonMatch userAgent Nodes
                        XmlNodeList agentNodes = idNode.SelectNodes("userAgent[@nonMatch]");
                        if (agentNodes.Count > 0)
                        {
                            foreach (XmlNode agentNode in agentNodes)
                            {
                                //Check the regex for nonMatch items.  If it's non match just skip.
                                attrib = null;
                                if (node != null)
                                {
                                    attrib = agentNode.Attributes["nonMatch"];
                                    if (attrib != null)
                                    {
                                        Match match = Regex.Match(userAgent, attrib.Value);
                                        if (match.Success)
                                        {
                                            nonMatch = true;
                                            break;//if we match a nonMatch in the useragent string then just continue looping the other browsers
                                        }
                                    }
                                }
                            }
                        }

                        //see if we hit a nonmatch
                        if (nonMatch)
                            continue;

                        //Now do the matching nodes
                        agentNodes = idNode.SelectNodes("userAgent[@match]");
                        if (agentNodes.Count > 0)
                        {
                            foreach (XmlNode agentNode in agentNodes)
                            {
                                attrib = agentNode.Attributes["match"];
                                if (attrib != null)
                                {
                                    Match match = Regex.Match(userAgent, attrib.Value);
                                    if (match.Success)
                                    {
                                        browserID = node.Attributes["id"].Value;
                                        nodeMatches = true;

                                        if (match.Groups.Count > 1)
                                        {
                                            // capture data in the agent value
                                            int start = attrib.Value.IndexOf("?'", 0);
                                            while (start > 0)
                                            {
                                                int end = attrib.Value.IndexOf("'", start + 2);

                                                string capName = attrib.Value.Substring(start + 2, end - (start + 2));

                                                if (captures.ContainsKey(capName))
                                                {
                                                    captures[capName] = match.Groups[capName].Value;
                                                }
                                                else
                                                {
                                                    captures.Add(capName, match.Groups[capName].Value);
                                                }
                                                start = attrib.Value.IndexOf("?'", end);
                                            }
                                        }
                                    }
                                }
                                //HACK The MME.browser file (Microsoft Mobile Explorer) seems to match the regex expresion for the second userAgent[@match] node on all 
                                //browsers because it includes Mozilla.  Everything from PIE, IE, Safari come back as MME.  So for MME we do only the first node.  I personally
                                //think the second node should be nonMatch and not match since Mozilla.browser has a nonMatch of MME.  These files are copied straight from
                                //2. .NET Framework in CONFIG (C:\Windows\Microsoft.NET\Framework\v2.0.50727\CONFIG\Browsers).
                                if (activeNode.Attributes["id"].Value == "MME") break;
                            }
                        }

                        if (nodeMatches) break;
                    }
                }
            }

            if (browserID == null)
                return null;

            // get captures (capabilities that have browser-supplied values)
            XmlNode captureNode = activeNode.SelectSingleNode("capture");
            if (captureNode != null)
            {
                XmlNodeList agentNodes = captureNode.SelectNodes("userAgent");

                // if it exists, regex against userAgent
                if (agentNodes != null)
                {
                    foreach (XmlNode agent in agentNodes)
                    {
                        // get match attrib
                        XmlAttribute attrib = agent.Attributes["match"];
                        if (attrib != null)
                        {
                            Match match = Regex.Match(userAgent, attrib.Value);

                            int start = attrib.Value.IndexOf("?'", 0);
                            while (start > 0)
                            {
                                int end = attrib.Value.IndexOf("'", start + 2);

                                string capName = attrib.Value.Substring(start + 2, end - (start + 2));

                                if (captures.ContainsKey(capName))
                                {
                                    captures[capName] = match.Groups[capName].Value;
                                }
                                else
                                {
                                    captures.Add(capName, match.Groups[capName].Value);
                                }
                                start = attrib.Value.IndexOf("?'", end);
                            }

                        }

                    }
                }
            }

            // get capabilities (config set abilities)
            XmlNode capsNode = activeNode.SelectSingleNode("capabilities");
            if (capsNode != null)
            {
                XmlNodeList caplist = capsNode.SelectNodes("capability");

                foreach (XmlNode cap in caplist)
                {
                    string name = cap.Attributes["name"].Value;

                    //if (name == "version")
                    //{
                    //    if (System.Diagnostics.Debugger.IsAttached)
                    //        System.Diagnostics.Debugger.Break();
                    //}

                    string value = cap.Attributes["value"].Value;
                    // check for capture substitution
                    while (value.IndexOf('$') != -1)
                    {
                        int replaceStart = value.IndexOf("${");
                        if (replaceStart >= 0)
                        {
                            string captureName = value.Substring(replaceStart + 2, value.IndexOf('}') - (replaceStart + 2));

                            // it's possible to have a replacement that doesn't exist in our captures list (Gecko is an example)
                            if (captures.ContainsKey(captureName))
                            {
                                value = value.Replace(string.Format("${{{0}}}", captureName), captures[captureName]);
                            }
                            else
                            {
                                value = null;
                                break;
                            }
                        }
                    }
                    try
                    {
                        if (value != null)
                        {
                            if (capabilities.ContainsKey(name))
                            {
                                capabilities[name] = value;
                            }
                            else
                            {
                                capabilities.Add(name, value);
                            }
                        }
                    }
                    catch/* (Exception ex)*/
                    {
                    }
                }
            }


            return browserID;
        }

        private Exception BuildParseError(Exception e, string capsKey)
        {
            ConfigurationErrorsException exception = new ConfigurationErrorsException(Resources.Invalid_string_from_browser_caps, e);
            HttpUnhandledException exception2 = new HttpUnhandledException(exception.Message,exception);
            //TODO Need to implemetn exception formatter exception2.SetFormatter(new UseLastUnhandledErrorFormatter(exception));
            return exception2;
        }
        #endregion

        #region Properties
        ///<summary>Gets a value indicating whether the browser supports ActiveX controls.</summary>
        ///<returns>true if the browser supports ActiveX controls; otherwise, false. The default is false.</returns>
        public bool ActiveXControls
        {
            get
            {
                if (!this._haveactivexcontrols)
                {
                    this._activexcontrols = this.CapsParseBool("activexcontrols");
                    this._haveactivexcontrols = true;
                }
                return this._activexcontrols;
            }
        }

        ///<summary>Gets a value indicating whether the client is an America Online (AOL) browser.</summary>
        ///<returns>true if the browser is an AOL browser; otherwise, false. The default is false.</returns>
        public bool AOL
        {
            get
            {
                if (!this._haveaol)
                {
                    this._aol = this.CapsParseBool("aol");
                    this._haveaol = true;
                }
                return this._aol;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports playing background sounds using the &lt;bgsounds&gt; HTML element.</summary>
        ///<returns>true if the browser supports playing background sounds; otherwise, false. The default is false.</returns>
        public bool BackgroundSounds
        {
            get
            {
                if (!this._havebackgroundsounds)
                {
                    this._backgroundsounds = this.CapsParseBool("backgroundsounds");
                    this._havebackgroundsounds = true;
                }
                return this._backgroundsounds;
            }
        }

        ///<summary>Gets a value indicating whether the browser is a beta version.</summary>
        ///<returns>true if the browser is a beta version; otherwise, false. The default is false.</returns>
        public bool Beta
        {
            get
            {
                if (!this._havebeta)
                {
                    this._beta = this.CapsParseBool("beta");
                    this._havebeta = true;
                }
                return this._beta;
            }
        }

        ///<summary>Gets the browser string (if any) that was sent by the browser in the User-Agent request header.</summary>
        ///<returns>The contents of the User-Agent request header sent by the browser.</returns>
        public string Browser
        {
            get
            {
                if (!this._havebrowser)
                {
                    this._browser = this["browser"];
                    this._havebrowser = true;
                }
                return this._browser;
            }
        }

        ///<summary>Gets an <see cref="T:System.Collections.ArrayList"></see> of the browsers in the <see cref="P:System.Web.Configuration.HttpCapabilitiesBase.Capabilities"></see> dictionary.</summary>
        ///<returns>An <see cref="T:System.Collections.ArrayList"></see> of the browsers in the <see cref="P:System.Web.Configuration.HttpCapabilitiesBase.Capabilities"></see> dictionary.</returns>
        public ArrayList Browsers
        {
            get
            {
                return new ArrayList(this._browsers.ToArray());
            }
        }

        ///<summary>Gets a value indicating whether the browser supports decks that contain multiple forms, such as separate cards.</summary>
        ///<returns>true if the browser supports decks that contain multiple forms; otherwise, false. The default is true.</returns>
        public virtual bool CanCombineFormsInDeck
        {
            get
            {
                if (!this._haveCanCombineFormsInDeck)
                {
                    this._canCombineFormsInDeck = this.CapsParseBoolDefault("canCombineFormsInDeck", true);
                    this._haveCanCombineFormsInDeck = true;
                }
                return this._canCombineFormsInDeck;
            }
        }

        ///<summary>Gets a value indicating whether the browser device is capable of initiating a voice call.</summary>
        ///<returns>true if the browser device is capable of initiating a voice call; otherwise, false. The default is false.</returns>
        public virtual bool CanInitiateVoiceCall
        {
            get
            {
                if (!this._haveCanInitiateVoiceCall)
                {
                    this._canInitiateVoiceCall = this.CapsParseBoolDefault("canInitiateVoiceCall", false);
                    this._haveCanInitiateVoiceCall = true;
                }
                return this._canInitiateVoiceCall;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports page content following WML &lt;select&gt; or &lt;input&gt; elements.</summary>
        ///<returns>true if the browser supports page content following HTML &lt;select&gt; or &lt;input&gt; elements; otherwise, false. The default is true.</returns>
        public virtual bool CanRenderAfterInputOrSelectElement
        {
            get
            {
                if (!this._haveCanRenderAfterInputOrSelectElement)
                {
                    this._canRenderAfterInputOrSelectElement = this.CapsParseBoolDefault("canRenderAfterInputOrSelectElement", true);
                    this._haveCanRenderAfterInputOrSelectElement = true;
                }
                return this._canRenderAfterInputOrSelectElement;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports empty HTML &lt;select&gt; elements.</summary>
        ///<returns>true if the browser supports empty HTML &lt;select&gt; elements; otherwise, false. The default is true.</returns>
        public virtual bool CanRenderEmptySelects
        {
            get
            {
                if (!this._haveCanRenderEmptySelects)
                {
                    this._canRenderEmptySelects = this.CapsParseBoolDefault("canRenderEmptySelects", true);
                    this._haveCanRenderEmptySelects = true;
                }
                return this._canRenderEmptySelects;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports WML INPUT and SELECT elements together on the same card.</summary>
        ///<returns>true if the browser supports WML &lt;input&gt; and &lt;select&gt; elements together; otherwise, false. The default is false.</returns>
        public virtual bool CanRenderInputAndSelectElementsTogether
        {
            get
            {
                if (!this._haveCanRenderInputAndSelectElementsTogether)
                {
                    this._canRenderInputAndSelectElementsTogether = this.CapsParseBoolDefault("canRenderInputAndSelectElementsTogether", true);
                    this._haveCanRenderInputAndSelectElementsTogether = true;
                }
                return this._canRenderInputAndSelectElementsTogether;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports WML &lt;option&gt; elements that specify both onpick and value attributes.</summary>
        ///<returns>true if the browser supports WML &lt;option&gt; elements that specify both onpick and value attributes; otherwise, false. The default is true.</returns>
        public virtual bool CanRenderMixedSelects
        {
            get
            {
                if (!this._haveCanRenderMixedSelects)
                {
                    this._canRenderMixedSelects = this.CapsParseBoolDefault("canRenderMixedSelects", true);
                    this._haveCanRenderMixedSelects = true;
                }
                return this._canRenderMixedSelects;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports WML &lt;onevent&gt; and &lt;prev&gt; elements that coexist within the same WML card.</summary>
        ///<returns>true if the browser supports WML &lt;onevent&gt; and &lt;prev&gt; elements that coexist within the same WML card; otherwise, false. The default is true.</returns>
        public virtual bool CanRenderOneventAndPrevElementsTogether
        {
            get
            {
                if (!this._haveCanRenderOneventAndPrevElementsTogether)
                {
                    this._canRenderOneventAndPrevElementsTogether = this.CapsParseBoolDefault("canRenderOneventAndPrevElementsTogether", true);
                    this._haveCanRenderOneventAndPrevElementsTogether = true;
                }
                return this._canRenderOneventAndPrevElementsTogether;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports WML cards for postback.</summary>
        ///<returns>true if the browser supports WML cards for postback; otherwise, false. The default is true.</returns>
        public virtual bool CanRenderPostBackCards
        {
            get
            {
                if (!this._haveCanRenderPostBackCards)
                {
                    this._canRenderPostBackCards = this.CapsParseBoolDefault("canRenderPostBackCards", true);
                    this._haveCanRenderPostBackCards = true;
                }
                return this._canRenderPostBackCards;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports WML &lt;setvar&gt; elements with a value attribute of 0.</summary>
        ///<returns>true if the browser supports WML &lt;setvar&gt; elements with a value attribute of 0; otherwise, false. The default is true.</returns>
        public virtual bool CanRenderSetvarZeroWithMultiSelectionList
        {
            get
            {
                if (!this._haveCanRenderSetvarZeroWithMultiSelectionList)
                {
                    this._canRenderSetvarZeroWithMultiSelectionList = this.CapsParseBoolDefault("canRenderSetvarZeroWithMultiSelectionList", true);
                    this._haveCanRenderSetvarZeroWithMultiSelectionList = true;
                }
                return this._canRenderSetvarZeroWithMultiSelectionList;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports sending e-mail by using the HTML &lt;mailto&gt; element for displaying electronic addresses.</summary>
        ///<returns>true if the browser supports sending e-mail by using the HTML &lt;mailto&gt; element for displaying electronic addresses; otherwise, false. The default is true.</returns>
        public virtual bool CanSendMail
        {
            get
            {
                if (!this._haveCanSendMail)
                {
                    this._canSendMail = this.CapsParseBoolDefault("canSendMail", true);
                    this._haveCanSendMail = true;
                }
                return this._canSendMail;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports Channel Definition Format (CDF) for webcasting.</summary>
        ///<returns>true if the browser supports CDF; otherwise, false. The default is false.</returns>
        public bool CDF
        {
            get
            {
                if (!this._havecdf)
                {
                    this._cdf = this.CapsParseBool("cdf");
                    this._havecdf = true;
                }
                return this._cdf;
            }
        }

        ///<summary>Gets the version of the .NET Framework that is installed on the client.</summary>
        ///<returns>The common language runtime <see cref="T:System.Version"></see>.</returns>
        public Version ClrVersion
        {
            get
            {
                Version[] clrVersions = this.GetClrVersions();
                if (clrVersions != null)
                {
                    return clrVersions[clrVersions.Length - 1];
                }
                return null;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports cookies.</summary>
        ///<returns>true if the browser supports cookies; otherwise, false. The default is false.</returns>
        public bool Cookies
        {
            get
            {
                if (!this._havecookies)
                {
                    this._cookies = this.CapsParseBool("cookies");
                    this._havecookies = true;
                }
                return this._cookies;
            }
        }

        ///<summary>Gets a value indicating whether the browser is a search engine Web crawler.</summary>
        ///<returns>true if the browser is a search engine; otherwise, false. The default is false.</returns>
        public bool Crawler
        {
            get
            {
                if (!this._havecrawler)
                {
                    this._crawler = this.CapsParseBool("crawler");
                    this._havecrawler = true;
                }
                return this._crawler;
            }
        }

        ///<summary>Returns the maximum number of Submit buttons that are allowed for a form.</summary>
        ///<returns>The maximum number of Submit buttons that are allowed for a form.</returns>
        public virtual int DefaultSubmitButtonLimit
        {
            get
            {
                if (!this._haveDefaultSubmitButtonLimit)
                {
                    string str = this["defaultSubmitButtonLimit"];
                    this._defaultSubmitButtonLimit = (str != null) ? Convert.ToInt32(this["defaultSubmitButtonLimit"], CultureInfo.InvariantCulture) : 1;
                    this._haveDefaultSubmitButtonLimit = true;
                }
                return this._defaultSubmitButtonLimit;
            }
        }

        ///<summary>Gets the version number of ECMAScript that the browser supports.</summary>
        ///<returns>The version number of ECMAScript that the browser supports.</returns>
        public Version EcmaScriptVersion
        {
            get
            {
                if (!this._haveecmascriptversion)
                {
                    this._ecmascriptversion = new Version(this["ecmascriptversion"]);
                    this._haveecmascriptversion = true;
                }
                return this._ecmascriptversion;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports HTML frames.</summary>
        ///<returns>true if the browser supports frames; otherwise, false. The default is false.</returns>
        public bool Frames
        {
            get
            {
                if (!this._haveframes)
                {
                    this._frames = this.CapsParseBool("frames");
                    this._haveframes = true;
                }
                return this._frames;
            }
        }

        ///<summary>Gets the major version number of the wireless gateway used to access the server, if known. </summary>
        ///<returns>The major version number of the wireless gateway used to access the server, if known. The default is 0.</returns>
        ///<exception cref="T:System.Web.HttpUnhandledException">The major version number of the wireless gateway cannot be parsed.</exception>
        public virtual int GatewayMajorVersion
        {
            get
            {
                if (!this._haveGatewayMajorVersion)
                {
                    this._gatewayMajorVersion = Convert.ToInt32(this["gatewayMajorVersion"], CultureInfo.InvariantCulture);
                    this._haveGatewayMajorVersion = true;
                }
                return this._gatewayMajorVersion;
            }
        }

        ///<summary>Gets the minor version number of the wireless gateway used to access the server, if known. </summary>
        ///<returns>The minor version number of the wireless gateway used to access the server, if known. The default is 0.</returns>
        ///<exception cref="T:System.Web.HttpUnhandledException">The minor version number of the wireless gateway cannot be parsed.</exception>
        public virtual double GatewayMinorVersion
        {
            get
            {
                if (!this._haveGatewayMinorVersion)
                {
                    this._gatewayMinorVersion = double.Parse(this["gatewayMinorVersion"], NumberStyles.Float, (IFormatProvider)NumberFormatInfo.InvariantInfo);
                    this._haveGatewayMinorVersion = true;
                }
                return this._gatewayMinorVersion;
            }
        }

        ///<summary>Gets the version of the wireless gateway used to access the server, if known.</summary>
        ///<returns>The version number of the wireless gateway used to access the server, if known. The default is None.</returns>
        public virtual string GatewayVersion
        {
            get
            {
                if (!this._haveGatewayVersion)
                {
                    this._gatewayVersion = this["gatewayVersion"];
                    this._haveGatewayVersion = true;
                }
                return this._gatewayVersion;
            }
        }

        ///<summary>Gets a value indicating whether the browser has a dedicated Back button.</summary>
        ///<returns>true if the browser has a dedicated Back button; otherwise, false. The default is true.</returns>
        public virtual bool HasBackButton
        {
            get
            {
                if (!this._haveHasBackButton)
                {
                    this._hasBackButton = this.CapsParseBoolDefault("hasBackButton", true);
                    this._haveHasBackButton = true;
                }
                return this._hasBackButton;
            }
        }

        ///<summary>Gets a value indicating whether the scrollbar of an HTML &lt;select multiple&gt; element with an align attribute value of right is obscured upon rendering.</summary>
        ///<returns>true if the scrollbar of an HTML &lt;select multiple&gt; element with an align attribute value of right is obscured upon rendering; otherwise, false. The default is false.</returns>
        public virtual bool HidesRightAlignedMultiselectScrollbars
        {
            get
            {
                if (!this._haveHidesRightAlignedMultiselectScrollbars)
                {
                    this._hidesRightAlignedMultiselectScrollbars = this.CapsParseBoolDefault("hidesRightAlignedMultiselectScrollbars", false);
                    this._haveHidesRightAlignedMultiselectScrollbars = true;
                }
                return this._hidesRightAlignedMultiselectScrollbars;
            }
        }

        ///<summary>Gets or sets the fully qualified class name of the <see cref="T:System.Web.UI.HtmlTextWriter"></see> to use.</summary>
        ///<returns>The fully qualified class name of the <see cref="T:System.Web.UI.HtmlTextWriter"></see> to use.</returns>
        public string HtmlTextWriter
        {
            get
            {
                return this._htmlTextWriter;
            }
            set
            {
                this._htmlTextWriter = value;
            }
        }

        ///<summary>Gets the internal identifier of the browser as specified in the browser definition file.</summary>
        ///<returns>Internal identifier of the browser as specified in the browser definition file.</returns>
        public string Id
        {
            get
            {
                if (this._browsers != null)
                {
                    return (string)this._browsers[this._browsers.Count - 1];
                }
                return string.Empty;
            }
        }

        ///<summary>Returns the type of input supported by browser.</summary>
        ///<returns>The type of input supported by browser. The default is telephoneKeypad.</returns>
        public virtual string InputType
        {
            get
            {
                if (!this._haveInputType)
                {
                    this._inputType = this["inputType"];
                    this._haveInputType = true;
                }
                return this._inputType;
            }
        }

        ///<summary>Gets a value indicating whether the browser has a color display.</summary>
        ///<returns>true if the browser has a color display; otherwise, false. The default is false.</returns>
        public virtual bool IsColor
        {
            get
            {
                if (!this._haveIsColor)
                {
                    if (this["isColor"] == null)
                    {
                        this._isColor = false;
                    }
                    else
                    {
                        this._isColor = Convert.ToBoolean(this["isColor"], CultureInfo.InvariantCulture);
                    }
                    this._haveIsColor = true;
                }
                return this._isColor;
            }
        }

        ///<summary>Gets a value indicating whether the browser is a recognized mobile device.</summary>
        ///<returns>true if the browser is a recognized mobile device; otherwise, false. The default is true.</returns>
        public virtual bool IsMobileDevice
        {
            get
            {
                if (!this._haveIsMobileDevice)
                {
                    this._isMobileDevice = this.CapsParseBoolDefault("isMobileDevice", false);
                    this._haveIsMobileDevice = true;
                }
                return this._isMobileDevice;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports Java.</summary>
        ///<returns>true if the browser supports Java; otherwise, false. The default is false.</returns>
        public bool JavaApplets
        {
            get
            {
                if (!this._havejavaapplets)
                {
                    this._javaapplets = this.CapsParseBool("javaapplets");
                    this._havejavaapplets = true;
                }
                return this._javaapplets;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports JavaScript.</summary>
        ///<returns>true if the browser supports JavaScript; otherwise, false. The default is false.</returns>
        [Obsolete("The recommended alternative is the EcmaScriptVersion property. A Major version value greater than or equal to 1 implies JavaScript support. http://go.microsoft.com/fwlink/?linkid=14202")]
        public bool JavaScript
        {
            get
            {
                if (!this._havejavascript)
                {
                    this._javascript = this.CapsParseBool("javascript");
                    this._havejavascript = true;
                }
                return this._javascript;
            }
        }

        ///<summary>Gets the Jscript version that the browser supports.</summary>
        ///<returns>The <see cref="T:System.Version"></see> of Jscript that the browser supports.</returns>
        public Version JScriptVersion
        {
            get
            {
                if (!this._havejscriptversion)
                {
                    this._jscriptversion = new Version(this["jscriptversion"]);
                    this._havejscriptversion = true;
                }
                return this._jscriptversion;
            }
        }

        ///<summary>Gets the major (integer) version number of the browser.</summary>
        ///<returns>The major version number of the browser.</returns>
        ///<exception cref="T:System.Web.HttpUnhandledException">The header value is not valid.</exception>
        public int MajorVersion
        {
            get
            {
                if (!this._havemajorversion)
                {
                    try
                    {
                        this._majorversion = int.Parse(this["majorversion"], CultureInfo.InvariantCulture);
                        this._havemajorversion = true;
                    }
                    catch (FormatException exception)
                    {
                        throw this.BuildParseError(exception, "majorversion");
                    }
                }
                return this._majorversion;
            }
        }

        ///<summary>Gets the maximum length in characters for the href attribute of an HTML &lt;a&gt; (anchor) element.</summary>
        ///<returns>The maximum length in characters for the href attribute of an HTML &lt;a&gt; (anchor) element.</returns>
        public virtual int MaximumHrefLength
        {
            get
            {
                if (!this._haveMaximumHrefLength)
                {
                    this._maximumHrefLength = Convert.ToInt32(this["maximumHrefLength"], CultureInfo.InvariantCulture);
                    this._haveMaximumHrefLength = true;
                }
                return this._maximumHrefLength;
            }
        }

        ///<summary>Gets the maximum length of the page, in bytes, which the browser can display. </summary>
        ///<returns>The maximum length of the page, in bytes, which the browser can display. The default is 2000.</returns>
        public virtual int MaximumRenderedPageSize
        {
            get
            {
                if (!this._haveMaximumRenderedPageSize)
                {
                    this._maximumRenderedPageSize = Convert.ToInt32(this["maximumRenderedPageSize"], CultureInfo.InvariantCulture);
                    this._haveMaximumRenderedPageSize = true;
                }
                return this._maximumRenderedPageSize;
            }
        }

        ///<summary>Returns the maximum length of the text that a soft-key label can display.</summary>
        ///<returns>The maximum length of the text that a soft-key label can display. The default is 5.</returns>
        public virtual int MaximumSoftkeyLabelLength
        {
            get
            {
                if (!this._haveMaximumSoftkeyLabelLength)
                {
                    this._maximumSoftkeyLabelLength = Convert.ToInt32(this["maximumSoftkeyLabelLength"], CultureInfo.InvariantCulture);
                    this._haveMaximumSoftkeyLabelLength = true;
                }
                return this._maximumSoftkeyLabelLength;
            }
        }

        ///<summary>Gets the minor (that is, decimal) version number of the browser.</summary>
        ///<returns>The minor version number of the browser.</returns>
        ///<exception cref="T:System.Web.HttpUnhandledException">The header value is not valid.</exception>
        public double MinorVersion
        {
            get
            {
                if (!this._haveminorversion)
                {
                    lock (_staticLock)
                    {
                        if (!this._haveminorversion)
                        {
                            try
                            {
                                this._minorversion = double.Parse(this["minorversion"], NumberStyles.Float, (IFormatProvider)NumberFormatInfo.InvariantInfo);
                                this._haveminorversion = true;
                            }
                            catch (FormatException exception)
                            {
                                string str = this["minorversion"];
                                int index = str.IndexOf('.');
                                if (index != -1)
                                {
                                    int length = str.IndexOf('.', index + 1);
                                    if (length != -1)
                                    {
                                        try
                                        {
                                            this._minorversion = double.Parse(str.Substring(0, length), NumberStyles.Float, (IFormatProvider)NumberFormatInfo.InvariantInfo);
                                            this._haveminorversion = true;
                                        }
                                        catch (FormatException)
                                        {
                                        }
                                    }
                                }
                                if (!this._haveminorversion)
                                {
                                    throw this.BuildParseError(exception, "minorversion");
                                }
                            }
                        }
                    }
                }
                return this._minorversion;
            }
        }

        ///<summary>Gets the minor (decimal) version number of the browser as a string.</summary>
        ///<returns>The minor version number of the browser.</returns>
        public string MinorVersionString
        {
            get
            {
                return this["minorversion"];
            }
        }

        ///<summary>Returns the name of the manufacturer of a mobile device, if known.</summary>
        ///<returns>The name of the manufacturer of a mobile device, if known. The default is Unknown.</returns>
        public virtual string MobileDeviceManufacturer
        {
            get
            {
                if (!this._haveMobileDeviceManufacturer)
                {
                    this._mobileDeviceManufacturer = this["mobileDeviceManufacturer"];
                    this._haveMobileDeviceManufacturer = true;
                }
                return this._mobileDeviceManufacturer;
            }
        }

        ///<summary>Gets the model name of a mobile device, if known.</summary>
        ///<returns>The model name of a mobile device, if known. The default is Unknown.</returns>
        public virtual string MobileDeviceModel
        {
            get
            {
                if (!this._haveMobileDeviceModel)
                {
                    this._mobileDeviceModel = this["mobileDeviceModel"];
                    this._haveMobileDeviceModel = true;
                }
                return this._mobileDeviceModel;
            }
        }

        ///<summary>Gets the version of Microsoft HTML (MSHTML) Document Object Model (DOM) that the browser supports.</summary>
        ///<returns>The number of the MSHTML DOM version that the browser supports.</returns>
        public Version MSDomVersion
        {
            get
            {
                if (!this._havemsdomversion)
                {
                    this._msdomversion = new Version(this["msdomversion"]);
                    this._havemsdomversion = true;
                }
                return this._msdomversion;
            }
        }

        ///<summary>Returns the number of soft keys on a mobile device.</summary>
        ///<returns>The number of soft keys supported on a mobile device. The default is 0.</returns>
        public virtual int NumberOfSoftkeys
        {
            get
            {
                if (!this._haveNumberOfSoftkeys)
                {
                    this._numberOfSoftkeys = Convert.ToInt32(this["numberOfSoftkeys"], CultureInfo.InvariantCulture);
                    this._haveNumberOfSoftkeys = true;
                }
                return this._numberOfSoftkeys;
            }
        }

        ///<summary>Gets the name of the platform that the client uses, if it is known.</summary>
        ///<returns>The operating system that the client uses, if it is known, otherwise the value is set to Unknown.</returns>
        public string Platform
        {
            get
            {
                if (!this._haveplatform)
                {
                    this._platform = this["platform"];
                    this._haveplatform = true;
                }
                return this._platform;
            }
        }

        ///<summary>Returns the MIME type of the type of image content typically preferred by the browser.</summary>
        ///<returns>The MIME type of the type of image content typically preferred by the browser. The default is image/gif.</returns>
        public virtual string PreferredImageMime
        {
            get
            {
                if (!this._havePreferredImageMime)
                {
                    this._preferredImageMime = this["preferredImageMime"];
                    this._havePreferredImageMime = true;
                }
                return this._preferredImageMime;
            }
        }

        ///<summary>Returns the MIME type of the type of content typically preferred by the browser.</summary>
        ///<returns>The MIME type of the type of content typically preferred by the browser. The default is text/html.</returns>
        public virtual string PreferredRenderingMime
        {
            get
            {
                if (!this._havePreferredRenderingMime)
                {
                    this._preferredRenderingMime = this["preferredRenderingMime"];
                    this._havePreferredRenderingMime = true;
                }
                return this._preferredRenderingMime;
            }
        }

        ///<summary>Gets the general name for the type of content that the browser prefers.</summary>
        ///<returns>html32 or chtml10. The default is html32.</returns>
        public virtual string PreferredRenderingType
        {
            get
            {
                if (!this._havePreferredRenderingType)
                {
                    this._preferredRenderingType = this["preferredRenderingType"];
                    this._havePreferredRenderingType = true;
                }
                return this._preferredRenderingType;
            }
        }

        ///<summary>Gets the request encoding preferred by the browser.</summary>
        ///<returns>The request encoding preferred by the browser.</returns>
        public virtual string PreferredRequestEncoding
        {
            get
            {
                if (!this._havePreferredRequestEncoding)
                {
                    this._preferredRequestEncoding = this["preferredRequestEncoding"];
                    this._havePreferredRequestEncoding = true;
                }
                return this._preferredRequestEncoding;
            }
        }

        ///<summary>Gets the response encoding preferred by the browser.</summary>
        ///<returns>The response encoding preferred by the browser.</returns>
        public virtual string PreferredResponseEncoding
        {
            get
            {
                if (!this._havePreferredResponseEncoding)
                {
                    this._preferredResponseEncoding = this["preferredResponseEncoding"];
                    this._havePreferredResponseEncoding = true;
                }
                return this._preferredResponseEncoding;
            }
        }

        ///<summary>Gets a value indicating whether the browser renders a line break before &lt;select&gt; or &lt;input&gt; elements.</summary>
        ///<returns>true if the browser renders a line break before &lt;select&gt; or &lt;input&gt; elements; otherwise, false. The default is false.</returns>
        public virtual bool RendersBreakBeforeWmlSelectAndInput
        {
            get
            {
                if (!this._haveRendersBreakBeforeWmlSelectAndInput)
                {
                    this._rendersBreakBeforeWmlSelectAndInput = this.CapsParseBoolDefault("rendersBreakBeforeWmlSelectAndInput", false);
                    this._haveRendersBreakBeforeWmlSelectAndInput = true;
                }
                return this._rendersBreakBeforeWmlSelectAndInput;
            }
        }

        ///<summary>Gets a value indicating whether the browser renders a line break after list-item elements.</summary>
        ///<returns>true if the browser renders a line break after list-item elements; otherwise, false. The default is true.</returns>
        public virtual bool RendersBreaksAfterHtmlLists
        {
            get
            {
                if (!this._haveRendersBreaksAfterHtmlLists)
                {
                    this._rendersBreaksAfterHtmlLists = this.CapsParseBoolDefault("rendersBreaksAfterHtmlLists", true);
                    this._haveRendersBreaksAfterHtmlLists = true;
                }
                return this._rendersBreaksAfterHtmlLists;
            }
        }

        ///<summary>Gets a value indicating whether the browser renders a line break after a stand-alone HTML &lt;a&gt; (anchor) element.</summary>
        ///<returns>true if the browser renders a line break after a stand-alone HTML &lt;a&gt; (anchor) element; otherwise, false. The default is false.</returns>
        public virtual bool RendersBreaksAfterWmlAnchor
        {
            get
            {
                if (!this._haveRendersBreaksAfterWmlAnchor)
                {
                    this._rendersBreaksAfterWmlAnchor = this.CapsParseBoolDefault("rendersBreaksAfterWmlAnchor", true);
                    this._haveRendersBreaksAfterWmlAnchor = true;
                }
                return this._rendersBreaksAfterWmlAnchor;
            }
        }

        ///<summary>Gets a value indicating whether the browser renders a line break after an HTML &lt;input&gt; element.</summary>
        ///<returns>true if the browser renders a line break after an HTML &lt;input&gt; element; otherwise, false. The default is false.</returns>
        public virtual bool RendersBreaksAfterWmlInput
        {
            get
            {
                if (!this._haveRendersBreaksAfterWmlInput)
                {
                    this._rendersBreaksAfterWmlInput = this.CapsParseBoolDefault("rendersBreaksAfterWmlInput", true);
                    this._haveRendersBreaksAfterWmlInput = true;
                }
                return this._rendersBreaksAfterWmlInput;
            }
        }

        ///<summary>Gets a value indicating whether the mobile-device browser renders a WML do-based form accept construct as an inline button rather than as a soft key.</summary>
        ///<returns>true if the mobile-device browser renders a WML do-based form-accept construct as an inline button; otherwise, false. The default is true.</returns>
        public virtual bool RendersWmlDoAcceptsInline
        {
            get
            {
                if (!this._haveRendersWmlDoAcceptsInline)
                {
                    this._rendersWmlDoAcceptsInline = this.CapsParseBoolDefault("rendersWmlDoAcceptsInline", true);
                    this._haveRendersWmlDoAcceptsInline = true;
                }
                return this._rendersWmlDoAcceptsInline;
            }
        }

        ///<summary>Gets a value indicating whether the browser renders WML &lt;select&gt; elements as menu cards, rather than as a combo box.</summary>
        ///<returns>true if the browser renders WML &lt;select&gt; elements as menu cards; otherwise, false. The default is false.</returns>
        public virtual bool RendersWmlSelectsAsMenuCards
        {
            get
            {
                if (!this._haveRendersWmlSelectsAsMenuCards)
                {
                    this._rendersWmlSelectsAsMenuCards = this.CapsParseBoolDefault("rendersWmlSelectsAsMenuCards", false);
                    this._haveRendersWmlSelectsAsMenuCards = true;
                }
                return this._rendersWmlSelectsAsMenuCards;
            }
        }

        ///<summary>Used internally to produce a meta-tag required by some browsers.</summary>
        ///<returns>A meta-tag required by some browsers.</returns>
        public virtual string RequiredMetaTagNameValue
        {
            get
            {
                if (!this._haveRequiredMetaTagNameValue)
                {
                    string str = this["requiredMetaTagNameValue"];
                    if (!string.IsNullOrEmpty(str))
                    {
                        this._requiredMetaTagNameValue = str;
                    }
                    else
                    {
                        this._requiredMetaTagNameValue = null;
                    }
                    this._haveRequiredMetaTagNameValue = true;
                }
                return this._requiredMetaTagNameValue;
            }
        }

        ///<summary>Gets a value indicating whether the browser requires colons in element attribute values to be substituted with a different character.</summary>
        ///<returns>true if the browser requires colons in element attribute values to be substituted with a different character; otherwise, false. The default is false.</returns>
        public virtual bool RequiresAttributeColonSubstitution
        {
            get
            {
                if (!this._haveRequiresAttributeColonSubstitution)
                {
                    this._requiresAttributeColonSubstitution = this.CapsParseBoolDefault("requiresAttributeColonSubstitution", false);
                    this._haveRequiresAttributeColonSubstitution = true;
                }
                return this._requiresAttributeColonSubstitution;
            }
        }

        ///<summary>Gets a value indicating whether the browser requires an HTML &lt;meta&gt; element for which the content-type attribute is specified.</summary>
        ///<returns>true if the browser requires an HTML &lt;meta&gt; element for which the content-type attribute is specified; otherwise, false. The default is false.</returns>
        public virtual bool RequiresContentTypeMetaTag
        {
            get
            {
                if (!this._haveRequiresContentTypeMetaTag)
                {
                    this._requiresContentTypeMetaTag = this.CapsParseBoolDefault("requiresContentTypeMetaTag", false);
                    this._haveRequiresContentTypeMetaTag = true;
                }
                return this._requiresContentTypeMetaTag;
            }
        }

        ///<summary>Gets a value indicating whether the browser requires control state to be maintained in sessions.</summary>
        ///<returns>true if the browser requires control state to be maintained in sessions; otherwise, false. The default is false.</returns>
        public bool RequiresControlStateInSession
        {
            get
            {
                if (!this._haverequiresControlStateInSession)
                {
                    if (this["requiresControlStateInSession"] != null)
                    {
                        this._requiresControlStateInSession = this.CapsParseBoolDefault("requiresControlStateInSession", false);
                    }
                    this._haverequiresControlStateInSession = true;
                }
                return this._requiresControlStateInSession;
            }
        }

        ///<summary>Gets a value indicating whether the browser requires a double-byte character set.</summary>
        ///<returns>true if the browser requires a double-byte character set; otherwise, false. The default is false.</returns>
        public virtual bool RequiresDBCSCharacter
        {
            get
            {
                if (!this._haveRequiresDBCSCharacter)
                {
                    this._requiresDBCSCharacter = this.CapsParseBoolDefault("requiresDBCSCharacter", false);
                    this._haveRequiresDBCSCharacter = true;
                }
                return this._requiresDBCSCharacter;
            }
        }

        ///<summary>Gets a value indicating whether the browser requires nonstandard error messages.</summary>
        ///<returns>true if the browser requires nonstandard error messages; otherwise, false. The default is false.</returns>
        public virtual bool RequiresHtmlAdaptiveErrorReporting
        {
            get
            {
                if (!this._haveRequiresHtmlAdaptiveErrorReporting)
                {
                    this._requiresHtmlAdaptiveErrorReporting = this.CapsParseBoolDefault("requiresHtmlAdaptiveErrorReporting", false);
                    this._haveRequiresHtmlAdaptiveErrorReporting = true;
                }
                return this._requiresHtmlAdaptiveErrorReporting;
            }
        }

        ///<summary>Gets a value indicating whether the browser requires the first element in the body of a Web page to be an HTML &lt;br&gt; element.</summary>
        ///<returns>true if the browser requires the first element in the body of a Web page to be an HTML BR element; otherwise, false. The default is false.</returns>
        public virtual bool RequiresLeadingPageBreak
        {
            get
            {
                if (!this._haveRequiresLeadingPageBreak)
                {
                    this._requiresLeadingPageBreak = this.CapsParseBoolDefault("requiresLeadingPageBreak", false);
                    this._haveRequiresLeadingPageBreak = true;
                }
                return this._requiresLeadingPageBreak;
            }
        }

        ///<summary>Gets a value indicating whether the browser does not support HTML &lt;br&gt; elements to format line breaks.</summary>
        ///<returns>true if the browser does not support HTML &lt;br&gt; elements; otherwise, false. The default is false.</returns>
        public virtual bool RequiresNoBreakInFormatting
        {
            get
            {
                if (!this._haveRequiresNoBreakInFormatting)
                {
                    this._requiresNoBreakInFormatting = this.CapsParseBoolDefault("requiresNoBreakInFormatting", false);
                    this._haveRequiresNoBreakInFormatting = true;
                }
                return this._requiresNoBreakInFormatting;
            }
        }

        ///<summary>Gets a value indicating whether the browser requires pages to contain a size-optimized form of markup language tags.</summary>
        ///<returns>true if the browser requires pages to contain a size-optimized form of markup language tags; otherwise, false. The default is false.</returns>
        public virtual bool RequiresOutputOptimization
        {
            get
            {
                if (!this._haveRequiresOutputOptimization)
                {
                    this._requiresOutputOptimization = this.CapsParseBoolDefault("requiresOutputOptimization", false);
                    this._haveRequiresOutputOptimization = true;
                }
                return this._requiresOutputOptimization;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports phone dialing based on plain text, or whether it requires special markup.</summary>
        ///<returns>true if the browser supports phone dialing based on plain text only; otherwise, false. The default is false.</returns>
        public virtual bool RequiresPhoneNumbersAsPlainText
        {
            get
            {
                if (!this._haveRequiresPhoneNumbersAsPlainText)
                {
                    this._requiresPhoneNumbersAsPlainText = this.CapsParseBoolDefault("requiresPhoneNumbersAsPlainText", false);
                    this._haveRequiresPhoneNumbersAsPlainText = true;
                }
                return this._requiresPhoneNumbersAsPlainText;
            }
        }

        ///<summary>Gets a value indicating whether the browser requires VIEWSTATE values to be specially encoded.</summary>
        ///<returns>true if the browser requires VIEWSTATE values to be specially encoded; otherwise, false. The default is false.</returns>
        public virtual bool RequiresSpecialViewStateEncoding
        {
            get
            {
                if (!this._haveRequiresSpecialViewStateEncoding)
                {
                    this._requiresSpecialViewStateEncoding = this.CapsParseBoolDefault("requiresSpecialViewStateEncoding", false);
                    this._haveRequiresSpecialViewStateEncoding = true;
                }
                return this._requiresSpecialViewStateEncoding;
            }
        }

        ///<summary>Gets a value indicating whether the browser requires unique form-action URLs.</summary>
        ///<returns>true if the browser requires unique form-action URLs; otherwise, false. The default is false.</returns>
        public virtual bool RequiresUniqueFilePathSuffix
        {
            get
            {
                if (!this._haveRequiresUniqueFilePathSuffix)
                {
                    this._requiresUniqueFilePathSuffix = this.CapsParseBoolDefault("requiresUniqueFilePathSuffix", false);
                    this._haveRequiresUniqueFilePathSuffix = true;
                }
                return this._requiresUniqueFilePathSuffix;
            }
        }

        ///<summary>Gets a value indicating whether the browser requires unique name attribute values of multiple HTML &lt;input type="checkbox"&gt; elements.</summary>
        ///<returns>true if the browser requires unique name attribute values of multiple HTML &lt;input type="checkbox"&gt; elements; otherwise, false. The default is false.</returns>
        public virtual bool RequiresUniqueHtmlCheckboxNames
        {
            get
            {
                if (!this._haveRequiresUniqueHtmlCheckboxNames)
                {
                    this._requiresUniqueHtmlCheckboxNames = this.CapsParseBoolDefault("requiresUniqueHtmlCheckboxNames", false);
                    this._haveRequiresUniqueHtmlCheckboxNames = true;
                }
                return this._requiresUniqueHtmlCheckboxNames;
            }
        }

        ///<summary>Gets a value indicating whether the browser requires unique name attribute values of multiple HTML &lt;input&gt; elements.</summary>
        ///<returns>true if the browser requires unique name attribute values of multiple HTML &lt;input&gt; elements; otherwise, false. The default is false.</returns>
        public virtual bool RequiresUniqueHtmlInputNames
        {
            get
            {
                if (!this._haveRequiresUniqueHtmlInputNames)
                {
                    this._requiresUniqueHtmlInputNames = this.CapsParseBoolDefault("requiresUniqueHtmlInputNames", false);
                    this._haveRequiresUniqueHtmlInputNames = true;
                }
                return this._requiresUniqueHtmlInputNames;
            }
        }

        ///<summary>Gets a value indicating whether postback data sent by the browser will be UrlEncoded.</summary>
        ///<returns>true if postback data sent by the browser will be UrlEncoded; otherwise, false. The default is false.</returns>
        public virtual bool RequiresUrlEncodedPostfieldValues
        {
            get
            {
                if (!this._haveRequiresUrlEncodedPostfieldValues)
                {
                    this._requiresUrlEncodedPostfieldValues = this.CapsParseBoolDefault("requiresUrlEncodedPostfieldValues", true);
                    this._haveRequiresUrlEncodedPostfieldValues = true;
                }
                return this._requiresUrlEncodedPostfieldValues;
            }
        }

        ///<summary>Returns the depth of the display, in bits per pixel.</summary>
        ///<returns>The depth of the display, in bits per pixel. The default is 1.</returns>
        public virtual int ScreenBitDepth
        {
            get
            {
                if (!this._haveScreenBitDepth)
                {
                    this._screenBitDepth = Convert.ToInt32(this["screenBitDepth"], CultureInfo.InvariantCulture);
                    this._haveScreenBitDepth = true;
                }
                return this._screenBitDepth;
            }
        }

        ///<summary>Returns the approximate height of the display, in character lines.</summary>
        ///<returns>The approximate height of the display, in character lines. The default is 6.</returns>
        public virtual int ScreenCharactersHeight
        {
            get
            {
                if (!this._haveScreenCharactersHeight)
                {
                    if (this["screenCharactersHeight"] == null)
                    {
                        int num = 480;
                        int num2 = 12;
                        if ((this["screenPixelsHeight"] != null) && (this["characterHeight"] != null))
                        {
                            num = Convert.ToInt32(this["screenPixelsHeight"], CultureInfo.InvariantCulture);
                            num2 = Convert.ToInt32(this["characterHeight"], CultureInfo.InvariantCulture);
                        }
                        else if (this["screenPixelsHeight"] != null)
                        {
                            num = Convert.ToInt32(this["screenPixelsHeight"], CultureInfo.InvariantCulture);
                            num2 = Convert.ToInt32(this["defaultCharacterHeight"], CultureInfo.InvariantCulture);
                        }
                        else if (this["characterHeight"] != null)
                        {
                            num = Convert.ToInt32(this["defaultScreenPixelsHeight"], CultureInfo.InvariantCulture);
                            num2 = Convert.ToInt32(this["characterHeight"], CultureInfo.InvariantCulture);
                        }
                        else if (this["defaultScreenCharactersHeight"] != null)
                        {
                            num = Convert.ToInt32(this["defaultScreenCharactersHeight"], CultureInfo.InvariantCulture);
                            num2 = 1;
                        }
                        this._screenCharactersHeight = num / num2;
                    }
                    else
                    {
                        this._screenCharactersHeight = Convert.ToInt32(this["screenCharactersHeight"], CultureInfo.InvariantCulture);
                    }
                    this._haveScreenCharactersHeight = true;
                }
                return this._screenCharactersHeight;
            }
        }

        ///<summary>Returns the approximate width of the display, in characters.</summary>
        ///<returns>The approximate width of the display, in characters. The default is 12.</returns>
        public virtual int ScreenCharactersWidth
        {
            get
            {
                if (!this._haveScreenCharactersWidth)
                {
                    if (this["screenCharactersWidth"] == null)
                    {
                        int num = 640;
                        int num2 = 8;
                        if ((this["screenPixelsWidth"] != null) && (this["characterWidth"] != null))
                        {
                            num = Convert.ToInt32(this["screenPixelsWidth"], CultureInfo.InvariantCulture);
                            num2 = Convert.ToInt32(this["characterWidth"], CultureInfo.InvariantCulture);
                        }
                        else if (this["screenPixelsWidth"] != null)
                        {
                            num = Convert.ToInt32(this["screenPixelsWidth"], CultureInfo.InvariantCulture);
                            num2 = Convert.ToInt32(this["defaultCharacterWidth"], CultureInfo.InvariantCulture);
                        }
                        else if (this["characterWidth"] != null)
                        {
                            num = Convert.ToInt32(this["defaultScreenPixelsWidth"], CultureInfo.InvariantCulture);
                            num2 = Convert.ToInt32(this["characterWidth"], CultureInfo.InvariantCulture);
                        }
                        else if (this["defaultScreenCharactersWidth"] != null)
                        {
                            num = Convert.ToInt32(this["defaultScreenCharactersWidth"], CultureInfo.InvariantCulture);
                            num2 = 1;
                        }
                        this._screenCharactersWidth = num / num2;
                    }
                    else
                    {
                        this._screenCharactersWidth = Convert.ToInt32(this["screenCharactersWidth"], CultureInfo.InvariantCulture);
                    }
                    this._haveScreenCharactersWidth = true;
                }
                return this._screenCharactersWidth;
            }
        }

        ///<summary>Returns the approximate height of the display, in pixels.</summary>
        ///<returns>The approximate height of the display, in pixels. The default is 72.</returns>
        public virtual int ScreenPixelsHeight
        {
            get
            {
                if (!this._haveScreenPixelsHeight)
                {
                    if (this["screenPixelsHeight"] == null)
                    {
                        int num = 40;
                        int num2 = 12;
                        if ((this["screenCharactersHeight"] != null) && (this["characterHeight"] != null))
                        {
                            num = Convert.ToInt32(this["screenCharactersHeight"], CultureInfo.InvariantCulture);
                            num2 = Convert.ToInt32(this["characterHeight"], CultureInfo.InvariantCulture);
                        }
                        else if (this["screenCharactersHeight"] != null)
                        {
                            num = Convert.ToInt32(this["screenCharactersHeight"], CultureInfo.InvariantCulture);
                            num2 = Convert.ToInt32(this["defaultCharacterHeight"], CultureInfo.InvariantCulture);
                        }
                        else if (this["characterHeight"] != null)
                        {
                            num = Convert.ToInt32(this["defaultScreenCharactersHeight"], CultureInfo.InvariantCulture);
                            num2 = Convert.ToInt32(this["characterHeight"], CultureInfo.InvariantCulture);
                        }
                        else if (this["defaultScreenPixelsHeight"] != null)
                        {
                            num = Convert.ToInt32(this["defaultScreenPixelsHeight"], CultureInfo.InvariantCulture);
                            num2 = 1;
                        }
                        this._screenPixelsHeight = num * num2;
                    }
                    else
                    {
                        this._screenPixelsHeight = Convert.ToInt32(this["screenPixelsHeight"], CultureInfo.InvariantCulture);
                    }
                    this._haveScreenPixelsHeight = true;
                }
                return this._screenPixelsHeight;
            }
        }

        ///<summary>Returns the approximate width of the display, in pixels.</summary>
        ///<returns>The approximate width of the display, in pixels. The default is 96.</returns>
        public virtual int ScreenPixelsWidth
        {
            get
            {
                if (!this._haveScreenPixelsWidth)
                {
                    if (this["screenPixelsWidth"] == null)
                    {
                        int num = 80;
                        int num2 = 8;
                        if ((this["screenCharactersWidth"] != null) && (this["characterWidth"] != null))
                        {
                            num = Convert.ToInt32(this["screenCharactersWidth"], CultureInfo.InvariantCulture);
                            num2 = Convert.ToInt32(this["characterWidth"], CultureInfo.InvariantCulture);
                        }
                        else if (this["screenCharactersWidth"] != null)
                        {
                            num = Convert.ToInt32(this["screenCharactersWidth"], CultureInfo.InvariantCulture);
                            num2 = Convert.ToInt32(this["defaultCharacterWidth"], CultureInfo.InvariantCulture);
                        }
                        else if (this["characterWidth"] != null)
                        {
                            num = Convert.ToInt32(this["defaultScreenCharactersWidth"], CultureInfo.InvariantCulture);
                            num2 = Convert.ToInt32(this["characterWidth"], CultureInfo.InvariantCulture);
                        }
                        else if (this["defaultScreenPixelsWidth"] != null)
                        {
                            num = Convert.ToInt32(this["defaultScreenPixelsWidth"], CultureInfo.InvariantCulture);
                            num2 = 1;
                        }
                        this._screenPixelsWidth = num * num2;
                    }
                    else
                    {
                        this._screenPixelsWidth = Convert.ToInt32(this["screenPixelsWidth"], CultureInfo.InvariantCulture);
                    }
                    this._haveScreenPixelsWidth = true;
                }
                return this._screenPixelsWidth;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports the ACCESSKEY attribute of HTML &lt;a&gt; (anchor) and &lt;input&gt; elements.</summary>
        ///<returns>true if the browser supports the accesskey attribute of HTML &lt;a&gt;  (anchor) and &lt;input&gt; elements; otherwise, false. The default is false.</returns>
        public virtual bool SupportsAccesskeyAttribute
        {
            get
            {
                if (!this._haveSupportsAccesskeyAttribute)
                {
                    this._supportsAccesskeyAttribute = this.CapsParseBoolDefault("supportsAccesskeyAttribute", false);
                    this._haveSupportsAccesskeyAttribute = true;
                }
                return this._supportsAccesskeyAttribute;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports the bgcolor attribute of the HTML &lt;body&gt; element.</summary>
        ///<returns>true if the browser supports the bgcolor attribute of the HTML &lt;body&gt; element; otherwise, false. The default is true.</returns>
        public virtual bool SupportsBodyColor
        {
            get
            {
                if (!this._haveSupportsBodyColor)
                {
                    this._supportsBodyColor = this.CapsParseBoolDefault("supportsBodyColor", false);
                    this._haveSupportsBodyColor = true;
                }
                return this._supportsBodyColor;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports HTML &lt;b&gt; elements to format bold text.</summary>
        ///<returns>true if the browser supports HTML &lt;b&gt;  elements to format bold text; otherwise, false. The default is false.</returns>
        public virtual bool SupportsBold
        {
            get
            {
                if (!this._haveSupportsBold)
                {
                    this._supportsBold = this.CapsParseBoolDefault("supportsBold", true);
                    this._haveSupportsBold = true;
                }
                return this._supportsBold;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports the cache-control value for the http-equiv attribute of HTML &lt;meta&gt; elements.</summary>
        ///<returns>true if the browser supports the cache-control value for the http-equiv attribute of HTML &lt;meta&gt; elements; otherwise, false. The default is true.</returns>
        public virtual bool SupportsCacheControlMetaTag
        {
            get
            {
                if (!this._haveSupportsCacheControlMetaTag)
                {
                    this._supportsCacheControlMetaTag = this.CapsParseBoolDefault("supportsCacheControlMetaTag", true);
                    this._haveSupportsCacheControlMetaTag = true;
                }
                return this._supportsCacheControlMetaTag;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports callback scripts.</summary>
        ///<returns>true if the browser supports callback scripts; otherwise, false. The default is false.</returns>
        public virtual bool SupportsCallback
        {
            get
            {
                if (!this._haveSupportsCallback)
                {
                    this._supportsCallback = this.CapsParseBoolDefault("supportsCallback", false);
                    this._haveSupportsCallback = true;
                }
                return this._supportsCallback;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports Cascading Style Sheets (CSS).</summary>
        ///<returns>true if the browser supports CSS; otherwise, false. The default is false.</returns>
        public virtual bool SupportsCss
        {
            get
            {
                if (!this._haveSupportsCss)
                {
                    this._supportsCss = this.CapsParseBoolDefault("supportsCss", false);
                    this._haveSupportsCss = true;
                }
                return this._supportsCss;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports the align attribute of HTML &lt;div&gt; elements.</summary>
        ///<returns>true if the browser supports the align attribute of HTML &lt;div&gt; elements; otherwise, false. The default is true.</returns>
        public virtual bool SupportsDivAlign
        {
            get
            {
                if (!this._haveSupportsDivAlign)
                {
                    this._supportsDivAlign = this.CapsParseBoolDefault("supportsDivAlign", false);
                    this._haveSupportsDivAlign = true;
                }
                return this._supportsDivAlign;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports the nowrap attribute of HTML &lt;div&gt; elements.</summary>
        ///<returns>true if the browser supports the nowrap attribute of HTML &lt;div&gt; elements; otherwise, false. The default is false.</returns>
        public virtual bool SupportsDivNoWrap
        {
            get
            {
                if (!this._haveSupportsDivNoWrap)
                {
                    this._supportsDivNoWrap = this.CapsParseBoolDefault("supportsDivNoWrap", false);
                    this._haveSupportsDivNoWrap = true;
                }
                return this._supportsDivNoWrap;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports empty (null) strings in cookie values.</summary>
        ///<returns>true if the browser supports empty (null) strings in cookie values; otherwise, false. The default is false.</returns>
        public virtual bool SupportsEmptyStringInCookieValue
        {
            get
            {
                if (!this._haveSupportsEmptyStringInCookieValue)
                {
                    this._supportsEmptyStringInCookieValue = this.CapsParseBoolDefault("supportsEmptyStringInCookieValue", true);
                    this._haveSupportsEmptyStringInCookieValue = true;
                }
                return this._supportsEmptyStringInCookieValue;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports the color attribute of HTML &lt;font&gt; elements.</summary>
        ///<returns>true if the browser supports the color attribute of HTML &lt;font&gt; elements; otherwise, false. The default is true.</returns>
        public virtual bool SupportsFontColor
        {
            get
            {
                if (!this._haveSupportsFontColor)
                {
                    this._supportsFontColor = this.CapsParseBoolDefault("supportsFontColor", false);
                    this._haveSupportsFontColor = true;
                }
                return this._supportsFontColor;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports the name attribute of HTML &lt;font&gt; elements.</summary>
        ///<returns>true if the browser supports the name attribute of HTML &lt;font&gt; elements; otherwise, false. The default is false.</returns>
        public virtual bool SupportsFontName
        {
            get
            {
                if (!this._haveSupportsFontName)
                {
                    this._supportsFontName = this.CapsParseBoolDefault("supportsFontName", false);
                    this._haveSupportsFontName = true;
                }
                return this._supportsFontName;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports the size attribute of HTML &lt;font&gt; elements.</summary>
        ///<returns>true if the browser supports the size attribute of HTML &lt;font&gt; elements; otherwise, false. The default is false.</returns>
        public virtual bool SupportsFontSize
        {
            get
            {
                if (!this._haveSupportsFontSize)
                {
                    this._supportsFontSize = this.CapsParseBoolDefault("supportsFontSize", false);
                    this._haveSupportsFontSize = true;
                }
                return this._supportsFontSize;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports using a custom image in place of a standard form Submit button.</summary>
        ///<returns>true if the browser supports using a custom image in place of a standard form Submit button; otherwise, false. The default is false.</returns>
        public virtual bool SupportsImageSubmit
        {
            get
            {
                if (!this._haveSupportsImageSubmit)
                {
                    this._supportsImageSubmit = this.CapsParseBoolDefault("supportsImageSubmit", false);
                    this._haveSupportsImageSubmit = true;
                }
                return this._supportsImageSubmit;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports i-mode symbols.</summary>
        ///<returns>true if the browser supports i-mode symbols; otherwise, false. The default is false.</returns>
        public virtual bool SupportsIModeSymbols
        {
            get
            {
                if (!this._haveSupportsIModeSymbols)
                {
                    this._supportsIModeSymbols = this.CapsParseBoolDefault("supportsIModeSymbols", false);
                    this._haveSupportsIModeSymbols = true;
                }
                return this._supportsIModeSymbols;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports the istyle attribute of HTML &lt;input&gt; elements.</summary>
        ///<returns>true if the browser supports the istyle attribute of HTML &lt;input&gt; elements; otherwise, false. The default is false.</returns>
        public virtual bool SupportsInputIStyle
        {
            get
            {
                if (!this._haveSupportsInputIStyle)
                {
                    this._supportsInputIStyle = this.CapsParseBoolDefault("supportsInputIStyle", false);
                    this._haveSupportsInputIStyle = true;
                }
                return this._supportsInputIStyle;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports the mode attribute of HTML &lt;input&gt; elements.</summary>
        ///<returns>true if the browser supports the mode attribute of HTML &lt;input&gt; elements; otherwise, false. The default is false.</returns>
        public virtual bool SupportsInputMode
        {
            get
            {
                if (!this._haveSupportsInputMode)
                {
                    this._supportsInputMode = this.CapsParseBoolDefault("supportsInputMode", false);
                    this._haveSupportsInputMode = true;
                }
                return this._supportsInputMode;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports HTML &lt;i&gt; elements to format italic text.</summary>
        ///<returns>true if the browser supports HTML &lt;i&gt; elements to format italic text; otherwise, false. The default is false.</returns>
        public virtual bool SupportsItalic
        {
            get
            {
                if (!this._haveSupportsItalic)
                {
                    this._supportsItalic = this.CapsParseBoolDefault("supportsItalic", true);
                    this._haveSupportsItalic = true;
                }
                return this._supportsItalic;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports J-Phone multimedia attributes.</summary>
        ///<returns>true if the browser supports J-Phone multimedia attributes; otherwise, false. The default is false.</returns>
        public virtual bool SupportsJPhoneMultiMediaAttributes
        {
            get
            {
                if (!this._haveSupportsJPhoneMultiMediaAttributes)
                {
                    this._supportsJPhoneMultiMediaAttributes = this.CapsParseBoolDefault("supportsJPhoneMultiMediaAttributes", false);
                    this._haveSupportsJPhoneMultiMediaAttributes = true;
                }
                return this._supportsJPhoneMultiMediaAttributes;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports J-Phone�specific picture symbols.</summary>
        ///<returns>true if the browser supports J-Phone�specific picture symbols; otherwise, false. The default is false.</returns>
        public virtual bool SupportsJPhoneSymbols
        {
            get
            {
                if (!this._haveSupportsJPhoneSymbols)
                {
                    this._supportsJPhoneSymbols = this.CapsParseBoolDefault("supportsJPhoneSymbols", false);
                    this._haveSupportsJPhoneSymbols = true;
                }
                return this._supportsJPhoneSymbols;
            }
        }

        internal bool SupportsMaintainScrollPositionOnPostback
        {
            get
            {
                if (!this._haveSupportsMaintainScrollPositionOnPostback)
                {
                    this._supportsMaintainScrollPositionOnPostback = this.CapsParseBoolDefault("supportsMaintainScrollPositionOnPostback", false);
                    this._haveSupportsMaintainScrollPositionOnPostback = true;
                }
                return this._supportsMaintainScrollPositionOnPostback;
            }
        }


        ///<summary>Gets a value indicating whether the browser supports a query string in the action attribute value of HTML &lt;form&gt; elements.</summary>
        ///<returns>true if the browser supports a query string in the action attribute value of HTML &lt;form&gt; elements; otherwise, false. The default is true.</returns>
        public virtual bool SupportsQueryStringInFormAction
        {
            get
            {
                if (!this._haveSupportsQueryStringInFormAction)
                {
                    this._supportsQueryStringInFormAction = this.CapsParseBoolDefault("supportsQueryStringInFormAction", true);
                    this._haveSupportsQueryStringInFormAction = true;
                }
                return this._supportsQueryStringInFormAction;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports cookies on redirection.</summary>
        ///<returns>true if the browser supports cookies on redirection; otherwise, false. The default is true.</returns>
        public virtual bool SupportsRedirectWithCookie
        {
            get
            {
                if (!this._haveSupportsRedirectWithCookie)
                {
                    this._supportsRedirectWithCookie = this.CapsParseBoolDefault("supportsRedirectWithCookie", true);
                    this._haveSupportsRedirectWithCookie = true;
                }
                return this._supportsRedirectWithCookie;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports the multiple attribute of HTML &lt;select&gt; elements.</summary>
        ///<returns>true if the browser supports the multiple attribute of HTML &lt;select&gt; elements; otherwise, false. The default is true.</returns>
        public virtual bool SupportsSelectMultiple
        {
            get
            {
                if (!this._haveSupportsSelectMultiple)
                {
                    this._supportsSelectMultiple = this.CapsParseBoolDefault("supportsSelectMultiple", false);
                    this._haveSupportsSelectMultiple = true;
                }
                return this._supportsSelectMultiple;
            }
        }

        ///<summary>Gets a value indicating whether the clearing of a checked HTML &lt;input type=checkbox&gt; element is reflected in postback data.</summary>
        ///<returns>true if the clearing of a checked HTML &lt;input type=checkbox&gt; element is reflected in postback data; otherwise, false. The default is true.</returns>
        public virtual bool SupportsUncheck
        {
            get
            {
                if (!this._haveSupportsUncheck)
                {
                    this._supportsUncheck = this.CapsParseBoolDefault("supportsUncheck", true);
                    this._haveSupportsUncheck = true;
                }
                return this._supportsUncheck;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports receiving XML over HTTP.</summary>
        ///<returns>true if the browser supports receiving XML over HTTP; otherwise, false. The default is false.</returns>
        public virtual bool SupportsXmlHttp
        {
            get
            {
                if (!this._haveSupportsXmlHttp)
                {
                    this._supportsXmlHttp = this.CapsParseBoolDefault("supportsXmlHttp", false);
                    this._haveSupportsXmlHttp = true;
                }
                return this._supportsXmlHttp;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports HTML &lt;table&gt; elements.</summary>
        ///<returns>true if the browser supports HTML &lt;table&gt; elements; otherwise, false. The default is false.</returns>
        public bool Tables
        {
            get
            {
                if (!this._havetables)
                {
                    this._tables = this.CapsParseBool("tables");
                    this._havetables = true;
                }
                return this._tables;
            }
        }

        ///<summary>Gets the name and major (integer) version number of the browser.</summary>
        ///<returns>The name and major version number of the browser.</returns>
        public string Type
        {
            get
            {
                if (!this._havetype)
                {
                    this._type = this["type"];
                    this._havetype = true;
                }
                return this._type;
            }
        }

        ///<summary>Gets a value indicating whether the browser supports Visual Basic Scripting edition (VBScript).</summary>
        ///<returns>true if the browser supports VBScript; otherwise, false. The default is false.</returns>
        public bool VBScript
        {
            get
            {
                if (!this._havevbscript)
                {
                    this._vbscript = this.CapsParseBool("vbscript");
                    this._havevbscript = true;
                }
                return this._vbscript;
            }
        }

        ///<summary>Gets the full version number (integer and decimal) of the browser as a string.</summary>
        ///<returns>The full version number of the browser as a string.</returns>
        public string Version
        {
            get
            {
                if (!this._haveversion)
                {
                    this._version = this["version"];
                    this._haveversion = true;
                }
                return this._version;
            }
        }

        ///<summary>Gets the version of the World Wide Web Consortium (W3C) XML Document Object Model (DOM) that the browser supports.</summary>
        ///<returns>The number of the W3C XML DOM version number that the browser supports.</returns>
        public Version W3CDomVersion
        {
            get
            {
                if (!this._havew3cdomversion)
                {
                    this._w3cdomversion = new Version(this["w3cdomversion"]);
                    this._havew3cdomversion = true;
                }
                return this._w3cdomversion;
            }
        }

        ///<summary>Gets a value indicating whether the client is a Win16-based computer.</summary>
        ///<returns>true if the browser is running on a Win16-based computer; otherwise, false. The default is false.</returns>
        public bool Win16
        {
            get
            {
                if (!this._havewin16)
                {
                    this._win16 = this.CapsParseBool("win16");
                    this._havewin16 = true;
                }
                return this._win16;
            }
        }

        ///<summary>Gets a value indicating whether the client is a Win32-based computer.</summary>
        ///<returns>true if the client is a Win32-based computer; otherwise, false. The default is false.</returns>
        public bool Win32
        {
            get
            {
                if (!this._havewin32)
                {
                    this._win32 = this.CapsParseBool("win32");
                    this._havewin32 = true;
                }
                return this._win32;
            }
        }

        #endregion
    }
}
