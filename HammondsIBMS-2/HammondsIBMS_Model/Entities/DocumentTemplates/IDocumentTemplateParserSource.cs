namespace HammondsIBMS_Domain.Entities.DocumentTemplates
{
    public interface IDocumentTemplateParserSource<I>
    {
        string GetFieldValue(string field, I recordSource);
        string Body { get; set; }
    }
}