using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows.Speech;

public class SpeechManager : Singleton<SpeechManager>
{
    [System.Serializable]
    public struct KeywordAndResponse
    {
        public string Keyword;
        public UnityEvent Response;
    }

    public KeywordAndResponse[] KeywordsAndResponses;
    private KeywordRecognizer keywordRecognizer;
    private readonly Dictionary<string, UnityEvent> responses = new Dictionary<string, UnityEvent>();

    private void Start()
    {
        int keywordCount = KeywordsAndResponses.Length;
        if (keywordCount == 0)
            return;
        string[] keywords = new string[keywordCount];
        for (int index = 0; index != keywordCount; ++index)
        {
            keywords[index] = KeywordsAndResponses[index].Keyword;
            responses[keywords[index]] = KeywordsAndResponses[index].Response;
        }
        keywordRecognizer = new KeywordRecognizer(keywords);
        keywordRecognizer.OnPhraseRecognized += keywordRecognizer_OnRecognized;

        keywordRecognizer.Start();

    }

    private void keywordRecognizer_OnRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Detected args " + args.text);
        UnityEvent unityEvent;
        if (responses.TryGetValue(args.text, out unityEvent))
            unityEvent.Invoke();
    }

    private void OnDestroy()
    {
        keywordRecognizer.Stop();
        keywordRecognizer.OnPhraseRecognized -= keywordRecognizer_OnRecognized;
        keywordRecognizer.Dispose();
    }
}
