using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostWatcher : MonoBehaviour
{
  private TextMeshProUGUI hostShort;

  private GameObject websocketObject;
  private WebsocketWatcher websocketScript;

  private void Start()
  {
    websocketObject = GameObject.Find("WebsocketWatcher");
    websocketScript = websocketObject.GetComponent<WebsocketWatcher>();

    // Generate a short code and show it too the user
    hostShort = GameObject.Find("HostText").GetComponent<TextMeshProUGUI>();
    hostShort.text = GenerateCode(5);

    if (websocketScript.websocket == null) {
      websocketScript.StartWebsocket();
    };

    websocketScript.websocket.OnOpen += WebsocketOpen;
    websocketScript.websocket.OnMessage += WebsocketResponse;
  }

  private void WebsocketOpen() {

  // Share the short code with the server so someone else can subscribe to it
          Action action = new Action();

      action.code = hostShort.text;
      action.action = "hosting";

      websocketScript.SendWebsocket(action);
  }

  private void WebsocketResponse(byte[] bytes) {
    // Once a partner has been found we can go to the next scene
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }

  private string GenerateCode(int length)
  {
		// I've removed characters that are hard to distinguish from each other
    string randomCharacters = "abcdefghjkmnopqrstuvwxyz023456789";
    string randomString = "";

    for (int i = 0; i < length; i++)
    {
      int randomIndex = UnityEngine.Random.Range(0, randomCharacters.Length);
      char randomCharacter = randomCharacters[randomIndex];

      randomString += randomCharacter;
    }

    return randomString;
  }

}