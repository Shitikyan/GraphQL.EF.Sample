﻿using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Types;

namespace GraphQL.EntityFramework
{
    public partial interface IEfGraphQLService
    {
        void AddQueryConnectionField<TReturn>(ObjectGraphType graph,
            string name,
            Func<ResolveFieldContext<object>, IQueryable<TReturn>> resolve,
            Type graphType = null,
            IEnumerable<QueryArgument> arguments = null,
            int pageSize = 10)
            where TReturn : class;

        void AddQueryConnectionField<TSource, TReturn>(
            ObjectGraphType graph,
            string name,
            Func<ResolveFieldContext<TSource>, IQueryable<TReturn>> resolve,
            Type graphType = null,
            IEnumerable<QueryArgument> arguments = null,
            int pageSize = 10)
            where TReturn : class;

        void AddQueryConnectionField<TSource, TReturn>(ObjectGraphType<TSource> graph,
            string name,
            Func<ResolveFieldContext<TSource>, IQueryable<TReturn>> resolve,
            Type graphType = null,
            IEnumerable<QueryArgument> arguments = null,
            int pageSize = 10)
            where TReturn : class;
    }
}