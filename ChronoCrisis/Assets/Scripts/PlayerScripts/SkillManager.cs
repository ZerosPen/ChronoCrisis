using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public Skill[] skillSlots = new Skill[4];
    private PlayerController playerController;
    private int? indexActiveSkill = null;
    [SerializeField]  private bool isAiming = false;

    private Dictionary<Skill, float> skillCooldowns = new Dictionary<Skill, float>();

    [SerializeField] private GameObject aoeIndicatorPrefab;
    [SerializeField] private GameObject singelTargetIndicatorPrefab;
    [SerializeField] private Transform skillIndicatorContainer;
    private GameObject aoeIndicatorInstance;
    private GameObject singelTargetInstance;
    private PointRatatioAction pra;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>(); // Finds the player object
        pra = FindObjectOfType<PointRatatioAction>();

        // Initialize cooldown tracking
        foreach (Skill skill in skillSlots)
        {
            if (skill != null)
                skillCooldowns[skill] = 0;
        }
    }

    private void Update()
    {
        HandleSkillUse();
        UpdateCooldowns();
        if (indexActiveSkill.HasValue)
        {
            indexActiveSkill = indexActiveSkill.Value;
        }
        if (isAiming && aoeIndicatorInstance != null)
        {
            aoeIndicatorInstance.transform.position = GetMouseWorldPosition();
        }

        if (isAiming && singelTargetInstance != null)
        {
            singelTargetInstance.transform.position = GetMouseWorldPosition();
        }
    }

    public void HandleSkillSwitching(int indexSkill)
    {
        if (indexSkill >= 0 && indexSkill < skillSlots.Length && skillSlots[indexSkill] != null)
        {
            indexActiveSkill = indexSkill;
            Debug.Log($"skill selected : {indexActiveSkill.HasValue}");
        }
    }

    void HandleSkillUse()
    {
        if(Input.GetMouseButtonDown(0) && indexActiveSkill.HasValue && playerController.ActiveSkill == true)
        {
            Skill activeSkill = skillSlots[indexActiveSkill.Value];
            if (skillCooldowns[activeSkill] > 0)
            {
                Debug.Log(activeSkill.skillName + " is on cooldown: " + skillCooldowns[activeSkill] + "s left.");
                return;
            }

            Debug.Log("Aiming");
            isAiming = true;
            startUseSkill(activeSkill.typeSkill);
            return;
        }
        else if (isAiming && Input.GetMouseButtonUp(0) && playerController.ActiveSkill == true)
        {
            Skill activeSkill = skillSlots[indexActiveSkill.Value];

            activeSkill.useSkill(playerController.gameObject);
            skillCooldowns[activeSkill] = activeSkill.coolDown; // Start cooldown
            playerController.ActiveSkill = false;
            StartCoroutine(playerController.CoolDownChangeSkill());

            isAiming = false;
            pra.isSkillActive = false;
            indexActiveSkill = null;
        }
    }
    void UpdateCooldowns()
    {
        List<Skill> keys = new List<Skill>(skillCooldowns.Keys);
        foreach (Skill skill in keys)
        {
            if (skillCooldowns[skill] > 0)
            {
                skillCooldowns[skill] -= Time.deltaTime;
            }
        }
    }

    void startUseSkill(string type)
    {
        Debug.Log("startUseSkill called with type: " + type);

        if (type == "AoE" && aoeIndicatorInstance == null && aoeIndicatorPrefab != null && isAiming == true)
        {
            pra.isSkillActive = true;
            aoeIndicatorInstance = Instantiate(aoeIndicatorPrefab, skillIndicatorContainer);
            Debug.Log("AoE Prefab Instantiated");
            aoeIndicatorInstance.SetActive(true);
            aoeIndicatorInstance.transform.position = GetMouseWorldPosition();
        }

        if ((type == "SingelTarget" || type == "Singel") && singelTargetInstance == null && singelTargetIndicatorPrefab != null && isAiming == true)
        {
            singelTargetInstance = Instantiate(singelTargetIndicatorPrefab, skillIndicatorContainer);
            Debug.Log("Single Target Prefab Instantiated");
            singelTargetInstance.SetActive(true);
            singelTargetInstance.transform.position = GetMouseWorldPosition();
        }

        // Prevent setting inactive if instances aren't created
        if (!isAiming)
        {
            if (aoeIndicatorInstance != null) 
            {
                aoeIndicatorInstance.SetActive(false);
                Destroy(aoeIndicatorInstance);
            }
            if (singelTargetInstance != null) { 
                singelTargetInstance.SetActive(false); 
                Destroy(singelTargetInstance);
            }
        }
    }


    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(mousePosition.x, mousePosition.y, 0f); // Force Z to 0
    }

}
