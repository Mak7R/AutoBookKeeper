using AutoBookKeeper.Core.ValueObjects;

namespace AutoBookKeeper.Core.Extensions;

public static class BookAccessExtensions
{
    public static bool HasFullAccess(this BookAccess access)
    {
        return access == BookAccess.Full;
    }

    public static bool HasUpdateAccess(this BookAccess access) {
        return access == BookAccess.Update || access.HasFullAccess();
    }

    public static bool HasAddAccess(this BookAccess access) { 
        return access == BookAccess.AddOnly || access.HasUpdateAccess();
    }

    public static bool HasViewAccess(this BookAccess access)
    {
        return access == BookAccess.View || access.HasAddAccess();
    }
}
