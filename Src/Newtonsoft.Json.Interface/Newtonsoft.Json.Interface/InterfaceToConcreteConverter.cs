﻿// ***
// *** Copyright(C) 2014-2020, Daniel M. Porrey. All rights reserved.
// *** 
// *** This program is free software: you can redistribute it and/or modify
// *** it under the terms of the GNU Lesser General Public License as published
// *** by the Free Software Foundation, either version 3 of the License, or
// *** (at your option) any later version.
// *** 
// *** This program is distributed in the hope that it will be useful,
// *** but WITHOUT ANY WARRANTY; without even the implied warranty of
// *** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// *** GNU Lesser General Public License for more details.
// *** 
// *** You should have received a copy of the GNU Lesser General Public License
// *** along with this program. If not, see http://www.gnu.org/licenses/.
// *** 
using System;
using Newtonsoft.Json.Linq;

namespace Newtonsoft.Json
{
	/// <summary>
	/// This convert can be sued on any interface definition to instruct the JSON
	/// serializer to use a specific concrete class when de-serializing the instance.
	/// </summary>
	/// <typeparam name="TInterface">The Type that was serialized into the JSON text.</typeparam>
	/// <typeparam name="TConcrete">The Type that specifies the class that will be created.</typeparam>
	public class InterfaceToConcreteConverter<TInterface, TConcrete> : JsonConverter
	{
		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		/// <param name="objectType">Type of the object.</param>
		/// <returns>Returns true if this instance can convert the specified object type, false otherwise.</returns>
		public override bool CanConvert(Type objectType)
		{
			return (objectType == typeof(TInterface));
		}

		/// <summary>
		/// Gets a value indicating whether this Newtonsoft.Json.JsonConverter can read.
		/// </summary>
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this Newtonsoft.Json.JsonConverter can write
		/// JSON.
		/// </summary>
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		/// <param name="reader">The Newtonsoft.Json.JsonReader to read from.</param>
		/// <param name="objectType">Type of the object.</param>
		/// <param name="existingValue">The existing value of object being read.</param>
		/// <param name="serializer">The calling serializer.</param>
		/// <returns>The object value.</returns>
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			object returnValue = null;

			if (objectType == typeof(TInterface))
			{
				// ***
				// *** Create the concrete type
				// ***
				returnValue = Activator.CreateInstance<TConcrete>();

				// ***
				// *** Deserialize the object to a temporary instance
				// ***
				JObject jsonObject = JObject.Load(reader);

				using (JsonReader serializerReader = jsonObject.CreateReader())
				{
					// ***
					// *** Populate the object
					// ***
					serializer.Populate(serializerReader, returnValue);
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		/// <param name="writer">The Newtonsoft.Json.JsonWriter to write to.</param>
		/// <param name="value">The value.</param>
		/// <param name="serializer">The calling serializer.</param>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			// ***
			// *** This method will not be called because CanWrite is returning false. JSON.Net
			// *** will use default logic to serial the object. This is Ok because it will never
			// *** be given an instance of an interface; only concrete classes
			// ***
			throw new NotSupportedException();
		}
	}
}