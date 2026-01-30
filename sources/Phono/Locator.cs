using Depository.Abstraction.Interfaces;
using Depository.Core;
using Depository.Extensions;
using HyPlayer.PlayCore;
using HyPlayer.PlayCore.Abstraction;
using Phono.Contracts.Services;
using Phono.Contracts.Services.App;
using Phono.Extensions.DependencyInjectionExtensions;
using Phono.Services.App;
using Richasy.WinUIKernel.Share.Toolkits;
using System;

namespace Phono
{
    public class Locator : IDisposable
    {
        // Make the Locator instance lazy to avoid early initialization during XAML/component construction
        private static readonly Lazy<Locator> _instance = new(() => new Locator());
        public static Locator Instance => _instance.Value;

        public IDepository? depository;

        // Cache for resolved singleton instances to avoid repeated container lookups
        private readonly System.Collections.Concurrent.ConcurrentDictionary<Type, object> singletonCache = new();
        private readonly System.Collections.Generic.HashSet<Type> knownSingletons = new();

        // Cache for delegate factories to avoid repeated ResolveDependency(Type) calls and reduce reflection overhead
        private readonly System.Collections.Concurrent.ConcurrentDictionary<Type, Func<object?>> resolveFactoryCache = new();

        public Locator()
        {
            depository = DepositoryFactory.CreateNew();
            RegisterSingletonAndRecord<PlayCoreBase, Chopin>();
            RegisterSingletonAndRecord<IActivationService, ActivationService>();
            RegisterSingletonAndRecord<INavigationService, NavigationService>();
            RegisterSingletonAndRecord<INavigationViewService, NavigationViewService>();
            RegisterSingletonAndRecord<IPageService, PageService>();

            // Toolkits
            RegisterSingletonAndRecord<IAppToolkit, SharedAppToolkit>();
            RegisterSingletonAndRecord<IFileToolkit, SharedFileToolkit>();
            RegisterSingletonAndRecord<ISettingsToolkit, SharedSettingsToolkit>();

            depository?.AddMvvm();
        }

        private void RegisterSingletonAndRecord<TService, TImpl>()
            where TService : class
            where TImpl : class, TService
        {
            depository?.AddSingleton<TService, TImpl>();
            knownSingletons.Add(typeof(TService));
        }

        public T GetService<T>()
            where T : class
        {
            try
            {
                var t = typeof(T);

                // Fast-path: if this is a known singleton registration or a singleton viewmodel, try cache first
                var isSingletonViewModel = typeof(Phono.Contracts.ViewModels.ISingletonViewModel).IsAssignableFrom(t);
                if (knownSingletons.Contains(t) || isSingletonViewModel)
                {
                    if (singletonCache.TryGetValue(t, out var cached) && cached is T cachedT)
                    {
                        return cachedT;
                    }
                }

                var factory = GetResolveFactory(t);
                object? resolved = null;
                try
                {
                    resolved = factory?.Invoke();
                }
                catch
                {
                    // Fall back to direct ResolveDependency in case cached factory fails for any reason
                    resolved = depository?.ResolveDependency(t);
                }

                if (resolved is not T service)
                {
                    throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within Locator.cs.");
                }

                // Cache singletons and singleton viewmodels to avoid repeated resolves
                if (knownSingletons.Contains(t) || isSingletonViewModel)
                {
                    singletonCache.TryAdd(t, service!);
                }

                return service;
            }
            catch (Exception ex)
            {
                // If the underlying DI throws a scope-related exception, rethrow with actionable guidance.
                if (ex.GetType().Name.Contains("ScopeNotSet") || ex.Message.Contains("scope", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException($"Failed to resolve {typeof(T)}: there is no active dependency scope. " +
                        "Create or activate a scope before resolving scoped services, or register the type as transient/singleton.", ex);
                }

                throw;
            }
        }

        // Get or create a fast factory delegate for resolving the given type
        private Func<object?> GetResolveFactory(Type t)
        {
            return resolveFactoryCache.GetOrAdd(t, type =>
            {
                return new Func<object?>(() => depository?.ResolveDependency(type));
            });
        }

        public void Dispose()
        {
            // Dispose cached singletons that require disposal
            foreach (var kv in singletonCache)
            {
                if (kv.Value is IDisposable d)
                {
                    try
                    {
                        d.Dispose();
                    }
                    catch
                    {
                        // Swallow during shutdown
                    }
                }
            }

            singletonCache.Clear();
            knownSingletons.Clear();
            resolveFactoryCache.Clear();
        }
    }
}
