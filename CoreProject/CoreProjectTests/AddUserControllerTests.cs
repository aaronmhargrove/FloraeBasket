using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoreProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CoreProject.Tests
{
    [TestClass()]
    public class AddUserControllerTests
    {
        // Test the method with valid inputs, result should be true
        [TestMethod()]
        public void createNewUserTestTrue()
        {
            bool result = AddUserController.CreateNewUser("Bob", "Bob123", "Hunter21!", 0);
            Assert.IsTrue(result);
        }

        // Test the method with invalid inputs, result should be false
        [TestMethod()]
        public void createNewUserTestFalse()
        {
            bool result = AddUserController.CreateNewUser("", "Bob123", "Hunter21!", 0);
            Assert.IsFalse(result);

            result = AddUserController.CreateNewUser("Bob", "", "Hunter21!", 0);
            Assert.IsFalse(result);

            result = AddUserController.CreateNewUser("Bob", "Bob123", "", 0);
            Assert.IsFalse(result);
        }

        // Batch Import Tests
        [TestMethod()]
        public void BatchImportTest()
        {
            // Assess
            // One .csv File for each basis path
            String goodCSV = "..\\..\\..\\CoreProject\\Resources\\testBatch.csv";
            String badCSV = "..\\..\\..\\CoreProject\\Resources\\testBatchBad.csv";
            String emptyCSV = "..\\..\\..\\CoreProject\\Resources\\testBatchEmpty.csv";

            // Basis Path 1 - correct .csv file formatting
            // Act
            List<String> result = AddUserController.BatchImport(goodCSV);
            // Assert - all 6 users added.
            Assert.IsTrue(result.Count() == 6);

            // Basis Path 2 - incorrect .csv file formatting
            // Act
            result = AddUserController.BatchImport(badCSV);
            // Assert - triggers error and fourth item in returned list is "--Batch Import File Error"
            Assert.IsTrue(result[3].Equals("--Batch Import File Error"));

            // Basis Path 3 - empty .csv file
            // Act 
            result = AddUserController.BatchImport(emptyCSV);
            // Assert - file is empty and returned list is empty
            Assert.IsTrue(result.Count == 0);
        }
    }
}