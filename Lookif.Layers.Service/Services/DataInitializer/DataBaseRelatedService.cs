using Lookif.Layers.Core.Infrastructure.Base;
using Lookif.Layers.Core.Infrastructure.Base.DataInitializer;
using Lookif.Library.Common;
using System.Collections.Generic;

namespace Lookif.Layers.Service.Services.DataInitializer
{
    public class DataBaseRelatedService : IDataBaseRelatedService , ISingletonDependency
    {

        public DataBaseRelatedService(IDataBaseService dataBaseService)
        {
            DataBaseService = dataBaseService;
        }

        public IDataBaseService DataBaseService { get; }
        public void RefreshDatabase(List<IDataInitializer> dataInitializers, bool Do_not_use_Migrations = false)
        {
            DataBaseService.RefreshDatabase(dataInitializers, Do_not_use_Migrations);
        }
    }
}
