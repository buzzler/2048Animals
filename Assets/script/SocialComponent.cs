using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

public enum SocialResult {
	BUSY = 0,
	COMPLETE = 1,
	ERROR_NETWORK,
	ERROR_FACEBOOK
}

public delegate void SocialDelegate(SocialResult result);

public class SocialComponent : MonoBehaviour {
	private SocialDelegate	oncomplete;

	/**
	 * for tutorial
	 */
	private string texts = "";
	private string textTitle = "Animal's Bang Title";
	private string textUrl = "http://www.unityscene.com";
	private string textDesc = "description blah";
	private string textCaption = "caption blah";
	private string textThumb = "";
	
	void OnGUI() {
		float gapA = 20;
		float gapB = 10;
		float widthButton = (Screen.width - gapB*2 - gapA*2) / 3;
		float heightButton = widthButton / 4;
		float widthInput = widthButton * 2 + gapB;
		float heightInput = 30;
		float widthArea = Screen.width - gapA*2;

		GUI.BeginGroup(new Rect(gapA,gapA,widthButton*3+gapB*2,heightButton));
		if (GUI.Button(new Rect(0,0,widthButton,heightButton), "FACEBOOK"))
			OnClickFacebook();
		if (GUI.Button(new Rect(widthButton+gapB,0,widthButton,heightButton), "DISCONNECT"))
			OnClickDisconnect();
		if (GUI.Button(new Rect(widthButton*2+gapB*2,0,widthButton,heightButton), "FEED"))
			OnClickFeed();
		GUI.EndGroup();

		GUI.BeginGroup(new Rect(gapA,gapA*2+heightButton,widthButton*3+gapB*2, heightButton));
		if (GUI.Button(new Rect(0,0,widthButton,heightButton), "REQUEST"))
			OnClickRequest();
		if (GUI.Button(new Rect(widthButton+gapB,0,widthButton,heightButton), ""))
			Debug.Log("");
		if (GUI.Button(new Rect(widthButton*2+gapB*2,0,widthButton, heightButton), ""))
			Debug.Log("");
		GUI.EndGroup();

		GUI.BeginGroup(new Rect(gapA, gapA*3+heightButton*2, widthButton*3+gapB*2, heightButton));
		if (GUI.Button(new Rect(0,0,widthButton, heightButton), ""))
			Debug.Log("");
		if (GUI.Button(new Rect(widthButton+gapB,0,widthButton, heightButton), ""))
			Debug.Log("");
		if (GUI.Button(new Rect(widthButton*2+gapB*2,0,widthButton, heightButton), ""))
			Debug.Log("");
		GUI.EndGroup();

		GUI.BeginGroup(new Rect(gapA, gapA*4+heightButton*3, widthInput, heightInput*5+gapB*4));
			textTitle	= GUI.TextField(new Rect(0,0,widthInput,heightInput), textTitle);
			textUrl		= GUI.TextField(new Rect(0,heightInput+gapB,widthInput,heightInput), textUrl);
			textDesc	= GUI.TextField(new Rect(0,(heightInput+gapB)*2,widthInput,heightInput), textDesc);
			textCaption	= GUI.TextField(new Rect(0,(heightInput+gapB)*3,widthInput,heightInput), textCaption);
			textThumb	= GUI.TextField(new Rect(0,(heightInput+gapB)*4,widthInput,heightInput), textThumb);
		GUI.EndGroup();

		float offsetY = gapA*5+gapB*4+heightButton*3+heightInput*5;
		float maxHeight = Screen.height - offsetY - gapA;
		GUI.TextArea(new Rect(gapA, offsetY, widthArea, maxHeight), texts);
	}

	private void OnClickFacebook() {
		this.FacebookConnect(ResultHandler);
	}

	private void OnClickDisconnect() {
		this.FacebookDisconnect(ResultHandler);
	}

	private void OnClickFeed() {
		this.FacebookFeed(textTitle, textUrl, textDesc, textCaption, textThumb, ResultHandler);
	}

	private void OnClickRequest() {
		this.FacebookRequest("install this game!!", "Select friends for 2048 requests", ResultHandler);
	}

	public	void ResultHandler(SocialResult result) {
		texts += result.ToString() + "\n";
	}
	/**
	 * for tutorial
	 */
	
	/**
	 * connect facebook account
	 */
	public void FacebookConnect(SocialDelegate oncomplete) {
		if (IsAvailable(oncomplete)) {
			FB.Init(oninitcomplete);
		}
	}

	/**
	 * disconnect facebook account
	 */
	public void FacebookDisconnect(SocialDelegate oncomplete) {
		if (IsAvailable(oncomplete)) {
			FB.Logout();
			PlayerPrefs.SetInt("resistered",0);
			CallDelegateAndFlush(SocialResult.COMPLETE);
		}
	}

	/**
	 * check connection of account
	 */
	public bool IsConnected() {
		if (PlayerPrefs.HasKey("resistered")){
			return (PlayerPrefs.GetInt("resistered")==1);
		} else {
			return false;
		}
	}

	/**
	 * feed on facebook
	 */
	public void FacebookFeed(string title, string url, string description, string caption, string thumbnail, SocialDelegate oncomplete) {
		if (IsAvailable(oncomplete)) {
			FB.Feed(
				link: url,
				linkName: title,
				linkDescription: description,
				linkCaption: caption,
				picture: thumbnail,
				callback: onfeedcomplete
				);
		}
	}

	/**
	 * invite friend from facebook
	 */
	public void FacebookRequest(string message, string title, SocialDelegate oncomplete) {
		if (IsAvailable(oncomplete)) {
			FB.AppRequest(message, null, null, null, 1, "", title, onrequestcomplete);
		}
	}

	private bool IsAvailable(SocialDelegate newdelegate) {
		if (!CheckForInternetConnection()) {
			newdelegate(SocialResult.ERROR_NETWORK);
			return false;
		}
		if (this.oncomplete!=null) {
			newdelegate(SocialResult.BUSY);
			return false;
		} else {
			this.oncomplete = newdelegate;
			return true;
		}
	}

	private bool CheckForInternetConnection() {
//		try
//		{
//			using (var client = new WebClient())
//			using (var stream = client.OpenRead("http://www.facebook.com"))
//			{
//				return true;
//			}
//		}
//		catch
//		{
//			return false;
//		}
		NetworkReachability nr = Application.internetReachability;
		switch(nr) {
		case NetworkReachability.NotReachable:
			return false;
		case NetworkReachability.ReachableViaCarrierDataNetwork:
		case NetworkReachability.ReachableViaLocalAreaNetwork:
			return true;
		default:
			return false;
		}
	}
	
	private void CallDelegateAndFlush(SocialResult result) {
		if (this.oncomplete!=null) {
			this.oncomplete(result);
		}
		this.oncomplete = null;
	}

	private	void oninitcomplete() {
		if (FB.IsInitialized) {
			if (FB.IsLoggedIn) {
				CallDelegateAndFlush(SocialResult.COMPLETE);
			} else {
				FB.Login("public_profile, user_friends", onlogincomplete);
			}
		} else {
			CallDelegateAndFlush(SocialResult.ERROR_FACEBOOK);
		}
	}
	
	private void onlogincomplete(FBResult result) {
		if (FB.IsLoggedIn) {
			PlayerPrefs.SetInt("resistered", 1);
			CallDelegateAndFlush(SocialResult.COMPLETE);
		} else {
			CallDelegateAndFlush(SocialResult.ERROR_FACEBOOK);
		}
	}

	private void onfeedcomplete(FBResult result) {
		if (String.IsNullOrEmpty(result.Error)) {
			CallDelegateAndFlush(SocialResult.COMPLETE);
		} else {
			CallDelegateAndFlush(SocialResult.ERROR_FACEBOOK);
		}
	}

	private void onrequestcomplete(FBResult result) {
		if (String.IsNullOrEmpty(result.Error)) {
			// result.text 
			CallDelegateAndFlush(SocialResult.COMPLETE);
		} else {
			CallDelegateAndFlush(SocialResult.ERROR_FACEBOOK);
		}
	}
}
