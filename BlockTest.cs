using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    [TestFixture]
    internal class BlockTest
    {
        [Test]
        [TestCase("100")]
        public void DobriParamteri(string id)
        {
            Block b = new Block(id);
            Assert.AreEqual(b.ID, id);
           

        }

        [Test]
        [TestCase(null)]
        public void TestPada(string id)
        {

            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    Block b = new Block(id);
                });
        }

        [Test]
        [TestCase("0")]
        public void TestGranicni(string id)
        {
            Block b = new Block(id);
            Assert.AreEqual(b.ID, id);
        }
    }
}
