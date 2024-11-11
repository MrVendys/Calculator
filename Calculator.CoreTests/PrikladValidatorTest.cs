using Calculator.Core;

namespace Calculator.CoreTests
{
    [TestClass]
    public class PrikladValidatorTest
    {
        private Counting _counting;
        private PrikladValidator _validator;

        public PrikladValidatorTest()
        {
            _counting = new Counting();
            _validator = new PrikladValidator(_counting);
        }

        [TestMethod]
        [DataRow('1')]
        [DataRow('(')]
        [DataRow('+')]
        [DataRow('√')]
        public void ValidatePridejSymbol(char symbol)
        {
            bool valid = _validator.ValidatePridejSymbol(symbol);

            Assert.IsTrue(valid);
        }

        [TestMethod]
        [DataRow("1+2*3")]
        [DataRow("3/(5+2)")]
        [DataRow("4!*5+√4")]
        public void ValidatePridejPriklad(string priklad)
        {
            bool valid = _validator.ValidatePridejPriklad(priklad);

            Assert.IsTrue(valid);
        }
    }
}
