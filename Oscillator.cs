using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {
    [SerializeField] Vector3 mV = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;
    float movementFactor;
    Vector3 startPos;

	void Start () {
        startPos = transform.position;
	}
	
	void Update () {
        if (period <= Mathf.Epsilon) { return; } //Prevent game from flipping if period is 0
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2; //About 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);
        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = mV * movementFactor;
        transform.position = startPos + offset;
	}
}
