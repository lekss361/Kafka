using API.Extensions;
using API.Model;

namespace APITests;

public class ConsumeMessagesKafkaTests
{
    
    //[Theory]
    //[MemberData(nameof(ConsumeMessagesKafkaData.Data), MemberType= typeof(ConsumeMessagesKafkaData))]
    //public async Task PrintLastMessages_ShouldPrintList(int ArrangeDataList, int CountPrintData, string expected)
    //{
    //    //Arrange
    //    for (int i = 0; i < ArrangeDataList; i++)
    //    {
    //    ConsumeMassagesKafka.messagesContexts.Add(new ResponseKafkaMessagesModel(i,$"{i}" ));
    //    }

    //    //Act
    //    var actual = ConsumeMassagesKafka.PrintLastMessages(CountPrintData);

    //    //Assert
    //    Assert.Equal(expected, actual.Result);
    //    ConsumeMassagesKafka.messagesContexts.Clear();
    //}

    //[Theory]
    //[InlineData(-1)]
    //public async Task PrintLastMessages_ShouldArgumentOutOfRangeException( int CountPrintData)
    //{
    //    //Arrange
    //    var expected = "Specified argument was out of the range of valid values. (Parameter 'Count<0')";

    //    //Act
    //    var actual = Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await ConsumeMassagesKafka.PrintLastMessages(CountPrintData));

    //    //Assert
    //    Assert.Equal(expected, actual.Result.Message);
    //}
}
