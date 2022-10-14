using API.Controllers;
using KafkaFlow;
using KafkaFlow.Producers;
using Moq;
using Newtonsoft.Json.Linq;
using System.Data;


namespace API.Tests
{
    public class KafkaMessageControllerTest
    {
        private Mock<IProducerAccessor> _producerAccessor;
        private KafkaMessagePublisher _publisher;

        

        [SetUp]
        public void Setup()
        {
            _producerAccessor = new Mock<IProducerAccessor>();
            _publisher = new KafkaMessagePublisher(_producerAccessor.Object);
        }

        #region PublishMessageTests
        [Test]
        public void PublishMessage_ShouldAddKafkaMessage()
        {
            var message = "string";
            var topic = "sample-topic";
            IMessageProducer messageProducer;


            var actual = Assert.ThrowsAsync<ArgumentNullException>(async () => await _publisher.PublishMessageAsync(message, topic))!
                .Message;


            //Assert.AreEqual(, actual);
        }

        [Test]
        public void PublishMessage_TopicIsNull_ShouldArgumentNullException()
        {
            string topicName = null;
            var expected = "Value cannot be null. (Parameter 'no topic for System.String')";

            var actual = Assert.ThrowsAsync<ArgumentNullException>(async () => await _publisher.PublishMessageAsync(It.IsAny<string>(), topicName))!
                .Message;


            Assert.AreEqual(expected,actual);
        }

        [Test]
        public void PublishMessage_TopicIsNotUsd_ShouldArgumentNullException()
        {
            string topicName = "";
            var expected = "Value cannot be null. (Parameter 'no producer for System.String')";

            var actual = Assert.ThrowsAsync<ArgumentNullException>(async () => await _publisher.PublishMessageAsync(It.IsAny<string>(), topicName))!
                .Message;


            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}