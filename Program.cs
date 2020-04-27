using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;


namespace Google
{

public class Program
{
	public static void Main()
	{
		String iParam0 = @"r34343";
		String imagePath = @"C:\Users\pc\Desktop\1.png";
		String imageUrl = null;
		decimal iParam3 = 30;
		String oParam0 = "";
		int oParam1 = 0;
		LabelDetection(iParam0, imagePath, imageUrl, iParam3, ref oParam0, ref oParam1, provideImage: 1);
		Console.WriteLine("oParam0");
	}

	private static JObject GetImageContentForCloudVision(string filePath)
	{
		return new JObject(new JProperty("content", Convert.ToBase64String(File.ReadAllBytes(filePath))));
	}

	private static JObject GetImageUriObjectForCloudVision(string url)
	{
		return new JObject(new JProperty("source", new JObject(new JProperty("gcsImageUri", url))));
	}

	private static JObject GetImageContentForCloudVision(FileInfo filePath)
	{
		return new JObject(new JProperty("content", Convert.ToBase64String(File.ReadAllBytes(filePath.FullName))));
	}

	private static JObject GetJsonRequestForCloudVision(string type, JToken image)
	{
		return new JObject(new JProperty("requests", new JArray(new JObject(new object[]{new JProperty("image", image), new JProperty("features", new JArray(new JObject(new JProperty("type", type))))}))));
	}

	private static void InvokeCloudVisionApi(String apiKey, String imagePath, String imageUrl, decimal timeout, ref String responseJson, ref int statusCode, int provideImage, string apiCallType)
	{
		String v = apiKey;
		FileInfo v2 = (provideImage == 1) ? new FileInfo(imagePath) : null;
		string v3 = (provideImage == 0) ? imageUrl : null;
		if (timeout < 1m)
		{
			Console.WriteLine("<Timeout> should be equal or greater than 1");
		}

		string url = "https://vision.googleapis.com/v1/images:annotate?key=" + v;
		try
		{
			JObject image = (provideImage == 0) ? GetImageUriObjectForCloudVision(v3) : GetImageContentForCloudVision(v2);
			string text = GetJsonRequestForCloudVision(apiCallType, image).ToString();
			var client = new RestClient(url);
			var request = new RestRequest();
			request.AddHeader("Accept", "application/json");
			request.Parameters.Clear();
			request.AddParameter("application/json", text, ParameterType.RequestBody);
			var response = client.Post(request);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
		}
	}

	public static void LabelDetection(String apiKey, String imagePath, String imageUrl, decimal timeout, ref String responseObject, ref int statusCode, int provideImage)
	{
		InvokeCloudVisionApi(apiKey, imagePath, imageUrl, timeout, ref responseObject, ref statusCode, provideImage, "LABEL_DETECTION");
	}

	public static void LandmarkDetection(String apiKey, String imagePath, String imageUrl, decimal timeout, ref String responseObject, ref int statusCode, int provideImage)
	{
		InvokeCloudVisionApi(apiKey, imagePath, imageUrl, timeout, ref responseObject, ref statusCode, provideImage, "LANDMARK_DETECTION");
	}

	public static void LogoDetection(String apiKey, String imagePath, String imageUrl, decimal timeout, ref String responseObject, ref int statusCode, int provideImage)
	{
		InvokeCloudVisionApi(apiKey, imagePath, imageUrl, timeout, ref responseObject, ref statusCode, provideImage, "LOGO_DETECTION");
	}

	public static void SafeSearchDetection(String apiKey, String imagePath, String imageUrl, decimal timeout, ref String responseObject, ref int statusCode, int provideImage)
	{
		InvokeCloudVisionApi(apiKey, imagePath, imageUrl, timeout, ref responseObject, ref statusCode, provideImage, "SAFE_SEARCH_DETECTION");
	}

	public static void TextDetection(String apiKey, String imagePath, String imageUrl, decimal timeout, ref String responseObject, ref int statusCode, int provideImage)
	{
		InvokeCloudVisionApi(apiKey, imagePath, imageUrl, timeout, ref responseObject, ref statusCode, provideImage, "TEXT_DETECTION");
	}
}
}
