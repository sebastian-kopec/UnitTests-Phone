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

        //TESTY METODY AddContact

        [TestMethod]
        public void AddContact_NewContact_ReturnsTrueAndIncreasesCount()
        {
            //Arrange
            Phone phone = new Phone("Jan", "123456789");

            //Act
            bool result = phone.AddContact("Anna", "987654321");

            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, phone.Count);
        }

        [TestMethod]
        public void AddContact_ExistingContact_ReturnsFalseAndCountRemainsUnchanged()
        {
            //Arrange
            Phone phone = new Phone("Jan", "123456789");
            phone.AddContact("Anna", "987654321");

            //Act
            bool result = phone.AddContact("Anna", "111222333");

            //Assert
            Assert.IsFalse(result);
            Assert.AreEqual(1, phone.Count); //Liczba kontaktów nie powinna wzrosnąć
        }

        [TestMethod]
        public void AddContact_PhoneBookFull_ThrowsInvalidOperationException()
        {
            //Arrange
            Phone phone = new Phone("Jan", "123456789");

            //Wypełniamy książkę telefoniczną do pełna (100 kontaktów)
            for (int i = 0; i < phone.PhoneBookCapacity; i++)
            {
                phone.AddContact($"Osoba{i}", "111222333");
            }

            //Act & Assert
            var ex = Assert.ThrowsExactly<InvalidOperationException>(() => phone.AddContact("NadmiarowaOsoba", "999888777"));
            Assert.AreEqual("Phonebook is full!", ex.Message);
        }

        //TESTY METODY Call

        [TestMethod]
        public void Call_ExistingContact_ReturnsProperMessage()
        {
            //Arrange
            Phone phone = new Phone("Jan", "123456789");
            string contactName = "Anna";
            string contactNumber = "987654321";
            phone.AddContact(contactName, contactNumber);

            //Act
            string result = phone.Call(contactName);

            //Assert
            string expectedMessage = $"Calling {contactNumber} ({contactName}) ...";
            Assert.AreEqual(expectedMessage, result);
        }

        [TestMethod]
        public void Call_NonExistingContact_ThrowsInvalidOperationException()
        {
            //Arrange
            Phone phone = new Phone("Jan", "123456789");
            phone.AddContact("Anna", "987654321");

            //Act & Assert
            var ex = Assert.ThrowsExactly<InvalidOperationException>(() => phone.Call("Tomasz"));
            Assert.AreEqual("Person doesn't exists!", ex.Message);
        }
    }
}