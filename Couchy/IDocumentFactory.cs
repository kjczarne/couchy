namespace Couchy
{
    public interface IDocumentFactory
    {
        T CreateDocument<T>(string json) where T: IDocument;
    }
}