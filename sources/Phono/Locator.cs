using Depository.Abstraction.Interfaces;
using Depository.Core;
using Depository.Extensions;
using HyPlayer.PlayCore;
using HyPlayer.PlayCore.Abstraction;
using Phono.Contracts.Services.App;
using Phono.Extensions.DependencyInjectionExtensions;
using Phono.Services.App;
using Richasy.WinUIKernel.Share.Toolkits;
using System;

namespace Phono
{
    public class Locator
    {
        public static Locator Instance { get; } = new Locator();

        public IDepository? depository;

        public Locator()
        {
            depository = DepositoryFactory.CreateNew();

            depository?.AddSingleton<PlayCoreBase, Chopin>();
            depository?.AddSingleton<IActivationService, ActivationService>();
            depository?.AddSingleton<INavigationService, NavigationService>();
            depository?.AddSingleton<IPageService, PageService>();

            // Toolkits
            depository?.AddSingleton<IAppToolkit, SharedAppToolkit>();
            depository?.AddSingleton<IFileToolkit, SharedFileToolkit>();
            depository?.AddSingleton<ISettingsToolkit, SharedSettingsToolkit>();

            depository?.AddMvvm();
        }

        public T GetService<T>()
            where T : class
        {
            if (depository?.ResolveDependency(typeof(T)) is not T service)
            {
                throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within Locator.cs.");
            }

            return service;
        }
    }
}
