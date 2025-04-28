using CommandLine;
using NetCoreAudio;
using SharpHook.Native;

public class Options
{
	[Option('m', "male", Default = false, SetName = "sex")]
	public bool Male { get; set; }

	[Option('f', "female", Default = false, SetName = "sex")]
	public bool Female { get; set; }

	private int _voice;

	[Option(Default = 0, Required = false)]
	public int Voice
	{
		get
		{
			if (_voice == 0)
			{
				return Random.Shared.Next(1, 5);
			}
			else
			{
				return _voice;
			}
		}
		set
		{
			if (value >= 1 && value <= 4)
			{
				_voice = value;
			}
		}
	}
}

class TestClass
{
	static List<KeyCode> keysInUse = new List<KeyCode>();

	public static string audioRoot = Path.Join("assets", "audio");
	public static string sfxRoot = Path.Join(audioRoot, "sfx");
	public static string animaleseRoot = Path.Join(audioRoot, "animalese");
	public static string vocalsRoot = Path.Join(audioRoot, "vocals");
	public static string? animalesePath;
	public static string? vocalsPath;

	static void HandleParsedCommandArgs(Options parsedArgs)
	{
		string? result;
		if (parsedArgs.Male)
		{
			result = "male";
		}
		else if (parsedArgs.Female)
		{
			result = "female";
		}
		else
		{
			return;
		}
		animalesePath = Path.Join(animaleseRoot, result, $"voice_{parsedArgs.Voice}");
		vocalsPath = Path.Join(vocalsRoot, result, $"voice_{parsedArgs.Voice}");
	}

	static void Main(string[] args)
	{

		Parser.Default.ParseArguments<Options>(args).WithParsed(HandleParsedCommandArgs);

		if (animalesePath == null || vocalsPath == null)
		{
			return;
		}

		UioHook.SetDispatchProc((SharpHook.Native.DispatchProc)((ref UioHookEvent eventArgs, nint userData) =>
		{
			if (eventArgs.Type == EventType.KeyPressed)
			{
				if (keysInUse.Contains(eventArgs.Keyboard.KeyCode)) return;

				keysInUse.Add(eventArgs.Keyboard.KeyCode);

				string? soundToPlay = GetSoundForKey(eventArgs.Keyboard.KeyCode, eventArgs.Mask.HasShift());

				if (soundToPlay != null)
				{
					new Player().Play(soundToPlay);
				}
				// Console.WriteLine($"Pressed {eventArgs.Keyboard.KeyCode}");
			}
			else if (eventArgs.Type == EventType.KeyReleased)
			{
				if (keysInUse.Contains(eventArgs.Keyboard.KeyCode))
				{
					keysInUse.Remove(eventArgs.Keyboard.KeyCode);
				}
			}

		}), nint.Zero);

		while (true)
		{
			UioHook.RunKeyboard();
		}
	}

	public static string? GetSoundForKey(KeyCode key, bool holdingShift = false)
	{

		string? fileMatchingKey;

		if (holdingShift)
		{
			fileMatchingKey = key switch
			{
				KeyCode.Vc1 => "exclamation",
				KeyCode.Vc2 => "at",
				KeyCode.Vc3 => "pound",
				KeyCode.Vc4 => "dollar",
				KeyCode.Vc5 => "percent",
				KeyCode.Vc6 => "caret",
				KeyCode.Vc7 => "ampersand",
				KeyCode.Vc8 => "asterisk",
				KeyCode.Vc9 => "parenthesis_open",
				KeyCode.Vc0 => "parenthesis_closed",
				KeyCode.VcCloseBracket => "brace_closed",
				KeyCode.VcOpenBracket => "brace_open",
				KeyCode.VcBackQuote => "tilde",
				KeyCode.VcSlash => "question",
				_ => null
			};

			if (fileMatchingKey != null)
			{
				return Path.Join(sfxRoot, $"{fileMatchingKey}.wav");
			}
		}
		else
		{
			fileMatchingKey = key switch
			{
				KeyCode.Vc1 => "0",
				KeyCode.Vc2 => "1",
				KeyCode.Vc3 => "2",
				KeyCode.Vc4 => "3",
				KeyCode.Vc5 => "4",
				KeyCode.Vc6 => "5",
				KeyCode.Vc7 => "6",
				KeyCode.Vc8 => "7",
				KeyCode.Vc9 => "8",
				KeyCode.Vc0 => "9",
				KeyCode.VcMinus => "10",
				KeyCode.VcEquals => "11",
				_ => null
			};

			if (fileMatchingKey != null)
			{
				return Path.Join(vocalsPath, $"{fileMatchingKey}.wav");
			}
		}

		fileMatchingKey = key switch
		{
			KeyCode.VcA => "a",
			KeyCode.VcB => "b",
			KeyCode.VcC => "c",
			KeyCode.VcD => "d",
			KeyCode.VcE => "e",
			KeyCode.VcF => "f",
			KeyCode.VcG => "g",
			KeyCode.VcH => "h",
			KeyCode.VcI => "i",
			KeyCode.VcJ => "j",
			KeyCode.VcK => "k",
			KeyCode.VcL => "l",
			KeyCode.VcM => "m",
			KeyCode.VcN => "n",
			KeyCode.VcO => "o",
			KeyCode.VcP => "p",
			KeyCode.VcQ => "q",
			KeyCode.VcR => "r",
			KeyCode.VcS => "s",
			KeyCode.VcT => "t",
			KeyCode.VcU => "u",
			KeyCode.VcV => "v",
			KeyCode.VcW => "w",
			KeyCode.VcX => "x",
			KeyCode.VcY => "y",
			KeyCode.VcZ => "z",
			_ => null
		};

		if (fileMatchingKey != null)
		{
			return Path.Join(animalesePath, $"{fileMatchingKey}.wav");
		}

		fileMatchingKey = key switch
		{
			KeyCode.VcDown => "arrow_down",
			KeyCode.VcLeft => "arrow_left",
			KeyCode.VcRight => "arrow_right",
			KeyCode.VcUp => "arrow_up",
			KeyCode.VcBackspace => "backspace",
			KeyCode.VcCloseBracket => "bracket_closed",
			KeyCode.VcOpenBracket => "bracket_open",
			KeyCode.VcSpace => "default",
			KeyCode.VcEnter => "enter",
			KeyCode.VcBackslash => "slash_back",
			KeyCode.VcSlash => "slash_forward",
			KeyCode.VcTab => "tab",
			_ => null
		};

		if (fileMatchingKey != null)
		{
			return Path.Join(sfxRoot, $"{fileMatchingKey}.wav");
		}

		return null;
	}

}