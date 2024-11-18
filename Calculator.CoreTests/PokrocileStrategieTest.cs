using Calculator.Core.Exceptions;
using Calculator.Core;
using Calculator.Core.Strategies;

namespace Calculator.CoreTests
{
    [TestClass]
    public class PokrocileStrategieTest
    {
        //Odchylka u porovnávání desetinných čísel
        private const double delta = 0.00000001;

        [TestMethod]
        [DataRow(2, 3, 8)]
        [DataRow(5, 0, 1)]
        [DataRow(-2, 3, -8)]
        [DataRow(4, 0.5, 2)]
        [DataRow(3, -2, 0.11111111)]
        public void PowerStrategyTest(double cislo1, double cislo2, double ocekavanyVysledek)
        {
            PowerStrategy pow = new PowerStrategy();

            double skutecnyVysledek = pow.Vypocitej(cislo1, cislo2);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek, delta);
        }

        [TestMethod]
        [DataRow(9, 3)]
        [DataRow(0, 0)]
        [DataRow(2.25, 1.5)]
        [DataRow(10, 3.16227766)]
        public void SquareRootStrategyTest(double cislo1, double ocekavanyVysledek)
        {
            SquareRootStrategy sqr = new SquareRootStrategy();

            double skutecnyVysledek = sqr.Vypocitej(cislo1, ocekavanyVysledek);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek, delta);
        }

        [TestMethod]
        [DataRow(0, 1)]
        [DataRow(7, 5040)]
        public void FactorialStrategyTest(double cislo1, double ocekavanyVysledek)
        {
            FactorialStrategy fct = new FactorialStrategy();

            double skutecnyVysledek = fct.Vypocitej(cislo1);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek);
        }

        [TestMethod]
        public void FactorialStrategy_DesetinneCislo_ChybovyKodChybaVeVypoctu()
        {
            double desetinneCislo = 1.1;
            FactorialStrategy fct = new FactorialStrategy();

            var exception = Assert.ThrowsException<InputValidationException>(() => fct.Vypocitej(desetinneCislo));

            Assert.AreEqual(exception.ChybovyKod, ChybovyKod.ChybaVeVypoctu);
        }

        [TestMethod]
        [DataRow(10, 3, 1)]
        [DataRow(9, -4, 1)]
        [DataRow(-7, 3, -1)]
        [DataRow(15, 5, 0)]
        public void ModuloStrategyTest(double cislo1, double cislo2, double ocekavanyVysledek)
        {
            ModuloStrategy mod = new ModuloStrategy();

            double skutecnyVysledek = mod.Vypocitej(cislo1, cislo2);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek);
        }

        [TestMethod]
        public void ModuloStrategy_Nula_ChybovyKodDeleniNulou()
        {
            double cislo = 5;
            ModuloStrategy mod = new ModuloStrategy();

            var exception = Assert.ThrowsException<InputValidationException>(() => mod.Vypocitej(cislo, 0));

            Assert.AreEqual(exception.ChybovyKod, ChybovyKod.DeleniNulou);
        }
    }
}