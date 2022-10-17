using API.Model;
using API.Serialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITests;

public class ConsumeMessagesKafkaData
{
   static CustomSerializer customSerializer = new CustomSerializer();
    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { 0, 0, customSerializer.JsonConvertSerilize(new List<ResponseKafkaMessagesModel>()).Result },
            new object[] { 0, 1, customSerializer.JsonConvertSerilize(new List<ResponseKafkaMessagesModel>()).Result },
            new object[] { 1, 0, customSerializer.JsonConvertSerilize(new List<ResponseKafkaMessagesModel>()).Result },
            new object[] { 1, 1, customSerializer.JsonConvertSerilize(new List<ResponseKafkaMessagesModel> { new ResponseKafkaMessagesModel(0, "0") }).Result },
            new object[] { 3, 0, customSerializer.JsonConvertSerilize(new List<ResponseKafkaMessagesModel>()).Result },
            new object[] { 3, 1, customSerializer.JsonConvertSerilize(new List<ResponseKafkaMessagesModel> { new ResponseKafkaMessagesModel(2, "2") }).Result },
            new object[] { 3, 4, customSerializer.JsonConvertSerilize(new List<ResponseKafkaMessagesModel> 
            { new ResponseKafkaMessagesModel(0, "0"), new ResponseKafkaMessagesModel(1, "1"), new ResponseKafkaMessagesModel(2, "2") }).Result },

        };
};
