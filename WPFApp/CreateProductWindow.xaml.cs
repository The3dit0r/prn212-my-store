
using Services;
using BusinessObjects;
using System.Windows;
using System.ComponentModel;
using Microsoft.IdentityModel.Tokens;

namespace WPFApp;

public partial class CreateProductWindow : Window, INotifyPropertyChanged {
  private readonly ProductService pservice;
  private readonly CategoryService cservice;

  List<Category> categories;

  Product _product = new Product();

  public event PropertyChangedEventHandler? PropertyChanged;

  public Product CurProd {
    get => _product;
    set {
      _product = value;
      OnPropertyChanged(nameof(CurProd));
    }
  }


  private void PopulateComboBox(List<Category> cat) {
    var boxItems = cat.Select((c) =>
      $"{c.CategoryId}: {c.CategoryName}"
     );

    CategoryComboList.ItemsSource = boxItems;
  }
  public CreateProductWindow() {
    InitializeComponent();
    pservice = new ProductService();
    cservice = new CategoryService();
    categories = cservice.GetCategories();

    ClearInput();
    //DataContext = CurProd;

    PopulateComboBox(categories);
  }

  private void AddButton_Click(object sender, RoutedEventArgs e) {
    try {
      pservice.AddProduct(CurProd);
      MessageBox.Show("Successfully added new item!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
      DialogResult = true;
    } catch (Exception err) {
      MessageBox.Show($"An error occured:\n{err.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
  }

  private void OnPropertyChanged(string name) {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
  }

  private void CategoryComboList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
    CurProd.CategoryId = categories[CategoryComboList.SelectedIndex].CategoryId;
    ValidateData();
  }

  private void ValidateData() {
    var name = CurProd.ProductName.Trim();

    var c1 = name.Length > 0 && name.Length <= 40;
    var c2 = CurProd.CategoryId != -1;
    var c3 = CurProd.UnitPrice >= 0;
    var c4 = CurProd.UnitsInStock >= 0;
    //MessageBox.Show("Current CATID:" + CurProd.CategoryId);

    var cf = c1 && c2 && c3 && c4;

    AddButton.IsEnabled = cf;

    //StatusBlock.Text = $"{c1} {c2} {c3} {c4}";
  }

  private void ClearInput() {
    CurProd.ProductName = "";
    CurProd.UnitPrice = -1;
    CurProd.UnitsInStock = -1;
    CurProd.CategoryId = categories[0] != null ? categories[0].CategoryId : -1;

    PriceText.Text = "";
    UiSText.Text = "";
    PriceText.Text = "";
    CategoryComboList.SelectedIndex = 0;
  }

  private void Name_TextChanged(object sender, object e) {
    CurProd.ProductName = NameText.Text;
    ValidateData();
  }

  private void UiS_TextChanged(object sender, object e) {
    try { 
      CurProd.UnitsInStock = short.Parse(UiSText.Text);
    } catch {
      CurProd.UnitsInStock = -1;
    }

    ValidateData();
  }

  private void Price_TextChanged(object sender, object e) {
    try {
      CurProd.UnitPrice = decimal.Parse(PriceText.Text);
    } catch {
      CurProd.UnitPrice = -1;
    }
    ValidateData();
  }
}
