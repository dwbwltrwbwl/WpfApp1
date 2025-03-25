using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.ApplicationData;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditRecipe.xaml
    /// </summary>
    public partial class EditRecipe : Page
    {
        private Recipes recipe;
        public EditRecipe(Recipes recipe)
        {
            InitializeComponent();
            this.recipe = recipe;

            EditRecipeName.Text = recipe.RecipeName;
            EditDescription.Text = recipe.DescriptionN;
            EditCookingTime.Text = recipe.CookingTime.ToString();

            LoadAuthors();
            LoadCategories();

            EditAuthor.SelectedItem = recipe.Authors;
            EditCategory.SelectedItem = recipe.Categories;
        }

        private void LoadAuthors()
        {
            var authors = AppConnect.model01.Authors.ToList();
            EditAuthor.ItemsSource = authors;
            EditAuthor.DisplayMemberPath = "AuthorName";
        }

        private void LoadCategories()
        {
            var categories = AppConnect.model01.Categories.ToList();
            EditCategory.ItemsSource = categories;
            EditCategory.DisplayMemberPath = "CategoryName";
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            recipe.RecipeName = EditRecipeName.Text;
            recipe.DescriptionN = EditDescription.Text;
            recipe.Authors = (Authors)EditAuthor.SelectedItem;
            recipe.Categories = (Categories)EditCategory.SelectedItem;

            recipe.CookingTime = EditCookingTime.Text;
            if (string.IsNullOrWhiteSpace(recipe.CookingTime))
            {
                MessageBox.Show("Пожалуйста, введите корректное время приготовления.");
                return;
            }

            AppConnect.model01.SaveChanges();
            NavigationService.GoBack();
        }
    }
}
