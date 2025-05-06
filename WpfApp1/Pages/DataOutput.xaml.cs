
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
            ComboFilter.SelectedIndex = 0;
            ComboSort.SelectedIndex = 0;

            allRecipes = AppConnect.model01.Recipes.ToList();
            listProducts.ItemsSource = allRecipes;

            var categories = AppConnect.model01.Categories.ToList();
            foreach (var category in categories)
            {
                ComboFilter.Items.Add(new ComboBoxItem { Content = category.CategoryName });
            }
            UpdateFoundCount(allRecipes.Count);
        }
        private void listProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedRecipe = listProducts.SelectedItem as Recipes;
            if (selectedRecipe != null)
            {
                Debug.WriteLine($"Выбран рецепт: {selectedRecipe.RecipeName}");
            }
        }
        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateRecipeList();
        }
        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateRecipeList();
        }
        private void ApplySearch_Click(object sender, RoutedEventArgs e)
        {
            UpdateRecipeList();
        }
        private void TextSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateRecipeList();
        }
        private void ResetSearch_Click(object sender, RoutedEventArgs e)
        {
            TextSearch.Text = string.Empty;
            ComboFilter.SelectedIndex = 0;
            UpdateRecipeList();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (listProducts.SelectedItem is Recipes selectedRecipe)
            {
                EditRecipe editPage = new EditRecipe(selectedRecipe);
                editPage.RecipeUpdated += UpdateRecipeList;
                NavigationService.Navigate(editPage);
            }
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Recipes newRecipe = new Recipes();
            EditRecipe editPage = new EditRecipe(newRecipe);
            editPage.RecipeUpdated += UpdateRecipeList;
            NavigationService.Navigate(editPage);
        }
        private void UpdateRecipeList()
        {
            string searchText = TextSearch.Text.ToLower();
            string selectedCategory = (ComboFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (allRecipes == null)
            {
                UpdateFoundCount(0);
                return;
            }
            var filteredRecipes = allRecipes.Where(recipe =>
                recipe != null &&
                recipe.RecipeName != null &&
                recipe.RecipeName.ToLower().Contains(searchText) &&
                (selectedCategory == "Все категории" ||
                 (recipe.Categories != null && recipe.Categories.CategoryName == selectedCategory)))
                .ToList();

            List<Recipes> sortedRecipes;
            if (ComboSort.SelectedItem is ComboBoxItem selectedItem)
            {
                string sortBy = selectedItem.Content.ToString();
                switch (sortBy)
                {
                    case "Сортировать по имени":
                        sortedRecipes = filteredRecipes.OrderBy(recipe => recipe.RecipeName).ToList();
                        break;
                    case "Сортировать по времени приготовления":
                        sortedRecipes = filteredRecipes.OrderBy(recipe => recipe.CookingTime).ToList();
                        break;
                    case "Не сортировать":
                    default:
                        sortedRecipes = filteredRecipes;
                        break;
                }
            }
            else
            {
                sortedRecipes = filteredRecipes;
            }
            listProducts.ItemsSource = sortedRecipes;
            UpdateFoundCount(sortedRecipes.Count);
        }
        private void UpdateFoundCount(int count)
        {
            TextFoundCount.Text = $"Найдено: {count}";
        }
        private void AddToFavoritesButton_Click(object sender, RoutedEventArgs e)
        {
            selectedRecipe = listProducts.SelectedItem as Recipes;

            if (selectedRecipe == null)
            {
                MessageBox.Show("Выберите рецепт!");
                return;
            }

            try
            {
                if (!selectedRecipe.AuthorID.HasValue)
                {
                    MessageBox.Show("У рецепта не указан автор!");
                    return;
                }
                int recipeAuthorId = selectedRecipe.AuthorID.Value;

                var existingLike = AppConnect.model01.LikeRecipes
                    .FirstOrDefault(l => l.AuthorID == recipeAuthorId
                                       && l.RecipeID == selectedRecipe.RecipeID);
                if (existingLike != null)
                {
                    MessageBox.Show("Этот рецепт уже в избранном!");
                    return;
                }
                var newLike = new LikeRecipes
                {
                    AuthorID = recipeAuthorId,
                    RecipeID = selectedRecipe.RecipeID
                };
                AppConnect.model01.LikeRecipes.Add(newLike);
                AppConnect.model01.SaveChanges();
                MessageBox.Show("Рецепт добавлен в избранное!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR: {ex}\n{ex.StackTrace}");
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        private void ViewFavoritesButton_Click(object sender, RoutedEventArgs e)
        {
            var favoritesPage = new PageLike();
            NavigationService.Navigate(favoritesPage);
        }
    }
}
