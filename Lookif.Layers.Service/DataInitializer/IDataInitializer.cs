using Lookif.Library.Common;

namespace Lookif.Layers.Service.DataInitializer
{
    public interface IDataInitializer : IScopedDependency
    {
        void InitializeData();
    }
}
