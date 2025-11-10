using System.Diagnostics;

namespace BPlusOne
{
    public class Util
    {
        private static Random rnd = new Random();

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

        public static int GetMemory()
        {
            Process currentProcess = Process.GetCurrentProcess();
            long workingSet = currentProcess.PeakWorkingSet64 / (1024 * 1024);
            return Convert.ToInt32(workingSet);
        }

    }
}
