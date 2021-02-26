using COL.XMEN.Core.Domain;

namespace COL.XMEN.Core.DTO
{
    public class Mutant : Entity
    {
        public string[] dna { get; set; }

        public string dnaAll { get; set; }

        public bool isMutant { get; set; }
    }
}
