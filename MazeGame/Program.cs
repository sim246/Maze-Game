using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

public class OpenFileDialogForm : Form
{
    [STAThread]
    public static void Main()
    {
        var game = new MazeGame.MazeGame();
        game.Run();
    }
}