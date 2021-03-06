﻿using System;

namespace FluentCassandra.Types
{
	internal class DecimalTypeConverter : CassandraObjectConverter<decimal>
	{
		public override bool CanConvertFrom(Type sourceType)
		{
			if (Type.GetTypeCode(sourceType) != TypeCode.Object)
				return true;

			return sourceType == typeof(byte[]);
		}

		public override bool CanConvertTo(Type destinationType)
		{
			if (Type.GetTypeCode(destinationType) != TypeCode.Object)
				return true;

			return destinationType == typeof(byte[]);
		}

		public override decimal ConvertFromInternal(object value)
		{
			if (value is byte[])
				return ((byte[])value).FromBytes<decimal>();

			return (decimal)Convert.ChangeType(value, typeof(decimal));
		}

		public override object ConvertToInternal(decimal value, Type destinationType)
		{
			if (destinationType == typeof(byte[]))
				return value.ToBytes();

			return Convert.ChangeType(value, destinationType);
		}
	}
}
