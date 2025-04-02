//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp1.ApplicationData
{
    using System;
    using System.Collections.Generic;
    
    public partial class Recipes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Recipes()
        {
            this.CookingSteps = new HashSet<CookingSteps>();
            this.RecipeImages = new HashSet<RecipeImages>();
            this.RecipeIngredients = new HashSet<RecipeIngredients>();
            this.RecipeTags = new HashSet<RecipeTags>();
            this.Reviews = new HashSet<Reviews>();
        }
        public string CurrentPhoto
        {
            get
            {
                if (String.IsNullOrEmpty(ImageE) || String.IsNullOrWhiteSpace(ImageE))
                    return @"\Images\nofoto.jpg";
                else
                    return @"\Images\" + ImageE;
            }
        }
        public int RecipeID { get; set; }
        public string RecipeName { get; set; }
        public string DescriptionN { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> AuthorID { get; set; }
        public string CookingTime { get; set; }
        public string ImageE { get; set; }
    
        public virtual Authors Authors { get; set; }
        public virtual Categories Categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CookingSteps> CookingSteps { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecipeImages> RecipeImages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecipeIngredients> RecipeIngredients { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecipeTags> RecipeTags { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reviews> Reviews { get; set; }
    }
}
