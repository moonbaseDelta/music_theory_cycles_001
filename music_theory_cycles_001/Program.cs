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
            // Neeeded 'cause of symbols used to name chords
            // Without it works fine, but prints '?' all around
            Console.OutputEncoding = System.Text.Encoding.UTF8;
               

            var neg = new List<SevenChord>(); 
            int countChordos = 0;
            foreach (var item in SevenChordTypes.SevenchordTypes) 
                for (int i = 0; i < 6; i++) 
                    for (int ii = 0; ii < 4; ii++)
                    {
                        var chordo = new SevenChord(JustNote.getNoteFromNumber(36), item.Value);
                        chordo.Mode = (SevenChordMode)i;
                        chordo.InverseForward((SevenChordInversion)ii);
                        //Console.WriteLine(chordo);
                        countChordos++;
                        neg.Add(chordo);
                    } 

            var Transformations = new List<int[]>();
            foreach (var chord1 in neg) 
                foreach (var chord2 in neg)
                {
                    Transformations.Add(SevenChordTransformation.GetATransformation(chord1, chord2));
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
  
            Console.WriteLine();
            Console.WriteLine("Total chords variations count is: " + countChordos);
            Console.WriteLine();

            int counttt = 0;
            List<TransformationWithData> TransData = new List<TransformationWithData>();
            foreach (var item in Transformations)
            {
                var a = new TransformationWithData();
                a.Formula = item;
                a.ApplicationsOnVariosChords = new List<TransformationDetailedInfo>();

                foreach (var chordtype in SevenChordTypes.SevenchordTypes.Keys) 
                    foreach (SevenChordMode mode in (SevenChordMode[])Enum.GetValues(typeof(SevenChordMode))) 
                        foreach (SevenChordInversion inv in (SevenChordInversion[])Enum.GetValues(typeof(SevenChordInversion)))
                        { 
                            var b = new TransformationDetailedInfo();

                            b.InitialChordType = chordtype;
                            b.InitialChordMode = mode;
                            b.InitialChordInversion = inv;

                            var testChord = new SevenChord(JustNote.getNoteFromNumber(48), SevenChordTypes.GetChordFormula(chordtype), mode, inv);

                            b.TheApplications = new List<SevenChordSignature>();
                            b.TheStepsTransitions = new List<ChordStepsTransition>();

                            var bb = new SevenChordSignature();
                            bb.ChordType = testChord.ChordType;
                            bb.Mode = testChord.Mode;
                            bb.Inversion = testChord.Inversion;
                            b.TheApplications.Add(bb);

                            var prevNotes = new JustNote[4];
                            for (int i = 0; i < 4; i++)
                                prevNotes[i] = testChord.ChordNotes[i];

                            var prevSize = testChord.GetChordSize();
                            var origSize = testChord.GetChordSize();

                            int simpleLoopFound = 0;
                            int fullLoopFound = 0;

                            int stepsTaken = 0;
                            while (SevenChordTransformation.ApplyATransformation(testChord, a.Formula) && !b.Cyclic)
                            {
                                var bbb = new SevenChordSignature();
                                bbb.ChordType = testChord.ChordType;
                                bbb.Mode = testChord.Mode;
                                bbb.Inversion = testChord.Inversion;
                                b.TheApplications.Add(bbb);

                                var bbc = new ChordStepsTransition();
                                bbc.TonicMoved = testChord.ChordNotes[0].getNoteNumber() - prevNotes[0].getNoteNumber();
                                bbc.ThirdMoved = testChord.ChordNotes[1].getNoteNumber() - prevNotes[1].getNoteNumber();
                                bbc.FifthMoved = testChord.ChordNotes[2].getNoteNumber() - prevNotes[2].getNoteNumber();
                                bbc.SeventhMoved = testChord.ChordNotes[3].getNoteNumber() - prevNotes[3].getNoteNumber();

                                for (int i = 0; i < 4; i++)
                                    prevNotes[i] = JustNote.moveNoteBySemitones(prevNotes[i], a.Formula[i]);

                                if (prevNotes[0].getNoteNumber() == testChord.ChordNotes[0].getNoteNumber())
                                    bbc.TonicInto = SevenChordStep.T;
                                else if (prevNotes[0].getNoteNumber() == testChord.ChordNotes[1].getNoteNumber())
                                    bbc.TonicInto = SevenChordStep.III;
                                else if (prevNotes[0].getNoteNumber() == testChord.ChordNotes[2].getNoteNumber())
                                    bbc.TonicInto = SevenChordStep.V;
                                else if (prevNotes[0].getNoteNumber() == testChord.ChordNotes[3].getNoteNumber())
                                    bbc.TonicInto = SevenChordStep.VII;

                                if (prevNotes[1].getNoteNumber() == testChord.ChordNotes[0].getNoteNumber())
                                    bbc.ThirdInto = SevenChordStep.T;
                                else if (prevNotes[1].getNoteNumber() == testChord.ChordNotes[1].getNoteNumber())
                                    bbc.ThirdInto = SevenChordStep.III;
                                else if (prevNotes[1].getNoteNumber() == testChord.ChordNotes[2].getNoteNumber())
                                    bbc.ThirdInto = SevenChordStep.V;
                                else if (prevNotes[1].getNoteNumber() == testChord.ChordNotes[3].getNoteNumber())
                                    bbc.ThirdInto = SevenChordStep.VII;

                                if (prevNotes[2].getNoteNumber() == testChord.ChordNotes[0].getNoteNumber())
                                    bbc.FifthInto = SevenChordStep.T;
                                else if (prevNotes[2].getNoteNumber() == testChord.ChordNotes[1].getNoteNumber())
                                    bbc.FifthInto = SevenChordStep.III;
                                else if (prevNotes[2].getNoteNumber() == testChord.ChordNotes[2].getNoteNumber())
                                    bbc.FifthInto = SevenChordStep.V;
                                else if (prevNotes[2].getNoteNumber() == testChord.ChordNotes[3].getNoteNumber())
                                    bbc.FifthInto = SevenChordStep.VII;

                                if (prevNotes[3].getNoteNumber() == testChord.ChordNotes[0].getNoteNumber())
                                    bbc.SeventhInto = SevenChordStep.T;
                                else if (prevNotes[3].getNoteNumber() == testChord.ChordNotes[1].getNoteNumber())
                                    bbc.SeventhInto = SevenChordStep.III;
                                else if (prevNotes[3].getNoteNumber() == testChord.ChordNotes[2].getNoteNumber())
                                    bbc.SeventhInto = SevenChordStep.V;
                                else if (prevNotes[3].getNoteNumber() == testChord.ChordNotes[3].getNoteNumber())
                                    bbc.SeventhInto = SevenChordStep.VII;

                                bbc.SizeDifference = testChord.GetChordSize() - prevSize;
                                prevSize = testChord.GetChordSize();

                                b.TheStepsTransitions.Add(bbc);

                                for (int i = 0; i < 4; i++)
                                    prevNotes[i] = testChord.ChordNotes[i];

                                stepsTaken++; 
                                
                                // TODO : More complicated and trusted way to determine exact cycle
                                if (testChord.ChordType == chordtype) simpleLoopFound++;
                                if (testChord.ChordType == chordtype && testChord.Mode == mode && testChord.Inversion == inv) fullLoopFound++;
                                if (fullLoopFound == 2)
                                    b.Cyclic = true;

                                if (stepsTaken > 27 && simpleLoopFound < 1)
                                    break;
                                if (stepsTaken > 55) 
                                    break; 
                            }


                            if (b.Cyclic)
                            { 
                                b.LoopByTypeDistance = (stepsTaken / 2 + 1) / (simpleLoopFound / 2) - 1;
                                b.FullLoopDistance = stepsTaken / 2  ;

                                var sum = b.TheStepsTransitions.Sum(st => st.TonicMoved);
                                if (sum == 0)
                                    b.StayInPlace = true;
                                else if (sum > 0)
                                    b.MovingUp = true;
                                else
                                    b.MovingDown = true;

                                b.TheApplications.RemoveRange(b.FullLoopDistance, b.TheApplications.Count - b.FullLoopDistance - 1);
                                b.TheStepsTransitions.RemoveRange((b.FullLoopDistance -1) , b.TheStepsTransitions.Count -  b.FullLoopDistance );
                                 
                                if (origSize - prevSize == 0)
                                    b.SizeKept = true;

                                a.ApplicationsOnVariosChords.Add(b);
                            } 
                        }
                if (a.ApplicationsOnVariosChords.Count > 0) 
                    TransData.Add(a);
                counttt++;
                Console.WriteLine(TransData.Count + " :: " + counttt + " :: " + " [" + a.Formula[0] + "  " + a.Formula[1] + "  " + a.Formula[2] + "  " + a.Formula[3] + "] "  + a.ApplicationsOnVariosChords.Count);
            }
            // TODO: Inspect transformations on different chords for being equal 

            int kk = 0;


            kk = TransData.Max(t => t.ApplicationsOnVariosChords.Max(tt => tt.FullLoopDistance));

            int kasdf = 0;
             

              kasdf = 1;
            //foreach (var chordtype in SevenChordTypes.SevenchordTypes.Keys)
            //{  
            //    var infos = new List<SimpleTransformationInfo>();
            //    foreach (var item in Transformations)
            //    {
            //        var testChord = new SevenChord(JustNote.getNoteFromNumber(48), SevenChordTypes.GetChordFormula(chordtype));

            //        var typeOfChord = testChord.ChordType;
            //        var typeOfChord2 = testChord.ChordType;

            //        var modeOfChord = testChord.Mode;
            //        var modeOfChord2 = testChord.Mode;

            //        var invOfChord = testChord.Inversion;
            //        var invOfChord2 = testChord.Inversion;


            //        int typeRepTwice = 0;
            //        int cycleCount = 0;
            //        if (item[0] == 0 && item[1] == 0 && item[2] == 0 && item[3] == -2)
            //        {
            //            // Console.WriteLine(testChord);
            //            // Console.WriteLine(SevenChord.DetermineChordType(testChord));
            //        }

            //        while (SevenChordTransformation.ApplyATransformation(testChord, new int[] { item[0], item[1], item[2], item[3] }))
            //        {
            //            typeOfChord2 = testChord.ChordType;
            //            modeOfChord2 = testChord.Mode;
            //            invOfChord2 = testChord.Inversion;


            //            if (typeOfChord2 == typeOfChord && modeOfChord2 == modeOfChord && invOfChord2 == invOfChord) typeRepTwice++;
            //            if (typeRepTwice == 2)
            //                break;

            //            if (item[0] == 0 && item[1] == 0 && item[2] == 0 && item[3] == -2)
            //            {
            //                // Console.WriteLine(testChord);
            //                // Console.WriteLine(SevenChord.DetermineChordType(testChord));
            //            }
            //            // Console.WriteLine(ch3);
            //            //Console.WriteLine(SevenChord.DetermineChordType(ch3));
            //            cycleCount++;

            //            if (cycleCount > 100)
            //                break;
            //        }

            //        if (typeRepTwice == 2)
            //        {
            //            var ku = new SimpleTransformationInfo();
            //            ku.count = cycleCount / 2 + 1;
            //            ku.mess = "The cycle [" + item[0] + "  " + item[1] + "  " + item[2] + "  " + item[3] + "] looped on " + (cycleCount / 2 + 1) + " turn.";

            //            infos.Add(ku);
            //            //Console.WriteLine("The cycle [" + item[0] + "  " + item[1] + "  " + item[2] + "  " + item[3] + "] looped on " + (cycleCount / 2 + 1) + " turn.");
            //        }
            //        else
            //        {
            //            //    if (cycleCount < 10 && cycleCount > 0)
            //            //        Console.WriteLine("The cycle [" + item[0] + "  " + item[1] + "  " + item[2] + "  " + item[3] + "] crashed on " + cycleCount + " turn."); 
            //        }
            //    } 
            //    infos.Sort(delegate (SimpleTransformationInfo ti1, SimpleTransformationInfo ti2) { return ti1.count.CompareTo(ti2.count); });

            //    var ninfos = infos.GroupBy(iii => iii.count).ToList();
            //    Console.WriteLine(chordtype + " loops by steps: ");
            //    ninfos.ForEach(niii => Console.WriteLine(niii.Key + ": " + niii.Count()));
            //} 

            Console.Read();
        }
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
