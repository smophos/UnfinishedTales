using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Timers;


// Class responsible for holding conversation data and parsing conversation xml files
public class Conversation  {

	// Variables for speakers, last speaker, xml element for parsing, and ProgressTracker
	List<ActiveAgent> subscribers = new List<ActiveAgent> ();
	ActiveAgent lastToSpeak;
	XElement element;
	ProgressTracker tracker;

	// Get and sets for response and conversation state information
	public Response lastResponse { get; set;}
	public int State { get; set;}
	public List<Response> responses { get; set; }

	// Constructor:
	// Parse correct xml file based on Speaker's teyp (or possibly name later)
	// Set the response list, add the passed speaker and listener as subscribers (may remove this later)
	// Get the singleton ProgressTracker instance
	// Set the initial conversation state to 1
	public Conversation (ActiveAgent speaker, ActiveAgent listener) {
		if (speaker is Fairy_Controller) {
			TextAsset progressData = Resources.Load ("FairyDialogue") as TextAsset;
			XDocument document = XDocument.Parse (progressData.text);
			element = document.Root;
			//element = XElement.Load (".\\Assets\\_scripts\\FairyDialogue.xml");
		} else if (speaker is WoodsmenBehaviour) {
			TextAsset progressData = Resources.Load ("WoodsmanDialogue") as TextAsset;
			XDocument document = XDocument.Parse (progressData.text);
			element = document.Root;
			//element = XElement.Load (".\\Assets\\_scripts\\WoodsmanDialogue.xml");
		}
		responses = ResponseList (element);
		subscribers.Add (speaker);
		subscribers.Add (listener);
		tracker = ProgressTracker.GetProgressTracker ();
		State = 1;
	}

	// Get the text of the last response
	public string GetLastResponseText () {
		if (lastResponse != null)
		if (lastResponse is PlayerResponse)
			return "Knight: " + lastResponse.Text;
		else
			return subscribers[0].GetName() + ": " + lastResponse.Text;
		return "";
	}

	// Begin the conversation with NPC
	public void Begin() {
		NPCUpdate ();
	}

	// Figure out who is next to respond and get the appropriate response
	// Returns true if response is not null
	// Returns false if response is null to end the conversation in DialogueManager
	public bool Next () {
		// Make sure the last response wasn't null
		if (lastResponse == null) {
			return false;
		}

		//Debug.Log(lastResponse.Text);

		// If the player spoke last or there were no player options for the last NPC response
		// Set the state according if need be, then get the next NPC response update
		if (lastResponse is PlayerResponse || lastResponse.PlayerResponses.First().Text == "") {
			if (!(lastResponse is PlayerResponse))
				State = lastResponse.PlayerResponses.First ().Next;
			NPCUpdate ();
			return true;
		}

		// If there is a player response, get it
		PlayerUpdate (lastResponse);
		return true;
	}

	// Handle the NPC's next response
	void NPCUpdate()
	{
		// Check if there are no more responses
		if (responses.Find (r => r.ID == State) == null) {
			lastResponse = null;
			return;
		}

		// If there is no condition, automatically give this response if in correct state
		if (responses.Find (r => r.ID == State).Condition.First().Key == "None") {
			lastResponse = responses.Find (r => r.ID == State);
		}

		// Get the correct next response based on the current state and progress tracker booleans
		Response response;
		var choices = responses.FindAll (r => r.ID == State);

		// Get the conditions from ProgressTracker and check against required conditiond for each response
		var conditionList = ProgressTracker.GetProgressTracker().GetConditionDict ();
		//Debug.Log ("Conditions:");
		foreach (var condition in conditionList)
		{
			//Debug.Log (condition);
			response = choices.Find (r => (tracker.GetBool(r.Condition.First().Key) == r.Condition.First().Value));
			if (response != null) {
				//Debug.Log (response.Condition.First());
				lastResponse = response;
				break;
			}
		}
	}

	// Handle the Player's next response
	void PlayerUpdate(Response response)
	{
		PlayerResponse r = response.PlayerResponses.First();
		State = r.Next;
		lastResponse = r;
	}

	// Helper method to convert parsed XML for condition and value
	// into the condition dictionary of a Response
	Dictionary<string, bool> ConvertToCondition (string condition, string value)
	{
		var dict = new Dictionary<string, bool> ();

		// Default if no condition information is given
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

	// Parse the XML file for a list of response elements and initialize a Response instance for each
	List<Response> ResponseList (XElement element)
	{
		var conversation = (from r in element.Elements("Response")
			select new Response
			{
				// Set up each of this responses fields based on file data
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

	// Class to hold parsed XML response data
	public class Response
	{
		public int ID { get; set; }
		public Dictionary<string, bool> Condition { get; set; } // Required conditions for this response
		public string Text { get; set; }
		public List<PlayerResponse> PlayerResponses { get; set; }

	}

	// Class to hold parsed XML player response data
	public class PlayerResponse : Response
	{
		public int Next { get; set; }
	}
}