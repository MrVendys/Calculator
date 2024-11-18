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
        [DataRow('1',"")]
        [DataRow('(',"1+")]
        [DataRow(')',"1+(2+3")]
        [DataRow('+',"2")]
        //Test přidávání symbolu do rozpracovaného příkladu
        public void ValidateSymbol(char symbol, string priklad)
        {
            _counting.TryPridejPriklad(priklad);

            bool valid = _validator.ValidatePridejSymbol(symbol);

            Assert.IsTrue(valid);
        }

        [TestMethod]
        [DataRow("1+2*3")]
        [DataRow("3/(5+2)")]
        [DataRow("4!*5+√4")]
        public void ValidatePriklad(string priklad)
        {
            bool valid = _validator.ValidatePridejPriklad(priklad);

            Assert.IsTrue(valid);
        }

        [TestMethod]
        [DataRow('a')]
        [DataRow('#')]
        //Test nepovolenych znaku
        public void Invalid_ValidatePridejSymbol(char symbol)
        {
            bool valid = _validator.ValidatePridejSymbol(symbol);

            Assert.IsFalse(valid);
        }

        [TestMethod]
        [DataRow('+', "")]
        [DataRow('√', "2")]
        [DataRow('-', "2+")]
        [DataRow(')', "1+")]
        [DataRow('(', "1+(2+3")]
        //Test nepřidávání symbolu do rozpracovaného příkladu
        public void Invalid_ValidateSymbol(char symbol, string priklad)
        {
            _counting.TryPridejPriklad(priklad);

            bool valid = _validator.ValidatePridejSymbol(symbol);

            Assert.IsFalse(valid);
        }
    }
}
