using TMPro;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public Game game;
    TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<TMP_Text>();
        text.enabled = false;
    }

    int CalculateMaxHP(int endurance, int level) {
        return (int)(80 + (endurance * 5) + (level - 1) * (endurance/2 + 2.5f));
    }

    public void EnableHP() {
        game.player.hp = game.player.maxHp;
        text.text = $"HP [-----] {game.player.hp}/{game.player.hp}";
        text.enabled = true;
    }

    public void AffectHealth(int health) {
        game.player.hp += health;
        string dashedHp = "";
        for (int i = 0; i < game.player.hp / game.player.maxHp; i++) {
            dashedHp += "-";
        }
        text.text = $"HP [{dashedHp.PadRight(5, ' ')}] {game.player.hp}/{game.player.maxHp}";
    }

    void Update() {
        game.player.maxHp = CalculateMaxHP(game.player.special.endurance, game.player.level);
    }
}
