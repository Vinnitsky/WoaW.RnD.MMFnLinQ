using System;

namespace WoaW.RnD.MMF
{
    public struct MyColor
    {
        public short Red;
        public short Green;
        public short Blue;
        public short Alpha;

        // Make the view brighter.
        public void Brighten(short value)
        {
            Red = (short)Math.Min(short.MaxValue, (int)Red + value);
            Green = (short)Math.Min(short.MaxValue, (int)Green + value);
            Blue = (short)Math.Min(short.MaxValue, (int)Blue + value);
            Alpha = (short)Math.Min(short.MaxValue, (int)Alpha + value);
        }
    }
}
