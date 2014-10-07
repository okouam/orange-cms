using System;
using CodeKinden.OrangeCMS.Repositories;

namespace CodeKinden.OrangeCMS.Application.Tests.Helpers
{
    class RepositoryTest : BaseTest
    {
        protected IDbContextScope dbContextScope;

        public override void SetUp()
        {
            base.SetUp();
            dbContextScope = container.GetInstance<IDbContextScope>();
        }
        
        protected void WithDbContext(Action<DatabaseContext> action)
        {
            var dbContext = dbContextScope.CreateDbContext();
            action(dbContext);
            dbContext.SaveChanges();
        }
    }
}
