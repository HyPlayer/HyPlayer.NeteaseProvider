using Depository;
using Depository.Abstraction.Interfaces;
using Depository.Core;
using Depository.Extensions;
using HyPlayer.PlayCore;
using HyPlayer.PlayCore.Abstraction;
using HyPlayer.PlayCore.Implementation.AudioGraphService;
using System;
using System.Collections.Generic;
using System.Text;

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
            depository?.AddSingleton<AudioGraphService>();
        }

        public T GetService<T>()
            where T : class
        {
            if (Locator.Instance.depository?.ResolveDependency(typeof(T)) is not T service)
            {
                throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within Locator.cs.");
            }

            return service;
        }
    }
}
