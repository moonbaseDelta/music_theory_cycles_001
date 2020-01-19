using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace music_theory_cycles_001
{ 
    public static class TriadTransformation
    {
        public static int[] GetATransformation(Triad triad1, Triad triad2)
        {
            var notes1 = triad1.ChordNotes.OrderBy(n => n.getNoteNumber()).ToArray();
            var notes2 = triad2.ChordNotes.OrderBy(n => n.getNoteNumber()).ToArray();

            var aTransformation = new int[3];
            var aTransformation2 = new int[3];

            for (int i = 0; i < 3; i++)
                aTransformation2[i] = notes2[i].getNoteNumber() - notes1[i].getNoteNumber();

            var indexes = new int[3] { 0, 0, 0 };
            for (int i = 0; i < 3; i++)
            {
                int ind2 = 0;
                while (triad1.ChordNotes[i] != notes1[ind2])
                {
                    ind2++;
                    indexes[i]++;
                }
            }

            aTransformation[0] = aTransformation2[indexes[0]];
            aTransformation[1] = aTransformation2[indexes[1]];
            aTransformation[2] = aTransformation2[indexes[2]]; 

            if (aTransformation[0] > 0)
                while (aTransformation[0] > 0)
                    for (int i = 0; i < 3; i++)
                        aTransformation[i] -= 1;
            else if (aTransformation[0] < 0)
                while (aTransformation[0] < 0)
                    for (int i = 0; i < 3; i++)
                        aTransformation[i] += 1;

            return aTransformation;
        }

        public static bool ApplyATransformation(Triad triad, int[] transformation, int transpose = 0)
        {
            triad.TriadType = Triad.DetermineTriadType(triad);
            var mode = TriadTypes.TriadModeFormula[triad.Mode];

            triad.ChordNotes[0] = JustNote.moveNoteBySemitones(triad.ChordNotes[0], transformation[0] + transpose);
            triad.ChordNotes[1] = JustNote.moveNoteBySemitones(triad.ChordNotes[1], transformation[1] + transpose);
            triad.ChordNotes[2] = JustNote.moveNoteBySemitones(triad.ChordNotes[2], transformation[2] + transpose);

            triad.TriadType = Triad.DetermineTriadType(triad);
            if (triad.TriadType == "undefined")
                return false;

            try
            {
                Triad.ResetTriadRootStepsModeAndInversion(triad);
                if (triad.ChordNotes.All(cn => cn.unboundOctave > 19 || cn.unboundOctave < -10))
                    return false;
                else
                    return true;
            }
            catch (Exception) { return false; }
        }
    }
}
