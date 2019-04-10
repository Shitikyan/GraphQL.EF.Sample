using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using WebApplication1.DataContexts;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GraphQlController
    {
        IDocumentExecuter executer;
        ISchema schema;

        public GraphQlController(ISchema schema, IDocumentExecuter executer)
        {
            this.schema = schema;
            this.executer = executer;
        }

        [HttpPost]
        public Task<ExecutionResult> Post(
       [BindRequired, FromBody] PostBody body,
       [FromServices] BookDbContext bookContext,
       [FromServices] AuthorDbContext authorContext,
       CancellationToken cancellation)
        {
            return Execute(bookContext, authorContext, body.Query, body.OperationName, body.Variables, cancellation);
        }

          public class PostBody
        {
            public string OperationName;
            public string Query;
            public JObject Variables;
        }

        [HttpGet]
        public Task<ExecutionResult> Get(
       [FromQuery] string query,
       [FromQuery] string variables,
       [FromQuery] string operationName,
       [FromServices] BookDbContext bookDbContext,
       [FromServices] AuthorDbContext authorDbContext,
       CancellationToken cancellation)
        {
            var jObject = ParseVariables(variables);
            return Execute(bookDbContext, authorDbContext, query, operationName, jObject, cancellation);
        }
        async Task<ExecutionResult> Execute(BookDbContext bookDbContext,
                    AuthorDbContext authorDbContext,
                                            string query,
                                            string operationName,
                                            JObject variables,
                                            CancellationToken cancellation)
        {
            var options = new ExecutionOptions
            {
                Schema = schema,
                Query = query,
                OperationName = operationName,
                Inputs = variables?.ToInputs(),
                UserContext = new DataContexts.DataContexts { authorDbContext = authorDbContext, bookDbContext = bookDbContext},

                CancellationToken = cancellation,
#if (DEBUG)
                ExposeExceptions = true,
                EnableMetrics = true,
#endif
            };

            var result = await executer.ExecuteAsync(options);

            if (result.Errors?.Count > 0)
            {

            }

            return result;
        }

        static JObject ParseVariables(string variables)
        {
            if (variables == null)
            {
                return null;
            }

            try
            {
                return JObject.Parse(variables);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not parse variables.", exception);
            }
        }

    }
}
