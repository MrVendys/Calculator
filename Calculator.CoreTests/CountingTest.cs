using Calculator.Core;
using Calculator.Core.Exceptions;

namespace Calculator.CoreTests
{
    [TestClass]
    public class CountingTest
    {
        private const double delta = 0.00000001;

        [TestMethod]
        [DataRow("1+2", 3)]
        [DataRow("3*(4/2)+5!", 126)]
        [DataRow("-5-4*5", -25)]
        [DataRow("√(2^2)", 2)]
        [DataRow("0", 0)]
        [DataRow("(3+5*2-(√9)+7^2/4!*(2^3))+((8/4)+√16)*5-(3!)", 50.33333333)]
        public void VypocitejTest(string priklad, double ocekavanyVysledek)
        {
            Counting counting = GetCounting();
            bool valid = counting.TryPridejPriklad(priklad);
            counting.Vypocitej();

            double skutecnyVysledek = double.Parse(counting.Priklad);

            Assert.IsTrue(valid);
            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek, delta);
        }

        [TestMethod]
        [DataRow("1++")]
        [DataRow("()")]
        [DataRow("5!4")]
        [DataRow("+2")]
        public void TryPridejPrikladTest(string priklad)
        {
            Counting counting = GetCounting();

            bool valid = counting.TryPridejPriklad(priklad);

            Assert.IsFalse(valid);
        }

        private Counting GetCounting()
        {
            return new Counting();
        }
    }
}
