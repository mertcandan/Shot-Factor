using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public Gun gun;
    public Camera mainCamera;
    public Transform battleGunPlacement;
    public Transform battleCameraPlacement;
    public GameObject gunBuildPhase;
    public GameObject battlePhase;
    public GameObject battleButton;
    public GameObject levelLostPanel;
    public GameObject levelWonPanel;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    #region Level Lost
    
    public void LevelLost()
    {
        Debug.Log("Level Lost");
        levelLostPanel.SetActive(true);
    }
    
    #endregion
    
    #region Level Won

    public void LevelWon()
    {
        Debug.Log("Level Won");
        levelWonPanel.SetActive(true);
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
        gun.Prepare(battleGunPlacement);
    }

    void ActivateBattle()
    {
        gunBuildPhase.SetActive(false);
        battlePhase.SetActive(true);
    }
    
    #endregion
}
