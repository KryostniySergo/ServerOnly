namespace ServerOnly.Interfaces
{
    public interface IDocument
    {
        MemoryStream GetMemoryStream(string TemplateName);
    }
}
