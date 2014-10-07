using Bounce.Framework;

namespace CodeKinden.OrangeCMS.Tasks.Bounce
{
    public class db_create
    {
        [Task(Command = "db:create", Description = "Creates an empty database.")]
        public void CreateDatabase(string sqlConnectionString)
        {
            DB.RunDatabaseMigrations(sqlConnectionString, true);
        }
    }
}