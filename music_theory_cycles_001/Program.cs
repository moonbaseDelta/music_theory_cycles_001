using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using static music_theory_cycles_001.SevenChordTypes;

namespace music_theory_cycles_001
{  
    public class Program
    {
        static void Main(string[] args)
        { 
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            //var k = new ChordNote(NoteName.AsBb, Octave.Small, SevenChordStep.III); 
            //k.nameOfTheNote -= 4; 
            //Console.WriteLine(k);


            Console.WriteLine(JustNote.getNoteFromNumber(119).ToString());


            Console.WriteLine();
            Console.WriteLine();


            var ch = new SevenChord(JustNote.getNoteFromNumber(48), SevenChordTypes.GetChordFormula("Maj7"));
            var ch2 = new SevenChord(JustNote.getNoteFromNumber(53), SevenChordTypes.GetChordFormula("Maj7"));
            ch2.InverseBackward(SevenChordInversion.III);
            var ch3 = new SevenChord(JustNote.getNoteFromNumber(48), SevenChordTypes.GetChordFormula("Maj7"), SevenChordMode.II);
            var ch4 = new SevenChord(JustNote.getNoteFromNumber(48), new int[] { 3, 4, 3, 2 }, inv: SevenChordInversion.III);
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
            foreach (var item in SevenChordTypes.SevenchordTypes)
            {
                Console.WriteLine("\t" + item.Key + "\t ::: \t" + item.Value[0] + item.Value[1] + item.Value[2] + item.Value[3]);
            }
            Console.WriteLine();
            Console.WriteLine();

            ch2.InverseBackward(SevenChordInversion.III);
            var jjj = SevenChordTransformation.GetATransformation(ch2, ch);
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
            foreach (var item in SevenChordTypes.SevenchordTypes)
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int ii = 0; ii < 4; ii++)
                    {
                        var chordo = new SevenChord(JustNote.getNoteFromNumber(36), item.Value);
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
                    Transformations.Add(SevenChordTransformation.GetATransformation(chord1, chord2));
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

            foreach (var chordtype in SevenChordTypes.SevenchordTypes.Keys)
            { 
                var infos = new List<SimpleTransformationInfo>();
                foreach (var item in Transformations)
                {
                    var testChord = new SevenChord(JustNote.getNoteFromNumber(48), SevenChordTypes.GetChordFormula(chordtype));

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

                    while (SevenChordTransformation.ApplyATransformation(testChord, new int[] { item[0], item[1], item[2], item[3] }))
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
}
