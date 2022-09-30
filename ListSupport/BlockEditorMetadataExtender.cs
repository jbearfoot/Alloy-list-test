using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;

namespace listtest.ListSupport
{
    internal class BlockEditorMetadataExtender : IMetadataExtender
    {
        public  void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            //ExtendedMetadata caches properties in a backing variable _properties, the problem is that the backing variable is an enumerable from a linq-select (without ToArray) so it will execute the select each time
            var fieldInfo = typeof(ExtendedMetadata).GetField("_properties", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            fieldInfo.SetValue(metadata, metadata.Properties.ToArray());

            var property = metadata.Properties.First(p => p.PropertyName == "Property");
            property.ShowForEdit = false;
            property.ShowForDisplay = false;
            property = metadata.Properties.First(p => p.PropertyName == "IsReadOnly");
            property.ShowForEdit = false;
            property.ShowForDisplay = false;
        }
    }

}
