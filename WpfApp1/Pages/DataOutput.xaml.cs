using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для DataOutput.xaml
    /// </summary>
    public partial class DataOutput : Page
    {
        private List<Recipes> allRecipes;
        private Recipes selectedRecipe;
        public DataOutput()
        {
            InitializeComponent();

            allRecipes = AppConnect.model01.Recipes.ToList();
            listProducts.ItemsSource = allRecipes;
        }
        private void listProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listProducts.SelectedItem is Recipes recipe)
            {
                selectedRecipe = recipe;
            }
        }
        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboSort.SelectedItem is ComboBoxItem selectedItem)
            {
                string sortBy = selectedItem.Content.ToString();
                List<Recipes> sortedRecipes;
                switch (sortBy)
                {
                    case "Сортировать по имени":
                        sortedRecipes = allRecipes.OrderBy(recipe => recipe.RecipeName).ToList();
                        break;
                    case "Сортировать по времени приготовления":
                        sortedRecipes = allRecipes.OrderBy(recipe => recipe.CookingTime).ToList();
                        break;
                    default:
                        sortedRecipes = allRecipes;
                        break;
                }
                listProducts.ItemsSource = sortedRecipes;
            }
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {


        }

        private void ApplySearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = TextSearch.Text.ToLower();
            var filteredRecipes = allRecipes.Where(recipe => recipe.RecipeName.ToLower().Contains(searchText)).ToList();
            listProducts.ItemsSource = filteredRecipes;
        }
        private void TextSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = TextSearch.Text.ToLower();
            var filteredRecipes = allRecipes.Where(recipe => recipe.RecipeName.ToLower().Contains(searchText)).ToList();
            listProducts.ItemsSource = filteredRecipes;
        }
        private void ResetSearch_Click(object sender, RoutedEventArgs e)
        {
            TextSearch.Text = string.Empty;
            listProducts.ItemsSource = allRecipes;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (listProducts.SelectedItem is Recipes selectedRecipe)
            {
                EditRecipe editPage = new EditRecipe(selectedRecipe);
                NavigationService.Navigate(editPage);
            }
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
