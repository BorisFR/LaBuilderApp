using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LaBuilderApp
{

	public class SingleValueArrayConverter<T> : JsonConverter
	{
		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException ();
		}

		public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			object retVal = new Object ();
			if (reader.TokenType == JsonToken.StartObject) {
				T instance = (T)serializer.Deserialize (reader, typeof (T));
				retVal = new List<T> () { instance };
			} else if (reader.TokenType == JsonToken.StartArray) {
				retVal = serializer.Deserialize (reader, objectType);
			}
			return retVal;
		}

		public override bool CanConvert (Type objectType)
		{
			return false;
		}
	}

	public static class EventsManager
	{

		public static List<Exhibition> All;

		public static void TheData (string data)
		{
			string temp = data.Replace ("AdminList\":{", "AdminList\":[{").Replace ("},\"BuilderList", "}],\"BuilderList")
							  .Replace ("BuilderList\":{", "BuilderList\":[{").Replace ("},\"Logo", "}],\"Logo");
			Tools.Trace ("TheData");
			try {
				All = JsonConvert.DeserializeObject<List<Exhibition>> (temp);
			} catch (Exception err) {
				Tools.Trace ("Error: " + err.Message);
			}
			Tools.Trace ("TheData: " + All.Count.ToString ());
		}

	}
}