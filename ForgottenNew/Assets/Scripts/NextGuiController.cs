using System;
using UnityEngine;
using UnityEngine.UI;

public class NextGuiController : MonoBehaviour
{
    public CellGrid CellGrid;
    public GameObject UnitsParent;

    public Button NextTurnButton;

    public GameObject InfoPanel;
    public GameObject GameOverPanel;
    public Canvas Canvas;

    private GameObject _infoPanel;
    private GameObject _gameOverPanel;

    void Start()
    {
        CellGrid.GameStarted += OnGameStarted;
        CellGrid.TurnEnded += OnTurnEnded;
        CellGrid.GameEnded += OnGameEnded;
    }

    private void OnGameStarted(object sender, EventArgs e)
    {
        foreach (Transform unit in UnitsParent.transform)
        {

            unit.GetComponent<Unit>().UnitHighlighted += OnUnitHighlighted;
            unit.GetComponent<Unit>().UnitDehighlighted += OnUnitDehighlighted;
            unit.GetComponent<Unit>().UnitDestroyed += OnUnitDestroyed;
            unit.GetComponent<Unit>().UnitAttacked += OnUnitAttacked;
        }
    }

    private void OnTurnEnded(object sender, EventArgs e)
    {
        NextTurnButton.interactable = ((sender as CellGrid).CurrentPlayer is HumanPlayer);
    }
    private void OnGameEnded(object sender, EventArgs e)
    {
    }

    private void OnUnitAttacked(object sender, AttackEventArgs e)
    {
        if (!(CellGrid.CurrentPlayer is HumanPlayer)) return;

        OnUnitDehighlighted(sender, e);

        if ((sender as Unit).HP <= 0) return;

        OnUnitHighlighted(sender, e);
    }
    private void OnUnitDestroyed(object sender, AttackEventArgs e)
    {
        Destroy(_infoPanel);
    }
    private void OnUnitDehighlighted(object sender, EventArgs e)
    {
        Destroy(_infoPanel);
    }
    private void OnUnitHighlighted(object sender, EventArgs e)
    {
        var unit = sender as GenericUnit;
        _infoPanel = Instantiate(InfoPanel);

        float hpScale = (float)((float)(unit).HP / (float)(unit).TotalHP);

        _infoPanel.transform.Find("Name").GetComponent<Text>().text = unit.UnitName;
        _infoPanel.transform.Find("HP").Find("Image").transform.localScale = new Vector3(hpScale,1,1);
        _infoPanel.transform.Find("Attack").Find("Image").transform.localScale = new Vector3((float)unit.AttackBase/100.0f,1,1);
        _infoPanel.transform.Find("Defence").Find("Image").transform.localScale = new Vector3((float)unit.Defence / 100.0f, 1, 1);
        _infoPanel.transform.Find("Energy").Find("Image").transform.localScale = new Vector3((float)unit.Energy / 10.0f, 1, 1);

        _infoPanel.GetComponent<RectTransform>().SetParent(Canvas.GetComponent<RectTransform>(),false);
    }

    public void DismissPanel()
    {
        Destroy(_gameOverPanel);
    }
    public void RestartLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}

