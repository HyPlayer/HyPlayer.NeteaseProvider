using Microsoft.UI.Xaml;
using System.Collections.Generic;


namespace Phono.Helpers
{
    internal class WindowHelper
    {
        public static List<Window> ActiveWindows { get { return _activeWindows; } }
        private static List<Window> _activeWindows = new List<Window>();

        public static Window CreateWindow()
        {
            var window = new Window();
            TrackWindow(window);
            return window;
        }

        public static void TrackWindow(Window window)
        {
            window.Closed += (sender, args) =>
            {
                _activeWindows.Remove(window);
            };
            _activeWindows.Add(window);
        }


        public static Window? GetWindowForElement(UIElement element)
        {
            if (element.XamlRoot != null)
            {
                foreach (Window window in _activeWindows)
                {
                    if (element.XamlRoot == window.Content.XamlRoot)
                    {
                        return window;
                    }
                }
            }
            return null;
        }
    }
}
