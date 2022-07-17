using System.IO;
using UnityEngine;

namespace Assets.GifAssets.PowerGif.Examples.Scripts
{
	/// <summary>
	/// Decoding GIF example.
	/// </summary>
	public class DecodeExample : MonoBehaviour
	{
		public AnimatedImage AnimatedImage;

		public void Start()
		{
			var path = "Assets/GifAssets/PowerGif/Examples/Samples/Large.gif";

			if (path == "") return;

			var bytes = File.ReadAllBytes(path);
			var gif = Gif.Decode(bytes);

			AnimatedImage.Play(gif);
		}

		public void Review()
		{
			Application.OpenURL("https://www.assetstore.unity3d.com/#!/content/121731");
		}
	}
}