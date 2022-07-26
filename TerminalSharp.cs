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

#####                             ### #     # #    # 
#     #  ####  #####   ####  #####  #  ##    # #   #  
#       #    # #    # #    #   #    #  # #   # #  #   
#  #### #    # #    # #    #   #    #  #  #  # ###    
#     # #    # #    # #    #   #    #  #   # # #  #   
#     # #    # #    # #    #   #    #  #    ## #   #  
#####   ####  #####   ####    #   ### #     # #    # 


- CommandParser: .01
- BashLikeCommands: .01
";

    //private Timer timer;
    [Export]
    public List<Resource> availableStories;
    private bool canSelectGame;

    public override void _Ready()
    {
        // Retrieve or create some Nodes we know we'll need quite often
        player = GetNode<InkPlayer>("InkPlayer");

        // player.LoadStory()
        output = GetNode<RichTextLabel>("Output");
        input = GetNode<LineEdit>("Input");

        //  container = GetNode<StoryContainer>("Container");

        timer = new Timer()
        {
            Autostart = false,
            WaitTime = 0.3f,
            OneShot = true,
        };
        AddChild(timer);
        PrintToTerminal(introText);
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

        if (Input.IsActionPressed("ui_accept"))
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

            if (player.HasChoices)
            {
                PrintToTerminal(prompt_template + " Selected: " + player.CurrentChoices[i]);
                player.ChooseChoiceIndex(i);
                if (!player.HasChoices)
                {
                    PrintToTerminal("Finished Story");
                    ShowGameSelect();
                }
            }
            if (canSelectGame)
            {

                selectGame(i);
            }

        }
    }

    private void selectGame(int i)
    {
        if (i > availableStories.Count)
            return;
        player.LoadStory(availableStories[i]);
        canSelectGame = false;

    }

    private void ShowGameSelect()
    {
        PrintToTerminal(introText);
        PrintToTerminal("Select Your Game :");
        var i = 0;
        availableStories.ForEach(e =>
       {
           PrintToTerminal(i + ": " + e.ResourcePath.Replace("res://ink_stories/", "").Replace(".json", ""));
           i++;

       });
        PrintToTerminal(prompt_template);
        canSelectGame = true;
        //throw new NotImplementedException();
    }

    public override void _Process(float delta)
    {
        // If the time is running, we want to wait
        if (timer.TimeLeft > 0) return;

        // Check if we have anything to consume
        if (player.CanContinue)
        {
            string text = player.Continue().Trim();
            if (text.Length > 0)
                PrintToTerminal(text);
            //container.Add(container.CreateText(text));

            // Maybe we have choices now that we moved on?
            if (player.HasChoices)
            {
                for (int i = 0; i < player.CurrentChoices.Length; ++i)
                    PrintToTerminal(i + ": " + player.CurrentChoices[i]);
            }
            timer.Start();
        }



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


