using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace music_theory_cycles_001
{
    /// <summary>
    /// Representation of a single note in 12-tone еqual temperament
    /// </summary>
    public class JustNote
    {
        /// <summary>
        /// An NoteName Enum's value representinh the latin name from C to B, semitones marked with #, not b
        /// </summary>
        public NoteName nameOfTheNote;
        /// <summary>
        /// An octave, from Subcontra to Fifth. Outbounded from piano marked as Octave :: 9 - OUTOFNORMALPIANO
        /// </summary>
        public Octave octave;
        /// <summary>
        /// For if you somehow needed for notes out of the normal piano scope. Also that number's used in NFugue
        /// </summary>
        public int unboundOctave;

        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="noteName">Name of given note from the NoteName Enum</param>
        /// <param name="boundedOctave">An octave with respect to the piano bounds</param>
        /// <param name="octaveAbsolute">Plain Int value representing the octave of the note without the piano bounds</param>
        public JustNote(NoteName noteName, Octave boundedOctave, int octaveAbsolute = 0)
        {
            nameOfTheNote = noteName;
            octave = boundedOctave;
            unboundOctave = octaveAbsolute;
        }

        /// <summary>
        /// Returns notename plus octave number: A1, C3, etc. Notes out of piano scope marked like: A13[WRN]
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (octave >= Octave.Subcontra && octave <= Octave.Fifthline)
                return nameOfTheNote.ToString() + octave.ToString("D");
            else
                return nameOfTheNote.ToString() + unboundOctave.ToString("D") + "[WRN]";
        }

        /// <summary>
        /// Returns an absolute number representing this note. Returns (octave times 12) + (NoteName Enum's position of current note)
        /// </summary>
        /// <returns></returns>
        public int getNoteNumber()
        {
            if (octave >= Octave.Subcontra && octave <= Octave.Fifthline)
                return (int)octave * 12 + (int)nameOfTheNote;
            else
                return unboundOctave * 12 + (int)nameOfTheNote;
        }

        /// <summary>
        /// Returns a note from given int
        /// </summary>
        /// <param name="number">A number representing single note</param>
        /// <returns></returns>
        public static JustNote getNoteFromNumber(int number)
        {
            int octaveNumber = number / 12;
            int noteShiftFromC = number % 12;
            if (octaveNumber >= 0 && octaveNumber <= 8)
                return new JustNote((NoteName)(noteShiftFromC), (Octave)(octaveNumber), octaveNumber);
            else
                return new JustNote((NoteName)(noteShiftFromC), Octave.OUTOFNORMALPIANO, octaveNumber);
        }

        /// <summary>
        /// Takes a current note and steps in semitones need to be performed, then returns a new note being moved by those semitones from the origin
        /// </summary>
        /// <param name="note">Note we want to move</param>
        /// <param name="number">Number of semitone steps upward or downward</param>
        /// <returns></returns>
        public static JustNote moveNoteBySemitones(JustNote note, int number)
        {
            return getNoteFromNumber(note.getNoteNumber() + number);
        }

        /// <summary>
        /// Returns a prepared note info for the NFugue usage
        /// </summary>
        /// <returns></returns>
        public string GetNFugueName()
        {
            string basename = "";

            switch (nameOfTheNote)
            {
                case NoteName.C:
                    basename = "C";
                    break;
                case NoteName.CsDb:
                    basename = "C#";
                    break;
                case NoteName.D:
                    basename = "D";
                    break;
                case NoteName.DsEb:
                    basename = "D#";
                    break;
                case NoteName.E:
                    basename = "E";
                    break;
                case NoteName.F:
                    basename = "F";
                    break;
                case NoteName.FsGb:
                    basename = "F#";
                    break;
                case NoteName.G:
                    basename = "G";
                    break;
                case NoteName.GsAb:
                    basename = "G#";
                    break;
                case NoteName.A:
                    basename = "A";
                    break;
                case NoteName.AsBb:
                    basename = "A#";
                    break;
                case NoteName.B:
                    basename = "B";
                    break;
                default:
                    break;
            }

            return basename + unboundOctave.ToString();
        } 

    }


    /// <summary>
    /// The latin note names Enum. From C to B, semitones marked with #, not b
    /// </summary>
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

    /// <summary>
    /// Octave names Enum. From Subcontra to Fifth. Outbounded from piano marked as Octave :: 9 - OUTOFNORMALPIANO
    /// </summary>
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
