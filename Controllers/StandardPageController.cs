using listtest.Models.Pages;
using listtest.Models.ViewModels;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using EPiServer.Framework.Hosting;
using EPiServer.Enterprise;
using JetBrains.Annotations;
using EPiServer.Web.Routing;

namespace listtest.Controllers;

public class StandardPageController : PageControllerBase<StandardPage>
{
    private readonly IContentRepository _contentRepository;
    private readonly IBlockPropertyFactory _blockPropertyFactory;
    private readonly UrlResolver _urlResolver;

    public StandardPageController(IContentRepository contentRepository, IBlockPropertyFactory blockPropertyFactory, UrlResolver urlResolver)
    {
        _contentRepository = contentRepository;
        _blockPropertyFactory = blockPropertyFactory;
        _urlResolver = urlResolver;
    }

    public ViewResult Index(StandardPage currentPage)
    {
        var model = CreateModel(currentPage);
        return View($"~/Views/{currentPage.GetOriginalType().Name}/Index.cshtml", model);
    }

    /// <summary>
    /// Creates a PageViewModel where the type parameter is the type of the page.
    /// </summary>
    /// <remarks>
    /// Used to create models of a specific type without the calling method having to know that type.
    /// </remarks>
    private static IPageViewModel<StandardPage> CreateModel(StandardPage page)
    {
        var type = typeof(PageViewModel<>).MakeGenericType(page.GetOriginalType());
        return Activator.CreateInstance(type, page) as IPageViewModel<StandardPage>;
    }

    [HttpPost]
    public IActionResult Comment(StandardPage currentPage, string user, string message)
    {
        var writable = currentPage.CreateWritableClone() as StandardPage;
        var comment = _blockPropertyFactory.Create<Comment>();
        comment.User = user;
        comment.Message = message;
        if (writable.Comments is null)
        {
            writable.Comments = new List<Comment>();
        }
        writable.Comments.Add(comment);
        _contentRepository.Save(writable, EPiServer.DataAccess.SaveAction.Patch, EPiServer.Security.AccessLevel.NoAccess);
        return Redirect(_urlResolver.GetUrl(currentPage.ContentLink));
    }
}
