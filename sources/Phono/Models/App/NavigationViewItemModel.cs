using FluentIcons.Common;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phono.Models.App
{
    public class NavigationViewItemModel
    {
        public string Title { get; set; }
        public Symbol Symbol { get; set; } 
        public string PageId { get; set; }
        public bool IsSelected { get; set; }
        public Visibility IsVisible { get; set; }
    }
}
