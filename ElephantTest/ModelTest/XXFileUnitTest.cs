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
            List<TDCTag> actual = xXFile.GetTagsList();

            // assert
            Assert.Equal(expected.ToString(), actual.ToString());
        }
    }
}
