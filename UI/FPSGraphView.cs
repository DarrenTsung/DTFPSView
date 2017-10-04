using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DTFPSView.Internal {
	public class FPSGraphView : MonoBehaviour {
		// PRAGMA MARK - Internal
		// NOTE (darren): make sure to change this on the shader side as well!
		private const int kQueueSize = 200;

		[Header("Outlets")]
		[SerializeField]
		private Material graphMaterial_;

		[SerializeField]
		private Image graphImage_;

		private float[] percentageQueue_ = new float[kQueueSize];
		private int headIndex_ = 0;

		private void Awake() {
			graphImage_.material = graphMaterial_;
		}

		private void Update() {
			float targetFramerate = Application.targetFrameRate;
			float fps = FPSView.FPS;

			float percentage = Mathf.Clamp01(fps / targetFramerate);
			percentageQueue_[headIndex_] = percentage;
			headIndex_ = (headIndex_ + 1) % percentageQueue_.Length;

			graphImage_.material.SetFloatArray("_PercentageQueue", percentageQueue_);
			graphImage_.material.SetInt("_HeadIndex", headIndex_);
		}
	}
}