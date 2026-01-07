using Depository.Abstraction.Enums;
using Depository.Abstraction.Interfaces;
using Phono.Contracts.ViewModels;
using System.Reflection;

namespace Phono.Extensions.DependencyInjectionExtensions;

public static class MvvmExtensions
{
    public static void AddViewModels(this IDepository depository, Assembly? scanningAssembly = null)
    {
        // Register view models previously marked as scoped as transient to avoid
        // requiring an active scope when resolving from UI components.
        depository.AddAllImplementationsOf<IScopedViewModel>(DependencyLifetime.Transient, scanningAssembly);
        depository.AddAllImplementationsOf<ISingletonViewModel>(DependencyLifetime.Singleton, scanningAssembly);
        depository.AddAllImplementationsOf<ITransientViewModel>(DependencyLifetime.Transient, scanningAssembly);
    }

    public static void AddMvvm(this IDepository depository, Assembly? scanningAssembly = null)
    {
        depository.AddViewModels(scanningAssembly);
    }
}