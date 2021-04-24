using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorkReport : MonoBehaviour
{
    [SerializeField] float maxCollisionsPerSegment = 5f;
    [SerializeField] float maxFlightPerSegment = 6f;

    [SerializeField] TMPro.TextMeshProUGUI collisionsHeader;
    [SerializeField] TMPro.TextMeshProUGUI flightHeader;
    [SerializeField] TMPro.TextMeshProUGUI landingHeader;
    [SerializeField] TMPro.TextMeshProUGUI cleaningsHeader;
    [SerializeField] TMPro.TextMeshProUGUI collisionsScoreText;
    [SerializeField] TMPro.TextMeshProUGUI flightScoreText;
    [SerializeField] TMPro.TextMeshProUGUI landingScoreText;
    [SerializeField] TMPro.TextMeshProUGUI cleaningsScoreText;
    [SerializeField] TMPro.TextMeshProUGUI totalScoreText;
    [SerializeField] Image record;
    [SerializeField] int cleaningsScore = 1000;
    [SerializeField] int landingScore = 25000;
    [SerializeField] int flightScore = 100;
    [SerializeField] int collisionScore = 100;
    ScoreSummary summary = new ScoreSummary(23, 11, 41.3f, true);
    int segments = 10;

    int maxCollisions
    {
        get
        {
            return Mathf.RoundToInt(maxCollisionsPerSegment * segments);
        }
    }

    int maxFlight
    {
        get
        {
            return Mathf.RoundToInt(maxFlightPerSegment * segments);
        }
    }

    void Start()
    {
        var vessel = FindObjectOfType<VesselController>();
        var builder = FindObjectOfType<FistulaBuilder>();
        if (vessel != null && builder != null)
        {
            SetScore(vessel.scoreKeeper.Summarize(), builder.nSegments);
        } else
        {
            UpdateHeaders();
            StartCoroutine(MakeReport());
        }
    }

    public void SetScore(ScoreSummary summary, int segments)
    {
        this.summary = summary;
        this.segments = segments;
        UpdateHeaders();
        StartCoroutine(MakeReport());
    }

    void UpdateHeaders()
    {
        collisionsHeader.text = string.Format("Collisions (x{0} below {1})", collisionScore, maxCollisions);
        flightHeader.text = string.Format("Flight Time (x{0} below {1})", flightScore, maxFlight);
        landingHeader.text = string.Format("Standing Landing (x{0})", landingScore);
        cleaningsHeader.text = string.Format("Gunk Removed (x{0})", cleaningsScore);
    }

    bool running = false;

    [SerializeField] float countingDelay = 0.1f;
    [SerializeField] float betweenCountingDelay = 0.75f;
    [SerializeField] float initialDelay = 1f;
    IEnumerator<WaitForSeconds> MakeReport()
    {
        if (running)
        {
            yield break;
        }
        running = true;
        yield return new WaitForSeconds(1f);

        List<int> totals = new List<int>();
        int cleanings = 0;
        for (int i=0; i<summary.cleanings; i++)
        {
            cleanings += cleaningsScore;
            cleaningsScoreText.text = cleanings.ToString();
            yield return new WaitForSeconds(countingDelay);
        }
        totals.Add(cleanings);
        yield return new WaitForSeconds(betweenCountingDelay);

        int collisions = 0;
        for (int i=maxCollisions; i>summary.collisions; i--)
        {
            collisions += collisionScore;
            collisionsScoreText.text = collisions.ToString();
            yield return new WaitForSeconds(countingDelay);
        }
        totals.Add(collisions);
        yield return new WaitForSeconds(betweenCountingDelay);

        int flightTime = 0;
        for (float i=maxFlight; i>summary.flightTime; i--)
        {
            flightTime += flightScore;
            flightScoreText.text = flightScore.ToString();
            yield return new WaitForSeconds(countingDelay);
        }
        totals.Add(flightTime);
        yield return new WaitForSeconds(betweenCountingDelay);

        int standing = 0;
        if (summary.standing)
        {
            standing += landingScore;
            landingScoreText.text = standing.ToString();
        }
        totals.Add(standing);
        yield return new WaitForSeconds(betweenCountingDelay);
        int total = 0;

        for (int i=0; i<totals.Count; i++)
        {
            total += totals[i];
            totalScoreText.text = total.ToString();
            yield return new WaitForSeconds(countingDelay);
        }

        if (PlayerPrefs.GetInt("bestScore", -1) < total)
        {
            record.enabled = true;
            PlayerPrefs.SetInt("bestScore", total);
        }
    }

    public void OnClickAgain()
    {
        SceneManager.LoadScene("Game");
    }
}
