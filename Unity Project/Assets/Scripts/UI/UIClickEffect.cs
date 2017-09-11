using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIClickEffect : ObjectCache {

	public ParticleSystem ps;
	public override void objEnable ()
	{
		base.objEnable ();
		mTrans.localScale = Vector3.one;
		ps.Play();
		Invoke("ParticleFinish",ps.duration);
	}

	public void ParticleFinish(){
		ObjectCachePool.instance.Store(this);
	}

	public override void objDisable (Transform poolroot)
	{
		base.objDisable (poolroot);
	}
}
