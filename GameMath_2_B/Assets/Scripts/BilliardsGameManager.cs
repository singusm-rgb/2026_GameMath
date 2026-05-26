using UnityEngine;
using UnityEngine.UI;

public class BilliardsGameManager : MonoBehaviour
{
    public static BilliardsGameManager Instance;

    public enum Turn { Player1, Player2 }
    [Header("Game States")]
    public Turn currentTurn = Turn.Player1;
    public float stopThreshold = 0.05f; // ���� ����ٰ� �Ǵ��� �ӵ� ���ذ�

    [Header("Balls Rigidbody Reference")]
    public Rigidbody p1BallRb;
    public Rigidbody p2BallRb;
    public Rigidbody[] targetBallRbs;

    [Header("UI Elements")]
    public Text turnText;
    public Text p1ScoreText;
    public Text p2ScoreText;
    public GameObject gameOverPanel;
    public Text winnerText;

    private int p1Score = 0;
    private int p2Score = 0;
    private bool ballsWereMoving = false;

    private BilliardsBallController p1Controller;
    private BilliardsBallController p2Controller;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        p1Controller = p1BallRb.GetComponent<BilliardsBallController>();
        p2Controller = p2BallRb.GetComponent<BilliardsBallController>();
        UpdateUI();
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (gameOverPanel.activeSelf) return;

        bool currentlyMoving = AreBallsMoving();

        // ������ �������� ������
        if (currentlyMoving && !ballsWereMoving)
        {
            ballsWereMoving = true;
        }
        // ������ �������ٰ� ��� ���� ���� (�� ���� ����)
        else if (!currentlyMoving && ballsWereMoving)
        {
            ballsWereMoving = false;
            EvaluateTurnResult();
        }
    }

    // ��� ���� �ӵ��� ���� ��(stopThreshold) �������� üũ
    public bool AreBallsMoving()
    {
        if (p1BallRb.linearVelocity.magnitude > stopThreshold) return true;
        if (p2BallRb.linearVelocity.magnitude > stopThreshold) return true;
        foreach (var rb in targetBallRbs)
        {
            if (rb.linearVelocity.magnitude > stopThreshold) return true;
        }
        return false;
    }

    public bool CanPlayerClick()
    {
        return !AreBallsMoving() && !ballsWereMoving && !gameOverPanel.activeSelf;
    }

    private void EvaluateTurnResult()
    {
        BilliardsBallController activeController = (currentTurn == Turn.Player1) ? p1Controller : p2Controller;

        // ��Ģ 6: ��� �÷��̾� ���� ���߸� 1�� ���� (���� 0��)
        if (activeController.hitOpponent)
        {
            if (currentTurn == Turn.Player1) p1Score = Mathf.Max(0, p1Score - 1);
            else p2Score = Mathf.Max(0, p2Score - 1);
        }
        // ��Ģ 5: ��� ���� �� ���߰� Target ��(2��)�� ��� ���߸� 1�� ȹ��
        else if (activeController.hitTargetCount >= targetBallRbs.Length)
        {
            if (currentTurn == Turn.Player1) p1Score++;
            else p2Score++;
        }

        // ��Ģ 7: �� �� �� ���� 5���� �����ϸ� ���� ����
        if (p1Score >= 5 || p2Score >= 5)
        {
            EndGame();
            return;
        }

        // �� ��ȯ �� �浹 ���� �ʱ�ȭ
        currentTurn = (currentTurn == Turn.Player1) ? Turn.Player2 : Turn.Player1;
        p1Controller.ResetTurnFlags();
        p2Controller.ResetTurnFlags();
        UpdateUI();
    }

    private void UpdateUI()
    {
        turnText.text = $"���� ��: {(currentTurn == Turn.Player1 ? "1P" : "2P")}";
        p1ScoreText.text = $"1P ����: {p1Score} / 5";
        p2ScoreText.text = $"2P ����: {p2Score} / 5";
    }

    private void EndGame()
    {
        gameOverPanel.SetActive(true);
        winnerText.text = (p1Score >= 5) ? "1P �÷��̾� ���� �¸�!" : "2P �÷��̾� ���� �¸�!";
    }
}