using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors.PropertyValueList;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.UI.Rest;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace listtest.ListSupport
{
    [EditorDescriptorRegistration(TargetType = typeof(IList<Url>))]
    internal class UrlListEditorDescriptor : PropertyValueListEditorDescriptor<Url>
    {
        public UrlListEditorDescriptor(LocalizationService localizationService,
            IMetadataStoreModelCreator metadataStoreModelCreator,
            IHttpContextAccessor httpContextServiceAccessor,
            ServiceAccessor<MetadataHandlerRegistry> metadataHandlerRegistryAccessor,
            IValidationAttributeAdapterProvider validationAttributeAdapterProvider)
            : base(localizationService, metadataStoreModelCreator, httpContextServiceAccessor, metadataHandlerRegistryAccessor, validationAttributeAdapterProvider)
        {
        }
    }
}
