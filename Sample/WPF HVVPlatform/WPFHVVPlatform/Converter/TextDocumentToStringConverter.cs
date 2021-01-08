using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace WPFHVVPlatform.Converter
{
    public class TextDocumentToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var textDocument = new TextDocument();
            textDocument.Text = value.ToString();

            return textDocument;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var textDocument = value as TextDocument;


            return textDocument.Text;
        }
    }
}
