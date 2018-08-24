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
    public class SecurityControllerTests
    {
        [TestMethod()]
        public void SecurityControllerTest()
        {
            // Assess
            String testPwd = "password";
            String testSalt = SecurityController.GenerateSalt();
            int testHashCode = SecurityController.GenerateHash(testPwd, testSalt);
            // Act
            bool result = SecurityController.ValidatePassword(testPwd, testSalt, testHashCode);
            // Assert
            Assert.IsTrue(result);
        }
    }
}