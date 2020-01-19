using System;
using System.Collections.Generic;
using System.Linq; 
using NFugue.Patterns;
using NFugue.Playing;

namespace music_theory_cycles_001
{
    static public class NFugueUsage
    {
        static public void playsomeTriads( List<TriadTransformationWithData> transdata, int times = 5)
        {
            for (int ttt = 0; ttt < times; ttt++)
            {
                var goodone = transdata[Utils.FairRandom.Next(transdata.Count)];
                var interestings = goodone.ApplicationsOnVariosTriads; //.Where(tt => tt.StayInPlace == true).ToList();

                if (interestings.Count() == 0)
                    return;

                var item = interestings[Utils.FairRandom.Next(interestings.Count)];

                var testTriad = new Triad(
                                            JustNote.getNoteFromNumber(48),
                                            TriadTypes.GetChordFormula(item.InitialTriadType),
                                            item.InitialTriadMode,
                                            item.InitialTriadInversion
                                          );

                using (var player = new Player())
                {
                    Pattern p1 = new Pattern($"T100 V0 I[Piano] {testTriad.ChordNotes[0].GetNFugueName()}q ");
                    Pattern p2 = new Pattern($"T100 V1 I[Flute] {testTriad.ChordNotes[1].GetNFugueName()}q ");
                    Pattern p3 = new Pattern($"T100 V2 I[Flute] {testTriad.ChordNotes[2].GetNFugueName()}q ");

                    player.Play(p1, p2, p3);
                }

                for (int i = 0; i < item.FullLoopDistance * 2; i++)
                {
                    Console.WriteLine("After :: " + " [" + goodone.Formula[0] + "  " + goodone.Formula[1] + "  " + goodone.Formula[2] + "] ");
                    Console.WriteLine(testTriad.ToString());

                    using (var player = new Player())
                    {
                        Pattern p1 = new Pattern($"T140 V0 I[Piano] {testTriad.ChordNotes[0].GetNFugueName()}w ");
                        Pattern p2 = new Pattern($"T140 V1 I[Flute] {testTriad.ChordNotes[1].GetNFugueName()}w ");
                        Pattern p3 = new Pattern($"T140 V2 I[Flute] {testTriad.ChordNotes[2].GetNFugueName()}w ");
                        int k21111 = 0;
                        player.Play(p1, p2, p3);
                    }
                    int k1111 = 0;
                    TriadTransformation.ApplyATransformation(testTriad, goodone.Formula);
                }
            } 
        }
    }
}
