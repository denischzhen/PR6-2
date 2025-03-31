using PR6_2;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PR6_2
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        public void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;

            using (var context = Entities.GetNewContext())
            {
                var (success, message) = TryLogin(login, password, context);
                if (success)
                {
                    MessageBox.Show("Успешный вход!", "Добро пожаловать", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public (bool success, string message) TryLogin(string login, string password, Entities context)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                return (false, "Введите логин и пароль!");
            }

            var user = context.Clients.FirstOrDefault(c => c.Email == login || c.PhoneNumber == login);

            if (user == null)
            {
                return (false, "Пользователь не найден!");
            }

            if (user.PasswordHash != HashPassword(password))
            {
                return (false, "Неверный пароль!");
            }

            return (true, "Успешный вход!");
        }

        public void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            this.Close();
        }

        private bool isPasswordVisible = false;

        public void TogglePassword_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (isPasswordVisible)
            {
                PasswordBox.Visibility = Visibility.Visible;
                VisiblePasswordBox.Visibility = Visibility.Collapsed;
                PasswordBox.Password = VisiblePasswordBox.Text;
                TogglePasswordText.Text = "Показать пароль 👁";
            }
            else
            {
                VisiblePasswordBox.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Collapsed;
                VisiblePasswordBox.Text = PasswordBox.Password;
                TogglePasswordText.Text = "Скрыть пароль 🔒";
            }

            isPasswordVisible = !isPasswordVisible;
        }

        public static string HashPassword(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }
    }
}