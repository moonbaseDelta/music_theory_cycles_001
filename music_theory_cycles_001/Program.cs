using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static music_theory_cycles_001.ChordTypes;

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
    }

    public class ChordNote : JustNote
    {
        public SevenChordStep Step;

        public ChordNote(NoteName n, Octave o, SevenChordStep step) : base(n, o)
        {
            this.Step = step;
        }
    }

    public enum SevenChordStep
    {
        I,
        III,
        V,
        VII
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

    public class Program
    {
        public static JustNote getNoteFromNumber(int number)
        {
            if (number / 12 >= 0 && number / 12 <= 8)
            {
                return new JustNote((NoteName)(number % 12), (Octave)(number / 12), (number / 12));
            }
            else
            {
                return new JustNote((NoteName)(number % 12), Octave.OUTOFNORMALPIANO, (number / 12));
                //return null;
            }
        }

        public static JustNote moveNoteBySemitones(JustNote note, int number)
        {
            return getNoteFromNumber(note.getNoteNumber() + number);
        }


        static void Main(string[] args)
        {


            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var k = new ChordNote(NoteName.AsBb, Octave.Small, SevenChordStep.III);

            k.nameOfTheNote -= 4;

            Console.WriteLine(k);
            Console.WriteLine(getNoteFromNumber(119).ToString());


            Console.WriteLine();
            Console.WriteLine();


            var ch = new SevenChord(getNoteFromNumber(48), ChordTypes.GetChordFormula("Maj7"));
            var ch2 = new SevenChord(getNoteFromNumber(53), ChordTypes.GetChordFormula("Maj7"));
            ch2.InverseBackward(SevenChordInversion.III);
            var ch3 = new SevenChord(getNoteFromNumber(48), ChordTypes.GetChordFormula("Maj7"), SevenChordMode.II);
            var ch4 = new SevenChord(getNoteFromNumber(48), new int[] { 3, 4, 3, 2 }, inv: SevenChordInversion.III);
            //Console.WriteLine(ch);
            //Console.WriteLine(ch2);
            Console.WriteLine(ch3);
            ch3.TransposeTheChord(0);
            Console.WriteLine(ch3);
            Console.WriteLine(ch3.ChordType);
            //ch3.InverseBackward(SevenChordInversion.IV);
            //Console.WriteLine(ch3);
            //ch3.InverseBackward(SevenChordInversion.III);
            Console.WriteLine(ch3);
            //ch3.InverseBackward(SevenChordInversion.II);
            //Console.WriteLine(ch3);
            //ch3.InverseBackward(SevenChordInversion.I);
            //Console.WriteLine(ch3);
            //Console.WriteLine((int)ch4.Inversion);

            Console.WriteLine();
            Console.WriteLine();
            foreach (var item in ChordTypes.SevenchordTypes)
            {
                Console.WriteLine("\t" + item.Key + "\t ::: \t" + item.Value[0] + item.Value[1] + item.Value[2] + item.Value[3]);
            }
            Console.WriteLine();
            Console.WriteLine();

            ch2.InverseBackward(SevenChordInversion.III);
            var jjj = ChordTransformation.GetATransformation(ch2, ch);
            string str = "";
            foreach (var item in jjj)
            {
                str += item + " ";
            }

            Console.WriteLine(str);

            Console.WriteLine(ch);
            //ch2.TransposeTheChord(5);
            Console.WriteLine(ch2);


            var neg = new List<SevenChord>();

            int countChordos = 0;
            foreach (var item in ChordTypes.SevenchordTypes)
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int ii = 0; ii < 4; ii++)
                    {
                        var chordo = new SevenChord(getNoteFromNumber(36), item.Value);
                        chordo.Mode = (SevenChordMode)i;
                        chordo.InverseForward((SevenChordInversion)ii);
                        //Console.WriteLine(chordo);
                        countChordos++;
                        neg.Add(chordo);
                    }
                }
            }

            var Transformations = new List<int[]>();
            foreach (var chord1 in neg)
            {
                foreach (var chord2 in neg)
                {
                    Transformations.Add(ChordTransformation.GetATransformation(chord1, chord2));
                }
            }
            int t2c = Transformations.Count;
            Transformations = Transformations.Distinct(new IntArrayComparer()).ToList();
            Transformations = Transformations.OrderBy((tr2) => tr2[1]).ThenBy((tr2) => tr2[2]).ThenBy((tr3) => tr3[3]).ToList();
            ////Transformations.Sort(delegate (int[] tr1, int[] tr2) { return tr1[1].CompareTo(tr2[1]); });
            //Transformations.Sort(delegate (int[] tr1, int[] tr2) { return tr1[2].CompareTo(tr2[2]); });
            ////Transformations.Sort(delegate (int[] tr1, int[] tr2) { return tr1[3].CompareTo(tr2[3]); });
            //foreach (var item in Transformations)
            //{
            //    Console.WriteLine(" \t" + item[0] + "  " + item[1] + "  " + item[2] + "  " + item[3]);
            //}
            Console.WriteLine("\n\t" + t2c + " -> " + Transformations.Count);


            neg.Sort(delegate (SevenChord ch1, SevenChord ch22) { return ch1.GetChordSize().CompareTo(ch22.GetChordSize()); });
            //foreach (var item in neg)
            //{
            //    Console.WriteLine(item.ToString());
            //}

            Console.WriteLine();
            Console.WriteLine("Total chords variations count is: " + countChordos);
            Console.WriteLine();



            //Console.WriteLine("Applying [0  1  -4  0 ] over chord: ");
            //Console.WriteLine(ch3); 
            //Console.WriteLine(SevenChord.DetermineChordType(ch3));

            foreach (var chordtype in ChordTypes.SevenchordTypes.Keys)
            {


                var infos = new List<SimpleTransformationInfo>();
                foreach (var item in Transformations)
                {
                    var testChord = new SevenChord(getNoteFromNumber(48), ChordTypes.GetChordFormula(chordtype));

                    var typeOfChord = testChord.ChordType;
                    var typeOfChord2 = testChord.ChordType;

                    var modeOfChord = testChord.Mode;
                    var modeOfChord2 = testChord.Mode;

                    var invOfChord = testChord.Inversion;
                    var invOfChord2 = testChord.Inversion;


                    int typeRepTwice = 0;
                    int cycleCount = 0;
                    if (item[0] == 0 && item[1] == 0 && item[2] == 0 && item[3] == -2)
                    {
                        // Console.WriteLine(testChord);
                        // Console.WriteLine(SevenChord.DetermineChordType(testChord));
                    }

                    while (ChordTransformation.ApplyATransformation(testChord, new int[] { item[0], item[1], item[2], item[3] }))
                    {
                        typeOfChord2 = testChord.ChordType;
                        modeOfChord2 = testChord.Mode;
                        invOfChord2 = testChord.Inversion;


                        if (typeOfChord2 == typeOfChord && modeOfChord2 == modeOfChord && invOfChord2 == invOfChord) typeRepTwice++;
                        if (typeRepTwice == 2)
                            break;

                        if (item[0] == 0 && item[1] == 0 && item[2] == 0 && item[3] == -2)
                        {
                            // Console.WriteLine(testChord);
                            // Console.WriteLine(SevenChord.DetermineChordType(testChord));
                        }
                        // Console.WriteLine(ch3);
                        //Console.WriteLine(SevenChord.DetermineChordType(ch3));
                        cycleCount++;

                        if (cycleCount > 100)
                            break;
                    }

                    if (typeRepTwice == 2)
                    {
                        var ku = new SimpleTransformationInfo();
                        ku.count = cycleCount / 2 + 1;
                        ku.mess = "The cycle [" + item[0] + "  " + item[1] + "  " + item[2] + "  " + item[3] + "] looped on " + (cycleCount / 2 + 1) + " turn.";

                        infos.Add(ku);
                        //Console.WriteLine("The cycle [" + item[0] + "  " + item[1] + "  " + item[2] + "  " + item[3] + "] looped on " + (cycleCount / 2 + 1) + " turn.");
                    }
                    else
                    {
                        //    if (cycleCount < 10 && cycleCount > 0)
                        //        Console.WriteLine("The cycle [" + item[0] + "  " + item[1] + "  " + item[2] + "  " + item[3] + "] crashed on " + cycleCount + " turn."); 
                    }
                }


                infos.Sort(delegate (SimpleTransformationInfo ti1, SimpleTransformationInfo ti2) { return ti1.count.CompareTo(ti2.count); });

                var ninfos = infos.GroupBy(iii => iii.count).ToList();
                Console.WriteLine(chordtype + " loops by steps: ");
                ninfos.ForEach(niii => Console.WriteLine(niii.Key + ": " + niii.Count()));
            }

            //int cycleCount = 0;
            //while (ChordTransformation.ApplyATransformation(ch3, new int[] { 0,  1,  -4 , 0 }))
            //{
            //    Console.WriteLine(ch3);
            //    Console.WriteLine(SevenChord.DetermineChordType(ch3));
            //    cycleCount++;
            //}
            //Console.WriteLine("The cycle crashed on " + cycleCount + " turn.");

            //ChordTransformation.ApplyATransformation(ch3, new int[] { 0, -2, -2, -2 }); 
            //Console.WriteLine(ch3); 
            //Console.WriteLine(SevenChord.DetermineChordType(ch3));

            //ChordTransformation.ApplyATransformation(ch3, new int[] { 0, -2, -2, -2 });
            //Console.WriteLine(ch3);
            //Console.WriteLine(SevenChord.DetermineChordType(ch3));

            //ChordTransformation.ApplyATransformation(ch3, new int[] { 0, -2, -2, -2 });
            //Console.WriteLine(ch3);
            //Console.WriteLine(SevenChord.DetermineChordType(ch3));

            //ChordTransformation.ApplyATransformation(ch3, new int[] { 0, -2, -2, -2 });
            //Console.WriteLine(ch3);
            //Console.WriteLine(SevenChord.DetermineChordType(ch3));


            NoteName asdf = NoteName.A;
            for (int i = 0; i < 6; i++)
            {
                asdf = (NoteName)(((int)NoteName.A + i) % 12);


                Console.WriteLine(asdf);

                //var sdf = SevenChordModeFormula[(SevenChordMode)i][0];
            }

            //int countChordos2 = 0;
            //foreach (var chord in neg)
            //{
            //    foreach (var chord2 in neg)
            //    {
            //        Console.WriteLine("\t" + chord + " => \t" + chord2);

            //        countChordos2++;
            //    }
            //}
            //Console.WriteLine();
            //Console.WriteLine("Total is: " + countChordos2);


            Console.Read();
        }
    }

    public class SimpleTransformationInfo
    {
        public string mess;
        public int count;
    }

    public class IntArrayComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[] x, int[] y)
        {
            return x[0] == y[0] && x[1] == y[1] && x[2] == y[2] && x[3] == y[3];
        }

        public int GetHashCode(int[] obj)
        {
            return (obj[0].ToString() + obj[1].ToString() + obj[2].ToString() + obj[3].ToString()).GetHashCode();
        }
    }

    static public class ChordTypes
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
            { "Δ",  new int[] { 4, 3, 4, 1 } },
            { "Δ+",  new int[] { 4, 4, 3, 1 } },
            { "Δ-",  new int[] { 4, 2, 5, 1 } },
            { "m7",  new int[] { 3, 4, 3, 2 } },
            { "m7+",  new int[] { 3, 5, 2, 2 } },
            { "Ø",  new int[] { 3, 3, 4, 2 } },
            { "7",  new int[] { 4, 3, 3, 2 } },
            { "7+",  new int[] { 4, 4, 2, 2 } },
            { "7-",  new int[] { 4, 2, 4, 2 } },
            { "mΔ7",  new int[] { 3, 4, 4, 1 } },
            { "mΔ7+",  new int[] { 3, 4, 3, 1 } },
            { "mΔ7-",  new int[] { 3, 3, 5, 1 } },
            { "⃝",  new int[] { 3, 3, 3, 3 } },
        };


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



    public class SevenChord
    { 
        public JustNote[] ChordNotes = new JustNote[4];

        public SevenChordMode Mode;
        public SevenChordInversion Inversion;
        public string ChordType;

        public SevenChord(JustNote baseNote, int[] theFormula, SevenChordMode mode = SevenChordMode.I, SevenChordInversion inv = SevenChordInversion.I)
        {
            ChordType = ChordTypes.SevenchordTypes.First(f => f.Value[0] == theFormula[0] && f.Value[1] == theFormula[1] && f.Value[2] == theFormula[2] && f.Value[3] == theFormula[3]).Key.ToString();

            ChordNotes[0] = baseNote;

            var tempNote = baseNote;
            for (int i = 1; i < 4; i++)
            {
                tempNote = Program.moveNoteBySemitones(tempNote, theFormula[i - 1]);
                ChordNotes[i] = tempNote;
            } 

            Mode = mode;

            switch (Mode)
            {
                case SevenChordMode.I: 
                    break;
                case SevenChordMode.II:
                    ChordNotes[1] = Program.moveNoteBySemitones(ChordNotes[1], 12);
                    ChordNotes[3] = Program.moveNoteBySemitones(ChordNotes[3], 12);
                    break;
                case SevenChordMode.III:
                    ChordNotes[2] = Program.moveNoteBySemitones(ChordNotes[2], 12);
                    break;
                case SevenChordMode.IV:
                    ChordNotes[1] = Program.moveNoteBySemitones(ChordNotes[1], 12);
                    break;
                case SevenChordMode.V:
                    ChordNotes[1] = Program.moveNoteBySemitones(ChordNotes[1], 12);
                    ChordNotes[2] = Program.moveNoteBySemitones(ChordNotes[2], 12);
                    break;
                case SevenChordMode.VI:
                    ChordNotes[1] = Program.moveNoteBySemitones(ChordNotes[1], 24);
                    ChordNotes[2] = Program.moveNoteBySemitones(ChordNotes[2], 12);
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


        public void InverseForward(SevenChordInversion inv, SevenChordStep stepToKeep = SevenChordStep.I)
        {
            var mode = ChordTypes.SevenChordModeFormula[Mode];

            var b = mode[(0 + (int)inv) % 4];
            var c = mode[(1 + (int)inv) % 4];
            var d = mode[(2 + (int)inv) % 4];
            var a = mode[(3 + (int)inv) % 4];
            while (ChordNotes[b].getNoteNumber() > ChordNotes[c].getNoteNumber()) 
                ChordNotes[c] = Program.moveNoteBySemitones(ChordNotes[c], 12); 
            while (ChordNotes[c].getNoteNumber() > ChordNotes[d].getNoteNumber()) 
                ChordNotes[d] = Program.moveNoteBySemitones(ChordNotes[d], 12); 
            while (ChordNotes[d].getNoteNumber() > ChordNotes[a].getNoteNumber()) 
                ChordNotes[a] = Program.moveNoteBySemitones(ChordNotes[a], 12); 

            Inversion = inv;
        }

        public void InverseBackward(SevenChordInversion inv, SevenChordStep stepToKeep = SevenChordStep.I)
        {
            var mode = ChordTypes.SevenChordModeFormula[Mode];

            var b = mode[(3 + (int)inv) % 4];
            var c = mode[(2 + (int)inv) % 4];
            var d = mode[(1 + (int)inv) % 4];
            var a = mode[(0 + (int)inv) % 4];
            while (ChordNotes[b].getNoteNumber() < ChordNotes[c].getNoteNumber()) 
                ChordNotes[c] = Program.moveNoteBySemitones(ChordNotes[c], -12); 
            while (ChordNotes[c].getNoteNumber() < ChordNotes[d].getNoteNumber()) 
                ChordNotes[d] = Program.moveNoteBySemitones(ChordNotes[d], -12); 
            while (ChordNotes[d].getNoteNumber() < ChordNotes[a].getNoteNumber()) 
                ChordNotes[a] = Program.moveNoteBySemitones(ChordNotes[a], -12); 

            Inversion = inv;
        }


        public void TransposeTheChord(int semitones)
        {
            int j = 0;
            foreach (var note in ChordNotes)
            { 
                ChordNotes[j] = Program.moveNoteBySemitones(note, semitones);
                j++;
            }
        }

        //
        public void ChangeTheChordMode(SevenChordMode mode)
        {

        }
        //

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

                notes[0] = Program.moveNoteBySemitones(notes[0], 12); 

                notes = notes.OrderBy(n => n.getNoteNumber()).ToArray();
                size = notes.Last().getNoteNumber() - notes.First().getNoteNumber();
                if (size > 12)
                {
                    notes[3] = Program.moveNoteBySemitones(notes[3], -12);
                    notes = notes.OrderBy(n => n.getNoteNumber()).ToArray(); 

                    size = notes.Last().getNoteNumber() - notes.First().getNoteNumber();
                    int k = 0;
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
                 
                if (ChordTypes.SevenchordTypes.Select(fff => fff.Value).Any(fo => fo[0] == fTemp[0] && fo[1] == fTemp[1] && fo[2] == fTemp[2] && fo[3] == fTemp[3]))
                {
                    neuChordo.ChordType = ChordTypes.SevenchordTypes.First(fo => fo.Value[0] == fTemp[0] && fo.Value[1] == fTemp[1] && fo.Value[2] == fTemp[2] && fo.Value[3] == fTemp[3]).Key.ToString();
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
                        if (n == 1 && m == 5  )
                        {
                            int k = 0;
                        }

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

                            //chordo.ChordNotes[0] = neuChord.ChordNotes[0];
                            //chordo.ChordNotes[1] = neuChord.ChordNotes[1];
                            //chordo.ChordNotes[2] = neuChord.ChordNotes[2];
                            //chordo.ChordNotes[3] = neuChord.ChordNotes[3];

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


    public static class ChordTransformation
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


            var indexes = new int[4] { 0,0,0,0 };
            for (int i = 0; i < 4; i++)
            {
                int ind2 = 0;
                while (chord1.ChordNotes[i] != notes1[ind2])
                {
                    ind2++;
                    indexes[i]++;
                }
            }


            // index of the note


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

            var mode = ChordTypes.SevenChordModeFormula[chordo.Mode];

            //var fTemp = new int[4];
            //switch (chordo.Inversion)
            //{
            //    case SevenChordInversion.I:
            //        fTemp[0] = mode[0];
            //        fTemp[1] = mode[1];
            //        fTemp[2] = mode[2];
            //        fTemp[3] = mode[3];
            //        break;
            //    case SevenChordInversion.II:
            //        fTemp[0] = mode[1];
            //        fTemp[1] = mode[2];
            //        fTemp[2] = mode[3];
            //        fTemp[3] = mode[0];
            //        break;
            //    case SevenChordInversion.III:
            //        fTemp[0] = mode[2];
            //        fTemp[1] = mode[3];
            //        fTemp[2] = mode[0];
            //        fTemp[3] = mode[1];
            //        break;
            //    case SevenChordInversion.IV:
            //        fTemp[0] = mode[3];
            //        fTemp[1] = mode[0];
            //        fTemp[2] = mode[1];
            //        fTemp[3] = mode[2];
            //        break;
            //    default:
            //        break;
            //}


            //chordo.ChordNotes[0] = Program.moveNoteBySemitones(chordo.ChordNotes[fTemp[0]], transformation[0]);
            //chordo.ChordNotes[1] = Program.moveNoteBySemitones(chordo.ChordNotes[fTemp[1]], transformation[1]);
            //chordo.ChordNotes[2] = Program.moveNoteBySemitones(chordo.ChordNotes[fTemp[2]], transformation[2]);
            //chordo.ChordNotes[3] = Program.moveNoteBySemitones(chordo.ChordNotes[fTemp[3]], transformation[3]); 

            chordo.ChordNotes[0] = Program.moveNoteBySemitones(chordo.ChordNotes[0], transformation[0]);
            chordo.ChordNotes[1] = Program.moveNoteBySemitones(chordo.ChordNotes[1], transformation[1]);
            chordo.ChordNotes[2] = Program.moveNoteBySemitones(chordo.ChordNotes[2], transformation[2]);
            chordo.ChordNotes[3] = Program.moveNoteBySemitones(chordo.ChordNotes[3], transformation[3]);


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
