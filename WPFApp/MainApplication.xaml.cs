using BusinessObjects;
using Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFApp {
  /// <summary>
  /// Interaction logic for MainApplication.xaml
  /// </summary>
  public partial class MainApplication : Window, INotifyPropertyChanged {
    private readonly ProductService pservice;
    private readonly CategoryService cservice;

    private List<Product> products;
    private List<Category> categories;

    private bool firstView = false;

    private string _prodStat = "Status: Unknown";

    public event PropertyChangedEventHandler? PropertyChanged;

    //private string _query;
    //public string Query {
    //  get => _query;
    //  set {
    //    _query = value;
    //  }
    //}

    public string ProdStat {
      get => _prodStat;
      set {
        _prodStat = value;
        OnPropertyChanged(nameof(ProdStat));
      }
    }

    private Product? curProduct;
    public Product? CurProduct {
      get => curProduct;
      set {
        curProduct = value;
        OnPropertyChanged(nameof(CurProduct));
      }
    }

    private void PopulateComboBox() {
      var boxItems = categories.Select((c) =>
        $"{c.CategoryId}: {c.CategoryName}"
       );

      CategoryComboList.ItemsSource = boxItems;
    }

    private void RefreshDataList() {
      products = pservice.GetProducts();
      ProdDataGrid.ItemsSource = products;
      ProdStat = $"Total products: {products.Count}";
    }

    public MainApplication() {
      InitializeComponent();
      DataContext = this;

      pservice = new ProductService();
      cservice = new CategoryService();

      categories = cservice.GetCategories();
      PopulateComboBox();

      RefreshDataList();

      ToggleSidebar(false);
    }

    private void ToggleSidebar(bool p) {
      Sidebar.Width = p ? 410 : 0;
    }

    private void OnPropertyChanged(string name) {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private void CloseSide_Click(object sender, RoutedEventArgs e) {
      ToggleSidebar(false);
    }

    private void ProdDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      if (sender is DataGrid) {
        firstView = true;

        CurProduct = (Product)((DataGrid)sender).SelectedItem;

        ProductDetails.DataContext = CurProduct;

        ToggleSidebar(true);

        if (null == CurProduct) {
          CategoryComboList.SelectedIndex = -1;
          return; 
        }
        int curProId = CurProduct.CategoryId ?? -1;


        CategoryComboList.SelectedIndex = Array.IndexOf(
          categories.Select(c => c.CategoryId).ToArray(),
          curProId
        );
      }
    }

    private void Update_Click(object sender, RoutedEventArgs e) {
      if (CurProduct == null) return;
      pservice.UpdateProduct(CurProduct);
      RefreshDataList();
    }

    private void CategoryComboList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      if (CurProduct == null) return;

      if (firstView) {
        firstView = false;
        return;
      }

      CurProduct.CategoryId = categories[CategoryComboList.SelectedIndex].CategoryId;
      // Not using binding, the only way to fix
      pservice.UpdateProduct(CurProduct);
      RefreshDataList();
    }

    private void Remove_Click(object sender, RoutedEventArgs e) {
      if (CurProduct == null) return;

      string d = $"Product: [ID: {CurProduct.ProductId}] {CurProduct.ProductName}";

      var ans = MessageBox.Show(
        "Are you sure you want to remove the following item?\n" 
        + d + "\n\nThis action is not revertable",
        "Confirmation",
        MessageBoxButton.YesNo,
        MessageBoxImage.Warning
      );

      if (ans != MessageBoxResult.Yes) {
        return;
      }

      pservice.DeleteProduct(CurProduct);
      RefreshDataList();

      try {
        CurProduct = null;
        MessageBox.Show("Product removed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
      } catch { }
    }

    private void LaunchMainWindow() {
      var initial = Application.Current.MainWindow;
      Application.Current.MainWindow = new MainWindow(true);
      Application.Current.MainWindow.Show();

      initial.Close();
    }

    private void Exit_Click(object sender, RoutedEventArgs e) {
      var ans = MessageBox.Show(
        "Are you sure you want to exit?",
        "Confirmation",
        MessageBoxButton.YesNo,
        MessageBoxImage.Question
      );

      if (ans != MessageBoxResult.Yes) {
        return;
      }

      LaunchMainWindow();
    }

    private void NewProduct_Click(object sender, RoutedEventArgs e) {
      var wind = new CreateProductWindow() {
        Owner = Window.GetWindow(this)
      };

      if (wind.ShowDialog() != true) return;
      RefreshDataList();
    }
    private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e) {
      if (e.PropertyName == "Category") {
        e.Column = null;
      }
    }
  }
}
