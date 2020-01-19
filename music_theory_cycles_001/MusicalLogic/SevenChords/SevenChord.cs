using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static music_theory_cycles_001.SevenChordTypes;

namespace music_theory_cycles_001
{ 
    public class SevenChord
    {
        public JustNote[] ChordNotes = new JustNote[4];

        public SevenChordMode Mode;
        public SevenChordInversion Inversion;
        public string ChordType;

        public SevenChord(JustNote baseNote, int[] theFormula, SevenChordMode mode = SevenChordMode.I, SevenChordInversion inv = SevenChordInversion.I)
        {
            ChordType = SevenChordTypes.SevenchordTypes.First(f => f.Value[0] == theFormula[0] && f.Value[1] == theFormula[1] && f.Value[2] == theFormula[2] && f.Value[3] == theFormula[3]).Key.ToString();

            ChordNotes[0] = baseNote;

            var tempNote = baseNote;
            for (int i = 1; i < 4; i++)
            {
                tempNote = JustNote.moveNoteBySemitones(tempNote, theFormula[i - 1]);
                ChordNotes[i] = tempNote;
            }

            Mode = mode;

            switch (Mode)
            {
                case SevenChordMode.I:
                    break;
                case SevenChordMode.II:
                    ChordNotes[1] = JustNote.moveNoteBySemitones(ChordNotes[1], 12);
                    ChordNotes[3] = JustNote.moveNoteBySemitones(ChordNotes[3], 12);
                    break;
                case SevenChordMode.III:
                    ChordNotes[2] = JustNote.moveNoteBySemitones(ChordNotes[2], 12);
                    break;
                case SevenChordMode.IV:
                    ChordNotes[1] = JustNote.moveNoteBySemitones(ChordNotes[1], 12);
                    break;
                case SevenChordMode.V:
                    ChordNotes[1] = JustNote.moveNoteBySemitones(ChordNotes[1], 12);
                    ChordNotes[2] = JustNote.moveNoteBySemitones(ChordNotes[2], 12);
                    break;
                case SevenChordMode.VI:
                    ChordNotes[1] = JustNote.moveNoteBySemitones(ChordNotes[1], 24);
                    ChordNotes[2] = JustNote.moveNoteBySemitones(ChordNotes[2], 12);
                    break;
                default:
                    break;
            }

            this.InverseForward(inv);
            Inversion = inv;

        }

        public SevenChord(JustNote baseNote, JustNote thirdNote, JustNote fifthNote, JustNote sevenNote, SevenChordMode mode, SevenChordInversion inv)
        {
            ChordNotes[0] = baseNote;
            ChordNotes[1] = thirdNote;
            ChordNotes[2] = fifthNote;
            ChordNotes[3] = sevenNote;

            Mode = mode;
            Inversion = inv;
        }


        // TODO : Keep specific chord steps on their place
        public void InverseForward(SevenChordInversion inv)
        {
            var mode = SevenChordTypes.SevenChordModeFormula[Mode];

            var b = mode[(0 + (int)inv) % 4];
            var c = mode[(1 + (int)inv) % 4];
            var d = mode[(2 + (int)inv) % 4];
            var a = mode[(3 + (int)inv) % 4];
            while (ChordNotes[b].getNoteNumber() > ChordNotes[c].getNoteNumber())
                ChordNotes[c] = JustNote.moveNoteBySemitones(ChordNotes[c], 12);
            while (ChordNotes[c].getNoteNumber() > ChordNotes[d].getNoteNumber())
                ChordNotes[d] = JustNote.moveNoteBySemitones(ChordNotes[d], 12);
            while (ChordNotes[d].getNoteNumber() > ChordNotes[a].getNoteNumber())
                ChordNotes[a] = JustNote.moveNoteBySemitones(ChordNotes[a], 12);

            Inversion = inv;
        }

        // TODO : Keep specific chord steps on their place
        public void InverseBackward(SevenChordInversion inv)
        {
            var mode = SevenChordTypes.SevenChordModeFormula[Mode];

            var b = mode[(3 + (int)inv) % 4];
            var c = mode[(2 + (int)inv) % 4];
            var d = mode[(1 + (int)inv) % 4];
            var a = mode[(0 + (int)inv) % 4];
            while (ChordNotes[b].getNoteNumber() < ChordNotes[c].getNoteNumber())
                ChordNotes[c] = JustNote.moveNoteBySemitones(ChordNotes[c], -12);
            while (ChordNotes[c].getNoteNumber() < ChordNotes[d].getNoteNumber())
                ChordNotes[d] = JustNote.moveNoteBySemitones(ChordNotes[d], -12);
            while (ChordNotes[d].getNoteNumber() < ChordNotes[a].getNoteNumber())
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

        public void ChangeTheChordMode(SevenChordMode mode)
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
            int tInterval = notes.ElementAt(3).getNoteNumber() - notes.ElementAt(2).getNoteNumber();

            return fInterval + " " + sInterval + " " + tInterval; 
        }

        static public string DetermineChordType(SevenChord chordo)
        {
            var f = new int[4];
            var neuChordo = new SevenChord(chordo.ChordNotes[0], chordo.ChordNotes[1], chordo.ChordNotes[2], chordo.ChordNotes[3], chordo.Mode, chordo.Inversion);

            int size = neuChordo.GetChordSize();

            var notes = neuChordo.ChordNotes.OrderBy(n => n.getNoteNumber()).ToArray();
            while (size > 12)
            {
                notes = notes.OrderBy(n => n.getNoteNumber()).ToArray();

                notes[0] = JustNote.moveNoteBySemitones(notes[0], 12);

                notes = notes.OrderBy(n => n.getNoteNumber()).ToArray();
                size = notes.Last().getNoteNumber() - notes.First().getNoteNumber();
                if (size > 12)
                {
                    notes[3] = JustNote.moveNoteBySemitones(notes[3], -12);
                    notes = notes.OrderBy(n => n.getNoteNumber()).ToArray();

                    size = notes.Last().getNoteNumber() - notes.First().getNoteNumber(); 
                }
            }

            f[0] = notes[1].getNoteNumber() - notes[0].getNoteNumber();
            f[1] = notes[2].getNoteNumber() - notes[1].getNoteNumber();
            f[2] = notes[3].getNoteNumber() - notes[2].getNoteNumber();
            f[3] = 12 - f[0] - f[1] - f[2];

            var fTemp = new int[4];

            for (int i = 0; i < 4; i++)
            {
                fTemp[0] = f[(i + 0) % 4];
                fTemp[1] = f[(i + 1) % 4];
                fTemp[2] = f[(i + 2) % 4];
                fTemp[3] = f[(i + 3) % 4];

                if (SevenChordTypes.SevenchordTypes.Select(fff => fff.Value).Any(fo => fo[0] == fTemp[0] && fo[1] == fTemp[1] && fo[2] == fTemp[2] && fo[3] == fTemp[3]))
                {
                    neuChordo.ChordType = SevenChordTypes.SevenchordTypes.First(fo => fo.Value[0] == fTemp[0] && fo.Value[1] == fTemp[1] && fo.Value[2] == fTemp[2] && fo.Value[3] == fTemp[3]).Key.ToString();
                    break;
                }

                if (i == 3)
                    neuChordo.ChordType = "undefined";
            }
            return neuChordo.ChordType;
        }

        static public void ResetChordRootStepsModeAndInversion(SevenChord chordo)
        {
            for (int n = 0; n < 4; n++)
                for (int m = 0; m < 6; m++)
                    for (int i = 0; i < 4; i++)
                    { 
                        var neuChord = new SevenChord(chordo.ChordNotes[n], GetChordFormula(chordo.ChordType), (SevenChordMode)m, (SevenChordInversion)i); 

                        var notesNeu = neuChord.ChordNotes.OrderBy(n2 => n2.getNoteNumber()).ToArray().Select(nn => nn.nameOfTheNote).ToList();
                        var notesOrig = chordo.ChordNotes.OrderBy(n2 => n2.getNoteNumber()).ToArray().Select(nn => nn.nameOfTheNote).ToList();

                        if (
                            notesNeu[0] == notesOrig[0] &&
                            notesNeu[1] == notesOrig[1] &&
                            notesNeu[2] == notesOrig[2] &&
                            notesNeu[3] == notesOrig[3]
                            )
                        {
                            JustNote[] notes = new JustNote[4];
                            notes[0] = chordo.ChordNotes[0];
                            notes[1] = chordo.ChordNotes[1];
                            notes[2] = chordo.ChordNotes[2];
                            notes[3] = chordo.ChordNotes[3];

                            chordo.ChordNotes[0] = notes.Where(no => no.nameOfTheNote == neuChord.ChordNotes[0].nameOfTheNote).First();
                            chordo.ChordNotes[1] = notes.Where(no => no.nameOfTheNote == neuChord.ChordNotes[1].nameOfTheNote).First();
                            chordo.ChordNotes[2] = notes.Where(no => no.nameOfTheNote == neuChord.ChordNotes[2].nameOfTheNote).First();
                            chordo.ChordNotes[3] = notes.Where(no => no.nameOfTheNote == neuChord.ChordNotes[3].nameOfTheNote).First();


                            chordo.Mode = neuChord.Mode;
                            chordo.Inversion = neuChord.Inversion;
                            return;
                        }
                    }
        }



        public override string ToString()
        {
            string str = "";
            foreach (var item in ChordNotes.OrderBy(n => n.getNoteNumber()))
            {
                str += item + " ";
            }
            return "\n\tChord " + ChordType + " " + "Inv." + Inversion + " " + "M." + Mode + " :: " + str + "\n\t Size : " + GetChordSize() + ". Actual formula: " + GetChordActualFormula()
                + "\n\t[T]:\t" + ChordNotes[0].nameOfTheNote + $"\t({ChordNotes[0].getNoteNumber()}) "
                + "\n\t[III]:\t" + ChordNotes[1].nameOfTheNote + $"\t({ChordNotes[1].getNoteNumber()}) "
                + "\n\t[V]:\t" + ChordNotes[2].nameOfTheNote + $"\t({ChordNotes[2].getNoteNumber()}) "
                + "\n\t[VII]:\t" + ChordNotes[3].nameOfTheNote + $"\t({ChordNotes[3].getNoteNumber()}) "
                + "\n";
        }


    }

}
