using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.ServiceLocation;
using EPiServer.Shell;
using EPiServer.Shell.ObjectEditing;
using EPiServer.SpecializedProperties;
using System.Collections.Concurrent;

namespace listtest.ListSupport
{
    internal class UIDescriptorRegistryListWrapper : UIDescriptorRegistry
    {
        private readonly ServiceAccessor<MetadataHandlerRegistry> _metadataHandlerRegistry;
        private readonly ConcurrentDictionary<Type, object> _listDescriptors = new();

        public UIDescriptorRegistryListWrapper(IEnumerable<UIDescriptor> uiDescriptors, IEnumerable<IUIDescriptorInitializer> initializers, IEnumerable<UIDescriptorProvider> descriptorProviders, 
            ServiceAccessor<MetadataHandlerRegistry> metadataHandlerRegistry) 
            : base(uiDescriptors, initializers, descriptorProviders)
        {
            _metadataHandlerRegistry = metadataHandlerRegistry;
        }

        public override IEnumerable<UIDescriptor> GetDescriptorsForType(Type type)
        {
            if (typeof(IPropertyCollection).IsAssignableFrom(type))
            {
                var itemType = type.GetGenericArguments()[0];
                _listDescriptors.GetOrAdd(itemType, t =>
                {
                    if (t.GetConstructors().Any(c => c.GetParameters().Length == 0))
                    {
                        var isBlockType = typeof(BlockData).IsAssignableFrom(t);
                        if (isBlockType)
                        {
                            _metadataHandlerRegistry().RegisterMetadataHandler(t, new BlockEditorMetadataExtender());
                        }
                        if (t == typeof(LinkItem))
                        {
                            _metadataHandlerRegistry().RegisterMetadataHandler(t, new LinkItemMetadataExtender());
                        }

                        var listType = typeof(IList<>).MakeGenericType(t);
                        var existingHandlers = _metadataHandlerRegistry().GetMetadataHandlers(listType);
                        if (!existingHandlers.Any(c => c is not DefaultEditorDescriptor))
                        {
                            var editorDescriptorType = typeof(GenericListEditorDescriptor<>).MakeGenericType(t);
                            var arguments = isBlockType ? 
                            new[] { "Property", "IsReadOnly" } :
                                (typeof(LinkItem).IsAssignableFrom(t) ?
                                    new[] { "LinkResolver" } :
                                    new string[0]);

                            _metadataHandlerRegistry().RegisterMetadataHandler(listType, Activator.CreateInstance(editorDescriptorType, new object[] { arguments }) as IMetadataHandler);
                        }
                    }

                    return new object();
                });
            }
            return base.GetDescriptorsForType(type);
        }
    }

    internal static class TypeExtensions
    {
        public static bool IsGenericList(this Type type, out Type itemType)
        {
            itemType = null;
            if (type is null || !type.IsGenericType)
            {
                return false;
            }

            var genericTypeDefinition = type.GetGenericTypeDefinition();

            if (genericTypeDefinition != typeof(IEnumerable<>) && genericTypeDefinition != typeof(ICollection<>) && genericTypeDefinition != typeof(IList<>))
            {
                return false;
            }

            itemType = type.GenericTypeArguments[0];
            return true;
        }
    }

}
