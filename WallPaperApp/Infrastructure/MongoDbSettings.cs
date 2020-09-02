namespace WallPaperApp.Infrastructure
{
    public class MongoDbSettings
    {
        public string ConnectionString;
        public string Database;


        #region Const Values

        public const string ConnectionStringValue = nameof(ConnectionString);
        public const string DatabaseValue = nameof(Database);

        #endregion
    }
}