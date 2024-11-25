using Calculator.Core;

namespace Calculator.CoreTests
{
    [TestClass]
    public class PrikladValidatorTest
    {
        [TestMethod]
        [DataRow("", '1')]
        [DataRow("1+", '(')]
        [DataRow("1+(2+3", ')')]
        [DataRow("2", '+')]
        public void ValidatePridejSymbol(string priklad, char symbol)
        {
            PrikladValidator prikladValidator = GetValidator(out Counting counting);
            bool prikladValid = counting.TryPridejPriklad(priklad);

            Assert.IsTrue(prikladValid);

            bool symbolValid = prikladValidator.ValidatePridejSymbol(symbol);

            Assert.IsTrue(symbolValid);
        }

        [TestMethod]
        [DataRow('a')]
        [DataRow('{')]
        public void ValidatePridejSymbol_NepovolenySymbol_False(char symbol)
        {
            PrikladValidator prikladValidator = GetValidator(out _);

            bool valid = prikladValidator.ValidatePridejSymbol(symbol);

            Assert.IsFalse(valid);
        }

        [TestMethod]
        [DataRow("(1+2)", '1')]
        [DataRow("1!", '1')]
        [DataRow("", '+')]
        [DataRow("", ')')]
        [DataRow("1+", ')')]
        [DataRow("(", ')')]
        [DataRow("1", '(')]
        [DataRow("(1+2)", '(')]
        [DataRow("5!", '(')]
        [DataRow("1+", '+')]
        [DataRow("1", '√')]
        [DataRow("(1+2)", '√')]
        [DataRow("(", '+')]
        [DataRow("√", '+')]
        //Test na přidávání symbolů, které nejdou přidat do rozpracovaného příkladu
        public void ValidatePridejSymbol_LogickyNespravnySymbol_False(string priklad, char symbol)
        {
            PrikladValidator prikladValidator = GetValidator(out Counting counting);
            bool prikladValid = counting.TryPridejPriklad(priklad);

            Assert.IsTrue(prikladValid);

            bool symbolValid = prikladValidator.ValidatePridejSymbol(symbol);

            Assert.IsFalse(symbolValid);
        }

        [TestMethod]
        public void ValidatePridejSymbol_DesetinnyOddelovac_PrazdnyPriklad_False()
        {
            PrikladValidator prikladValidator = GetValidator(out Counting counting);

            bool symbolValid = prikladValidator.ValidatePridejSymbol(counting.DesetinnyOddelovac[0]);

            Assert.IsFalse(symbolValid);
        }

        [TestMethod]
        public void ValidatePridejSymbol_DesetinnyOddelovac_Duplicitni_False()
        {
            PrikladValidator prikladValidator = GetValidator(out Counting counting);
            bool prikladValid = counting.TryPridejPriklad($"12{counting.DesetinnyOddelovac[0]}34");

            Assert.IsTrue(prikladValid);

            bool symbolValid = prikladValidator.ValidatePridejSymbol(counting.DesetinnyOddelovac[0]);

            Assert.IsFalse(symbolValid);
        }

        [TestMethod]
        [DataRow("3/(5+2)")]
        [DataRow("4!*5+√4")]
        public void ValidatePridejPriklad(string priklad)
        {
            PrikladValidator prikladValidator = GetValidator(out _);

            bool valid = prikladValidator.ValidatePridejPriklad(priklad);

            Assert.IsTrue(valid);
        }

        #region Helpers

        private PrikladValidator GetValidator(out Counting counting)
        {
            counting = new Counting();
            return new PrikladValidator(counting);
        }

        #endregion
    }
}
