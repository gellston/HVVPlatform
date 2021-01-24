using DevExpress.Xpf.Core;
using Notifications.Wpf.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace WPFHVVPlatform.Service
{
    public class MessageDialogService
    {

        NotificationManager notificaitonManager = new NotificationManager();

        public MessageDialogService()
        {
           

   
        }

        public void ShowToastErrorMessage(string title, string message)
        {
            notificaitonManager.ShowAsync(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = Notifications.Wpf.Core.NotificationType.Error
            });
        }

        public void ShowErrorMessage(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                DXMessageBox.Show(message, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            });

        }

        public bool ShowConfirmMessage(string message)
        {
            bool check = false;
            Application.Current.Dispatcher.Invoke(() =>
            {
                var Result = DXMessageBox.Show(message, "Confirm", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Information);
                if (MessageBoxResult.Yes == Result) check = true;
                else check = false;
            });

            return check;
        }

    }
}
