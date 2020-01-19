using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static music_theory_cycles_001.SevenChordTypes;
using static music_theory_cycles_001.TriadTypes;

namespace music_theory_cycles_001
{
    public static class CalculateAllTransformations
    {
        // TODO : There will be placed all logic to create data from scratch. 
        // It will not be used often 'cause data is static and will be stored in DB
        public static List<SevenChord> GetAllPossibleSevenChords()
        { 
            var listo = new List<SevenChord>();
            foreach (var item in SevenChordTypes.SevenchordTypes)
                for (int m = 0; m < 6; m++)
                    for (int i = 0; i < 4; i++)
                    {
                        var chordo = new SevenChord(JustNote.getNoteFromNumber(36), item.Value);
                        chordo.Mode = (SevenChordMode)m;
                        chordo.InverseForward((SevenChordInversion)i);
                        listo.Add(chordo);
                    }
            return listo;
        }

        public static List<Triad> GetAllPossibleTriads()
        {
            var listo = new List<Triad>();
            foreach (var item in TriadTypes.TriadsTypes)
                for (int m = 0; m < 2; m++)
                    for (int i = 0; i < 3; i++)
                    {
                        var chordo = new Triad(JustNote.getNoteFromNumber(36), item.Value);
                        chordo.Mode = (TriadMode)m;
                        chordo.InverseForward((TriadInversion)i);
                        listo.Add(chordo);
                    }
            return listo;
        }


        public static List<int[]> GetRawListOfAcceptableTransormations(List<SevenChord> chords)  
        { 
            var Transformations = new List<int[]>();
            foreach (var chord1 in chords)
                foreach (var chord2 in chords)
                    Transformations.Add(SevenChordTransformation.GetATransformation(chord1, chord2));  
            return Transformations;
        }

        public static List<int[]> GetRawListOfAcceptableTransormations(List<Triad> triads)
        {
            var Transformations = new List<int[]>();
            foreach (var triad1 in triads)
                foreach (var triad2 in triads)
                    Transformations.Add(TriadTransformation.GetATransformation(triad1, triad2));
            return Transformations;
        }

        public static List<int[]> SortAndDistintTheTransformations(List<int[]> transforms)
        { 
            transforms = transforms.Distinct(new IntArrayComparer()).ToList();
            switch (transforms[0].Length)
            {
                case 4:
                    transforms = transforms.OrderBy( (tr2) => tr2[1] ).ThenBy( (tr2) => tr2[2]).ThenBy( (tr3) => tr3[3] ).ToList();
                    break;
                case 3:
                    transforms = transforms.OrderBy( (tr2) => tr2[1] ).ThenBy( (tr2) => tr2[2] ).ToList();
                    break;
                default:
                    break;
            }
            return transforms;
        }



        public static List<SevenChordTransformationWithData> CalculateTransDataForSevenChords(List<int[]> sChordsTransformations)
        { 
            var TransData = new List<SevenChordTransformationWithData>();

            int counttt = 0;
            foreach (var item in sChordsTransformations)
            {
                var a = new SevenChordTransformationWithData();
                a.Formula = item;
                a.ApplicationsOnVariosChords = new List<SevenChordTransformationDetailedInfo>();

                foreach (var chordtype in SevenChordTypes.SevenchordTypes.Keys)
                    foreach (SevenChordMode mode in (SevenChordMode[])Enum.GetValues(typeof(SevenChordMode)))
                        foreach (SevenChordInversion inv in (SevenChordInversion[])Enum.GetValues(typeof(SevenChordInversion)))
                        {
                            var b = new SevenChordTransformationDetailedInfo();

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
                                b.FullLoopDistance = stepsTaken / 2;

                                var sum = b.TheStepsTransitions.Sum(st => st.TonicMoved);
                                if (sum == 0)
                                    b.StayInPlace = true;
                                else if (sum > 0)
                                    b.MovingUp = true;
                                else
                                    b.MovingDown = true;

                                b.TheApplications.RemoveRange(b.FullLoopDistance, b.TheApplications.Count - b.FullLoopDistance - 1);
                                b.TheStepsTransitions.RemoveRange((b.FullLoopDistance - 1), b.TheStepsTransitions.Count - b.FullLoopDistance);

                                if (origSize - prevSize == 0)
                                    b.SizeKept = true;

                                a.ApplicationsOnVariosChords.Add(b);
                            }
                        }
                if (a.ApplicationsOnVariosChords.Count > 0)
                    TransData.Add(a);
                counttt++;
                Console.WriteLine(TransData.Count + " :: " + counttt + " :: " + " [" + a.Formula[0] + "  " + a.Formula[1] + "  " + a.Formula[2] + "  " + a.Formula[3] + "] " + a.ApplicationsOnVariosChords.Count);
            }
            return TransData;
        }



        public static List<TriadTransformationWithData> CalculateTransDataForTriads(List<int[]> triadsTransformations)
        {
            var TransData = new List<TriadTransformationWithData>(); 

            int counttt = 0;
            foreach (var item in triadsTransformations)
            {
                var a = new TriadTransformationWithData();
                a.Formula = item;
                a.ApplicationsOnVariosTriads = new List<TriadTransformationWithDetailedInfo>();

                foreach (var triadtype in TriadTypes.TriadsTypes.Keys)
                    foreach (TriadMode mode in (TriadMode[])Enum.GetValues(typeof(TriadMode)))
                        foreach (TriadInversion inv in (TriadInversion[])Enum.GetValues(typeof(TriadInversion)))
                        {
                            var b = new TriadTransformationWithDetailedInfo();

                            b.InitialTriadType = triadtype;
                            b.InitialTriadMode = mode;
                            b.InitialTriadInversion = inv;

                            var testTriad = new Triad(JustNote.getNoteFromNumber(48), TriadTypes.GetChordFormula(triadtype), mode, inv);

                            b.TheApplications = new List<TriadSignature>();
                            b.TheStepsTransitions = new List<TriadStepsTransition>();

                            var bb = new TriadSignature();
                            bb.ChordType = testTriad.TriadType;
                            bb.Mode = testTriad.Mode;
                            bb.Inversion = testTriad.Inversion;
                            b.TheApplications.Add(bb);

                            var prevNotes = new JustNote[3];
                            for (int i = 0; i < 3; i++)
                                prevNotes[i] = testTriad.ChordNotes[i];

                            var prevSize = testTriad.GetChordSize();
                            var origSize = testTriad.GetChordSize();

                            int simpleLoopFound = 0;
                            int fullLoopFound = 0;

                            int stepsTaken = 0;
                            while (TriadTransformation.ApplyATransformation(testTriad, a.Formula) && !b.Cyclic)
                            {
                                var bbb = new TriadSignature();
                                bbb.ChordType = testTriad.TriadType;
                                bbb.Mode = testTriad.Mode;
                                bbb.Inversion = testTriad.Inversion;
                                b.TheApplications.Add(bbb);

                                var bbc = new TriadStepsTransition();
                                bbc.TonicMoved = testTriad.ChordNotes[0].getNoteNumber() - prevNotes[0].getNoteNumber();
                                bbc.ThirdMoved = testTriad.ChordNotes[1].getNoteNumber() - prevNotes[1].getNoteNumber();
                                bbc.FifthMoved = testTriad.ChordNotes[2].getNoteNumber() - prevNotes[2].getNoteNumber(); 

                                for (int i = 0; i < 3; i++)
                                    prevNotes[i] = JustNote.moveNoteBySemitones(prevNotes[i], a.Formula[i]);

                                if (prevNotes[0].getNoteNumber() == testTriad.ChordNotes[0].getNoteNumber())
                                    bbc.TonicInto = TriadStep.T;
                                else if (prevNotes[0].getNoteNumber() == testTriad.ChordNotes[1].getNoteNumber())
                                    bbc.TonicInto = TriadStep.III;
                                else if (prevNotes[0].getNoteNumber() == testTriad.ChordNotes[2].getNoteNumber())
                                    bbc.TonicInto = TriadStep.V; 

                                if (prevNotes[1].getNoteNumber() == testTriad.ChordNotes[0].getNoteNumber())
                                    bbc.ThirdInto = TriadStep.T;
                                else if (prevNotes[1].getNoteNumber() == testTriad.ChordNotes[1].getNoteNumber())
                                    bbc.ThirdInto = TriadStep.III;
                                else if (prevNotes[1].getNoteNumber() == testTriad.ChordNotes[2].getNoteNumber())
                                    bbc.ThirdInto = TriadStep.V; 

                                if (prevNotes[2].getNoteNumber() == testTriad.ChordNotes[0].getNoteNumber())
                                    bbc.FifthInto = TriadStep.T;
                                else if (prevNotes[2].getNoteNumber() == testTriad.ChordNotes[1].getNoteNumber())
                                    bbc.FifthInto = TriadStep.III;
                                else if (prevNotes[2].getNoteNumber() == testTriad.ChordNotes[2].getNoteNumber())
                                    bbc.FifthInto = TriadStep.V;  

                                bbc.SizeDifference = testTriad.GetChordSize() - prevSize;
                                prevSize = testTriad.GetChordSize();

                                b.TheStepsTransitions.Add(bbc);

                                for (int i = 0; i < 3; i++)
                                    prevNotes[i] = testTriad.ChordNotes[i];

                                stepsTaken++;

                                // TODO : More complicated and trusted way to determine exact cycle
                                if (testTriad.TriadType == triadtype) simpleLoopFound++;
                                if (testTriad.TriadType == triadtype && testTriad.Mode == mode && testTriad.Inversion == inv) fullLoopFound++;
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
                                b.FullLoopDistance = stepsTaken / 2;

                                var sum = b.TheStepsTransitions.Sum(st => st.TonicMoved);
                                if (sum == 0)
                                    b.StayInPlace = true;
                                else if (sum > 0)
                                    b.MovingUp = true;
                                else
                                    b.MovingDown = true;

                                b.TheApplications.RemoveRange(b.FullLoopDistance, b.TheApplications.Count - b.FullLoopDistance - 1);
                                b.TheStepsTransitions.RemoveRange((b.FullLoopDistance - 1), b.TheStepsTransitions.Count - b.FullLoopDistance);

                                if (origSize - prevSize == 0)
                                    b.SizeKept = true;

                                a.ApplicationsOnVariosTriads.Add(b);
                            }
                        }
                if (a.ApplicationsOnVariosTriads.Count > 0)
                    TransData.Add(a);
                counttt++;
                Console.WriteLine(TransData.Count + " :: " + counttt + " :: " + " [" + a.Formula[0] + "  " + a.Formula[1] + "  " + a.Formula[2] + "] " + a.ApplicationsOnVariosTriads.Count);
            }


            return TransData;
        }


    }
}
