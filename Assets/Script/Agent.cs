using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Agent : MonoBehaviour,IInteractable {
    [SerializeField] LightBase[] lights;
    [SerializeField] bool missionComplete;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform agentDestination;
    enum Mission { TurnDownTheLights, TurnOn2Light, FollowMe }
    Mission currentMission;
    Dictionary<Mission, Func<bool>> MissionToFunc;
    [SerializeField] TalkSequencer Sequencer;

    private void Awake() {
        missionComplete = true;
        MissionToFunc = new Dictionary<Mission, Func<bool>>();
        MissionToFunc.Add(Mission.TurnDownTheLights, TurnDownTheLightsCompletes);
        MissionToFunc.Add(Mission.TurnOn2Light, TurnOn2LightComplet);
        MissionToFunc.Add(Mission.FollowMe, FollowMeComplet);
    }
    public void OnInteract() {
        if (!missionComplete) {
            if (MissionToFunc[currentMission].Invoke()) {
                missionComplete = true;
                currentMission += 1;
            }
            else {
                Sequencer.PlayAgainLinearSequence();
                return;
            }
        }
        Sequencer.PlayNextLinearSequence();
    }
    public void SelectNo() {
        Sequencer.PlayNextABSequence(TalkSequencer.Option.B);
    }
    public void SelectYes() {
        Sequencer.PlayNextABSequence(TalkSequencer.Option.A);
    }
    public void NeedTOCompletAMisson() {
        missionComplete = false;
    }
    bool TurnOn2LightComplet() {
        int litLight = 0;
        foreach (LightBase light in lights) {
            if (light.isOn) {
                litLight++;
            }
        }
        if (litLight == 2) return true;
        else return false;
    }
    bool TurnDownTheLightsCompletes() {
        bool isAllOF = true;
        foreach (LightBase light in lights) {
            if (light.isOn) {
                isAllOF = false;
                break;
            }
        }
        return isAllOF;
    }
    bool FollowMeComplet() {
        if (agent.remainingDistance <= agent.stoppingDistance) {
            return true;
        }
        else return false;
    }
    public void MoveToDestination() {
        if (!missionComplete) return;
        agent.SetDestination(agentDestination.position);
    }
    public void GetOutOfPlayMode() {
        EditorApplication.isPlaying = false;
    }
}
