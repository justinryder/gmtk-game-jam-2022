using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace Assets.GifAssets.PowerGif.Examples.Scripts
{
	/// <summary>
	/// This example shows how to check encoding/decoding operations performance.
	/// </summary>
	public class PerformanceExample : MonoBehaviour
	{
		public AnimatedImage AnimatedImage;
		private readonly Stopwatch _stopwatch = new Stopwatch();

		public void Start()
		{
			var path = "Assets/GifAssets/PowerGif/Examples/Samples/Small.gif";

			if (path == "") return;

			var bytes = File.ReadAllBytes(path);

			_stopwatch.Reset();
			_stopwatch.Start();

			var gif = Gif.Decode(bytes);

			UnityEngine.Debug.LogFormat("Decoded in {0:N2}s", _stopwatch.Elapsed.TotalSeconds);

			_stopwatch.Reset();
			_stopwatch.Start();

			gif.Encode();

			UnityEngine.Debug.LogFormat("Encoded in {0:N2}s", _stopwatch.Elapsed.TotalSeconds);

			AnimatedImage.Play(gif);
		}

		public void Review()
		{
			Application.OpenURL("https://www.assetstore.unity3d.com/#!/content/121731");
		}
	}
}