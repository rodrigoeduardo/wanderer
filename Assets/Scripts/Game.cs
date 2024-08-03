using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public TextAsset locationsJson;
    public List<MajorLocation> majorLocations;

    public Typewriter typewriter;
    public TMP_InputField inputField;
    public HealthScript health;
    public LevelScript level;
    public UserInputSignalScript userInputSignal;

    bool hasStarted = false;

    public Player player = new();

    public List<MajorLocation> possibleMajorLocations = new();
    public MajorLocation currentMajorLocation;

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

    void GetUserInput(Action<string> response) {
        inputField.ActivateInputField();
        userInputSignal.Activate();
        inputField.onEndEdit.AddListener(val => {
            if (Input.GetKeyDown(KeyCode.Return) && val != "") {
                response(val);
                inputField.text = "";
            }
        });
    }

    void RespondUserCommand(string command) {
        switch (command)
        {
            case "":
            default:
                typewriter.AddNewLine("[ERROR] Command not recognized.");
                break;
        }
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

        typewriter.AddNewLine(stat + ":");
        GetUserInput((response) => {
            int input;
            try {
                input = int.Parse(response);
            } catch (FormatException) {
                typewriter.AddNewLine("[ERROR] Please enter an integer.");
                return;
            }

            if (input > 10 || input < 1) {
                typewriter.AddNewLine("[ERROR] Please enter an integer between 1 and 10.");
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
        List<MajorLocation> twoLocations = GetRandomLocations(2);

        typewriter.SkipLine();
        typewriter.AddNewLine("- Okay. Let's do this.");
        typewriter.AddNewLine("<i>The vault door opens slowly.</i>");
        typewriter.AddNewLine("You can see the sun shining for the first time before your eyes.");
        possibleMajorLocations.AddRange(twoLocations);
        typewriter.AddLine($"You see on the horizon: a {twoLocations[0].name} and a {twoLocations[1].name}.");
        typewriter.AddNewLine("Where do you wanna go?");
    }

    public List<MajorLocation> GetRandomLocations(int count)
    {
        List<MajorLocation> selectedLocations = new();
        double totalProbability = 0.0;
        
        List<MajorLocation> availableLocations = new(majorLocations);

        for (int i = 0; i < count; i++)
        {
            foreach (MajorLocation location in availableLocations)
            {
                totalProbability += location.probability;
            }
            double randomValue = UnityEngine.Random.Range(0f, 1f) * totalProbability;
            double cumulativeProbability = 0.0;

            for (int j = 0; j < availableLocations.Count; j++)
            {
                cumulativeProbability += availableLocations[j].probability;
                if (randomValue <= cumulativeProbability)
                {
                    selectedLocations.Add(availableLocations[j]);
                    availableLocations.RemoveAt(j);
                    break;
                }
            }
        }

        return selectedLocations;
    }
}
