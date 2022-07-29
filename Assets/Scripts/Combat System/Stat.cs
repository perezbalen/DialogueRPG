using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stat", menuName = "Stat")]
public class Stat : ScriptableObject
{
    public string statName;
    public string shortName; //Weird por WILL. 
    public Color colorMain;
    public Color colorAccent;
    [TextArea]
    public string description; //Para poner en el tooltip
    public Sprite combatTooltipBackground; //me imagino lo que va detras del tooltip.
    public Sprite buttonOverlayBackground; //de pronto, una imagen para poner como botón de selección de accion 
}
