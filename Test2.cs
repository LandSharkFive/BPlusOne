using BPlusOne;

namespace UnitTest
{
    [TestClass]
    public sealed class Test2
    {
        [TestMethod]
        public void TestFive()
        {
            int maxSize = 1000;

            BPTree a = new BPTree();

            DateTime dtBTreeStart = DateTime.Now;

            for (int i = 0; i < maxSize; i++)
            {
                a.Insert(i);
            }

            a.WriteToFile("a.txt");

            BPTree b = new BPTree();

            b.ReadFile("a.txt");

            for (int i = 0; i < maxSize; i++)
            {
                Assert.IsTrue(b.Exist(i));
            }

            Console.WriteLine("height " + a.GetHeight());
            Console.WriteLine("height " + b.GetHeight());

            a.Clear();
            b.Clear();
        }

    }
}
