using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DapperTestbed
{
	public class RandomBinaryToken
	{
		public RandomBinaryToken(int byteLength)
		{
			Contract.Requires(byteLength > 0);

			var value = new byte[byteLength];

			new RNGCryptoServiceProvider().GetBytes(value);

			Value = Convert.ToBase64String(value);
		}

		protected RandomBinaryToken()
		{
		}

		public virtual string Value { get; private set; }
	}
}
