using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Shell.ObjectEditing;

namespace listtest.ListSupport
{
    public class GenericListEditorDescriptor<TItemType> : CollectionEditorDescriptor<TItemType> where TItemType : new()
    {
        private readonly IEnumerable<string> _excludedColumns;

        public GenericListEditorDescriptor(string[] excludedColumns)
        {
            _excludedColumns = excludedColumns ?? Enumerable.Empty<string>();
        }

        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            foreach (var excludedColumn in _excludedColumns)
            {
                GridDefinition.ExcludedColumns.Add(excludedColumn);
            }
            base.ModifyMetadata(metadata, attributes);
        }
    }
}
