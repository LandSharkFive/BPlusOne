using System.Diagnostics;

namespace BPlusOne
{
    public class Util
    {
        private static Random rnd = new Random();

        /// <summary>
        /// Shuffle the list.
        /// </summary>
        /// <param name="list">List</param>
        public static void Shuffle(List<int> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                int value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Is the list sorted?
        /// </summary>
        /// <param name="a">List</param>
        /// <returns>bool</returns>
        public static bool IsSorted(List<int> a)
        {
            for (int i = 1; i < a.Count; i++)
            {
                if (a[i - 1] > a[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Does list have any duplicates?
        /// </summary>
        /// <param name="source">List</param>
        /// <returns>bool</returns>
        public static bool HasDuplicate(List<int> source)
        {
            var set = new HashSet<int>();
            foreach (var item in source)
            {
                if (!set.Add(item))
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Get memory in megabytes.
        /// </summary>
        /// <returns>int</returns>
        public static int GetMemory()
        {
            Process currentProcess = Process.GetCurrentProcess();
            long workingSet = currentProcess.PeakWorkingSet64 / (1024 * 1024);
            return Convert.ToInt32(workingSet);
        }

    }
}
