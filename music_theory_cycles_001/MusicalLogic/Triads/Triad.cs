using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static music_theory_cycles_001.TriadTypes;

namespace music_theory_cycles_001
{
    public class Triad
    {
        public JustNote[] ChordNotes = new JustNote[3];

        public TriadMode Mode;
        public TriadInversion Inversion;
        public string TriadType;



        public Triad(JustNote baseNote, int[] theFormula, TriadMode mode = TriadMode.I, TriadInversion inv = TriadInversion.I)
        {
            TriadType = TriadTypes.TriadsTypes.First(f => f.Value[0] == theFormula[0] && f.Value[1] == theFormula[1] && f.Value[2] == theFormula[2]).Key.ToString();

            ChordNotes[0] = baseNote;

            var tempNote = baseNote;
            for (int i = 1; i < 3; i++)
            {
                tempNote = JustNote.moveNoteBySemitones(tempNote, theFormula[i - 1]);
                ChordNotes[i] = tempNote;
            }

            Mode = mode;

            switch (Mode)
            {
                case TriadMode.I:
                    break;
                case TriadMode.II:
                    ChordNotes[1] = JustNote.moveNoteBySemitones(ChordNotes[1], 12);
                    break; 
                default:
                    break;
            }

            this.InverseForward(inv);
            Inversion = inv; 
        } 

        public Triad(JustNote baseNote, JustNote thirdNote, JustNote fifthNote, TriadMode mode, TriadInversion inv)
        {
            ChordNotes[0] = baseNote;
            ChordNotes[1] = thirdNote;
            ChordNotes[2] = fifthNote; 

            Mode = mode;
            Inversion = inv;
        }
         


        // TODO : Keep specific chord steps on their place
        public void InverseForward(TriadInversion inv)
        {
            var mode = TriadTypes.TriadModeFormula[Mode];

            var b = mode[(0 + (int)inv) % 3];
            var c = mode[(1 + (int)inv) % 3];
            var a = mode[(2 + (int)inv) % 3]; 

            while (ChordNotes[b].getNoteNumber() > ChordNotes[c].getNoteNumber())
                ChordNotes[c] = JustNote.moveNoteBySemitones(ChordNotes[c], 12);
            while (ChordNotes[c].getNoteNumber() > ChordNotes[a].getNoteNumber())
                ChordNotes[a] = JustNote.moveNoteBySemitones(ChordNotes[a], 12); 

            Inversion = inv;
        }

        // TODO : Keep specific chord steps on their place
        public void InverseBackward(TriadInversion inv)
        {
            var mode = TriadTypes.TriadModeFormula[Mode];
             

            var b = mode[(2 + (int)inv) % 3];
            var c = mode[(1 + (int)inv) % 3];
            var a = mode[(0 + (int)inv) % 3];

            while (ChordNotes[b].getNoteNumber() < ChordNotes[c].getNoteNumber())
                ChordNotes[c] = JustNote.moveNoteBySemitones(ChordNotes[c], -12);
            while (ChordNotes[c].getNoteNumber() < ChordNotes[a].getNoteNumber())
                ChordNotes[a] = JustNote.moveNoteBySemitones(ChordNotes[a], -12); 

            Inversion = inv;
        }

        public void TransposeTheChord(int semitones)
        {
            int j = 0;
            foreach (var note in ChordNotes)
            {
                ChordNotes[j] = JustNote.moveNoteBySemitones(note, semitones);
                j++;
            }
        }

        public void ChangeTheChordMode(TriadMode mode)
        {
            // TODO :: Make it changable from any mode to any mode
        }

        public int GetChordSize()
        {
            var notes = ChordNotes.OrderBy(n => n.getNoteNumber());
            int size = notes.Last().getNoteNumber() - notes.First().getNoteNumber();

            return size;
        }

        public string GetChordActualFormula()
        {
            var notes = ChordNotes.OrderBy(n => n.getNoteNumber());

            int fInterval = notes.ElementAt(1).getNoteNumber() - notes.ElementAt(0).getNoteNumber();
            int sInterval = notes.ElementAt(2).getNoteNumber() - notes.ElementAt(1).getNoteNumber(); 

            return fInterval + " " + sInterval;
        }



        static public string DetermineTriadType(Triad triad)
        {
            var f = new int[3];
            var neuTriad = new Triad(triad.ChordNotes[0], triad.ChordNotes[1], triad.ChordNotes[2], triad.Mode, triad.Inversion);

            int size = neuTriad.GetChordSize();

            var notes = neuTriad.ChordNotes.OrderBy(n => n.getNoteNumber()).ToArray();

            while (size > 12)
            {
                notes = notes.OrderBy(n => n.getNoteNumber()).ToArray();

                notes[0] = JustNote.moveNoteBySemitones(notes[0], 12);

                notes = notes.OrderBy(n => n.getNoteNumber()).ToArray();
                size = notes.Last().getNoteNumber() - notes.First().getNoteNumber();
                if (size > 12)
                {
                    notes[2] = JustNote.moveNoteBySemitones(notes[2], -12);
                    notes = notes.OrderBy(n => n.getNoteNumber()).ToArray();

                    size = notes.Last().getNoteNumber() - notes.First().getNoteNumber(); 
                }
            } 

            f[0] = notes[1].getNoteNumber() - notes[0].getNoteNumber();
            f[1] = notes[2].getNoteNumber() - notes[1].getNoteNumber(); 
            f[2] = 12 - f[0] - f[1];

            var fTemp = new int[3];

            for (int i = 0; i < 3; i++)
            {
                fTemp[0] = f[(i + 0) % 3];
                fTemp[1] = f[(i + 1) % 3];
                fTemp[2] = f[(i + 2) % 3]; 

                if (TriadTypes.TriadsTypes.Select(fff => fff.Value).Any(fo => fo[0] == fTemp[0] && fo[1] == fTemp[1] && fo[2] == fTemp[2]))
                {
                    neuTriad.TriadType = TriadTypes.TriadsTypes.First(fo => fo.Value[0] == fTemp[0] && fo.Value[1] == fTemp[1] && fo.Value[2] == fTemp[2]).Key.ToString();
                    break;
                }

                if (i == 3)
                    neuTriad.TriadType = "undefined";
            }
            return neuTriad.TriadType;
        }
         

        static public void ResetTriadRootStepsModeAndInversion(Triad triad)
        {
            for (int n = 0; n < 3; n++)
                for (int m = 0; m < 2; m++)
                    for (int i = 0; i < 3; i++)
                    {
                        var neuTriad = new Triad(triad.ChordNotes[n], GetChordFormula(triad.TriadType), (TriadMode)m, (TriadInversion)i);

                        var notesNeu = neuTriad.ChordNotes.OrderBy(n2 => n2.getNoteNumber()).ToArray().Select(nn => nn.nameOfTheNote).ToList();
                        var notesOrig = triad.ChordNotes.OrderBy(n2 => n2.getNoteNumber()).ToArray().Select(nn => nn.nameOfTheNote).ToList();

                        if (
                            notesNeu[0] == notesOrig[0] &&
                            notesNeu[1] == notesOrig[1] &&
                            notesNeu[2] == notesOrig[2] 
                            )
                        {
                            JustNote[] notes = new JustNote[4];
                            notes[0] = triad.ChordNotes[0];
                            notes[1] = triad.ChordNotes[1];
                            notes[2] = triad.ChordNotes[2];

                            triad.ChordNotes[0] = notes.Where(no => no.nameOfTheNote == neuTriad.ChordNotes[0].nameOfTheNote).First();
                            triad.ChordNotes[1] = notes.Where(no => no.nameOfTheNote == neuTriad.ChordNotes[1].nameOfTheNote).First();
                            triad.ChordNotes[2] = notes.Where(no => no.nameOfTheNote == neuTriad.ChordNotes[2].nameOfTheNote).First(); 

                            triad.Mode = neuTriad.Mode;
                            triad.Inversion = neuTriad.Inversion;
                            return;
                        }
                    }
        } 



        public override string ToString()
        {
            string str = "";
            foreach (var item in ChordNotes.OrderBy(n => n.getNoteNumber())) 
                str += item + " "; 

            return "\n\tChord " + TriadType + " " + "Inv." + Inversion + " " + "M." + Mode + " :: " + str + "\n\t Size : " + GetChordSize() + ". Actual formula: " + GetChordActualFormula()
                + "\n\t[T]:\t" + ChordNotes[0].nameOfTheNote + $"\t({ChordNotes[0].getNoteNumber()}) "
                + "\n\t[III]:\t" + ChordNotes[1].nameOfTheNote + $"\t({ChordNotes[1].getNoteNumber()}) "
                + "\n\t[V]:\t" + ChordNotes[2].nameOfTheNote + $"\t({ChordNotes[2].getNoteNumber()}) " 
                + "\n";
        }

    }
}
