using System.Text.RegularExpressions;
using HammondsIBMS_Domain.Entities.DocumentTemplates;

namespace HammondsIBMS_Application.DocumentParser
{
    public class DocumentTemplateParser<B, R>
        where B : IDocumentTemplateParserSource<R>
        where R : class
    {
        private readonly B _documentParserSource;
        private readonly IParserFormatConverter _parserFormatConverter;
        private R _recordSource;
        private readonly string _regexMatch;

        public DocumentTemplateParser(B documentParserSource, IParserFormatConverter parserFormatConverter=null , string regexMatch = @"\$(\S+)\$")
        {
            _documentParserSource = documentParserSource;
            _parserFormatConverter = parserFormatConverter;
            _regexMatch = regexMatch;
        }

        public string ReturnParsedDocument(R recordSource)
        {
            _recordSource = recordSource;
            var regex = new Regex(_regexMatch);
            var matchEvaluator = new MatchEvaluator(SubstituteField);

            var parsedDocument = regex.Replace(_documentParserSource.Body, matchEvaluator);
            if(_parserFormatConverter!=null)
                parsedDocument = _parserFormatConverter.Convert(parsedDocument);

            return parsedDocument;
        }

        private string SubstituteField(Match field)
        {
            return _documentParserSource.GetFieldValue(field.Value, _recordSource);
        }
    }
}
