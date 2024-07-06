using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class ImageSaver : MonoBehaviour
{

	[SerializeField] int imageSize = 28;
	[SerializeField] bool greyscale = true;
	//[SerializeField] DataFile[] dataFiles;
	[SerializeField] List<string> labelNames;
	[SerializeField] string datasetFileName;
	string currentLabel;
	public TMPro.TMP_Text currentLabelUI;
	List<Image> allImages;
	Image[] images;

	DrawingController drawingController;

	//public int NumImages => images.Length;
	//public int InputSize => imageSize * imageSize * (greyscale ? 1 : 3);
	//public int OutputSize => labelNames.Length;
	//public string[] LabelNames => labelNames;

	void Awake()
	{
		//images = LoadImages();
		drawingController = FindObjectOfType<DrawingController>();
		allImages = new List<Image>();
		currentLabel = labelNames[0];
		currentLabelUI.text = currentLabel;
	}

	int Mod(int x, int m)
	{
		int r = x % m;
		return r < 0 ? r + m : r;
	}

	private void Update()
    {
		if (Input.GetKeyDown(KeyCode.S))
		{
			RenderTexture digitRenderTexture = drawingController.RenderOutputTexture();
			Image image = ImageHelper.TextureToImage(digitRenderTexture, labelNames.IndexOf(currentLabel));
			CacheImage(image);
			print("Saving... " + currentLabel);
		}
	
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			currentLabel = labelNames[Mod(labelNames.IndexOf(currentLabel) + 1, labelNames.Count)];
			currentLabelUI.text = currentLabel;
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			currentLabel = labelNames[Mod(labelNames.IndexOf(currentLabel) - 1, labelNames.Count)];
			currentLabelUI.text = currentLabel;
		}
	}

    //public Image GetImage(int i)
    //{
    //	return images[i];
    //}

    //public DataPoint[] GetAllData()
    //{
    //	DataPoint[] allData = new DataPoint[images.Length];
    //	for (int i = 0; i < allData.Length; i++)
    //	{
    //		allData[i] = DataFromImage(images[i]);
    //	}
    //	return allData;
    //}

    //DataPoint DataFromImage(Image image)
    //{
    //	return new DataPoint(image.pixelValues, image.label, OutputSize);
    //}

    public void CacheImage(Image image)
    {
		//if (!System.IO.File.Exists("Assets//Data//" + "Custom//" + "Data//" + datasetFileName + ".bytes")) {

		//}
		allImages.Add(image);
	}

	public void Save()
    {
		string imageFile = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Assets//Data//Custom", "Images " + datasetFileName + ".bytes");
		string labelFile = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Assets//Data//Custom", "Labels " + datasetFileName + ".bytes");
		try
		{
			using (var fs = new FileStream(imageFile, FileMode.Create, FileAccess.Write))
			{
				List<byte> byteArray = new List<byte>();
				foreach (Image el in allImages)
                {
					byteArray.AddRange(el.GetPixelValuesToBytes());
                }
				fs.Write(byteArray.ToArray(), 0, byteArray.Count);
			}
			using (var fs = new FileStream(labelFile, FileMode.Create, FileAccess.Write))
			{
				List<byte> byteArray = new List<byte>();
				foreach (Image el in allImages)
				{
					print(el.label);
					byteArray.Add(el.GetLabelValueToBytes());
				}
				fs.Write(byteArray.ToArray(), 0, byteArray.Count);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("Exception caught in process: {0}", ex);
			return;
		}
		//NetworkSaveData.SaveToFile(neuralNetwork, path);
		//Debug.Log("Saved network to: " + path);
	}

	//Image[] LoadImages()
	//{
	//	List<Image> allImages = new List<Image>();

	//	foreach (var file in dataFiles)
	//	{
	//		Image[] images = LoadImages(file.imageFile.bytes, file.labelFile.bytes);
	//		allImages.AddRange(images);
	//	}

	//	return allImages.ToArray();


	//	Image[] LoadImages(byte[] imageData, byte[] labelData)
	//	{
	//		int numChannels = (greyscale) ? 1 : 3;
	//		int bytesPerImage = imageSize * imageSize * numChannels;
	//		int bytesPerLabel = 1;

	//		int numImages = imageData.Length / bytesPerImage;
	//		int numLabels = labelData.Length / bytesPerLabel;
	//		Debug.Assert(numImages == numLabels, $"Number of images doesn't match number of labels ({numImages} / {numLabels})");

	//		int dataSetSize = System.Math.Min(numImages, numLabels);
	//		var images = new Image[dataSetSize];

	//		// Scale pixel values from [0, 255] to [0, 1]
	//		double pixelRangeScale = 1 / 255.0;
	//		double[] allPixelValues = new double[imageData.Length];

	//		System.Threading.Tasks.Parallel.For(0, imageData.Length, (i) =>
	//		{
	//			allPixelValues[i] = imageData[i] * pixelRangeScale;
	//		});

	//		// Create images
	//		System.Threading.Tasks.Parallel.For(0, numImages, (imageIndex) =>
	//		{
	//			int byteOffset = imageIndex * bytesPerImage;
	//			double[] pixelValues = new double[bytesPerImage];
	//			System.Array.Copy(allPixelValues, byteOffset, pixelValues, 0, bytesPerImage);
	//			Image image = new Image(imageSize, greyscale, pixelValues, labelData[imageIndex]);
	//			images[imageIndex] = image;
	//		});

	//		return images;
	//	}


	//}

	[System.Serializable]
	public struct DataFile
	{
		public TextAsset imageFile;
		public TextAsset labelFile;
	}

}
