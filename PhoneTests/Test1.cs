using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ClassLibrary;

namespace PhoneTests
{
    [TestClass]
    public class Test1
    {
        //TESTY KONSTRUKTORA I WŁAŚCIWOŚCI

        [TestMethod]
        public void Constructor_ValidParameters_CreatesPhoneProperly()
        {
            // Arrange
            string owner = "Jan Kowalski";
            string number = "123456789";

            // Act
            Phone phone = new Phone(owner, number);

            // Assert
            Assert.AreEqual(owner, phone.Owner);
            Assert.AreEqual(number, phone.PhoneNumber);
            Assert.AreEqual(0, phone.Count);
            Assert.AreEqual(100, phone.PhoneBookCapacity);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        public void Constructor_InvalidOwner_ThrowsArgumentException(string invalidOwner)
        {
            // Arrange
            string validNumber = "123456789";

            // Act & Assert
            var ex = Assert.ThrowsExactly<ArgumentException>(() => new Phone(invalidOwner, validNumber));
            Assert.IsTrue(ex.Message.Contains("Owner name is empty or null!"));
        }

        [TestMethod]
        [DataRow(null, "Phone number is empty or null!")]
        [DataRow("", "Phone number is empty or null!")]
        [DataRow("12345678", "Invalid phone number!")] //Zbyt krótki
        [DataRow("1234567890", "Invalid phone number!")] //Zbyt długi
        [DataRow("1234a6789", "Invalid phone number!")] //Zawiera literę
        public void Constructor_InvalidPhoneNumber_ThrowsArgumentException(string invalidNumber, string expectedMessage)
        {
            // Arrange
            string validOwner = "Jan Kowalski";

            // Act & Assert
            var ex = Assert.ThrowsExactly<ArgumentException>(() => new Phone(validOwner, invalidNumber));
            Assert.IsTrue(ex.Message.Contains(expectedMessage));
        }
    }
}