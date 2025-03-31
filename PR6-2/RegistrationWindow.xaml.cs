using PR6_2;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PR6_2
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка на пустые значения
                if (string.IsNullOrEmpty(FullNameTextBox.Text) ||
                    BirthDatePicker.SelectedDate == null ||
                    string.IsNullOrEmpty(PhoneNumberTextBox.Text) ||
                    string.IsNullOrEmpty(EmailTextBox.Text) ||
                    string.IsNullOrEmpty(PasswordBox.Password) ||
                    string.IsNullOrEmpty(ConfirmPasswordBox.Password))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.");
                    return;
                }

                // Проверка на совпадение паролей
                if (PasswordBox.Password != ConfirmPasswordBox.Password)
                {
                    MessageBox.Show("Пароли не совпадают.");
                    return;
                }

                // Получение введенных данных
                string fullName = FullNameTextBox.Text;
                DateTime birthDate = BirthDatePicker.SelectedDate.Value;
                string phoneNumber = PhoneNumberTextBox.Text;
                string email = EmailTextBox.Text;
                string password = PasswordBox.Password;

                // Создание нового клиента
                Clients newClient = new Clients
                {
                    FullName = fullName,
                    BirthDate = birthDate,
                    PhoneNumber = phoneNumber,
                    Email = email,
                    RegistrationDate = DateTime.Now,
                    PasswordHash = HashPassword(password),
                    //RoleID = 2 // Присваиваем роль, например, роль "Пользователь"
                };

                // Добавление клиента в базу данных
                using (var context = Entities.GetContext())
                {
                    context.Clients.Add(newClient);
                    context.SaveChanges();
                }

                MessageBox.Show("Регистрация прошла успешно!");

                // После успешной регистрации открываем окно авторизации
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}");
            }
        }

        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        public bool isPasswordVisible = false;

        public void TogglePassword_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (isPasswordVisible)
            {
                PasswordBox.Visibility = Visibility.Visible;
                ConfirmPasswordBox.Visibility = Visibility.Visible;
                VisiblePasswordBox.Visibility = Visibility.Collapsed;
                VisibleConfirmPasswordBox.Visibility = Visibility.Collapsed;

                TogglePasswordIcon.Text = "👁";
            }
            else
            {
                VisiblePasswordBox.Visibility = Visibility.Visible;
                VisibleConfirmPasswordBox.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Collapsed;
                ConfirmPasswordBox.Visibility = Visibility.Collapsed;

                VisiblePasswordBox.Text = PasswordBox.Password;
                VisibleConfirmPasswordBox.Text = ConfirmPasswordBox.Password;

                TogglePasswordIcon.Text = "🔒";
            }

            isPasswordVisible = !isPasswordVisible;
        }

        // Метод для хеширования пароля
        public static string HashPassword(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }
    }
}
