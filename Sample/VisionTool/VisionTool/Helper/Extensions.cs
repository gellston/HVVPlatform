using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;
using System.Windows.Media;
using VisionTool.Model.DiagramProperty;

namespace VisionTool.Helper
{
    static class Extensions
    {
        public static List<BaseDiagramProperty> Clone(this List<BaseDiagramProperty> listToClone)
        {
            var list = new List<BaseDiagramProperty>();

            foreach(var item in listToClone)
            {
                var newItem = (BaseDiagramProperty)item.Clone();
                list.Add(newItem);
                
            }

            return list;
        }

        public static object DeepClone(object obj)
        {
            object objResult = null;

            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, obj);

                ms.Position = 0;
                objResult = bf.Deserialize(ms);
            }

            return objResult;
        }

        private static T FindAnchestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }
    }
}
