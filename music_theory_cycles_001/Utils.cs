using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace music_theory_cycles_001
{   
    public static class Utils
    {
        public static Random FairRandom { get; set; }  
    }

    public class IntArrayComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[] x, int[] y)
        {
            bool ans = true;

            if (x.Length != y.Length) return false;

            for (int i = 0; i < x.Length; i++)
                ans = ans && x[i] == y[i];

            return ans;
        }

        public int GetHashCode(int[] obj)
        {
            StringBuilder hashseed = new StringBuilder();
            for (int i = 0; i < obj.Length; i++)
                hashseed.Append(obj[i].ToString());
            return (hashseed.ToString()).GetHashCode();
        }
    }
}
