namespace OrangeCMS.Tooling
{
    public class TableImportSpec
    {
        public TableImportSpec(string tableName, string query)
        {
            TableName = tableName;
            Query = query;
        }

        public TableImportSpec(string tableName)
            : this(tableName, "SELECT * FROM [dbo].[" + tableName + "]")
        {

        }

        public string TableName { get; set; }

        public string Query { get; set; }
    }
}
