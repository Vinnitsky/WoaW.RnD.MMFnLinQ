using System;
using System.Collections.Generic;
using System.Text;

namespace WoaW.RnD.LinQProvider
{
    public abstract class FileSystemElement
    {
        public string Path { get; private set; }
        public abstract ElementType ElementType { get; }

        protected FileSystemElement(string path)
        {
            Path = path;
        }
    }
}
