using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFApp;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
  bool _loading = false;

  bool Loading {
    get => _loading;
    set {
      if (value == _loading) return;

      _loading = value;

      if (value) {
        LoadingString(13, -3);
      }
    }
  }

  public MainWindow(bool disableLoad) {
    InitializeComponent();

    if (disableLoad) {
      LoadingText.Text = "Please login to continue";
      LoadingStatus.Opacity = 0;
      ButtonsControl.Opacity = 1;
      Loading = false;

      return;
    }
  }

  public MainWindow() {
    InitializeComponent();

    ButtonsControl.Opacity = 0;
    Loading = true;

    FakeDelayLoadLoginWindow();
  }

  private async void LoadingString(int len, int cur) {
    var str = "▉";

    if (!Loading) {
      LoadingStatus.Opacity = 0;
      return; 
    }

    LoadingStatus.Opacity = 1;
    LoadingFrame.Text = "";
    for (int i = 0; i < len; i++) {
      bool upper = i <= cur;
      bool lower = i >= cur - 3;
      LoadingFrame.Text += $"{(upper && lower ? str : " ")}";
    }

    LoadingFrame.Text += "";


    await Task.Delay(60);
    LoadingString(len, cur + 1 > len + 2 ? -3 : cur + 1);
  }

  private async void FakeDelayLoadLoginWindow() {
    await Task.Delay(2000);
    LoadAuthWindow();
  }

  private async void LoadAuthWindow() {
    LoadingText.Text = "Waiting for authentication\n\n. . . . .";
    Loading = false;
    ButtonsControl.Opacity = 0;

    Window win = new LoginWindow() {
      Owner = Window.GetWindow(this)
    };

    bool? success = win.ShowDialog();
    if (success == true) {
      Loading = true;
      ButtonsControl.IsEnabled = false;

      LoadingText.Text = "Initialize UI";
      await Task.Delay(1000);
      LoadingText.Text = "Launching application";
      await Task.Delay(1000);

      LaunchMainApplication();
    } else {
      MessageBox.Show("Authentication failed! Please try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      ButtonsControl.Opacity = 1;
      LoadingText.Text = "Please login to continue";
      Loading = false;
      //Application.Current.Shutdown();
    }
  }

  private void LaunchMainApplication() {
    var initial = Application.Current.MainWindow;
    Application.Current.MainWindow = new MainApplication();
    Application.Current.MainWindow.Show();

    initial.Close();
  }

  private void LoginButton_Click(object sender, RoutedEventArgs e) {
    LoadAuthWindow();
  }

  private void CloseButton_Click(object sender, RoutedEventArgs e) {
    Application.Current.Shutdown();
  }
}
