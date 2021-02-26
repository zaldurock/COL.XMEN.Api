using COL.XMEN.Core.DTO;
using COL.XMEN.Infraestructure.CosmosDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COL.XMEN.Application
{
    /// <summary>
    /// Mutant Service Class
    /// </summary>
    public class MutantService : IMutantService
    {
        private readonly IRepository<Mutant> _mutantRepository;

        public MutantService(IRepository<Mutant> mutantRepository)
        {
            _mutantRepository = mutantRepository;
        }

        /// <summary>
        /// This methods valid the dna 
        /// </summary>
        /// <param name="mutantDNA">DNA</param>
        /// <returns>Is Mutant</returns>
        public async Task<Mutant> ValidateIsMutant(Mutant mutantDNA)
        {
            if (mutantDNA is null)
                throw new Exception("The Entity DNA is null");

            if (mutantDNA.dna is null)
                throw new Exception("The DNA is null");

            var matrix = LoadMatrix(mutantDNA.dna);
            var possibilities = PossibilitiesDNA(matrix);
            var matchDNA = MatchSectionDNA(possibilities);
            mutantDNA.dnaAll = TransformDNA(mutantDNA.dna);

            if (matchDNA > 1)
                mutantDNA = await CheckSaveNewRecord(mutantDNA, true);
            else
                mutantDNA = await CheckSaveNewRecord(mutantDNA, false);

            return mutantDNA;
        }

        /// <summary>
        /// Report Mutant Stats
        /// </summary>
        /// <returns>The Mutant Stasts Class</returns>
        public async Task<MutantStats> StatsMutant()
        {
            var resultSearchMutant = await _mutantRepository
              .Where("isMutant = true")
              .ConfigureAwait(false);

            var resultSearchNotMutant = await _mutantRepository
              .Where("isMutant = false")
              .ConfigureAwait(false);

            double ratio = resultSearchMutant.Count() / (resultSearchNotMutant.Count() == 0 ? 1 : resultSearchNotMutant.Count());

            var stats = new MutantStats
            {
                count_human_dna = resultSearchNotMutant.Count(),
                count_mutant_dna = resultSearchMutant.Count(),
                ratio = ratio
            };

            return stats;
        }

        /// <summary>
        /// Transform and concat the DNA
        /// </summary>
        /// <param name="dna">The DNA</param>
        /// <returns>string DNA</returns>
        private string TransformDNA(string[] dna)
        {
            string dnaresponse = string.Empty;

            foreach (var item in dna)
            {
                dnaresponse += item;
            }

            return dnaresponse;
        }

        /// <summary>
        /// Check and Save new records
        /// </summary>
        /// <param name="mutantDNA">Mutant DNA</param>
        /// <param name="isMutant">Is Mutant</param>
        /// <returns>Class Mutant save or searched</returns>
        private async Task<Mutant> CheckSaveNewRecord(Mutant mutantDNA, bool isMutant)
        {
            var resultSearch = await _mutantRepository
               .Where($"dnaAll = '{mutantDNA.dnaAll}'")
               .ConfigureAwait(false);

            var result = new Mutant();

            if (!resultSearch.Any())
            {
                mutantDNA.isMutant = isMutant;
                mutantDNA.Id = Guid.NewGuid();
                mutantDNA.CreatedDate = DateTime.Now;
                result = await _mutantRepository.AddAsync(mutantDNA).ConfigureAwait(false);
            }
            else
            {
                result = resultSearch.FirstOrDefault();
            }

            return result;
        }

        /// <summary>
        /// Match the section on DNA if is human o mutant
        /// </summary>
        /// <param name="possibilities">kind of diferences</param>
        /// <returns>number the mutant match</returns>
        private int MatchSectionDNA(List<string> possibilities)
        {
            var matchDNA = 0;

            foreach (var item in possibilities)
            {
                for (int i = 0; i < item.Length; i++)
                {
                    var comper = item[i];
                    var flagMutan = 0;

                    for (int x = i; x < item.Length; x++)
                    {
                        if (comper == item[x])
                            flagMutan++;
                    }

                    if (flagMutan == 4)
                    {
                        matchDNA++;
                        break;
                    }
                }
            }

            return matchDNA;
        }

        /// <summary>
        /// Add king of possibilities
        /// </summary>
        /// <param name="matrix">The Matrix loaded</param>
        /// <returns>List of possiblities</returns>
        private List<string> PossibilitiesDNA(string[,] matrix)
        {
            var possibilities = new List<string>();
            var diagonal = DiagonalMatrix(matrix);
            var across = AcrossMatrix(matrix);
            var down = DownMatrix(matrix);

            possibilities.AddRange(diagonal);
            possibilities.AddRange(across);
            possibilities.AddRange(down);

            return possibilities;
        }

        /// <summary>
        /// Load the initial Matrix
        /// </summary>
        /// <param name="dna">The DNA to check</param>
        /// <returns></returns>
        private string[,] LoadMatrix(string[] dna)
        {
            var sizeMatriz = dna[0].Length;
            var matrix = new string[sizeMatriz, sizeMatriz];

            for (int row = 0; row < sizeMatriz; row++)
            {
                for (int col = 0; col < sizeMatriz; col++)
                {
                    matrix[row, col] = dna[row].Substring(col, 1);
                }
            }

            if (matrix.GetLength(0) != matrix.GetLength(1))
                throw new Exception("The Matrix not NxN");

            return matrix;
        }

        /// <summary>
        /// Search the section of DNA in diagonal matrix
        /// </summary>
        /// <param name="mat">matrix to search</param>
        /// <returns>the section of DNA</returns>
        private string[] DiagonalMatrix(string[,] mat)
        {
            var sizeMatrix = mat.GetLength(0);
            string[] array = new string[1];
            var frac = string.Empty;

            for (int row = 0; row < sizeMatrix; row++)
            {
                for (int col = 0; col < sizeMatrix; col++)
                {
                    if (row == col)
                    {
                        frac += mat[row, col];
                    }
                }
            }

            array[0] = frac;

            return array;
        }

        /// <summary>
        /// Search the section of DNA in across matrix
        /// </summary>
        /// <param name="mat">matrix to search</param>
        /// <returns>the section of DNA</returns>
        private string[] AcrossMatrix(string[,] mat)
        {
            var sizeMatrix = mat.GetLength(0);
            string[] array = new string[sizeMatrix];

            for (int row = 0; row < sizeMatrix; row++)
            {
                var frac = string.Empty;
                for (int col = 0; col < sizeMatrix; col++)
                {
                    frac += mat[row, col];
                }
                array[row] = frac;
            }

            return array;
        }

        /// <summary>
        /// Search the section of DNA in down matrix
        /// </summary>
        /// <param name="mat">matrix to search</param>
        /// <returns>the section of DNA</returns>
        private string[] DownMatrix(string[,] mat)
        {
            var sizeMatrix = mat.GetLength(0);
            string[] array = new string[sizeMatrix];

            for (int row = 0; row < sizeMatrix; row++)
            {
                var frac = string.Empty;
                for (int col = 0; col < sizeMatrix; col++)
                {
                    frac += mat[col, row];
                }
                array[row] = frac;
            }

            return array;
        }

       
    }
}
