using System.Threading.Tasks;

namespace Couchy
{
    public interface IDb
    {
        string Name { get; set; }

        Task Create(IDocument item);

        Task<IDocument> Read(string id, string revision);

        Task Update(IDocument item);

        Task Delete(IDocument item);
    }
}