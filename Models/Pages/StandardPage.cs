using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace listtest.Models.Pages;


[ContentType (AvailableInEditMode = false)]
public class Comment : BlockData
{
    public virtual string User { get; set; }
    public virtual string Message { get; set; }
}

[ContentType (AvailableInEditMode = false)]
public class ImageListItem : BlockData
{
    public virtual string Title { get; set; }
    [UIHint(UIHint.Image)]
    public virtual ContentReference Image { get; set; }
}

/// <summary>
/// Used for the pages mainly consisting of manually created content such as text, images, and blocks
/// </summary>
[SiteContentType(GUID = "9CCC8A41-5C8C-4BE0-8E73-520FF3DE8267")]
[SiteImageUrl(Globals.StaticGraphicsFolderPath + "page-type-thumbnail-standard.png")]
public class StandardPage : SitePageData
{
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 310)]
    [CultureSpecific]
    public virtual XhtmlString MainBody { get; set; }

    [Display(
        GroupName = SystemTabNames.Content,
        Order = 320)]
    public virtual ContentArea MainContentArea { get; set; }

    public virtual IList<ImageListItem> ImageLists { get; set; }
    public virtual IList<Comment> Comments { get; set; }

}
