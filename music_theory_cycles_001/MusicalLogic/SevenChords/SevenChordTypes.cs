using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace music_theory_cycles_001
{ 
    static public class SevenChordTypes
    {
        static public int[] GetChordFormula(string name)
        {
            switch (name)
            {
                case "Δ":
                case "Maj7":
                    return SevenchordTypes["Δ"];
                case "Δ+":
                case "Maj7+":
                    return SevenchordTypes["Δ+"];
                case "Δ-":
                case "Maj7-":
                    return SevenchordTypes["Δ-"];
                case "m7":
                case "Min7":
                    return SevenchordTypes["m7"];
                case "m7+":
                case "Min7+":
                    return SevenchordTypes["m7+"];
                case "Ø":
                case "Min7-":
                    return SevenchordTypes["Ø"];
                case "7":
                case "Dom7":
                    return SevenchordTypes["7"];
                case "7+":
                case "Dom7+":
                    return SevenchordTypes["7+"];
                case "7-":
                case "Dom7-":
                    return SevenchordTypes["7-"];
                case "mΔ7":
                case "MinMaj7":
                    return SevenchordTypes["mΔ7"];
                case "mΔ7+":
                case "MinMaj7+":
                    return SevenchordTypes["mΔ7+"];
                case "mΔ7-":
                case "MinMaj7-":
                    return SevenchordTypes["mΔ7-"];
                case "⃝":
                case "Dim7":
                    return SevenchordTypes["⃝"];
                default:
                    return null;
            }
        }

        static public Dictionary<string, int[]> SevenchordTypes = new Dictionary<string, int[]>()
        {
            { "Δ",      new int[] { 4, 3, 4, 1 } },
            { "Δ+",     new int[] { 4, 4, 3, 1 } },
            { "Δ-",     new int[] { 4, 2, 5, 1 } },
            { "m7",     new int[] { 3, 4, 3, 2 } },
            { "m7+",    new int[] { 3, 5, 2, 2 } },
            { "Ø",      new int[] { 3, 3, 4, 2 } },
            { "7",      new int[] { 4, 3, 3, 2 } },
            { "7+",     new int[] { 4, 4, 2, 2 } },
            { "7-",     new int[] { 4, 2, 4, 2 } },
            { "mΔ7",    new int[] { 3, 4, 4, 1 } },
            { "mΔ7+",   new int[] { 3, 4, 3, 1 } },
            { "mΔ7-",   new int[] { 3, 3, 5, 1 } },
            { "⃝",      new int[] { 3, 3, 3, 3 } },
        };

        /// <summary>
        /// A map describing the relation between a chord-Mode and the base order of chord steps. The base case: modeI I-III-V-VII
        /// </summary>
        static public Dictionary<SevenChordMode, int[]> SevenChordModeFormula = new Dictionary<SevenChordMode, int[]>()
        {
            {   SevenChordMode.I,     new int[] {0,1,2,3}    },
            {   SevenChordMode.II,    new int[] {0,2,1,3}    },
            {   SevenChordMode.III,   new int[] {0,1,3,2}    },
            {   SevenChordMode.IV,    new int[] {0,2,3,1}    },
            {   SevenChordMode.V,     new int[] {0,3,1,2}    },
            {   SevenChordMode.VI,    new int[] {0,3,2,1}    }
        };


        public enum SevenChordMode
        {
            I,
            II,
            III,
            IV,
            V,
            VI
        } 

        public enum SevenChordInversion
        {
            I,
            II,
            III,
            IV
        }
    } 
}
