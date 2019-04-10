using GraphQL.EntityFramework;

namespace WebApplication1.GraphTypes
{
    public class AuthorGraph : EfObjectGraphType<Author>
    {
        public AuthorGraph(IEfGraphQLService efGraphQLService) : base(efGraphQLService)
        {
            Field(x => x.Id);
            Field(x => x.Name);
        }
    }

    public class Author
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
