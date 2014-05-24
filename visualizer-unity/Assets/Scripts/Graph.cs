using UnityEngine;
using System.Collections;

public class Graph : MonoBehaviour {

	private int resolution = 50;	
	private ParticleSystem.Particle[] points;
	
	void Start () {
		points = new ParticleSystem.Particle[resolution];
		float increment = 1f / (resolution - 1);
		for (int i = 0; i < resolution; i++) {
			float x = i * increment;
			points[i].position = new Vector3(x, 0f, 0f);
			points[i].color = new Color(x, 0f, 0f);
			points[i].size = 0.1f;
		}

	}

	void Update () {
		particleSystem.SetParticles(points, points.Length);
	}

}
