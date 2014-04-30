using System;

namespace C3.Framework
{
	public interface ITypeConverter
	{
		int ToInt(object source);
		int ToInt(object source, int defaultValue);
        int? ToNullInt(object source);
        int? ToNullInt(object source, int? defaultValue);
		bool ToBool(object source);
		bool ToBool(object source, bool defaultValue);
        bool? ToNullBool(object source);
        bool? ToNullBool(object source, bool? defaultValue);
		string ToString(object source);
		string ToString(object source, string defaultValue);
		char ToChar(object source);
		char ToChar(object source, char defaultValue);
        short ToShort(object source);
        short ToShort(object source, short defaultValue);
		double ToDouble(object source);
		double ToDouble(object source, double defaultValue);
        double? ToNullDouble(object source);
        double? ToNullDouble(object source, double? defaultValue);
		DateTime? ToDate(object source);
		DateTime? ToDate(object source, DateTime? defaultValue);
        DateTime? ToDate(string date, string format);
        Guid ToGuid(object source);
        Guid ToGuid(object source, Guid defaultValue);
        Decimal ToDecimal(object source);
        Decimal ToDecimal(object source, Decimal defaultValue);
        Decimal? ToNullDecimal(object source);
        Decimal? ToNullDecimal(object source, Decimal? defaultValue);
	}
}