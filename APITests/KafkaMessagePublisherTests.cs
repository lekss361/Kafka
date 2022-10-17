using API.Services;
using KafkaFlow.Producers;
using Moq;

namespace APITests
{
    public class KafkaMessagePublisherTests
    {
        private Mock<IProducerAccessor> _producerAccessorMock;
        private MessagePublisherService _kafkaMessagePublisher;

        [Fact]
        public void GetMessagesTests_Arg()
        {
            //Arrange
            _producerAccessorMock = new Mock<IProducerAccessor>();
            _kafkaMessagePublisher = new MessagePublisherService(_producerAccessorMock.Object);
            string topic = null;
            var expected = "Value cannot be null. (Parameter 'no topic for System.Func`1[System.String]')";

            //Act
            var actual = Assert.ThrowsAsync<ArgumentNullException>(async () => await _kafkaMessagePublisher.PublishMessageAsync(It.IsAny<string>, topic));


            //Assert
            Assert.Equal(expected, actual.Result.Message);

        }
        
        [Fact]
        public void GetMessagesTests_ShouldListsMessages()
        {
            //Arrange
            _producerAccessorMock = new Mock<IProducerAccessor>();
            _kafkaMessagePublisher = new MessagePublisherService(_producerAccessorMock.Object);
            string topic = "";
            var expected = "Value cannot be null. (Parameter 'no producer for System.Func`1[System.String]')";

            //Act
            var actual = Assert.ThrowsAsync<ArgumentNullException>(async () => await _kafkaMessagePublisher.PublishMessageAsync(It.IsAny<string>, topic));


            //Assert
            Assert.Equal(expected, actual.Result.Message);

        }
    }
}