using Microsoft.IdentityModel.Tokens;
using System.Windows;
using System.Windows.Media.Imaging;


namespace WPFApp;

public enum MessageImage
{
  Info,
  Warning,
  Error,
  Question,
  Success,
}

public enum MessageActions
{
  Ok,
  YesNo,
  CancelOk,
  TestOnly
}

public enum MessageResults
{
  Ok,
  Yes,
  No,
  Cancel,
  None
}

public class MessageOptions
{
  public string Title { get; set; } = "Message";
  public string Message { get; set; } = "";
  public string Description { get; set; } = "";
  public MessageImage Image { get; set; } = MessageImage.Info;
  public MessageActions Actions { get; set; } = MessageActions.Ok;

  public Window? Owner { get; set; }
}

public partial class MessageWindow : Window
{

  public MessageResults Results = MessageResults.None;

  private void ShowUIElements(List<UIElement> elements, params int[] excepts)
  {
    if (elements == null) return;

    for (int i = 0; i < elements.Count; i++)
    {
      if (excepts.Contains(i))
      {
        elements[i].Visibility = Visibility.Collapsed;
      }
      else
      {
        elements[i].Visibility = Visibility.Visible;
      }
    }
  }

  public void SetImage(MessageImage img)
  {
    string ImageSource = "";

    switch (img)
    {
      case MessageImage.Info:
        {
          ImageSource = "Assets/info.png";
          break;
        }

      case MessageImage.Warning:
        {
          ImageSource = "Assets/warning.png";
          break;
        }

      case MessageImage.Error:
        {
          ImageSource = "Assets/error.png";
          break;
        }

      case MessageImage.Question:
        {
          ImageSource = "Assets/question.png";
          break;
        }

      case MessageImage.Success:
        {
          ImageSource = "Assets/success.png";
          break;
        }
    }

    if (ImageSource.Length > 0)
    {

      string strUri = $"pack://application:,,,/{ImageSource}";
      CmpImage.Source = new BitmapImage(new Uri(strUri));

      CmpImage.Width = 60;
      CmpImage.Height = 60;
    }
    else
    {
      CmpImage.Width = 0;
      CmpImage.Height = 0;
    }
  }

  public void SetActions(MessageActions actions)
  {
    List<UIElement> buttons = new List<UIElement> { BttCancel, BttOk, BttYes, BttNo };

    switch (actions)
    {
      case MessageActions.CancelOk:
        {
          ShowUIElements(buttons, 2, 3);
          break;
        }

      case MessageActions.YesNo:
        {
          ShowUIElements(buttons, 0, 1);
          break;
        }

      case MessageActions.TestOnly:
        {
          ShowUIElements(buttons);
          break;
        }

      case MessageActions.Ok:
      default:
        {
          ShowUIElements(buttons, 0, 2, 3);
          break;
        }
    }
  }

  public void SetMessage(string msg)
  {
    if (msg.IsNullOrEmpty())
    {
      CmpMsg.Visibility = Visibility.Collapsed;
      return;
    }

    CmpMsg.Visibility = Visibility.Visible;
    CmpMsg.Text = msg;
  }

  public void SetDescription(string desc)
  {
    if (desc.IsNullOrEmpty())
    {
      CmpDesc.Visibility = Visibility.Collapsed;
      return;
    }

    CmpDesc.Visibility = Visibility.Visible;
    CmpDesc.Text = desc;

  }

  public void SetTitle(string title)
  {
    this.Title = $"My Store DB | {title}";
  }

  public MessageWindow(string title, string message, string desc, MessageImage image, MessageActions actions)
  {
    InitializeComponent();

    SetTitle(title);
    SetMessage(message);
    SetDescription(desc);
    SetImage(image);
    SetActions(actions);

    //this.SizeToContent = SizeToContent.Height;
    this.SizeToContent = SizeToContent.WidthAndHeight;
  }

  public MessageWindow()
  {
    InitializeComponent();
    //this.SizeToContent = SizeToContent.Height;
    this.SizeToContent = SizeToContent.WidthAndHeight;
  }

  private void ActionBtt_Click(object sender, RoutedEventArgs e)
  {
    if (sender is not System.Windows.Controls.Button btn) return;

    var name = btn.Name;
    var results = MessageResults.None;

    //MessageBox.Show(name, "Clicked on");

    switch (name)
    {
      case "BttOk": { results = MessageResults.Ok; break; }
      case "BttCancel": { results = MessageResults.Cancel; break; }
      case "BttYes": { results = MessageResults.Yes; break; }
      case "BttNo": { results = MessageResults.No; break; }

      default:
        {
          results = MessageResults.None;
          break;
        }
    }

    this.Results = results;
    this.DialogResult = true;
  }

  public static MessageResults Show(MessageOptions options)
  {
    var PopUp = new MessageWindow(
      options.Title,
      options.Message,
      options.Description,
      options.Image,
      options.Actions
    )
    {
      Owner = options.Owner
    };

    if (options.Owner == null)
    {
      PopUp.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }
    else
    {
      PopUp.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    }

    PopUp.ShowDialog();
    return PopUp.Results;
  }

}

