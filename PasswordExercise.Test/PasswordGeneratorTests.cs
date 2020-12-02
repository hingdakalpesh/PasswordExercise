using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasswordExercise.Models;

namespace PasswordExcercise.Tests
{
    [TestClass]
    public class PasswordGeneratorTests
    {
        [TestMethod]
        public void TestGenerateLengthOnly()
        {
            PasswordGenerator generator = new PasswordGenerator();

            string result = generator.GeneratePassword(new PasswordRequirements()
            {
                MaxLength = 16,
                MinLength = 8,
            });

            Assert.IsTrue(result.Length >= 8 && result.Length <= 16);
        }

        [TestMethod]
        public void TestGenerateAllRequirements()
        {
            PasswordGenerator generator = new PasswordGenerator();

            string result = generator.GeneratePassword(new PasswordRequirements()
            {
                MaxLength = 16,
                MinLength = 8,
                MinLowerAlphaChars = 1,
                MinUpperAlphaChars = 1,
                MinNumericChars = 1,
                MinSpecialChars = 1
            });

            Assert.IsTrue(result.Length >= 8 && result.Length <= 16);
            Assert.IsTrue(result.Any(char.IsUpper));
            Assert.IsTrue(result.Any(char.IsLower));
            Assert.IsTrue(result.Any(char.IsNumber));
            Assert.IsTrue(result.Any(char.IsSymbol) || result.Any(char.IsPunctuation));
        }

        [TestMethod]
        public void TestGenerateAllRequirments_Multiple()
        {
            PasswordGenerator generator = new PasswordGenerator();

            string result = generator.GeneratePassword(new PasswordRequirements()
            {
                MaxLength = 8,
                MinLength = 8,
                MinLowerAlphaChars = 2,
                MinUpperAlphaChars = 2,
                MinNumericChars = 2,
                MinSpecialChars = 2
            });

            Assert.IsTrue(result.Length == 8);
            Assert.IsTrue(result.Where(char.IsUpper).Count() == 2);
            Assert.IsTrue(result.Where(char.IsLower).Count() == 2);
            Assert.IsTrue(result.Where(char.IsNumber).Count() == 2);

            int countSpecial = result.Count(char.IsSymbol) + result.Count(char.IsPunctuation);
            Assert.IsTrue(countSpecial == 2);
        }

        #region Additional tests added by Kalpesh
        [TestMethod]
        public void TestNullPasswordRequirements()
        {
            //Arrange
            PasswordGenerator generator = new PasswordGenerator();
            PasswordRequirements requirements = null;
            
            var expectedExcetpion = new Exception("Password requirements are null.");
            Exception resultExcetpion = null;

            //Act
            try
            {
                string result = generator.GeneratePassword(requirements);
            }
            catch (Exception ex)
            {
                resultExcetpion = ex;
            }

            //Assert
            Assert.AreEqual(expectedExcetpion.Message, resultExcetpion.Message);
        }

        [TestMethod]
        public void TestInvalidMinLength()
        {
            //Arrange
            PasswordGenerator generator = new PasswordGenerator();
            var requirements = new PasswordRequirements()
            {
                MaxLength = 8,
                MinLength = 16,
            };
            var expectedExcetpion = new Exception("MinLength cannot be greater than MaxLength.");
            Exception resultExcetpion = null;

            //Act
            try
            {
                string result = generator.GeneratePassword(requirements);
            }
            catch (Exception ex)
            {
                resultExcetpion = ex;
            }

            //Assert
            Assert.AreEqual(expectedExcetpion.Message, resultExcetpion.Message);
        }

        [TestMethod]
        public void TestInvalidPasswordParameters_Multiple()
        {
            //Arrange
            PasswordGenerator generator = new PasswordGenerator();
            var requirements = new PasswordRequirements()
            {
                MaxLength = 16,
                MinLength = 8,
                MinLowerAlphaChars = 5,
                MinNumericChars =4,
                MinSpecialChars=4,
                MinUpperAlphaChars =4
            };
            var expectedExcetpion = new Exception("Combination of MinLowerAlphaChars, MinNumericChars, MinSpecialChars and MinUpperAlphaChars cannot be greater than MaxLength.");
            Exception resultExcetpion = null;

            //Act
            try
            {
                string result = generator.GeneratePassword(requirements);
            }
            catch (Exception ex)
            {
                resultExcetpion = ex;
            }

            //Assert
            Assert.AreEqual(expectedExcetpion.Message, resultExcetpion.Message);
        }
        #endregion
    }
}
