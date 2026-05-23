using AwesomeAssertions;
using HyPlayer.NeteaseProvider.Models;
using HyPlayer.PlayCore.Abstraction.Interfaces.Provider;
using HyPlayer.PlayCore.Abstraction.Models;
using HyPlayer.PlayCore.Abstraction.Models.Containers;
using HyPlayer.PlayCore.Abstraction.Models.SingleItems;

namespace HyPlayer.NeteaseProvider.Tests;

[Retry(3)]
public class NeteaseProviderContractTests
{
    private readonly NeteaseProvider _provider = new();

    [Before(Test)]
    public Task InitializeAsync()
    {
        _provider.Handler.Option.AdditionalParameters = Secrets.AdditionalParameters;
        return Task.CompletedTask;
    }

    [Test]
    public async Task PriorityContracts_Should_BeImplemented()
    {
        _provider.Should().BeAssignableTo<IAuthenticationProvidable>();
        _provider.Should().BeAssignableTo<IContainerManagementProvidable>();
        _provider.Should().BeAssignableTo<ISearchSuggestionProvidable>();
        _provider.Should().BeAssignableTo<IContainerPageProvidable>();
        _provider.Should().BeAssignableTo<IPersonalFmProvidable>();
        await Task.CompletedTask;
    }

    [Test]
    public async Task AuthenticationSessionRoundTrip_Should_StayProviderNeutral()
    {
        IAuthenticationProvidable provider = _provider;
        var sessionValues = new Dictionary<string, string>
        {
            ["MUSIC_U"] = "test-cookie",
            ["__csrf"] = "test-csrf"
        };

        await provider.ImportSessionAsync(sessionValues);
        var exported = await provider.ExportSessionAsync();
        await provider.LogoutAsync();

        exported.Should().BeEquivalentTo(sessionValues);
        var cleared = await provider.ExportSessionAsync();
        cleared.Should().BeEmpty();
    }

    [Test]
    [Arguments("海阔天空")]
    public async Task GetSearchSuggestionsAsync_Should_ReturnPlayCoreContainer(string keyword)
    {
        ISearchSuggestionProvidable provider = _provider;

        var container = await provider.GetSearchSuggestionsAsync(keyword);
        var items = await ((LinerContainerBase)container).GetAllItemsAsync();

        container.Should().BeOfType<NeteaseActionGettableContainer>();
        container.ProviderId.Should().Be(_provider.Id);
        container.ActualId.Should().Be(keyword);
        items.Should().AllBeAssignableTo<ProvidableItemBase>();
        items.Any(item => (item.GetType().Namespace ?? string.Empty).Contains("NeteaseApi")).Should().BeFalse();
    }

    [Test]
    [Arguments("2778408564", 0, 5)]
    public async Task GetContainerItemsPageAsync_Should_ReturnPlayCoreItems(string playlistId, int offset, int count)
    {
        IContainerPageProvidable provider = _provider;

        var page = await provider.GetContainerItemsPageAsync(playlistId, offset, count);

        page.Items.Should().NotBeEmpty();
        page.Items.Should().AllBeAssignableTo<ProvidableItemBase>();
        page.Items.Should().OnlyContain(item => item.ProviderId == _provider.Id);
        page.NextOffset.Should().Be(page.HasMore ? offset + count : null);
    }

    [Test]
    public async Task ContainerManagementMethods_Should_ExposePlayCoreContractTypes()
    {
        var createMethod = typeof(IContainerManagementProvidable).GetMethod(nameof(IContainerManagementProvidable.CreateContainerAsync));
        var deleteMethod = typeof(IContainerManagementProvidable).GetMethod(nameof(IContainerManagementProvidable.DeleteContainerAsync));
        var privacyMethod = typeof(IContainerManagementProvidable).GetMethod(nameof(IContainerManagementProvidable.SetContainerPrivacyAsync));

        createMethod.Should().NotBeNull();
        createMethod!.ReturnType.Should().Be(typeof(Task<ContainerBase>));
        deleteMethod.Should().NotBeNull();
        deleteMethod!.ReturnType.Should().Be(typeof(Task));
        privacyMethod.Should().NotBeNull();
        privacyMethod!.ReturnType.Should().Be(typeof(Task));
        await Task.CompletedTask;
    }

    [Test]
    public async Task PersonalFmMethods_Should_ExposePlayCoreContractTypes()
    {
        var queueMethod = typeof(IPersonalFmProvidable).GetMethod(nameof(IPersonalFmProvidable.GetPersonalFmQueueAsync));
        var trashMethod = typeof(IPersonalFmProvidable).GetMethod(nameof(IPersonalFmProvidable.TrashPersonalFmSongAsync));
        var contextMethod = typeof(IPersonalFmProvidable).GetMethod(nameof(IPersonalFmProvidable.GetPersonalFmContextAsync));

        queueMethod.Should().NotBeNull();
        queueMethod!.ReturnType.Should().Be(typeof(Task<List<SingleSongBase>>));
        trashMethod.Should().NotBeNull();
        trashMethod!.ReturnType.Should().Be(typeof(Task));
        contextMethod.Should().NotBeNull();
        contextMethod!.ReturnType.Should().Be(typeof(Task<ContainerBase>));
        await Task.CompletedTask;
    }
}
