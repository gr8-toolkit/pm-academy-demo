using System;
using System.Threading;
using System.Threading.Tasks;
using School.Services;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace School.Tests.Unit
{

    public class MessageTests
    {
        [Fact]
        public void FromDomainEvent_Should_Create_Message()
        {
            var @event = new FakeEvent() { Text = Guid.NewGuid().ToString() };

            var eventSerializer = Substitute.For<IEventSerializer>();
            var expectedPayload = $"{@event.Text}";
            eventSerializer.Serialize(@event)
                .Returns(expectedPayload);

            var sut = Message.FromDomainEvent(@event, eventSerializer);
            sut.Should().NotBeNull();
            sut.Payload.Should().Be(expectedPayload);
            sut.ProcessedAt.Should().BeNull();
            sut.Type.Should().Be(typeof(FakeEvent).FullName);
        }

        [Fact]
        public async Task Process_Should_Publish_Message()
        {
            var @event = new FakeEvent() { Text = Guid.NewGuid().ToString() };

            var eventSerializer = Substitute.For<IEventSerializer>();
            
            var sut = Message.FromDomainEvent(@event, eventSerializer);

            sut.ProcessedAt.Should().BeNull();

            var publisher = Substitute.For<IMessagePublisher>();

            await sut.Process(publisher, CancellationToken.None);

            await publisher.Received(1).PublishAsync(sut, CancellationToken.None);

            sut.ProcessedAt.Should().NotBeNull();
        }
    }

    internal class FakeEvent : IDomainEvent {
        public string Text;
    }
}