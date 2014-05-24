using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Record : MonoBehaviour {

	public GameObject[] recordBalks;

	void Start () {
	
	}

	void Update () {
		if (Graph2.instance.coz > 600) {
			renderer.material.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, 1.0f));
		} else {
			renderer.material.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, 0.2f));
		}
	}

	void OnMouseDown() {

		double[][] recorded = Graph2.instance.recordedBuffer;

		double avgTheta = 0;
		double avgAlpha = 0;
		double avgBeta = 0;
		double avgGamma = 0;
		for (int i = 200; i < 400; i++) {
			avgTheta += recorded[i][0];
			avgAlpha += recorded[i][1];
			avgBeta += recorded[i][2];
			avgGamma += recorded[i][3];
		}

		avgTheta = avgTheta / 200.0;
		avgAlpha = avgAlpha / 200.0;
		avgBeta = avgBeta / 200.0;
		avgGamma = avgGamma / 200.0;

		LeanTween.moveLocal(recordBalks[0], new Vector3(recordBalks[0].transform.localPosition.x,
		                                                (float)(avgTheta * 3.0f - 0.5f),
		                                                recordBalks[0].transform.localPosition.z), 1.0f).setEase(LeanTweenType.easeOutElastic);

		LeanTween.moveLocal(recordBalks[1], new Vector3(recordBalks[1].transform.localPosition.x,
		                                                 (float)(avgAlpha * 3.0f - 0.5f),
		                                                recordBalks[1].transform.localPosition.z), 1.0f).setEase(LeanTweenType.easeOutElastic);

		LeanTween.moveLocal(recordBalks[2], new Vector3(recordBalks[2].transform.localPosition.x,
		                                                (float)(avgBeta * 3.0f - 0.5f),
		                                                recordBalks[2].transform.localPosition.z), 1.0f).setEase(LeanTweenType.easeOutElastic);

		LeanTween.moveLocal(recordBalks[3], new Vector3(recordBalks[3].transform.localPosition.x,
		                                                (float)(avgGamma * 3.0f - 0.5f),
		                                                recordBalks[3].transform.localPosition.z), 1.0f).setEase(LeanTweenType.easeOutElastic);



	}
}
