using HyPlayer.NeteaseApi;

namespace HyPlayer.NeteaseProvider.Tests;

public static class Secrets
{
    public static AdditionalParameters AdditionalParameters = new()
    {
        Cookies = new()
        {
            ["WEVNSM"] = "1.0.0",
            ["NMTID"] = "00O3eFL99JclNUE_02Em86OaahuIfYAAAGUzZi8-A",
            ["os"] = "pc",
            ["deviceId"] = "8C1C9EF6E672942A46200F21E64C42CF6EDB162000155D017E56",
            ["osver"] = "Microsoft-Windows-10-Enterprise-Edition-build-26100-64bit",
            ["appver"] = "3.1.3.203419",
            ["clientSign"] =
                "00:15:5D:01:7E:56@@@@@@@@@5f7697e5ffc3cb412c0058ec73e9ac5c92cc7172f294b78dd1507c307d60f642",
            ["channel"] = "netease",
            ["mode"] = "Virtual Machine",
            ["__csrf"] = "3d50c15034b705b351dfef911cd17d9c",
            ["MUSIC_U"] =
                "002DFAEFC0C0378B17DE9EB40F591ACBB58AE4FC7993B0174944EF94DAC4D66D6D9F1EB26BD4E564586CFC222C6F522F66518C2C8A549A93EFE13BEE76E0D9D7AB4902C6672F5CA908206E29C4BD628599A4E705BCC5E8CD011F319E1EF01BF1B74A050AF340AC7E6E54387D9F5532AE9C3A190B2757D8489CE6CD238935EB3C8D35B4FA424BDF92F74843988DDA0A5A895AB382CE5720651761C852545B5A289307DAE2F358316F34CB775C92632148E23195760E69CD48545FC541E5F0840BA89CCE32F35771ECB8DFF30B6B8C4C5D0AD731AA38EB49C486119AAA3251853A11243F27778C58D47CB1151F897C094D5A8AF6F1419A3CBDB6817986AA29CF1D987E0032DD8420C091A437A677C46B0B6A7CFF569F2AE409FCAFA37B9270AEEFBBE65AA150DA19F1E35A29919CFF5CD2E94D599C93E441D3D1B9B1D1D29A48FD3A469732026535A102B2D7E8714DE0F1E94DC6E787731B202D83C7F7BD6AD9D88B67D2E7CD3C90A700CFC652E91FB13E2092F1A1CAEC88908C2D41368E9AE2406211189132479183D433C34F41BE49796315D2BB83C6E8EE2995C68FF2254D04800093306A03CC0829ED3DECF176B6B09F26111249C1D6910A35E7F1A9D687C496",
            ["ntes_kaola_ad"] = "1",
            ["WNMCID"] = "bbyxkz.1738616757624.01.0"
        },
        Headers = new()
        {
            { "mconfig-info", """{"IuRPVVmc3WWul9fT":{"version":733184,"appver":"3.1.3.203419"}}""" },
            { "origin", "orpheus://orpheus" },
            {
                "user-agent",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Safari/537.36 Chrome/91.0.4472.164 NeteaseMusicDesktop/3.1.3.203419"
            },
        },
        EApiHeaders = new()
        {
            ["clientSign"] =
                "00:15:5D:01:7E:56@@@@@@@@@5f7697e5ffc3cb412c0058ec73e9ac5c92cc7172f294b78dd1507c307d60f642",
            ["os"] = "pc",
            ["appver"] = "3.1.3.203419",
            ["deviceId"] = "8C1C9EF6E672942A46200F21E64C42CF6EDB162000155D017E56",
            ["requestId"] = "0",
            ["osver"] = "Microsoft-Windows-10-Enterprise-Edition-build-26100-64bit"
        },
        DataTokens = []
    };
}