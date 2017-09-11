using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainPanel : Window {

	public CanvasGroup cg;
	public Transform SwichFuncTrans;
	public CanvasGroup SwichFuncCg;
	public Image SwichFuncBtnImg1;
	public Image SwichFuncBtnImg2;

	public GameObject InfoObj;
	public Text InfoTx;

	public GameObject[] BtnObjs;

	public GameObject InfoBtnObj;
	public GameObject UnlockBtnObj;
	public GameObject ResearchBtnObj;
	public GameObject LevelUpBtnObj;
	public GameObject trainBtnObj;

	private bool isShowFunc = true;
	private string curSceneName;
	private string curBuildingName;


	// Use this for initialization
	public override void open (IDictionary windowParams,bool playTween = false)
	{
		base.open (windowParams);

	}

	public override void close (bool playTween)
	{
		base.close(playTween);
	}

	public override void justShow ()
	{
		base.justShow ();
	}

}
