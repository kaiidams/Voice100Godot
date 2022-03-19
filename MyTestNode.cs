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
                "ttsalign_en_conv_base-20210808.onnx",
                "ttsaudio_en_mt_conv_base-20220316.onnx",
                true);
        }

        var button = GetChild(1) as Button;
        button.Connect("pressed", this, "OnClick");
    }

    public void OnClick()
    {
        var player = GetChild(0) as AudioStreamPlayer;
        _playback = player.GetStreamPlayback() as AudioStreamGeneratorPlayback;
        (player.Stream as AudioStreamGenerator).MixRate = 16000;
        byte[] byteData;
        string text = null;

        string[] phonemes;
        _speechSynthesizer.Speak("Hello, I am a rocket.", out byteData, out phonemes);
        _waveData = MemoryMarshal.Cast<byte, short>(byteData).ToArray();
        _waveIndex = 0;
        text = string.Join("/", phonemes);

        player.Play();
        Console.WriteLine("waveData.Length: {0}", _waveData.Length);
        Console.WriteLine("phonemes: {0}", text);
        var textEdit = GetChild(2) as TextEdit;
        textEdit.Text = text;
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
