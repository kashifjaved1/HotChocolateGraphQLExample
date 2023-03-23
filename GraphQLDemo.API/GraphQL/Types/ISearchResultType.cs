using HotChocolate.Types;
using System;

namespace GraphQLDemo.API.GraphQL.Types
{
    [InterfaceType("SearchResult")] // Use for types which have shared properties/fields, e.g. course & instructor both have shared property 'Id', and that property need to provide with graphQL fragments while quering data and will get data in shared properties/fields within the fragment without explicilty asking in fragments.
    
    // [UnionType("SearchResult")] // Vise versa of InterfaceType.
    public interface ISearchResultType
    {
       Guid Id { get; } // comment it, for testing UnionType.
    }
}
