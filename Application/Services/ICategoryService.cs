using System.Collections.Generic;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> FindByClient(long id); 
    }
}