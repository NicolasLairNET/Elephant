using System.Collections.Generic;
using Xunit;
using Elephant.Services;
using Elephant.Model;

namespace ElephantTest.Services
{
    public class EBFileUnitTest
    {

        [Fact]
        public void EBFileTest()
        {
            // expected
            List<TDCTag> expected = new List<TDCTag>();
            TDCTag t1 = new() { Name = "PP308", Origin = "EB", Parameter = "PNTTYPE", Value = "ANINNIM" };
            TDCTag t2 = new() { Name = "PP308", Origin = "EB", Parameter = "NODETYP", Value = "HPM" };
            TDCTag t3 = new() { Name = "PP308", Origin = "EB", Parameter = "PNTFORM", Value = "FULL" };
            TDCTag t4 = new() { Name = "PP308", Origin = "EB", Parameter = "PTDESC", Value = "PRESSION TETE T306" };
            TDCTag t5 = new() { Name = "PP308", Origin = "EB", Parameter = "EUDESC", Value = "BAR" };
            TDCTag t6 = new() { Name = "PP308", Origin = "EB", Parameter = "KEYWORD", Value = "T306" };

            expected.Add(t1);
            expected.Add(t2);
            expected.Add(t3);
            expected.Add(t4);
            expected.Add(t5);
            expected.Add(t6);

            // actual
            string filePath = "..\\..\\..\\FilesTest\\FICHIER.EB";
            UCNFile xXFile = new(filePath);
            List<TDCTag> actual = xXFile.GetTagsList();

            // assert
            Assert.Equal(expected.ToString(), actual.ToString());
        }
    }
}
