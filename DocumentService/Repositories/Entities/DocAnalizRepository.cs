using DocumentService.Data;
using DocumentService.Entities;
using DocumentService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.Repositories.Entities
{
    public class DocAnalizRepository : IDocAnaliz
    {
        private readonly DocumentDbContext context;

        public DocAnalizRepository(DocumentDbContext context)
        {
            this.context = context;
        }
        public void DeleteDocAnaliz(Guid GuidDocAnaliz)
        {
            throw new NotImplementedException();
        }

        public DocAnaliz GetDocAnalizEntity(Guid entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DocAnaliz> GetDocsAnaliz()
        {
           return context.DocAnaliz;
        }

        public void SavePersons(DocAnaliz entity)
        {
            throw new NotImplementedException();
        }
    }
}
