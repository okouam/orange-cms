using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Services
{
    public class CategoryService : ICategoryService
    {
        public async Task<IEnumerable<Category>> FindByClient(long id)
        {
            using (var dbContext = new DatabaseContext())
            {
                return await dbContext.Categories.Where(x => x.Client.Id == id).OrderBy(x => x.Name).ToListAsync();
            }
        }
    }
}
