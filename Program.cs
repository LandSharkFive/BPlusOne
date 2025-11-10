namespace BPlusOne
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TestOne();
        }

        static void TestOne()
        {
            int maxSize = 1000;
            BPTree t = new BPTree();
            for (int i = 0; i < maxSize; i++)
            {
                t.Insert(i);
            }

            for (int i = 0; i < maxSize; i++)
            {
                bool found = t.Exist(i);
                if (!found)
                {
                    Console.WriteLine("{0} not found", i);
                }
            }

            Console.WriteLine("height " + t.GetHeight());
            Console.WriteLine("node " + t.GetNodeCount());

            List<int> a = t.GetData();
            Console.WriteLine("count " + a.Count);
            Console.WriteLine("sorted " + Util.IsSorted(a));
            Console.WriteLine("memory {0} mb", Util.GetMemory());

            t.Display();

            for (int i = 0; i < maxSize; i++)
            {
                t.Remove(i);
            }

            for (int i = 0; i < maxSize; i++)
            {
                bool found = t.Exist(i);
                if (found)
                {
                    Console.WriteLine("{0} found", i);
                }
            }

            t.Clear();
        }

    }
}
