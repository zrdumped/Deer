using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMatcher
{
	public float missRange = 100;
	public int matchScoreRate = 2;
	public int totalScore;
	public int maxMatchTime = 100;
	public int matchTimeStep = 2;
	public int currentMatchTime;
	public int passScore = 120;
	public int ignoreScore;
	public bool active = true;
	public void ReSet() {
		totalScore = 0;
		ignoreScore = 0;
		currentMatchTime = 0;
	}

	public KeyMatchStatus TestMatch(float key, float standardKey)
	{
		if (!active)
			return KeyMatchStatus.INPROGRESS;
		if (key == 0){
			ignoreScore += matchScoreRate;
		}
		else{
			bool match = (Mathf.Abs(key - standardKey) < missRange) ? true : false;
			//Debug.Log(key + "," + standardKey);
			if (match)
			{
				totalScore += matchScoreRate;
			}
		}
		currentMatchTime += matchTimeStep;
		if (currentMatchTime >= maxMatchTime) {
			if (totalScore >= passScore){

				Debug.Log("SUCCESS" + totalScore);
				ReSet();
				return KeyMatchStatus.SUCCESS;
			}
			else if(ignoreScore >= passScore){

				ReSet();
				return KeyMatchStatus.IGNORED;
			}
			else {
				ReSet();
				return KeyMatchStatus.FAILURE;
			}
		}
		return KeyMatchStatus.INPROGRESS;
	}

}

public enum KeyMatchStatus { 
	SUCCESS, FAILURE, INPROGRESS, IGNORED
}