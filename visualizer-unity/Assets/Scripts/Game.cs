using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using AForge.Math;

public class Game : MonoBehaviour {

	UDPPacketIO udp;
	Osc handler;

	List<OscMessage> buffer;
	int windowSize = 512;

	List<Complex[]> fourierReeksen;
	double delta;
	double alpha;
	double theta;
	double gamma;
	double beta;

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
		} else {
			Debug.Log(buffer.Count);
		}
	}

	bool started = false;
	void StartProcessingBufferWindow() {
		if (started)
			return;
		started = true;

		List<OscMessage> bufferCopy = new List<OscMessage>(buffer);

		int frameSize = 128;
		for (int i = 0; i < (bufferCopy.Count - (frameSize / 2)); i += (frameSize / 2)) {
			
			OscMessage[] subset = bufferCopy.GetRange(i, 128).ToArray();
//			double[] subset = ch.Skip(i).Take(128).ToArray();

			// do nothing
//			subset = EegData.Filter.Tools.applyWindow(subset);


			Complex[] complexSubset = DoubleToComplex(subset);

			AForge.Math.FourierTransform.FFT(complexSubset, FourierTransform.Direction.Backward);
			fourierReeksen.Add(complexSubset);
		}

		Complex[] outputTransform = new Complex[frameSize];    
		for (int i = 0; i < frameSize; i++)
		{
			outputTransform[i] = new Complex(0, 0);
			for (int r = 0; r < fourierReeksen.Count-1; r++)
			{
				outputTransform[i] = Complex.Add(fourierReeksen[r][i], outputTransform[i]);
			}
			outputTransform[i] = Complex.Divide(outputTransform[i], fourierReeksen.Count);
		}

		double[] power = GetPowerSpectrum(outputTransform);
		double[] freqv = GetFrequencyVector(frameSize, 5000);

		delta = dForPowerRange(1, 3, power, freqv);
		theta = dForPowerRange(4, 7, power, freqv);
		alpha = dForPowerRange(8, 12, power, freqv);
		beta = dForPowerRange(13, 30, power, freqv);
		gamma = dForPowerRange(31, 50, power, freqv);

		Graph2.instance.UpdateLines(delta, alpha, theta, gamma, beta);

		fourierReeksen.Clear();
		started = false;
	}

	void OnGUI() {
		GUI.Label(new Rect(0,0,200, 50), "theta " + theta);
		GUI.Label(new Rect(0,30,200, 50), "alpha " + alpha);
		GUI.Label(new Rect(0,60,200, 50), "beta " + beta);
		GUI.Label(new Rect(0,90,200, 50), "gamma " + gamma);
	}

	public double dForPowerRange(int start, int end, double[] power, double[] freqv) {

		double v = 0;
		int i = 0;

		while (freqv[i] < start) {
			//skip
			i++;
		}

		while (freqv[i] < end) {
			v += power[i];
			i++;
		}

		return v / System.Convert.ToDouble((end-start)+1);
	}

	public static double[] GetPowerSpectrum(Complex[] fft)
	{
		int n = (int)System.Math.Ceiling((fft.Length + 1) / 2.0);
		
		double[] mx = new double[n];
		mx[0] = fft[0].SquaredMagnitude / fft.Length;
		
		for (int i = 1; i < n; i++)
		{
			mx[i] = fft[i].SquaredMagnitude * 2.0 / fft.Length;
		}
		
		return mx;
	}

	public static double[] GetFrequencyVector(int length, int sampleRate)
	{
		int numUniquePts = (int)System.Math.Ceiling((length + 1) / 2.0);
		double[] freq = new double[numUniquePts];
		for (int i = 0; i < numUniquePts; i++)
		{
			freq[i] = (double)i * sampleRate / length;
		}
		return freq;
	}

	Complex[] DoubleToComplex(OscMessage[] messages) {
		Complex[] c = new Complex[messages.Length];
		for (int i = 0; i < c.Length; i++) {
			c[i] = new Complex(System.Convert.ToDouble(messages[i].Values[0]), 0);
		}
		return c;
	}
}