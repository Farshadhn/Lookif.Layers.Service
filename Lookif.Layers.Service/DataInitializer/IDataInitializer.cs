using Lookif.Library.Common;

namespace ESS.Service.DataInitializer
{
    public interface IDataInitializer : IScopedDependency
    {
        void InitializeData();
    }
}
