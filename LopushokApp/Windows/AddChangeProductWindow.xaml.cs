using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using LopushokApp.Model;
using System;
using System.Linq;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace LopushokApp.Views.Windows
{
    public partial class AddChangeProductWindow : Window
    {
        public LopushokDatabaseModel Database { get; set; }

        public Product Product { get; set; } = null;

        private List<Material> _materials = new List<Material>();

        private string _imageSource = null;

        public AddChangeProductWindow()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            textBoxTitle.Name = "Название";
            textBoxArticle.Name = "Артикул";
            textBoxMinimumPrice.Name = "Цена";

            foreach(ProductType type in Database.ProductTypes)
            {
                comboBoxType.Items.Add(type.Title);
            }

            if(Product == null)
            {
                this.Title = "Добавление продукта";
            }
            else
            {
                this.Title = "Изменение продукта";

                textBoxTitle.Text = Product.Title;
                textBoxArticle.Text = Product.ArticleNumber;
                textBoxMinimumPrice.Text = Product.MinCostForAgent.ToString();
                textBoxDescription.Text = Product.Description;
                comboBoxType.SelectedIndex = (int)Product.ProductTypeID - 1;
                textBoxPersonCount.Text = Product.ProductionPersonCount.ToString();
                textBoxProductionNumber.Text = Product.ProductionWorkshopNumber.ToString();

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri($@"{Environment.CurrentDirectory}\Resources\{Product.Image}");
                image.EndInit();

                ProductPicture.Source = image;

                UpdateMaterialsList();
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (WindowValidated() == false) return;

            if (Product == null)
            {
                Product = CreateNewProduct();
                Database.Products.Add(Product);
            }
            else
            {
                Product.Title = textBoxTitle.Text;
                Product.ArticleNumber = textBoxArticle.Text;
                Product.MinCostForAgent = Convert.ToDecimal(textBoxMinimumPrice.Text.Substring(0, textBoxMinimumPrice.Text.Length - 3));
                Product.Description = textBoxDescription.Text;
                Product.ProductTypeID = comboBoxType.SelectedIndex + 1;
                Product.ProductionPersonCount = Convert.ToInt32(textBoxPersonCount.Text);
                Product.ProductionWorkshopNumber = Convert.ToInt32(textBoxProductionNumber.Text);

                if(_imageSource != null)
                {
                    Product.Image = _imageSource;
                }
            }

            try
            {
                Database.SaveChanges();
                DialogResult = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool WindowValidated()
        {
            List<TextBox> errors = new List<TextBox>();
            string errorsEnumeration = "";

            List<TextBox> textBoxes = new List<TextBox>
            {
                textBoxTitle,
                textBoxMinimumPrice,
                textBoxArticle
            };

            foreach (TextBox tb in textBoxes)
            {
                if (tb.Text == "")
                {
                    errors.Add(tb);
                    tb.BorderBrush = Brushes.Red;
                    errorsEnumeration += $"{tb.Name}\n";
                }
            }

            if (errors.Count > 0)
            {
                MessageBox.Show($"Поля не должны быть пустыми:\n\n{errorsEnumeration}");
                return false;
            }

            string txt = textBoxMinimumPrice.Text;

            bool Convertable = int.TryParse(txt.Substring(0, txt.Length - 3), out int i);

            if (!Convertable)
            {
                MessageBox.Show("Поле «Цена» должен содержать только числа!");
                return false;
            }

            if(comboBoxType.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите тип продукта!");
                return false;
            }

            bool convertablePersonsCount = int.TryParse(textBoxPersonCount.Text, out int j);

            if (!convertablePersonsCount)
            {
                MessageBox.Show("Поле «Кол-во человек для производства»\nдолжен содержать только числа!");
                return false;
            }

            bool convertableWorkshopNumber = int.TryParse(textBoxProductionNumber.Text, out int f);

            if (!convertableWorkshopNumber)
            {
                MessageBox.Show("Поле «Номер для производства»\nдолжен содержать только числа!");
                return false;
            }

            return true;
        }

        private Product CreateNewProduct()
        {
            Product product = new Product
            {
                Title = textBoxTitle.Text,
                ArticleNumber = textBoxArticle.Text,
                MinCostForAgent = Convert.ToDecimal(textBoxMinimumPrice.Text),
                Description = textBoxDescription.Text,
                ProductTypeID = comboBoxType.SelectedIndex + 1,
                ProductionPersonCount = Convert.ToInt32(textBoxPersonCount.Text),
                ProductionWorkshopNumber = Convert.ToInt32(textBoxProductionNumber.Text)
            };

            product.Image = _imageSource;

            return product;
        }

        private void ChangeProductImage(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Filter = "Image files | *png; *jpg; *bmp";

            if (fileDialog.ShowDialog() == true)
            {
                _imageSource = fileDialog.FileName;

                ProductPicture.Source = new BitmapImage(new Uri(_imageSource));
            }
        }

        private void UpdateMaterialsList()
        {
            foreach (ProductMaterial productMaterial in Database.ProductMaterials)
            {
                if (productMaterial.ProductID == Product.ID)
                {
                    Material tempMat = Database.Materials.Find(productMaterial.MaterialID);
                    _materials.Add(tempMat);
                }
            }

            MaterialsList.ItemsSource = _materials;
        }

        private void DeleteSelectedMaterial(object sender, RoutedEventArgs e)
        {
            Material material = (Material)MaterialsList.SelectedItem;

            if (material == null) return;

            foreach (ProductMaterial productMaterial in Database.ProductMaterials)
            {
                if ((productMaterial.ProductID == Product.ID) && (productMaterial.MaterialID == material.ID))
                {
                    MessageBox.Show($"Вы действительно хотите удалить материал\n{material.Title}\nдля продукта\n{Product.Title}?",
                        "Удаление материала", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    Database.ProductMaterials.Remove(productMaterial);
                }
            }

            try
            {
                Database.SaveChanges();
                UpdateMaterialsList();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
