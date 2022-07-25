extends Control

onready var _ink_player = $InkPlayer

export var prompt_template := '\n[color=#66aaff]godot@terminal:~ [b]$[/b][/color] '
export var input_template := '[color=#ffffff]%s[/color]'
export var error_template := '\n[color=#dd0000][ERROR] %s[/color]'

var parser := CommandParser.new()
var commands := BashLikeCommands.new()

onready var output := $Output as RichTextLabel
onready var input := $Input as LineEdit


func _ready() -> void:
	#output_print(prompt_template)
	input.grab_focus()
	input.connect('gui_input', self, 'on_input')
	output_print(commands.newIntroScreen())
	output_print(prompt_template)	
	_readyInk()
	
	#yield(get_tree().create_timer(1.0), "timeout")

	#forceInput("info")
func on_input(event: InputEvent):
	if event is InputEventKey:
		if event.is_action_pressed('cmd_enter'):
			execute_input()

# Implementing this here as the BashLikeCommands should not allow people
# to quit a game that uses those commands.
# warning-ignore:unused_argument
# warning-ignore:unused_argument
func cmd_exit(args: Array, stdin: String):
	get_tree().call_deferred('quit')
	return ''

func execute_input():
	
	# Tokenize and execute the input
	if	input.text.is_valid_integer():
		print('trying with int')
		var choiceAsInt = int(input.text)  
		_select_choice(choiceAsInt)
	else:
		var result := parser.tokenize(input.text)
		var stdout := parser.execute(result, [self, commands], error_template)	
	# Print everything
		output_print(input_template % input.text)
		output_print(stdout)
	output_print(prompt_template)	
	# Clear the input
	input.text = ''

func output_print(txt: String, addLine: bool = false):
	output.bbcode_text += txt 
	if addLine:
		output.bbcode_text += '\n'


	

func print_ink(listOfText):

	output_print(listOfText)	
	output_print(prompt_template)	



# INK Stuff 
func _readyInk():
		_ink_player.connect("loaded", self, "_story_loaded")
		_ink_player.connect("continued", self, "_continued")
		_ink_player.connect("prompt_choices", self, "_prompt_choices")
		_ink_player.connect("ended", self, "_ended")
	
		_ink_player.create_story()
	
	
func _story_loaded(successfully: bool):
		if !successfully:
			return
	
		_ink_player.continue_story()
	
	
func _continued(text, tags):
		print_ink(text)
		_ink_player.continue_story()
	
	
func _prompt_choices(choices):
		var index = 0 ; 
		output_print("Select", true )
		for c in choices :
			c = str(index) + ": " +  c + '\n'
			index = index + 1 
			output_print(c)



	
	
			# In a real world scenario, _select_choice' could be
			# connected to a signal, like 'Button.pressed'.
			#_select_choice(0)
	
	
func _ended():
		print_ink(["The End"])
	
	
func _select_choice(index):
		_ink_player.choose_choice_index(index)
#		_ink_player._continue_story()
