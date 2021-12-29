using Godot;
using System;
using System.Runtime.InteropServices;

public class MyTestNode : Node
{
#if true
    const string DllName = "voice100_native";
#else
        const string DllName = "__Internal";
#endif

    [DllImport(DllName)]
    private static extern int Voice100Sharp_VocoderDecode(
        float[] f0, float[] logspc, float[] codedap, int f0_length,
        int fft_size, double frame_period, int fs, float log_offset, short[] y, int y_length);
    
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var f0 = new float[100];
        var codecp = new float[100 * 257];
        var logspc = new float[100];
        int l = Voice100Sharp_VocoderDecode(
            f0, logspc, codecp, 100, 512, 5.0, 16000, 1e-15f,
            null, 0);
        Console.WriteLine("test: {0}", l);
        var y = new short[l];
        Voice100Sharp_VocoderDecode(
            f0, logspc, codecp, 100, 512, 5.0, 16000, 1e-15f,
            y, l);
    }
}
