using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class HelperUtils
{
	public static long hlpGetMilliseconds()
	{
		return (long)((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000);
	}

	static public float hlpObjectToFloat( this object str )
	{
		float result = 0;
		float.TryParse(str.ToString(), out result);
		return result;
	}

	static public int hlpObjectToInt( this object str )
	{
		int result = 0;
		int.TryParse(str.ToString(), out result);
		return result;
	}

	static public long hlpObjectToLong( this object lng )
	{
		long result = 0;
		long.TryParse(lng.ToString(), out result);
		return result;
	}

	static public void hlpUILayoutGroupEnable(GameObject go, bool isEnable )
	{
		UnityEngine.UI.LayoutGroup[] layoutGroups = go.GetComponents<UnityEngine.UI.LayoutGroup>();
		for( int i = 0; i < layoutGroups.Length; ++i )
		{
			layoutGroups[i].enabled = isEnable;
		}
	}

	public static string hlpGetCountDownTimeOriginalStringMS(int totalSeconds){
		string timeFormat = "";
		int leftSeconds = totalSeconds % 60;
		int minutes = totalSeconds / 60;

		string result = null;
		timeFormat = "{0}:{1}";
		result = string.Format(timeFormat, HelperUtils.hlpFillZero(1, minutes),HelperUtils.hlpFillZero(2, leftSeconds));
		return result;
	}

	public static string hlpGetCountDownTimeStringDHMS(int totalSeconds,bool useOriginal = false){

		string timeFormat = "";
		int leftSeconds = totalSeconds % 60;
		int minutes = totalSeconds / 60;
		int leftHMinutes = minutes % 60;
		int hours = minutes / 60;
		int leftDHours = hours %24;
		int days = hours / 24;

		string result = null;
		if(days >= 1){

			if( leftDHours > 0 )
			{
				if( useOriginal )
				{
					timeFormat = "{0}:{1}";
				}
				else{
					timeFormat = Localization.Get("ui_time_mod_format_dh");
				}
				result = string.Format(timeFormat, HelperUtils.hlpFillZero(1, days),HelperUtils.hlpFillZero(1, leftDHours));
			}
			else
			{
				if( useOriginal )
				{
					timeFormat = "{0}";
				}
				else{
					timeFormat = Localization.Get("ui_time_mod_format_d");
				}
				result = string.Format(timeFormat, HelperUtils.hlpFillZero(1, days));
			}
		}
		else if(hours >= 1){
			if( leftHMinutes > 0 )
			{
				if( useOriginal )
				{
					timeFormat = "{0}:{1}";
				}
				else{
					timeFormat = Localization.Get("ui_time_mod_format_hm");
				}
				result = string.Format(timeFormat, HelperUtils.hlpFillZero(1, hours),HelperUtils.hlpFillZero(1, leftHMinutes));
			}
			else
			{
				if( useOriginal )
				{
					timeFormat = "{0}";
				}
				else{
					timeFormat = Localization.Get("ui_time_mod_format_h");
				}
				result = string.Format(timeFormat, HelperUtils.hlpFillZero(1, hours),HelperUtils.hlpFillZero(1, leftHMinutes));
			}
		}
		else if(minutes >= 1){
			if( leftSeconds >= 0 )
			{
				if( useOriginal )
				{
					timeFormat = "{0}:{1}";
				}
				else{
					timeFormat = Localization.Get("ui_time_mod_format_ms");
				}
				result = string.Format(timeFormat, HelperUtils.hlpFillZero(1, minutes),HelperUtils.hlpFillZero(2, leftSeconds));
			}
			else
			{
				if( useOriginal )
				{
					timeFormat = "{0}";
				}
				else{
					timeFormat = Localization.Get("ui_time_mod_format_m");
				}
				result = string.Format(timeFormat, HelperUtils.hlpFillZero(1, minutes));
			}
		}
		else {
			if( useOriginal )
			{
				timeFormat = "{0}";
			}
			else{
				timeFormat = Localization.Get("ui_time_mod_format_s");
			}
			result = string.Format(timeFormat, HelperUtils.hlpFillZero(1, leftSeconds));
		}
		return result;
	}

	public static string hlpFillZero(int digit, int number){
		return number.ToString ().PadLeft( digit, '0' );
	}

	public static void SetBtnGrayState( UnityEngine.UI.Image img, bool isSet, Material uiGrayMat){
		if(isSet){
			img.material = uiGrayMat;
			img.material.SetFloat("GrayScale Amount", 1f);
		}else{
			img.material = null;
		}
	}
}



