using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HyPlayer.NeteaseApi.ApiContracts;
using HyPlayer.NeteaseApi.ApiContracts.Login;
using HyPlayer.NeteaseApi.ApiContracts.Utils;
using HyPlayer.NeteaseProvider;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Phono.Contracts.ViewModels;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage.Streams;

namespace Phono.ViewModels.Netease
{
    public partial class SignInViewModel : ObservableRecipient, IScopedViewModel
    {
        private dynamic key;
        private readonly NeteaseProvider _neteaseProvider;

        [ObservableProperty] private BitmapImage _qrCodeImage;
        [ObservableProperty] private string _nowQrKey;

        public SignInViewModel(NeteaseProvider neteaseProvider)
        {
            _neteaseProvider = neteaseProvider;
        }

        public async Task LoadDataAsync(Image image)
        {
            try
            {
                
                try
                {
                    key = await _neteaseProvider.RequestAsync(NeteaseApis.LoginQrCodeUnikeyApi, new LoginQrCodeUnikeyRequest());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"LoginQrCodeUnikeyApi Error: {ex.Message}");
                    return;
                }
                if (key.IsError)
                {
                    Debug.WriteLine($"LoginQrCodeUnikeyApi Error: {key.Error}");
                    return;
                }
                Debug.WriteLine("生成二维码成功，等待扫码登录");
                _ = GenerateQrCodeImage(key.Value.Unikey);
                
                NowQrKey = key.Value.Unikey;
                Task.Run(() => ListenToLoginEvent()).SafeFireAndForget();
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoadDataAsync Error: {ex.Message}");
            }

        }

        private async Task ListenToLoginEvent()
        {
            while (_neteaseProvider.LoginedUser == null && NowQrKey == key.Value.Unikey)
            {
                var res = await _neteaseProvider.RequestAsync(NeteaseApis.LoginQrCodeCheckApi,
                                                       new LoginQrCodeCheckRequest() { Unikey = key.Value.Unikey });
                if (res.Value.Code == 800)
                {
                    key = await _neteaseProvider.RequestAsync(NeteaseApis.LoginQrCodeUnikeyApi, new LoginQrCodeUnikeyRequest());
                    if (key.IsError)
                    {
                        Debug.WriteLine("获取UniKey失败" + key.Error.Message);
                        return;
                    }
                    try
                    {
                        _ = GenerateQrCodeImage(key.Value.Unikey);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message + (ex.InnerException ?? new Exception()).Message);
                    }
                }
                else if (res.Value.Code == 801)
                {

                }
                else if (res.Value.Code == 803)
                {

                    //await LoginDone();
                    break;
                }
                else if (res.Value.Code == 802)
                {

                }

                await Task.Delay(2000);
            }

        }

        private async Task GenerateQrCodeImage(string unikey)
        {
            var QrUri = new Uri("https://music.163.com/login?codekey=" + unikey);
            var img = new BitmapImage();

            var qrGenerator = new QRCodeGenerator();
            var qrData = qrGenerator.CreateQrCode(QrUri.ToString(), QRCodeGenerator.ECCLevel.M);
            var qrCode = new BitmapByteQRCode(qrData);
            var qrImage = qrCode.GetGraphic(20);
            using (var memoryStream = new MemoryStream(qrImage))
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                var randomAccessStream = memoryStream.AsRandomAccessStream();

                // 使用 SetSourceAsync 加载
                await img.SetSourceAsync(randomAccessStream);
            }
            var dispatcher = DispatcherQueue.GetForCurrentThread();
            // 直接设置 Image 源，绕过绑定
            QrCodeImage = img;
            // 设置到属性（必须在 UI 线程）
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
