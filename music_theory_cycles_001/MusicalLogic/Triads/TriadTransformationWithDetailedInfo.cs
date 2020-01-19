using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static music_theory_cycles_001.TriadTypes;

namespace music_theory_cycles_001
{ 
    public class TriadTransformationWithDetailedInfo
    {
        public string InitialTriadType { get; set; }
        public TriadMode InitialTriadMode { get; set; }
        public TriadInversion InitialTriadInversion { get; set; }

        public List<string> SameToItselfOnType { get; set; }
        public List<int> SameToItselfOnTypeXonFromStep { get; set; }

        public bool Cyclic { get; set; }

        //public int CrashedDistance { get; set; }

        public int LoopByTypeDistance { get; set; }
        public int FullLoopDistance { get; set; }

        public bool MovingDown { get; set; }
        public bool MovingUp { get; set; }
        public bool StayInPlace { get; set; }

        public bool SizeKept { get; set; }

        public List<TriadSignature> TheApplications { get; set; }
        public List<TriadStepsTransition> TheStepsTransitions { get; set; }
    }

    public struct TriadSignature
    {
        public string ChordType;
        public TriadMode Mode;
        public TriadInversion Inversion;
    }

    public struct TriadStepsTransition
    {
        public int TonicMoved;
        public int ThirdMoved;
        public int FifthMoved; 

        public TriadStep TonicInto;
        public TriadStep ThirdInto;
        public TriadStep FifthInto; 

        public int SizeDifference;
    }

    public enum TriadStep
    {
        T,
        III,
        V 
    }
}
