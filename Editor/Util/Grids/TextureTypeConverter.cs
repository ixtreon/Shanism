using Shanism.Editor.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Editor.Utils
{
    class TextureTypeConverter : TypeConverter
    {
        static TextureViewModel[] textures;

        public static void SetTextures(TextureViewModel[] texs)
        {
            textures = texs;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) 
            => true; // display drop

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) 
            => false; // drop-down vs combo

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
            => new StandardValuesCollection(textures);

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            => sourceType == typeof(string);
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            => destinationType == typeof(string);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return textures.FirstOrDefault(t => t.Name.Equals(value));
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return (value as TextureViewModel)?.Name;
        }
    }
}
