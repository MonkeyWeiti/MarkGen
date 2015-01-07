using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarkGen.Generation;
using NUnit.Framework;

namespace MarkGen.UnitTest
{
    public class Class1
    {
        [Test]
        public void TestStuff() {
            var g = new GalleryGenerator();
            g.Generate("Castrum Plaviense", @"C:\MarkGen\Reychsausritt\Castrum Plaviense");
            g.Generate("Vimaria", @"C:\MarkGen\Reychsausritt\Vimaria");
        }
    }
}
