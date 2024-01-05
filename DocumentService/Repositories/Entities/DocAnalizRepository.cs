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
            DocAnaliz? docAnaliz = this.context.DocAnaliz.Where(p => p.DocLink == entity).FirstOrDefault();
            if (docAnaliz == null) { return new DocAnaliz(); }
            else
                return docAnaliz;
        }

        public IQueryable<DocAnaliz> GetDocsAnaliz()
        {
           return context.DocAnaliz;
        }

        public void SavePersons(DocAnaliz entity)
        {
            if (entity.IDALL == default)
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
