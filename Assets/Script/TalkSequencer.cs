using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TalkSequencer : MonoBehaviour {
    [SerializeField] SequenceAction[] linearSequenceAction;
    [SerializeField] ABSequenceAction[] AB;
    [SerializeField] TextMeshProUGUI subtitleUGUI;
    [SerializeField] GameObject subtitlePanal;
    [SerializeField] AudioSource source;
    public List<BindSignToString> signatures;
    public int currentPlayingLinearSequence { get; private set; }
    public int currentPlayingABSequence { get; private set; }
    public enum Option { A, B }
    CancellationTokenSource talkTokenSource;
    [HideInInspector] public bool IsPlaying { get; private set; }
    private void Awake() {
        currentPlayingLinearSequence = -1;
        subtitlePanal.SetActive(false);
        talkTokenSource = new();
    }
    public async void PlayLinearSequence(int index) {
        try {
            IsPlaying = true;
            if (index >= linearSequenceAction.Length) index = linearSequenceAction.Length - 1;
            subtitlePanal.SetActive(true);
            currentPlayingLinearSequence = index;

            talkTokenSource.Cancel();
            talkTokenSource.Dispose();
            talkTokenSource = new();
            CancellationToken token = talkTokenSource.Token;

            for (int i = 0; i < linearSequenceAction[currentPlayingLinearSequence].Sequence.Talks.Length; i++) {
                source.clip = linearSequenceAction[currentPlayingLinearSequence].Sequence.Talks[i].Clip;
                source.Play();
                if (linearSequenceAction[currentPlayingLinearSequence].Sequence.Talks[i].NeedSignature) {
                    subtitleUGUI.text = ReplaceSignWith(linearSequenceAction[currentPlayingLinearSequence].Sequence.Talks[i].Subtitle);
                }
                else subtitleUGUI.text = linearSequenceAction[currentPlayingLinearSequence].Sequence.Talks[i].Subtitle;


                for (int j = 0; j < linearSequenceAction[currentPlayingLinearSequence].Actions.Length; j++) {
                    if (linearSequenceAction[currentPlayingLinearSequence].Actions[j].Order == SequencerMethodReference.AcionInvokeOrder.Start) {
                        linearSequenceAction[currentPlayingLinearSequence].Actions[j].Invoke();
                    }
                }

                await Awaitable.WaitForSecondsAsync(linearSequenceAction[currentPlayingLinearSequence].Sequence.Talks[i].Clip.length +
                    linearSequenceAction[currentPlayingLinearSequence].Sequence.Talks[i].Delay, token);
            }
            subtitlePanal.SetActive(false);
            IsPlaying = false;
        }
        catch (OperationCanceledException) { }
        finally {
            for (int j = 0; j < linearSequenceAction[currentPlayingLinearSequence].Actions.Length; j++) {
                if (linearSequenceAction[currentPlayingLinearSequence].Actions[j].Order == SequencerMethodReference.AcionInvokeOrder.End) {
                    linearSequenceAction[currentPlayingLinearSequence].Actions[j].Invoke();
                }
            }
        }
    }
    public void PlayNextLinearSequence() {
        PlayLinearSequence(currentPlayingLinearSequence + 1);
    }
    public void PlayAgainLinearSequence() {
        PlayLinearSequence(currentPlayingLinearSequence);
    }
    public async void PlayABSequence(int index, Option option) {
        try {
            IsPlaying = true;
            if (index >= AB.Length) index = AB.Length - 1;
            subtitlePanal.SetActive(true);
            currentPlayingABSequence = index;

            talkTokenSource.Cancel();
            talkTokenSource.Dispose();
            talkTokenSource = new();
            CancellationToken token = talkTokenSource.Token;
            switch (option) {
                case Option.A:
                    for (int i = 0; i < AB[currentPlayingABSequence].A.Sequence.Talks.Length; i++) {
                        source.clip = AB[currentPlayingABSequence].A.Sequence.Talks[i].Clip;
                        source.Play();
                        if (AB[currentPlayingABSequence].A.Sequence.Talks[i].NeedSignature) {
                            subtitleUGUI.text = ReplaceSignWith(AB[currentPlayingABSequence].A.Sequence.Talks[i].Subtitle);
                        }
                        else subtitleUGUI.text = AB[currentPlayingABSequence].A.Sequence.Talks[i].Subtitle;

                        for (int j = 0; j < AB[currentPlayingABSequence].A.Actions.Length; j++) {
                            if (AB[currentPlayingABSequence].A.Actions[j].Order == SequencerMethodReference.AcionInvokeOrder.Start) {
                                AB[currentPlayingABSequence].A.Actions[j].Invoke();
                            }
                        }

                        await Awaitable.WaitForSecondsAsync(AB[currentPlayingABSequence].A.Sequence.Talks[i].Clip.length +
                            AB[currentPlayingABSequence].A.Sequence.Talks[i].Delay, token);
                    }
                    break;
                case Option.B:
                    for (int i = 0; i < AB[currentPlayingABSequence].B.Sequence.Talks.Length; i++) {
                        source.clip = AB[currentPlayingABSequence].B.Sequence.Talks[i].Clip;
                        source.Play();
                        if (AB[currentPlayingABSequence].B.Sequence.Talks[i].NeedSignature) {
                            subtitleUGUI.text = ReplaceSignWith(AB[currentPlayingABSequence].B.Sequence.Talks[i].Subtitle);
                        }
                        else subtitleUGUI.text = AB[currentPlayingABSequence].B.Sequence.Talks[i].Subtitle;

                        for (int j = 0; j < AB[currentPlayingABSequence].B.Actions.Length; j++) {
                            if (AB[currentPlayingABSequence].B.Actions[j].Order == SequencerMethodReference.AcionInvokeOrder.Start) {
                                AB[currentPlayingABSequence].B.Actions[j].Invoke();
                            }
                        }

                        await Awaitable.WaitForSecondsAsync(AB[currentPlayingABSequence].B.Sequence.Talks[i].Clip.length +
                            AB[currentPlayingABSequence].B.Sequence.Talks[i].Delay, token);
                    }

                    break;
            }
            subtitlePanal.SetActive(false);
            IsPlaying = false;
        }
        catch (OperationCanceledException) { }
        finally {
            switch (option) {
                case Option.A:
                    for (int j = 0; j < AB[currentPlayingABSequence].A.Actions.Length; j++) {
                        if (AB[currentPlayingABSequence].A.Actions[j].Order == SequencerMethodReference.AcionInvokeOrder.End) {
                            AB[currentPlayingABSequence].A.Actions[j].Invoke();
                        }
                    }
                    break;
                case Option.B:
                    for (int j = 0; j < AB[currentPlayingABSequence].B.Actions.Length; j++) {
                        if (AB[currentPlayingABSequence].B.Actions[j].Order == SequencerMethodReference.AcionInvokeOrder.End) {
                            AB[currentPlayingABSequence].B.Actions[j].Invoke();
                        }
                    }
                    break;
            }
        }
    }
    public void PlayNextABSequence(Option option) {
        PlayABSequence(currentPlayingABSequence, option);
    }
    public void PlayAgainABSequence(Option option) {
        PlayABSequence(currentPlayingABSequence, option);
    }
    private string ReplaceSignWith(string text) {
        string newTest = "";
        bool signSets = false;
        foreach (char item in text) {
            signSets = false;
            foreach (var sign in signatures) {
                if (item == sign.Sign) {
                    newTest += $"<color=red>{sign.String}</color>";
                    signSets = true;
                    continue;
                }
            }
            if (!signSets) newTest += item;
        }
        return newTest;
    }
}
[Serializable]
public class ABSequenceAction {
    public SequenceAction A;
    public SequenceAction B;
}
[Serializable]
public class SequenceAction {
    public TalkSequence Sequence;
    public SequencerMethodReference[] Actions;
}
[Serializable]
public class BindSignToString {
    public Char Sign;
    public string String;
    public BindSignToString(char sign, string toString) {
        Sign = sign;
        String = toString;
    }
}
