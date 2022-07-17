using System;
using System.Collections.Generic;
using SimpleGif.Data;
using UnityEngine;

namespace Assets.GifAssets.PowerGif
{
	/// <summary>
	/// Main class for working with GIF format. It is a wrapper over SimpleGif.Gif.
	/// PLEASE REFER TO EXAMPLES FIRST!
	/// </summary>
	public class Gif
	{
		/// <summary>
		/// List of GIF frames.
		/// </summary>
		public List<GifFrame> Frames;

		/// <summary>
		/// Create a new instance from GIF frames.
		/// </summary>
		public Gif(List<GifFrame> frames)
		{
			Frames = frames;
		}

		/// <summary>
		/// Quantize images colors.
		/// </summary>
		public Gif QuantizeColors()
        {
            Frames = Converter.ConvertFrames(new SimpleGif.Gif(Converter.ConvertFrames(Frames)).QuantizeColors().Frames);

            return this;
        }

		/// <summary>
		/// Decode byte array and return a new instance.
		/// </summary>
		public static Gif Decode(byte[] bytes, FilterMode filterMode = FilterMode.Point)
		{
			var frames = Converter.ConvertFrames(SimpleGif.Gif.Decode(bytes).Frames);

			return new Gif(frames);
		}

		/// <summary>
		/// Decode byte array and return frames as iterator.
		/// </summary>
		public static IEnumerable<GifFrame> DecodeIterator(byte[] bytes, FilterMode filterMode = FilterMode.Point)
		{
			var iterator = SimpleGif.Gif.DecodeIterator(bytes);

			foreach (var frame in iterator)
			{
				var texture = Converter.ConvertTexture(frame.Texture);

				yield return new GifFrame(texture, frame.Delay);
			}
		}

		/// <summary>
		/// Get frame count. Can be used with DecodeIterator to display a progress bar.
		/// </summary>
		public static int GetDecodeIteratorSize(byte[] bytes)
		{
			return SimpleGif.Gif.GetDecodeIteratorSize(bytes);
		}

		/// <summary>
		/// Encode to byte array.
		/// </summary>
		public byte[] Encode(int scale = 1)
		{
			var frames = Converter.ConvertFrames(Frames);

			return new SimpleGif.Gif(frames).RemoveTranslucency().QuantizeColors().Encode(scale);
		}

        /// <summary>
		/// Encode to byte array and return it by parts with an iterator.
		/// </summary>
		public IEnumerable<List<byte>> EncodeIterator()
		{
			var frames = Converter.ConvertFrames(Frames);
			var iterator = new SimpleGif.Gif(frames).RemoveTranslucency().QuantizeColors().EncodeIterator();

			foreach (var part in iterator)
			{
				yield return part;
			}
		}

		/// <summary>
		/// Get parts count for EncodeIterator. Can be used with EncodeIterator to display a progress bar.
		/// First part is a first frame, penultimate part is GIF header, last part is ending. Thus it always return frame number plus 2.
		/// </summary>
		public int GetEncodeIteratorSize()
		{
			return Frames.Count + 2;
		}

		/// <summary>
		/// Parallel encoding.
		/// </summary>
		public void EncodeParallel(Action<EncodeProgress> onProgress, int scale = 1)
		{
			Converter.Convert(this).RemoveTranslucency().QuantizeColors().EncodeParallel(onProgress, scale); // Threads are started here.
		}
	}
}