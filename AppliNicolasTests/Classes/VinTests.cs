using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppliNicolas.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppliNicolas.Classes.Tests
{
    [TestClass()]
    public class VinTests
    {       
        [TestMethod()]
        public void RecupereVinDansBDDTest()
        {
            ConnexionBD.Instance.ConfigurerConnexion("home", "home");

            Vin vin = new  Vin();

            List<Vin> lesVins = vin.RecupereVinDansBDD();
            Assert.IsInstanceOfType(lesVins, typeof(List<Vin>), "Le try block devrait retourner une List<Vin>");
        }
    }
}