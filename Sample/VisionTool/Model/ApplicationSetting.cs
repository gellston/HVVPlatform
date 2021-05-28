using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Model
{
    public class ApplicationSetting
    {
        public ApplicationSetting()
        {

        }

        public string ModulePath { get; set; }
        public string ModuleConfigPath { get; set; }
        public string ModuleMainPath { get; set; }
        public string ModuleThirdPartyDLLPath { get; set; }


        public string DiagramPath { get; set; }
        public string DiagramConfigPath { get; set; }

        public string DiagramImagePath { get; set; }



        private ObservableCollection<string> _DiagramDataTypeCollection = null;
        public ObservableCollection<string> DiagramDataTypeCollection
        {
            get
            {
                _DiagramDataTypeCollection ??= new ObservableCollection<string>();
                return _DiagramDataTypeCollection;
            }
        }


        private ObservableCollection<string> _DiagramCategoryCollection = null;
        public ObservableCollection<string> DiagramCategoryCollection
        {
            get
            {
                _DiagramCategoryCollection ??= new ObservableCollection<string>();
                return _DiagramCategoryCollection;
            }
        }



        private ObservableCollection<string> _DiagramPropertyDataTypeCollection = null;
        public ObservableCollection<string> DiagramPropertyDataTypeCollection
        {
            get
            {
                _DiagramPropertyDataTypeCollection ??= new ObservableCollection<string>();
                return _DiagramPropertyDataTypeCollection;
            }
        }


    }
}
