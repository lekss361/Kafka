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
        public void PublishMessageAsync_ShouldPublishMessage()
        {
            //Arrange
            _producerAccessorMock = new Mock<IProducerAccessor>();
            _kafkaMessagePublisher = new MessagePublisherService(_producerAccessorMock.Object);
            string topic = "sample-topic";
            _producerAccessorMock.Setup(p => p.GetProducer(topic).ProduceAsync(topic,null, topic, null)).ReturnsAsync(new Confluent.Kafka.DeliveryResult<byte[], byte[]>());
            var expected = "Value cannot be null. (Parameter 'no topic for System.Func`1[System.String]')";

            //Act
            var actual =  _kafkaMessagePublisher.PublishMessageAsync(It.IsAny<string>, topic);

            //Assert
            _producerAccessorMock.Verify(p=>p.GetProducer(topic), Times.Once());

        }
        
        [Fact]
        public void PublishMessageAsync_ProducerNotFound_ShouldArgumentNullException()
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