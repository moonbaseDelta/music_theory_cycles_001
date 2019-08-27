using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace music_theory_cycles_001
{
    public class JustNote
    {
        public NoteName nameOfTheNote;
        public Octave octave;
        public int unboundOctave;

        public JustNote(NoteName n, Octave o, int newoct = 0)
        {
            nameOfTheNote = n;
            octave = o;
            unboundOctave = newoct;
        }

        public override string ToString()
        {
            if (octave >= Octave.Subcontra && octave <= Octave.Fifthline)
                return nameOfTheNote.ToString() + octave.ToString("D");
            else
                return nameOfTheNote.ToString() + " ERR " + unboundOctave.ToString("D");
        }

        public int getNoteNumber()
        {
            if (octave >= Octave.Subcontra && octave <= Octave.Fifthline)
                return (int)octave * 12 + (int)nameOfTheNote;
            else
                return (int)unboundOctave * 12 + (int)nameOfTheNote;
        }

        public static JustNote getNoteFromNumber(int number)
        {
            int oct = number / 12;
            int rem = number % 12;
            if (oct >= 0 && oct <= 8)
            {
                return new JustNote((NoteName)(rem), (Octave)(oct), oct);
            }
            else
            {
                return new JustNote((NoteName)(rem), Octave.OUTOFNORMALPIANO, oct);
                //return null;
            }
        }

        public static JustNote moveNoteBySemitones(JustNote note, int number)
        {
            return getNoteFromNumber(note.getNoteNumber() + number);
        }

        public string GetNFugueName()
        {
            string res = "";

            switch (nameOfTheNote)
            {
                case NoteName.C:
                    res = "C";
                    break;
                case NoteName.CsDb:
                    res = "C#";
                    break;
                case NoteName.D:
                    res = "D";
                    break;
                case NoteName.DsEb:
                    res = "D#";
                    break;
                case NoteName.E:
                    res = "E";
                    break;
                case NoteName.F:
                    res = "F";
                    break;
                case NoteName.FsGb:
                    res = "F#";
                    break;
                case NoteName.G:
                    res = "G";
                    break;
                case NoteName.GsAb:
                    res = "G#";
                    break;
                case NoteName.A:
                    res = "A";
                    break;
                case NoteName.AsBb:
                    res = "A#";
                    break;
                case NoteName.B:
                    res = "B";
                    break;
                default:
                    break;
            }

            return res + unboundOctave.ToString();
        } 

    } 

    public enum NoteName
    {
        C,
        CsDb,
        D,
        DsEb,
        E,
        F,
        FsGb,
        G,
        GsAb,
        A,
        AsBb,
        B
    }
    public enum Octave
    {
        Subcontra,
        Contra,
        Great,
        Small,
        FirstLine,
        SecondLine,
        ThirdLine,
        FourthLine,
        Fifthline,
        OUTOFNORMALPIANO
    } 
}
