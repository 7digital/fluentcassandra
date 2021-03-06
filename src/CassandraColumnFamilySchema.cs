﻿using System;
using System.Collections.Generic;
using System.Linq;
using Apache.Cassandra;
using FluentCassandra.Types;

namespace FluentCassandra
{
	public class CassandraColumnFamilySchema
	{
		public static readonly AsciiType DefaultKeyName = "KEY";

		public CassandraColumnFamilySchema(CfDef def)
		{
			var familyType = ColumnType.Standard;
			Enum.TryParse<ColumnType>(def.Column_type, out familyType);

			var keyType = CassandraObject.ParseType(def.Key_validation_class);
			var defaultColumnValueType = CassandraObject.ParseType(def.Default_validation_class);
			CassandraType columnNameType, superColumnNameType;

			if (familyType == ColumnType.Super)
			{
				superColumnNameType = CassandraObject.ParseType(def.Comparator_type);
				columnNameType = CassandraObject.ParseType(def.Subcomparator_type);
			}
			else
			{
				superColumnNameType = null;
				columnNameType = CassandraObject.ParseType(def.Comparator_type);
			}

			FamilyType = familyType;
			FamilyName = def.Name;
			FamilyDescription = def.Comment;

			KeyName = CassandraObject.GetTypeFromDatabaseValue(def.Key_alias, CassandraType.BytesType);
			KeyType = keyType;
			SuperColumnNameType = superColumnNameType;
			ColumnNameType = columnNameType;
			DefaultColumnValueType = defaultColumnValueType;

			Columns = def.Column_metadata.Select(col => new CassandraColumnSchema(col, columnNameType)).ToList();
		}

		public CassandraColumnFamilySchema(string name = null, ColumnType type = ColumnType.Standard)
		{
			FamilyType = type;
			FamilyName = name;
			FamilyDescription = null;

			KeyName = DefaultKeyName;
			KeyType = CassandraType.BytesType;
			SuperColumnNameType = type == ColumnType.Super ? CassandraType.BytesType : null;
			ColumnNameType = CassandraType.BytesType;
			DefaultColumnValueType = CassandraType.BytesType;

			Columns = new List<CassandraColumnSchema>();
		}

		public ColumnType FamilyType { get; set; }
		public string FamilyName { get; set; }
		public string FamilyDescription { get; set; }

		public CassandraObject KeyName { get; set; }
		public CassandraType KeyType { get; set; }
		public CassandraType SuperColumnNameType { get; set; }
		public CassandraType ColumnNameType { get; set; }
		public CassandraType DefaultColumnValueType { get; set; }

		public IList<CassandraColumnSchema> Columns { get; set; }
	}
}
