using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Playtime : MonoBehaviour {

	public Text timerText;

	private float secondsCount;
	private int minuteCount;
	private int hourCount;

	void Update(){
		UpdateTimerUI();
	}
	//call this on update
	public void UpdateTimerUI(){
		//set timer UI
		secondsCount += Time.deltaTime;
		timerText.text = hourCount +":"+ minuteCount +":"+(int)secondsCount;
		if(secondsCount >= 60){
			minuteCount++;
			secondsCount = 0;
		}else if(minuteCount >= 60){
			hourCount++;
			minuteCount = 0;
		}    
	}
}
