using System.Collections.Generic;
using System.IO;
using System.Text;
using Estudos.RemoteConfigurationProvider.Helpers;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Estudos.RemoteConfigurationProvider.Tests.Tests.Helpers
{
    public class ParseHelperTest
    {
        private readonly string JsonContent = @"
             {                  
                ""REMOTE_APPSETTINGS"": ""Public"",
                ""COMPLEX"": {
                    ""ITEM1"": 1,                        
                    ""ITEM2"": [1, 2, 3],
                    ""ITEM3"": [""a"", ""b"", ""c""]
                },                   
                ""BOOL"": true
            }       
        ";

        [Fact]
        public void Should_Parse_JObject()
        {
            JObject jObj = JsonConvert.DeserializeObject<JObject>(JsonContent);
            ValidateResult(ParseHelper.Parse(jObj));
        }
        
        [Fact]
        public void Should_Parse_Stream()
        {
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(JsonContent));
            ValidateResult(ParseHelper.Parse(stream));
            
        }

        private void ValidateResult(IDictionary<string, string> result)
        {
            result["REMOTE_APPSETTINGS"].Should().Be("Public");
            result["COMPLEX:ITEM1"].Should().Be("1");
            result["COMPLEX:ITEM2:0"].Should().Be("1");
            result["COMPLEX:ITEM2:1"].Should().Be("2");
            result["COMPLEX:ITEM2:2"].Should().Be("3");
            result["COMPLEX:ITEM3:0"].Should().Be("a");
            result["COMPLEX:ITEM3:1"].Should().Be("b");
            result["COMPLEX:ITEM3:2"].Should().Be("c");
            result["BOOL"].Should().Be("True");
        }
    }
}