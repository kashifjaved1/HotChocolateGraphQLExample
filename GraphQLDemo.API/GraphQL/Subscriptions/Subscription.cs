using GraphQLDemo.API.GraphQL.Mutations;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using System;
using System.Threading.Tasks;

namespace GraphQLDemo.API.GraphQL.Subscriptions
{
    public class Subscription
    {
        [Subscribe]
        public CourseResult CourseCreated([EventMessage] CourseResult course) => course;

        [SubscribeAndResolve]
        public ValueTask<ISourceStream<CourseResult>> CourseUpdated(Guid courseId, [Service] ITopicEventReceiver topicEventReceiver)
        {
            string topicName = $"{courseId}_{nameof(Subscription.CourseUpdated)}";

            return topicEventReceiver.SubscribeAsync<string, CourseResult>(topicName);
        }
    }
}
