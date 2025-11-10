using System.ComponentModel.Design.Serialization;

namespace BPlusOne
{
    public class Node
    {
        private const int MaxDegree = 40;

        public bool IsLeaf { get; set; }
        public int Size { get; set; }      // Number of Active Keys (Non-Empty).
        public int[] Key { get; set; }
        public Node[] Child { get; set; }

        public Node()
        {
            Key = new int[MaxDegree];
            Child = new Node[MaxDegree + 1];    // +1 for Children
            IsLeaf = false;
            Size = 0;
        }

        public static int GetHeight(Node n)
        {
            if (n == null) 
            { 
                return 0; 
            }
            int max = 0;
            for (int i = 0; i < n.Size; i++)
            {
                Node a = n.Child[i];
                if (a == null)
                    continue;
                int ht = Node.GetHeight(a);
                if (ht > max)
                {
                    max = ht;
                }
            }
            return max + 1;
        }

        public static int GetNodeCount(Node n)
        {
            if (n == null) 
            { 
                return 0; 
            }
            int result = 0;
            for (int i = 0; i < n.Size; i++)
            {
                Node a = n.Child[i];
                if (a == null)
                    continue;
                result += Node.GetNodeCount(a);
            }
            return result + 1;
        }

        public static List<int> GetData(Node node)
        {
            if (node == null)
            {
                return new List<int>();
            }

            List<int> data = new List<int>();
            GetDataInOrder(node, data);
            return data;
        }

        private static void GetDataInOrder(Node node, List<int> data)
        {
            if (node == null)
            {
                return;
            }

            if (node.IsLeaf)
            {
                for (int i = 0; i < node.Size; i++)
                {
                    data.Add(node.Key[i]);
                }
            }
            else
            {
                for (int i = 0; i <= node.Size; i++)
                {
                    GetDataInOrder(node.Child[i], data);
                }
            }
        }

        public static void WriteToStream(Node node, StreamWriter sw)
        {
            if (node == null)
            {
                return;
            }

            if (node.IsLeaf)
            {
                for (int i = 0; i < node.Size; i++)
                {
                    sw.WriteLine(node.Key[i]);
                }
            }
            else
            {
                for (int i = 0; i <= node.Size; i++)
                {
                    WriteToStream(node.Child[i], sw);
                }
            }
        }

        public void Clear(Node n)
        {
            if (n == null)
            {
                return;
            }
            foreach (var c in n.Child)
            {
                Clear(c);
            }
            Array.Clear(n.Child);
            Array.Clear(n.Key);
        }



    }
}
