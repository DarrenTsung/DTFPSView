using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using DTFPSView.Internal;

namespace DTFPSView {
	public class FPSView : MonoBehaviour {
		// PRAGMA MARK - Public Interface
		public static event Action OnFPSViewEnabledChanged = delegate {};
		public static bool Enabled {
			get { return enabled_; }
			set {
				#if !DEBUG
				if (!Debug.isDebugBuild) return;
				#endif

				if (enabled_ == value) return;

				enabled_ = value;
				OnFPSViewEnabledChanged.Invoke();
			}
		}

		public static float FPS {
			get; private set;
		}

		private static bool enabled_ = false;

		[RuntimeInitializeOnLoadMethod]
		private static void Initialize() {
			if (Application.isEditor) {
				FPSView.Enabled = true;
			}
		}


		// PRAGMA MARK - Internal
		private const float kUpdateInterval = 0.1f;

		[Header("Outlets")]
		[SerializeField]
		private GameObject viewPrefab_;

		private float accumulatedFPS_ = 0; // FPS accumulated over the interval
		private int frames_ = 0; // Frames drawn over the interval
		private float timeleft_; // Left time for current interval

		private void Awake() {
			GameObject.Instantiate(viewPrefab_, parent: this.transform);

			HandleFPSViewEnabledChanged();
			OnFPSViewEnabledChanged += HandleFPSViewEnabledChanged;
		}

		private void OnDestroy() {
			OnFPSViewEnabledChanged -= HandleFPSViewEnabledChanged;
		}

		#if UNITY_EDITOR
		private void Reset() {
			viewPrefab_ = AssetDatabaseUtil.LoadSpecificAssetNamed<GameObject>("FPSViewContainer");
		}
		#endif

		private void HandleFPSViewEnabledChanged() {
			this.gameObject.SetActive(Enabled);
		}

		private void Update() {
			timeleft_ -= Time.deltaTime;
			accumulatedFPS_ += Time.timeScale / Time.deltaTime;
			frames_++;

			// Interval ended - update GUI text and start new interval
			if (timeleft_ <= 0.0) {
				FPS = accumulatedFPS_ / frames_;

				timeleft_ = kUpdateInterval;
				accumulatedFPS_ = 0.0f;
				frames_ = 0;
			}
		}
	}
}