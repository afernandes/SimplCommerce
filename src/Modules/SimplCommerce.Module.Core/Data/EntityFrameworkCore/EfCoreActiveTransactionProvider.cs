using System;
using System.Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace SimplCommerce.Module.Core.Data.EntityFrameworkCore
{
    public class EfCoreActiveTransactionProvider : IActiveTransactionProvider //, ITransientDependency
    {
        /*private readonly IIocResolver _iocResolver;

        public EfCoreActiveTransactionProvider(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }
        */
        private readonly IServiceProvider _serviceProvider;

        public EfCoreActiveTransactionProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IDbTransaction GetActiveTransaction(ActiveTransactionProviderArgs args)
        {
            return GetDbContext(args).Database.CurrentTransaction?.GetDbTransaction();
        }

        public IDbConnection GetActiveConnection(ActiveTransactionProviderArgs args)
        {
            return GetDbContext(args).Database.GetDbConnection();
        }

        private DbContext GetDbContext(ActiveTransactionProviderArgs args)
        {
            Type dbContextProviderType = typeof(IDbContextProvider<>).MakeGenericType((Type)args["ContextType"]);

            //using (IDisposableDependencyObjectWrapper dbContextProviderWrapper = _iocResolver.ResolveAsDisposable(dbContextProviderType))
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContextProvider = scope.ServiceProvider.GetRequiredService(dbContextProviderType);

                MethodInfo method = dbContextProvider.GetType()
                                     .GetMethod(
                                        nameof(IDbContextProvider<SimplDbContext>.GetDbContext)
                                     //,new[] { typeof(MultiTenancySides) }
                                     );

                /*MethodInfo method = dbContextProviderWrapper.Object.GetType()
                                                            .GetMethod(
                                                                nameof(IDbContextProvider<SimplDbContext>.GetDbContext)
                                                                //,new[] { typeof(MultiTenancySides) }
                                                            );*/

                return (DbContext)method.Invoke(
                    dbContextProvider,
                    //new object[] { (MultiTenancySides?)args["MultiTenancySide"] }
                    null
                );
            }
        }
    }
}
