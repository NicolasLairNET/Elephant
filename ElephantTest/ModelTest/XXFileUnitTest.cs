using System.Collections.Generic;
using Xunit;
using Elephant.Model;
using Elephant.Services;

namespace ElephantTest.ModelTest
{
    public class XXFileUnitTest
    {
        [Fact]
        public void TestUCNFile()
        {
            // expected
            List<TDCTag> expected = new List<TDCTag>();
            TDCTag t1 = new() { Name = "name1", Origin = "UCN", Parameter = "ENT_REF", Value = "value1" };
            TDCTag t2 = new() { Name = "name2", Origin = "UCN", Parameter = "ENT_REF", Value = "value2" };
            TDCTag t3 = new() { Name = "name3122323", Origin = "UCN", Parameter = "ENT_REF", Value = "value3122323" };
            expected.Add(t1);
            expected.Add(t2);
            expected.Add(t3);

            // actual
            string filePath = "..\\..\\..\\FilesTest\\UCN.XX";
            XXFile xXFile = new(filePath);
            List<TDCTag> actual = xXFile.Read();

            // assert
            Assert.Equal(expected[0].Name, actual[0].Name);
            Assert.Equal(expected[0].Origin, actual[0].Origin);
            Assert.Equal(expected[0].Parameter, actual[0].Parameter);
            Assert.Equal(expected[0].Value, actual[0].Value);

            Assert.Equal(expected[1].Name, actual[1].Name);
            Assert.Equal(expected[1].Origin, actual[1].Origin);
            Assert.Equal(expected[1].Parameter, actual[1].Parameter);
            Assert.Equal(expected[1].Value, actual[1].Value);

            Assert.Equal(expected[2].Name, actual[2].Name);
            Assert.Equal(expected[2].Origin, actual[2].Origin);
            Assert.Equal(expected[2].Parameter, actual[2].Parameter);
            Assert.Equal(expected[2].Value, actual[2].Value);
        }
    }
}
