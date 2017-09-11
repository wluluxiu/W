using UnityEngine;
using System;
using System.Collections.Generic;

public interface ICaster
{
	void gameLogin( object loginData, object dataExt = null );
	void gameLogout();
}

public class Player
{	
	private Dictionary<Type, ICaster> mProxies = new Dictionary<Type, ICaster>();
	public System.DateTime loginday;

	public Player()
	{
		
	}

	~Player()
	{
		//LogModule.DebugLog( "------->>> Player Destroy" );
	}

	public void OnInit()
	{
		plyLogin(null);
	}

    public void OnQuit()
    {
        plyLogout();
    }

    public void plyLogin( object loginData )
	{
		DataAccessor.Instance().resInitMap();
		loginday = new System.DateTime( System.DateTime.UtcNow.Year , System.DateTime.UtcNow.Month , System.DateTime.UtcNow.Day , 0 , 0 , 0 , 0);
	}
		
	public void plyLogout()
	{
		loginday = new System.DateTime( System.DateTime.UtcNow.Year ,System.DateTime.UtcNow.Month , System.DateTime.UtcNow.Day , 0 , 0 , 0 , 0  );
	}
		
	public void plyEnterMain()
	{

	}

	public void plyEnterBattle(  )
	{

	}
		
}
