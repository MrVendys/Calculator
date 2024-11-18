using Calculator.Core;
using Calculator.Core.Exceptions;
using Calculator.Core.Strategies;

namespace Calculator.CoreTests
{
    [TestClass]
    public class ZakladniStrategieTest
    {
        [TestMethod]
        [DataRow(5, 3, 8)]
        [DataRow(-2, 4, 2)]
        [DataRow(2.51233058, 3.20591493, 5.71824551)]
        public void PlusStrategyTest(double cislo1, double cislo2, double ocekavanyVysledek)
        {
            PlusStrategy op = new PlusStrategy();

            double skutecnyVysledek = op.Vypocitej(cislo1, cislo2);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek);
        }

        [TestMethod]
        [DataRow(10, 5, 5)]
        [DataRow(3, -4, 7)]
        [DataRow(7.5, 2.3, 5.2)]
        [DataRow(-6, -2, -4)]
        public void MinusStrategyTest(double cislo1, double cislo2, double ocekavanyVysledek)
        {
            MinusStrategy op = new MinusStrategy();

            double skutecnyVysledek = op.Vypocitej(cislo1, cislo2);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek);
        }

        [TestMethod]
        [DataRow(4, 3, 12)]
        [DataRow(0, 6, 0)]
        [DataRow(1.5, 2.5, 3.75)]
        [DataRow(-2, -4, 8)]
        public void MultiplyStrategyTest(double cislo1, double cislo2, double ocekavanyVysledek)
        {
            MultiplyStrategy op = new MultiplyStrategy();

            double skutecnyVysledek = op.Vypocitej(cislo1, cislo2);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek);
        }

        [TestMethod]
        [DataRow(10, 2, 5)]
        [DataRow(0, 5, 0)]
        [DataRow(7.5, 2.5, 3)]
        [DataRow(-6, -2, 3)]
        public void DivideStrategyTest(double cislo1, double cislo2, double ocekavanyVysledek)
        {
            DivideStrategy op = new DivideStrategy();

            double skutecnyVysledek = op.Vypocitej(cislo1, cislo2);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek);
        }

        [TestMethod]
        public void DivideStrategy_Nula_ChybovyKodDeleniNulou()
        {
            double cislo = 5;
            DivideStrategy op = new DivideStrategy();

            var exception = Assert.ThrowsException<InputValidationException>(() => op.Vypocitej(cislo, 0));

            Assert.AreEqual(exception.ChybovyKod, ChybovyKod.DeleniNulou);
        }
    }
}