using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Shell.ObjectEditing;
using EPiServer.SpecializedProperties;

namespace listtest.Models.Pages
{
    [ContentType(AvailableInEditMode = false)]
    public class InlineBlock : BlockData
    { 
        public virtual string BlockHeading { get; set; }
        public virtual XhtmlString BlockBody { get; set; }
    }

    [ContentType]
    public class ListPage : SitePageData
    {
        public virtual IList<InlineBlock> BlockList { get; set;}
        public virtual IList<Url> UrlList { get; set; }
        public virtual IList<XhtmlString> XhtmlList { get; set; }
        public virtual IList<string> JsonStringList { get; set; }
        [BackingType(typeof(PropertyCollection))]
        public virtual IList<string> StringList { get; set; }
        public virtual IList<ContentReference> JsonContentLinkList { get; set; }
        [BackingType(typeof(PropertyCollection))]
        public virtual IList<ContentReference> ContentLinkList { get; set; }
        public virtual IList<LinkItem> LinkItemList { get; set; }
    }
}
