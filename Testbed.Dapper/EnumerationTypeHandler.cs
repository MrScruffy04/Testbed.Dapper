namespace Testbed.Dapper
{
	using System;
	using System.Data;

	using global::Dapper;

	public class EnumerationTypeHandler<T, TId> : SqlMapper.TypeHandler<T> where T : Enumeration<T, TId>
	{
		public override T Parse(object value)
		{
			return Enumeration<T, TId>.FromId((TId)value);
		}

		public override void SetValue(IDbDataParameter parameter, T value)
		{
			parameter.DbType = GetDbType();

			parameter.Value = value == null
				? DBNull.Value
				: (object)value.Id;
		}

		private DbType GetDbType()
		{
			if (typeof(TId) == typeof(byte))
			{
				return DbType.Byte;
			}

			if (typeof(TId) == typeof(short))
			{
				return DbType.Int16;
			}

			if (typeof(TId) == typeof(int))
			{
				return DbType.Int32;
			}

			if (typeof(TId) == typeof(long))
			{
				return DbType.Int64;
			}

			return default(DbType);
		}
	}
}
