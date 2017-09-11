using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataBase
{
	public virtual void Init(){}
}

public class DataAccessor : Singleton<DataAccessor> {

	private static DataAccessor instance_;

	public Dictionary< string, DataBase > dataMaps = new Dictionary<string, DataBase>();

	public void resInitMap()
	{
		dataMaps.Clear();

	}
}
