using Calculator.Core;
using Calculator.Core.Exceptions;
using Calculator.Core.Strategies;

namespace Calculator.CoreTests
{
    [TestClass]
    public class CountingTest
    {
        //Odchylka u porovnávání desetinných čísel
        private const double delta = 0.00000001;

        [TestMethod]
        [DataRow("1+2", 3)]
        [DataRow("3*(4/2)+5!", 126)]
        [DataRow("-5-4*5", -25)]
        [DataRow("(3+5*2-(√9)+7^2/4!*(2^3))+((8/4)+√16)*5-(3!)", 50.33333333)]
        public void VypocitejTest(string priklad, double ocekavanyVysledek)
        {
            Counting counting = GetCounting();

            bool valid = counting.TryPridejPriklad(priklad);

            Assert.IsTrue(valid);

            counting.Vypocitej();
            double skutecnyVysledek = double.Parse(counting.Priklad);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek, delta);
        }

        [TestMethod]
        public void PrazdnyPrikladHandledTest()
        {
            Counting counting = GetCounting();
            //Counting.Priklad je při inicializaci ""

            //Test, jestli je tento případ ošetřený a aplikace nespadne
            counting.Vypocitej();

            Assert.AreEqual("", counting.Priklad);
        }

        [TestMethod]
        [DataRow("1++")]
        [DataRow("()")]
        [DataRow("5!4")]
        [DataRow("+2")]
        public void Invalid_TryPridejPrikladTest(string priklad)
        {
            Counting counting = GetCounting();

            bool valid = counting.TryPridejPriklad(priklad);

            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void AddOperaceTest()
        {
            Counting counting = GetCounting();
            OperaceNahrad nahrad = new OperaceNahrad();
            counting.AddOperace(nahrad);

            Assert.IsTrue(counting.ZnakyOperaci.Contains(nahrad.ZnakOperatoru));
        }

        [TestMethod]
        public void VypocetNoveOperaceTest()
        {
            Counting counting = GetCounting();
            OperaceNahrad nahrad = new OperaceNahrad();
            counting.AddOperace(nahrad);

            bool valid = counting.TryPridejPriklad($"1{nahrad.ZnakOperatoru}2");

            Assert.IsTrue(valid);

            counting.Vypocitej();
            double skutecnyVysledek = double.Parse(counting.Priklad);

            Assert.AreEqual(2, skutecnyVysledek);
        }

        [TestMethod]
        public void AddOperace_Duplicitni_Exception()
        {
            Counting counting = GetCounting();
            DuplicitniOperace duplicitniOp = new DuplicitniOperace();

            var exception = Assert.ThrowsException<SpatnePouzitiException>(() => counting.AddOperace(duplicitniOp));

            Assert.AreEqual(exception.ChybovyKod, ChybovyKodSpatnePouziti.DuplicitniOperace);
        }

        [TestMethod]
        public void AddOperace_PrazdnySymbol_Exception()
        {
            Counting counting = GetCounting();
            PrazdnaOperace prazdnaOp = new PrazdnaOperace();

            var exception = Assert.ThrowsException<SpatnePouzitiException>(() => counting.AddOperace(prazdnaOp));

            Assert.AreEqual(exception.ChybovyKod, ChybovyKodSpatnePouziti.ChybiZnak);
        }

        #region Helpers

        private Counting GetCounting()
        {
            return new Counting();
        }

        #endregion

        #region Helper Classes

        private class OperaceNahrad : OperaceBase
        {
            public override char ZnakOperatoru => '@';

            public override double Vypocitej(double cislo1, double cislo2)
            {
                return cislo2;
            }
        }

        private class DuplicitniOperace : OperaceBase
        {
            public override char ZnakOperatoru => '+';
        }

        private class PrazdnaOperace : OperaceBase
        {
            public override char ZnakOperatoru => ' ';
        }

        #endregion
    }
}
