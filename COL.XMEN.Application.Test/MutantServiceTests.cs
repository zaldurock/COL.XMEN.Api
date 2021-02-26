using COL.XMEN.Core.DTO;
using COL.XMEN.Infraestructure.CosmosDB;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace COL.XMEN.Application.Test
{
    public class MutantServiceTests
    {
        private readonly Mock<IRepository<Mutant>> fakeRepository;

        public MutantServiceTests()
        {
            fakeRepository = new Mock<IRepository<Mutant>>();
        }

        [Fact]
        public async Task MutantServiceTests_Successful()
        {
            //arrange
            var mutan = new Mutant() { isMutant = true };
            fakeRepository.Setup(x => x.AddAsync(It.IsAny<Mutant>())).ReturnsAsync(mutan);

            var mutantservice = new MutantService(fakeRepository.Object);
            string[] dna = { "ATGCGA", "CAGTGC", "TTATGT", "AGAAGG", "CCCCTA", "TCACTG" };
            var mutant = new Mutant { dna = dna };
            var ismutantExcpected = true;

            //act
            var ismutantActual = await mutantservice.ValidateIsMutant(mutant);

            //assert
            Assert.Equal(ismutantExcpected, ismutantActual.isMutant);
        }

        [Fact]
        public async Task MutantServiceTests_DNANull_Exception()
        {
            //arrange
            var mutantservice = new MutantService(fakeRepository.Object);
            string[] dna = null;
            var mutant = new Mutant { dna = dna };

            //act
            var handleTask = mutantservice.ValidateIsMutant(mutant);

            //assert
            var result = await Assert.ThrowsAsync<Exception>(async () => await handleTask);
        }

        [Fact]
        public async Task MutantServiceTests_EntityDNANull_Exception()
        {
            //arrange
            var mutantservice = new MutantService(fakeRepository.Object);

            //act
            var handleTask = mutantservice.ValidateIsMutant(null);

            //assert
            var result = await Assert.ThrowsAsync<Exception>(async () => await handleTask);
        }
        
    }
}
