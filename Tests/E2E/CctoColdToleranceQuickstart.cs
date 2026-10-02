using RimWorks.Quickstarts;
using Verse;

namespace CropColdToleranceOverhaul.E2E
{
    public sealed class CctoColdToleranceQuickstart : AbstractQuickstart
    {
        public override TaggedString description
        {
            get
            {
                return "Deterministic small colony used only by CCTO Pickle end-to-end tests.";
            }
        }

        public override int mapSize
        {
            get { return 50; }
        }

        public override string seed
        {
            get { return "CCTO-Cold-Tolerance-E2E"; }
        }
    }
}
