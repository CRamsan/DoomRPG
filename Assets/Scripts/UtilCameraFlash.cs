using UnityEngine;
using System.Collections;

public class CameraFlash : MonoBehaviour {
	public Color color = Color.red;
	public float startAlpha=0.0f;
	public float maxAlpha=1.0f;
	public float rampUpTime=0.5f;
	public float holdTime=0.5f;
	public float rampDownTime=0.5f;

	private Texture2D pixel;
	private enum FLASHSTATE {OFF,UP,HOLD,DOWN}
	private float timer;
	private float max_time;
	private FLASHSTATE state = FLASHSTATE.OFF;
	private bool isRunning;

	// Use this for initialization
	void Start(){
		pixel = new Texture2D(1,1);
		color.a = startAlpha;
		pixel.SetPixel(0,0,color);
		pixel.Apply();
		isRunning = false;
	}

	public void Update() {
		if (isRunning) {
			timer += Time.deltaTime;
			switch (state) {
			case FLASHSTATE.UP:
				if (timer < max_time) {
					state = FLASHSTATE.HOLD;
					timer = 0f;
					max_time = holdTime;
				}
				break;
			case FLASHSTATE.HOLD:
				if (timer < max_time) {
					state = FLASHSTATE.DOWN;
					timer = 0f;
					max_time = rampDownTime;
				}
				break;
			case FLASHSTATE.DOWN:
				if (timer < max_time) {
					state = FLASHSTATE.OFF;
					isRunning = false;
				}
				break;
			}
		}
	}

	private void SetPixelAlpha(float a){
		color.a = a;
		pixel.SetPixel(0,0,color);
		pixel.Apply();
	}

	public void OnGUI(){
		switch(state){
		case FLASHSTATE.UP:
			SetPixelAlpha(Mathf.Lerp(startAlpha,maxAlpha,Elapsed()));
			break;
		case FLASHSTATE.DOWN:
			SetPixelAlpha(Mathf.Lerp(maxAlpha,startAlpha,Elapsed()));
			break;
		}
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), pixel);
	}

	private float Elapsed() {	
		return Mathf.Clamp(timer/max_time,0,1);
	}

	public void TriggerFlash(){
		timer = 0f;
		max_time = rampUpTime;
		state = FLASHSTATE.UP;
		isRunning = true;
	}

}