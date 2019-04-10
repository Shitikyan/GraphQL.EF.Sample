using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.EntityFramework;
using WebApplication1.DataContexts;

namespace WebApplication1.GraphTypes
{
    public class BookGraph : EfObjectGraphType<Book>
    {
        public BookGraph(IEfGraphQLService efGraphQLService) : base(efGraphQLService)
        {
            Field(x => x.Id);
            Field(x => x.Name);
        }
    }

    public class Book
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
