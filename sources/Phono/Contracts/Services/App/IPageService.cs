using System;
using System.Collections.Generic;
using System.Text;

namespace Phono.Contracts.Services.App
{
    public interface IPageService
    {
        Type GetPageType(string key);
    }
}
