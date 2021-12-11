using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LopushokApp.Model;

namespace LopushokApp.Views.Windows
{
    public partial class ProductsWindow : Window
    {
        public LopushokDatabaseModel Database { get; set; }

        public ProductsWindow()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            ProductsList.ItemsSource = GetNormalizedProductsList(Database);
        }

        private List<NormilizedProduct> GetNormalizedProductsList(LopushokDatabaseModel database)
        {
            List<Product> productsList = database.Products.ToList();

            List<NormilizedProduct> normilizedProducts = new List<NormilizedProduct>();

            foreach(Product product in productsList)
            {
                NormilizedProduct normProd = new NormilizedProduct(product, database);

                normilizedProducts.Add(normProd);
            }

            return normilizedProducts;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void DeleteSelectedItems(object sender, RoutedEventArgs e)
        {
            string prodList = "";

            foreach(NormilizedProduct normilizedProduct in ProductsList.SelectedItems)
            {
                prodList += $"\n{normilizedProduct.Title}";
            }

            if(MessageBox.Show($"Вы действительно хотите удалить:{prodList}?\nЭто действие невозможно отменить!",
                "Удаление элементов", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            foreach (NormilizedProduct product in ProductsList.SelectedItems)
            {
                Database.Products.Remove(Database.Products.Find(product.ID));
                try
                {
                    Database.SaveChanges();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }

            ProductsList.ItemsSource = GetNormalizedProductsList(Database);
        }

        private void OnProductsSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(ProductsList.SelectedItems.Count > 1)
            {
                ChangeButton.Visibility = Visibility.Hidden;
            }
            else
            {
                DeleteButton.Visibility = Visibility.Visible;
                ChangeButton.Visibility = Visibility.Visible;
            }
        }

        private void AddItem(object sender, RoutedEventArgs e)
        {
            AddChangeProductWindow window = new AddChangeProductWindow
            {
                Database = Database
            };

            if (window.ShowDialog() == true)
            {
                ProductsList.ItemsSource = GetNormalizedProductsList(Database);
            }
        }

        private void ChangeSelectedItem(object sender, RoutedEventArgs e)
        {
            NormilizedProduct selectedProduct = (NormilizedProduct)ProductsList.SelectedItem;

            if(selectedProduct == null) 
            { 
                return; 
            }

            AddChangeProductWindow window = new AddChangeProductWindow
            {
                Database = Database,
                Product = Database.Products.Find(selectedProduct.ID),
            };

            if (window.ShowDialog() == true)
            {
                ProductsList.ItemsSource = GetNormalizedProductsList(Database);
            }
        }

        private void FilterTextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
