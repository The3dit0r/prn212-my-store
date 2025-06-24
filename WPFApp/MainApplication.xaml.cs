using BusinessObjects;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.ApplicationServices;
using Services;
using System;
using System.Collections;
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

namespace WPFApp
{
  /// <summary>
  /// Interaction logic for MainApplication.xaml
  /// </summary>
  public partial class MainApplication : Window, INotifyPropertyChanged
  {
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

    public string ProdStat
    {
      get => _prodStat;
      set
      {
        _prodStat = value;
        OnPropertyChanged(nameof(ProdStat));
      }
    }

    private Product? curProduct;
    public Product? CurProduct
    {
      get => curProduct;
      set
      {
        curProduct = value;
        OnPropertyChanged(nameof(CurProduct));
      }
    }

    private void PopulateComboBox()
    {
      var boxItems = categories.Select((c) =>
        $"{c.CategoryId}: {c.CategoryName}"
       );

      CategoryComboList.ItemsSource = boxItems;
    }

    private void RefreshDataList()
    {
      var newProducts = pservice.GetProducts();
      products = newProducts.ToList();
      ProdDataGrid.ItemsSource = products.Where(FilterProdByQuery);

      ProdStat = $"Current product count: {products.Count}";
    }

    public MainApplication()
    {
      InitializeComponent();
      DataContext = this;

      pservice = new ProductService();
      cservice = new CategoryService();

      categories = cservice.GetCategories();
      PopulateComboBox();

      SearchQuery = "";
      //RefreshDataList();

      ToggleSidebar(false);
    }

    private void ToggleSidebar(bool p)
    {
      Sidebar.Width = p ? 410 : 0;
    }

    private void OnPropertyChanged(string name)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private void CloseSide_Click(object sender, RoutedEventArgs e)
    {
      ToggleSidebar(false);
    }

    private void ToggleProductDetail(Product prod)
    {
      CurProduct = prod;

      firstView = true;
      ProductDetails.DataContext = CurProduct;
      ToggleSidebar(true);

      if (null == CurProduct)
      {
        CategoryComboList.SelectedIndex = -1;
        return;
      }

      int curProId = CurProduct.CategoryId ?? -1;


      CategoryComboList.SelectedIndex = Array.IndexOf(
        categories.Select(c => c.CategoryId).ToArray(),
        curProId
      );
    }

    private void ProdDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (sender is not DataGrid)
      {
        return;
      }

      var prod = (Product)((DataGrid)sender).SelectedItem;
      ToggleProductDetail(prod);
    }

    private void Update_Click(object sender, RoutedEventArgs e)
    {
      if (CurProduct == null) return;
      pservice.UpdateProduct(CurProduct);
      RefreshDataList();
    }

    private void CategoryComboList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (CurProduct == null) return;

      if (firstView)
      {
        firstView = false;
        return;
      }

      CurProduct.CategoryId = categories[CategoryComboList.SelectedIndex].CategoryId;
      // Not using binding, the only way to fix
      pservice.UpdateProduct(CurProduct);
      RefreshDataList();
    }

    private void Remove_Click(object sender, RoutedEventArgs e)
    {
      if (CurProduct == null) return;

      string d = $"[ID: {CurProduct.ProductId}] {CurProduct.ProductName}";

      var ans = MessageWindow.Show(new MessageOptions
      {
        Title = "Confirmation",
        Message = "Are you sure you want to remove the following product?",
        Description = $"Removing: {d}"
         + "\n\nWarning: This action is not reversable!",
        Image = MessageImage.Question,
        Actions = MessageActions.YesNo,
        Owner = GetWindow(this)
      });

      if (ans != MessageResults.Yes)
      {
        return;
      }

      try
      {
        pservice.DeleteProduct(CurProduct);
        RefreshDataList();
        CurProduct = null;

        MessageWindow.Show(new MessageOptions
        {
          Title = "Success",
          Message = "Product removed from the database!",
          Description = $"Removed: {d}",
          Image = MessageImage.Success,
          Owner = GetWindow(this)
        });

      }
      catch (Exception ex)
      {
        MessageWindow.Show(new MessageOptions
        {
          Title = "Error",
          Message = "An error occured while removing product!",
          Description = $"Error message: {ex.Message}",
          Image = MessageImage.Error,
          Owner = GetWindow(this)
        });
      }
    }

    private void LaunchMainWindow()
    {
      var initial = Application.Current.MainWindow;
      Application.Current.MainWindow = new MainWindow(true);
      Application.Current.MainWindow.Show();

      initial.Close();
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
      var ans = MessageWindow.Show(new MessageOptions
      {
        Title = "Exit confirmation",
        Message = "Are you sure you want to exit?",
        Description = "This action will also log you out!",
        Image = MessageImage.Question,
        Actions = MessageActions.YesNo,
        Owner = GetWindow(this)
      });

      if (ans != MessageResults.Yes)
      {
        return;
      }

      LaunchMainWindow();
    }

    private void NewProduct_Click(object sender, RoutedEventArgs e)
    {
      var wind = new CreateProductWindow()
      {
        Owner = Window.GetWindow(this)
      };

      if (wind.ShowDialog() != true) return;
      RefreshDataList();
    }

    private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
    {
      if (e.PropertyName == "Category")
      {
        e.Column = null;
      }
    }

    private string _query = "";
    string SearchQuery
    {
      get => _query;
      set
      {
        _query = value;
        RefreshDataList();
      }
    }

    private bool FilterProdByQuery(Product prod)
    {
      var s1 = prod.ProductName.ToLower();
      var s2 = $"id:{prod.ProductId}".ToLower();
      var s3 = $"cat:{prod.CategoryId}".ToLower();
      var q = _query.ToLower();

      if (s1.Contains(q)) return true;
      if (q == s2 || q == s3) return true;

      return false;
    }

    private void SQuery_TextChanged(object sender, object _)
    {
      if (SQueryBox.IsFocused)
      {
        SearchQuery = SQueryBox.Text;
      }
    }

    private void SetBoxText(bool focus)
    {
      var showPlaceholder = !focus && _query.Length == 0;

      if (showPlaceholder)
      {
        SQueryBox.Text = "Search for products (query | id:<id> | cat:<category_id>) . . .";
        SQueryBox.Foreground = Brushes.Gray;

        return;
      }

      SQueryBox.Text = _query;
      SQueryBox.Foreground = Brushes.Black;
    }

    private void SQueryBox_GotFocus(object sender, object _)
    {
      SetBoxText(true);
    }

    private void SQueryBox_LostFocus(object sender, object _)
    {
      SetBoxText(false);
    }

    private void ClearBtt_Click(object sender, RoutedEventArgs e)
    {
      SearchQuery = "";
      SQueryBox.Text = "";
    }

    private void RndBtt_Click(object sender, RoutedEventArgs e)
    {
      if (products.IsNullOrEmpty()) return;

      var r = products[new Random().Next(products.Count)];
      var s = $"id:{r.ProductId}";

      SQueryBox.Text = s;
      SearchQuery = s;

      ToggleProductDetail(products[products.IndexOf(r)]);
    }
  }
}
