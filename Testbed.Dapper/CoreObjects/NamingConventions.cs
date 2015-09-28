namespace Testbed.Dapper.CoreObjects
{
	[System.Flags]
	public enum NamingConventions
	{
		None,
		Underscore,
		CamelCase,

		UnderscoreAndCamelCase = Underscore | CamelCase,
	}
}
