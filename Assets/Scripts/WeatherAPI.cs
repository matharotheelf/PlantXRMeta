using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;
using SimpleJSON;
public class WeatherAPI : MonoBehaviour {
	//public string url = "http://api.openweathermap.org/data/2.5/weather?lat=43.667027&lon=-79.424759&APPID=391013b34d8368540317a2dd0dd2536d";

	public float humidity;
	public float precipitation;

	[SerializeField] string currentIP;
	[SerializeField] string currentCity;
	[SerializeField] string currentLatitude;
	[SerializeField] string currentLongitude;

	[SerializeField] string WeatherAPIToken = "DUMMY";
	[SerializeField] string LocationAPIToken = "DUMMY";

	[SerializeField] GameObject humidityMeter;
	[SerializeField] GameObject precipitationMeter;

	[SerializeField] float humidityDisplayMultiple = 0.1f;
	[SerializeField] float precipitationDisplayMultiple = 1f;

	private string weatherAPItokenParam;
	private string locationParam;
	private string dateParam;

	private string WeatherAPIURL = "https://api.openweathermap.org";
	private string IpAPIURL = "https://api.ipify.org";
	private string LocationAPIURL = "https://ipinfo.io";

	// Use this for initialization
	IEnumerator Start () {

		weatherAPItokenParam = $"&APPID={WeatherAPIToken}";

		//get the players IP, City, Country
		//Network.Connect("https://api.ipify.org");
		//currentIP = Network.player.externalIP;
		//Network.Disconnect();

		if (string.IsNullOrEmpty(currentCity))
        {
			StartCoroutine(getLocationFromIP());

			yield return getLocationFromIP();
		} else
        {
			StartCoroutine(getLocationFromCityName());

			yield return getLocationFromCityName();
		}

		StartCoroutine(getTodayWeather());
    }

	// get the weather of the current location
	IEnumerator getTodayWeather()
	{
		string currentDate = System.DateTime.Now.ToString("yyyy-MM-dd");

		dateParam = $"&date={currentDate}";

		string weatherUrl = $"{WeatherAPIURL}/data/3.0/onecall/day_summary?{locationParam}{dateParam}{weatherAPItokenParam}";

		using (UnityWebRequest weatherWebRequest = UnityWebRequest.Get(weatherUrl))
		{
			// Request and wait for the desired page.
			yield return weatherWebRequest.SendWebRequest();

			switch (weatherWebRequest.result)
			{
				case UnityWebRequest.Result.ConnectionError:
					Debug.LogError("Connection Error: " + weatherWebRequest.error);
					break;
				case UnityWebRequest.Result.DataProcessingError:
					Debug.LogError("Error: " + weatherWebRequest.error);
					break;
				case UnityWebRequest.Result.ProtocolError:
					Debug.LogError("HTTP Error: " + weatherWebRequest.error);
					break;
				case UnityWebRequest.Result.Success:
					JSONNode weatherData = JSON.Parse(weatherWebRequest.downloadHandler.text);

					setWeatherAttributes(weatherData);
					break;
			}
		}
	}

	// get the location from the device's ip address in lat and long
	IEnumerator getLocationFromIP()
	{
		currentIP = new System.Net.WebClient().DownloadString(IpAPIURL);

		string locationUrl =  $"{LocationAPIURL}/{currentIP}?token={LocationAPIToken}";

		using (UnityWebRequest locationWebRequest = UnityWebRequest.Get(locationUrl))
		{
			// Request and wait for the desired page.
			yield return locationWebRequest.SendWebRequest();

			switch (locationWebRequest.result)
			{
				case UnityWebRequest.Result.ConnectionError:
					Debug.LogError("Connection Error: " + locationWebRequest.error);
					break;
				case UnityWebRequest.Result.DataProcessingError:
					Debug.LogError("Error: " + locationWebRequest.error);
					break;
				case UnityWebRequest.Result.ProtocolError:
					Debug.LogError("HTTP Error: " + locationWebRequest.error);
					break;
				case UnityWebRequest.Result.Success:
					JSONNode locationData = JSON.Parse(locationWebRequest.downloadHandler.text);

					setLocationParamFromIpApiData(locationData);
					break;
			}
		}
	}

	// get the lat and long of the current city
	IEnumerator getLocationFromCityName()
	{
		string locationUrl= $"{WeatherAPIURL}/geo/1.0/direct?q={currentCity}&limit=1{weatherAPItokenParam}";

		using (UnityWebRequest locationWebRequest = UnityWebRequest.Get(locationUrl))
		{
			// Request and wait for the desired page.
			yield return locationWebRequest.SendWebRequest();

			switch (locationWebRequest.result)
			{
				case UnityWebRequest.Result.ConnectionError:
					Debug.LogError("Connection Error: " + locationWebRequest.error);
					break;
				case UnityWebRequest.Result.DataProcessingError:
					Debug.LogError("Error: " + locationWebRequest.error);
					break;
				case UnityWebRequest.Result.ProtocolError:
					Debug.LogError("HTTP Error: " + locationWebRequest.error);
					break;
				case UnityWebRequest.Result.Success:
					JSONNode locationData = JSON.Parse(locationWebRequest.downloadHandler.text)[0];

					setLocationParamFromCityApiData(locationData);
					break;
			}
		}
	}

	// set the 
	void setLocationParamFromCityApiData(JSONNode locationData)
	{
		currentLatitude = locationData["lat"].Value;
		currentLongitude = locationData["lon"].Value;
		locationParam = $"lat={currentLatitude}&lon={currentLongitude}";
	}

	void setLocationParamFromIpApiData(JSONNode locationData)
	{
		currentLatitude = locationData["loc"].Value.Split(",")[0];
		currentLongitude = locationData["loc"].Value.Split(",")[1];
	}

		// set the weather attributes from request data
		void setWeatherAttributes(JSONNode weatherJson)
	{
		precipitation = weatherJson["precipitation"]["total"].AsFloat;
        humidity = weatherJson["humidity"].Children.First().AsFloat;

		setHumidityMeterScale();
		setPrecipitationMeterScale();
	}

	// set the humidity meter height to display humidty
    private void setHumidityMeterScale()
    {
		humidityMeter.transform.localScale += humidityDisplayMultiple * humidity * humidityMeter.transform.up;
	}

	// set the precipitation meter height to display precipitation
    private void setPrecipitationMeterScale()
    {
		precipitationMeter.transform.localScale += precipitationDisplayMultiple * precipitation * precipitationMeter.transform.up;
	}
}