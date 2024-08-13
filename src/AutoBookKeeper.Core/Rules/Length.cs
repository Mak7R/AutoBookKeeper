using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBookKeeper.Core.Rules;

public static class Length
{   
    public static class UserStrings
    {
        public const int MinUserNameLength = 4;
        public const int MaxUserNameLength = 64;
        public const int MaxEmailLength = 320;
        public const int MaxPasswordHashLength = 128;
        public const int MaxPasswordLength = 256;
        public const int MinPasswordLength = 4;
    } 

    public static class BookStrings
    {
        public const int MinTitleLength = 3;
        public const int MaxTitleLength = 64;
        public const int MaxDescriptionLength = 256;
    }

    public static class RoleStrings
    {
        public const int MinNameLength = 2;
        public const int MaxNameLength = 16;
    }

    public static class TransactionStrings
    {
        public const int MinNameIdentifierLength = 4;
        public const int MaxNameIdentifierLength = 64;

        public const int MaxDescriptionLength = 256;
    }
    public static class TransactionTypeStrings
    {
        public const int MinNameLength = 2;
        public const int MaxNameLength = 32;

        public const int MaxDescriptionLength = 256;
    }
}