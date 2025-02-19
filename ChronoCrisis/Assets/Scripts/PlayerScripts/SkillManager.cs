using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public Skill[] skillSlots = new Skill[4];
    private PlayerController playerController;
    private int? indexActiveSkill = null;
    [SerializeField] private bool isAiming = false;

    private Dictionary<Skill, float> skillCooldowns = new Dictionary<Skill, float>();

    [SerializeField] private GameObject aoeIndicatorPrefab;
    [SerializeField] private GameObject singleTargetIndicatorPrefab;
    [SerializeField] private Transform skillIndicatorContainer;
    [SerializeField] private Transform weaponTransform; // Assign this in the Inspector

    private Quaternion originalWeaponRotation;
    private GameObject aoeIndicatorInstance;
    private GameObject singleTargetInstance;
    private PointRatatioAction pra;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        pra = FindObjectOfType<PointRatatioAction>();

        // Initialize cooldown tracking
        foreach (Skill skill in skillSlots)
        {
            if (skill != null)
                skillCooldowns[skill] = 0;
        }
    }

    void Update()
    {
        HandleSkillUse();
        UpdateCooldowns();

        if (isAiming && pra.isSkillActive && indexActiveSkill.HasValue)
        {
            Skill activeSkill = skillSlots[indexActiveSkill.Value];

            if (activeSkill.typeSkill == "AoE" && aoeIndicatorInstance != null)
            {
                aoeIndicatorInstance.transform.position = pra.GetCursorPosition();
            }
            else if ((activeSkill.typeSkill == "SingelTarget" || activeSkill.typeSkill == "Singel") && singleTargetInstance != null)
            {
                Vector3 cursorPosition = pra.GetCursorPosition();
                Vector3 direction = cursorPosition - weaponTransform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                singleTargetInstance.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }
    }

    public void HandleSkillSwitching(int indexSkill)
    {
        if (indexSkill >= 0 && indexSkill < skillSlots.Length && skillSlots[indexSkill] != null)
        {
            indexActiveSkill = indexSkill;
            Debug.Log($"Skill selected: {indexActiveSkill.Value}");
        }
    }

    void HandleSkillUse()
    {
        if (Input.GetMouseButtonDown(0) && indexActiveSkill.HasValue && playerController.ActiveSkill)
        {
            Skill activeSkill = skillSlots[indexActiveSkill.Value];
            if (skillCooldowns[activeSkill] > 0)
            {
                Debug.Log(activeSkill.skillName + " is on cooldown: " + skillCooldowns[activeSkill] + "s left.");
                return;
            }

            isAiming = true;
            StartUseSkill(activeSkill.typeSkill);
            return;
        }
        else if (isAiming && Input.GetMouseButtonUp(0) && playerController.ActiveSkill)
        {
            Skill activeSkill = skillSlots[indexActiveSkill.Value];

            activeSkill.useSkill(playerController.gameObject);
            skillCooldowns[activeSkill] = activeSkill.coolDown;
            playerController.ActiveSkill = false;
            StartCoroutine(playerController.CoolDownChangeSkill());

            isAiming = false;
            pra.isSkillActive = false;
            indexActiveSkill = null;

            DestroyIndicators();
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

    void StartUseSkill(string type)
    {
        Debug.Log("StartUseSkill called with type: " + type);

        if (weaponTransform != null)
        {
            originalWeaponRotation = weaponTransform.rotation;
        }

        if (type == "AoE" && aoeIndicatorInstance == null && aoeIndicatorPrefab != null && isAiming)
        {
            pra.isSkillActive = true;
            aoeIndicatorInstance = Instantiate(aoeIndicatorPrefab, skillIndicatorContainer);
            aoeIndicatorInstance.SetActive(true);
            aoeIndicatorInstance.transform.position = pra.GetCursorPosition();
        }
        else if ((type == "SingelTarget" || type == "Singel") && singleTargetInstance == null && singleTargetIndicatorPrefab != null && isAiming)
        {
            singleTargetInstance = Instantiate(singleTargetIndicatorPrefab, skillIndicatorContainer);
            singleTargetInstance.SetActive(true);
            singleTargetInstance.transform.position = weaponTransform.position;
        }
    }

    private void DestroyIndicators()
    {
        if (aoeIndicatorInstance != null)
        {
            Destroy(aoeIndicatorInstance);
            aoeIndicatorInstance = null;
        }
        if (singleTargetInstance != null)
        {
            Destroy(singleTargetInstance);
            singleTargetInstance = null;
        }
    }
}
