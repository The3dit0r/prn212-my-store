using Services;
using System.Windows;


namespace WPFApp;

/// <summary>
/// Interaction logic for LoginWindow.xaml
/// </summary>
public partial class LoginWindow : Window {
  private readonly IAccountService service;


  public LoginWindow() {
    InitializeComponent();
    service = new AccountService();
  }

  private void Login_Click(object sender, RoutedEventArgs e) {
    var username = UsernameInput.Text;
    var password = PasswordInput.Password;

    try {
      var user = service.Login(username, password);
      MessageBox.Show($"Login successfully\nWelcome, {user.FullName}", "Login successful", MessageBoxButton.OK, MessageBoxImage.Information);
      this.DialogResult = true;
    } catch (Exception ex) {
      MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
  }
}
