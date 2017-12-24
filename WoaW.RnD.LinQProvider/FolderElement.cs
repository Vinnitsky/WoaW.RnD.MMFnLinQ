namespace WoaW.RnD.LinQProvider
{
    internal class FolderElement : FileSystemElement
    {
        public FolderElement(string path) : base(path)
        {
        }

        public override ElementType ElementType { get { return ElementType.Folder; } }
    }
}