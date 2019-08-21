using System.Collections.Generic;
using static music_theory_cycles_001.SevenChordTypes;

namespace music_theory_cycles_001
{
    public class TransformationDetailedInfo
    {
        public string InitialChordType { get; set; } 
        public List<string> SameToItselfOnType { get; set; }
        public List<int> SameToItselfOnTypeXonFromStep { get; set; }

        public bool Cyclic { get; set; } 

        public int CrashedDistance { get; set; }

        public int LoopByTypeDistance { get; set; }
        public int FullLoopDistance { get; set; } 

        public bool MovingDown { get; set; }
        public bool MovingUp { get; set; } 

        public bool SizeKept { get; set; }

        public List<SevenChordSignature> TheApplications { get; set; }
        public List<ChordStepsTransition> TheStepsTransitions { get; set; }
    }

    public struct SevenChordSignature
    {
        public string ChordType;
        public SevenChordMode Mode;
        public SevenChordInversion Inversion; 
    }

    public struct ChordStepsTransition
    {
        public int TonicMoved;
        public int ThirdMoved;
        public int FifthMoved;
        public int SeventhMoved;

        public SevenChordStep TonicInto;
        public SevenChordStep ThirdInto;
        public SevenChordStep FifthInto;
        public SevenChordStep SeventhInto; 
    }

    public enum SevenChordStep
    {
        T,
        III,
        V,
        VII
    }
}