using System.Windows;

namespace SvgViewer.Utility
{
    public static class FreezableEx
    {
        public static T DoFreeze<T>(this T freezable)where T:Freezable
        {
            if(freezable.CanFreeze & freezable.IsFrozen is false)
                freezable.Freeze();
            return freezable;
        }
    }
}
