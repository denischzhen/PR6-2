using Microsoft.VisualStudio.TestTools.UnitTesting;
using PR6_2;
using System.Linq;

namespace UnitTestProject
{
    [TestClass]
    public class LoginTests
    {
        private LoginWindow _loginWindow;
        private Entities _context;

        [TestInitialize]
        public void Setup()
        {
            _loginWindow = new LoginWindow();
            _context = Entities.GetContext();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Dispose();
            _loginWindow.Close();
        }

        [TestMethod]
        public void Login_ValidEmailAndPassword_Succeeds()
        {
            // Act
            var (success, message) = _loginWindow.TryLogin("ivanov@example.com", "Pass123!", _context);

            // Assert
            Assert.IsTrue(success, "Авторизация должна быть успешной");
            Assert.AreEqual("Успешный вход!", message, "Сообщение должно быть о успешном входе");
        }

        [TestMethod]
        public void Login_ValidPhoneNumberAndPassword_Succeeds()
        {
            // Act
            var (success, message) = _loginWindow.TryLogin("+79123456789", "Pass123!", _context);

            // Assert
            Assert.IsTrue(success, "Авторизация должна быть успешной");
            Assert.AreEqual("Успешный вход!", message, "Сообщение должно быть о успешном входе");
        }

        [TestMethod]
        public void Login_InvalidPassword_Fails()
        {
            // Act
            var (success, message) = _loginWindow.TryLogin("ivanov@example.com", "WrongPass", _context);

            // Assert
            Assert.IsFalse(success, "Авторизация должна завершиться ошибкой");
            Assert.AreEqual("Неверный пароль!", message, "Сообщение должно быть о неверном пароле");
        }

        [TestMethod]
        public void Login_NonExistentUser_Fails()
        {
            // Act
            var (success, message) = _loginWindow.TryLogin("nonexistent@example.com", "Pass123!", _context);

            // Assert
            Assert.IsFalse(success, "Авторизация должна завершиться ошибкой");
            Assert.AreEqual("Пользователь не найден!", message, "Сообщение должно быть о несуществующем пользователе");
        }

        [TestMethod]
        public void Login_EmptyFields_Fails()
        {
            // Act
            var (success, message) = _loginWindow.TryLogin("", "", _context);

            // Assert
            Assert.IsFalse(success, "Авторизация должна завершиться ошибкой");
            Assert.AreEqual("Введите логин и пароль!", message, "Сообщение должно быть о пустых полях");
        }
    }
}