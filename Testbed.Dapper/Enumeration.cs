namespace Testbed.Dapper
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Reflection;

	public abstract class Enumeration<T, TId> : IEquatable<T> where T : Enumeration<T, TId>
	{
		#region Static

		public static bool operator ==(Enumeration<T, TId> x, T y)
		{
			return Object.ReferenceEquals(x, y) || (!Object.ReferenceEquals(x, null) && x.Equals(y));
		}

		public static bool operator !=(Enumeration<T, TId> first, T second)
		{
			return !(first == second);
		}

		public static T FromId(TId id)
		{
			Contract.Requires(((object) id) != null);

			return AllValues.Where(value => value.Id.Equals(id)).FirstOrDefault();
		}

		public static readonly ReadOnlyCollection<T> AllValues = FindValues();

		private static ReadOnlyCollection<T> FindValues()
		{
			var values = typeof(T)
				.GetFields(BindingFlags.Static | BindingFlags.Public)
				.Where(fieldInfo => fieldInfo.FieldType == typeof(T))
				.Select(fieldInfo => (T) fieldInfo.GetValue(null))
				.ToList()
				.AsReadOnly();

			var duplicateGroups = values
				.GroupBy(value => value.Id)
				.Where(group => group.Count() > 1)
				.ToList();

			if (duplicateGroups.Count > 0)
			{
				throw new AggregateException(
					duplicateGroups
						.Select(group => new DuplicateEnumerationIdException(
							String.Format("Enumeration type {0} duplicates ID {1}", typeof(T), group.Key)))
						.ToArray());
			}

			return values;
		}

		#endregion

		protected Enumeration(TId id, string name)
		{
			Contract.Requires(((object) id) != null);
			Contract.Requires(!String.IsNullOrEmpty(name));

			Id = id;
			Name = name;
		}

		protected Enumeration()
		{
		}

		#region Overrides : Object

		public override bool Equals(object obj)
		{
			return Equals(obj as T);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override string ToString()
		{
			return Name;
		}

		#endregion

		#region IEquatable

		public virtual bool Equals(T other)
		{
			//  Normally this would include a type equality check (GetType() == other.GetType()), but that is broken 
			//  by proxies. We have to allow derived types to equal base types for enumerations. They should not have 
			//  deep inheritance hierarchies, but the ORM dictates they cannot be sealed.

			return other != null && IdComparer.Equals(Id, other.Id);
		}

		#endregion

		public virtual TId Id { get; private set; }

		public virtual string Name { get; private set; }

		protected virtual IEqualityComparer<TId> IdComparer
		{
			get { return EqualityComparer<TId>.Default; }
		}
	}
}