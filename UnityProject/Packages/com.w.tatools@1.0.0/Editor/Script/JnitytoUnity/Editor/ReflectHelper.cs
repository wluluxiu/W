using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class ReflectHelper 
{
	/// <summary>
	/// 修改实例对应只读或可读写属性的值
	/// </summary>
	/// <typeparam name="T">实例对象类型</typeparam>
	/// <typeparam name="S">属性类型</typeparam>
	/// <param name="instance">实例对象</param>
	/// <param name="membername">属性名</param>
	/// <param name="Singlevalue">赋予的单值</param>
	/// <param name="assignlist">赋予的List值</param>
	/// <param name="appendOrReplace">对于泛型集合属性，appendOrReplace为true,单值默认为false</param>
	/// <param name="StartIndex">>对于泛型集合属性,赋予list新值在原List的StartIndex索引，默认为-1</param>
	/// <param name="length">索引长度，默认为0</param>
	public static void ModifyFieldsValue<T, S>(T instance, string membername, object Singlevalue = null, List<S> assignlist = null, bool appendOrReplace = false, int StartIndex = -1, int length = 0)
	{
		var newType = instance.GetType();
		var pInfo = newType.GetRuntimeProperties().FirstOrDefault(p => p.Name == membername);
		if (pInfo != null && pInfo.CanWrite)
		{
			if (appendOrReplace)
			{
				List<S> fVal = (List<S>)pInfo.GetValue(instance);
				//原List属性没有值时：
				//if (fVal == null)
				//{
					SetListValueInField(instance, assignlist, pInfo);
				//}
				////原List属性存在值时：
				//else
				//{
				//	AppendOrReplaceList(assignlist, StartIndex, length, fVal);
				//	pInfo.SetValue(instance, fVal);
				//}
			}
			else
			{
				//(S)Convert.ChangeType(Singlevalue, typeof(S))
				pInfo.SetValue(instance, Singlevalue);
			}
		}
		else if (pInfo != null)
		{
			var backFildInfo = newType.GetRuntimeFields().FirstOrDefault(f => f.Name.Contains($"<{membername}>") && f.Name.Contains("BackingField"));
			if (appendOrReplace)
			{
				List<S> fVal = (List<S>)backFildInfo.GetValue(instance);
				if (fVal == null)
				{
					SetListValueInField(instance, assignlist, backFildInfo);
				}
				else
				{
					AppendOrReplaceList(assignlist, StartIndex, length, fVal);
				}
			}
			else
			{
				backFildInfo.SetValue(instance, Singlevalue);
			}
		}
		else
		{
			return;
		}

	}

	private static void AppendOrReplaceList<S>(List<S> assignlist, int StartIndex, int length, List<S> fVal)
	{
		//if (fVal.Count < length || fVal.Count < assignlist.Count)
		//{
		//	throw new Exception("传入的length值或assignlist不对，其应小于或等于原集合长度");
		//}
		//if (StartIndex != -1 && length > 0)
		//{
		//	for (int index = 0; index < length; index++)
		//	{
		//		fVal[StartIndex + index] = assignlist[index];
		//	}
		//}
		fVal = assignlist;
	}

	private static void SetListValueInField<T, S>(T instance, List<S> assginlist, MemberInfo pInfo)
	{
		var type1 = typeof(List<>);
		if (pInfo is PropertyInfo)
		{
			type1 = type1.MakeGenericType((pInfo as PropertyInfo).PropertyType.GenericTypeArguments.First());
			object instanceVal = CreateInstanceWithList(assginlist, type1);
			(pInfo as PropertyInfo).SetValue(instance, instanceVal);
		}
		if (pInfo is FieldInfo)
		{
			type1 = type1.MakeGenericType(((pInfo as FieldInfo)).FieldType.GenericTypeArguments.First());
			object instanceVal = CreateInstanceWithList(assginlist, type1);
			(pInfo as FieldInfo).SetValue(instance, instanceVal);
		}
	}

	private static object CreateInstanceWithList<S>(List<S> assginlist, Type type1)
	{
		var instanceVal = Activator.CreateInstance(type1);
		var add = type1.GetMethod("Add", type1.GetGenericArguments());
		foreach (var obj in assginlist)
		{
			List<object> parametersList = new List<object>();
			parametersList.Add(obj);
			add.Invoke(instanceVal, parametersList.ToArray());
		}
		return instanceVal;
	}
}
