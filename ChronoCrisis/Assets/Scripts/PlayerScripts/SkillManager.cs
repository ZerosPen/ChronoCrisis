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
    private bool isCasting = false;

    private Dictionary<Skill, float> skillCooldowns = new Dictionary<Skill, float>();

    [SerializeField] private GameObject aoeIndicatorPrefab;
    [SerializeField] private GameObject singleTargetIndicatorPrefab;
    [SerializeField] private Transform skillIndicatorContainer;
    [SerializeField] private Transform weaponTransform; // Assign this in the Inspector
    private Vector3 originalWeaponPosition;

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
        originalWeaponPosition = weaponTransform.position;
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
                // Move only the AoE indicator, NOT the weapon
                aoeIndicatorInstance.transform.position = pra.GetCursorPosition();

                // Rotate the weapon to face the cursor
                RotateWeaponTowardsCursor();
            }
            else if ((activeSkill.typeSkill == "SingelTarget" || activeSkill.typeSkill == "Singel") && singleTargetInstance != null)
            {
                // Rotate the weapon towards the cursor for single target skills
                RotateWeaponTowardsCursor();
            }
        }
        if(isCasting == false)
        {
            ResetPosition();
        }
    }


    public void HandleSkillSwitching(int indexSkill)
    {
        if (indexSkill >= 0 && indexSkill < skillSlots.Length && skillSlots[indexSkill] != null)
        {
            indexActiveSkill = indexSkill;
            isCasting = true;
            Debug.Log($"Skill selected: {indexActiveSkill.Value}");
        }
    }

    void HandleSkillUse()
    {
        if (Input.GetMouseButtonDown(0) && indexActiveSkill.HasValue && playerController.ActiveSkill && isCasting)
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
        else if (isAiming && Input.GetMouseButtonUp(0) && playerController.ActiveSkill && isCasting && playerController.currManaPoint >= skillSlots[indexActiveSkill.Value].manaUse)
        {
            Skill activeSkill = skillSlots[indexActiveSkill.Value];

            activeSkill.useSkill(playerController.gameObject);
            skillCooldowns[activeSkill] = activeSkill.coolDown;
            playerController.ActiveSkill = false;
            StartCoroutine(playerController.CoolDownChangeSkill());

            isAiming = false;
            pra.isSkillActive = false;
            indexActiveSkill = null;

            // Store last AoE position BEFORE destroying the indicator
            Vector3 lastAoEPosition = aoeIndicatorInstance != null ? aoeIndicatorInstance.transform.position : skillIndicatorContainer.position;

            DestroyIndicators();
            isCasting = false;
            // Rotate weapon to face last AoE position
            Vector3 direction = lastAoEPosition - skillIndicatorContainer.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weaponTransform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    void ResetPosition()
    {
        skillIndicatorContainer.position = Vector3.zero;
        weaponTransform.localPosition = new Vector3(0.73f, 0f, 0f);
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

    void RotateWeaponTowardsCursor()
    {
        Vector3 cursorPosition = pra.GetCursorPosition();
        Vector3 direction = cursorPosition - weaponTransform.position;

        // Calculate rotation angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply rotation without changing position
        weaponTransform.rotation = Quaternion.Euler(0f, 0f, angle);
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
