using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    public TextAsset locationsJson;
    public MajorLocation[] majorLocations;

    public Typewriter typewriter;
    public TMP_InputField inputField;
    public HealthScript health;
    public LevelScript level;
    public UserInputSignalScript userInputSignal;

    bool hasStarted = false;

    string buffer = "";

    public Player player = new();

    // Start is called before the first frame update
    void Start()
    {
        majorLocations = JsonUtility.FromJson<MajorLocationsObject>(locationsJson.text).majorLocations;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !hasStarted && Time.time > 2f) {
            hasStarted = true;
            StartGame();
        }
    }

    void StartGame() {
        typewriter.SkipLine();
        typewriter.AddNewLine("Choose your SPECIAL:");
        typewriter.AddNewLine("You have 40 points to distribute as you want. The maximum for each stat is 10.");
        typewriter.SkipLine();

        AskUserStatValue(1);
    }

    void GetUserInput(string line, Action<string> response) {
        typewriter.AddNewLine(line);
        inputField.ActivateInputField();
        userInputSignal.Activate();
        inputField.onEndEdit.AddListener(val => {
            if (Input.GetKeyDown(KeyCode.Return) && val != "") {
                response(val);
                inputField.text = "";
            }
        });
    }

    void AskUserStatValue(int statIndex) {
        if (statIndex == 8) {
            health.EnableHP();
            level.EnableLevel();

            StartAdventure();
            return;
        }

        string stat = "";

        switch (statIndex) {
            case 1:
                stat = "STRENGTH";
                break;
            case 2:
                stat = "PERCEPTION";
                break;
            case 3:
                stat = "ENDURANCE";
                break;
            case 4:
                stat = "CHARISMA";
                break;
            case 5:
                stat = "INTELLIGENCE";
                break;
            case 6:
                stat = "AGILITY";
                break;
            case 7:
                stat = "LUCK";
                break;
            default:
                return;
        }

        GetUserInput(stat + ":", (response) => {
            int input;
            try {
                input = int.Parse(response);
            } catch (FormatException) {
                typewriter.AddNewLine("[ERROR] Please enter an integer.");
                return;
            }

            if (input > 10 || input < 0) {
                typewriter.AddNewLine("[ERROR] Please enter an integer between 0 and 10.");
                return;
            }

            if (input + player.special.Sum() > 40) {
                typewriter.AddNewLine("[ERROR] Please respect the limit of 40 points.");
                return;
            }

            switch (statIndex) {
                case 1:
                    player.special.strength = input;
                    break;
                case 2:
                    player.special.perception = input;
                    break;
                case 3:
                    player.special.endurance = input;
                    break;
                case 4:
                    player.special.charisma = input;
                    break;
                case 5:
                    player.special.intelligence = input;
                    break;
                case 6:
                    player.special.agility = input;
                    break;
                case 7:
                    player.special.luck = input;
                    break;
                default:
                    return;
            }

            typewriter.AddNewLine("Okay. " + input + " for " + stat + ".");
            inputField.onEndEdit.RemoveAllListeners();
            userInputSignal.Deactivate();
            AskUserStatValue(++statIndex);
        });
    }

    void StartAdventure() {
        MajorLocation location1 = majorLocations[(int)UnityEngine.Random.Range(0f, majorLocations.Length - 1f)];
        MajorLocation location2 = majorLocations[(int)UnityEngine.Random.Range(0f, majorLocations.Length - 1f)];
        while (location2.name.Equals(location1.name)) {
            location2 = majorLocations[(int)UnityEngine.Random.Range(0f, majorLocations.Length - 1f)];
        }

        typewriter.SkipLine();
        typewriter.AddNewLine("- Okay. Let's do this.");
        typewriter.AddNewLine("<i>The vault door opens slowly.</i>");
        typewriter.AddNewLine("You can see the sun shining for the first time before your eyes.");
        buffer = string.Format("You see on the horizon: a {0} and a {1}.", location1.name, location2.name);
        typewriter.AddLine(buffer);
        typewriter.AddNewLine("Where do you wanna go?");

        buffer = "";
    }
}
