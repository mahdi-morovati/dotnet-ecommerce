using _01_LampshadeQuery.Contracts.Slide;
using ShopManagement.Infrastructure.EFCore;

namespace _01_LampshadeQuery.Query;

public class SlideQuery : ISlideQuery
{
    private readonly ShopContext _context;

    public SlideQuery(ShopContext context)
    {
        _context = context;
    }

    public List<SlideQueryModel> GetSlides()
    {
        return _context.Slides.Where(x => x.IsRemoved == false).Select(x => new SlideQueryModel
        {
            Picture = x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            BtnText = x.BtnText,
            Heading = x.Heading,
            Link = x.Link,
            Title = x.Title,
        }).ToList();
    }
}