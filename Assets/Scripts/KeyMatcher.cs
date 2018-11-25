using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMatcher : MonoBehaviour {
	public float missRange = 100;
	public int matchScoreRate = 2;
	public int currentMatchTime;
	public int passScore = 120;
    public enum KeyMatchStatus { SUCCESS, FAILURE, IGNORED};
    private Character character;
    //public bool active = true;

    private float standardKey;
    private int totalScore;
    private int ignoreScore;

    public void Start()
    {
        character = GameObject.Find("Character").GetComponent<Character>();
    }

    public void OnEnable()
    {
        standardKey = this.transform.lossyScale.x;
        totalScore = 0;
        ignoreScore = 0;
    }

    public void Update()
    {
        float key = character.getLightBallLossyScale();
        if (key == 0)
        {
            ignoreScore += matchScoreRate;
        }
        else
        {
            bool match = (Mathf.Abs(key - standardKey) * 100 < missRange) ? true : false;
            Debug.Log(key + "," + standardKey + "," + match);
            if (match)
            {
                totalScore += matchScoreRate;
                Debug.Log(totalScore);
            }
        }
    }

    public KeyMatchStatus TestMatch()
    {
        if (totalScore >= passScore)
        {
            Debug.Log("SUCCESS" + totalScore);
            return KeyMatchStatus.SUCCESS;
        }
        else if (ignoreScore >= passScore)
        {
            return KeyMatchStatus.IGNORED;
        }
        else
        {
            return KeyMatchStatus.FAILURE;
        }
    }
}