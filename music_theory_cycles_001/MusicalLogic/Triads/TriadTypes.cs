using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace music_theory_cycles_001
{ 
    static public class TriadTypes
    {
        static public int[] GetChordFormula(string name)
        {
            switch (name)
            { 
                case "Maj":
                    return TriadsTypes["Maj"]; 
                case "Min": 
                    return TriadsTypes["Min"];
                case "Dim":
                    return TriadsTypes["Dim"];
                case "Aug":
                    return TriadsTypes["Aug"];
                default:
                    return null;
            }
        }

        static public Dictionary<string, int[]> TriadsTypes = new Dictionary<string, int[]>()
        {
            { "Maj",     new int[] { 4, 3, 5 } },
            { "Min",     new int[] { 3, 4, 5 } },
            { "Dim",     new int[] { 3, 3, 6 } },
            { "Aug",     new int[] { 4, 4, 4 } },
        };

        /// <summary>
        /// A map describing the relation between a chord-Mode and the base order of chord steps. The base case: modeI I-III-V-VII
        /// </summary>
        static public Dictionary<TriadMode, int[]> TriadModeFormula = new Dictionary<TriadMode, int[]>()
        {
            {   TriadMode.I,     new int[] {0,1,2} },
            {   TriadMode.II,    new int[] {0,2,1} }
        };


        public enum TriadMode
        {
            I,
            II
        }

        public enum TriadInversion
        {
            I,
            II,
            III
        }
    }

}
