using System.Collections.Generic;
using GraphQL.Query.Builder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GraphQl_VFCM.models
{
    public static class GraphqlHelper
    {
        public static IQuery<R> AddQueryParameters<R,P>(string operation, string alias,P parametersModel,List<string> fields)
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

        public static string BuildQueryString(this string query)
        {
            var splittedQuery = query.Split(':',2);
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