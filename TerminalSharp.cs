using Godot;
using System;

public class TerminalSharp : Control
{

				public String  CurrentStoryResPath;
				public String  PromptTemplate  = '\n[Color] = ; //66aaff]godot@terminal:~ [b]$[/b][/color] ';
				public String  InputTemplate  = '[Color] = ; //ffffff]%s[/color]';
				public String = '\n[Color] = ; //dd0000][ERROR] %s[/color]' ;

				var parser = new CommandParser();
				var commands = new BashLikeCommands();

				Onready Var Output : = GetNode("Output") as RichTextLabel;
				Onready Var Input : = GetNode("Input") as LineEdit;


				public override void _Ready()
				{
					//output_print(prompt_template)

					Input.GrabFocus();
					Input.Connect('guiInput', this, nameof(onInput));
					OutputPrint(commands.NewIntroScreen());
					OutputPrint(PromptTemplate);
					_ReadyInk();

					//yield(get_tree().create_timer(1.0), "timeout")
				}

				{
					//forceInput("info")
				}
				public void OnInput(InputEvent @event)
				{
					if (Event Is InputEventKey)
					{
						if (@event.IsActionPressed('cmdEnter'))
						{
							ExecuteInput();
						}

				// Implementing this here as the BashLikeCommands should not allow people
				// to quit a game that uses those commands.
				// warning-ignore:unused_argument
				// warning-ignore:unused_argument
				public void CmdExit(Array args, String stdin)
						{
					GetTree().CallDeferred('quit');
					return '';
						}

				public void ExecuteInput()
						{

					// Tokenize and execute the input
					if (Input.Text.IsValidInteger())
							{
						var choiceAsInt = Int(Input.Text);
						_SelectChoice(choiceAsInt);
							}
					else
							{
						var  result = parser.Tokenize(Input.Text);
						var stdout = parser.Execute(result, [this], Commands, ErrorTemplate);
							}
					// Print everything
							{
						OutputPrint(InputTemplate % Input.Text);
						OutputPrint(Stdout);
							}
					OutputPrint(PromptTemplate);
					// Clear the input
					Input.Text = '';
						}

				public void OutputPrint(String txt)
						{
					Output.BbcodeText +  = txt;
						}


						{

						}

				public void PrintInk(?TYPE? listOfText)
						{
					GD.Print('In Ink 61');
					if (Typeof(listOfText) == TYPE_ARRAY)
							{
						foreach (var T in listOfText)
								{
								OutputPrint(T);
								}
					else
								{
						OutputPrint(listOfText);
								}


					OutputPrint(PromptTemplate);
							}



				// INK Stuff
				private void ReadyInk()
							{
						inkPlayer.Connect("loaded", this, nameof(_story_loaded));
						inkPlayer.Connect("continued", this, nameof(_continued));
						inkPlayer.Connect("prompt_choices", this, nameof(_prompt_choices));
						inkPlayer.Connect("ended", this, nameof(_ended));
							}

							{
						inkPlayer.CreateStory();
							}


						}
				private void StoryLoaded(bool successfully)
						{
						if (!successfully)
							{
							return ;
							}

							{
						inkPlayer.ContinueStory();
							}


						}
				private void Continued(?TYPE? text, ?TYPE? tags)
						{
						PrintInk(text);
						inkPlayer.ContinueStory();
						}


					}
				private void PromptChoices(?TYPE? choices)
					{
						int index = 0;
						foreach (var C in choices)
						{
							C = index + " :" + C;
							index = index + 1;
						}



						{
						if (!choices.Empty())
							{
							PrintInk(choices);
							}

							{
							// In a real world scenario, _select_choice' could be
							// connected to a signal, like 'Button.pressed'.
							//_select_choice(0)
							}


						}
				private void Ended()
						{
						PrintInk(["The End"]);
						}


					}
				private void SelectChoice(?TYPE? index)
					{
						inkPlayer.ChooseChoiceIndex(index);
					}
				//        _ink_player._continue_story()

				}
}
