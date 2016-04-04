using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Timers;

public class Conversation  {

	List<ActiveAgent> subscribers = new List<ActiveAgent> ();
	ActiveAgent lastToSpeak;
	public Response lastResponse { get; set;}
	public int State { get; set;}
	public List<Response> responses { get; set; }
	XElement element;

	public Conversation (ActiveAgent speaker, ActiveAgent listener) {
		if (speaker is Fairy_Controller) {
			element = XElement.Load (".\\Assets\\_scripts\\FairyDialogue.xml");
		} else if (speaker is WoodsmenBehaviour) {
			element = XElement.Load (".\\Assets\\_scripts\\WoodsmanDialogue.xml");
		}
		responses = ResponseList (element);
		subscribers.Add (speaker);
		subscribers.Add (listener);
		State = 1;
	}

	public void Begin() {
		NPCUpdate ();
	}

	public bool Next () {
		if (lastResponse == null) {
			return false;
		}

		Debug.Log(lastResponse.Text);

		if (lastResponse is PlayerResponse || lastResponse.PlayerResponses.First().Text == "") {
			if (!(lastResponse is PlayerResponse))
				State = lastResponse.PlayerResponses.First ().Next;
			NPCUpdate ();
			return true;
		}
		PlayerUpdate (lastResponse);
		return true;
	}
		
	void NPCUpdate()
	{
		if (responses.Find (r => r.ID == State) == null) {
			lastResponse = null;
			return;
		}

		if (responses.Find (r => r.ID == State).Condition.First().Key == "None") {
			lastResponse = responses.Find (r => r.ID == State);
		}

		Response response;
		var choices = responses.FindAll (r => r.ID == State);
		var conditionList = ProgressTracker.GetProgressTracker().GetConditionDict ();
		//Debug.Log ("Conditions:");
		foreach (var condition in conditionList)
		{
			//Debug.Log (condition);
			choices.Find (r => (r.Condition.First().Key == condition.Key && r.Condition.First().Value == condition.Value));
			response = choices.Find (r => (r.Condition.First().Key == condition.Key && r.Condition.First().Value == condition.Value));
			if (response != null) {
				//Debug.Log (response.Condition.First());
				lastResponse = response;
				break;
			}
		}
	}

	void PlayerUpdate(Response response)
	{
		PlayerResponse r = response.PlayerResponses.First();
		State = r.Next;
		lastResponse = r;
	}
		
	Dictionary<string, bool> ConvertToCondition (string condition, string value)
	{
		var dict = new Dictionary<string, bool> ();
		if (condition == "" && value == "") {
			dict.Add("None", true);
			return dict;
		}

		if (value.ToLower().Equals("true"))
			dict.Add(condition, true);
		else if (value.ToLower().Equals("false"))
			dict.Add(condition, false);
		return dict;
	}

	List<Response> ResponseList (XElement element)
	{
		var conversation = (from r in element.Elements("Response")
			select new Response
			{
				ID = int.Parse(r.Attribute("id").Value),
				Condition = ConvertToCondition(r.Attribute("condition").Value, r.Attribute("value").Value),
				Text = r.Element("Text").Value,
				PlayerResponses = (from p in r.Descendants("PlayerResponse")
					select new PlayerResponse
					{
						Text = p.Element("Text").Value,
						Next = int.Parse(p.Element("Next").Value),
					}).ToList(),
			}).ToList();
		return conversation;
	}
		
	public class Response
	{
		public int ID { get; set; }
		public Dictionary<string, bool> Condition { get; set; }
		public string Text { get; set; }
		public List<PlayerResponse> PlayerResponses { get; set; }

	}

	public class PlayerResponse : Response
	{
		public int Next { get; set; }
	}
}
