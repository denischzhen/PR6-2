using Microsoft.VisualStudio.TestTools.UnitTesting;
using PR6_2;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UnitTestProject
{
    [TestClass]
    public class RegistrationTests
    {
        private RegistrationWindow _registrationWindow;
        private Entities _context;

        [TestInitialize]
        public void Setup()
        {
            _registrationWindow = new RegistrationWindow();
            _context = Entities.GetContext();

            _registrationWindow.FullNameTextBox.Text = "Тестовый Пользователь";
            _registrationWindow.BirthDatePicker.SelectedDate = new DateTime(1990, 1, 1);
            _registrationWindow.PhoneNumberTextBox.Text = "+79991234567";
            _registrationWindow.EmailTextBox.Text = "testuser@example.com";
            _registrationWindow.PasswordBox.Password = "TestPass123!";
            _registrationWindow.ConfirmPasswordBox.Password = "TestPass123!";
        }

        [TestCleanup]
        public void Cleanup()
        {
            var testUser = _context.Clients.FirstOrDefault(c => c.Email == "testuser@example.com");
            if (testUser != null)
            {
                _context.Clients.Remove(testUser);
                _context.SaveChanges();
            }

            _context.Dispose();
            _registrationWindow.Close();
        }

        [TestMethod]
        public void Register_ValidData_Succeeds()
        {
            // Act
            _registrationWindow.RegisterButton_Click(null, null);

            // Assert
            var user = _context.Clients.FirstOrDefault(c => c.Email == "testuser@example.com");
            Assert.IsNotNull(user, "Пользователь должен быть добавлен в базу данных");
            Assert.AreEqual("Тестовый Пользователь", user.FullName, "ФИО должно совпадать");
            Assert.AreEqual(new DateTime(1990, 1, 1), user.BirthDate, "Дата рождения должна совпадать");
            Assert.AreEqual("+79991234567", user.PhoneNumber, "Номер телефона должен совпадать");
            Assert.AreEqual("testuser@example.com", user.Email, "Email должен совпадать");
            Assert.AreEqual(RegistrationWindow.HashPassword("TestPass123!"), user.PasswordHash, "Хеш пароля должен совпадать");
        }

        [TestMethod]
        public void Register_EmptyFields_Fails()
        {
            // Arrange
            _registrationWindow.FullNameTextBox.Text = "";

            // Act
            _registrationWindow.RegisterButton_Click(null, null);

            // Assert
            var user = _context.Clients.FirstOrDefault(c => c.Email == "testuser@example.com");
            Assert.IsNull(user, "Пользователь не должен быть добавлен в базу данных из-за пустых полей");
        }

        [TestMethod]
        public void Register_PasswordsDoNotMatch_Fails()
        {
            // Arrange
            _registrationWindow.ConfirmPasswordBox.Password = "DifferentPass123!";

            // Act
            _registrationWindow.RegisterButton_Click(null, null);

            // Assert
            var user = _context.Clients.FirstOrDefault(c => c.Email == "testuser@example.com");
            Assert.IsNull(user, "Пользователь не должен быть добавлен в базу данных из-за несовпадения паролей");
        }

        [TestMethod]
        public void Register_DuplicateEmail_Fails()
        {
            // Arrange
            _registrationWindow.EmailTextBox.Text = "ivanovv@example.com";


            // Act
            try
            {
                _registrationWindow.RegisterButton_Click(null, null);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsTrue(ex.Message.Contains("unique"), "Должна быть ошибка из-за дублирования email");
                var user = _context.Clients.Count(c => c.Email == "ivanov@example.com");
                Assert.AreEqual(1, user, "Не должно быть добавлено дублирующих записей с email");
                return;
            }

            Assert.Fail("Должна быть ошибка регистрации из-за дублирования email");
        }

        [TestMethod]
        public void Register_DuplicatePhoneNumber_Fails()
        {
            // Arrange
            _registrationWindow.PhoneNumberTextBox.Text = "+79123456789";

            // Act
            try
            {
                _registrationWindow.RegisterButton_Click(null, null);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsTrue(ex.Message.Contains("unique"), "Должна быть ошибка из-за дублирования номера телефона");
                var user = _context.Clients.Count(c => c.PhoneNumber == "+79123456789");
                Assert.AreEqual(1, user, "Не должно быть добавлено дублирующих записей с номером телефона");
                return;
            }

            Assert.Fail("Должна быть ошибка регистрации из-за дублирования номера телефона");
        }
    }
}