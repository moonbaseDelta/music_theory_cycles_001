using NFugue.Patterns;
using NFugue.Playing;
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
            Utils.FairRandom = new Random();

            // Neeeded 'cause of symbols used to name chords
            // Without it works fine, but prints '?' all around
            Console.OutputEncoding = System.Text.Encoding.UTF8;


            /* TRIADS TESTING */
            var triads = CalculateAllTransformations.GetAllPossibleTriads();
            int countTriads = triads.Count;

            var TriadsTransformations = CalculateAllTransformations.GetRawListOfAcceptableTransormations(triads);

            int initialTriadsTransormationsFound = TriadsTransformations.Count;
            TriadsTransformations = CalculateAllTransformations.SortAndDistintTheTransformations(TriadsTransformations);

            Console.WriteLine("\nTriads transformations ::: " + initialTriadsTransormationsFound + " -> " + TriadsTransformations.Count); 

            triads.Sort(delegate (Triad tr1, Triad tr2) { return tr1.GetChordSize().CompareTo(tr2.GetChordSize()); });

            Console.WriteLine();
            Console.WriteLine("Total triads variations count is: " + countTriads);
            Console.WriteLine();


            var TriadsTransData = CalculateAllTransformations.CalculateTransDataForTriads(TriadsTransformations);

            int triadsMaxDistance = 0;
            triadsMaxDistance = TriadsTransData.Max(t => t.ApplicationsOnVariosTriads.Max(tt => tt.FullLoopDistance));


            var stopme  = 1;
            NFugueUsage.playsomeTriads(TriadsTransData, 5); 


            stopme = 1;


            /* SEVEN CHORDS TESTING */
            var neg = CalculateAllTransformations.GetAllPossibleSevenChords(); 
            int countChordos = neg.Count;  

            var Transformations = CalculateAllTransformations.GetRawListOfAcceptableTransormations(neg); 

            int initialTransormationsFound = Transformations.Count; 
            Transformations = CalculateAllTransformations.SortAndDistintTheTransformations(Transformations);

            Console.WriteLine("\nSevenChords transformations ::: " + initialTransormationsFound + " -> " + Transformations.Count);


            neg.Sort(delegate (SevenChord ch1, SevenChord ch22) { return ch1.GetChordSize().CompareTo(ch22.GetChordSize()); });
  
            Console.WriteLine();
            Console.WriteLine("Total chords variations count is: " + countChordos);
            Console.WriteLine();



            int counttt = 0;
            var TransData = CalculateAllTransformations.CalculateTransDataForSevenChords(Transformations);
            // TODO: Inspect transformations on different chords for being equal 

            int kk = 0;
            kk = TransData.Max(t => t.ApplicationsOnVariosChords.Max(tt => tt.FullLoopDistance));

            int kasdf = 0;
             

              kasdf = 1;
            var rand = new Random();
            //var goodone = TransData[rand.Next(TransData.Count)]; 
            //foreach (var item in goodone.ApplicationsOnVariosChords)
            //{

            //    //var testChord = new SevenChord(JustNote.getNoteFromNumber(48), 
            //    //    SevenChordTypes.GetChordFormula(item.InitialChordType), 
            //    //    item.InitialChordMode, item.InitialChordInversion);

            //    for (int i = 0; i < item.FullLoopDistance; i++)
            //    {

            //       // Console.WriteLine("After :: " + " [" + goodone.Formula[0] + "  " + goodone.Formula[1] + "  " + goodone.Formula[2] + "  " + goodone.Formula[3] + "] ");
            //       // Console.WriteLine(testChord.ToString());

            //        //using (var player = new Player())
            //        //{
            //        //    Pattern p1 = new Pattern($"T140 V0 I[Piano] {testChord.ChordNotes[0].GetNFugueName()}w ");
            //        //    Pattern p2 = new Pattern($"T140 V1 I[Flute] {testChord.ChordNotes[1].GetNFugueName()}w ");
            //        //    Pattern p3 = new Pattern($"T140 V2 I[Flute] {testChord.ChordNotes[2].GetNFugueName()}w ");
            //        //    Pattern p4 = new Pattern($"T140 V3 I[Flute] {testChord.ChordNotes[3].GetNFugueName()}w ");

            //        //    //player.Play(p1, p2, p3, p4);
            //        //}
            //        int k1111 = 0;
            //        //SevenChordTransformation.ApplyATransformation(testChord, goodone.Formula); 
            //    }
            //}
             

            playsmthn();
            for (int i = 0; i < 5; i++)
            {
                playsmthn();
            }

            kasdf = 1;

            void playsmthn()
            {
                var goodone = TransData[rand.Next(TransData.Count)];

                var interestings = goodone.ApplicationsOnVariosChords; //.Where(tt => tt.StayInPlace == true).ToList();

                if (interestings.Count() == 0)
                    return;

                var item = interestings[rand.Next(interestings.Count)];
                 
                    var testChord = new SevenChord(JustNote.getNoteFromNumber(48),
                        SevenChordTypes.GetChordFormula(item.InitialChordType),
                        item.InitialChordMode, item.InitialChordInversion);

                    using (var player = new Player())
                    {
                        Pattern p1 = new Pattern($"T100 V0 I[Piano] {testChord.ChordNotes[0].GetNFugueName()}q ");
                        Pattern p2 = new Pattern($"T100 V1 I[Flute] {testChord.ChordNotes[1].GetNFugueName()}q ");
                        Pattern p3 = new Pattern($"T100 V2 I[Flute] {testChord.ChordNotes[2].GetNFugueName()}q ");
                        Pattern p4 = new Pattern($"T100 V3 I[Flute] {testChord.ChordNotes[3].GetNFugueName()}q ");

                        player.Play(p1, p2, p3, p4);
                    }

                for (int i = 0; i < item.FullLoopDistance * 2; i++)
                    {
                        Console.WriteLine("After :: " + " [" + goodone.Formula[0] + "  " + goodone.Formula[1] + "  " + goodone.Formula[2] + "  " + goodone.Formula[3] + "] ");
                        Console.WriteLine(testChord.ToString());

                        using (var player = new Player())
                        {
                            Pattern p1 = new Pattern($"T140 V0 I[Piano] {testChord.ChordNotes[0].GetNFugueName()}w ");
                            Pattern p2 = new Pattern($"T140 V1 I[Flute] {testChord.ChordNotes[1].GetNFugueName()}w ");
                            Pattern p3 = new Pattern($"T140 V2 I[Flute] {testChord.ChordNotes[2].GetNFugueName()}w ");
                            Pattern p4 = new Pattern($"T140 V3 I[Flute] {testChord.ChordNotes[3].GetNFugueName()}w ");
                         int k21111 = 0;
                         player.Play(p1, p2, p3, p4);
                        }
                        int k1111 = 0;
                        SevenChordTransformation.ApplyATransformation(testChord, goodone.Formula); 
                    } 

            }


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



}
