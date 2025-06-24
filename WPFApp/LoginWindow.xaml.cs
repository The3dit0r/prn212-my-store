using Microsoft.VisualBasic.ApplicationServices;
using Services;
using System.Windows;


namespace WPFApp;

/// <summary>
/// Interaction logic for LoginWindow.xaml
/// </summary>
public partial class LoginWindow : Window
{
  private readonly IAccountService service;


  public LoginWindow()
  {
    InitializeComponent();
    service = new AccountService();
  }

  private void Login_Click(object sender, RoutedEventArgs e)
  {
    var username = UsernameInput.Text;
    var password = PasswordInput.Password;

    try
    {
      var user = service.Login(username, password);
      MessageWindow.Show(new MessageOptions
      {
        Title = "Success",
        Message = "Login successfully!",
        Description = $"Welcome back, {user.FullName}",
        Image = MessageImage.Success,
        Owner = GetWindow(this)
      });


      this.DialogResult = true;
    }
    catch (Exception ex)
    {
      MessageWindow.Show(new MessageOptions
      {
        Title = "Failed",
        Message = "Login failed!",
        Description = $"Error: {ex.Message}",
        Image = MessageImage.Warning,
        Owner = GetWindow(this)
      });
    }
  }
}
