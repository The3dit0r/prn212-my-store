using Microsoft.IdentityModel.Tokens;
using System.Windows;
using System.Windows.Controls;

namespace WPFApp;

public partial class TestWindows : Window
{
  public TestWindows()
  {
    InitializeComponent();

    List<Enum> ImageEnums = new List<Enum> {
      MessageImage.Info,
      MessageImage.Question,
      MessageImage.Warning,
      MessageImage.Error,
      MessageImage.Success
    };

    List<Enum> ActionsEnums = new List<Enum> {
      MessageActions.Ok,
      MessageActions.YesNo,
      MessageActions.CancelOk,
      MessageActions.TestOnly
    };

    FillComboBoxWithEnums(ImageInput, ImageEnums);
    FillComboBoxWithEnums(ActionsInput, ActionsEnums);
  }

  public void FillComboBoxWithEnums<TEnum>(ComboBox comboBox, List<TEnum> enumList) where TEnum : Enum
  {
    if (comboBox == null) return;
    if (enumList == null) return;
    if (enumList.IsNullOrEmpty()) return;

    comboBox.ItemsSource = enumList;
    comboBox.SelectedIndex = 0;
  }

  private void Button_Click(object sender, RoutedEventArgs e)
  {
    var title = TitleInput.Text;
    var message = MessageInput.Text;
    var desc = DescriptionInput.Text;
    var image = (MessageImage)ImageInput.SelectedItem;
    var actions = (MessageActions)ActionsInput.SelectedItem;


    var results = MessageWindow.Show(new MessageOptions
    {
      Title = title,
      Message = message,
      Description = desc,
      Image = image,
      Actions = actions,
      Owner = GetWindow(this)
    });

    TextResults.Text = "Latest results: " + results.ToString();
  }
}
