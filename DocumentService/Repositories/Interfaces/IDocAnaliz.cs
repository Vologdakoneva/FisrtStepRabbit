using DocumentService.Entities;

namespace DocumentService.Repositories.Interfaces
{
    public interface IDocAnaliz
    {
        IQueryable<DocAnaliz> GetDocsAnaliz();
        DocAnaliz GetDocAnalizEntity(Guid entity);
        void SavePersons(DocAnaliz entity);
        void DeleteDocAnaliz(Guid GuidDocAnaliz);
    }
}
