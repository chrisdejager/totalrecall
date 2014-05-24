using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using AForge.Math;

public class Game : MonoBehaviour {

	UDPPacketIO udp;
	Osc handler;

	List<OscMessage> buffer;
	int windowSize = 5;

	List<Complex[]> fourierReeksen;

	void Start () {
		buffer = new List<OscMessage>();
		fourierReeksen = new List<Complex[]>();

		udp = GetComponent<UDPPacketIO>();
		udp.init("127.0.0.1", 3000, 3001);

		handler = GetComponent<Osc>();
		handler.init(udp);
		handler.SetAddressHandler("/muse/eeg/raw", ListenEvent);
	}

	void ListenEvent(OscMessage message) {
		buffer.Add(message);

		if (buffer.Count > windowSize) {
			buffer.RemoveAt(0);
		}
	}

	void Update () {
		if (buffer.Count >= windowSize) {
			StartProcessingBufferWindow();
		}
	}

	bool started = false;
	void StartProcessingBufferWindow() {
		if (started)
			return;
		started = true;

		Debug.Log("StartProcessingBufferWindow");

		int frameSize = 128;
		for (int i = 0; i < (buffer.Count - (frameSize / 2)); i += (frameSize / 2)) {

			OscMessage[] subset = buffer.GetRange(i, 128).ToArray();
//			double[] subset = ch.Skip(i).Take(128).ToArray();

			// do nothing
//			subset = EegData.Filter.Tools.applyWindow(subset);


			Complex[] complexSubset = DoubleToComplex(subset);

			Debug.Log("before: " + complexSubset[0].SquaredMagnitude);

			AForge.Math.FourierTransform.FFT(complexSubset, FourierTransform.Direction.Backward);
			fourierReeksen.Add(complexSubset);

			Debug.Log("after: " + complexSubset[0].SquaredMagnitude);
		}

		
	}

	Complex[] DoubleToComplex(OscMessage[] messages) {
		Complex[] c = new Complex[messages.Length];
		for (int i = 0; i < c.Length; i++) {
			c[i] = new Complex(System.Convert.ToDouble(messages[i].Values[0]), 0);
		}
		return c;
	}
}