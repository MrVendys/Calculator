using Calculator.Core;
using Calculator.Core.Exceptions;
using Calculator.Core.Strategies;

namespace Calculator.CoreTests
{
    [TestClass]
    public class PokrocileStrategieTest
    {
        //Odchylka u porovnávání desetinných čísel
        private const double delta = 0.00000001;

        [TestMethod]
        [DataRow(5, 0, 1)]
        [DataRow(-2, 3, -8)]
        [DataRow(3, -2, 0.11111111)]
        public void OperaceMocninaTest(double cislo1, double cislo2, double ocekavanyVysledek)
        {
            OperaceMocnina moc = new OperaceMocnina();

            double skutecnyVysledek = moc.Vypocitej(cislo1, cislo2);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek, delta);
        }

        [TestMethod]
        [DataRow(9, 3)]
        [DataRow(10, 3.16227766)]
        public void OperaceOdmocninaTest(double cislo1, double ocekavanyVysledek)
        {
            OperaceOdmocnina odm = new OperaceOdmocnina();

            double skutecnyVysledek = odm.Vypocitej(cislo1, ocekavanyVysledek);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek, delta);
        }

        [TestMethod]
        public void OperaceOdmocnina_ZaporneCislo_ChybovyKodChybaVeVypoctu()
        {
            double zaporneCislo = -4;
            OperaceOdmocnina odm = new OperaceOdmocnina();

            var exception = Assert.ThrowsException<NeplatnyVstupException>(() => odm.Vypocitej(zaporneCislo));

            Assert.AreEqual(exception.ChybovyKod, ChybovyKod.ChybaVeVypoctu);
        }

        [TestMethod]
        [DataRow(0, 1)]
        [DataRow(7, 5040)]
        public void OperaceFaktorialTest(double cislo1, double ocekavanyVysledek)
        {
            OperaceFaktorial fck = new OperaceFaktorial();

            double skutecnyVysledek = fck.Vypocitej(cislo1);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek);
        }

        [TestMethod]
        public void OperaceFaktorialTest_DesetinneCislo_ChybovyKodChybaVeVypoctu()
        {
            double desetinneCislo = 1.1;
            OperaceFaktorial fkt = new OperaceFaktorial();

            var exception = Assert.ThrowsException<NeplatnyVstupException>(() => fkt.Vypocitej(desetinneCislo));

            Assert.AreEqual(exception.ChybovyKod, ChybovyKod.ChybaVeVypoctu);
        }

        [TestMethod]
        [DataRow(10, 3, 1)]
        [DataRow(-7, 3, -1)]
        public void OperaceModuloTest(double cislo1, double cislo2, double ocekavanyVysledek)
        {
            OperaceModulo mod = new OperaceModulo();

            double skutecnyVysledek = mod.Vypocitej(cislo1, cislo2);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek);
        }

        [TestMethod]
        public void OperaceModulo_Nula_ChybovyKodDeleniNulou()
        {
            double cislo = 5;
            OperaceModulo mod = new OperaceModulo();

            var exception = Assert.ThrowsException<NeplatnyVstupException>(() => mod.Vypocitej(cislo, 0));

            Assert.AreEqual(exception.ChybovyKod, ChybovyKod.DeleniNulou);
        }
    }
}