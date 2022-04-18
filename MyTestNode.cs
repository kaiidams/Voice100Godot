using Godot;
using System;
using System.Runtime.InteropServices;
using Voice100;

public class MyTestNode : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    private SpeechSynthesizer _speechSynthesizer;
    private AudioStreamGeneratorPlayback _playback;
    private short[] _waveData;
    private int _waveIndex;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (_speechSynthesizer == null)
        {
            _speechSynthesizer = new SpeechSynthesizer(
                "ttsalign_en_conv_base-20220409.onnx",
                "ttsaudio_en_mt_conv_base-20220316.onnx",
                true);
        }

        var button = GetNode<Button>("Button");
        button.Connect("pressed", this, "OnClick");

        var textEdit = GetNode<TextEdit>("TextEdit");
        textEdit.Text = "Hello, I am a rocket.";
    }

    public void OnClick()
    {
        var player = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        _playback = player.GetStreamPlayback() as AudioStreamGeneratorPlayback;
        (player.Stream as AudioStreamGenerator).MixRate = 16000;

        var textEdit = GetNode<TextEdit>("TextEdit");
        string text = textEdit.Text;

        string[] phonemes;
        _speechSynthesizer.Speak(text, out _waveData, out phonemes);
        _waveIndex = 0;
        string phoneText = string.Join("/", phonemes);

        player.Play();
        Console.WriteLine("waveData.Length: {0}", _waveData.Length);
        Console.WriteLine("phonemes: {0}", text);
        var textEdit2 = GetNode<TextEdit>("TextEdit2");
        textEdit2.Text = phoneText;
    }

    public override void _ExitTree()
    {
        if (_speechSynthesizer != null)
        {
            _speechSynthesizer.Dispose();
            _speechSynthesizer = null;
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (_playback != null)
        {
            var toFill = Math.Min(_waveData.Length - _waveIndex, _playback.GetFramesAvailable());

            if (toFill > 0)
            {
                Console.WriteLine("{0}/{1}", 0, _playback.GetFramesAvailable());
                var buffer = new Vector2[toFill];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = (Vector2.One / short.MaxValue) * _waveData[_waveIndex + i];
                }
                _playback.PushBuffer(buffer);
                _waveIndex += toFill;
            }
        }
    }
}
