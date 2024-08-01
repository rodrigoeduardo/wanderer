using TMPro;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    public Game game;
    TMP_Text text;
    
    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<TMP_Text>();
        text.enabled = false;
    }

    public void EnableLevel() {
        text.enabled = true;
        text.text = "LVL 1";
    }

    public void LevelUp() {
        game.player.level += 1;
        text.text = "LVL " + game.player.level;
    }
}
