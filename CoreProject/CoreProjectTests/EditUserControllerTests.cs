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
    public class EditUserControllerTests
    {
        [TestMethod()]
        public void EditUserTest()
        {
            // Assess
            User testUser = new User("John", "John123", "password", 1);
            bool result;

            //Basis Path 1 - All inputs match current values
            // Act
            result = EditUserController.EditUser("John", "John123", "password", 1, testUser);
            // Assert
            Assert.IsTrue(result);

            //Basis Path 2 - Input name does not match current value
            // Act
            result = EditUserController.EditUser("Jonny", "John123", "password", 1, testUser);
            // Assert
            Assert.IsTrue(result);

            //Basis Path 3 - Input user name does not match current value
            // Act
            result = EditUserController.EditUser("John", "johnboy", "password", 1, testUser);
            // Assert
            Assert.IsTrue(result);

            //Basis Path 4 - Input password does not match current value
            // Act
            result = EditUserController.EditUser("John", "John123", "hunter21", 1, testUser);
            // Assert
            Assert.IsTrue(result);

            //Basis Path 5 - Input type does not match current value
            // Act
            result = EditUserController.EditUser("John", "John123", "password", 12, testUser);
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void DeleteUserTest()
        {
            // Assess
            User testUser = new User("John", "John123", "password", 1);
            // Act
            bool result = EditUserController.DeleteUser(testUser);
            // Assert
            Assert.IsTrue(result);
        }
        
    }
}