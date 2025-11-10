using BPlusOne;

namespace UnitTest
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestOne()
        {
            BPTree t = new BPTree();

            int maxSize = 1000;

            DateTime dtBTreeStart = DateTime.Now;

            for (int i = 0; i < maxSize; i++)
            {
                t.Insert(i);
            }

            DateTime dtBTreeEnd = DateTime.Now;

            Console.WriteLine("insert {0}", dtBTreeEnd - dtBTreeStart);

            Console.WriteLine("height " + t.GetHeight());

            dtBTreeStart = DateTime.Now;

            for (int i = 0; i < maxSize; i++)
            {
                Assert.IsTrue(t.Exist(i));
            }

            dtBTreeEnd = DateTime.Now;

            Console.WriteLine("exist {0}", dtBTreeEnd - dtBTreeStart);

            dtBTreeStart = DateTime.Now;

            for (int i = 0; i < maxSize; i++)
            {
                t.Remove(i);
            }

            dtBTreeEnd = DateTime.Now;

            Console.WriteLine("remove {0}", dtBTreeEnd - dtBTreeStart);

            for (int i = 0; i < maxSize; i++)
            {
                Assert.IsFalse(t.Exist(i));
            }

            t.Clear();
        }

        [TestMethod]
        public void TestTwo()
        {
            Random rnd = new Random();

            List<int> a = new List<int>();

            for (int i = 0; i < 1000; i++)
            {
                a.Add(rnd.Next(1000000));
            }

            a = a.Distinct().ToList();

            Console.WriteLine("count " + a.Count);

            BPTree t = new BPTree();

            DateTime dtBTreeStart = DateTime.Now;

            for (int i = 0; i < a.Count; i++)
            {
                t.Insert(a[i]);
            }

            DateTime dtBTreeEnd = DateTime.Now;

            Console.WriteLine("insert {0}", dtBTreeEnd - dtBTreeStart);

            Console.WriteLine("height " + t.GetHeight());

            dtBTreeStart = DateTime.Now;

            for (int i = 0; i < a.Count; i++)
            {
                Assert.IsTrue(t.Exist(a[i]));
            }

            dtBTreeEnd = DateTime.Now;

            Console.WriteLine("exist {0}", dtBTreeEnd - dtBTreeStart);

            Util.Shuffle(a);

            dtBTreeStart = DateTime.Now;

            for (int i = 0; i < a.Count; i++)
            {
                t.Remove(a[i]);
            }

            dtBTreeEnd = DateTime.Now;

            Console.WriteLine("remove {0}", dtBTreeEnd - dtBTreeStart);

            for (int i = 0; i < a.Count; i++)
            {
                Assert.IsFalse(t.Exist(a[i]));
            }

            t.Clear();
        }

        [TestMethod]
        public void TestThree()
        {
            int maxSize = 1000;

            BPTree t = new BPTree();

            for (int i = 0; i < maxSize; i++)
            {
                t.Insert(i);
            }

            List<int> b = t.GetData();

            Assert.AreEqual(maxSize, b.Count);
            Assert.IsTrue(Util.IsSorted(b));
            t.Clear();
        }

        [TestMethod]
        public void TestFour()
        {
            Random rnd = new Random();

            List<int> a = new List<int>();

            for (int i = 0; i < 1000; i++)
            {
                a.Add(rnd.Next(1000000));
            }

            a = a.Distinct().ToList();

            Console.WriteLine("count " + a.Count);

            BPTree t = new BPTree();

            for (int i = 0; i < a.Count; i++)
            {
                t.Insert(a[i]);
            }

            List<int> b = t.GetData();

            Assert.AreEqual(a.Count, b.Count);
            Assert.IsTrue(Util.IsSorted(b));
            t.Clear();
        }
    }
}
