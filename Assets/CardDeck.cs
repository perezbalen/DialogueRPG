using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    [Tooltip("You need to put the cards you are going to use here, in the game deck. Otherwise they wont load. (Also, the Tooltip in the response button needs this object.")]
    public List<Card> deck;
}
