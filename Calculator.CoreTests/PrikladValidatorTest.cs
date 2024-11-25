using Calculator.Core;

namespace Calculator.CoreTests
{
    [TestClass]
    public class PrikladValidatorTest
    {
        [TestMethod]
        [DataRow('1', "")]
        [DataRow('(', "1+")]
        [DataRow(')', "1+(2+3")]
        [DataRow('+', "2")]
        public void ValidatePridejSymbol(char symbol, string priklad)
        {
            PrikladValidator prikladValidator = GetValidator(out Counting counting);
            bool valid = counting.TryPridejPriklad(priklad);

            Assert.IsTrue(valid);

            valid = prikladValidator.ValidatePridejSymbol(symbol);

            Assert.IsTrue(valid);
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
        [DataRow('1', "(1+2)")]
        [DataRow('1', "1!")]
        [DataRow('+', "")]
        [DataRow(')', "")]
        [DataRow(')', "1+")]
        [DataRow(')', "(")]
        [DataRow('(', "1")]
        [DataRow('(', "(1+2)")]
        [DataRow('(', "5!")]
        [DataRow('+', "1+")]
        [DataRow('√', "1")]
        [DataRow('√', "(1+2)")]
        [DataRow('+', "(")]
        [DataRow('+', "√")]
        //Test na přidávání symbolů, které nejdou přidat do rozpracovaného příkladu
        public void ValidatePridejSymbol_LogickyNespravnySymbol_False(char symbol, string priklad)
        {
            PrikladValidator prikladValidator = GetValidator(out Counting counting);
            bool valid = counting.TryPridejPriklad(priklad);

            Assert.IsTrue(valid);

            valid = prikladValidator.ValidatePridejSymbol(symbol);

            Assert.IsFalse(valid);
        }

        [DataRow("")]
        [DataRow("12.34")]
        public void ValidatePridejSymbol_DesetinnyOddelovac_False(string priklad)
        {
            PrikladValidator prikladValidator = GetValidator(out Counting counting);
            bool valid = counting.TryPridejPriklad(priklad);

            Assert.IsTrue(valid);

            valid = prikladValidator.ValidatePridejSymbol(counting.DesetinnyOddelovac.First());

            Assert.IsFalse(valid);
        }

        [TestMethod]
        [DataRow("3/(5+2)")]
        [DataRow("4!*5+√4")]
        public void ValidatePriklad(string priklad)
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
