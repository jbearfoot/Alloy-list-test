using EPiServer.Shell.ObjectEditing;

namespace listtest.ListSupport
{

    internal class LinkItemMetadataExtender : IMetadataExtender
    {
        public void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            //ExtendedMetadata caches properties in a backing variable _properties, the problem is that the backing variable is an enumerable from a linq-select (without ToArray) so it will execute the select each time
            var fieldInfo = typeof(ExtendedMetadata).GetField("_properties", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var properties = metadata.Properties.ToArray();
            var linkResolver = properties.First(p => p.PropertyName == "LinkResolver");
            properties = properties.Except(new[] { linkResolver }).ToArray();
            fieldInfo.SetValue(metadata, properties);
        }
    }
}
