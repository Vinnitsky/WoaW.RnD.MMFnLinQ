using System.Runtime.InteropServices;

namespace WoaW.RnD.MMF
{
    //[StructLayout(LayoutKind.Explicit)]
    //public struct ItemRecord
    //{
    //    [FieldOffset(0)]
    //    public int PageNum;
    //    [FieldOffset(0)]
    //    public int Offset;
    //    [FieldOffset(0)]
    //    public int Length;
    //    [MarshalAs(UnmanagedType.LPStr)]
    //    public string TypeName;
    //}

    public struct ItemRecord
    {
        public int PageNum;
        public int Offset;
        public int Length;
    }
}
