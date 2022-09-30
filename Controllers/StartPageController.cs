using listtest.Models.Pages;
using listtest.Models.ViewModels;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using EPiServer.Framework.Hosting;
using EPiServer.Enterprise;

namespace listtest.Controllers;

public class StartPageController : PageControllerBase<StartPage>
{
    public IActionResult Index(StartPage currentPage)
    {
        var model = PageViewModel.Create(currentPage);

        // Check if it is the StartPage or just a page of the StartPage type.
        if (SiteDefinition.Current.StartPage.CompareToIgnoreWorkID(currentPage.ContentLink))
        {
            // Connect the view models logotype property to the start page's to make it editable
            var editHints = ViewData.GetEditHints<PageViewModel<StartPage>, StartPage>();
            editHints.AddConnection(m => m.Layout.Logotype, p => p.SiteLogotype);
            editHints.AddConnection(m => m.Layout.ProductPages, p => p.ProductPageLinks);
            editHints.AddConnection(m => m.Layout.CompanyInformationPages, p => p.CompanyInformationPageLinks);
            editHints.AddConnection(m => m.Layout.NewsPages, p => p.NewsPageLinks);
            editHints.AddConnection(m => m.Layout.CustomerZonePages, p => p.CustomerZonePageLinks);
        }

        //if (Request.Query["import"].Any())
        //{
        //    var fileProvider = HttpContext.RequestServices.GetRequiredService<ICompositeFileProvider>();
        //    var importer = HttpContext.RequestServices.GetRequiredService<IDataImporter>();
        //    using (var importStream = fileProvider.GetFileInfo("/App_Data/DefaultSiteContent.episerverdata").CreateReadStream())
        //    {
        //        var log = importer.Import(importStream, ContentReference.RootPage, new ImportOptions
        //        {
        //            KeepIdentity = false
        //        });
        //    }
        //}

        //var contentRepository = HttpContext.RequestServices.GetRequiredService<IContentRepository>();
        //var blockPropertyFactory = HttpContext.RequestServices.GetRequiredService<IBlockPropertyFactory>();
        //var firstBlock = blockPropertyFactory.Create<InlineBlock>();
        //firstBlock.BlockHeading = "heading 1";
        //firstBlock.BlockBody = new XhtmlString("<p>body 1</p>");
        //var secondBlock = blockPropertyFactory.Create<InlineBlock>();
        //secondBlock.BlockHeading = "heading 2";
        //secondBlock.BlockBody = new XhtmlString("<p>body 2</p>");
        //var page = contentRepository.Get<ListPage>(new ContentReference(105)).CreateWritableClone() as ListPage;
        //page.BlockList = new[] { firstBlock, secondBlock };
        //page.XhtmlList = new[] { new XhtmlString("<p>first item</p>"), new XhtmlString("<p>second item</p>") };
        //contentRepository.Save(page, EPiServer.DataAccess.SaveAction.Patch, EPiServer.Security.AccessLevel.NoAccess);
        

        return View(model);
    }
}
