using UnityEngine;
using System.Collections;
using Vectrosity;

public class Graph2 : MonoBehaviour {

	public static Graph2 instance;
	public Material lineMaterial;

	double[][] values;
	VectorLine[] lines;

	public GameObject[] cubes;

	void Start () {
		instance = this;

		int length = Screen.width;

		values = new double[5][];
		values[0] = new double[length];
		values[1] = new double[length];
		values[2] = new double[length];
		values[3] = new double[length];

		lines = new VectorLine[4];
	}

	bool shouldRemoveLines = false;
	public void UpdateLines(double delta, double alpha, double theta, double gamma, double beta) {

		DrawLineForIndex(0, theta, Color.red);
		DrawLineForIndex(1, alpha, Color.green);
		DrawLineForIndex(2, beta, Color.blue);
		DrawLineForIndex(3, gamma, Color.black);

		shouldRemoveLines = true;
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
		bar.gameObject.transform.localScale = new Vector3(1.0f, (float)(newValue / 500.0f), 1.0f);
		bar.gameObject.transform.localPosition = new Vector3(bar.gameObject.transform.localPosition.x, 
		                                                     bar.gameObject.transform.localScale.y / 2.0f - 0.5f, 
		                                                     bar.gameObject.transform.localPosition.z);
	}
}
