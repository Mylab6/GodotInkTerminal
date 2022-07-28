using Godot;
using System;
using System.Collections.Generic;

public class TerminalSharp : Control
{
    private string gameOverText = "Game Over, Thank You ! ";
    public InkPlayer player;
    // private StoryContainer container;
    [Export]

    public string prompt_template = "godot@terminal:~ ";
    [Export]

    public string input_template = "[color=#ffffff]%s[/color]";
    [Export]
    public string error_template = "\n[color=#dd0000][ERROR] %s[/color]";
    private Timer timer;
    public RichTextLabel output;

    public LineEdit input;

    [Export]

    public string introText = @"
   ______                 __         _   _____           __       
 .' ___  |               |  ]       / |_|_   _|         [  |  _   
/ .'   \_|   .--.    .--.| |  .--. `| |-' | |   _ .--.   | | / ]  
| |   ____ / .'`\ \/ /'`\' |/ .'`\ \| |   | |  [ `.-. |  | '' <   
\ `.___]  || \__. || \__/  || \__. || |, _| |_  | | | |  | |`\ \  
 `._____.'  '.__.'  '.__.;__]'.__.' \__/|_____|[___||__][__|  \_]                                                                
     ";

    //private Timer timer;
    [Export]
    public List<Resource> availableStories;
    private bool canSelectGame;
    [Export]
    public string copyRightText = @"
GodotInkTerminal 0.0.1
MIT Licensed.     
";
    public override void _Ready()
    {
        // Retrieve or create some Nodes we know we'll need quite often
        player = GetNode<InkPlayer>("InkPlayer");

        // player.LoadStory()
        output = GetNode<RichTextLabel>("Output");
        input = GetNode<LineEdit>("Input");
        player.Connect(nameof(InkPlayer.InkContinued), this, nameof(ContinueGame));

			player.Connect(nameof(InkPlayer.InkChoices), this, nameof(ShowChoices));
			player.Connect(nameof(InkPlayer.InkEnded), this, nameof(endGame));


        //player.EmitSignal
        //  container = GetNode<StoryContainer>("Container");

        timer = new Timer()
        {
            Autostart = false,
            WaitTime = 0.4f,
            OneShot = true,
        };
        AddChild(timer);
        PrintToTerminal(copyRightText);
        ShowGameSelect();
        //  Connect("InkEnded", this, "gameEnd");
        //  player.Connect("InkEnded", this, "gameEnd");
    }

    public void endGame()
    {
        PrintToTerminal("Game end");
        ShowGameSelect();


    }
    public override void _Input(InputEvent inputEvent)
    {

        if (Input.IsActionJustPressed("ui_accept"))
        {
            var fixedText = input.Text.Trim().ToLower();
            if (fixedText == "exit" || fixedText == "reset")
            {
                ShowGameSelect();
                //input.Text = string.Empty;
                return;
            }
            int i = -1;
            bool result = int.TryParse(input.Text, out i); //i now = 108  
            input.Text = string.Empty;
            i = i - 1;

            if(canSelectGame){
                selectGame(i); 
            }
            if (player.HasChoices)
            {
                PrintToTerminal(prompt_template + " Selected: " + player.CurrentChoices[i]);
                player.ChooseChoiceIndex(i);

            }

        }
    }

    private void selectGame(int i)
    {
        try
        {
            player.LoadStory(availableStories[i]);
            canSelectGame = false;
        }
        catch (Exception e)
        {

            PrintToTerminal(e.Message);
        }
    }

    private void ShowGameSelect()
    {
        PrintToTerminal(introText);
        PrintToTerminal("Select Your Game :");
        player.inkStory = null;
        var i = 0;
        availableStories.ForEach(e =>
       {
           PrintToTerminal((i + 1) + ": " + e.ResourcePath.Replace("res://ink_stories/", "").Replace(".json", ""));
           i++;

       });
        PrintToTerminal(prompt_template);
        canSelectGame = true;
        //throw new NotImplementedException();
    }
    // fix me, but 
    public void ContinueGame(string text, string[] tags)
    {
        if (player.CanContinue)
        {
          //  string text = player.Continue().Trim();
            if (text.Length > 0)
                PrintToTerminal(text);
      
        }

    }

    private void ShowChoices()
    {
        for (int i = 0; i < player.CurrentChoices.Length; ++i)
            PrintToTerminal((i + 1) + ": " + player.CurrentChoices[i]);
    }

    public void PrintToTerminal(string text, bool newLine = true)
    {
        //func output_print(txt: String):
        output.Text += text;
        if (newLine)
        {

            output.Text += "\n";
        }

    }
}


