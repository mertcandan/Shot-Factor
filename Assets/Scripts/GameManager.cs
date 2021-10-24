using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public Gun gun;
    public GunBuilder gunBuilder;
    public Camera mainCamera;
    public Transform battleGunPlacement;
    public Transform battleCameraPlacement;
    public GameObject gunBuildPhase;
    public GameObject battlePhase;
    public GameObject battleButton;
    public GameObject levelLostPanel;
    public GameObject levelWonPanel;

    private bool _levelInProgress;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _levelInProgress = true;
    }

    #region Level Lost
    
    public void LevelLost()
    {
        if (!_levelInProgress)
        {
            return;
        }
        Debug.Log("Level Lost");
        levelLostPanel.SetActive(true);
        gun.Deactivate();
        _levelInProgress = false;
    }
    
    #endregion
    
    #region Level Won

    public void LevelWon()
    {
        if (!_levelInProgress)
        {
            return;
        }
        
        Debug.Log("Level Won");
        levelWonPanel.SetActive(true);
        gun.Deactivate();
        _levelInProgress = false;
    }
    
    #endregion
    
    #region Restart

    public void OnRestartPressed()
    {
        ReloadScene();
    }
    
    void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    
    #endregion
    
    #region Battle Transition
    
    public void OnBattlePressed()
    {
        battleButton.SetActive(false);
        PrepareGun();
        PrepareCamera();
        ActivateBattle();
    }

    void PrepareCamera()
    {
        mainCamera.transform.SetPositionAndRotation(
            battleCameraPlacement.position,
            battleCameraPlacement.rotation);
    }

    void PrepareGun()
    {
        gunBuilder.MergeExtensions(gun.transform);
        gun.Prepare(battleGunPlacement);
    }

    void ActivateBattle()
    {
        gunBuildPhase.SetActive(false);
        battlePhase.SetActive(true);
    }
    
    #endregion
}
