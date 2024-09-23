using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;


public class InternetConnection : MonoBehaviour {
	public static InternetConnection CheckInternet;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	 void Awake(){
		if (CheckInternet == null) {
			CheckInternet = this;
		}


	}

	public bool IsInternetActive(){
		
		string HtmlText = GetHtmlFromUri("http://google.com");
		if(HtmlText == "")
		{
			//No connection
			return true;
		}
		else
		{
			//success
			return false;
		}
	}

	public string GetHtmlFromUri(string resource){
		string html = string.Empty;
		HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
		try
		{
			using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
			{
				bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
				if (isSuccess)
				{

					using (TextReader reader = new StreamReader(resp.GetResponseStream()))
					{
						//We are limiting the array to 80 so we don't have
						//to parse the entire html document feel free to 
						//adjust (probably stay under 300)
						char[] cs = new char[80];
						reader.Read(cs, 0, cs.Length);
						foreach(char ch in cs)
						{
							html +=ch;
						}
					}
				}
			}
		}
		catch
		{
			return "";
		}
		return html;
	}
}
