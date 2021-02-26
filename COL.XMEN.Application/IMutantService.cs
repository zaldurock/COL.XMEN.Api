using COL.XMEN.Core.DTO;
using System.Threading.Tasks;

namespace COL.XMEN.Application
{
    /// <summary>
    /// Mutant Interface Class
    /// </summary>
    public interface IMutantService
    {
        /// <summary>
        /// This methods valid the dna 
        /// </summary>
        /// <param name="mutantDNA">DNA</param>
        /// <returns>Is Mutant</returns>
        Task<Mutant> ValidateIsMutant(Mutant mutantDNA);

        /// <summary>
        /// Report Mutant Stats
        /// </summary>
        /// <returns>The Mutant Stasts Class</returns>
        Task<MutantStats> StatsMutant();
    }
}
