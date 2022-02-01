using System.Collections.Generic;
using GraphQL.Query.Builder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GraphQL_Extension.Helpers
{
    public static class GraphqlExtension
    {
        /// <summary>
        /// This extension method is used to create queries in Graphql.
        /// </summary>
        /// <param name="operation">This parameter refers to the operation that will be performed</param>
        /// <param name="alias">This is a alias for the operation</param>
        /// <param name="parametersModel">Is the data model that will be used as parameters for the query.</param>
        /// <typeparam name="R">This is the type of the data model that will be used as a parameter for the selected fields in the graphql query.</typeparam>
        /// <typeparam name="P">This is the type of the data model that will be used as a parameter for the query.</typeparam>
        /// <returns>Returns a Query object with the added parameters.</returns>
        public static IQuery<R> AddQueryParameters<R,P>(string operation, string alias,P parametersModel)
            where P : class
            where R :class
        {
            var stringResult = JsonConvert.SerializeObject(parametersModel);
            var jsonDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(stringResult);
            var argsDictionary = GenerateArguments(jsonDic);
            var query = new Query<R>(operation).Alias(alias);
            foreach (var (key,value) in argsDictionary)
            {
                query.AddArgument(key, value);
            }
            return query;
        }

        /// <summary>
        /// This method is needed to convert the query to string.
        /// </summary>
        /// <returns>Return a query string</returns>
        public static string BuildQueryString<T>(this IQuery<T> query) where T : class
        {
            var queryString = query.Build();
            var splittedQuery = queryString.Split(':',2);
            var newQuery = "query " + splittedQuery[0] + "{" + splittedQuery[1] + "}";
            return newQuery;
        }
        private static IDictionary<string,object> GenerateArguments(IDictionary<string, object> dictionary)
        {
            var result = new Dictionary<string,object>();
            foreach (var (key,value) in dictionary)
            {
                switch (value)
                {
                    case JObject _:
                    {
                        var jsonString = JsonConvert.SerializeObject(value);
                        var innerDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
                        result.Add(key,GenerateArguments(innerDictionary));
                        break;
                    }
                    case JArray _:
                    {
                        var jsonString = JsonConvert.SerializeObject(value);
                        var innerDictionary = JsonConvert.DeserializeObject<string[]>(jsonString);
                        result.Add(key,innerDictionary);
                        break;
                    }
                    default:
                        result.Add(key,value);
                        break;
                }
            }
            return result;
        }
    }
}