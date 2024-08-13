using _0_framework.Application;
using _0_framework.Infrastructure;
using ShopManagement.Application.Contracts.Slide;
using ShopManagement.Domain.SlideAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository;

public class SlideRepository : RepositoryBase<long, Slide>, ISlideRepository
{
    private readonly ShopContext _context;

    public SlideRepository(ShopContext context) : base(context)
    {
        _context = context;
    }

    public EditSlide GetDetails(long id)
    {
        return _context.Slides.Select(x => new EditSlide
        {
            Id = id,
            BtnText = x.BtnText,
            Heading = x.Heading,
            Picture = x.Picture,
            PictureTitle = x.PictureTitle,
            PictureAlt = x.PictureAlt,
            Title = x.Title,
            Text = x.Text,
        }).FirstOrDefault(x => x.Id == id);
    }

    public List<SlideViewModel> GetList()
    {
        return _context.Slides.Select(x => new SlideViewModel
        {
            Id = x.Id,
            Heading = x.Heading,
            Picture = x.Picture,
            Title = x.Title,
            CreationDate = x.CreationDate.ToFarsi()
        }).OrderByDescending(x => x.Id).ToList();
    }
}