using System.Collections.Generic;
using System.Linq;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Services
{
    public class CategoryService : ICategoryService
    {
        public IEnumerable<Category> FindByClient(long id)
        {
            using (var dbContext = new DatabaseContext())
            {
                return dbContext.Categories.Where(x => x.Client.Id == id).OrderBy(x => x.Name).ToList();
            }
        }
    }
}
