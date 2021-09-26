using System.Collections.Generic;
using Xunit;
using Elephant.Model;
using Elephant.Services;

namespace ElephantTest.Services
{
    public class XXFileUnitTest
    {
        [Fact]
        public void UCNFileTest()
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
            UCNFile xXFile = new(filePath);
            List<TDCTag> actual = xXFile.GetTagsList();

            // assert
            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public void CLAMFileTest()
        {
            // expected
            List<TDCTag> expected = new List<TDCTag>();
            TDCTag t1 = new() { Name = "name1", Origin = "CL", Parameter = "CL AM", Value = "value1" };
            TDCTag t2 = new() { Name = "name2", Origin = "CL", Parameter = "CL AM", Value = "value2" };
            TDCTag t3 = new() { Name = "name3", Origin = "CL", Parameter = "CL AM", Value = "value3" };
            TDCTag t4 = new() { Name = "name4", Origin = "CL", Parameter = "CL AM", Value = "value4" };
            TDCTag t5 = new() { Name = "name5", Origin = "CL", Parameter = "CL AM", Value = "value5" };
            expected.Add(t1);
            expected.Add(t2);
            expected.Add(t3);
            expected.Add(t4);
            expected.Add(t5);

            // actual
            string filePath = "..\\..\\..\\FilesTest\\CLAMABCD.XX";
            UCNFile xXFile = new(filePath);
            List<TDCTag> actual = xXFile.GetTagsList();

            // assert
            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public void HWYFileTest()
        {
            // expected
            List<TDCTag> expected = new List<TDCTag>();
            TDCTag t1 = new() { Name = "name1", Origin = "HIWAY", Parameter = "ENT REF", Value = "value1" };
            TDCTag t2 = new() { Name = "name2", Origin = "HIWAY", Parameter = "ENT REF", Value = "value2" };
            TDCTag t3 = new() { Name = "name3", Origin = "HIWAY", Parameter = "ENT REF", Value = "value3" };
            TDCTag t4 = new() { Name = "name4", Origin = "HIWAY", Parameter = "ENT REF", Value = "--" };
            expected.Add(t1);
            expected.Add(t2);
            expected.Add(t3);
            expected.Add(t4);

            // actual
            string filePath = "..\\..\\..\\FilesTest\\HIWAY.XX";
            UCNFile xXFile = new(filePath);
            List<TDCTag> actual = xXFile.GetTagsList();

            // assert
            Assert.Equal(expected.ToString(), actual.ToString());
        }
    }
}
