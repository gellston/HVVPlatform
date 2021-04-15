using ActiproSoftware.Text;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Markup;

namespace VisionTool.UC
{
    public class AttachedLangaugeProperty : DependencyObject
    {
        public static readonly DependencyProperty LanguageProperty = DependencyProperty.RegisterAttached("Language", typeof(ISyntaxLanguage), typeof(AttachedLangaugeProperty), new PropertyMetadata(""));
        public static ISyntaxLanguage GetSecurityId(DependencyObject d)
        {
            return (ISyntaxLanguage)d.GetValue(LanguageProperty);
        }
        public static void SetSecurityId(DependencyObject d, ISyntaxLanguage value)
        {
            d.SetValue(LanguageProperty, value);
        }
    }
}
