# GraphQLDemoWithHotChocolate
Simple project with the basic implementation of almost all concepts of hotchocolate graphQL.
Whats implemented?
  1. Queries
  2. Mutations
  3. Subscriptions
  4. Dataloader
  5. Pagination
  6. Filtering with custom filter
  7. Sorting with custom sorter
  8. Projection
  9. JWT Authentication and Authorization - ongoing
  10. Code Cleanup & Folder structure - pending until project fully finish
  11. Firebase login - pending for now.
  
# [NOTE]
There's not much difference between RestAPI and GraphQL API. Main difference is following:
  1. Rest -> Controller = GraphQL -> Mutation/Queries/Subscription
  2. Single endpoint "/graphql", and request be like query/mutation {functionName (input: optional) {return field(s)}}.
