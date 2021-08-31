using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Elephant.Model;

namespace ElephantTest
{
    public class UnitTest
    {
        [Fact]
        public void Test1()
        {
            XXFile file = new XXFile("","");
            string actual = file.CreateUCNTag("NET>&I05  5      5     HPMM     FL3    GSW003                           --                                                          ").Name;
            string expected = "GSW003";
            Assert.Equal(expected, actual);
        }
    }
}
