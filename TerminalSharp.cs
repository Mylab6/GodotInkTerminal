using Godot;

public class TerminalSharp : Control
{
    public InkPlayer player;
    // private StoryContainer container;
    public string prompt_template = "\n[color=#66aaff]godot@terminal:~ [b]$[/b][/color]";

    public string input_template = "[color=#ffffff]%s[/color]";
    public string error_template = "\n[color=#dd0000][ERROR] %s[/color]";
    private Timer timer;
    public RichTextLabel output;
    public LineEdit input;


    public string introText = @"

#####                             ### #     # #    # 
#     #  ####  #####   ####  #####  #  ##    # #   #  
#       #    # #    # #    #   #    #  # #   # #  #   
#  #### #    # #    # #    #   #    #  #  #  # ###    
#     # #    # #    # #    #   #    #  #   # # #  #   
#     # #    # #    # #    #   #    #  #    ## #   #  
#####   ####  #####   ####    #   ### #     # #    # 


- CommandParser: % s
- BashLikeCommands: % s
";

    //private Timer timer;

    public override void _Ready()
    {
        // Retrieve or create some Nodes we know we'll need quite often
        player = GetNode<InkPlayer>("InkPlayer");
        output = GetNode<RichTextLabel>("Output");
        input = GetNode<LineEdit>("Input");
        PrintToTerminal(introText);
        PrintToTerminal(prompt_template);
        //  container = GetNode<StoryContainer>("Container");

        timer = new Timer()
        {
            Autostart = false,
            WaitTime = 0.3f,
            OneShot = true,
        };
        AddChild(timer);
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
                /* container.Add(container.CreateSeparation(), 0.2f);
				 // Add a button for each choice
				 for (int i = 0; i < player.CurrentChoices.Length; ++i)
					 container.Add(container.CreateChoice(player.CurrentChoices[i], i), 0.4f);
			 */
            }
            timer.Start();
        }
        else if (!player.HasChoices)
        {

            SetProcess(false);
        }
    }

    protected void OnChoiceClick(int choiceIndex)
    {
        //container.CleanChoices();
        // Choose the clicked choice
        player.ChooseChoiceIndex(choiceIndex);
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


