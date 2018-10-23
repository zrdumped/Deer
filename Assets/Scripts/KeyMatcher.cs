using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMatcher
{
	public float missRange = 10f;
	public int matchScoreRate = 2;
	public int totalScore;
	public int maxMatchTime = 100;
	public int matchTimeStep = 2;
	public int currentMatchTime;
	public int passScore = 60;
	public int ignoreScore = 10;
	public bool active = true;
	public void ReSet() {
		totalScore = 0;
		currentMatchTime = 0;
	}

	public KeyMatchStatus TestMatch(float key, float standardKey)
	{
		bool match = (Mathf.Abs(key - standardKey) < missRange) ? true : false;
		Debug.Log(key + "," + standardKey);
		if (match)
		{
			totalScore += matchScoreRate;
		}
		currentMatchTime += matchTimeStep;
		if (currentMatchTime >= maxMatchTime) {
			if (totalScore >= passScore){
				ReSet();
				Debug.Log("SUCCESS");
				return KeyMatchStatus.SUCCESS;
			}
			else if(totalScore < ignoreScore){
				ReSet();
				Debug.Log("IGNORED");
				return KeyMatchStatus.IGNORED;
			}
			else {
				ReSet();
				Debug.Log("FAILURE");
				return KeyMatchStatus.FAILURE;
			}
		}
		Debug.Log("INPROGRESS: " + currentMatchTime);
		return KeyMatchStatus.INPROGRESS;
	}

}

public enum KeyMatchStatus { 
	SUCCESS, FAILURE, INPROGRESS, IGNORED
}