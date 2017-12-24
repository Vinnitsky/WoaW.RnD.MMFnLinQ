namespace WoaW.RnD.LinQProvider
{
    internal class FileElement : FileSystemElement
    {
        public FileElement(string path) : base(path)
        {
        }
        public override ElementType ElementType { get { return ElementType.File; } }

    }
}