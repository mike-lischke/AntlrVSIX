﻿//using System.Threading;
//using System.Threading.Tasks;
//using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
//using OmniSharp.Extensions.LanguageServer.Protocol.Models;
//using OmniSharp.Extensions.LanguageServer.Protocol.Server;
//using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

//namespace LanguageServer.Handlers
//{
//    internal sealed class HoverHandler : IHoverHandler
//    {
//        private readonly LanguageServer.Exec.LanguageServerWorkspace _workspace;
//        private readonly TextDocumentRegistrationOptions _registrationOptions;

//        public HoverHandler(LanguageServer.Exec.LanguageServerWorkspace workspace, TextDocumentRegistrationOptions registrationOptions)
//        {
//            _workspace = workspace;
//            _registrationOptions = registrationOptions;
//        }

//        public TextDocumentRegistrationOptions GetRegistrationOptions() => _registrationOptions;

//        public async Task<Hover> Handle(HoverParams request, CancellationToken token)
//        {
//            var (document, position) = _workspace.GetLogicalDocument(request);
//            return null;

//            //if (item != null)
//            //{
//            //    Range symbolRange;

//            //    var content = item.Content;
//            //    var markdownText = $"``` {Helpers.ToLspLanguage(document.Language)}\n{content.MainDescription.GetFullText()}\n```\n";

//            //    if (!content.Documentation.IsEmpty)
//            //    {
//            //        markdownText += "---\n";
//            //        markdownText += content.Documentation.GetFullText();
//            //    }

//            //    symbolRange = Helpers.ToRange(document.SourceText, item.TextSpan);

//            //    return new Hover
//            //    {
//            //        Contents = new MarkedStringsOrMarkupContent(new MarkupContent { Kind = MarkupKind.Markdown, Value = markdownText }),
//            //        Range = symbolRange
//            //    };
//            //}
//            //else
//            //{
//            //    return null;
//            //}
//        }

//        public void SetCapability(HoverCapability capability) { }
//    }
//}