using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingWindow : Window {

	public Image BackGroundImage;
	public Image ProgressFill;
	public Text ProgressText;

	public const string initBaseText = "正在初始化游戏...";
	public const string updateAssetsText = "正在更新资源...";
	public const string loadAssetsText = "正在加载资源...";

	public override void open (IDictionary windowParams, bool playTween)
	{
		base.open (windowParams,playTween);
		StartCoroutine(UpdateProgress());
	}

	public virtual IEnumerator UpdateProgress()
	{
		while (true)
		{
			yield return new WaitForEndOfFrame();
		}
	}

	public virtual void OnLoadingDone()
	{
		ProgressFill.fillAmount = 1;
		WUIManager.UIMCloseLoading();
	}

	// Update is called once per frame
	public override void close (bool playTween)
	{
		base.close(playTween);
	}
}
