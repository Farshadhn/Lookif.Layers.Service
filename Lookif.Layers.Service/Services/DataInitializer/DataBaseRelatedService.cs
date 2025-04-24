using Lookif.Layers.Core.Infrastructure.Base;
using Lookif.Layers.Core.Infrastructure.Base.DataInitializer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lookif.Layers.Service.Services.DataInitializer;

public class DataBaseRelatedService : IDataBaseRelatedService, ISingletonDependency
{

    public DataBaseRelatedService(IDataBaseService dataBaseService)
    {
        DataBaseService = dataBaseService;
    }

    public IDataBaseService DataBaseService { get; }
    public async Task RefreshDatabaseAsync(List<IDataInitializer> dataInitializers, bool useMigration = true)
    {
       await  DataBaseService.RefreshDatabaseAsync(dataInitializers, useMigration);
    }

   
}
