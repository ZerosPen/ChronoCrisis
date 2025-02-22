using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] ButtonManagement bm;

    private Text manyEnemies;
    private void Awake()
    {
        manyEnemies = GetComponent<Text>();
    }
    private void Update()
    {
        manyEnemies.text = SaveManager.instance.money + "$";
    }
}


