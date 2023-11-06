namespace ServerOnly.Interfaces
{
    public interface IDocument
    {
        MemoryStream GetMemoryStream(Dictionary<string, object> data, string TemplateName);
    }
}
