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
    [SerializeField] private bool isCasting = false;

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
            Skill selectedSkill = skillSlots[indexSkill];

            if (skillCooldowns.ContainsKey(selectedSkill) && skillCooldowns[selectedSkill] > 1.5f)
            {
                Debug.Log($"Cannot select {selectedSkill.skillName}, cooldown: {skillCooldowns[selectedSkill]}s left.");
                return;
            }
            else
            {
                // Select the skill
                indexActiveSkill = indexSkill;
                isCasting = true;
                Debug.Log($"Skill selected: {selectedSkill.skillName}");
            }
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
        else if (Input.GetMouseButtonUp(0) && isAiming && playerController.ActiveSkill && isCasting && playerController.currManaPoint >= skillSlots[indexActiveSkill.Value].manaUse)
        {
            Debug.Log("Mouse Button Up Detected");
            Debug.Log("isAiming: " + isAiming);
            Debug.Log("ActiveSkill: " + playerController.ActiveSkill);
            Debug.Log("isCasting: " + isCasting);
            Debug.Log("Mana: " + playerController.currManaPoint + " / " + skillSlots[indexActiveSkill.Value].manaUse);

            Skill activeSkill = skillSlots[indexActiveSkill.Value];
            Debug.Log("Casting skill: " + activeSkill.skillName);

            activeSkill.useSkill(playerController.gameObject);
            skillCooldowns[activeSkill] = activeSkill.coolDown;
            playerController.ActiveSkill = false;
            playerController.currManaPoint -= skillSlots[indexActiveSkill.Value].manaUse;
            StartCoroutine(playerController.CoolDownChangeSkill());

            StartCoroutine(ResetAimingAfterCast());

            Vector3 lastAoEPosition = aoeIndicatorInstance != null ? aoeIndicatorInstance.transform.position : skillIndicatorContainer.position;

            DestroyIndicators();
            isCasting = false;

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

        // Destroy previous indicators to avoid conflicts
        if (aoeIndicatorInstance != null)
        {
            Destroy(aoeIndicatorInstance);
        }
        if (singleTargetInstance != null)
        {
            Destroy(singleTargetInstance);
        }

        if (type == "AoE" && aoeIndicatorPrefab != null && isAiming)
        {
            pra.isSkillActive = true;
            aoeIndicatorInstance = Instantiate(aoeIndicatorPrefab, skillIndicatorContainer);

            // Ensure valid skill index before setting scale
            if (indexActiveSkill.HasValue && indexActiveSkill.Value < skillSlots.Length)
            {
                aoeIndicatorInstance.transform.localScale = new Vector3(
                    skillSlots[indexActiveSkill.Value].radiusAoE,
                    skillSlots[indexActiveSkill.Value].radiusAoE,
                    skillSlots[indexActiveSkill.Value].radiusAoE
                );
            }
            else
            {
                Debug.LogError("Invalid skill index!");
            }

            aoeIndicatorInstance.SetActive(true);
            aoeIndicatorInstance.transform.position = pra.GetCursorPosition();
        }
        else if ((type == "SingelTarget" || type == "Singel") && singleTargetIndicatorPrefab != null && isAiming)
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

    IEnumerator ResetAimingAfterCast()
    {
        yield return new WaitForSeconds(0.1f); // Small delay to allow re-aiming
        isAiming = false;
        pra.isSkillActive = false;
        indexActiveSkill = null;
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
