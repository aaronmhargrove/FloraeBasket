using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoreProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreProject.Tests
{
    [TestClass()]
    public class LoginControllerTests
    {

        [TestMethod()]
        public void SaltAndHashTest()
        {
            // Assess
            LoginController ctrl = new LoginController();
            int expected = 84891720;
            int actual;

            // Act
            actual = ctrl.SaltAndHash("password", "rlrrlrllrl");

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void IsValidTest()
        {
            // Assess
            LoginController ctrl = new LoginController();
            Boolean expected1 = false;
            Boolean expected2 = true;

            // Act
            Boolean actual1 = ctrl.IsValid("  ");
            Boolean actual2 = ctrl.IsValid("Testing");

            // Assert
            Assert.AreEqual(expected1, actual1);
            Assert.AreEqual(expected2, actual2);
        }

        [TestMethod()]
        public void IsMatchTest()
        {
            // Assess
            LoginController ctrl = new LoginController();
            String expected1 = "Dr.Bloom";
            String actual1;
            User actual2 = null;

            // Act
            actual1 = ctrl.IsMatch("admin", "password").GetName();
            actual2 = ctrl.IsMatch("amdin", "pasword");

            // Assert
            Assert.AreEqual(expected1, actual1);
            Assert.AreEqual(null, actual2);
        }
    }
}