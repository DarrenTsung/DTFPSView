using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DTFPSView.Internal {
	public class FPSTextView : MonoBehaviour {
		// PRAGMA MARK - Internal
		[Header("Outlets")]
		[SerializeField]
		private Text fpsText_;

		private void Update() {
			float targetFramerate = Application.targetFrameRate;
			float fps = FPSView.FPS;

			// display two fractional digits (f2 format)
			fpsText_.text = string.Format("{0:F2} FPS", fps);

			if (fps < targetFramerate / 2.0f) {
				fpsText_.color = Color.yellow;
			} else if (fps < targetFramerate / 6.0f) {
				fpsText_.color = Color.red;
			} else {
				fpsText_.color = Color.green;
			}
		}
	}
}