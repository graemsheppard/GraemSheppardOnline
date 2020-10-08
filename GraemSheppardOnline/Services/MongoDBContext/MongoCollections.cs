namespace GraemSheppardOnline.Services.MongoDBContext
{
    public partial class MongoDBContext
    {

        public UserCollection<User> Users;
        public ConnectionCollection<Connection> Connections;
        public AuthenticateCollection<Authenticate> Authenticate;
        public ItemCollection<Item> Items;
        public ArticleCollection<Article> Articles;
        public FileCollection<File> Files;

        private void GetCollections ()
        {
            Users = new UserCollection<User>(GetCollection<User>("Users"));
            Authenticate = new AuthenticateCollection<Authenticate>(GetCollection<Authenticate>("Authenticate"));
            Items = new ItemCollection<Item>(GetCollection<Item>("Items"));
            Connections = new ConnectionCollection<Connection>(GetCollection<Connection>("Connections"));
            Articles = new ArticleCollection<Article>(GetCollection<Article>("Articles"));
            Files = new FileCollection<File>(GetCollection<File>("Files"));
        }
    }
}
