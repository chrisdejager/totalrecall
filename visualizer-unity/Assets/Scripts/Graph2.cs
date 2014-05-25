using UnityEngine;
using System.Collections;
using Vectrosity;

public class Graph2 : MonoBehaviour {

	public static Graph2 instance;
	public Material lineMaterial;

	double[][] values;
	VectorLine[] lines;

	public GameObject[] cubes;

	public double[][] recordedBuffer;

	void Start () {
		instance = this;

		int length = 1200;

		values = new double[5][];
		values[0] = new double[length];
		values[1] = new double[length];
		values[2] = new double[length];
		values[3] = new double[length];

		lines = new VectorLine[4];

		recordedBuffer = new double[600][];
//		for (int i = 0; i < recordedBuffer.Length; i++) {
//			recordedBuffer[i] = new double[4] { 0, 0.0, beta, gamma };
//		}

//		gparent = GameObject.FindGameObjectWithTag("graphPArent");

		Color baseLineColor = new Color(0.5f, 0.5f, 0.5f);

		VectorLine l = VectorLine.SetLine(baseLineColor, new Vector3[]{ 
			new Vector3(- 8.0f, - 3.5f, 0), 
			new Vector3(-1.0f, - 3.5f, 0) });
		l.Draw3D();

		VectorLine l2 = VectorLine.SetLine(baseLineColor, new Vector3[]{ 
			new Vector3(8.0f, - 3.5f, 0), 
			new Vector3(1.0f, - 3.5f, 0) });
		l2.Draw3D();
	}

	bool shouldRemoveLines = false;
	public int coz = 0;
	public void UpdateLines(double delta, double alpha, double theta, double gamma, double beta) {

		DrawLineForIndex(0, theta, new Color(242/255.0f, 24/255.0f, 225/255.0f));
		DrawLineForIndex(1, alpha, new Color(50/255.0f, 199/255.0f, 109/255.0f));
		DrawLineForIndex(2, beta, new Color(24/255.0f, 242/255.0f, 192/255.0f));
		DrawLineForIndex(3, gamma, new Color(36/255.0f, 89/255.0f, 77/255.0f));

		shouldRemoveLines = true;

//		double[] v = recordedBuffer[index];
		for (int i = 0; i < recordedBuffer.Length-1; i++) {
			recordedBuffer[i] = recordedBuffer[i+1];
		}
		recordedBuffer[recordedBuffer.Length-1] = new double[4] { theta, alpha, beta, gamma };

		coz += 1;
	}

	void OnGUI() {
		GUI.Label(new Rect(0,0,100,20), coz+"");
	}

	bool keepvalueslevel = false;
	public void JumpToValuesForShortPeriod(double theta, double alpha, double beta, double gamma) {
		keepvalueslevel = true;

		double[] z = new double[4] { theta, alpha, beta, gamma };

		for (int i = 0; i < 4; i++) {
			GameObject bar = cubes[i];



			Vector3 scale = new Vector3(1.0f, (float)(z[i] * 3), 1.0f);
			Vector3 pos = new Vector3(bar.gameObject.transform.localPosition.x, 
			                                                     (float)(z[i] * 3) / 2.0f - 0.5f, 
			                                                     bar.gameObject.transform.localPosition.z);

			LeanTween.scale(bar, scale, 1.0f).setEase(LeanTweenType.easeOutElastic);
			LeanTween.moveLocal(bar, pos, 1.0f).setEase(LeanTweenType.easeOutElastic);
		}

		LeanTween.delayedCall(4.0f, () => {
			keepvalueslevel = false;
		});
	}

//	GameObject gparent;
	public void DrawLineForIndex(int index, double newValue, Color color) {


		// shift everything down
		double[] v = values[index];
		for (int i = 0; i < v.Length-1; i++) {
			v[i] = v[i+1];
		}
		v[v.Length-1] = newValue;

		values[index] = v;

		Vector3[] deltaPoints = new Vector3[v.Length];

//		float lastX = -8.0f;
		for (int i = 0; i < v.Length; i++) {
//			deltaPoints[i] = new Vector3(i/30.0f - 8.0f, (float)v[i] * 8.0f, 0);
			deltaPoints[i] = new Vector3(i/60.0f - 10.0f, (float)v[i] * 10.0f - 3.5f, 0);
		}

		if (shouldRemoveLines)
			VectorLine.Destroy(ref lines[index]);
		
//		VectorLine deltaLine = new VectorLine("MyLine", deltaPoints, color, lineMaterial , 2.0f, LineType.Continuous, Joins.Fill);
		VectorLine deltaLine = VectorLine.SetLine(color, deltaPoints);

		deltaLine.joins = Joins.Fill;
		deltaLine.Draw3D();
		lines[index] = deltaLine;

//		deltaLine.vectorObject.transform.parent = gparent.transform;

		if (!keepvalueslevel) {
			GameObject bar = cubes[index];
			bar.gameObject.transform.localScale = new Vector3(1.0f, (float)(newValue * 3), 1.0f);
			bar.gameObject.transform.localPosition = new Vector3(bar.gameObject.transform.localPosition.x, 
			                                                     bar.gameObject.transform.localScale.y / 2.0f - 0.5f, 
			                                                     bar.gameObject.transform.localPosition.z);
		}


	}
}
