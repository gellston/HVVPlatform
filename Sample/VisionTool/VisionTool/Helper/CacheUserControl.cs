using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace VisionTool.Helper
{

    public class ViewFactory
    {
        private static readonly Dictionary<string, Func<UIElement>> _registry = new Dictionary<string, Func<UIElement>>();

        private static string Key(Type viewModelType)
        {
            return viewModelType.FullName;
        }

        public static void RegisterView(Type viewModelType, Func<UIElement> createView)
        {
            _registry.Add(Key(viewModelType), createView);
        }

        public static UIElement GetView(Type viewModelType)
        {
            var key = Key(viewModelType);
            if (!_registry.ContainsKey(key))
                return null;

            return _registry[key]();
        }
    }

    public class CacheContentControl : ContentControl
    {
        private static Dictionary<Type, Control> cache = new Dictionary<Type, Control>();
        public CacheContentControl()
        {
            Unloaded += CacheContentControl_Unloaded;
        }
        private void CacheContentControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Content = null;
        }
        private Type _contentType;
        public Type ContentType
        {
            get { return _contentType; }
            set
            {
                _contentType = value;
                Content = GetView(_contentType);
            }
        }
        public Control GetView(Type type)
        {
            if (!cache.ContainsKey(type))
            {
                cache.Add(type, (Control)Activator.CreateInstance(type));
            }
            return cache[type];
        }
    }

}
