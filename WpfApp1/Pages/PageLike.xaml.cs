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
    /// Логика взаимодействия для PageLike.xaml
    /// </summary>
    public partial class PageLike : Page
    {
        private List<Recipes> recipes;
        public PageLike()
        {
            InitializeComponent();
            UpdateLikeRecipes();
        }
        private void UpdateLikeRecipes()
        {
            var likeRecipes = AppConnect.model01.LikeRecipes.Where(x => x.AuthorID == AppConnect.AuthorID).Select(x => x.RecipeID).ToList();
            recipes = AppConnect.model01.Recipes.Where(x => likeRecipes.Contains(x.RecipeID)).ToList();
            listProducts.ItemsSource = recipes;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DataOutput());
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите удалить рецепт из избранного?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var button = sender as Button;
                var recipe = (Recipes)button.DataContext;
                var itemToRemove = AppConnect.model01.LikeRecipes.FirstOrDefault(r => r.RecipeID == recipe.RecipeID && AppConnect.AuthorID == r.AuthorID);
                AppConnect.model01.LikeRecipes.Remove(itemToRemove);
                AppConnect.model01.SaveChanges();
                UpdateLikeRecipes();
                MessageBox.Show("Рецепт удален из избранного!");
            }
        }
    }
}
