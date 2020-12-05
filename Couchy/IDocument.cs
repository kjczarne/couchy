namespace Couchy
{
    public interface IDocument
    {
        string Id { get; }

        string ToJson();
    }
}