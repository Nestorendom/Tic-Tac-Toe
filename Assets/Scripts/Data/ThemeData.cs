using UnityEngine;

[CreateAssetMenu(fileName = "ThemeData", menuName = "TicTacToe/Theme Data")]
public class ThemeData : ScriptableObject
{
    public string themeName;

    [Header("Marks")]
    public Sprite xSprite;
    public Sprite oSprite;

    [Header("Board")]
    public Sprite boardBackground;
    public Sprite gridSprite;
}