using Oragon.Spring.Context;
using Oragon.Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Oragon.Spring.Vault.Tests
{
    public static class SpringHelper
    {

        public static IApplicationContext GetContainer(string fileName)
        {
            return new XmlApplicationContext(GetFilePath(fileName, typeof(SpringHelper)));
        }

        public static string GetFilePath(string fileName, Type associatedTestType)
        {
            if (associatedTestType == null)
            {
                return fileName;
            }
            string[] dataFolders = { ".\\Configs", "." };
            StringBuilder path = null;
            foreach (var testDataFolder in dataFolders)
            {
                // check filesystem
                path = new StringBuilder(testDataFolder).Append(Path.DirectorySeparatorChar.ToString());
                path.Append(associatedTestType.Namespace.Replace(".", Path.DirectorySeparatorChar.ToString()));
                path.Append(Path.DirectorySeparatorChar.ToString()).Append(fileName);
                FileInfo file = new FileInfo(path.ToString());
                if (file.Exists)
                {
                    return path.ToString();
                }
            }
            // interpret as assembly resource
            fileName = $"assembly://{associatedTestType.Assembly.FullName}/{associatedTestType.Namespace}.Configs/{fileName}";
            return fileName;
        }
    }
}
