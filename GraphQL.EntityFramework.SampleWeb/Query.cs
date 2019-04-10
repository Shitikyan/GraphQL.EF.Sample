using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.EntityFramework;
using WebApplication1.DataContexts;

namespace WebApplication1
{
    public class Query : QueryGraphType
    {
        public Query(IEfGraphQLService efGraphQlService) : base(efGraphQlService)
        {
            AddQueryField(
                name: "books",
                resolve: context =>
                {
                    var dataContext = (context.UserContext as DataContexts.DataContexts);
                    return dataContext.bookDbContext.Books;
                });

            AddQueryField(
                name: "authors",
                resolve: context =>
                {
                    var dataContext = (context.UserContext as DataContexts.DataContexts);
                    return dataContext.authorDbContext.Author;
                });
        }
    }
}
