using System;
using System.Collections.Generic;
using buildings;
using UnityEngine;
using UnityEngine.UI;
using peasants;

public class WorkManager : MonoBehaviour
{
    public Transform peasantAppearancePoint;


    private int _availableCounter;
    private Dictionary<Profession, int> _requiredCounters;
    private Dictionary<Profession, int> _assignedCounters;

    static class UILib
    {
        public static Profession GetProfByInputName(string name)
        {
            switch (name)
            {
                case "PeasantProfInputMiner": return Profession.Miner;
                case "PeasantProfInputBuilder": return Profession.Builder;
                case "PeasantProfInputLumberjack": return Profession.Lumberjack;
                case "PeasantProfInputFisherman": return Profession.Fisherman;
            }
            return Profession.None;
        }

        public static string GetInputNameByProf(Profession prof)
        {
            switch (prof)
            {
                case Profession.Miner: return "PeasantProfInputMiner";
                case Profession.Builder: return "PeasantProfInputBuilder";
                case Profession.Lumberjack: return "PeasantProfInputLumberjack";
                case Profession.Fisherman: return "PeasantProfInputFisherman";
            }
            return null;
        }

        public static string GetLabelNameByProf(Profession prof)
        {
            switch (prof)
            {
                case Profession.None: return "CurPeasantsProfessionsNone";
                case Profession.Miner: return "CurPeasantsProfessionsMiner";
                case Profession.Builder: return "CurPeasantsProfessionsBuilder";
                case Profession.Lumberjack: return "CurPeasantsProfessionsLumberjack";
                case Profession.Fisherman: return "CurPeasantsProfessionsFisherman";
            }
            return null;
        }
    }

    void Awake()
    {
        InitDictionaries();
    }

    void Start()
    {
        CalcRequiredCounters();
        ApplyCounters();
    }

    private void InitDictionaries()
    {
        _requiredCounters = new Dictionary<Profession, int>();
        _assignedCounters = new Dictionary<Profession, int>();

        foreach (Profession prof in Enum.GetValues(typeof(Profession)))
        {
            if (prof == Profession.None) continue;

            _requiredCounters[prof] = 0;
            _assignedCounters[prof] = 0;
        }
    }

    public void CalcRequiredCounters()
    {
        if(_requiredCounters == null)
            InitDictionaries();

        var r = GameManager.Instance.BuildingManager.GetRequiredWorkers();
        foreach (Profession k in Enum.GetValues(typeof(Profession)))
        {
            if (r.ContainsKey(k))
                _requiredCounters[k] = r[k];
            else
                _requiredCounters[k] = 0;
        }
    }

    public void ApplyCounters(Profession prof)
    {
        _availableCounter = GameManager.Instance.PeasantManager.peasants.Count;

        foreach (var kv in _assignedCounters)
        {
            _availableCounter -= kv.Value;
        }

        GameManager.Instance.PeasantManager.AssignWork(prof, _assignedCounters[prof]);

        UpdateInputs();

        UpdateLabels();
    }

    public void ApplyCounters()
    {
        _availableCounter = GameManager.Instance.PeasantManager.peasants.Count;

        foreach (var kv in _assignedCounters)
        {
            _availableCounter -= kv.Value;
            GameManager.Instance.PeasantManager.AssignWork(kv.Key, kv.Value);
        }

        UpdateInputs();

        UpdateLabels();
    }

    public void ReAssignByWorkplace(WorkBuilding b)
    {
        b.ReleaseWorkers();
        GameManager.Instance.WorkManager.CalcRequiredCounters();
        GameManager.Instance.WorkManager.ApplyCounters();
    }

    public void OnChangeInput(string type)
    {
        GameObject input = GameObject.Find(type);
        var val = (int)uint.Parse(input.GetComponent<InputField>().text);
        var maxVal = _availableCounter + _assignedCounters[UILib.GetProfByInputName(type)];

        if (val > maxVal) 
            val = maxVal;

        _assignedCounters[UILib.GetProfByInputName(type)] = val;

        ApplyCounters(UILib.GetProfByInputName(type));
    }

    public void OnClickCreatePeasant()
    {
        GameManager.Instance.PeasantManager.Create(peasantAppearancePoint.position);
        _availableCounter++;

        UpdateLabels();
    }

    void UpdateLabels()
    {
        foreach (Profession p in Enum.GetValues(typeof(Profession)))
        {
            if(p == Profession.None) continue;

            GameObject.Find(UILib.GetLabelNameByProf(p)).GetComponent<Text>().text = _requiredCounters[p].ToString();
        }

        GameObject.Find(UILib.GetLabelNameByProf(Profession.None)).GetComponent<Text>().text = _availableCounter.ToString();
    }

    private void UpdateInputs()
    {
        foreach (Profession prof in Enum.GetValues(typeof(Profession)))
        {
            if (prof == Profession.None) continue;

            var g = GameObject.Find(UILib.GetInputNameByProf(prof));
            if (g != null)
            {
                g.GetComponent<InputField>().text = _assignedCounters[prof].ToString();
            }
        }
    }
}
