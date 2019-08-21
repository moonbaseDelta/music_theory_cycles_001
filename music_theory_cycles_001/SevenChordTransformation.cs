using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace music_theory_cycles_001
{

    public static class SevenChordTransformation
    {
        public static int[] GetATransformation(SevenChord chord1, SevenChord chord2)
        {
            var notes1 = chord1.ChordNotes.OrderBy(n => n.getNoteNumber()).ToArray();
            var notes2 = chord2.ChordNotes.OrderBy(n => n.getNoteNumber()).ToArray();

            var aTransformation = new int[4];
            var aTransformation2 = new int[4];

            for (int i = 0; i < 4; i++)
            {
                aTransformation2[i] = notes2[i].getNoteNumber() - notes1[i].getNoteNumber();
            }


            var indexes = new int[4] { 0, 0, 0, 0 };
            for (int i = 0; i < 4; i++)
            {
                int ind2 = 0;
                while (chord1.ChordNotes[i] != notes1[ind2])
                {
                    ind2++;
                    indexes[i]++;
                }
            }

            aTransformation[0] = aTransformation2[indexes[0]];
            aTransformation[1] = aTransformation2[indexes[1]];
            aTransformation[2] = aTransformation2[indexes[2]];
            aTransformation[3] = aTransformation2[indexes[3]];

            if (aTransformation[0] > 0)
            {
                while (aTransformation[0] > 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        aTransformation[i] -= 1;
                    }
                }
            }
            else if (aTransformation[0] < 0)
            {
                while (aTransformation[0] < 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        aTransformation[i] += 1;
                    }
                }
            }

            return aTransformation;
        }

        public static bool ApplyATransformation(SevenChord chordo, int[] transformation)
        {

            chordo.ChordType = SevenChord.DetermineChordType(chordo);

            var mode = SevenChordTypes.SevenChordModeFormula[chordo.Mode];


            chordo.ChordNotes[0] = JustNote.moveNoteBySemitones(chordo.ChordNotes[0], transformation[0]);
            chordo.ChordNotes[1] = JustNote.moveNoteBySemitones(chordo.ChordNotes[1], transformation[1]);
            chordo.ChordNotes[2] = JustNote.moveNoteBySemitones(chordo.ChordNotes[2], transformation[2]);
            chordo.ChordNotes[3] = JustNote.moveNoteBySemitones(chordo.ChordNotes[3], transformation[3]);


            chordo.ChordType = SevenChord.DetermineChordType(chordo);
            if (chordo.ChordType == "undefined")
                return false;

            try
            {
                SevenChord.ResetChordRootStepsModeAndInversion(chordo);

                if (chordo.ChordNotes.All(cn => cn.unboundOctave > 9 || cn.unboundOctave < 0))
                    return false;
                else
                    return true;
            }
            catch (Exception) { return false; }
        }
    }
}
