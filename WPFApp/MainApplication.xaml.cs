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

    private string _query;
    public string Query {
      get => _query;
      set {
        _query = value;
      }
    }

    public string ProdStat {
      get => _prodStat;
      set {
        _prodStat = value;
        OnPropertyChanged(nameof(ProdStat));
      }
    }

    private Product curProduct = new Product();
    public Product CurProduct {
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

    public MainApplication() {
      InitializeComponent();
      DataContext = this;

      pservice = new ProductService();
      cservice = new CategoryService();

      products = pservice.GetProducts();
      categories = cservice.GetCategories();
      PopulateComboBox();

      ProdDataGrid.ItemsSource = products;
      ProdStat = $"Total products: {products.Count}";

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

        CategoryComboList.SelectedIndex = Array.IndexOf(
          categories.Select(c => c.CategoryId).ToArray(),
          CurProduct.CategoryId
        );
      }
    }

    private void Update_Click(object sender, RoutedEventArgs e) {
      pservice.UpdateProduct(CurProduct);
    }

    private void CategoryComboList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      if (firstView) {
        firstView = false;
        return;
      }

      CurProduct.CategoryId = categories[CategoryComboList.SelectedIndex].CategoryId;
      // Not using binding, the only way to fix
      pservice.UpdateProduct(CurProduct);
      products = pservice.GetProducts();
      ProdDataGrid.ItemsSource = products;
    }
  }
}
