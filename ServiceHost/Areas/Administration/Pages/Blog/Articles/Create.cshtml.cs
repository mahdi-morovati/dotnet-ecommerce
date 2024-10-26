using BlogManagement.Application.Contracts.Article;
using BlogManagement.Application.Contracts.ArticleCategory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Administration.Pages.Blog.Articles
{
    public class CreateModel : PageModel
    {
        public CreateArticle Command;
        public SelectList ArticleCategories;

        private readonly IArticleApplication _articleApplication;
        private readonly IArticleCategoryApplication _articleCategoryApplication;

        public CreateModel(IArticleApplication articleApplication, IArticleCategoryApplication articleCategoryApplication)
        {
            _articleApplication = articleApplication;
            _articleCategoryApplication = articleCategoryApplication;
        }

        public void OnGet()
        {
            ArticleCategories = new SelectList(_articleCategoryApplication.GetArticleCategories(), "Id", "Name");
        }

        public IActionResult OnPost(CreateArticle command)
        {
            if (!ModelState.IsValid)
            {
                // اگر اعتبارسنجی ناموفق بود، دسته‌ها را دوباره بارگذاری کنید
                ArticleCategories = new SelectList(_articleCategoryApplication.GetArticleCategories(), "Id", "Name");
                return Page(); // دوباره صفحه را بارگذاری کنید تا خطاها نمایش داده شوند
            }
            
            var result = _articleApplication.Create(command);
            
            
            if (!result.IsSuccedded)
            {
                // اگر ایجاد مقاله ناموفق بود، خطا را به مدل اضافه کنید
                ModelState.AddModelError("", result.Message);
        
                // دوباره دسته‌ها را بارگذاری کنید
                ArticleCategories = new SelectList(_articleCategoryApplication.GetArticleCategories(), "Id", "Name");
                return Page();
            }

            
            return RedirectToPage("./Index");
        }
    }
}
