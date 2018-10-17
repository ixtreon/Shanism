using System;
using System.Globalization;

namespace Shanism.Client.Models.Text
{

    public enum UnicodeClass
    {
        Letter,
        Mark,
        Number,
        Punctuation,
        Symbol,
        Separator,
        Other
    }

    static class UnicodeClassExtensions
    {
        public static UnicodeClass GetUnicodeClass(this char c)
        {
            switch (CharUnicodeInfo.GetUnicodeCategory(c))
            {
                case UnicodeCategory.ClosePunctuation:
                case UnicodeCategory.ConnectorPunctuation:
                case UnicodeCategory.DashPunctuation:
                case UnicodeCategory.FinalQuotePunctuation:
                case UnicodeCategory.InitialQuotePunctuation:
                case UnicodeCategory.OpenPunctuation:
                case UnicodeCategory.OtherPunctuation:
                    return UnicodeClass.Punctuation;

                case UnicodeCategory.CurrencySymbol:
                case UnicodeCategory.MathSymbol:
                case UnicodeCategory.ModifierSymbol:
                case UnicodeCategory.OtherSymbol:
                    return UnicodeClass.Symbol;

                case UnicodeCategory.LineSeparator:
                case UnicodeCategory.ParagraphSeparator:
                case UnicodeCategory.SpaceSeparator:
                    return UnicodeClass.Separator;

                case UnicodeCategory.Control:
                case UnicodeCategory.Format:
                case UnicodeCategory.OtherNotAssigned:
                case UnicodeCategory.PrivateUse:
                case UnicodeCategory.Surrogate:
                    return UnicodeClass.Other;

                case UnicodeCategory.LetterNumber:
                case UnicodeCategory.OtherNumber:
                case UnicodeCategory.DecimalDigitNumber:
                    return UnicodeClass.Number;

                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.ModifierLetter:
                case UnicodeCategory.OtherLetter:
                case UnicodeCategory.TitlecaseLetter:
                case UnicodeCategory.UppercaseLetter:
                    return UnicodeClass.Letter;

                case UnicodeCategory.NonSpacingMark:
                case UnicodeCategory.EnclosingMark:
                case UnicodeCategory.SpacingCombiningMark:
                    return UnicodeClass.Mark;
            }
            throw new NotImplementedException();
        }

    }
}
