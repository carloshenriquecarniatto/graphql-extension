using System;
using GraphQL_Extension.Helpers;
using GraphQL_Extension.models;
using GraphQl_VFCM.models;
using Xunit;
using Xunit.Abstractions;

namespace GraphQL_Extension
{
    public class GraphqlQueryBuilder
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly SimpleModelParameters _simpeopleModel =
            new SimpleModelParameters("Josh",20,'M');
        public GraphqlQueryBuilder(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData("","")]
        [InlineData("","GenerateTokenFromPNO")]
        [InlineData("tokenGeneratorQuery","")]
        public void When_operation_or_alias_is_empty_throw_expection_argument_exception(string operation,string alias)
        {  
            Action querytest = () =>
            {
                var query = GraphqlExtension.AddQueryParameters<SimpleModelFields, SimpleModelParameters>(operation,
                    alias, _simpeopleModel);
                query.AddField(c => c.FullName);
                query.AddField(c => c.Gender);
                query.AddField(c => c.Age);
                var queryString = query.BuildQueryString();
            };
            var ex = Record.Exception(querytest);
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
        }
        [Theory]
        [InlineData(null,null)]
        [InlineData(null,"GenerateTokenFromPNO")]
        [InlineData("tokenGeneratorQuery",null)]
        public void When_operation_or_alias_is_null_throw_expection_argument_null_exception(string operation,string alias)
        {
            Action querytest = () =>
            {
                var query = GraphqlExtension.AddQueryParameters<SimpleModelFields, SimpleModelParameters>(operation,
                    alias, _simpeopleModel);
                query.AddField(c => c.FullName);
                query.AddField(c => c.Gender);
                query.AddField(c => c.Age);
                var queryString = query.BuildQueryString();
            };
            var ex = Record.Exception(querytest);
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory]
        [InlineData("tokenGeneratorQuery","GenerateTokenFromPNO")]
        public void Gived_a_complex_parameters_model(string alias,string operation)
        {
            var queryArguments = new ComplexModelParameters("Jakson", 10);
            queryArguments.Address = new Address("My street 2", "Frankfurt", "Germany");
            var query = GraphqlExtension.AddQueryParameters<ComplexModelParameters, ComplexModelParameters>(operation,
                alias, queryArguments);
            query.AddField(c => c.Age);
            query.AddField(c => c.Name);
            var queryString = query.BuildQueryString();
            _testOutputHelper.WriteLine(queryString);
            Assert.NotNull(queryString);;
        }
        
        [Theory]
        [InlineData("Test1","peoplesByGender")]
        public void When_need_a_sub_query(string alias,string operation)
        {
            var queryArguments = new ComplexModelParameters("Jakson", 10);
            queryArguments.Address = new Address("My street 2", "Frankfurt", "Germany");
            var query = GraphqlExtension.AddQueryParameters<ComplexModelParameters, ComplexModelParameters>(operation,
                alias, queryArguments);
            query.AddField(c => c.Age);
            query.AddField(c => c.Name);
            query.AddField(c => c.Address, addresssubquery =>
            {
                addresssubquery.AddField(c => c.Street);
                addresssubquery.AddField(c => c.City);
                addresssubquery.AddField(c => c.Country);
                return addresssubquery;
            });
            var queryString = query.BuildQueryString();
            _testOutputHelper.WriteLine(queryString);
            Assert.NotNull(queryString);;
        }
    }
    
}