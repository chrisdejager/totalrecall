using UnityEngine;
using System.Collections;
using Vectrosity;

public class Graph3 : MonoBehaviour {
	
	public static Graph3 instance;
	public Material lineMaterial;
	
	double[] values;
	VectorLine line;

	public double minValue;
	public double maxValue;

	void Start () {
		instance = this;
		
		minValue = 750.0;
		maxValue = 890.0;

		int length = 1400;
		values = new double[length];
	}
	
	bool shouldRemoveLines = false;
	public void UpdateLine(double signal) {

		if (values.Length > 1) {
			minValue = 750.0;
			maxValue = 890.0;
			for (int i=0; i < values.Length; i++) {
				minValue = System.Math.Min(minValue, values[i]);
				maxValue = System.Math.Max(maxValue, values[i]);
			}
		}

		DrawLineForIndex(signal, new Color(191/255.0f, 191/255.0f, 191/255.0f));

		shouldRemoveLines = true;
		for (int i = 0; i < values.Length-1; i++) {
			values[i] = values[i+1];
		}
		values[values.Length-1] = signal;
	}
	
	//	GameObject gparent;
	public void DrawLineForIndex(double newValue, Color color) {
		
		// shift everything down
		double[] v = values;
		for (int i = 0; i < v.Length-1; i++) {
			v[i] = v[i+1];
		}
		v[v.Length-1] = newValue;
		
		values = v;
		
		Vector3[] deltaPoints = new Vector3[v.Length];
		
		//		float lastX = -8.0f;
		for (int i = 0; i < v.Length; i++) {
			//			deltaPoints[i] = new Vector3(i/30.0f - 8.0f, (float)v[i] * 8.0f, 0);
			deltaPoints[i] = new Vector3(i/60.0f - 12.0f, (float)((v[i] - minValue) / (maxValue - minValue)) * 8.0f - 3.5f, 0);
		}
		
		if (shouldRemoveLines)
			VectorLine.Destroy(ref line);
		
		//		VectorLine deltaLine = new VectorLine("MyLine", deltaPoints, color, lineMaterial , 2.0f, LineType.Continuous, Joins.Fill);
		VectorLine deltaLine = VectorLine.SetLine(color, deltaPoints);
		
		deltaLine.joins = Joins.Fill;
		deltaLine.Draw3D();
		line = deltaLine;
		
		//		deltaLine.vectorObject.transform.parent = gparent.transform;
	}
}
