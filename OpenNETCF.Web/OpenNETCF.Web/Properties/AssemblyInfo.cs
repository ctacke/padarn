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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("OpenNETCF.Web")]
[assembly: AssemblyDescription("ASP.NET for Windows CE")]
#if DEBUG
[assembly: AssemblyConfiguration("DEBUG")]
#else
[assembly: AssemblyConfiguration("RETAIL")]
#endif
[assembly: AssemblyCompany("OpenNETCF Consulting, LLC")]
[assembly: AssemblyProduct("OpenNETCF.Web")]
[assembly: AssemblyCopyright("Copyright © 2007-2018 OpenNETCF Consulting, LLC")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("614839ce-0b88-4467-8932-56d520e4a59c")]
[assembly: InternalsVisibleTo("OpenNETCF.Configuration, PublicKey=00240000048000009400000006020000002400005253413100040000010001002beeba3bfe7c548e085cffb8c2b6fd61ddd02b06d70864bb7de8bb22473edf5ab4b2196ff98e232c3e87f11fd7986b743d5d3fdd6ecaf624bacfed116e1cefa50cd652365371d0ebd2702eb1084fed46df79ac0f59f4d66c547918613d565dcf106843f3458516d3cd26f057a346d9f645fc24a7410a095c754835916e13cdbe")]
[assembly: InternalsVisibleTo("OpenNETCF.Web.Services.Test, PublicKey=0024000004800000940000000602000000240000525341310004000001000100bdf2561aa1ecee67406dcf38ffcf6ff38c02648bda1c9263a70154cddaf4ec2111f650ee9b9cd94e15fe3487f417a4733fc6525424cca99ffbb52b56c93d7de57650929799e54f39f2ff4297fdcdfe1730fe0d960c38332a9b6d39c8835286a2a0ef58db85db95f0a76b4af4db31565ab3f24118d129d89e6bb2886e95063bd3")]
[assembly: InternalsVisibleTo("OpenNETCF.Web.Integration.Test, PublicKey=0024000004800000940000000602000000240000525341310004000001000100bdf2561aa1ecee67406dcf38ffcf6ff38c02648bda1c9263a70154cddaf4ec2111f650ee9b9cd94e15fe3487f417a4733fc6525424cca99ffbb52b56c93d7de57650929799e54f39f2ff4297fdcdfe1730fe0d960c38332a9b6d39c8835286a2a0ef58db85db95f0a76b4af4db31565ab3f24118d129d89e6bb2886e95063bd3")]
[assembly: InternalsVisibleTo("OpenNETCF.Web.Test, PublicKey=00240000048000009400000006020000002400005253413100040000010001002beeba3bfe7c548e085cffb8c2b6fd61ddd02b06d70864bb7de8bb22473edf5ab4b2196ff98e232c3e87f11fd7986b743d5d3fdd6ecaf624bacfed116e1cefa50cd652365371d0ebd2702eb1084fed46df79ac0f59f4d66c547918613d565dcf106843f3458516d3cd26f057a346d9f645fc24a7410a095c754835916e13cdbe")]
[assembly: InternalsVisibleTo("OpenNETCF.Web.Unit.Test")]

[assembly: AssemblyVersion("1.6.16137.0")]

#if !WindowsCE
[assembly: AssemblyFileVersion("1.6.16137.0")]
#endif