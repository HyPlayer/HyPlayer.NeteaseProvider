using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.ApiContracts.Utils;
using HyPlayer.NeteaseProvider;
using Phono.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.ExchangeActiveSyncProvisioning;

namespace Phono.ViewModels.Netease
{
    public partial class SignInViewModel : ObservableRecipient, IScopedViewModel
    {
        private readonly NeteaseProvider _neteaseProvider;

        public SignInViewModel(NeteaseProvider neteaseProvider)
        {
            _neteaseProvider = neteaseProvider;
        }

        [RelayCommand]
        private async Task CreateRandomDeviceId()
        {
            var option = _neteaseProvider.Handler.Option;
            option.Cookies.Clear();
            option.AdditionalParameters.Headers.Clear();

            var uri = new Uri("ms-appx:///Assets/deviceid.txt");
            var storagefile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri);
            var lines = await Windows.Storage.FileIO.ReadLinesAsync(storagefile);
            var idx = new Random().Next(lines.Count - 1);
            var deviceId = lines[idx];

            option.AdditionalParameters.Cookies["deviceId"] = deviceId;
            option.AdditionalParameters.Cookies["os"] = "pc";
            option.AdditionalParameters.Cookies["approver"] = "3.1.3.203419";

            var rst = await _neteaseProvider.RequestAsync(NeteaseApis.RegisterAnonymousApi, new RegisterAnonymousRequest()
            {
                DeviceId = deviceId
            });

            if (rst.IsError)
            {
                Debug.WriteLine($"RegisterAnonymousApi Error: {rst.Error}");
            }

            else
            {
                Debug.WriteLine($"RegisterAnonymousApi Success, Current Id: {deviceId}");
            }
        }

        [RelayCommand]
        private async Task RegisterByCurrentDeviceId()
        {
            try
            {
                // get current device guid
                var deviceInfo = new EasClientDeviceInformation();
                var deviceId = deviceInfo.Id;
                var androidId = deviceId.ToString("N").Substring(0, 16);
                var imei = deviceId.ToString("N").Substring(16);
                var rst = await _neteaseProvider.RequestAsync(NeteaseApis.LoginAnnounceDeviceApi, new LoginAnnounceDeviceRequest
                {
                    Imei = imei,
                    AndroidId = androidId,
                    LocalId = null,
                    DeviceName = deviceInfo.FriendlyName,
                });
                if (rst.IsError)
                {
                    Debug.WriteLine("设备ID注册失败, 请尝试其他方案", "获取失败: " + rst.Error.Message);
                    return;
                }
                Debug.WriteLine("设备ID注册成功", "临时用户 ID: " + rst.Value.Data?.Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("设备ID注册失败, 请尝试其他方案", "错误: " + ex.Message);
                return;
            }
        }
    }
}
