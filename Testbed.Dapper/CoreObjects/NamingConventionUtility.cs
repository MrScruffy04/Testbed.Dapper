namespace Testbed.Dapper.CoreObjects
{
	using System;
	using System.Text;

	public class NamingConventionUtility
	{
		private readonly string _originalName;

		public NamingConventionUtility(string originalName)
		{
			if (string.IsNullOrEmpty(originalName))
			{
				throw new ArgumentException();
			}

			_originalName = originalName;
		}

		public string ApplyConventions(NamingConventions namingConvention)
		{
			var sb = new StringBuilder(GetPrefix(namingConvention));

			if ((namingConvention & NamingConventions.CamelCase) == NamingConventions.CamelCase)
			{
				sb.Append(GetCamelCase());
			}

			return sb.ToString();
		}

		private string GetPrefix(NamingConventions namingConvention)
		{
			if ((namingConvention & NamingConventions.Underscore) == NamingConventions.Underscore)
			{
				return "_";
			}

			return null;
		}

		private string GetCamelCase()
		{
			var sb = new StringBuilder();

			sb.Append(char.ToLowerInvariant(_originalName[0]));

			sb.Append(_originalName.Substring(1));

			return sb.ToString();
		}
	}
}
