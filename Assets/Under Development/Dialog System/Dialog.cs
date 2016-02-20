using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Conversation which can be triggered to display Text on the screen
/// </summary>
[RequireComponent(typeof(Canvas))]
public class Conversation : MonoBehaviour
{
    [Tooltip("The phrases to be used in the conversation")]
    public string[] Phrases;

    [Tooltip("The (Optional) Canvas on which to have this conversation")]
    public Canvas Canvas;

    [Tooltip("The (Optional) Text where the conversation will be drawn.  If a custom Text is used, it must be a child of this GameObject.")]
    public Text Text;

    [Tooltip("The (Optional) Image used as a background for the text.  If an image is used, it must be a child of this GameObject")]
    public Image Image;

    [Tooltip("Offset to use from the GameObject")]
    public Vector3 Offset = new Vector3(0, 2, 0);

    private string input;
    private float timeDelay;
    private bool resetConversationAtEnd;
    private int phraseCounter;
    private GameObject audience;

    public delegate bool CanCyclePhraseDelegate();
    public CanCyclePhraseDelegate CanCyclePhraseMethod;

    //private Vector3 DEFAULT_OFFSET_BY_SCALE = new Vector3(0, 2, 0);
    private Vector3 DEFAULT_SCALE = new Vector3(0.001f, 0.001f, 0.001f);
    private const string DEFAULT_FONT = "Arial.ttf";
    private const int DEFAULT_FONT_SIZE = 300;

    private void Update()
    {
        UpdateConversation();
        RotateIntoView();
    }

    /// <summary>
    /// Initiates the conversation where input is the name of the input used to continue to the conversation, seconds is the number of seconds waited until the conversation is continued, or 0 if none, resetConversation is whether or start a conversation over at the beginning when reinitiated, or to just say the last phrase again, canCyclePhrase is the method which returns a bool and determines if the conversation can continue, and audience is the position that the Text will be rotated towards
    /// </summary>
    /// <param name="input">The name of the input used to continue to the conversation</param>
    /// <param name="seconds">The number of seconds waited until the conversation is continued, or 0 if none</param>
    /// <param name="resetConversation">Whether or start a conversation over at the beginning when reinitiated, or to just say the last phrase again</param>
    /// <param name="canCyclePhrase">Method which returns a bool and determines if the conversation can continue</param>
    /// <param name="audience">GameObject that the text will rotate towards</param>
    public void InitiateConversation(string input, float seconds = 0, bool resetConversation = true, CanCyclePhraseDelegate canCyclePhrase = null, GameObject audience = null)
    {
        InitializeInputParameters(input, seconds, resetConversation, canCyclePhrase, audience);
        BeginConversation();
    }

    private void CreateTextField()
    {
        this.Canvas = GetComponent<Canvas>();
        Text text = transform.GetComponentInChildren<Text>();
        if (!text)
        {
            GameObject textGameObject = new GameObject("Text");
            textGameObject.transform.parent = transform;
            text = textGameObject.AddComponent<Text>();
            text.transform.position = Offset;
            text.transform.localScale = DEFAULT_SCALE;
            text.font = Resources.GetBuiltinResource(typeof(Font), DEFAULT_FONT) as Font;
            text.fontSize = DEFAULT_FONT_SIZE;
            ContentSizeFitter contentSizeFitter = textGameObject.AddComponent<ContentSizeFitter>();
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
        this.Text = text;
        this.Text.enabled = false;
        Image image = transform.GetComponentInChildren<Image>();
        if (image)
        {
            this.Image = image;
            this.Image.enabled = false;
            image.transform.position = Offset;
        }
    }

    private void InitializeInputParameters(string input, float miliseconds, bool resetConversation, CanCyclePhraseDelegate CanCyclePhrase, GameObject audience)
    {
        if (input == null && miliseconds <= 0)
        {
            Debug.Log("The input name and miliseconds to wait between phrases are null and 0.  One of these must be active in order for the Conversation system to work.");
            return;
        }
        if (Phrases == null || Phrases.Length == 0)
        {
            Debug.Log("There are no phrases available.");
            return;
        }
        this.input = input;
        this.timeDelay = miliseconds;
        this.resetConversationAtEnd = resetConversation;
        this.CanCyclePhraseMethod = CanCyclePhrase != null ? CanCyclePhrase : CanAlwaysCyclePhrase;
        this.audience = audience;
    }

    private void BeginConversation()
    {
        this.Text.enabled = true;
        this.Image.enabled = true;
        CycleNextPhrase();
        if (timeDelay > 0)
        {
            StartCoroutine(CyclePhrasesOverTime());
        }
    }

    private IEnumerator CyclePhrasesOverTime()
    {
        while (phraseCounter <= Phrases.Length)
        {
            yield return new WaitForSeconds(timeDelay);
            CycleNextPhrase(true);
        }
    }

    private void UpdateConversation()
    {
        if (input == null)
        {
            return;
        }
        try
        {
            if (Input.GetKeyDown(input))
            {
                CycleNextPhrase();
                return;
            }
        }
        catch
        {
            //The input was not a valid key
        }
        try
        {
            if (Input.GetButtonDown(input))
            {
                CycleNextPhrase();
                return;
            }
        }
        catch
        {
            //The input was not a valid mouse
            Debug.Log("These was no valid input name to cycle through the Conversation");
        }
    }

    private void RotateIntoView()
    {
        if (audience == null)
        {
            return;
        }
        Text.transform.forward = transform.position - audience.transform.position;
        if (Image)
        {
            Image.transform.forward = transform.position - audience.transform.position;
        }
    }

    private void CycleNextPhrase(bool automaticCycle = false)
    {
        if (!CanCyclePhraseMethod())
        {
            return;
        }
        if (phraseCounter < Phrases.Length)
        {
            this.Text.text = Phrases[phraseCounter++];
        }
        else if (phraseCounter == Phrases.Length)
        {
            this.Text.text = "";
            phraseCounter++;
        }
        else if (phraseCounter > Phrases.Length && !automaticCycle)
        {
            if (resetConversationAtEnd)
            {
                phraseCounter = 0;
                this.Text.text = Phrases[phraseCounter++];
            }
            else
            {
                this.Text.text = Phrases[Phrases.Length - 1];
                phraseCounter--;
            }
        }
    }

    private bool CanAlwaysCyclePhrase()
    {
        return true;
    }
}