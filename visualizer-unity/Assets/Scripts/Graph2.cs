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

		int length = Screen.width;

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
	}

	bool shouldRemoveLines = false;
	public int coz = 0;
	public void UpdateLines(double delta, double alpha, double theta, double gamma, double beta) {

		DrawLineForIndex(0, theta, Color.red);
		DrawLineForIndex(1, alpha, Color.green);
		DrawLineForIndex(2, beta, Color.blue);
		DrawLineForIndex(3, gamma, Color.black);

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

	public void DrawLineForIndex(int index, double newValue, Color color) {
		// shift everything down
		double[] v = values[index];
		for (int i = 0; i < v.Length-1; i++) {
			v[i] = v[i+1];
		}
		v[v.Length-1] = newValue;

		values[index] = v;

		Vector2[] deltaPoints = new Vector2[v.Length];
		for (int i = 0; i < v.Length; i++) {
			deltaPoints[i] = new Vector2(i, (float)v[i]);
		}

		if (shouldRemoveLines)
			VectorLine.Destroy(ref lines[index]);
		
//		VectorLine deltaLine = new VectorLine("MyLine", deltaPoints, color, lineMaterial , 2.0f, LineType.Continuous, Joins.Fill);
		VectorLine deltaLine = VectorLine.SetLine(color, deltaPoints);

		deltaLine.joins = Joins.Fill;
		deltaLine.Draw();
		lines[index] = deltaLine;

		GameObject bar = cubes[index];
		bar.gameObject.transform.localScale = new Vector3(1.0f, (float)(newValue * 3), 1.0f);
		bar.gameObject.transform.localPosition = new Vector3(bar.gameObject.transform.localPosition.x, 
		                                                     bar.gameObject.transform.localScale.y / 2.0f - 0.5f, 
		                                                     bar.gameObject.transform.localPosition.z);
	}
}
