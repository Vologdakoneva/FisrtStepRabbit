using DocumentService.Data;
using DocumentService.Entities;
using DocumentService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text;

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
            //DocAnaliz[] docs =  context.DocAnaliz.OrderByDescending(p => p.DataChange).ToArray();

            //string text = docs[0].Fio;
            
            //Encoding utf8 = Encoding.GetEncoding("UTF-8");
            //Encoding win1251 = Encoding.Default; // GetEncoding("1251");
            
            //byte[] utf8Bytes = win1251.GetBytes(text);
            //byte[] win1251Bytes = Encoding.Convert(win1251, utf8, utf8Bytes);

            //Console.WriteLine(win1251.GetString(win1251Bytes));
            return context.DocAnaliz.OrderByDescending(p => p.DataChange); 
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
