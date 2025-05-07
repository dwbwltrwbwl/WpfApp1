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
using Word = Microsoft.Office.Interop.Word;
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

        private void btnWord_Click(object sender, RoutedEventArgs e)
        {
            string templatePath = @"C:\Users\10220448\Source\Repos\WpfApp1\WpfApp1\RecipeTemplate32.docx";
            string outputPath = @"C:\Users\10220448\Source\Repos\WpfApp1\WpfApp1\Recipe1.docx";
            Word.Application wordApp = new Word.Application();
            Word.Document wordDoc = wordApp.Documents.Open(templatePath);
            Word.Document newDocument = wordApp.Documents.Add();

            foreach (var rec in recipes)
            {
                Word.Range range = wordDoc.Content;
                range.Copy();
                Word.Range newRange = newDocument.Content;
                newRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
                newRange.Paste();
                Word.Find findObject = newDocument.Content.Find;
                findObject.ClearFormatting();
                findObject.Text = "{{NameRecipes}}";
                findObject.Replacement.ClearFormatting();
                findObject.Replacement.Text = rec.RecipeName;
                findObject.Execute(Replace: Word.WdReplace.wdReplaceAll);

                Word.Range bookmarkRange = newDocument.Bookmarks["ResepisImages"].Range;
                bookmarkRange.Delete();

                var photoPath = @"C:\Users\10220448\Source\Repos\WpfApp1\WpfApp1\" + rec.CurrentPhoto.Substring(1);
                Word.InlineShape newInlineShape = bookmarkRange.InlineShapes.AddPicture(photoPath, false, true);
                //newInlineShape.Width = 210;
                //newInlineShape.Height = 210;

                findObject.Text = "{AuthorRecipes}";
                findObject.Replacement.ClearFormatting();
                findObject.Replacement.Text = rec.Authors.AuthorName;
                findObject.Execute(Replace: Word.WdReplace.wdReplaceAll);

                findObject.Text = "{{Category}}";
                findObject.Replacement.ClearFormatting();
                findObject.Replacement.Text = rec.Categories.CategoryName;
                findObject.Execute(Replace: Word.WdReplace.wdReplaceAll);

                findObject.Text = "{{Description}}";
                findObject.Replacement.ClearFormatting();
                findObject.Replacement.Text = rec.DescriptionN;
                findObject.Execute(Replace: Word.WdReplace.wdReplaceAll);

                newRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
                newRange.InsertBreak(Word.WdBreakType.wdPageBreak);
            }
            newDocument.SaveAs2(outputPath);
            newDocument.Close();
            wordDoc.Close();
            wordApp.Quit();

            MessageBox.Show("Документ успешно создан!");
            // Переход на страницу с QR-кодом
            NavigationService.Navigate(new PageQRCode());
        }
    }
}
