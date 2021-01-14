using DevExpress.Xpf.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace WPFHVVPlatform.Service
{
    public class FileDialogService
    {
        public FileDialogService()
        {

        }

        public string OpenFolder()
        {
            DXFolderBrowserDialog dialog = new DXFolderBrowserDialog();
            if (dialog.ShowDialog() == false)
                throw new Exception("Folder is not selected");

            return dialog.SelectedPath;
        }

        public string SaveFile(string filter)
        {
            DXSaveFileDialog dialog = new DXSaveFileDialog();
            dialog.Filter = filter;

            if (dialog.ShowDialog() == false)
                return "";

            return dialog.FileName;
        }

        public string OpenFile(string filter)
        {
            DXOpenFileDialog dialog = new DXOpenFileDialog();
            dialog.Filter = filter;

            if (dialog.ShowDialog() == false)
                return "";

            return dialog.FileName;
        }

    }
}
