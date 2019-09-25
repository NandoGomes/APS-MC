using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using APS.MC.Shared.APSShared.Entities;

namespace APS.MC.Shared.APSShared.Commands
{
	public static class ResponseBuilder
	{
		public static E Build<T, E>(this E result, T entity, string returnFields = "") where E : ICommandResult where T : Entity
		{
			List<PropertyInfo> fields = new List<PropertyInfo>();
			List<PropertyInfo> resultProperties = result.GetType().GetProperties().ToList();

			if (returnFields != null && returnFields.Trim() != string.Empty)
				fields = resultProperties.Where(resultProperty => returnFields.Split(',').Where(returnField => returnField.ToLower().Trim() == resultProperty.Name.ToLower()).FirstOrDefault() != null).ToList();

			else
				fields = resultProperties.Where(resultProperty => entity.GetType().GetProperties().Where(entityProperty => entityProperty.Name.ToLower() == resultProperty.Name.ToLower()).FirstOrDefault() != null).ToList();

			foreach (PropertyInfo property in fields)
				if (property.SetMethod != null)
					property.SetValue(result, entity.GetType().GetProperty(property.Name).GetValue(entity));

			return result;
		}

		public static E Build<T, E, F>(this E result, IEnumerable<T> entities, string returnFields = "") where E : ICommandResult where T : Entity
		{
			IEnumerable<KeyValuePair<PropertyInfo, PropertyInfo>> fields = new List<KeyValuePair<PropertyInfo, PropertyInfo>>();
			IReadOnlyCollection<PropertyInfo> entityProperties = typeof(T).GetProperties();
			IReadOnlyCollection<PropertyInfo> resultProperties = typeof(F).GetProperties();

			if (returnFields != null && returnFields.Trim() != string.Empty)
				fields = from EP in entityProperties
						 join RP in resultProperties
							on
								EP.Name.ToLower()
							equals
								RP.Name.ToLower()
						 join RF in returnFields.Split(',')
							on
								EP.Name.ToLower()
							equals
								RF.ToLower().Trim()
						 where RP.SetMethod != null
						 select new KeyValuePair<PropertyInfo, PropertyInfo>(RP, EP);

			else
				fields = from EP in entityProperties
						 join RP in resultProperties
							on
								EP.Name.ToLower()
							equals
								RP.Name.ToLower()
						 where RP.SetMethod != null
						 select new KeyValuePair<PropertyInfo, PropertyInfo>(RP, EP);

			List<F> resultList = new List<F>();

			foreach (T entity in entities)
			{
				F resultObject = (F)Activator.CreateInstance(typeof(F), true);

				foreach (KeyValuePair<PropertyInfo, PropertyInfo> field in fields)
					field.Key.SetValue(resultObject, field.Value.GetValue(entity));

				resultList.Add(resultObject);
			}

			PropertyInfo commandResultProperty = typeof(E).GetProperties().Where(property => property.PropertyType == typeof(IEnumerable<F>) && property.SetMethod != null).FirstOrDefault();

			commandResultProperty.SetValue(result, resultList);

			return result;
		}
	}
}