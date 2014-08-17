using System.Collections.Generic;
using System.Linq;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DatabaseContext dbContext;

        public CategoryService(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Category> FindByClient(long id)
        {
            return dbContext.Categories.Where(x => x.Client.Id == id).OrderBy(x => x.Name);
        }
    }
}
