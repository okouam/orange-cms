using System.Collections.Generic;
using System.Threading.Tasks;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Services
{
    public interface ICategoryService
    {
       Task<IEnumerable<Category>> FindByClient(long id); 
    }
}